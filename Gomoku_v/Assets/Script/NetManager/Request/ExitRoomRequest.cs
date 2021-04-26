using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class ExitRoomRequest : BaseRequest
{

    private bool isExit = false;
    public RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.Exit;
        base.Awake();
    }

    private void Update()
    {
        if(isExit)
        {
            roomPanel.ExitRoomRequest();
            isExit = false;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        isExit = true;
    }

    public void SendRequest()
    {
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;

        pack.Str = "ExitRoomRequest";
        base.SendRequest(pack);
    }
}
