using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FIL.Web.Feel.ViewModels.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels;
using FIL.Contracts.Enums;
using Microsoft.AspNetCore.Identity;
using FIL.Web.Core.Helpers;
using FIL.Web.Core.Providers;
using FIL.Web.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using FIL.Contracts.Queries.User;

namespace FIL.Web.Feel.Controllers
{
    public class SessionController : Controller
    {
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;

        public SessionController(IAuthenticationHelper authenticationHelper,
            ISessionProvider sessionProvider,
            IPasswordHasher<string> passwordHasher, IQuerySender querySender)
        {
            _authenticationHelper = authenticationHelper;
            _passwordHasher = passwordHasher;
            _querySender = querySender; 
            _sessionProvider = sessionProvider;
        }

        [HttpGet]
        [Route("api/session")]
        public async Task<SessionViewModel> Get()
        {
            var session = await _sessionProvider.Get();
            if (session.IsAuthenticated)
            {
                ViewBag.session = session;
            }
            return session;
        }

        [HttpPost]
        [Route("api/session/login")]
        public async Task<LoginResponseViewModel> Login([FromBody] LoginFormDataViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isUserLocked = false;
                bool isActivated = true;
                viewModel.ChannelId = Channels.Feel;
                viewModel.SignUpMethodId = SignUpMethods.Regular;
                var authenticated = await _authenticationHelper.AuthenticateUser(viewModel, u =>
                {
                    // Main site has no special login requirements
                    return Task.FromResult(true);
                });

                if (authenticated)
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddMinutes(60);
                    option.Domain = ".feelitlive.com";
                    var userSession = await _sessionProvider.Get();
                    var token = _passwordHasher.HashPassword(userSession.User.AltId.ToString(), "428D28C9-54EC-487F-845E-06EB1294747E");
                    Response.Cookies.Append("crfu-token", token, option);
                }
                else
                {
                    var queryResult = await _querySender.Send(new UserSearchQuery
                    {
                        Email = viewModel.Email,
                        ChannelId = Channels.Feel
                    });
                    if (queryResult.Success && queryResult.User.LockOutEnabled)
                    {
                        isUserLocked = true;
                    }

                    if (queryResult.Success && !queryResult.User.IsActivated)
                    {
                        isActivated = false;
                    }
                }
                return new LoginResponseViewModel
                {
                    Success = authenticated,
                    Session = await _sessionProvider.Get(),
                    IsLockedOut = isUserLocked,
                    IsActivated = isActivated
                };
            }
            else
            {
                return new LoginResponseViewModel();
            }
        }

        [HttpGet]
        [Route("api/session/logout")]
        public Task<RedirectResult> Logout()
        {
            return _authenticationHelper.SignOut();
        }
    }
}
