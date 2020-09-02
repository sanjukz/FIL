using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.Commands.Users;
using FIL.Contracts.Queries.UserProfile;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Kitms.Feel.ViewModels.Account;
using FIL.Web.Kitms.Feel.ViewModels.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using FIL.Web.Core;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;
        private IHostingEnvironment _env;
        private readonly IAmazonS3FileProvider _amazonS3FileProvider;

        public AccountController(ICommandSender commandSender, IPasswordHasher<string> passwordHasher, IQuerySender querySender, ISessionProvider sessionProvider, IHostingEnvironment env, IAmazonS3FileProvider amazonS3FileProvider)
        {
            _commandSender = commandSender;
            _passwordHasher = passwordHasher;
            _querySender = querySender;
            _sessionProvider = sessionProvider;
            _env = env;
            _amazonS3FileProvider = amazonS3FileProvider;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/account/register")]
        public IActionResult Register([FromBody]RegistrationFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new { Succeeded = true };
                var passwordHasher = new PasswordHasher<string>();
                _commandSender.Send(new RegisterUserCommand
                {
                    Email = model.Email,
                    PasswordHash = passwordHasher.HashPassword(model.Email, model.Password),
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                });
                if (result.Succeeded)
                {
                    return Ok(new RegistrationResponseViewModel());
                }
                else
                {
                    var response = new RegistrationResponseViewModel
                    {
                        //IsLockedOut = result.IsLockedOut,
                        //IsNotAllowed = result.IsNotAllowed,
                        //RequiresTwoFactor = result.RequiresTwoFactor,

                    };
                    return BadRequest(response);
                }
            }
            else
            {
                return BadRequest("Error");
            }
        }


        [HttpPost]
        [Route("api/account/changepassword")]
        public async Task<ChangePasswordResponseViewModel> ChangePasswordAsync([FromBody]ChangePasswordFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var session = await _sessionProvider.Get();
                var result = new { Success = true };
                var passwordHasher = new PasswordHasher<string>();
                try
                {
                    var user = await _querySender.Send(new UserProfileQuery
                    {
                        AltId = session.User.AltId,
                    });
                    if (user.Profile != null)
                    {
                        if (_passwordHasher.VerifyHashedPassword(user.Profile.Email, user.Profile.Password, model.OldPassword) ==
                        PasswordVerificationResult.Success)
                        {
                            await _commandSender.Send(new ChangePasswordCommand
                            {
                                AltId = session.User.AltId,
                                PasswordHash = passwordHasher.HashPassword(user.Profile.Email, model.NewPassword),
                                ModifiedBy = session.User.AltId,
                            });
                            return new ChangePasswordResponseViewModel
                            {
                                Success = result.Success,
                            };
                        }
                        else
                        {
                            return new ChangePasswordResponseViewModel
                            {
                                Success = false
                            };
                        }
                    }
                    else
                    {
                        return new ChangePasswordResponseViewModel
                        {
                            Success = result.Success,
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new ChangePasswordResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new ChangePasswordResponseViewModel
                {
                    Success = false
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
                return "https://s3-us-west-2.amazonaws.com/kz-cdn/Images/ProfilePictures/default.jpg";
            }
        }

        [HttpPost]
        [Route("api/upload/profilepicture")]
        public async Task<bool> UploadImage()
        {
            try
            {
                var session = await _sessionProvider.Get();
                var files = Request.Form.Files;
                bool updateSuccess = false;
                long size = files.Sum(f => f.Length);
                var filePath = Path.Combine(_env.WebRootPath.Replace("\\wwwroot", ""), "ClientApp\\Images\\ProfilePictures\\" + session.User.AltId + ".jpg");

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        try
                        {
                            _amazonS3FileProvider.UploadFeelImage(filePath, session.User.AltId.ToString());
                            updateSuccess = true;
                        }
                        catch
                        {
                            updateSuccess = false;
                        }
                        if (updateSuccess == true)
                        {
                            await ChangeProfilePicAsync(files[0].FileName);
                        }
                        System.IO.File.Delete(filePath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ChangeProfilePicResponseViewModel> ChangeProfilePicAsync(string imageName)
        {
            if (ModelState.IsValid)
            {
                var session = await _sessionProvider.Get();
                var result = new { Success = true };
                string profilePic = imageName.Split('.').ToString();
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