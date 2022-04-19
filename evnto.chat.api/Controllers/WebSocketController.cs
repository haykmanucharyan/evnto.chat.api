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

        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>(); 

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
                WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();                    
                _sockets.TryAdd(context.Request.Headers["Token"], socket);
            }
            else
                context.Response.StatusCode = 400; // bad request
        }
    }
}
