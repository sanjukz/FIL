using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Payment
{
    public class PromoCodeResponceViewModel
    {
        public bool Success { get; set; }
        public bool IsLimitExceeds { get; set; }
        public long TransactionId { get; set; }
        public int CurrencyId { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
        public bool IsPaymentBypass { get; set; }

    }
}
