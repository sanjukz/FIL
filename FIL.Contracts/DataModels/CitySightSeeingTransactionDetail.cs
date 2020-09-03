using FluentValidation;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CitySightSeeingTransactionDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long TransactionId { get; set; }
        public Guid? AltId { get; set; }
        public string TicketId { get; set; }
        public string BookingDistributorReference { get; set; }
        public string BookingReference { get; set; }
        public string ReservationReference { get; set; }
        public string ReservationDistributorReference { get; set; }
        public string ReservationValidUntil { get; set; }
        public bool HasTimeSlot { get; set; }
        public string FromDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string TimeSlot { get; set; }
        public bool? IsOrderConfirmed { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingTransactionDetaillValidator : AbstractValidator<CitySightSeeingTransactionDetail>, IFILValidator
    {
        public CitySightSeeingTransactionDetaillValidator()
        {
            RuleFor(s => s.TicketId).NotEmpty().WithMessage("Ticket Id is required");
            RuleFor(s => s.TransactionId).NotEmpty().WithMessage("Transaction is required");
        }
    }
}