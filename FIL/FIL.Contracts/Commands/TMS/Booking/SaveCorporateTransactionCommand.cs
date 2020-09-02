using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.TMS;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.TMS.Booking
{
    public class CorporateTicketAllocationDetail
    {
        public long? Id { get; set; }
        public long? EventDetailId { get; set; }
        public long? TicketcategoryId { get; set; }
        public long sponsorId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public short TotalTickets { get; set; }
        public TicketType TicketTypeId { get; set; }
        public decimal discount { get; set; }
    }

    public class SaveCorporateTransactionCommand : ICommandWithResult<SaveCorporateTransactionCommandResult>
    {
        public Channels ChannelId { get; set; }
        public TransactingOption TransactingOptionId { get; set; }
        public List<CorporateTicketDetail> CorporateTicketDetails { get; set; }
        public List<SeatDetail> SeatDetails { get; set; }
        public string PaymentType { get; set; }
        public string Ip { get; set; }
        public Guid ModifiedBy { get; set; }
    }

    public class CorporateTicketDetail
    {
        public long SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public decimal PricePerTicket { get; set; }
        public int TotalTickets { get; set; }
        public long EventTicketAttributeId { get; set; }
        public decimal ConvenceCharge { get; set; }
        public decimal ServiceTax { get; set; }
        public decimal DiscountAmount { get; set; }
        public int CurrencyId { get; set; }
        public TicketType TicketTypeId { get; set; }
    }

    public class SaveCorporateTransactionCommandResult : ICommandResult
    {
        public long Id { get; set; }
        public short TotalTickets { get; set; }
        public TicketType TicketTypeId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}