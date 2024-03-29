﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    Message,
    Start,
    Login,
    Logon,
    RoomList,
    Room,
    Game,
    GameOver
}

public class UIManager : BaseManager
{
    public UIManager(GameFace gameFace) : base(gameFace) { }

    private Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    private Dictionary<PanelType, string> panelPath = new Dictionary<PanelType, string>();

    private Stack<BasePanel> panelStack = new Stack<BasePanel>();

    private MessagePanel messagePanel;
    private Transform canvasTransform;


    public override void OnInit()
    {

        base.OnInit();
        InitPanel();
        canvasTransform = GameObject.Find("Canvas").transform;

        PushPanel(PanelType.Message);
        PushPanel(PanelType.Start);

    }

    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="panelType"></param>
    public BasePanel PushPanel(PanelType panelType)
    {
        if(panelDict.TryGetValue(panelType,out BasePanel panel))
        {
            if(panelStack.Count>0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }
            panelStack.Push(panel);
            panel.OnEnter();
            return panel;
        }
        else
        {
            Debug.Log(panelType);
            BasePanel tempPanel = SpawnPanel(panelType);
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }
            panelStack.Push(tempPanel);
            tempPanel.OnEnter();
            return tempPanel;
        }
    }

    /// <summary>
    /// 移除UI
    /// </summary>
    public void PopPanel()
    {
        if (panelStack.Count == 0) return;

        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        BasePanel panel = panelStack.Peek();
        panel.OnRecovery();

    }

    /// <summary>
    /// 实例化UI
    /// </summary>
    /// <param name="panelType"></param>
    private BasePanel SpawnPanel(PanelType panelType)
    {
        if(panelPath.TryGetValue(panelType,out string path))
        {
            GameObject obj = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            obj.transform.SetParent(canvasTransform, false);
            BasePanel panel = obj.GetComponent<BasePanel>();
            panel.SetUIMag = this;
            panelDict.Add(panelType, panel);
            return panel;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 初始化UI路径
    /// </summary>
    private void InitPanel()
    {
        string panelpath = "Panel/";
        string[] path = new string[] { "MessagePanel", "StartPanel", "LoginPanel", "LogonPanel", "RoomListPanel", "RoomPanel" };
        panelPath.Add(PanelType.Message, panelpath + path[0]);
        panelPath.Add(PanelType.Start, panelpath + path[1]);
        panelPath.Add(PanelType.Login, panelpath + path[2]);
        panelPath.Add(PanelType.Logon, panelpath + path[3]);
        panelPath.Add(PanelType.RoomList, panelpath + path[4]);
        panelPath.Add(PanelType.Room, panelpath + path[5]);
    }

    public void SetMessagePanel(MessagePanel message)
    {
        messagePanel = message;
    }

    public void ShowMessage(string str, bool sync = false)
    {
        messagePanel.ShowMessage(str, sync);
    }
}
