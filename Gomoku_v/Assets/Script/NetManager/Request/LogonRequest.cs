using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;
using Google;

public class LogonRequest : BaseRequest
{

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Logon;

        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        switch(pack.ReturnCode)
        {
            case ReturnCode.Succeed:
                face.ShowMessage("注册成功", true);
                break;
            case ReturnCode.Fail:
                face.ShowMessage("注册失败", true);
                break;
            default:
                
                break;
        }
        
    }

    public void SendRequest(string user,string pass)
    {
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
