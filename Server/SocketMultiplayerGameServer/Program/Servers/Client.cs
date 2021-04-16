using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using SocketMultiplayerGameServer.Tools;
using SocketMultiplayerGameServer.DAO;
using SocketGameProtocol;
using MySql.Data.MySqlClient;

namespace SocketMultiplayerGameServer.Servers
{
    class Client
    {
        private Socket socket;
        private Message message;
        private UserData userData;
        private Server server;
        private MySqlConnection sqlConnection;

        private const string connstr = "database=socketgame;data source = 127.0.0.1;user=root;password=machunyang;pooling = false;charset=utf8;port=3306";

        public UserData GetUserData
        {
            get { return userData; }
        }

        public Client(Socket socket,Server server)
        {
            message = new Message();
            userData = new UserData();
            sqlConnection = new MySqlConnection(connstr);

            sqlConnection.Open(); 


            this.server = server;
            this.socket = socket;

            StartRecive();
        }

        void StartRecive()
        {
            socket.BeginReceive(message.Buffer, message.StartIndex, message.Remsize, SocketFlags.None, ReceiveCallBack, null);

        }

        void ReceiveCallBack(IAsyncResult iar)
        {
            try
            {
                if (socket == null || socket.Connected == false) return;
                int len = socket.EndReceive(iar);
                if (len == 0)
                {
                    return;
                }

                message.ReadBuffer(len,HandleRequest);
                StartRecive();
            }
            catch
            {

            }
        }

        public void Send(MainPack pack)
        {
            socket.Send(Message.PackData(pack));
        }

        void HandleRequest(MainPack pack)
        {
            server.HandleRequest(pack, this);
        }

        public bool Logon(MainPack pack)
        {
            return GetUserData.Logon(pack,sqlConnection);
        }

        private void Close()
        {
            server.RemoveClient(this);
            socket.Close();
            sqlConnection.Close();
        }
    }
}
