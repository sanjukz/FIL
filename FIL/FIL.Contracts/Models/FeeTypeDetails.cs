using FIL.Contracts.Enums;

namespace FIL.Contracts.Models
{
    public class FeeTypeDetails
    {
        public Channels ChannelId { get; set; }
        public FeeType FeeId { get; set; }
        public FIL.Contracts.Enums.ValueTypes ValueTypeId { get; set; }
        public decimal Value { get; set; }
    }
}