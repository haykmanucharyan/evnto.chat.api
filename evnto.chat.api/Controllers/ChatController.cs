using evnto.chat.api.Models;
using evnto.chat.bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace evnto.chat.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : EvntoChatControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public ChatController(ILogger<UserController> logger, IBLFactory blFactory) : base(blFactory)
        {
            _logger = logger;
        }

        [HttpPut]
        [Authorize(Constants.EvntoAuthScheme)]
        public void Put([FromBody] ChatModel chat)
        {
            int userId = GetAuthenticatedUser();
            
            IChatBL bl = BLFactory.CreateChatBL();
            bl.InitiateChat(userId, chat.RecipientUserId);
        }

        [HttpPost]
        [Authorize(Constants.EvntoAuthScheme)]
        public void Post([FromBody] ChatModel chat)
        {
            int userId = GetAuthenticatedUser();

            IChatBL bl = BLFactory.CreateChatBL();

            if (chat.State == ChatState.Closed)
                bl.CloseChat(chat.ChatId, userId);
            else
                bl.AcceptOrRejectChat(chat.ChatId, userId, chat.State == ChatState.Accepted);
        }

        [HttpGet]
        [Authorize(Constants.EvntoAuthScheme)]
        public IEnumerable<ChatModel> Get()
        {
            int userId = GetAuthenticatedUser();

            IChatBL bl = BLFactory.CreateChatBL();
            return from c in bl.GetActiveChats(userId)
                   select new ChatModel()
                   {
                       ChatId = c.ChatId,
                       Created = c.Created,
                       InitiatorUserId = c.InitiatorUserId,
                       RecipientUserId = c.RecipientUserId,
                       State = (ChatState)c.State
                   };
        }
    }
}
