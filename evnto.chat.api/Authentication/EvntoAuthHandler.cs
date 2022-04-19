using evnto.chat.bll.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace evnto.chat.api.Authentication
{
    public class EvntoAuthHandler : AuthenticationHandler<EvntoAuthSchemeOptions>
    {
        IBLFactory _bLFactory;

        public EvntoAuthHandler(
            IOptionsMonitor<EvntoAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IBLFactory bLFactory)
            : base(options, logger, encoder, clock)
        {
            _bLFactory = bLFactory;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Token"))
                return Task.FromResult(AuthenticateResult.Fail("Missing token."));

            IUserBL bl = _bLFactory.CreateUserBL();
            int userId = bl.GetAuthenticatedUser(Request.Headers["Token"]);

            if (userId == 0)
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));

            Claim[] claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };

            ClaimsIdentity identity = new ClaimsIdentity(claims, nameof(EvntoAuthHandler));

            AuthenticationTicket ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), this.Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
