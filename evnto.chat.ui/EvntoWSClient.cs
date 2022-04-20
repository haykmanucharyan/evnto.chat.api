using evnto.chat.ui.Models;
using System.Net.WebSockets;

namespace evnto.chat.ui
{
    public class EvntoWSClient
    {
        ClientWebSocket _socket;
        string _url;
        public UserSessionModel Session { get; private set; }

        public EvntoWSClient(string url, UserSessionModel session)
        {
            _url = url;
            Session = session;
            _socket = new ClientWebSocket();
        }

        public async Task RecieveAsync(CancellationToken cancellationToken)
        {
            _socket.Options.SetRequestHeader("Token", Session.Token);

            await _socket.ConnectAsync(new Uri(_url), cancellationToken);

            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);

            while (true)
            {
                WebSocketReceiveResult rcvResult = await _socket.ReceiveAsync(buffer, cancellationToken);
                byte[] msgBytes = buffer.Skip(buffer.Offset).Take(rcvResult.Count).ToArray();
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                if (_socket.State == WebSocketState.Open)
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal closure.", CancellationToken.None);

                _socket.Dispose();
            }
            catch
            {
                _socket.Dispose();
            }
        }
    }
}
