using FIL.Contracts.Models.ASI;
using FIL.Contracts.Models.PaymentChargers;
using System;

namespace FIL.Contracts.Commands.ASI
{
    public class ASIPaymentCommand : Contracts.Interfaces.Commands.ICommandWithResult<ASIPaymentCommandResult>
    {
        public Guid ModifiedBy { get; set; }
        public bool IsSuccess { get; set; }
        public ASIResponseFormData aSIResponseFormData { get; set; }
    }

    public class ASIPaymentCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public IPaymentResponse PaymentResponse { get; set; }
        public Guid TransactionAltId { get; set; }
        public FIL.Contracts.DataModels.Transaction Transaction { get; set; }
        public long TransactionId { get; set; }
        public bool Success { get; set; }
    }
}