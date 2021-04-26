using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Button join;
    public Text title, num, state;

    public RoomListPanel roomListPanel;

    void Start()
    {
        join.onClick.AddListener(OnJoinClick);
    }

    private void OnJoinClick()
    {
        roomListPanel.JoinRoom(title.text);
    }

    public void SetRoomInfo(string title, int curnum,int maxnum, int state)
    {
        this.title.text = title + "的房间";
        this.num.text = curnum + "/" + maxnum;
        switch(state)
        {
            case 0:
                this.state.text = "等待加入";
                break;
            case 1:
                this.state.text = "房间已满";
                break;
            case 2:
                this.state.text = "游戏中";
                break;
            default:
                this.state.text = "未知";
                break;

        }
    }
}
