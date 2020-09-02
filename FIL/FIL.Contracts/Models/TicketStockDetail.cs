using System;

namespace FIL.Contracts.Models
{
    public class TicketStockDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string TicketStockStartSrNo { get; set; }
        public string TicketStockEndSrNo { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}