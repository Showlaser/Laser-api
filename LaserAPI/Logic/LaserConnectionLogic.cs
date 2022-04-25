using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Connections;
using System;
using System.Collections.Generic;
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

        public static async Task SendMessages(IReadOnlyList<LaserMessage> messages)
        {
            int messagesLength = messages.Count;
            bool messagesCannotBeSend = messagesLength == 0;
            if (RanByUnitTest || messagesCannotBeSend)
            {
                return;
            }

            if (TcpClient?.Connected is null || !TcpClient.Connected)
            {
                NetworkConnect();
            }

            await SendNetworkDataToLaser(messages, messagesLength);
        }

        private static async Task SendNetworkDataToLaser(IReadOnlyList<LaserMessage> messages, int messagesLength)
        {
            try
            {
                byte[] msg = Utf8Json.JsonSerializer.Serialize(messages);
                await _stream.WriteAsync(msg);

                byte[] bytes = new byte[msg.Length];
                await _stream.ReadAsync(bytes);
                PreviousMessage = messages[messagesLength - 1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
