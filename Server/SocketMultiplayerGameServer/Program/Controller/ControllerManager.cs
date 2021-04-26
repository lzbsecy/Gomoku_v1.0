using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;
using System.Reflection;
using SocketMultiplayerGameServer.Servers;

namespace SocketMultiplayerGameServer.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controlDict = new Dictionary<RequestCode, BaseController>();

        private Server server;

        public ControllerManager(Server server)
        {
            this.server = server;

            UserController userController = new UserController();
            controlDict.Add(userController.GetRequestcode,userController);
            RoomController roomController = new RoomController();
            controlDict.Add(roomController.GetRequestcode, roomController);
        }

        public void HandleRequest(MainPack pack,Client client)
        {
            if(controlDict.TryGetValue(pack.RequestCode,out BaseController controller))
            {
                string metname = pack.ActionCode.ToString();
                Console.WriteLine(metname);
                MethodInfo method = controller.GetType().GetMethod(metname);
                if(method==null)
                {
                    Console.WriteLine("没有找到对应的处理方法!");
                    return;
                }
                object[] obj = new object[] { server, client, pack };
                object ret = method.Invoke(controller, obj);
                if (ret != null) 
                {
                    client.Send(ret as MainPack);
                }
            }
            else
            {
                Console.WriteLine("没有找到对应的controller");
            }
        }

    }
}
