using System;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using FIL.Web.Feel.ViewModels.DeliveryOptions;
using FIL.Contracts.Queries.DeliveryOptions;
using Microsoft.AspNetCore.Mvc;
using FIL.Contracts.Commands.Transaction;
using FIL.Web.Feel.Modules.SiteExtensions;
using FIL.Logging;

namespace FIL.Web.Feel.Controllers
{
    public class DeliveryOptionsController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;
        private readonly ISessionProvider _sessionProvider;
        private readonly IGeoCurrency _geoCurrency;
        private readonly ILogger _logger;

        public DeliveryOptionsController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider, ILogger logger, IGeoCurrency geoCurrency)

        {
            _commandSender = commandSender;
            _sessionProvider = sessionProvider;
            _querySender = querySender;
            _geoCurrency = geoCurrency;
            _logger = logger;
        }


        [HttpGet]
        [Route("api/get/deliveryoptions/{eventDetailId}")]
        public async Task<DeliveryOptionsResponseViewModel> GetAllDeliveryptions(long eventDetailId)
        {
            var session = await _sessionProvider.Get();

            var queryResult = await _querySender.Send(new DeliveryOptionsQuery
            {
                EventDetailId = eventDetailId,
                UserId = session.User.AltId
            });

            if(queryResult.EventDeliveryTypeDetails != null && queryResult.UserDetails != null)
            {
                return new DeliveryOptionsResponseViewModel
                {
                    EventDeliveryTypeDetails = queryResult.EventDeliveryTypeDetails,
                    UserDetails = queryResult.UserDetails
                };
            }
            return new DeliveryOptionsResponseViewModel();
        }

        [HttpPost]
        [Route("api/deliveryoptions")]
        public async Task<UpdateTransactionResponseViewModel> UpdateTransaction([FromBody]DeliveryOptionsFormDataViewModel model)
        {
            string _TargetCurrencyCode = _geoCurrency.GetSessionCurrency(HttpContext);
            if (ModelState.IsValid)
            {
                var result = new { Succeeded = true };
                try
                {
                    UpdateTransactionCommandResult updateTransactionCommandResult = await _commandSender.Send<UpdateTransactionCommand, UpdateTransactionCommandResult>(new UpdateTransactionCommand
                    {
                        TransactionId = model.TransactionId,
                        DeliveryDetail = model.DeliveryDetail,
                        TargetCurrencyCode = _TargetCurrencyCode,
                        EventTicketAttributeList = model.EventTicketAttributeList
                    });

                    UpdateTransactionResponseViewModel _returnvalue = new UpdateTransactionResponseViewModel
                    {
                        Success = true,
                        TransactionId = updateTransactionCommandResult.Id,
                        CurrencyId = updateTransactionCommandResult.CurrencyId,
                        GrossTicketAmount = updateTransactionCommandResult.GrossTicketAmount,
                        DeliveryCharges = updateTransactionCommandResult.DeliveryCharges,
                        ConvenienceCharges = updateTransactionCommandResult.ConvenienceCharges,
                        ServiceCharge = updateTransactionCommandResult.ServiceCharge,
                        DiscountAmount = updateTransactionCommandResult.DiscountAmount,
                        NetTicketAmount = updateTransactionCommandResult.NetTicketAmount,
                    };

                    //perform check for currency change to geo
                    _geoCurrency.DeliveryOptionsUpdate(_returnvalue, HttpContext);

                    return _returnvalue;
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new UpdateTransactionResponseViewModel { Success = false };
                }
            }
            else
            {
                return new UpdateTransactionResponseViewModel { Success = false };
            }
        }
    }
}
