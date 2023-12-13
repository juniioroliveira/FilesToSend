using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FilesToSend
{
    public class WebSocketFileSender
    {
        private readonly Uri _serverUri;

        public WebSocketFileSender(string serverUri)
        {
            _serverUri = new Uri(serverUri);
        }

        public async Task SendFileAsync(string filePath)
        {
            byte[] buffer = new byte[1024 * 4];
            ClientWebSocket webSocket = new ClientWebSocket();

            await webSocket.ConnectAsync(_serverUri, CancellationToken.None);

            using (FileStream stream = File.OpenRead(filePath))
            {
                int bytesRead;
                do
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), WebSocketMessageType.Binary, bytesRead < buffer.Length, CancellationToken.None);
                }
                while (bytesRead == buffer.Length);

                //await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            }
        }
    }
}

