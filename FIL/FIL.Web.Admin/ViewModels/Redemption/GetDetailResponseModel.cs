using System;
using System.Collections.Generic;
using FIL.Contracts.Models;

namespace FIL.Web.Kitms.Feel.ViewModels.Redemption
{
    public class GetDetailResponseModel
    {
        public List<EventTicketAttribute> EventTicketAttribute { get; set; }
        public List<TransactionDetail> TransactionDetail { get; set; }
        public List<EventTicketDetail> EventTicketDetail { get; set; }
        public List<TicketCategory> TicketCategory { get; set; }
        public List<EventDetail> EventDetail { get; set; }
    }
}
