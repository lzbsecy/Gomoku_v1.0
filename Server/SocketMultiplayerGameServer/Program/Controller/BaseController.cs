﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketGameProtocol;

namespace SocketMultiplayerGameServer.Controller
{
    abstract class BaseController
    {
        protected RequestCode requestCode = RequestCode.RequestNone;

        public RequestCode GetRequestcode
        {
            get { return requestCode; }
        }


    }
}
