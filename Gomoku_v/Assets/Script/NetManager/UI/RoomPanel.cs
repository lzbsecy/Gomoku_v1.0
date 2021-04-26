using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketGameProtocol;

public class RoomPanel : BasePanel
{
    public Button backBtn, startBtn;

    public Transform content;
    public GameObject userItem;

    public ExitRoomRequest exitRoomRequest;

    private void Start()
    {
        startBtn.onClick.AddListener(OnStartClick);
        backBtn.onClick.AddListener(OnBackClick);
    }

    private void OnStartClick()
    {

    }
    private void OnBackClick()
    {
        exitRoomRequest.SendRequest();
    }


    public void UpdatePlayerList(MainPack pack)
    {
        for(int i=0;i<content.childCount;i++)
        {
            Destroy(content.GetChild(i).gameObject);

        }
        foreach(var player in pack.PlayerPack)
        {
            if (PlayerPrefs.GetString("username").Equals(player.PlayerName))
            {
                PlayerPrefs.SetString("id", pack.PlayerPack.IndexOf(player).ToString());
            }
            UserItem useritem = Instantiate(userItem, Vector3.zero, Quaternion.identity).GetComponent<UserItem>();
            useritem.gameObject.transform.SetParent(content);
            useritem.SetPlayerInfo(player.PlayerName);
        }
    }

    public void ExitRoomRequest()
    {
        uiMag.PopPanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Enter();
    }

    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }

    public override void OnPause()
    {
        base.OnPause();
        Exit();
    }

    public override void OnRecovery()
    {
        base.OnRecovery();
        Enter();
    }

    private void Enter()
    {
        gameObject.SetActive(true);
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
