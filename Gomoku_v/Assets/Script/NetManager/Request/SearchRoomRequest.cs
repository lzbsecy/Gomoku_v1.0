using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class SearchRoomRequest : BaseRequest
{
    public RoomListPanel roomListPanel;
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.SearchRoom;
        base.Awake();
    }

    private void Update()
    {
        if (pack != null)
        {
            roomListPanel.SearchRoomResponse(pack);
            pack = null;
        }
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        pack.Str = "SearchRoom";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
