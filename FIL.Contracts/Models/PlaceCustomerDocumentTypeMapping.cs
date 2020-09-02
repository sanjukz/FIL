namespace FIL.Contracts.Models
{
    public class PlaceCustomerDocumentTypeMapping
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public long CustomerDocumentType { get; set; }
        public bool IsEnabled { get; set; }
    }
}