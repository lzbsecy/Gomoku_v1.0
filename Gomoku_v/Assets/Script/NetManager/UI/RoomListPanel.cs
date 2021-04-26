using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    public Button backBtn, searchBtn, createBtn;
    public InputField roomName;

    public Transform roomListTransform;
    public GameObject roomitem;

    public CreateRoomRequest createRoomRequest;
    public SearchRoomRequest searchRoomRequest;
    public JoinRoomRequest joinRoomRequest;

    private void Start()
    {
        backBtn.onClick.AddListener(OnBackClick);
        searchBtn.onClick.AddListener(OnSerchClick);
        createBtn.onClick.AddListener(OnCreateClick);
    }

    private void OnBackClick()
    {
        uiMag.PopPanel();
    }

    private void OnSerchClick()
    {
        searchRoomRequest.SendRequest();
    }

    private void OnCreateClick()
    {
        Debug.Log("查询房间");
        if (roomName.text == "") 
        {
            uiMag.ShowMessage("房间名不能为空！");
            return;
        }
        createRoomRequest.SendRequest(roomName.text, 2);
    }

    public void CreateRoomResponse(MainPack pack)
    {
        Debug.Log("创建房间！");
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uiMag.ShowMessage("创建成功！");
                RoomPanel roomPanel = uiMag.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                roomPanel.UpdatePlayerList(pack);
                break;
            case ReturnCode.Fail:
                uiMag.ShowMessage("创建失败！");
                break;
            default:

                break;
        }
    }

    public void SearchRoomResponse(MainPack pack)
    {
        Debug.Log("查找房间！");
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uiMag.ShowMessage("查询成功!");
                break;
            case ReturnCode.Fail:
                uiMag.ShowMessage("查询出错!");
                break;
            case ReturnCode.NotRoom:
                uiMag.ShowMessage("当前没有房间！");
                break;
            default:

                break;
        }
        UpdateRoomList(pack);
    }

    private void UpdateRoomList(MainPack packs)
    {
        //清空房间
        for(int i=0;i<roomListTransform.childCount;i++)
        {
            Destroy(roomListTransform.GetChild(i).gameObject);
        }
        //重新加载房间
        if (packs.RoomPack.Count == 0) return;
        foreach(var room in packs.RoomPack)
        {
            RoomItem item = Instantiate(roomitem, Vector3.zero,Quaternion.identity).GetComponent<RoomItem>();
            item.roomListPanel = this;
            item.gameObject.transform.SetParent(roomListTransform);
            item.SetRoomInfo(room.RoomName, room.CurNum, room.MaxNum, room.RoomState);
        }
    }

    public void JoinRoomResponse(MainPack pack)
    {
        Debug.Log("加入房间！");
        switch (pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                uiMag.ShowMessage("加入成功!");
                RoomPanel roomPanel = uiMag.PushPanel(PanelType.Room).GetComponent<RoomPanel>();
                roomPanel.UpdatePlayerList(pack);
                break;
            case ReturnCode.Fail:
                uiMag.ShowMessage("加入失败!");
                UpdateRoomList(pack);
                break;
            default:
                uiMag.ShowMessage("房间已失效!");
                UpdateRoomList(pack);
                break;
        }
    }

    public void JoinRoom(string roomName)
    {
        joinRoomRequest.SendRequest(roomName);
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
