using System;
using FIL.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TransactionScheduleDetail : IId<long>, IAuditable
    {
        public long Id { get; set; }
        public long TransactionDetailId { get; set; }
        public long ScheduleDetailId { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}
