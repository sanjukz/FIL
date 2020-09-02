using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.ASI
{
    public class ASIUpdateTicketStatusCommand : Contracts.Interfaces.Commands.ICommandWithResult<ASIUpdateTicketStatusCommandResult>
    {
        public Guid ModifiedBy { get; set; }
        public List<FIL.Contracts.Models.ASI.ASIPaymentResponseDetailTicketMapping> ASIPaymentResponseDetailTicketMappings { get; set; }
    }

    public class ASIUpdateTicketStatusCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool Success { get; set; }
    }
}