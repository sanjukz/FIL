using FIL.Contracts.Commands.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.DeliveryOptions
{
    public class DeliveryOptionsFormDataViewModel
    {
        public long TransactionId { get; set; }
        public List<DeliveryDetail> DeliveryDetail { get; set; }
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
    }
}
