using FIL.Contracts.Enums;

namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IHdfcEnrollmentResponse
    {
        IHdfcEnrolledCharge HdfcEnrolledCharge { get; set; }
        PaymentGatewayError PaymentGatewayError { get; set; }
    }

    public class HdfcEnrollmentResponse : IHdfcEnrollmentResponse
    {
        public IHdfcEnrolledCharge HdfcEnrolledCharge { get; set; }
        public PaymentGatewayError PaymentGatewayError { get; set; }
    }
}