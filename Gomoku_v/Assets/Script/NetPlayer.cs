using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetPlayer : NetworkBehaviour
{
    
    RaycastHit hit;
    //public static int chess = 1;
    [SyncVar]
    public ChessType turn;

    private NetGameSystem system;
    private NetGameStatus status;
    private GameObject backMoveBtn;

    private GameObject textBox;

    void Start()
    {
        textBox = GameObject.Find("Window").gameObject;
        system = GameObject.Find("netboard").GetComponent<NetGameSystem>();
        status = GameObject.Find("netboard").GetComponent<NetGameStatus>();
        backMoveBtn = GameObject.Find("BackMove").gameObject;

        GameObject.Find("BackMove").GetComponent<Button>().onClick.AddListener(BackMove);

        if(isServer)
        {
            if(status.chess<3)
            {
                turn = (ChessType)status.chess;
                status.chess++;
            }
            else
            {
                turn = ChessType.Null;
            }
        }
    }
    void Update()
    {
        IsGameOver();
    }

    void FixedUpdate()
    {
        PutChess();
    }

    /// <summary>
    /// 下棋
    /// </summary>
    public void PutChess()
    {
        if (turn == status.turn)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if(isLocalPlayer)
                    {
                        Vector2 pos = new Vector2((int)(hit.point.x + 0.5f), (int)(hit.point.y + 0.5f));
                        //Debug.Log((int)(hit.point.x + 0.5f) + " , " + (int)(hit.point.y + 0.5f));
                        CmdPut(pos);
                    }
                }
            }
        }
    }
    public void BackMove()
    {
        if(turn ==status.turn)
        {
            CmdBackMove();
        }
    }

    /// <summary>
    /// 游戏结束事件
    /// </summary>
    public void IsGameOver()
    {
        if(checkOver())
        {
            GameOverEvent(status.turn);
        }
    }

    public void GameOverEvent(ChessType type)
    {
        if (type == ChessType.black)
        {
            Debug.Log("黑棋胜");
            status.IsOver = true;
            if (turn == ChessType.black)
            {
                WindowBox("恭喜！黑棋胜");
            }
            else
            {
                WindowBox("阁下！五子不行");
            }
        }
        else if (type == ChessType.white)
        {
            Debug.Log("白棋胜");
            status.IsOver = true;
            if (turn == ChessType.white)
            {
                WindowBox("恭喜！白棋胜");
            }
            else
            {
                WindowBox("阁下！五子不行！");
            }
        }

        Debug.Log("游戏结束");
    }

    public bool checkOver()
    {
        if (status.IsOver)
        {
            Time.timeScale = 0;
            return true;
        }
        else
        {
            Time.timeScale = 1;
            return false;
        }
    }

    public void WindowBox(string text)
    {
        textBox.SetActive(true);
        textBox.GetComponentInChildren<Text>().text = text;
    }

    [Command]
    public void CmdPut(Vector2 pos)
    {
        system.Put(pos);
    }
    [Command]
    public void CmdBackMove()
    {
        system.BackMove();
    }
}
