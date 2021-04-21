using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : BasePanel
{

    public Text text;
    string msg = null;

    public override void OnEnter()
    {
        base.OnEnter();
        uiMag.SetMessagePanel(this);
        text.CrossFadeAlpha(0, 0.1f, false);
    }

    private void Update()
    {
        if (msg != null) 
        {
            ShowText(msg);
            msg = null;
        }
    }

    public void ShowMessage(string str ,bool sync = false)
    {
        if(sync)
        {
            //异步显示
            msg = str;
        }
        else
        {
            ShowText(str);
        }  
    }

    private void ShowText(string str)
    {
        text.text = str;
        text.CrossFadeAlpha(1, 0.1f, false);
        Invoke("HideText", 1);
    }

    private void HideText()
    {
        text.CrossFadeAlpha(1, 0.1f, false);
    }
}
