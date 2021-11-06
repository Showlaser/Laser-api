using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LaserAPI
{
    public class LaserConnection
    {
        private ClientWebSocket _clientWebSocket;
        private readonly Uri _url;

        public LaserConnection()
        {
            var uri = new UriBuilder("ws://192.168.1.229:81");
            _url = uri.Uri;

            var t = Task.Run(async () => await Connect());
            t.Wait();
        }

        private async Task Connect()
        {
            _clientWebSocket = new ClientWebSocket();
            await _clientWebSocket.ConnectAsync(_url, CancellationToken.None);
        }

        public async Task Disconnect()
        {
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
        }

        public async Task SendMessage(string message)
        {
            var rcvBytes = Encoding.ASCII.GetBytes(message);
            var rcvBuffer = new ArraySegment<byte>(rcvBytes);
            bool messageSend = false;
            int connectionAttempts = 0;

            while (!messageSend)
            {
                connectionAttempts++;
                if (connectionAttempts > 1)
                {
                    Console.WriteLine(connectionAttempts);
                    Console.Beep(500, 250);
                }

                try
                {
                    await _clientWebSocket.SendAsync(rcvBuffer, WebSocketMessageType.Binary, true, CancellationToken.None);
                    messageSend = true;
                }
                catch (Exception)
                {
                    await Connect();
                }
            }
        }
    }
}
