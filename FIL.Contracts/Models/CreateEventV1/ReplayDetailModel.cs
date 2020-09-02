using System;

namespace FIL.Contracts.Models.CreateEventV1
{
    public class ReplayDetailModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPaid { get; set; }
        public bool IsEnabled { get; set; }
        public decimal? Price { get; set; }
        public int? CurrencyId { get; set; }
    }
}