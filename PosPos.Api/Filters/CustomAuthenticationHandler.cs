using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pospos.Core.Common;
using Pospos.Core.Helpers;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Pospos.Api.Filters
{
    public class CustomAuthenticationSchemeOptions : AuthenticationSchemeOptions { }
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationSchemeOptions>
    {
        private readonly SecurityHelper _securityHelper;
        //private readonly UserService _userService;
        public CustomAuthenticationHandler(SecurityHelper securityHelper, ILoggerFactory logger, IOptionsMonitor<CustomAuthenticationSchemeOptions> options,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _securityHelper = securityHelper;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            UserToken model;
            if (!Request.Headers.ContainsKey("token"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Header not found"));
            }
            string token = Request.Headers["token"].ToString();
            if (string.IsNullOrWhiteSpace(token))
            {
                return Task.FromResult(AuthenticateResult.Fail("token can not be empty"));
            }
            try
            {
                model = _securityHelper.DecodeToken(token);
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));
            }
            if (model != null)
            {
                if (model.ExpireDateTime < DateTime.Now) return Task.FromResult(AuthenticateResult.Fail("token is not valid"));
            }
            else
                model = new UserToken();

            //Check token in db

            var claims = new[] {
                new Claim(ClaimTypes.SerialNumber, model.Id),
                new Claim(ClaimTypes.Role,model.RoleIds) };
            var claimsIdentity = new ClaimsIdentity(claims, nameof(CustomAuthenticationHandler));

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
