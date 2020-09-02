using System;
using System.Threading.Tasks;
using FIL.Configuration;
using FIL.Contracts.Commands.Users;
using FIL.Foundation.Senders;
using FIL.Logging;
using FIL.Web.Core.Helpers;
using FIL.Web.Core.Providers;
using FIL.Web.Core.ViewModels.Login;
using FIL.Web.Feel.ViewModels.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Feel.Controllers
{
    public class OTPLoginController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISettings _settings;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IAuthenticationHelper _authenticationHelper;
        private readonly ISessionProvider _sessionProvider;
        private readonly IOTPProvider _otpProvider;
        private readonly ILogger _logger;
        private readonly IClientIpProvider _clientIpProvider;

        public OTPLoginController(ICommandSender commandSender, IQuerySender querySender, IPasswordHasher<string> passwordHasher, IAuthenticationHelper authenticationHelper, ISessionProvider sessionProvider, IOTPProvider otpProvider, ILogger logger, ISettings settings, IClientIpProvider clientIpProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _passwordHasher = passwordHasher;
            _authenticationHelper = authenticationHelper;
            _sessionProvider = sessionProvider;
            _otpProvider = otpProvider;
            _logger = logger;
            _settings = settings;
            _clientIpProvider = clientIpProvider;
        }


        [HttpPost]
        [Route("api/otp/send-and-validate")]
        public SendAndValidateOTPResponseModel SendAndValidate([FromBody] SendAndValidateOTPFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.SendOTP)
                    {
                        var otp = _otpProvider.SendOTP(model.PhoneCode.Split("~")[0], model.PhoneNumber).Result;
                        if (otp != 0)
                        {
                            string mobileNumber = model.PhoneCode.Split("~")[0] + "-" + model.PhoneNumber;
                            var token = _otpProvider.GenerateToken(mobileNumber, otp);
                            if (!string.IsNullOrEmpty(token))
                            {
                                return new SendAndValidateOTPResponseModel
                                {
                                    PhoneCode = model.PhoneCode,
                                    PhoneNumber = model.PhoneNumber,
                                    Success = true,
                                    Token = token,
                                    IsOTPSent = true
                                };
                            }
                            else
                            {
                                return new SendAndValidateOTPResponseModel { Success = false };
                            }
                        }
                        else
                        {
                            return new SendAndValidateOTPResponseModel { Success = false };
                        }
                    }
                    else if (!string.IsNullOrEmpty(model.Token))
                    {
                        string mobileNumber = model.PhoneCode.Split("~")[0] + "-" + model.PhoneNumber;
                        var isValidOtp = _otpProvider.ValidateOTP(mobileNumber, model.Token, model.OTP).Result;
                        if (isValidOtp)
                        {
                            return new SendAndValidateOTPResponseModel
                            {
                                Success = true,
                                IsOtpValid = true
                            };
                        }
                        else
                        {
                            return new SendAndValidateOTPResponseModel
                            {
                                Success = false,
                                IsOtpValid = false
                            };
                        }
                    }
                    else
                    {
                        return new SendAndValidateOTPResponseModel { Success = false };
                    }

                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Warn, ex);
                    return new SendAndValidateOTPResponseModel { Success = false };
                }
            }
            else
            {
                return new SendAndValidateOTPResponseModel { Success = false };
            }

        }
        [HttpPost]
        [Route("api/otp/login")]
        public async Task<LoginWithOTPResponseModel> LoginWithOTP([FromBody] LoginWithOTPFormModel model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Email))
                {
                    var userAuthenticate = AuthenticateUser(model.PhoneCode.Split("~")[0], model.PhoneNumber).Result;
                    return new LoginWithOTPResponseModel
                    {
                        Success = userAuthenticate.Success,
                        Session = await _sessionProvider.Get(),
                        IsAdditionalFieldsReqd = userAuthenticate.IsAdditionalFieldsReqd
                    };
                }
                else
                {
                    //new user or new phoneNumber 
                    var passwordHasher = new PasswordHasher<string>();
                    string password = new Random().NextDouble().ToString();
                    string PasswordHash = passwordHasher.HashPassword(model.Email, password);
                    RegisterUserWithOTPCommandCommandResult registerUserResult = await _commandSender.Send<RegisterUserWithOTPCommand, RegisterUserWithOTPCommandCommandResult>(new RegisterUserWithOTPCommand
                    {
                        Email = model.Email,
                        PasswordHash = PasswordHash,
                        UserName = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneCode = model.PhoneCode,
                        PhoneNumber = model.PhoneNumber,
                        ChannelId = Contracts.Enums.Channels.Feel,
                        SignUpMethodId = Contracts.Enums.SignUpMethods.OTP,
                        ReferralId = model.ReferralId,
                        Ip = _clientIpProvider.Get(),
                    });
                    if (registerUserResult.Success)
                    {
                        var userAuthenticate = AuthenticateUser(model.PhoneCode.Split("~")[0], model.PhoneNumber).Result;
                        return new LoginWithOTPResponseModel
                        {
                            Success = userAuthenticate.Success,
                            Session = await _sessionProvider.Get()
                        };
                    }
                    else if (Convert.ToBoolean(registerUserResult.EmailAlreadyRegistered))
                    {
                        return new LoginWithOTPResponseModel
                        {
                            Success = false,
                            EmailAlreadyRegistered = true
                        };
                    }
                    else
                    {
                        return new LoginWithOTPResponseModel
                        {
                            Success = false,
                        };
                    }
                }
            }
            else
            {
                return new LoginWithOTPResponseModel { Success = false };
            }

        }
        public async Task<LoginWithOTPResponseModel> AuthenticateUser(string phoneCode, string phoneNumber)
        {
            var UserData = new LoginWithOTPFormModel
            {
                PhoneCode = phoneCode,
                PhoneNumber = phoneNumber,
                SignUpMethodId = Contracts.Enums.SignUpMethods.OTP,
                ChannelId = Contracts.Enums.Channels.Feel
            };

            var authenticatedResponse = await _authenticationHelper.AuthenticateUserWithOTP(UserData, u =>
            {
                return Task.FromResult(true);
            });
            return authenticatedResponse;
        }
    }
}
