using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Messaging.Models.TextMessages;
using FIL.Messaging.Senders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Web.Core.Providers
{
    public interface IOTPProvider
    {
        Task<long> SendOTP(string PhoneCode, string PhoneNumber);
        string GenerateToken(string mobileNumber, long otp);
        Task<bool> ValidateOTP(string mobileNumber, string token, long? otp);
    }

    public class OTPProvider : IOTPProvider
    {
        private readonly ISettings _settings;
        private readonly ITwilioTextMessageSender _twilioTextMessageSender;
        private readonly IGupShupTextMessageSender _gupShupTextMessageSender;
        private readonly FIL.Logging.ILogger _logger;

        public OTPProvider(
             ITwilioTextMessageSender twilioTextMessageSender,
             FIL.Logging.ILogger logger,
            IGupShupTextMessageSender gupShupTextMessageSender,
            ISettings settings)
        {
            _twilioTextMessageSender = twilioTextMessageSender;
            _gupShupTextMessageSender = gupShupTextMessageSender;
            _logger = logger;
            _settings = settings;
        }

        public async Task<long> SendOTP(string PhoneCode, string PhoneNumber)
        {
            try
            {
                long otp = new Random().Next(100000, 999999);
                TextMessage message = new TextMessage();
                message.To = "+" + PhoneCode + "" + PhoneNumber;
                message.From = "FeelitLIVE";
                message.Body = "Your FeelitLIVE verification code is " + otp + " and is valid for 5 minutes.";

                await _twilioTextMessageSender.Send(message);

                return otp;
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return 0;
            }
        }
        public string GenerateToken(string mobileNumber, long otp)
        {
            try
            {
                var sec = _settings.GetConfigSetting<string>(SettingKeys.Security.SecretKeyOne);
                var sec1 = _settings.GetConfigSetting<string>(SettingKeys.Security.SecretKeyTwo);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sec));
                var securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));

                var handler = new JwtSecurityTokenHandler();
                var signingCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha512);

                List<Claim> claims = new List<Claim>()
                {
                 new Claim("MobileNumber", mobileNumber),
                 new Claim("OTP", otp.ToString())
                };

                var ep = new EncryptingCredentials(
                    securityKey1,
                    SecurityAlgorithms.Aes128KW,
                    SecurityAlgorithms.Aes128CbcHmacSha256);

                handler = new JwtSecurityTokenHandler();

                var jwtSecurityToken = handler.CreateJwtSecurityToken(
                    "issuer",
                    "Audience",
                    new ClaimsIdentity(claims),
                    DateTime.Now,
                    DateTime.Now.AddMinutes(5),
                    DateTime.Now,
                    signingCredentials,
                    ep);


                var tokenString = handler.WriteToken(jwtSecurityToken);
                return tokenString;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        public async Task<bool> ValidateOTP(string mobileNumber, string token, long? otp)
        {
            try
            {
                var sec = _settings.GetConfigSetting<string>(SettingKeys.Security.SecretKeyOne);
                var sec1 = _settings.GetConfigSetting<string>(SettingKeys.Security.SecretKeyTwo);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sec));
                var securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));
                var handler = new JwtSecurityTokenHandler();

                var tokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudiences = new string[]
                    {
                     "Audience"
                    },
                    ValidIssuers = new string[]
                    {
                        "issuer"
                    },
                    IssuerSigningKey = securityKey,
                    // This is the decryption key
                    TokenDecryptionKey = securityKey1
                };

                SecurityToken validatedToken;

                var claimsPrincipal = handler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                var responseList = claimsPrincipal.Claims.ToList();
                var responseOtp = responseList[1].Value;
                var responseMobileNumber = responseList[0].Value;
                if (responseMobileNumber == mobileNumber && responseOtp == otp.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
