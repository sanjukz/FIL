using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class CitySightSeeingLocation : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingTicket : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string VenueName { get; set; }
        public string Language { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Currency { get; set; }
        public int CitySightSeeingLocationId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingCompanyOpeningTime : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string TicketId { get; set; }
        public string Day { get; set; }
        public string StartFrom { get; set; }
        public string EndTo { get; set; }
        public int CitySightSeeingTicketId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingTicketTypeDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string TicketId { get; set; }
        public string TicketType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public string UnitPrice { get; set; }
        public string UnitListPrice { get; set; }
        public string UnitDiscount { get; set; }
        public string UnitGrossPrice { get; set; }
        public int CitySightSeeingTicketId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingTicketDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string TicketId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string ProductLanguage { get; set; }
        public string TxtLanguage { get; set; }
        public string TicketEntryNotes { get; set; }
        public string BookSizeMin { get; set; }
        public string BookSizeMax { get; set; }
        public string SupplierUrl { get; set; }
        public int TicketClass { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BookingStartDate { get; set; }
        public string Currency { get; set; }
        public string PickupPoints { get; set; }
        public string CombiTicket { get; set; }
        public int CitySightSeeingTicketId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingTicketDetailImage : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string TicketId { get; set; }
        public string Image { get; set; }
        public int CitySightSeeingTicketId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingExtraOption : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string ExtraOptionId { get; set; }
        public string ExtraOptionName { get; set; }
        public string ExtraOptionType { get; set; }
        public int IsMandatory { get; set; }
        public int CitySightSeeingTicketTypeDetailId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingExtraSubOption : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string SubOptionId { get; set; }
        public string SubOptionName { get; set; }
        public string SubOptionPrice { get; set; }
        public int CitySightSeeingExtraOptionId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingEventDetailMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int CitySightSeeingTicketId { get; set; }
        public long EventDetailId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class CitySightSeeingEventTicketDetailMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int CitySightSeeingTicketTypeDetailId { get; set; }
        public long EventTicketDetailId { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}