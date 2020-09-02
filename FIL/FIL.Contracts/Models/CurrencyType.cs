namespace FIL.Contracts.Models
{
    public class CurrencyType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? Taxes { get; set; }
    }
}