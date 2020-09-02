using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.Transaction
{
    public class DeliveryDetail
    {
        public long EventTicketAttributeId { get; set; }
        public DeliveryTypes DeliveryTypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneCode { get; set; }
        public string PhoneNumber { get; set; }
        public string RepresentativeFirstName { get; set; }
        public string RepresentativeLastName { get; set; }
        public string RepresentativeEmail { get; set; }
        public string RepresentativePhoneCode { get; set; }
        public string RepresentativePhoneNumber { get; set; }
        public string CourierAddress { get; set; }
        public int CourierZipcode { get; set; }
    }

    public class UpdateTransactionCommand : Contracts.Interfaces.Commands.ICommandWithResult<UpdateTransactionCommandResult>
    {
        public long TransactionId { get; set; }
        public List<DeliveryDetail> DeliveryDetail { get; set; }
        public List<EventTicketAttribute> EventTicketAttributeList { get; set; }
        public string PickUpAddress { get; set; }
        public bool ISRasv { get; set; }
        public Guid ModifiedBy { get; set; }
        public string TargetCurrencyCode { get; set; }
    }

    public class UpdateTransactionCommandResult : Contracts.Interfaces.Commands.ICommandResult
    {
        public long Id { get; set; }
        public int CurrencyId { get; set; }
        public decimal? GrossTicketAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ConvenienceCharges { get; set; }
        public decimal? ServiceCharge { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? NetTicketAmount { get; set; }
    }
}