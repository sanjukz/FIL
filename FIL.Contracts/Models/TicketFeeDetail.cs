namespace FIL.Contracts.Models
{
    public class TicketFeeDetail
    {
        public int Id { get; set; }
        public long EventTicketAttributeId { get; set; }
        public int FeeId { get; set; }
        public string DisplayName { get; set; }
        public int ValueTypeId { get; set; }
        public decimal Value { get; set; }
        public short? FeeGroupId { get; set; }
        public bool IsEnabled { get; set; }
    }
}