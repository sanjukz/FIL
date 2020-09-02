using FIL.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIL.Contracts.DataModels
{
    public class ValueRetailVillage : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public string VillageCode { get; set; }
        public string CultureCode { get; set; }
        public string CurrencyCode { get; set; }
        public string VillageName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class JourneyRoute
    {
        public string VillageCode { get; set; }
        public string CultureCode { get; set; }
        public int JourneyId { get; set; }
        public string Name { get; set; }
        public List<int> RouteIds { get; set; }
    }

    public class ValueRetailBookingDetail : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public Guid AltId { get; set; }
        public int JobId { get; set; }
        public string Email { get; set; }
        public DateTime? Date { get; set; }
        public string VillageCode { get; set; }
        public string CultureCode { get; set; }
        public decimal Pricing { get; set; }
        public string ValueRetailAltId { get; set; }
        public string ReferenceURL { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ValueRetailRoute : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int VillageId { get; set; }
        public int JourneyType { get; set; }
        public int RouteId { get; set; }
        public string DepartureTime { get; set; }
        public int LinkedRouteId { get; set; }
        public string ReturnTime { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public int StopId { get; set; }
        public int StopOrder { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildrenPrice { get; set; }
        public decimal FamilyPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ValueRetailReturnStop : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int ValueRetailRouteId { get; set; }
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public int StopOrder { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string ReturnTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class GiftCardEventMapping : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public long EventId { get; set; }
        public int GiftCardId { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ValueRetailPackageRoute : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int VillageId { get; set; }
        public int PackageId { get; set; }
        public int JourneyType { get; set; }
        public int RouteId { get; set; }
        public string DepartureTime { get; set; }
        public int LinkedRouteId { get; set; }
        public string ReturnTime { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public int StopId { get; set; }
        public int StopOrder { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal AdultPrice { get; set; }
        public decimal ChildrenPrice { get; set; }
        public decimal FamilyPrice { get; set; }
        public decimal InfantPrice { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }

    public class ValueRetailPackageReturn : IId<int>, IAuditable, IAuditableDates
    {
        public int Id { get; set; }
        public int ValueRetailPackageRouteId { get; set; }
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public int StopOrder { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string ReturnTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        [NotMapped]
        public Guid ModifiedBy { get; set; }
    }
}