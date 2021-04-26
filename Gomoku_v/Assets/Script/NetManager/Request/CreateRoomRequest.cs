using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;
public class CreateRoomRequest : BaseRequest
{
    public RoomListPanel roomListPanel;
    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    private void Update()
    {
        if(pack!=null)
        {
            roomListPanel.CreateRoomResponse(pack);
            pack = null;
        }
    }

    public void SendRequest(string roomName,int maxnum)
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        RoomPack room = new RoomPack();
        room.RoomName = roomName;
        room.MaxNum = maxnum;
        pack.RoomPack.Add(room);

        pack.Str = "CreateRoom";
        base.SendRequest(pack);
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }
}
