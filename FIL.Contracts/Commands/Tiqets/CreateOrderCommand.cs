using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Tiqets
{
    public class CreateOrderCommand : ICommandWithResult<CreateOrderCommandResult>
    {
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
        public Guid UserAltId { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CreateOrderCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
        public string OrderRefernceId { get; set; }
        public string PaymentToken { get; set; }
    }
}