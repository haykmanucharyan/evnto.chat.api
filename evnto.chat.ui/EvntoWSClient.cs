using evnto.chat.ui.Models;
using System.Net.WebSockets;

namespace evnto.chat.ui
{
    public class EvntoWSClient
    {
        ClientWebSocket _socket;
        string _url;
        public UserSessionModel Session { get; private set; }

        public delegate void MessageArrivedHandler(RmqMessage message);
        public event MessageArrivedHandler MessageArrived;

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
                WebSocketReceiveResult result = await _socket.ReceiveAsync(buffer, cancellationToken);
                byte[] data = buffer.Skip(buffer.Offset).Take(result.Count).ToArray();

                MessageArrived?.Invoke(RmqMessage.FromBytes(data));
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
