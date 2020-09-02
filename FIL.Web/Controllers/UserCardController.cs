using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIL.Foundation.Senders;
using FIL.Contracts.Commands.Users;
using Microsoft.AspNetCore.Identity;
using System;
using FIL.Web.Feel.ViewModels.Login;
using FIL.Web.Feel.ViewModels.Account;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Account;
using FIL.Web.Core.Providers;
using FIL.Contracts.Queries.UserCard;

namespace FIL.Web.Controllers
{
    public class UserCardController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;

        public UserCardController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _sessionProvider = sessionProvider;
        }

        [HttpPost]
        [Route("api/usercard")]
        public async Task<SaveCardResponseViewModel> SaveAsync([FromBody]SaveCardFormDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var session = await _sessionProvider.Get();
                var result = new { Success = true };
                try
                {
                    await _commandSender.Send(new SaveCardCommand
                    {
                        UserAltId = session.User.AltId,
                        NameOnCard = model.NameOnCard,
                        CardNumber = model.CardNumber,
                        ExpiryMonth = model.ExpiryMonth ?? 0,
                        ExpiryYear = model.ExpiryYear ?? 0,
                        CardTypeId = model.CardTypeId ?? 0,
                    });
                    return new SaveCardResponseViewModel
                    {
                        Success = result.Success,
                    };
                }
                catch (Exception ex)
                {
                    return new SaveCardResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new SaveCardResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        [Route("api/usercard/delete")]
        public async Task<DeleteCardResponseViewModel> DeleteAsync([FromBody]DeleteCardDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new { Success = true };
                try
                {
                    await _commandSender.Send(new DeleteCardCommand
                    {
                        AltId = model.AltId,
                    });
                    return new DeleteCardResponseViewModel
                    {
                        Success = result.Success,
                    };
                }
                catch (Exception ex)
                {
                    return new DeleteCardResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            else
            {
                return new DeleteCardResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        [Route("api/usercard/all")]
        public async Task<CardResponseViewModel> GetCardList([FromBody]GetCardListDataViewModel model)
        {
            var session = await _sessionProvider.Get();
            if (session.User != null)
            {
                var queryResult = await _querySender.Send(new UserCardQuery
                {
                    UserAltId = session.User.AltId,
                });

                return queryResult.UserCards != null ? new CardResponseViewModel { UserCards = queryResult.UserCards } : null;
            }
            else
            {
                return null;
            }
        }
    }
}
