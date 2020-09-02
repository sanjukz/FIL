using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Payment
{
    public class StripeConnectAccountCommand : ICommandWithResult<StripeConnectAccountCommandResult>
    {
        public string AuthorizationCode { get; set; }
        public bool IsStripeConnect { get; set; }
        public Channels channels { get; set; }
        public Guid EventId { get; set; }
        public Decimal ExtraCommisionFlat { get; set; }
        public Decimal ExtraCommisionPercentage { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class StripeConnectAccountCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public FIL.Contracts.DataModels.Event Event { get; set; }
        public FIL.Contracts.DataModels.EventDetail EventDetail { get; set; }
        public FIL.Contracts.DataModels.CurrencyType CurrencyType { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategory> TicketCategories { get; set; }
        public List<FIL.Contracts.DataModels.EventTicketDetail> EventTicketDetail { get; set; }
        public List<FIL.Contracts.DataModels.EventTicketAttribute> EventTicketAttribute { get; set; }
        public FIL.Contracts.DataModels.DayTimeMappings DayTimeMappings { get; set; }
        public int ParentCategoryId { get; set; }
        public string Email { get; set; }
    }
}