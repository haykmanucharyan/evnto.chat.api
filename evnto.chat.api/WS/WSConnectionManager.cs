using evnto.chat.bll.Interfaces;
using evnto.chat.bus;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace evnto.chat.api.WS
{
    public class WSConnectionManager : IWSConnectionManager
    {
        #region Fields

        ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>();
        IBLFactory _blFactory;

        #endregion

        #region Ctor

        public WSConnectionManager(IBLFactory blFactory)
        {
            _blFactory = blFactory;
            _blFactory.GetRmqConnector().RmqMessageArrived += WSConnectionManager_RmqMessageArrived;
        }

        #endregion        

        #region Methods

        private async Task InternalWriteAsync(int userId, WebSocket socket, RmqMessage message)
        {
            bool flagSuccess = true;
            try
            {
                if (socket.State == WebSocketState.Open)
                {
                    ArraySegment<byte> segment = new ArraySegment<byte>(message.ToBytes());
                    await socket.SendAsync(segment, WebSocketMessageType.Binary, true, CancellationToken.None);
                }
            }
            catch // avoid awaiting in catch antipattern
            {
                flagSuccess = false;
            }

            if (!flagSuccess)
                await RemoveSocketAsync(userId);
        }

        public async Task WriteAsync(int userId, RmqMessage message)
        {
            if (_sockets.TryGetValue(userId, out WebSocket socket))
                await InternalWriteAsync(userId, socket, message);
        }

        public async Task Write2AllAsync(RmqMessage message)
        {
            // take entry count without locking
            int cnt = _sockets.Skip(0).Count();

            // avoiding list resize and pass count to list ctor
            List<Task> tasks = new List<Task>(cnt);

            foreach (KeyValuePair<int, WebSocket> pair in _sockets)
                tasks.Add(InternalWriteAsync(pair.Key, pair.Value, message));

            await Task.WhenAll(tasks);
        }

        public bool AddSocket(int userId, WebSocket socket)
        {
            return _sockets.TryAdd(userId, socket);
        }

        public async Task RemoveSocketAsync(int userId)
        {
            if (_sockets.TryRemove(userId, out WebSocket socket))
            {
                IUserBL bl = _blFactory.CreateUserBL();
                bl.SignOut(userId);

                try
                {
                    if (socket.State == WebSocketState.Open)
                        await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Normal closure", CancellationToken.None);

                    socket.Dispose();
                }
                catch
                {
                    socket.Dispose();
                }
            }
        }

        public WebSocket GetWebSocket(int userId)
        {
            if(_sockets.TryGetValue(userId, out WebSocket socket))
                return socket;

            return null;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            _blFactory.GetRmqConnector().RmqMessageArrived -= WSConnectionManager_RmqMessageArrived;
        }

        #endregion

        #region Rmq event handler

        private async void WSConnectionManager_RmqMessageArrived(bool isGlobal, RmqMessage message)
        {
            if (isGlobal)
                await Write2AllAsync(message);
            else
            { 
                
            }
        }

        #endregion
    }
}
