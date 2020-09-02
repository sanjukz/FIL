using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Web.Core.ViewModels;
using Newtonsoft.Json;

namespace FIL.Web.Feel.ViewModels.Checkout {
    public class UserCheckoutResponseViewModel : IResponseViewModel {
        public bool Success { get; set; }
        public SessionViewModel Session { get; set; }

        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsLockedOut { get; set; } = false;
        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool RequiresTwoFactor { get; set; } = false;
        [JsonProperty (DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsNotAllowed { get; set; } = false;
        public long TransactionId { get; set; }
        public Guid TransactionAltId { get; set; }
        public bool? IsTiqetsOrderFailure { get; set; }
        public bool IsPaymentByPass { get; set; }
        public FIL.Contracts.Enums.StripeAccount StripeAccount { get; set; }
    }
}