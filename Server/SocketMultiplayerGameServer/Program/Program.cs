using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketMultiplayerGameServer.Servers;


namespace SocketMultiplayerGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Servers已启动!");
            Server servers = new Server(6666);
            Console.Read();
        }
    }
}
