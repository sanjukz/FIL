using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FIL.Foundation.Senders;
using FIL.Contracts.Commands.FeelNewsLetterSignUp;
using System.Threading.Tasks;
using FIL.Contracts.Queries.FeelNewsLetterSignUp;
using FIL.Web.feel.ViewModels.Footer;
using FIL.Logging;

namespace FIL.Web.Feel.Controllers
{
    public class FooterController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ILogger _logger;

        public FooterController(ICommandSender commandSender, IQuerySender querySender, ILogger logger)
        {
            _commandSender = commandSender;
            _querySender = querySender;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/newsletter/register")]
        public async Task<NewsLetterSignUpResponseDataViewModel> NewsLetterRegister([FromBody]NewsLetterSignUpDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var queryResult = await _querySender.Send(new FeelNewsLetterSignUpSearchQuery
                    {
                        Email = model.Email,
                        IsFeel = true,
                    });
                    if (queryResult.IsExisting)
                    {
                        return new NewsLetterSignUpResponseDataViewModel
                        {
                            Success = false,
                            IsExisting = true,
                        };
                    }
                    else
                    {
                        await _commandSender.Send(new FeelNewsLetterSignUpUserCommand
                        {
                            Email = model.Email,
                            IsEnabled = true,
                            IsFeel=true,
                        });
                        return new NewsLetterSignUpResponseDataViewModel
                        {
                            Success = true,
                            IsExisting = false,
                        };
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    new NewsLetterSignUpResponseDataViewModel
                    {
                        Success = false,
                        IsExisting = false,
                    };
                }
            }
            return new NewsLetterSignUpResponseDataViewModel
            {
                Success = false,
                IsExisting = false,
            };
        }
    }
}
