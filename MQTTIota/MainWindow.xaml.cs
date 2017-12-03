using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
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
        int count = 0;
        private Timer aTimer = new Timer();

        public MainWindow()
        {
            main = this;
            InitializeComponent();
            MQTTClient = new MQTT("iot.eclipse.org", "/IotaTransaction");
            IotaClient = new Iota("localhost", 14265);
            InitTimer();

        }

        private void DoTransfer()
        {

            MQTTClient.SendMessage(1);
            //IotaClient.CreateTransaction();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            MQTTClient.LogValues();
        }

        public void InitTimer()
        { 
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            count += 1;
            MQTTClient.SendMessage(count);
        }
    }
}
