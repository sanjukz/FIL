namespace FIL.Contracts.Models.TournamentLayouts
{
    public class EventFeeTypeMappings
    {
        public long id { get; set; }
        public int feeType { get; set; }
        public int valueType { get; set; }
        public decimal Value { get; set; }
        public int channel { get; set; }
    }
}