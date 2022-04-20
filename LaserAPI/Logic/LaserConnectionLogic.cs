using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public static class LaserConnectionLogic
    {
        public static bool RanByUnitTest { get; set; } = false;
        public static string ComputerIpAddress { get; set; }
        public static LaserMessage PreviousMessage { get; set; } = new();
        public static bool ConnectionPending { get; private set; }
        private static TcpListener _server;
        private static NetworkStream _stream;
        public static TcpClient TcpClient { get; private set; }
        private static readonly SerialPort SerialPort = new();
        private static Thread _networkConnectionThread;

        public static void NetworkConnect()
        {
            try
            {
                IPAddress localAddress = IPAddress.Parse(ComputerIpAddress);
                _server = new TcpListener(localAddress, 50000)
                {
                    Server =
                    {
                        SendTimeout = -1
                    }
                };

                ConnectionPending = true;
                _server.Start();
                Console.WriteLine("Waiting");

                TcpClient = _server.AcceptTcpClient();
                ConnectionPending = false;
                Console.WriteLine("Connected with laser");
                _stream = TcpClient.GetStream();
            }
            catch (Exception)
            {
                TcpClient?.Close();
            }
        }

        public static void SendMessages(List<LaserMessage> messages)
        {
            if (RanByUnitTest)
            {
                return;
            }

            if (TcpClient?.Connected is false)
            {
                NetworkConnect();
            }

            try
            {
                int messagesLength = messages.Count;
                bool messagesCannotBeSend = messagesLength == 0 || _networkConnectionThread?.IsAlive is true;
                if (messagesCannotBeSend)
                {
                    return;
                }

                async void Start() => await SendNetworkDataToLaser(messages, messagesLength);
                _networkConnectionThread = new Thread(Start)
                {
                    Priority = ThreadPriority.Highest,
                    IsBackground = true
                };

                _networkConnectionThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                TcpClient?.Close();
                _server.Stop();
                NetworkConnect();
            }
        }

        private static async Task SendNetworkDataToLaser(IReadOnlyList<LaserMessage> messages, int messagesLength)
        {
            string json = ConvertMessagesToJson(messages, messagesLength);
            UTF8Encoding utf8 = new();
            byte[] msg = utf8.GetBytes(json);
            await _stream.WriteAsync(msg);

            var bytes = new byte[msg.Length];
            await _stream.ReadAsync(bytes);
            PreviousMessage = messages[^1];
        }

        private static string ConvertMessagesToJson(IReadOnlyList<LaserMessage> messages, int messagesLength)
        {
            string json = "[";
            for (int i = 0; i < messagesLength; i++)
            {
                LaserMessage message = messages[i];
                string jsonMessage = "{\"r\":" + message.RedLaser + ",\"g\":" + message.GreenLaser + ",\"b\":" +
                                     message.BlueLaser + ",\"x\":" + message.X + ",\"y\":" + message.Y + "}";
                if (i + 1 != messagesLength)
                {
                    jsonMessage += ",";
                }

                json += jsonMessage;
            }

            json += "]";
            return json;
        }

        public static string[] GetAvailableComDevices()
        {
            return SerialPort.GetPortNames();
        }

        public static void ConnectSerial(string portName)
        {
            if (SerialPort.IsOpen)
            {
                return;
            }

            SerialPort.PortName = portName;
            SerialPort.BaudRate = 9600;
            SerialPort.Open();
        }

        public static void SetLaserSettingsBySerial(string json)
        {
            if (!SerialPort.IsOpen)
            {
                throw new ConnectionAbortedException("No connection to the com device was available");
            }

            SerialPort.WriteLine(json);

            string returnMessage = "";
            int iterations = 0;
            while (!returnMessage.Contains("Settings ip is:") && iterations < 25)
            {
                returnMessage = SerialPort.ReadLine();
                iterations++;
                Thread.Sleep(500);
            }

            SerialPort.Close();
            if (!returnMessage.Contains("Settings ip is:"))
            {
                throw new InvalidOperationException("The laser was not in settings mode!");
            }
        }
    }
}
