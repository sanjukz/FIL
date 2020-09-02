using FIL.Contracts.Queries.Transaction;
using FIL.Foundation.Senders;
using FIL.Web.Feel.ViewModels.Transaction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FIL.Web.Feel.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IQuerySender _querySender;
        public TransactionController(IQuerySender querySender)
        {
            _querySender = querySender;
        }

        [HttpGet]
        [Route("api/transaction/{transactionAltId}")]
        public async Task<TransactionResponseViewModel> Get(Guid transactionAltId)
        {

            var queryResult = await _querySender.Send(new TransactionInfoQuery
            {
                TransactionAltId = transactionAltId
            });

            return new TransactionResponseViewModel
            {
                Id = queryResult.Transaction.Id,
                ChannelId = queryResult.Transaction.ChannelId,
                ConvenienceCharges = queryResult.Transaction.ConvenienceCharges,
                Currency = queryResult.CurrencyName,
                DeliveryCharges = queryResult.Transaction.DeliveryCharges,
                DiscountAmount = queryResult.Transaction.DiscountAmount,
                GrossTicketAmount = queryResult.Transaction.GrossTicketAmount,
                NetTicketAmount = queryResult.Transaction.NetTicketAmount,
                DonationAmount = queryResult.Transaction.DonationAmount,
                ServiceCharge = queryResult.Transaction.ServiceCharge,
                TotalTickets = queryResult.Transaction.TotalTickets,
                TransactionStatusId = queryResult.Transaction.TransactionStatusId.ToString(),
                Events = queryResult.Events
            };
        }
    }
}
