using evnto.chat.bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace evnto.chat.api.Controllers
{
    public class EvntoChatControllerBase : ControllerBase
    {
        protected IBLFactory BLFactory { get; private set; }

        public EvntoChatControllerBase(IBLFactory blFactory)
        {
            BLFactory = blFactory;
        }

        protected int GetAuthenticatedUser()
        {
            return int.Parse(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value);
        }
    }
}
