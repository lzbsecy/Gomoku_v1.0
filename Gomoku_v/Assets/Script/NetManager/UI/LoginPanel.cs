using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{

    public LogonRequest loginRequest;
    public InputField user, pass;
    public Button loginBtn, switchBtn;


    private void Start()
    {
        loginBtn.onClick.AddListener(OnLoginClick);
        switchBtn.onClick.AddListener(SwitchLogon);
    }

    private void OnLoginClick()
    {
        if (user.text == "" || pass.text == "")
        {
            Debug.LogWarning("用户名或密码不能为空");
            return;
        }
        loginRequest.SendRequest(user.text, pass.text);
    }

    private void SwitchLogon()
    {
        uiMag.PushPanel(PanelType.Logon); 
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
