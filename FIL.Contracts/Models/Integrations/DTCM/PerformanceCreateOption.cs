namespace FIL.Contracts.Models.Integrations.DTCM.PerformanceCreateOption
{
    public class PerformanceCreateOption : TokenResponse
    {
        public string PerformanceCode { get; set; }
        public string Channel { get; set; }
        public string SellerCode { get; set; }
        public string SectionId { get; set; }
    }
}