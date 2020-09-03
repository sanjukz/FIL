using System;
using FIL.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.TicketLookup
{
    public class TicketLookupResponseDataViewModel
    {
        public Transaction Transaction { get; set; }
        public string PaymentOption  { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public List<TicketLookupSubContainer> TicketLookupSubContainer { get; set; }
        public string PayconfigNumber { get; set; }
    }
}
