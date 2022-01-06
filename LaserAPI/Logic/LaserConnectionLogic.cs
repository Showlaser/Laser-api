using LaserAPI.Interfaces.Helper;
using LaserAPI.Models.Helper.Laser;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserConnectionLogic : ILaserConnectionLogic
    {
        private Socket _socket;

        public LaserConnectionLogic()
        {
            Task.Run(async () => await Connect()).Wait();
        }

        public async Task Connect()
        {
            IPAddress address = IPAddress.Parse("192.168.1.177");
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
            try
            {
                short[] value = { message.RedLaser, message.GreenLaser, message.BlueLaser, message.X, message.Y };
                string result = string.Join(",", value);
                string json = "{d:[" + result + "]}";

                byte[] msg = Encoding.ASCII.GetBytes(json);
                _socket.Send(msg);
            }
            catch (Exception)
            {
                Task.Run(async () => await Connect()).Wait();
            }
        }
    }
}
