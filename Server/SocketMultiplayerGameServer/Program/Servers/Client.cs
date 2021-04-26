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

        public string UserName
        {
            get; set;
        }

        public UserData GetUserData
        {
            get { return userData; }
        }

        public Room GetRoom
        {
            get;set;
        }

        public MySqlConnection GetMySqlConnect
        {
            get { return sqlConnection; }
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
                    Close();
                    return;
                }

                message.ReadBuffer(len,HandleRequest);
                StartRecive();
            }
            catch(Exception e)
            { 
                Console.WriteLine(e.Message);
                Close();
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

        private void Close()
        {
            if(GetRoom != null)
            {
                GetRoom.Exit(server,this);
            }
            server.RemoveClient(this);
            socket.Close();
            sqlConnection.Close();
        }
    }
}
