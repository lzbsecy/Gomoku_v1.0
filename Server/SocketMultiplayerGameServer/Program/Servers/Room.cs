using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using Google.Protobuf.Collections;

namespace SocketMultiplayerGameServer.Servers
{
    class Room
    {
        private RoomPack roomInfo;
        private Server server;

        private List<Client> clientList = new List<Client>();

        public RoomPack GetRoomInfo{get { return roomInfo; } }

        public Room(Client client, RoomPack pack ,Server server)
        {
            roomInfo = pack;
            clientList.Add(client);
            client.GetRoom = this;
            this.server = server;
        }
        
        public RepeatedField<PlayerPack> GetPlayerInfo()
        {
            RepeatedField<PlayerPack> pack = new RepeatedField<PlayerPack>();
            foreach(var c in clientList)
            {
                PlayerPack player = new PlayerPack();
                player.PlayerName = c.UserName;

                pack.Add(player);
            }
            return pack;
        }

        public void Broadcast(Client client,MainPack pack)
        {
            foreach(var c in clientList)
            {
                if(c.Equals(client))
                {
                    continue;
                }
                c.Send(pack);
            }
        }

        public void Join(Client client)
        {
            clientList.Add(client);
            if (roomInfo.MaxNum >= clientList.Count)
            {
                //满人
                roomInfo.RoomState = 1;
            }
            client.GetRoom = this;
            MainPack pack = new MainPack();
            pack.ActionCode = ActionCode.PlayerList;
            foreach(var player in GetPlayerInfo())
            {
                pack.PlayerPack.Add(player);
            }
            Broadcast(client, pack);
        }

        public void Exit(Server server,Client client)
        {
            MainPack pack = new MainPack();
            if (client == clientList[0])
            {
                //房主离开房间
                client.GetRoom = null;
                pack.ActionCode = ActionCode.Exit;
                Broadcast(client, pack);
                server.RemoveRoom(this);
                return;
            }

            clientList.Remove(client);
            roomInfo.RoomState = 0;
            client.GetRoom = null;
            pack.ActionCode = ActionCode.PlayerList;
            foreach (var player in GetPlayerInfo())
            {
                pack.PlayerPack.Add(player);
            }
            Broadcast(client, pack);
        }
    }
}
