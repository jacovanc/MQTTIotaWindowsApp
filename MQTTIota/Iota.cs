using Iota.Lib.CSharp.Api;
using Iota.Lib.CSharp.Api.Core;
using Iota.Lib.CSharp.Api.Model;
using Iota.Lib.CSharp.Api.Utils;
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
        IotaApi client;
        string seed = "ACRXCTKPIBJ9DWXHRKNWUMGBOPGGRKJHRMDIXNPUSMYQL9QLAPUYVQL9SWRHHXFJSGETLI99CZNVWLUD9";

        public Iota(string host, int port)
        {
            _host = host;
            _port = port;

            Initialise();
        }
        private void Initialise()
        {
            client = new IotaApi(_host, _port);
            Debug.Write("test");
        }

        internal void CreateTransaction()
        {
            var address = client.GetNewAddress(seed, 0, false, 1, false);
            Transfer transfer = new Transfer(address.First(), 0, TrytesConverter.ToTrytes("Test Iota Message"), TrytesConverter.ToTrytes("Test Tag"));
            client.SendTransfer(seed, 10, 18, new Transfer[] { transfer });
        }

        internal void GetTransfers()
        {
            var transfers = client.GetTransfers(seed, 0, 5, false);
            MainWindow.main.Dispatcher.Invoke(new Action(delegate ()
            {
                MainWindow.main.receiveText.Text += Environment.NewLine + transfers.ToString();
            }));
        }
    }
}
