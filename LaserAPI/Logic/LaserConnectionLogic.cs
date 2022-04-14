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
        public static TcpClient Client;
        private static readonly SerialPort SerialPort = new();

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

                Client = _server.AcceptTcpClient();
                ConnectionPending = false;
                Console.WriteLine("Connected with laser");
                _stream = Client.GetStream();
            }
            catch (Exception)
            {
                Client?.Close();
            }
        }

        public static async Task SendMessages(List<LaserMessage> messages)
        {
            if (RanByUnitTest)
            {
                return;
            }

            if (Client != null && !Client.Connected)
            {
                NetworkConnect();
            }

            try
            {
                if (!messages.Any())
                {
                    return;
                }

                string json = "[";
                int messageLength = messages.Count;
                for (int i = 0; i < messageLength; i++)
                {
                    LaserMessage message = messages[i];
                    string jsonMessage = "{\"r\":" + message.RedLaser + ",\"g\":" + message.GreenLaser + ",\"b\":" + message.BlueLaser + ",\"x\":" + message.X + ",\"y\":" + message.Y + "}";
                    if (i + 1 != messageLength)
                    {
                        jsonMessage += ",";
                    }

                    json += jsonMessage;
                }

                json += "]";
                UTF8Encoding utf8 = new();

                byte[] msg = utf8.GetBytes(json);
                await _stream.WriteAsync(msg);

                byte[] bytes = new byte[msg.Length];
                await _stream.ReadAsync(bytes);
                PreviousMessage = messages.Last();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Client.Close();
                _server.Stop();
                NetworkConnect();
            }
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

        public static void SendDataAsJson(string json)
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
