using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Diagnostics;
using System.IO;

namespace MQTTIota
{
    public class MQTT
    {
        public MqttClient client;
        private string _topic;
        private string _host;
        private static List<KeyValuePair<int, DateTime>> sendTimes = new List<KeyValuePair<int,DateTime>>();
        private static List<KeyValuePair<int, DateTime>> receiveTimes = new List<KeyValuePair<int, DateTime>>();

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

        internal void LogValues()
        {
            var folderName = @"C:\MQTTValues";
            var fileName = Path.GetRandomFileName();
            Directory.CreateDirectory(folderName);
            var pathString = Path.Combine(folderName, fileName);

                //write to file
                using (StreamWriter file = new StreamWriter(pathString))
                {
                    foreach(var x in sendTimes)
                    {
                        try
                        {
                            var receiveTime = receiveTimes.First(y => y.Key == x.Key);

                            TimeSpan span = receiveTime.Value - x.Value;
                            file.WriteLine(x.Value + "," + x.Key + "," + ((int)span.TotalMilliseconds).ToString());
                    } catch { }

                    }

                }

        }

        static void client_recievedMessage(object sender, MqttMsgPublishEventArgs e)
        {
            // Handle message received
            var message = Encoding.Default.GetString(e.Message);
            MainWindow.main.Dispatcher.Invoke(new Action(delegate ()
            {
                MainWindow.main.receiveText.Text += Environment.NewLine + "Received message: " + message;
            }));
            try
            {
                receiveTimes.Add(new KeyValuePair<int, DateTime>(Convert.ToInt32(message), DateTime.Now));
            } catch { }

        }

        internal void SendMessage(int count)
        {
            try
            {
                sendTimes.Add(new KeyValuePair<int, DateTime>(count, DateTime.Now));
            }
            catch { }

            client.Publish(_topic, Encoding.UTF8.GetBytes(count.ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            MainWindow.main.Dispatcher.Invoke(new Action(delegate ()
            {
                MainWindow.main.receiveText.Text = Environment.NewLine + "Sent message: " + count;
            }));
        }
    }
}
