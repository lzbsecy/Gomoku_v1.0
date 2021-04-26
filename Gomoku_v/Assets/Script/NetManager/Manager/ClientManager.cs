using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Google.Protobuf;
using SocketGameProtocol;

public class ClientManager : BaseManager
{
    private Socket socket;
    private Message message;

    public ClientManager(GameFace face) : base(face) { }

    public override void OnInit()
    {
        base.OnInit();
        message = new Message();
        InitSocket();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        message = null;
        CloseSocket();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitSocket()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            //10.51.96.91 本机
            //82.156.215.225 服务器
            socket.Connect("10.51.96.91", 6666);
            //连接成功
            StartReceive();
            Debug.Log("连接成功!");
            face.ShowMessage("连接成功!");
        }
        catch(Exception e)
        {
            //连接出错
            Debug.LogWarning(e);
            face.ShowMessage("连接失败!");
        }
    }

    /// <summary>
    /// 关闭socket
    /// </summary>
    private void CloseSocket()
    {
        if(socket.Connected && socket!=null)
        {
            socket.Close();
        }
    }

    private void StartReceive()
    {
        socket.BeginReceive(message.Buffer, message.StartIndex, message.Remsize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult iar)
    {
        try
        {
            if (socket == null || socket.Connected == false) return;
            int len = socket.EndReceive(iar);
            if (len == 0)
            {
                CloseSocket();
                return;
            }

            message.ReadBuffer(len, HandleResponse);
            StartReceive();
        }
        catch(Exception e)
        {
            Debug.LogWarning(e);
        }
    }

    private void HandleResponse(MainPack mainPack)
    {
        face.HandleResponse(mainPack);
    }

    public void Send(MainPack pack)
    {
        Debug.Log(pack.ActionCode);
        socket.Send(Message.PackData(pack));
    }
}
