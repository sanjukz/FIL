using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models.Zoom
{
    public class LiveOnlineTransactionDetailResponseModel
    {
        public long EventId { get; set; }
        public long EventcategoryId { get; set; }
        public String SubCategoryDisplayName { get; set; }
        public String ParentCategoryName { get; set; }
        public int ParentCategoryId { get; set; }
        public long TicketCategoryId { get; set; }
        public FIL.Contracts.Enums.TransactionType TransactionType { get; set; }
        public string Name { get; set; }
        public Channels Channel { get; set; }
        public string Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime VisitDate { get; set; }
        public DateTime VisitEndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Guid UserTransactionAltId { get; set; }
        public Guid CreatorAltId { get; set; }
    }
}