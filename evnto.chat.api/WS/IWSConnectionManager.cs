using evnto.chat.bus;
using System.Net.WebSockets;

namespace evnto.chat.api.WS
{
    public interface IWSConnectionManager : IDisposable
    {
        bool AddSocket(int userId, WebSocket socket);

        WebSocket GetWebSocket(int userId);

        Task WriteAsync(int userId, RmqMessage message);

        Task Write2AllAsync(RmqMessage message);

        Task RemoveSocketAsync(int userId);
    }
}
