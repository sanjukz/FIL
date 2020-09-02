namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IHdfcEnrolledCharge : ICharge
    {
        string Result { get; set; }
        string Error { get; set; }
        string Eci { get; set; }
        string AcsUrl { get; set; }
        string PaymentAuthenticationRequest { get; set; }
        string PaymentId { get; set; }
    }

    public class HdfcEnrolledCharge : Charge, IHdfcEnrolledCharge
    {
        public string Result { get; set; }
        public string Error { get; set; }
        public string Eci { get; set; }
        public string AcsUrl { get; set; }
        public string PaymentAuthenticationRequest { get; set; }
        public string PaymentId { get; set; }
    }
}