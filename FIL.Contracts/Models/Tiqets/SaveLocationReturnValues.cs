namespace FIL.Contracts.Models.Tiqets
{
    public class SaveLocationReturnValues
    {
        public int cityId { get; set; }
        public long countryId { get; set; }
        public int venueId { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
    }
}