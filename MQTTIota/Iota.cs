using Iota.Lib.CSharp.Api;
using Iota.Lib.CSharp.Api.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTIota
{
    public class Iota
    {
        public string _host;
        public int _port;

        public Iota(string host, int port)
        {
            _host = host;
            _port = port;

            Initialise();
        }
        private void Initialise()
        {
            IotaApi client = new IotaApi(_host, _port);
            GetNodeInfoResponse nodeInfo = client.GetNodeInfo();
            Debug.Write("test");
        }
    }
}
