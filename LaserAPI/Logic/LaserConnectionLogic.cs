using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Connections;
using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
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
        private static byte[] _lastSendMessages;

        /// <summary>
        /// The datetime when the laser will be available for executing messages
        /// </summary>
        public static DateTime LaserAvailableDateTime { get; private set; }

        public static bool LaserIsAvailable() => DateTime.Now >= LaserAvailableDateTime;

        public static void NetworkConnect()
        {
            try
            {
                IPAddress localAddress = IPAddress.Parse(ComputerIpAddress);
                _server = new TcpListener(localAddress, 50000)
                {
                    Server =
                    {
                        SendTimeout = -1,
                        ReceiveBufferSize = 10000
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

        public static async Task SendMessages(LaserCommando commando)
        {
            int messagesLength = commando.Messages.Length;
            bool messagesInvalid = messagesLength == 0;
            
            if (!commando.Stop)
            {
                if (RanByUnitTest || messagesInvalid || !LaserIsAvailable())
                {
                    return;
                }
            }

            if (TcpClient?.Connected is null || !TcpClient.Connected)
            {
                NetworkConnect();
            }

            await SendNetworkDataToLaser(commando);
        }

        private static async Task SendNetworkDataToLaser(LaserCommando commando)
        {
            _lastSendMessages = Utf8Json.JsonSerializer.Serialize(commando);
            LaserAvailableDateTime = DateTime.Now.AddMilliseconds(commando.DurationInMilliseconds);
            await _stream.WriteAsync(_lastSendMessages);
            PreviousMessage = commando.Messages[^1];
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
