using FIL.Contracts.Commands.Account;
using FIL.Contracts.Commands.ResetPassword;
using FIL.Contracts.Commands.Users;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Account;
using FIL.Contracts.Queries.User;
using FIL.Contracts.Queries.UserProfile;
using FIL.Foundation.Senders;
using FIL.Logging;
using FIL.MailChimp.Models;
using FIL.Messaging.Senders;
using FIL.Web.Core;
using FIL.Web.Core.Providers;
using FIL.Web.Core.UrlsProvider;
using FIL.Web.Feel.ViewModels.Account;
using FIL.Web.Feel.ViewModels.Login;
using FIL.Web.Feel.ViewModels.ResetPassword;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IConfirmationEmailSender _confirmationEmailSender;
        private readonly IAccountEmailSender _accountEmailSender;
        private readonly IQuerySender _querySender;
        private readonly ISiteIdProvider _siteIdProvider;
        private readonly ISiteUrlsProvider _siteUrlsProvider;
        private readonly MailChimp.IMailChimpProvider _mailChimpProvider;
        private readonly ISessionProvider _sessionProvider;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;
        private readonly ILogger _logger;
        private readonly IClientIpProvider _clientIpProvider;
        private IHostingEnvironment _env;

        public AccountController(ICommandSender commandSender, IConfirmationEmailSender confirmationEmailSender, IAccountEmailSender accountEmailSender, IQuerySender querySender,
            ISiteIdProvider siteIdProvider, IClientIpProvider clientIpProvider,
            ISiteUrlsProvider siteUrlsProvider, ISessionProvider sessionProvider, IAmazonS3FileProvider amazonS3FileProvider, IHostingEnvironment env, ILogger logger, MailChimp.IMailChimpProvider mailChimpProvider)
        {
            _commandSender = commandSender;
            _confirmationEmailSender = confirmationEmailSender;
            _accountEmailSender = accountEmailSender;
            _querySender = querySender;
            _siteIdProvider = siteIdProvider;
            _siteUrlsProvider = siteUrlsProvider;
            _sessionProvider = sessionProvider;
            _amazonS3FileProvider = amazonS3FileProvider;
            _env = env;
            _clientIpProvider = clientIpProvider;
            _logger = logger;
            _mailChimpProvider = mailChimpProvider;
        }


        public string GetHostName()
        {
            var hostName = HttpContext.Request.Host.ToString();
            if (hostName.Contains("dev."))
            {
                hostName = "dev.feelitlive.com";
            }
            else
            {
                hostName = "www.feelitlive.com";
            }
            return hostName;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/register")]
        public async Task<RegistrationResponseViewModel> Register([FromBody] RegistrationFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hostName = GetHostName();
                var passwordHasher = new PasswordHasher<string>();
                try
                {
                    var queryResult = await _querySender.Send(new UserSearchQuery
                    {
                        Email = model.Email,
                        ChannelId = Channels.Feel,
                        SignUpMethodId = SignUpMethods.Regular,
                        PhoneCode = model.PhoneCode
                    });
                    if (queryResult.Success)
                    {
                        return new RegistrationResponseViewModel
                        {
                            Success = false,
                            IsExisting = true,
                        };
                    }
                    else
                    {
                        await _commandSender.Send(new RegisterUserCommand
                        {
                            Email = model.Email,
                            PasswordHash = passwordHasher.HashPassword(model.Email, model.Password),
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            ChannelId = Channels.Feel,
                            SignUpMethodId = SignUpMethods.Regular,
                            PhoneCode = model.PhoneCode,
                            PhoneNumber = model.PhoneNumber,
                            Ip = _clientIpProvider.Get(),
                            ReferralId = model.ReferralId,
                            IsMailOpt = model.IsMailOpt
                        });
                        var query = await _querySender.Send(new UserSearchQuery
                        {
                            Email = model.Email,
                            ChannelId = Channels.Feel,
                            SignUpMethodId = SignUpMethods.Regular,
                            PhoneCode = model.PhoneCode
                        });

                        Messaging.Models.Emails.Email email = new Messaging.Models.Emails.Email();
                        email.To = model.Email;
                        email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>"; // XXX: TODO: Add feel email template
                        email.TemplateName = "feelUserSignUp";
                        email.ConfigurationSetName = "FIL-Signup";
                        email.Variables = new Dictionary<string, object>
                        {
                            ["activationurl"] = "<a href='https://" + hostName + "/activate/" + query.User.AltId.ToString() + "' style='margin -right:100px;'><img src='https://static1.feelitlive.com/images/feel-mail/activate-account.png' width='215' height='36' style='border:0' alt='Activate Your Account' /></a>",
                            ["sitename"] = "feelitLIVE"
                        };
                        await _confirmationEmailSender.Send(email);

                        // adding user to MailChimp contacts
                        try
                        {
                            await _mailChimpProvider.AddFILMember(new MCUserModel
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                PhoneCode = model.PhoneCode,
                                PhoneNumber = model.PhoneNumber,
                                IsCreator = false,
                                SignUpType = "Regular"
                            }, query.Country);
                        }
                        catch (Exception e)
                        {
                            _logger.Log(Logging.Enums.LogCategory.Error, e);
                        }

                        return new RegistrationResponseViewModel { Success = true, IsExisting = false, };
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new RegistrationResponseViewModel
                    {
                        Success = false,
                        IsExisting = false,
                    };
                }
            }
            else
            {
                return new RegistrationResponseViewModel
                {
                    Success = false,
                    IsExisting = false,
                };
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/account/activate/{altid}")]
        public async Task<ActivationResponseViewModel> ActiavteUser(Guid AltId)
        {
            try
            {
                await _commandSender.Send(new ActivateUserCommand
                {
                    AltId = AltId
                });

                return new ActivationResponseViewModel { Success = true };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new ActivationResponseViewModel
                {
                    Success = false,
                };
            }

        }

        [HttpPost]
        [Route("api/account/forgotpassword")]
        public async Task<ForgotPasswordResponseViewModel> ForgotPassword([FromBody] ForgotPasswordFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var hostName = GetHostName();
                    var queryResult = await _querySender.Send(new UserSearchQuery
                    {
                        Email = model.Email,
                        ChannelId = Channels.Feel,
                        SignUpMethodId = SignUpMethods.Regular,
                    });
                    if (queryResult.Success)
                    {
                        Messaging.Models.Emails.Email email = new Messaging.Models.Emails.Email();
                        email.To = model.Email;
                        email.From = "FeelitLIVE  <no-reply@feelitLIVE.com>";
                        email.TemplateName = "FeelResetPassword";
                        email.Variables = new Dictionary<string, object>
                        {
                            ["passwordresetlink"] = "<a href='https://" + hostName + "/reset-password?" + queryResult.User.AltId.ToString() + "'><img src='https://static1.feelitlive.com/images/feel-mail/choose-password.png' width='231' height='36px' style='border:0' alt='Choose A New Password' /></a>",
                        };
                        await _accountEmailSender.Send(email);
                        return new ForgotPasswordResponseViewModel { Success = true, IsExisting = true, };
                    }
                    else
                    {
                        return new ForgotPasswordResponseViewModel
                        {
                            Success = true,
                            IsExisting = false,
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new ForgotPasswordResponseViewModel
                    {
                        Success = false,
                        IsExisting = false,
                    };
                }
            }
            else
            {
                return new ForgotPasswordResponseViewModel
                {
                    Success = false,
                    IsExisting = false,
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/resetpassword")]
        public async Task<ResetPasswordResponseViewModel> ResetPassword([FromBody] ResetPasswordFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var passwordHasher = new PasswordHasher<string>();
                try
                {
                    var user = await _querySender.Send(new UserProfileQuery
                    {
                        AltId = new Guid(model.AltId.ToString()),
                    });

                    //If only requested user details
                    if (Convert.ToBoolean(model.IsRequestedUserDetails))
                    {
                        return new ResetPasswordResponseViewModel { Success = true, User = user.Profile };
                    }
                    if (user.Profile != null)
                    {
                        await _commandSender.Send(new ResetPasswordCommand
                        {
                            AltId = user.Profile.AltId,
                            PasswordHash = passwordHasher.HashPassword(user.Profile.Email, model.Password),
                        });
                        return new ResetPasswordResponseViewModel { Success = true, };
                    }
                    else
                    {
                        return new ResetPasswordResponseViewModel { Success = false, };
                    }

                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new ResetPasswordResponseViewModel
                    {
                        Success = false,
                    };
                }
            }
            else
            {
                return new ResetPasswordResponseViewModel
                {
                    Success = false,
                };
            }
        }
        [HttpGet]
        [Route("/api/account/profilepicture")]
        public async Task<string> GetProfilePicture()
        {
            var session = await _sessionProvider.Get();
            try
            {
                var imagePath = _amazonS3FileProvider.GetImagePath(session.User.AltId.ToString());
                return imagePath;
            }
            catch
            {
                _logger.Log(Logging.Enums.LogCategory.Warn, $"account profile picture does not load for {session.User.AltId.ToString()}");
                return "https://s3-us-west-2.amazonaws.com/kz-cdn/Images/ProfilePictures/default.jpg";
            }
        }

        [HttpPost]
        [Route("api/upload/profilepicture")]
        public async Task<bool> UploadImage(IFormFile file)
        {
            try
            {
                var session = await _sessionProvider.Get();
                var files = Request.Form.Files;
                bool updateSuccess = false;
                long size = files.Sum(f => f.Length);
                //var filePath = Path.Combine(_env.WebRootPath.Replace("\\wwwroot", ""), "ClientApp\\Images\\ProfilePictures\\" + session.User.AltId + ".jpg");

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        try
                        {
                            Stream stream = file.OpenReadStream();
                            var img = Bitmap.FromStream(stream);
                            img = ResizeImage(img, 100, 100);
                            _amazonS3FileProvider.UploadImage(img, session.User.AltId.ToString());
                            updateSuccess = true;
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(Logging.Enums.LogCategory.Warn, ex);
                            updateSuccess = false;
                        }
                        if (updateSuccess == true)
                        {
                            await ChangeProfilePicAsync(session.User.AltId.ToString() + ".jpg");
                        }
                        //System.IO.File.Delete(filePath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Warn, ex);
                return false;
            }
        }
        [HttpPost]
        [Route("api/save/guest-user-details")]
        public async Task<GuestUserSaveDetailResponseModel> SaveGuestUserDetails([FromBody] GuestUserSaveDetailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var session = await _sessionProvider.Get();
                    await _commandSender.Send(new SaveGuestUserDetailCommand
                    {
                        UserId = session.User.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DocumentType = model.DocumentType,
                        DocumentNumber = model.DocumentNumber,
                        Nationality = model.Nationality,
                        ModifiedBy = session.User.AltId
                    });
                    return new GuestUserSaveDetailResponseModel
                    {
                        Success = true
                    };

                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Warn, ex);
                    return new GuestUserSaveDetailResponseModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new GuestUserSaveDetailResponseModel
                {
                    Success = false
                };
            }

        }
        [HttpPost]
        [Route("api/update/guest-user-details")]
        public async Task<GuestUserUpdateDetailResponseModel> UpdateGuestUserDetails([FromBody] GuestUserUpdateDetailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var session = await _sessionProvider.Get();
                    await _commandSender.Send(new UpdateGuestUserDetailCommand
                    {
                        UserId = model.UserAltId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        DocumentType = model.DocumentType,
                        DocumentNumber = model.DocumentNumber,
                        Nationality = model.Nationality,
                        ModifiedBy = session.User.AltId
                    });
                    return new GuestUserUpdateDetailResponseModel
                    {
                        Success = true
                    };
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Warn, ex);
                    return new GuestUserUpdateDetailResponseModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new GuestUserUpdateDetailResponseModel
                {
                    Success = false
                };
            }
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("api/delete/guest-user-details/{altid}")]
        public async Task<GuestUserUpdateDetailResponseModel> DeleteGuestUserDetails(Guid AltId)
        {
            try
            {
                await _commandSender.Send(new DeleteGuestUserDetailCommand
                {
                    UserId = AltId
                });

                return new GuestUserUpdateDetailResponseModel { Success = true };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Warn, ex);
                return new GuestUserUpdateDetailResponseModel
                {
                    Success = false,
                };
            }

        }
        [HttpGet]
        [AllowAnonymous]
        [Route("api/get/guest-user-details")]
        public async Task<GetAllGuestDetailResponseModel> GetAllUserDetails()
        {
            try
            {
                var session = await _sessionProvider.Get();
                if (session != null)
                {
                    var queryResult = await _querySender.Send(new GetAllGuestUserDetailQuery
                    {
                        UserId = session.User.Id
                    });
                    return new GetAllGuestDetailResponseModel
                    {
                        GuestUserDetails = queryResult.GuestUserDetails
                    };
                }
                else
                {
                    throw new ArgumentNullException(session.ToString(), "Session returned Null for guest login");
                }

            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Warn, ex);
                return new GetAllGuestDetailResponseModel
                {
                    GuestUserDetails = null
                };
            }

        }
        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public async Task<ChangeProfilePicResponseViewModel> ChangeProfilePicAsync(string imageName)
        {
            if (ModelState.IsValid)
            {
                var session = await _sessionProvider.Get();
                var result = new { Success = true };
                try
                {
                    await _commandSender.Send(new ChangeProfilePicCommand
                    {
                        AltId = session.User.AltId,
                        ProfilePic = true,
                        ModifiedBy = session.User.AltId
                    });
                    return new ChangeProfilePicResponseViewModel
                    {
                        Success = result.Success,
                    };

                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new ChangeProfilePicResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new ChangeProfilePicResponseViewModel
                {
                    Success = false
                };
            }
        }
    }
}

