namespace FIL.Contracts.Models.TMS.Invoice
{
    public class InvoiceDetailModel
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string InvoicePrefix { get; set; }
        public string InvoiceDate { get; set; }
        public string SponsorName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EventDetailName { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string TicketCategoryName { get; set; }
        public short Quantity { get; set; }
        public decimal LocalPrice { get; set; }
        public string CurrencyName { get; set; }
        public decimal ConvenienceCharge { get; set; }
        public decimal ServiceCharge { get; set; }
    }
}