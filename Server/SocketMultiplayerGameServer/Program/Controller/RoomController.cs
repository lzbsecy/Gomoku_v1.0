using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using SocketMultiplayerGameServer.Servers;

namespace SocketMultiplayerGameServer.Controller
{
    class RoomController:BaseController
    {
        public RoomController()
        {
            requestCode = RequestCode.Room;
        }

        public MainPack CreateRoom(Server server, Client client, MainPack pack)
        {
            return server.CreateRoom(client, pack);
        }

        public MainPack SearchRoom(Server server, Client client, MainPack pack)
        {
            return server.SearchRoom();
        }

        public MainPack JoinRoom(Server server, Client client, MainPack pack)
        {
            return server.JoinRoom(client, pack);
        }
        public MainPack Exit(Server server, Client client, MainPack pack)
        {
            return server.ExitRoom(client, pack);
        }

    }
}
