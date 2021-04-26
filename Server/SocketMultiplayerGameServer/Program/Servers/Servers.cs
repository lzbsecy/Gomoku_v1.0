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
        private List<Room> roomList = new List<Room>();

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

        public void HandleRequest(MainPack pack,Client client)
        {
            controllerManager.HandleRequest(pack, client);
        }

        public void RemoveClient(Client client)
        {
            clientList.Remove(client);
            client = null;
        }

        public MainPack CreateRoom(Client client, MainPack pack)
        {
            try
            {
                Room room = new Room(client, pack.RoomPack[0],this);
                roomList.Add(room);
                foreach (var p in room.GetPlayerInfo())
                {
                    pack.PlayerPack.Add(p);
                }
                pack.ReturnCode = ReturnCode.Succeed;
                return pack;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                pack.ReturnCode = ReturnCode.Fail;
                return pack;
            }
            
        }

        public MainPack SearchRoom()
        {
            MainPack pack = new MainPack();
            pack.ActionCode = ActionCode.SearchRoom;
            try
            {
                if(roomList.Count==0)
                {
                    pack.ReturnCode = ReturnCode.NotRoom;
                    return pack;
                }
                foreach (var room in roomList)
                {
                    //pack.ActionCode = ActionCode.SearchRoom;
                    pack.RoomPack.Add(room.GetRoomInfo);
                }
                pack.ReturnCode = ReturnCode.Succeed;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                pack.ReturnCode = ReturnCode.Fail;
            }
            return pack;
        }

        public MainPack JoinRoom(Client client,MainPack pack)
        {
            foreach(var room in roomList)
            {
                if(room.GetRoomInfo.RoomName.Equals(pack.Str))
                { 
                    if (room.GetRoomInfo.RoomState == 0)//有房间
                    {
                        //可以加入
                        room.Join(client);
                        pack.RoomPack.Add(room.GetRoomInfo);
                        foreach (var p in room.GetPlayerInfo())
                        {
                            pack.PlayerPack.Add(p);
                        }
                        pack.ReturnCode = ReturnCode.Succeed;
                        return pack;
                    }
                    else
                    {
                        //房间不可加入
                        pack.ReturnCode = ReturnCode.Fail;
                        return pack;
                    }
                }
            }
            //没有找到房间
            pack.ReturnCode = ReturnCode.NotRoom;
            return pack;
        }

        public MainPack ExitRoom(Client client,MainPack pack)
        {
            if (client.GetRoom == null)
            {
                pack.ReturnCode = ReturnCode.Fail;
                return pack;
            }

            client.GetRoom.Exit(this,client);
            pack.ReturnCode = ReturnCode.Succeed;
            return pack;
        }

        public void RemoveRoom(Room room)
        {
            roomList.Remove(room);
        }
    }

}
