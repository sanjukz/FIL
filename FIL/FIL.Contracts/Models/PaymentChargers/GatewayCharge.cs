namespace FIL.Contracts.Models.PaymentChargers
{
    public interface IGatewayCharge : ICharge
    {
        string QueryString { get; set; }
        string Response { get; set; }
    }

    public class GatewayCharge : Charge, IGatewayCharge
    {
        public string QueryString { get; set; }
        public string Response { get; set; }
    }
}