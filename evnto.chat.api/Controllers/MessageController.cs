using evnto.chat.api.Models;
using evnto.chat.bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace evnto.chat.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : EvntoChatControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public MessageController(ILogger<UserController> logger, IBLFactory blFactory) : base(blFactory)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Constants.EvntoAuthScheme)]
        public IEnumerable<MessageModel> Get(int chatId)
        {
            int userId = GetAuthenticatedUser();

            IMessageBL bl = BLFactory.CreateMessageBL();
            return from m in bl.GetChatMessages(userId, chatId)
                   select new MessageModel()
                   {
                       MessageId = m.MessageId,
                       AuthorUserId = m.AuthorUserId,
                       Created = m.Created,
                       Text = m.Text,
                       AuthorUser = new UserModel() { FullName = m.AuthorUser.FullName, UserId = m.AuthorUserId, UserName = m.AuthorUser.UserName }
                   };
        }

        [HttpPut]
        [Authorize(Constants.EvntoAuthScheme)]
        public void Put([FromBody] MessageModel message)
        {
            int userId = GetAuthenticatedUser();

            IMessageBL bl = BLFactory.CreateMessageBL();
            bl.CreteMessage(userId, message.ChatId, message.Text);
        }
    }
}
