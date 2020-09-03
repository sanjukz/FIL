using FluentValidation;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class TransactionDeliveryDetail : IId<long>, IAuditable, IAuditableDates
    {
        public long Id { get; set; }
        public long TransactionDetailId { get; set; }
        public DeliveryTypes DeliveryTypeId { get; set; }
        public short? PickupBy { get; set; }
        public string SecondaryName { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryEmail { get; set; }
        public string CourierAddress { get; set; }
        public int? CourierZipcode { get; set; }
        public int? PickupOTP { get; set; }
        public bool? DeliveryStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliverdTo { get; set; }
        public string TicketNumber { get; set; }
        public string PickUpAddress { get; set; }
        public CourierService? CourierServiceId { get; set; }
        public string TrackingId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class TransactionDeliveryDetailValidator : AbstractValidator<TransactionDeliveryDetail>, IFILValidator
    {
        public TransactionDeliveryDetailValidator()
        {
            RuleFor(s => s.ModifiedBy).NotEmpty().WithMessage("ModifiedBy is required");
        }
    }
}