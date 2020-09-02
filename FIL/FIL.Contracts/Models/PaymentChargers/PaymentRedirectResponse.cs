using System;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IPaymentRedirectResponse
    {
        Uri Uri { get; set; }
        string Error { get; set; }
    }

    public class PaymentRedirectResponse : IPaymentRedirectResponse
    {
        public Uri Uri { get; set; }
        public string Error { get; set; }
    }
}