namespace FIL.Contracts.Models.Tiqets
{
    public class GetTicketResponseModel
    {
        public bool success { get; set; }
        public string order_reference_id { get; set; }
        public string order_status { get; set; }
        public string how_to_use_info { get; set; }
        public string post_purchase_info { get; set; }
        public string tickets_pdf_url { get; set; }
    }
}