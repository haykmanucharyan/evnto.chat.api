using evnto.chat.bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace evnto.chat.api.Controllers
{
    [ApiController]
    [Route("/ws")]
    public class WebSocketController : EvntoChatControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>(); 

        public WebSocketController(ILogger<UserController> logger, IBLFactory blFactory) : base(blFactory)
        {
            _logger = logger;
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
                if (_sockets.TryAdd(userId, socket))
                {
                    IUserBL bl = BLFactory.CreateUserBL();
                    bl.UpdateSessionApiKey(context.Request.Headers["Token"]);
                }
            }
            else
                context.Response.StatusCode = 400; // bad request
        }
    }
}
