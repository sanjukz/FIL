using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels.POne
{
    public class POneEvent : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public int POneId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TermsAndConditions { get; set; }
        public int POneEventCategoryId { get; set; }
        public int POneEventSubCategoryId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneEventCategory : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneEventSubCategory : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int POneEventCategoryId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneEventDetail : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public int POneId { get; set; }
        public string Name { get; set; }
        public int POneEventId { get; set; }
        public int POneVenueId { get; set; }
        public DateTime StartDateTime { get; set; }
        public string MetaDetails { get; set; }
        public string Description { get; set; }
        public DeliveryTypes DeliveryTypeId { get; set; }
        public string DeliveryNotes { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneVenue : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneTicketCategory : IId<int>, IAuditableDates
    {
        public int Id { get; set; }         // will map to seating_id of the api data
        public int POneId { get; set; }         // will map to seating_id of the api data
        public string Name { get; set; }    // will map to the seating field of the api data
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneEventTicketDetail : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public int POneTicketCategoryId { get; set; }
        public int POneEventDetailId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneEventTicketAttribute : IId<long>, IAuditableDates
    {
        public long Id { get; set; }                 // map to the SKU for
        public long POneId { get; set; }                 // map to the SKU for
        public int POneEventTicketDetailId { get; set; }
        public int AvailableTicketForSale { get; set; }
        public string TicketCategoryDescription { get; set; }
        public bool IsDateConfirmed { get; set; }
        public decimal Price { get; set; }
        public decimal ShippingCharge { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneEventDetailMapping : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public int POneEventDetailId { get; set; }
        public long ZoongaEventDetailId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneTransactionDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long TransactionId { get; set; }
        public long POneOrderId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class POneEventTicketAttributeMapping : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public long POneEventTicketAttributeId { get; set; }
        public long ZoongaEventTicketAttributeId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }

    public class POneEventTicketDetailMapping : IId<int>, IAuditableDates
    {
        public int Id { get; set; }
        public long EventTicketDetailId { get; set; }
        public int POneEventTicketDetailId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}