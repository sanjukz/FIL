﻿using FIL.Contracts.Models;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults.PlaceInventory
{
    public class GetPlaceInventoryQueryResult
    {
        public FIL.Contracts.Models.Event Event { get; set; }
        public List<TicketCategoryInfo> TicketCategoryContainer { get; set; }
        public List<FIL.Contracts.Models.EventDetail> EventDetails { get; set; }
        public FIL.Contracts.DataModels.EventAttribute EventAttribute { get; set; }
        public List<string> TicketValidityTypes { get; set; }
        public List<string> DeliveryTypes { get; set; }
        public List<FIL.Contracts.Models.CustomerDocumentType> CustomerDocumentTypes { get; set; }
        public List<FIL.Contracts.Models.EventTicketDetailTicketCategoryTypeMapping> EventTicketDetailTicketCategoryTypeMappings { get; set; }
        public List<FIL.Contracts.Models.PlaceCustomerDocumentTypeMapping> PlaceCustomerDocumentTypeMappings { get; set; }
        public List<FIL.Contracts.Models.PlaceTicketRedemptionDetail> PlaceTicketRedemptionDetails { get; set; }
        public List<FIL.Contracts.Models.EventDeliveryTypeDetail> eventDeliveryTypeDetails { get; set; }
        public List<FIL.Contracts.Models.PlaceHolidayDate> PlaceHolidayDates { get; set; }
        public List<FIL.Contracts.Models.PlaceWeekOff> PlaceWeekOffs { get; set; }
        public List<FIL.Contracts.Models.EventCustomerInformationMapping> EventCustomerInformationMappings { get; set; }
        public List<FIL.Contracts.Models.CustomerInformation> CustomerInformations { get; set; }
        public RegularViewModel RegularTimeModel;
        public List<SeasonViewModel> SeasonTimeModel;
        public List<SpecialDayViewModel> SpecialDayModel;
    }

    public class TicketCategoryInfo
    {
        public FIL.Contracts.Models.EventTicketDetail EventTicketDetail { get; set; }
        public FIL.Contracts.Models.TicketCategory TicketCategory { get; set; }
        public FIL.Contracts.Models.EventTicketAttribute EventTicketAttribute { get; set; }
        public long TicketCategorySubTypeId { get; set; }
        public long TicketCategoryTypeId { get; set; }
        public List<TicketFeeDetail> TicketFeeDetails { get; set; }
    }
}