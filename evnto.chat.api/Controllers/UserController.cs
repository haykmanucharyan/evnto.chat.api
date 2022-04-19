using evnto.chat.api.Models;
using evnto.chat.bll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace evnto.chat.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : EvntoChatControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IBLFactory blFactory) : base(blFactory)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Constants.EvntoAuthScheme)]
        public IEnumerable<UserModel> Get()
        {
            int userId = GetAuthenticatedUser();

            IUserBL bl = BLFactory.CreateUserBL();
            return from u in bl.GetOnlineUsers(userId)
                   select new UserModel()
                   { 
                       FullName = u.FullName, 
                       UserId = u.UserId, 
                       UserName = u.UserName
                   };
        }

        [HttpPut]
        public void Put([FromBody] SignUpModel signUpModel)
        {
            IUserBL bl = BLFactory.CreateUserBL();
            bl.SignUp(signUpModel.FullName, signUpModel.UserName, signUpModel.Password);
        }

        [HttpPost]
        public UserSessionModel Post([FromBody] SignModel signModel)
        {
            IUserBL bl = BLFactory.CreateUserBL();

            if (signModel.UserId > 0)
            {
                bl.SignOut(signModel.UserId);
                return new UserSessionModel() { UserId = 0, Token = string.Empty };
            }
            else
            {
                var session = bl.SignIn(signModel.UserName, signModel.Password);
                return new UserSessionModel() { UserId = session.UserId, Token = session.Token };
            }
        }
    }
}