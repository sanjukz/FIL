using FIL.Contracts.Enums;
using FIL.Contracts.Queries.User;
using FIL.Foundation.Senders;
using FIL.Web.Core.ViewModels.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FIL.Web.Core.Helpers
{
    public interface IAuthenticationHelper
    {
        Task<bool> AuthenticateUser(LoginFormDataViewModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation);
        Task<bool> AuthenticateGoogleUser(LoginWithGoogleFormDataViewModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation);
        Task<bool> AuthenticateFacebookUser(LoginWithFacebookFormDataViewModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation);
        Task<LoginWithOTPResponseModel> AuthenticateUserWithOTP(LoginWithOTPFormModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation);
        bool ValidateLoginForm(string Email, string Password, SignUpMethods? SignUpMethodId);
        Task<RedirectResult> SignOut();
    }

    public class AuthenticationHelper : IAuthenticationHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQuerySender _querySender;

        public AuthenticationHelper(IHttpContextAccessor httpContextAccessor,
            IQuerySender querySender)
        {
            _httpContextAccessor = httpContextAccessor;
            _querySender = querySender;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginFormDataViewModel"></param>
        /// <param name="accessValidation">Custom access validation. ie. check for module links, companies, roles, etc.</param>
        /// <returns></returns>
        public async Task<bool> AuthenticateUser(LoginFormDataViewModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation)
        {
            bool isFormValid = ValidateLoginForm(loginFormDataViewModel.Email, loginFormDataViewModel.Password, SignUpMethods.Regular);
            if (!isFormValid)
            {
                return false;
            }

            var email = loginFormDataViewModel.Email;
            var queryResult = await _querySender.Send(new LoginUserQuery
            {
                Email = email,
                Password = loginFormDataViewModel.Password,
                ChannelId = loginFormDataViewModel.ChannelId,
                SignUpMethodId = loginFormDataViewModel.SignUpMethodId,
                SiteId = loginFormDataViewModel.SiteId ?? Site.ComSite
            });

            var user = queryResult.User;
            if (!queryResult.Success || user == null || (loginFormDataViewModel.ChannelId == Channels.Feel && !user.IsActivated))
            {
                return false;
            }

            var accessGranted = await accessValidation(queryResult.User);
            if (accessGranted)
            {
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AltId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Roles", user.RolesId.ToString())
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));

                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                {
                    IsPersistent = loginFormDataViewModel.RememberLogin,
                    ExpiresUtc = DateTime.UtcNow.AddDays(15)
                });

                // Set so we can access SessionProvider in a normal way during the rest of the request
                _httpContextAccessor.HttpContext.User = claimsPrincipal;
            }
            return accessGranted;
        }

        public async Task<bool> AuthenticateGoogleUser(LoginWithGoogleFormDataViewModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation)
        {
            bool isFormValid = ValidateLoginForm(loginFormDataViewModel.Email, "", SignUpMethods.Google);
            if (!isFormValid)
            {
                return false;
            }
            var email = loginFormDataViewModel.Email;
            var queryResult = await _querySender.Send(new LoginWithGoogleUserQuery
            {
                Email = email,
                SocialLoginId = loginFormDataViewModel.SocialLoginId,
                ChannelId = loginFormDataViewModel.ChannelId,
                SignUpMethodId = loginFormDataViewModel.SignUpMethodId,
                SiteId = loginFormDataViewModel.SiteId ?? Site.ComSite,
            });

            var user = queryResult.User;
            if (!queryResult.Success || user == null)
            {
                return false;
            }

            var accessGranted = await accessValidation(queryResult.User);
            if (accessGranted)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AltId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Roles", user.RolesId.ToString())
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));
                await _httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);
                // Set so we can access SessionProvider in a normal way during the rest of the request
                _httpContextAccessor.HttpContext.User = claimsPrincipal;
            }
            return accessGranted;
        }

        public async Task<bool> AuthenticateFacebookUser(LoginWithFacebookFormDataViewModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation)
        {
            bool isFormValid = ValidateLoginForm(loginFormDataViewModel.SocialLoginId, "", SignUpMethods.Facebook);
            if (!isFormValid)
            {
                return false;
            }
            var email = loginFormDataViewModel.Email;
            var queryResult = await _querySender.Send(new LoginWithFacebookUserQuery
            {
                Email = email,
                SocialLoginId = loginFormDataViewModel.SocialLoginId,
                ChannelId = loginFormDataViewModel.ChannelId,
                SignUpMethodId = loginFormDataViewModel.SignUpMethodId,
                SiteId = loginFormDataViewModel.SiteId ?? Site.ComSite,
            });

            var user = queryResult.User;
            if (!queryResult.Success || user == null)
            {
                return false;
            }

            var accessGranted = await accessValidation(queryResult.User);
            if (accessGranted)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AltId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Roles", user.RolesId.ToString())
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));
                await _httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);
                // Set so we can access SessionProvider in a normal way during the rest of the request
                _httpContextAccessor.HttpContext.User = claimsPrincipal;
            }
            return accessGranted;
        }
        public async Task<LoginWithOTPResponseModel> AuthenticateUserWithOTP(LoginWithOTPFormModel loginFormDataViewModel, Func<Contracts.Models.User, Task<bool>> accessValidation)
        {
            if (string.IsNullOrEmpty(loginFormDataViewModel.PhoneCode)
                || string.IsNullOrEmpty(loginFormDataViewModel.PhoneNumber))
            {
                return new LoginWithOTPResponseModel { Success = false };
            }
            var queryResult = await _querySender.Send(new LoginWithOTPQuery
            {
                PhoneCode = loginFormDataViewModel.PhoneCode,
                PhoneNumber = loginFormDataViewModel.PhoneNumber,
                ChannelId = loginFormDataViewModel.ChannelId,
                SignUpMethodId = loginFormDataViewModel.SignUpMethodId
            });

            var user = queryResult.User;
            if (!queryResult.Success || user == null)
            {
                return new LoginWithOTPResponseModel
                {
                    Success = false,
                    IsAdditionalFieldsReqd = queryResult.IsAdditionalFieldsReqd
                };
            }

            var accessGranted = await accessValidation(queryResult.User);
            if (accessGranted)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.AltId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Roles", user.RolesId.ToString())
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "login"));
                await _httpContextAccessor.HttpContext.SignInAsync(claimsPrincipal);
                // Set so we can access SessionProvider in a normal way during the rest of the request
                _httpContextAccessor.HttpContext.User = claimsPrincipal;
            }
            return new LoginWithOTPResponseModel
            {
                Success = true,
                User = queryResult.User
            };
        }

        public bool ValidateLoginForm(string Email, string Password, SignUpMethods? SignUpMethodId)
        {
            if (SignUpMethodId != null)
            {
                if (SignUpMethodId == SignUpMethods.Regular || SignUpMethodId == SignUpMethods.None)
                {
                    if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                    {
                        return false;
                    }
                }
                else if (SignUpMethodId == SignUpMethods.Google || SignUpMethodId == SignUpMethods.Facebook || SignUpMethodId == SignUpMethods.LinkedIn)
                {
                    if (string.IsNullOrWhiteSpace(Email))
                    {
                        return false;
                    }
                }
            }
            else if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }
            return true;
        }
        public async Task<RedirectResult> SignOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new RedirectResult("/login");
        }
    }
}
