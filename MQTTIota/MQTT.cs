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
        string messages;

        public MQTT(string host, string topic)
        {
            client = Initialise(host, topic);
        }

        private MqttClient Initialise(string host, string topic)
        {
            client = new MqttClient(host);
            client.MqttMsgPublishReceived += client_recievedMessage;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            client.Subscribe(new String[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
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
   }
}
