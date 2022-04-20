using evnto.chat.api.WS;
using evnto.chat.bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace evnto.chat.api.Controllers
{
    [ApiController]
    [Route("/ws")]
    public class WebSocketController : EvntoChatControllerBase
    {
        private readonly ILogger<UserController> _logger;
        IWSConnectionManager _wSConnectionManager;

        public WebSocketController(ILogger<UserController> logger, IBLFactory blFactory, IWSConnectionManager wSConnectionManager) : base(blFactory)
        {
            _logger = logger;
            _wSConnectionManager = wSConnectionManager;
        }

        [HttpGet]
        [Authorize(Constants.EvntoAuthScheme)]
        public async Task Get()
        {
            HttpContext context = ControllerContext.HttpContext;

            if (context.WebSockets.IsWebSocketRequest)
            {
                int userId = GetAuthenticatedUser();
                WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
                if (_wSConnectionManager.AddSocket(userId, socket))
                {
                    IUserBL bl = BLFactory.CreateUserBL();
                    bl.UpdateSessionApiKey(context.Request.Headers["Token"]);
                }

                bool flagSuccess = true;

                try
                {
                    ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
                    while (true)
                        await socket.ReceiveAsync(buffer, CancellationToken.None);
                }
                catch // avoid awaiting in catch block
                {
                    flagSuccess = false;
                }

                if (!flagSuccess)
                    await _wSConnectionManager.RemoveSocketAsync(userId);
            }
            else
                context.Response.StatusCode = 400; // bad request
        }
    }
}
