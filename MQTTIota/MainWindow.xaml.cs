using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using MQTTIota;
namespace MQTTIota
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static MainWindow main;
        MQTT MQTTClient;
        Iota IotaClient;

        public MainWindow()
        {
            main = this;
            InitializeComponent();
            MQTTClient = new MQTT("iot.eclipse.org", "/IotaTransaction");
            IotaClient = new Iota("localhost", 14265);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DoTransfer();
        }

        private void DoTransfer()
        {
            MQTTClient.SendMessage("Example message");
            IotaClient.CreateTransaction();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            IotaClient.GetTransfers();
        }
    }
}
