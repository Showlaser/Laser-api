using LaserAPI.CustomExceptions;
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
        public static string ComputerIpAddress { get; set; }
        public static LaserMessage PreviousMessage { get; set; } = new();
        public static string ConnectionMethod { get; set; }
        public static string ComPort { get; set; }
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
                _server?.Server.Dispose();

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

        private static bool MessagesFormADot(LaserCommando commando, int messagesLength)
        {
            if (messagesLength == 0)
            {
                return false;
            }

            if (messagesLength == 1)
            {
                return true;
            }

            int closeToEachOtherCount = 0;
            int valueToTrigger = Convert.ToInt32(messagesLength * 0.6);

            for (int i = 1; i < messagesLength; i++)
            {
                LaserMessage previousMessage = commando.Messages[i - 1];
                LaserMessage message = commando.Messages[i];
                bool messagesAreCloseToEachOther = Math.Abs(previousMessage.X - message.X) < 20 &&
                                                   Math.Abs(previousMessage.Y - message.Y) < 20;
                if (messagesAreCloseToEachOther)
                {
                    closeToEachOtherCount++;
                }

                if (closeToEachOtherCount >= valueToTrigger)
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task SendMessages(LaserCommando commando)
        {
            int messagesLength = commando.Messages.Length;
            bool messagesInvalid = messagesLength <= 1 || MessagesFormADot(commando, messagesLength);

            if (!commando.Stop)
            {
                if (messagesInvalid)
                {
                    throw new UnsafePatternDetectedException("The pattern is unsafe to project and will not be projected.");
                }
                if (!LaserIsAvailable())
                {
                    return;
                }
            }

            switch (ConnectionMethod)
            {
                case "Network":
                {
                    if (TcpClient?.Connected is null || !TcpClient.Connected)
                    {
                        NetworkConnect();
                    }

                    await SendNetworkDataToLaser(commando);
                    break;
                }
                case "Usb":
                {
                    if (!SerialPort.IsOpen)
                    {
                        ConnectSerial(ComPort);
                    }

                    SendSerialDataToLaser(commando);
                    break;
                }
            }
        }

        private static async Task SendNetworkDataToLaser(LaserCommando commando)
        {
            try
            {
                byte[] messageBytes = Utf8Json.JsonSerializer.Serialize(commando);
                int messageBytesLength = messageBytes.Length + 2;

                byte[] bytes = new byte[messageBytesLength];
                bytes[0] = Convert.ToByte('(');
                bytes[messageBytesLength - 1] = Convert.ToByte(')');

                for (int i = 1; i < messageBytesLength - 1; i++)
                {
                    bytes[i] = messageBytes[i - 1];
                }

                _lastSendMessages = bytes;
                LaserAvailableDateTime = DateTime.Now.AddMilliseconds(commando.DurationInMilliseconds);
                await _stream.WriteAsync(_lastSendMessages);
                PreviousMessage = commando.Messages[^1];
            }
            catch (Exception)
            {
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

        public static void SendSerialDataToLaser(LaserCommando commando)
        {
            try
            {
                byte[] messageBytes = Utf8Json.JsonSerializer.Serialize(commando);
                SerialPort.Write(messageBytes, 0, messageBytes.Length);
                _lastSendMessages = messageBytes;
                LaserAvailableDateTime = DateTime.Now.AddMilliseconds(commando.DurationInMilliseconds);
                PreviousMessage = commando.Messages[^1];
            }
            catch (Exception)
            {
                NetworkConnect();
            }
        }

        public static void SetLaserSettingsBySerial(string json)
        {
            Console.WriteLine("Attempting to save settings");
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

            Console.WriteLine("Settings saved!");
        }
    }
}
