using FIL.Contracts.Interfaces.Commands;
using System;

namespace FIL.Contracts.Commands.MatchLayout
{
    public class UpdateMatchLayoutCommand : ICommandWithResult<UpdateMatchLayoutCommandResult>
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int Capacity { get; set; }
        public int EntryGateId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public decimal LocalPrice { get; set; }
        public int LocalCurrencyId { get; set; }
        public bool IsSeatSelection { get; set; }
        public int EventDetailId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class UpdateMatchLayoutCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}