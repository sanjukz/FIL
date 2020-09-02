namespace FIL.Contracts.Models.Tiqets
{
    public class CreateOrderResponseModel
    {
        public bool success { get; set; }
        public string payment_confirmation_token { get; set; }
        public string order_reference_id { get; set; }
    }
}