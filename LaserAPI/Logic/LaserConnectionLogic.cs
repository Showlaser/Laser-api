using LaserAPI.Models.Helper.Laser;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserConnectionLogic
    {
        private readonly bool _ranByUnitTest;
        private Socket _socket;
        public int LastXPosition { get; private set; }
        public int LastYPosition { get; private set; }
        private string _ipAddress;

        public LaserConnectionLogic(bool ranByUnitTest)
        {
            _ranByUnitTest = ranByUnitTest;
        }

        public async Task Connect(string ipAddress)
        {
            _ipAddress = ipAddress;
            IPAddress address = IPAddress.Parse(ipAddress);
            IPEndPoint remoteEp = new(address, 80);

            _socket = new Socket(address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            await _socket.ConnectAsync(remoteEp);
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void SendMessage(LaserMessage message)
        {
            if (_ranByUnitTest)
            {
                return;
            }
            try
            {
                int[] value = { message.RedLaser, message.GreenLaser, message.BlueLaser, message.X, message.Y };
                string result = string.Join(",", value);
                string json = "{d:[" + result + "]}";

                byte[] msg = Encoding.ASCII.GetBytes(json);
                _socket.Send(msg);
                LastXPosition = message.X;
                LastYPosition = message.Y;
            }
            catch (Exception)
            {
                Task.Run(async () => await Connect(_ipAddress)).Wait();
            }
        }
    }
}
