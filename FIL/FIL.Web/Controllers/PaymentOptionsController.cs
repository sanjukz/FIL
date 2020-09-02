using System.Threading.Tasks;
using FIL.Contracts.Queries.Payment;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels.Payment;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Feel.Controllers
{
    public class PaymentOptionsController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IQuerySender _querySender;

        public PaymentOptionsController(ICommandSender commandSender, IQuerySender querySender)
        {
            _commandSender = commandSender;
            _querySender = querySender;
        }

        [HttpGet]
        [Route("api/paymentOptions")]
        public async Task<PaymentOptionsResponseViewModel> GetAll()
        {
            var queryResult = await _querySender.Send(new PaymentOptionsQuery());
            return new PaymentOptionsResponseViewModel
            {
                BankDetails = queryResult.BankDetails,
                CashCardDetails = queryResult.CashCardDetails
            };
        }
    }
}