using System;

namespace FIL.Contracts.Commands.Discount
{
    public class ApplyDiscountCommand : Contracts.Interfaces.Commands.ICommandWithResult<ApplyDiscountCommandResult>
    {
        public long TransactionId { get; set; }
        public string Promocode { get; set; }
        public FIL.Contracts.Enums.Channels Channel { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class ApplyDiscountCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public bool IsLimitExceeds { get; set; }
        public int CurrencyId { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
        public bool IsPaymentBypass { get; set; }
    }
}