using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class GameFace : MonoBehaviour
{

    private ClientManager clientManager;
    private RequestManager requestManager;
    private UIManager uIManager;
    private static GameFace face;

    public static GameFace Face
    {
        get
        {
            if(face ==null)
            {
                return GameObject.Find("GameFace").GetComponent<GameFace>();
            }
            return face;
        }
    }

    void Awake()
    {
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        uIManager = new UIManager(this);

        clientManager.OnInit();
        requestManager.OnInit();
        uIManager.OnInit();
    }

    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
        uIManager.OnDestroy();
    }


    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    public void HandleResponse(MainPack pack)
    {
        //do
        requestManager.HandleResponse(pack);
    }

    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }

    public void ShowMessage(string str, bool sync = false)
    {
        uIManager.ShowMessage(str, sync);
    }
}
