using FIL.Contracts.Models.ValueRetail;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ValueRetail
{
    public class Basket
    {
        public IList<object> Items { get; set; }
        public object OriginalItems { get; set; }
        public string Id { get; set; }
        public int JobId { get; set; }
        public object Booker { get; set; }
        public Village Village { get; set; }
        public object PaymentId { get; set; }
        public bool IsRetry { get; set; }
        public string Status { get; set; }
    }

    public class RequestStatus
    {
        public bool Success { get; set; }
        public IList<object> Errors { get; set; }
    }

    public class CreateShoppingCartResponse
    {
        public Basket Basket { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}