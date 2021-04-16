using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using SocketMultiplayerGameServer.Controller;
using SocketGameProtocol;

namespace SocketMultiplayerGameServer.Servers
{
    class Server
    {
        private Socket socket;
        private List<Client> clientList = new List<Client>();
        private ControllerManager controllerManager;

        public Server(int port)
        {
            controllerManager = new ControllerManager(this);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(0);
            StartAccept();
        }

        void StartAccept()
        {
            socket.BeginAccept(AcceptCallBack, null);
        }

        void AcceptCallBack(IAsyncResult iat)
        {
            Socket client = socket.EndAccept(iat);
            clientList.Add(new Client(client,this));
            StartAccept();
        }

        //public bool Logon(Client client,MainPack pack)
        //{
        //    return client.GetUserData.Logon(pack);
        //}

        public void HandleRequest(MainPack pack,Client client)
        {
            controllerManager.HandleRequest(pack, client);
        }

        public void RemoveClient(Client client)
        {
            clientList.Remove(client);
            client = null;
        }
    }
}
