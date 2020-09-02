using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.BoxOffice
{
    public class EventTicketAttribute
    {
        public long Id { get; set; }
        public short TotalTickets { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public TicketType TicketTypeId { get; set; }
        public DeliveryTypes DeliveryTypes { get; set; }
        public decimal discountedPrice { get; set; }
        public bool IsSeasonPackage { get; set; }
        public string DiscountCode { get; set; }
        public long EventTicketDetailId { get; set; }
    }

    public class SaveTransactionCommand : ICommandWithResult<SaveTransactionCommandResult>
    {
        public Channels ChannelId { get; set; }
        public Guid UserAltId { get; set; }
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
        public List<PartPaymentFormModel> PartPaymentDataList { get; set; }
        public BoCustomerDetail boCustomerDetail { get; set; }
        public string Ip { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class SaveTransactionCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public short TotalTickets { get; set; }
        public TicketType TicketTypeId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class BoCustomerDetail
    {
        public string PaymentOption { get; set; }
        public string BankName { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime ChequeDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public bool IsEticketEnabled { get; set; }
        public decimal TransactionCharge { get; set; }
    }

    public class PartPaymentFormModel
    {
        public int CurrencyId { get; set; }
        public int paymentOptionId { get; set; }
        public long Value { get; set; }
    }

    public class SeatDetail
    {
        public long EventTicketDetailId { get; set; }
        public long MatchLayoutSectionSeatId { get; set; }
        public string SeatTag { get; set; }
        public short SeatTypeId { get; set; }
        public short TicketTypeId { get; set; }
    }
}