using System;
using FIL.Contracts.Enums;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Admin.ViewModels.TicketLookup;
using FIL.Contracts.Queries.TicketLookup;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIL.Web.Admin.Controllers
{
    public class TicketLookupController
    {
        private readonly IQuerySender _querySender;
        public TicketLookupController(IQuerySender querySender)
        {
            _querySender = querySender;
        }
        [HttpGet]
        [Route("api/ticketlookup/{transactionId}")]
        public async Task<TicketLookupResponseDataViewModel> GetTicketLokup(long transactionId)
        {
             var queryResult = await _querySender.Send(new TicketLookupQuery
             {
                 TransactionId = transactionId
             });
             return new TicketLookupResponseDataViewModel
             {
                Transaction = queryResult.Transaction,
                PaymentOption = queryResult.PaymentOption,
                CurrencyType = queryResult.CurrencyType,
                TicketLookupSubContainer = queryResult.TicketLookupSubContainer,
                PayconfigNumber = queryResult.PayconfigNumber
             };
            
        }
        [HttpGet]
        [Route("api/ticketlookupemaildetail/{email}")]
        public async Task<TicketLookupUserEmailDetailResponseDataViewModel> GetTicketLokupEmailDetail(string email)
        {

            var queryResult = await _querySender.Send(new TicketLookupEmailDetailQuery
            {
                Email = email,
                Name = null,
                Phone = null
            });
            if (queryResult.TicketLookupEmailDetailContainer != null)
            {
                queryResult.TicketLookupEmailDetailContainer = queryResult.TicketLookupEmailDetailContainer.OrderBy(o => o.Transaction.FirstName).ToList();
            }
            return new TicketLookupUserEmailDetailResponseDataViewModel
            {
                TicketLookupEmailDetailContainer = queryResult.TicketLookupEmailDetailContainer
            };
        }

        [HttpGet]
        [Route("api/ticketlookupnamedetail/{name}")]
        public async Task<TicketLookupUserEmailDetailResponseDataViewModel> GetTicketLokupNameDetail(string name)
        {

            var queryResult = await _querySender.Send(new TicketLookupEmailDetailQuery
            {
                Name = name,
                Email = null,
                Phone = null
            });
            if (queryResult.TicketLookupEmailDetailContainer != null)
            {
                queryResult.TicketLookupEmailDetailContainer = queryResult.TicketLookupEmailDetailContainer.OrderBy(o => o.Transaction.FirstName).ToList();
            }
            return new TicketLookupUserEmailDetailResponseDataViewModel
            {
                TicketLookupEmailDetailContainer = queryResult.TicketLookupEmailDetailContainer
            };
        }

        [HttpGet]
        [Route("api/ticketlookupphonedetail/{phone}")]
        public async Task<TicketLookupUserEmailDetailResponseDataViewModel> GetTicketLokupPhoneNumberDetail(string phone)
        {
            var queryResult = await _querySender.Send(new TicketLookupEmailDetailQuery
            {
                Name = null,
                Email = null,
                Phone = phone
            });
            if (queryResult.TicketLookupEmailDetailContainer != null)
            {
                queryResult.TicketLookupEmailDetailContainer = queryResult.TicketLookupEmailDetailContainer.OrderBy(o => o.Transaction.FirstName).ToList();
            }
            return new TicketLookupUserEmailDetailResponseDataViewModel
            {
                TicketLookupEmailDetailContainer = queryResult.TicketLookupEmailDetailContainer
            };
        }

    }
}
