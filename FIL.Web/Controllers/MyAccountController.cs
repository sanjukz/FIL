using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Feel.ViewModels.Account;
using FIL.Contracts.Commands.Account;
using FIL.Contracts.Queries.UserProfile;
using FIL.Contracts.Queries.User;
using FIL.Web.Feel.ViewModels.UserOrder;
using FIL.Contracts.Queries.UserOrders;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.MailChimp;
using FIL.Logging;

namespace FIL.Web.Feel.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;
        private readonly IGeoCurrency _geoCurrency;
        private readonly IMailChimpProvider _mailChimpProvider;
        private readonly ILogger _logger;

        public MyAccountController(ICommandSender commandSender, IPasswordHasher<string> passwordHasher, IQuerySender querySender, ISessionProvider sessionProvider, IGeoCurrency geoCurrency, IMailChimpProvider mailChimpProvider, ILogger logger)
        {
            _commandSender = commandSender;
            _passwordHasher = passwordHasher;
            _querySender = querySender;
            _sessionProvider = sessionProvider;
            _geoCurrency = geoCurrency;
            _mailChimpProvider = mailChimpProvider;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/account/personal-info")]
        public async Task<UpdateUserProfileViewModel> GetPersonalInfoDetails([FromBody] UpdateUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UpdateUserCommandResult result = await _commandSender.Send<UpdateUserCommand, UpdateUserCommandResult>(new UpdateUserCommand { UserProfile = model.UserProfile }, new TimeSpan(2, 0, 0));

                    // Update Mailchimp Contact
                    try
                    {
                        await _mailChimpProvider.AddFILMemberAdditionalDetails(new MailChimp.Models.MCUserAdditionalDetailModel
                        {
                            FirstName = result.UserProfile.FirstName,
                            LastName = result.UserProfile.LastName,
                            Gender = result.UserProfile.Gender,
                            DOB = result.UserProfile.DOB,
                            PhoneCode = result.UserProfile.PhoneCode,
                            PhoneNumber = result.UserProfile.PhoneNumber
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    }

                    return new UpdateUserProfileViewModel
                    {
                        UserProfile = result.UserProfile
                    };
                }
                catch (Exception e)
                {
                    return new UpdateUserProfileViewModel { };
                }
            }
            else
            {
                return new UpdateUserProfileViewModel { };
            }
        }

        [HttpPost]
        [Route("api/account/mobile/exists")]
        public async Task<MobileExistViewModel> CheckMobileExists([FromBody] MobileExistViewModel model)
        {
            var queryResult = await _querySender.Send(new GetUserWithMobileQuery
            {
                PhoneCode = model.PhoneCode.Split("~")[0],
                PhoneNumber = model.PhoneNumber,
                ChannelId = Contracts.Enums.Channels.Feel
            });
            return new MobileExistViewModel
            {
                IsExist = queryResult.IsExist
            };

        }
        [HttpPost]
        [Route("api/account/notification")]
        public async Task<UpdateNotificationViewModel> NotificationDetails([FromBody] UpdateNotificationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UpdateNotificationCommandResult result = await _commandSender.Send<UpdateNotificationCommand, UpdateNotificationCommandResult>(new UpdateNotificationCommand { UserAltId = model.UserAltId, IsMailOpt = model.IsOptedForMail, ShouldUpdate = model.ShouldUpdate }, new TimeSpan(2, 0, 0));
                    return new UpdateNotificationViewModel
                    {
                        IsOptedForMail = result.IsMailOpt,
                    };
                }
                catch (Exception e)
                {
                    return new UpdateNotificationViewModel { };
                }
            }
            else
            {
                return new UpdateNotificationViewModel { };
            }
        }

        [HttpPost]
        [Route("api/account/login-and-security")]
        public async Task<ChangePasswordResponseViewModel> ChangePasswordAsync([FromBody] ChangePasswordFormDataViewModel model)
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
                        if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
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
                                    Profile = user.Profile
                                };
                            }
                            else
                            {
                                return new ChangePasswordResponseViewModel
                                {
                                    Success = false,
                                    WrongPassword = true,
                                    Profile = user.Profile
                                };
                            }
                        }
                        else
                        {
                            return new ChangePasswordResponseViewModel
                            {
                                Profile = user.Profile
                            };
                        }
                    }
                    else
                    {
                        return new ChangePasswordResponseViewModel
                        {
                            Success = result.Success,
                            Profile = user.Profile
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
        [Route("api/userorders/{UserAltId}")]

        public async Task<UserOrderRespnseViewModel> Get(Guid userAltId)
        {
            var utcOffset = "";
            List<FIL.Contracts.Models.Transaction> transactions = new List<Contracts.Models.Transaction>();
            if (Request.Cookies["utcTimeOffset"] != null)
            {
                utcOffset = Request.Cookies["utcTimeOffset"];
            }
            var queryResult = await _querySender.Send(new UserOrdersQuery
            {
                UserAltId = userAltId
            });
            
            try
            {
                if (queryResult.Transaction.Any())
                {
                    _geoCurrency.UpdateTransactions(queryResult.Transaction.ToList(), HttpContext);
                }
            }
            catch (Exception)
            {
            }

            foreach (var currentTransaction in queryResult.Transaction)
            {
                var transaction = currentTransaction;
                if (utcOffset.Contains("+"))
                {
                    var hours = Convert.ToInt32(utcOffset.Split(":")[0].Split("+")[1]);
                    var mins = Convert.ToInt32(utcOffset.Split(":")[1]);
                    transaction.CreatedUtc = transaction.CreatedUtc.AddHours(hours).AddMinutes(mins);
                }
                else if (utcOffset.Contains("-"))
                {
                    var hours = Convert.ToInt32(utcOffset.Split(":")[0].Split("-")[1]);
                    var mins = Convert.ToInt32(utcOffset.Split(":")[1]);
                    transaction.CreatedUtc = transaction.CreatedUtc.AddHours(-hours).AddMinutes(-mins);
                }
                transactions.Add(transaction);
            }

            return new UserOrderRespnseViewModel
            {
                Event = queryResult.Event,
                Transaction = transactions,
                transactionDetail = queryResult.transactionDetail,
                EventTicketAttribute = queryResult.EventTicketAttribute,
                EventTicketDetail = queryResult.EventTicketDetail,
                TicketCategory = queryResult.TicketCategory,
                TransactionPaymentDetail = queryResult.TransactionPaymentDetail,
                CurrencyType = queryResult.CurrencyType,
                CurrentDateTime = DateTime.UtcNow,
                EventDetail = queryResult.EventDetail,
                EventCategories = queryResult.EventCategories,
                EventCategoryMappings = queryResult.EventCategoryMappings
            };
        }
    }
}