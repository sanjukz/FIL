using FIL.Contracts.Enums;
using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Reporting
{
    public class ReportResponseDataViewModel
    {
        public IEnumerable<FIL.Contracts.Models.Transaction> Transaction { get; set; }
        public IEnumerable<TransactionDetail> TransactionDetail { get; set; }
        public IEnumerable<TransactionDeliveryDetail> TransactionDeliveryDetail { get; set; }
        public IEnumerable<TransactionPaymentDetail> TransactionPaymentDetail { get; set; }
        public IEnumerable<CurrencyType> CurrencyType { get; set; }
        public IEnumerable<EventTicketAttribute> EventTicketAttribute { get; set; }
        public IEnumerable<EventTicketDetail> EventTicketDetail { get; set; }
        public IEnumerable<TicketCategory> TicketCategory { get; set; }
        public IEnumerable<FIL.Contracts.Models.Event> Event { get; set; }
        public IEnumerable<EventDetail> EventDetail { get; set; }
        public IEnumerable<EventAttribute> EventAttribute { get; set; }
        public IEnumerable<FIL.Contracts.Models.Venue> Venue { get; set; }
        public IEnumerable<FIL.Contracts.Models.City> City { get; set; }
        public IEnumerable<FIL.Contracts.Models.State> State { get; set; }
        public IEnumerable<FIL.Contracts.Models.Country> Country { get; set; }
        public IEnumerable<FIL.Contracts.Models.User> User { get; set; }
        public IEnumerable<FIL.Contracts.Models.UserCardDetail> UserCardDetail { get; set; }
        public IEnumerable<FIL.Contracts.Models.ReportingColumn> ReportColumns { get; set; }
        public IEnumerable<FIL.Contracts.Models.IPDetail> IPDetail { get; set; }
        public IEnumerable<TicketFeeDetail> TicketFeeDetail { get; set; }
        public  Pagination Pagination { get; set; }
    }
}
