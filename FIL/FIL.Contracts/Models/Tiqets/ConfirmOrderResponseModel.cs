namespace FIL.Contracts.Models.Tiqets
{
    public class ConfirmOrderResponseModel
    {
        public string order_reference_id { get; set; }
        public bool success { get; set; }
    }
}