using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LaserAPI
{
    public class LaserConnection
    {
        private Socket _socket;

        public LaserConnection()
        {
            Task.Run(async () => await Connect()).Wait();
        }

        private async Task Connect()
        {
            IPAddress address = IPAddress.Parse("192.168.1.177");
            IPEndPoint remoteEP = new IPEndPoint(address, 80);

            _socket = new Socket(address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            await _socket.ConnectAsync(remoteEP);
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void SendMessage(string message)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(message);
                _socket.Send(msg);
            }
            catch (Exception)
            {
                Task.Run(async () => await Connect()).Wait();
            }
        }
    }
}
