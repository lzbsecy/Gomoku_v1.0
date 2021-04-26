using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;
using Google;

public class LogonRequest : BaseRequest
{

    public LogonPanel logonPanel;

    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Logon;

        base.Awake();
    }

    private void Update()
    {
        if(pack!=null)
        {
            logonPanel.OnResPonse(pack);
            pack = null;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }

    public void SendRequest(string user,string pass)
    {
        Debug.Log("send request");
        MainPack pack = new MainPack();
        pack.RequestCode = requestCode;
        pack.ActionCode = actionCode;
        
        LoginPack loginPack = new LoginPack();
        loginPack.Username = user;
        loginPack.Password = pass;

        pack.LoginPack = loginPack;
        base.SendRequest(pack);
    }

}
