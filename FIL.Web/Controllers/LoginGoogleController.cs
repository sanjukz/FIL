using FIL.Contracts.Commands.Users;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.User;
using FIL.Foundation.Senders;
using FIL.Logging;
using FIL.MailChimp.Models;
using FIL.Web.Core.Helpers;
using FIL.Web.Core.Providers;
using FIL.Web.Core.ViewModels.Login;
using FIL.Web.Feel.ViewModels.SocialSignIn;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FIL.Web.Feel.Controllers
{
    public class LoginGoogleController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly ISessionProvider _sessionProvider;
        private readonly ILogger _logger;
        private readonly IClientIpProvider _clientIpProvider;
        private readonly MailChimp.IMailChimpProvider _mailChimpProvider;

        public LoginGoogleController(ICommandSender commandSender, IQuerySender querySender, IAuthenticationHelper authenticationHelper, ISessionProvider sessionProvider, ILogger logger,
            IClientIpProvider clientIpProvider, MailChimp.IMailChimpProvider mailChimpProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _authenticationHelper = authenticationHelper;
            _sessionProvider = sessionProvider;
            _logger = logger;
            _clientIpProvider = clientIpProvider;
            _mailChimpProvider = mailChimpProvider;
        }

        [HttpPost]
        [Route("api/login/google")]
        public async Task<GoogleSignInResponseViewModel> SignInGoogle([FromBody] GoogleSignInFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new { Succeeded = true };
                var email = model.Email;
                var passwordHasher = new PasswordHasher<string>();
                string password = new Random().NextDouble().ToString();
                string PasswordHash = passwordHasher.HashPassword(model.Email, password);
                try
                {
                    var Email = model.Email;
                    var userSearchResult = await _querySender.Send(new UserSearchQuery
                    {
                        Email = model.Email,
                        ChannelId = Channels.Feel,
                        SignUpMethodId = SignUpMethods.Google,
                    });
                    if (userSearchResult.Success)
                    {
                        var UserData = new LoginWithGoogleFormDataViewModel
                        {
                            Email = model.Email,
                            SocialLoginId = model.SocialLoginId,
                            ChannelId = Channels.Feel,
                            SignUpMethodId = SignUpMethods.Google,
                        };

                        var authenticated = await _authenticationHelper.AuthenticateGoogleUser(UserData, u =>
                        {
                            return Task.FromResult(true);
                        });

                        return new GoogleSignInResponseViewModel
                        {
                            Success = authenticated,
                            Session = await _sessionProvider.Get()
                        };
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.Email))
                        {
                            return new GoogleSignInResponseViewModel
                            {
                                Success = false,
                                IsEmailRequired = true,
                                Session = await _sessionProvider.Get()
                            };
                        }
                        await _commandSender.Send(new RasvRegisterUserCommand
                        {
                            Email = model.Email,
                            PasswordHash = PasswordHash,
                            UserName = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PhoneCode = model.PhoneCode,
                            PhoneNumber = model.PhoneNumber,
                            ChannelId = Channels.Feel,
                            RolesId = 11,
                            SocialLoginId = model.SocialLoginId,
                            OptedForMailer = true,
                            Ip = _clientIpProvider.Get(),
                            SignUpMethodId = SignUpMethods.Google,
                            ReferralId = model.ReferralId
                        });
                        var UserData = new LoginWithGoogleFormDataViewModel
                        {
                            Email = model.Email,
                            SocialLoginId = model.SocialLoginId,
                            ChannelId = Channels.Feel,
                            SignUpMethodId = SignUpMethods.Google,
                        };

                        var authenticated = await _authenticationHelper.AuthenticateGoogleUser(UserData, u =>
                        {
                            return Task.FromResult(true);
                        });

                        // adding user to mailChimp contacts
                        try
                        {
                            var query = await _querySender.Send(new UserSearchQuery
                            {
                                Email = model.Email,
                                ChannelId = Channels.Feel,
                                SignUpMethodId = SignUpMethods.Google,
                            });

                            await _mailChimpProvider.AddFILMember(new MCUserModel
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                PhoneCode = model.PhoneCode,
                                PhoneNumber = model.PhoneNumber,
                                IsCreator = false,
                                SignUpType = "Google"
                            }, query.Country);
                        }
                        catch (Exception e)
                        {
                            _logger.Log(Logging.Enums.LogCategory.Error, e);
                        }
                        

                        return new GoogleSignInResponseViewModel
                        {
                            Success = authenticated,
                            Session = await _sessionProvider.Get()
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new GoogleSignInResponseViewModel { Success = false };
                }
            }
            else
            {
                return new GoogleSignInResponseViewModel { Success = false };
            }
        }


        [HttpPost]
        [Route("api/login/facebook")]
        public async Task<FacebookSignInResponseViewModel> SignInFacebook([FromBody] GoogleSignInFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new { Succeeded = true };
                var email = model.Email;
                var passwordHasher = new PasswordHasher<string>();
                string password = new Random().NextDouble().ToString();
                string PasswordHash = passwordHasher.HashPassword(model.Email, password);
                try
                {
                    var Email = model.Email;
                    var userSearchResult = await _querySender.Send(new UserSearchQuery
                    {
                        Email = model.Email,
                        ChannelId = Channels.Feel,
                        SignUpMethodId = SignUpMethods.Facebook,
                        SocialLoginId = model.SocialLoginId
                    });
                    if (userSearchResult.Success)
                    {
                        var UserData = new LoginWithFacebookFormDataViewModel
                        {
                            Email = model.Email,
                            SocialLoginId = model.SocialLoginId,
                            ChannelId = Channels.Feel,
                            SignUpMethodId = SignUpMethods.Facebook,
                        };

                        var authenticated = await _authenticationHelper.AuthenticateFacebookUser(UserData, u =>
                        {
                            return Task.FromResult(true);
                        });

                        return new FacebookSignInResponseViewModel
                        {
                            Success = authenticated,
                            Session = await _sessionProvider.Get()
                        };
                    }
                    else if (string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.SocialLoginId))
                    {
                        return new FacebookSignInResponseViewModel
                        {
                            Success = false,
                            IsEmailReqd = true
                        };
                    }
                    else
                    {

                        await _commandSender.Send(new RasvRegisterUserCommand
                        {
                            Email = model.Email,
                            PasswordHash = PasswordHash,
                            UserName = model.Email,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            PhoneCode = model.PhoneCode,
                            RolesId = 11,
                            PhoneNumber = model.PhoneNumber,
                            ChannelId = Channels.Feel,
                            SocialLoginId = model.SocialLoginId,
                            OptedForMailer = true,
                            Ip = _clientIpProvider.Get(),
                            SignUpMethodId = SignUpMethods.Facebook,
                            ReferralId = model.ReferralId,
                        });
                        var UserData = new LoginWithFacebookFormDataViewModel
                        {
                            Email = model.Email,
                            SocialLoginId = model.SocialLoginId,
                            ChannelId = Channels.Feel
                        };

                        var authenticated = await _authenticationHelper.AuthenticateFacebookUser(UserData, u =>
                        {
                            return Task.FromResult(true);
                        });

                        //adding userr to MailChimp contacts
                        try
                        {
                            var query = await _querySender.Send(new UserSearchQuery
                            {
                                Email = model.Email,
                                ChannelId = Channels.Feel,
                                SignUpMethodId = SignUpMethods.Facebook,
                            });

                            await _mailChimpProvider.AddFILMember(new MCUserModel
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                PhoneCode = model.PhoneCode,
                                PhoneNumber = model.PhoneNumber,
                                SignUpType = "Facebook",
                                IsCreator = false
                            }, query.Country);
                        }
                        catch (Exception e)
                        {
                            _logger.Log(Logging.Enums.LogCategory.Error, e);
                        }
                        
                        return new FacebookSignInResponseViewModel
                        {
                            Success = authenticated,
                            Session = await _sessionProvider.Get()
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new FacebookSignInResponseViewModel { Success = false };
                }
            }
            else
            {
                return new FacebookSignInResponseViewModel { Success = false };
            }
        }
    }
}
