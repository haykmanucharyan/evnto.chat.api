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

        public async Task ConnectAsync()
        {
            ClientWebSocketOptions options = _socket.Options;
            options.SetRequestHeader("Token", Session.Token);

            await _socket.ConnectAsync(new Uri(_url), CancellationToken.None);
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
