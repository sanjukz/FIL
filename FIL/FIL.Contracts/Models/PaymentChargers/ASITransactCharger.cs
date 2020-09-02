using FIL.Contracts.Models.ASI;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IASITransactCharge : ICharge
    {
        ASIResponseFormData asiFormData { get; set; }
        bool IsSuccess { get; set; }
        string RedirectUrl { get; set; }
    }

    public class ASITransactCharge : Charge, IASITransactCharge
    {
        public ASIResponseFormData asiFormData { get; set; }
        public bool IsSuccess { get; set; }
        public string RedirectUrl { get; set; }
    }
}