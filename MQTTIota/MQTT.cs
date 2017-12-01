using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Diagnostics;

namespace MQTTIota
{
    public class MQTT
    {
        public MqttClient client;
        private string _topic;
        private string _host;

        public MQTT(string host, string topic)
        {
            _topic = topic;
            _host = host;
            client = Initialise();
        }

        private MqttClient Initialise()
        {
            client = new MqttClient(_host);
            client.MqttMsgPublishReceived += client_recievedMessage;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.Subscribe(new String[] { _topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            return client;
        }

        static void client_recievedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            // Handle message received
            var message = Encoding.Default.GetString(e.Message);
            Debug.Write("Message received: " + message);
            MainWindow.main.Dispatcher.Invoke(new Action(delegate ()
            {
                MainWindow.main.receiveText.Text += Environment.NewLine + message;
            }));
        }

        internal void SendMessage(string value)
        {
            client.Publish(_topic, Encoding.UTF8.GetBytes(value), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
    }
}
