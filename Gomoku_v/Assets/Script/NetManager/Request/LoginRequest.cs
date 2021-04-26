using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class LoginRequest : BaseRequest
{
    public LoginPanel loginPanel;

    private MainPack pack = null;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;

        base.Awake();
    }

    private void Update()
    {
        if(pack!=null)
        {
            loginPanel.OnResPonse(pack);
            pack = null;
        }
    }

    public override void OnResponse(MainPack pack)
    {
        this.pack = pack;
    }

    public void SendRequest(string user, string pass)
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
