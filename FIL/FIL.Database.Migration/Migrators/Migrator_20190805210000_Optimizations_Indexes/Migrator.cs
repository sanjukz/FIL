using FIL.Database.Migration.Core.Attributes;
using FIL.Database.Migration.Core.Migrators;

namespace FIL.Database.Migration.Migrators.Migrator_20190806180000_Optimizations_Indexes
{
    [Migration(2019, 08, 06, 18, 00, 0)]
    public class Migrator : BaseMigrator
    {
        public override void Up()
        {
            if (!Schema.Table("PlaceWeekOffs").Index("IX_PlaceWeekOffs_IsEnabled").Exists())
            {
                Create.Index()
                  .OnTable("Users")
                  .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("EventPartners").Index("idx_EventPartners_AltId").Exists())
            {
                Create.Index()
                 .OnTable("EventPartners")
                 .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("TournamentLayoutSectionSeats").Index("idx_TournamentLayoutSectionSeats_AltId").Exists())
            {
                Create.Index()
                .OnTable("TournamentLayoutSectionSeats")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("PlaceDocumentTypeMappings").Index("IX_PlaceDocumentTypeMappings_IsEnabled").Exists())
            {
                Create.Index()
              .OnTable("PlaceDocumentTypeMappings")
             .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("MatchLayouts").Index("idx_MatchLayouts_AltId").Exists())
            {
                Create.Index()
                .OnTable("MatchLayouts")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("EventTicketDetails").Index("idx_EventTicketDetails_EventDetailId_TicketCategoryId").Exists())
            {
                Create.Index()
               .OnTable("EventTicketDetails")
               .OnColumn("EventDetailId").Ascending()
               .OnColumn("TicketCategoryId").Ascending();

            }
            if (!Schema.Table("CustomerInformations").Index("IX_CustomerInformations_IsEnabled").Exists())
            {
                Create.Index()
           .OnTable("CustomerInformations")
           .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("MatchLayoutSections").Index("idx_MatchLayoutSections_AltId").Exists())
            {
                Create.Index()
                .OnTable("MatchLayoutSections")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("CustomerDocumentTypes").Index("IX_CustomerDocumentTypes_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("CustomerDocumentTypes")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("MasterDynamicStadiumCoordinates").Index("IX_MasterDynamicStadiumCoordinates_IsEnabled").Exists())
            {
                Create.Index()
            .OnTable("MasterDynamicStadiumCoordinates")
            .OnColumn("IsEnabled").Ascending();


            }
            if (!Schema.Table("EventCustomerInformationMappings").Index("IX_EventCustomerInformationMappings_IsEnabled").Exists())
            {
                Create.Index()
            .OnTable("EventCustomerInformationMappings")
            .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("PlaceCustomerDocumentTypeMappings").Index("IX_PlaceCustomerDocumentTypeMappings_IsEnabled").Exists())
            {
                Create.Index()
           .OnTable("PlaceCustomerDocumentTypeMappings")
           .OnColumn("IsEnabled").Ascending();

            }
            if (!Schema.Table("MasterDynamicStadiumSectionDetails").Index("IX_MasterDynamicStadiumSectionDetails_IsEnabled").Exists())
            {
                Create.Index()
            .OnTable("MasterDynamicStadiumSectionDetails")
            .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("MatchLayoutSectionSeats").Index("idx_MatchLayoutSectionSeats_AltId").Exists())
            {
                Create.Index()
               .OnTable("MatchLayoutSectionSeats")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("MatchLayoutSectionSeats").Index("idx_MatchLayoutSectionSeats_RowOrder_ColumnOrder").Exists())
            {
                Create.Index()
              .OnTable("MatchLayoutSectionSeats")
              .OnColumn("RowOrder").Ascending()
              .OnColumn("ColumnOrder").Ascending();
            }
            if (!Schema.Table("PlaceTicketRedemptionDetails").Index("IX_PlaceTicketRedemptionDetails_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("PlaceTicketRedemptionDetails")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("MatchSeatTicketDetails").Index("idx_MatchSeatTicketDetails_AltId").Exists())
            {
                Create.Index()
               .OnTable("MatchSeatTicketDetails")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("MatchSeatTicketDetails").Index("idx_MatchSeatTicketDetails_TransactionId_MatchLayoutSectionSeatId_EventTicketDetailId_BarcodeNumber").Exists())
            {
                Create.Index()
           .OnTable("MatchSeatTicketDetails")
           .OnColumn("MatchLayoutSectionSeatId").Ascending()
                .OnColumn("EventTicketDetailId").Ascending()
                .OnColumn("BarcodeNumber").Ascending()
                .OnColumn("TransactionId").Ascending();
            }
            if (!Schema.Table("MatchSeatTicketDetails").Index("idx_MatchSeatTicketDetails_TransactionId").Exists())
            {
                Create.Index()
                .OnTable("MatchSeatTicketDetails")
                .OnColumn("TransactionId").Ascending();
            }
            if (!Schema.Table("MatchSeatTicketDetails").Index("idx_MatchSeatTicketDetails_SeatStatusId").Exists())
            {
                Create.Index()
               .OnTable("MatchSeatTicketDetails")
               .OnColumn("SeatStatusId").Ascending();
            }
            if (!Schema.Table("MatchSeatTicketDetails").Index("idx_MatchSeatTicketDetails_EventTicketDetailId_SeatStatusId_BarcodeNumber_IsEnabled").Exists())
            {
                Create.Index()
                .OnTable("MatchSeatTicketDetails")
                 .OnColumn("EventTicketDetailId").Ascending()
                  .OnColumn("SeatStatusId").Ascending()
                  .OnColumn("BarcodeNumber").Ascending()
                .OnColumn("IsEnabled").Ascending();

            }
            if (!Schema.Table("MatchSeatTicketDetails").Index("IX_MatchSeatTicketDetails_MatchLayoutSectionSeatId").Exists())
            {
                Create.Index()
             .OnTable("MatchSeatTicketDetails")
             .OnColumn("MatchLayoutSectionSeatId").Ascending();
            }
            if (!Schema.Table("RefundPolicies").Index("IX_RefundPolicies_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("RefundPolicies")
                .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("CustomerUpdates").Index("IX_CustomerUpdates_SiteId").Exists())
            {
                Create.Index()
               .OnTable("CustomerUpdates")
               .OnColumn("SiteId").Ascending();

            }
            if (!Schema.Table("CustomerUpdates").Index("IX_CustomerUpdates_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("CustomerUpdates")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("DynamicStadiumCoordinates").Index("IX_DynamicStadiumCoordinate_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("DynamicStadiumCoordinates")
               .OnColumn("IsEnabled").Ascending();

            }
            if (!Schema.Table("DynamicStadiumCoordinates").Index("idx_DynamicStadiumCoordinates_AltId").Exists())
            {
                Create.Index()
               .OnTable("DynamicStadiumCoordinates")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("DynamicStadiumCoordinates").Index("IX_DynamicStadiumCoordinates_VenueId").Exists())
            {
                Create.Index()
               .OnTable("DynamicStadiumCoordinates")
               .OnColumn("VenueId").Ascending();

            }
            if (!Schema.Table("DynamicStadiumTicketCategoriesDetails").Index("IX_DynamicStadiumTicketCategoriesDetails_IsEnabled").Exists())
            {
                Create.Index()
           .OnTable("DynamicStadiumTicketCategoriesDetails")
           .OnColumn("IsEnabled").Ascending();

            }
            if (!Schema.Table("DynamicStadiumTicketCategoriesDetails").Index("idx_DynamicStadiumTicketCategoriesDetails_AltId").Exists())
            {
                Create.Index()
                .OnTable("DynamicStadiumTicketCategoriesDetails")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("AccessTokens").Index("idx_AccessTokens_AltId").Exists())
            {
                Create.Index()
             .OnTable("AccessTokens")
             .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("CorporateTicketAllocationDetails").Index("idx_CorporateTicketAllocationDetails_AltId").Exists())
            {
                Create.Index()
                 .OnTable("CorporateTicketAllocationDetails")
                 .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("Events").Index("idx_Events_AltId").Exists())
            {
                Create.Index()
                .OnTable("Events")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("Events").Index("idx_Events_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("Events")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("Events").Index("IX_Events_Slug").Exists())
            {
                Create.Index()
                .OnTable("Events")
                .OnColumn("Slug").Ascending();
            }

            if (!Schema.Table("Zipcodes").Index("idx_Zipcodes_AltId").Exists())
            {
                Create.Index()
         .OnTable("Zipcodes")
         .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("UserAddressDetails").Index("idx_UserAddressDetails_AltId").Exists())
            {
                Create.Index()
               .OnTable("UserAddressDetails")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("CorporateTransactionDetails").Index("idx_CorporateTransactionDetails_AltId").Exists())
            {
                Create.Index()
               .OnTable("CorporateTransactionDetails")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("EventDetails").Index("idx_EventDetails_VenueId").Exists())
            {
                Create.Index()
               .OnTable("EventDetails")
               .OnColumn("VenueId").Ascending();
            }
            if (!Schema.Table("EventDetails").Index("idx_EventDetails_EventId_VenueId_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("EventDetails")
               .OnColumn("EventId").Ascending()
               .OnColumn("VenueId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("EventSiteContentMappings").Index("idx_EventSiteContentMappings_AltId").Exists())
            {
                Create.Index()
               .OnTable("EventSiteContentMappings")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("Teams").Index("idx_Teams_AltId").Exists())
            {
                Create.Index()
               .OnTable("Teams")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("CorporateTransactionPaymentDetails").Index("idx_CorporateTransactionPaymentDetails_AltId").Exists())
            {
                Create.Index()
               .OnTable("CorporateTransactionPaymentDetails")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("TransactionDetails").Index("idx_TransactionDetails_TransactionId").Exists())
            {
                Create.Index()
               .OnTable("TransactionDetails")
               .OnColumn("TransactionId").Ascending();
            }
            if (!Schema.Table("TicketCategoryTypes").Index("IX_TicketCategoryTypes_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("TicketCategoryTypes")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("Cities").Index("idx_Cities_AltId").Exists())
            {
                Create.Index()
               .OnTable("Cities")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("EventGalleryImages").Index("idx_EventGalleryImages_AltId").Exists())
            {
                Create.Index()
               .OnTable("EventGalleryImages")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("TicketCategorySubTypes").Index("IX_TicketCategorySubTypes_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("TicketCategorySubTypes")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("BO_RetailCustomer").Index("idx_BO_RetailCustomer_TransId").Exists())
            {
                Create.Index()
               .OnTable("BO_RetailCustomer")
               .OnColumn("Trans_Id").Ascending();
            }
            if (!Schema.Table("ClientPointOfContacts").Index("idx_ClientPointOfContacts_AltId").Exists())
            {
                Create.Index()
               .OnTable("ClientPointOfContacts")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("NetBankingBankDetails").Index("idx_NetBankingBankDetails_AltId").Exists())
            {
                Create.Index()
               .OnTable("NetBankingBankDetails")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("EventTicketDetailTicketCategoryTypeMappings").Index("IX_EventTicketDetailTicketCategoryTypeMappings_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("EventTicketDetailTicketCategoryTypeMappings")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("EventTicketDetailTicketCategoryTypeMappings").Index("NonClusteredIndex-20190730-165431").Exists())
            {
                Create.Index()
              .OnTable("EventTicketDetailTicketCategoryTypeMappings")
              .OnColumn("EventTicketDetailId").Ascending();

            }
            if (!Schema.Table("OfflineBarcodeDetails").Index("idx_OfflineBarcodeDetails_BarcodeNumber").Exists())
            {
                Create.Index()
               .OnTable("OfflineBarcodeDetails")
               .OnColumn("BarcodeNumber").Ascending();
            }
            if (!Schema.Table("OfflineBarcodeDetails").Index("idx_OfflineBarcodeDetails_EventTicketDetailId").Exists())
            {
                Create.Index()
               .OnTable("OfflineBarcodeDetails")
               .OnColumn("EventTicketDetailId").Ascending();
            }
            if (!Schema.Table("BoCustomerDetails").Index("idx_BoCustomerDetails_AltId").Exists())
            {
                Create.Index()
                .OnTable("BoCustomerDetails")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("CashCardDetails").Index("idx_CashCardDetails_AltId").Exists())
            {
                Create.Index()
                .OnTable("CashCardDetails")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("BoUserVenues").Index("idx_BoUserVenues_AltId").Exists())
            {
                Create.Index()
               .OnTable("BoUserVenues")
               .OnColumn("AltId").Ascending();

            }
            if (!Schema.Table("Countries").Index("idx_Countries_AltId").Exists())
            {
                Create.Index()
          .OnTable("Countries")
          .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("States").Index("idx_States_AltId").Exists())
            {
                Create.Index()
               .OnTable("States")
               .OnColumn("AltId").Ascending();

            }
            if (!Schema.Table("PlaceVisitDurations").Index("IX_PlaceVisitDurations_IsEnabled").Exists())
            {
                Create.Index()
           .OnTable("PlaceVisitDurations")
           .OnColumn("IsEnabled").Ascending();

            }
            if (!Schema.Table("EventTicketAttributes").Index("idx_EventTicketAttributes_EventTicketDetailId_Price").Exists())
            {
                Create.Index()
               .OnTable("EventTicketAttributes")
               .OnColumn("Price").Ascending()
               .OnColumn("EventTicketDetailId").Ascending();
            }
            if (!Schema.Table("TicketFeeDetails").Index("idx_TicketFeeDetails_EventTicketAttributeId").Exists())
            {
                Create.Index()
              .OnTable("TicketFeeDetails")
              .OnColumn("EventTicketAttributeId").Ascending();
            }
            if (!Schema.Table("TransactionReleasedLog").Index("idx_TransactionReleasedLog_TransactionId").Exists())
            {
                Create.Index()
                             .OnTable("TransactionReleasedLog")
                             .OnColumn("TransactionId").Ascending();
            }
            if (!Schema.Table("TransactionDeliveryDetails").Index("idx_TransactionDeliveryDetails_TransactionDetailId").Exists())
            {
                Create.Index()
                        .OnTable("TransactionDeliveryDetails")
                        .OnColumn("TransactionDetailId").Ascending();

            }
            if (!Schema.Table("TransactionPaymentDetails").Index("IX_TransactionPaymentDetails_TransactionId").Exists())
            {


                Create.Index()
                     .OnTable("TransactionPaymentDetails")
                     .OnColumn("TransactionId").Ascending();
            }
            if (!Schema.Table("DTCMEventTicketDetailMapping").Index("idx_DTCMEventTicketDetailMapping_AltId").Exists())
            {
                Create.Index()
                       .OnTable("DTCMEventTicketDetailMapping")
                       .OnColumn("AltId").Ascending();

            }
            if (!Schema.Table("DTCMTransactionBarcodes").Index("idx_DTCMTransactionBarcodes_AltId").Exists())
            {
                Create.Index()
                          .OnTable("DTCMTransactionBarcodes")
                          .OnColumn("AltId").Ascending();

            }
            if (!Schema.Table("EntryGates").Index("idx_EntryGates_AltId").Exists())
            {
                Create.Index()
                         .OnTable("EntryGates")
                         .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("Transactions").Index("idx_Transactions_AltId_EmailId_PhoneNumber").Exists())
            {
                Create.Index()
                        .OnTable("Transactions")
                        .OnColumn("PhoneNumber").Ascending()
                        .OnColumn("EmailId").Ascending()
                        .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("Transactions").Index("idx_Transactions_TransactionStatusId_PhoneNumber").Exists())
            {
                Create.Index()
                        .OnTable("Transactions")
                        .OnColumn("TransactionStatusId").Ascending()
                        .OnColumn("PhoneNumber").Ascending();
            }
            if (!Schema.Table("Transactions").Index("idx_Transactions_CreatedUtc_CreatedBy").Exists())
            {
                Create.Index()
                         .OnTable("Transactions")
                         .OnColumn("CreatedUtc").Ascending()
                         .OnColumn("CreatedBy").Ascending();

            }
            if (!Schema.Table("Transactions").Index("idx_Transactions_ChannelId").Exists())
            {
                Create.Index()
                         .OnTable("Transactions")
                         .OnColumn("ChannelId").Ascending();
            }
            if (!Schema.Table("DTCMTransactionMapping").Index("idx_DTCMTransactionMapping_AltId").Exists())
            {
                Create.Index()
                          .OnTable("DTCMTransactionMapping")
                          .OnColumn("AltId").Ascending();

            }
            if (!Schema.Table("MasterVenueLayouts").Index("idx_MasterVenueLayouts_AltId").Exists())
            {
                Create.Index()
                         .OnTable("MasterVenueLayouts")
                         .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("TransactionSeatDetails").Index("idx_TransactionSeatDetails_TransactionDetailId").Exists())
            {
                Create.Index()
                           .OnTable("TransactionSeatDetails")
                           .OnColumn("TransactionDetailId").Ascending();
            }
            if (!Schema.Table("EventBannerMappings").Index("idx_EventBannerMappings_AltId").Exists())
            {
                Create.Index()
                        .OnTable("EventBannerMappings")
                        .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("MasterVenueLayoutSections").Index("idx_MasterVenueLayoutSections_AltId").Exists())
            {
                Create.Index()
                        .OnTable("MasterVenueLayoutSections")
                        .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("UserCardDetails").Index("idx_UserCardDetails_AltId").Exists())
            {
                Create.Index()
                      .OnTable("UserCardDetails")
                      .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("EventDeliveryTypeDetails").Index("idx_EventDeliveryTypeDetails_EventDetailId").Exists())
            {
                Create.Index()
                .OnTable("EventDeliveryTypeDetails")
                .OnColumn("EventDetailId").Ascending();
            }
            if (!Schema.Table("PlaceHolidayDates").Index("IX_PlaceHolidayDates_IsEnabled").Exists())
            {
                Create.Index()
               .OnTable("PlaceHolidayDates")
               .OnColumn("IsEnabled").Ascending();
            }
            if (!Schema.Table("MasterVenueLayoutSectionSeats").Index("idx_MasterVenueLayoutSectionSeats_AltId").Exists())
            {
                Create.Index()
               .OnTable("MasterVenueLayoutSectionSeats")
               .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("Users").Index("idx_Users_AltId").Exists())
            {
                Create.Index()
               .OnTable("Users")
               .OnColumn("AltId").Ascending()
               .OnColumn("FirstName").Ascending()
               .OnColumn("LastName").Ascending()
               .OnColumn("Email").Ascending()
               .OnColumn("PhoneNumber").Ascending();
            }
            if (!Schema.Table("Users").Index("idx_Users_Email").Exists())
            {
                Create.Index()
              .OnTable("Users")
              .OnColumn("Email").Ascending();

            }
            if (!Schema.Table("TournamentLayouts").Index("idx_TournamentLayouts_AltId").Exists())
            {
                Create.Index()
           .OnTable("TournamentLayouts")
           .OnColumn("AltId").Ascending();

            }
            if (!Schema.Table("EventHistories").Index("idx_EventHistories_AltId").Exists())
            {
                Create.Index()
            .OnTable("EventHistories")
            .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("Venues").Index("idx_Venues_AltId").Exists())
            {
                Create.Index()
                         .OnTable("Venues")
                         .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("EventIntegrationDetails").Index("idx_EventIntegrationDetails_AltId").Exists())
            {
                Create.Index()
                .OnTable("EventIntegrationDetails")
                .OnColumn("AltId").Ascending();
            }
            if (!Schema.Table("TournamentLayoutSections").Index("idx_TournamentLayoutSections_AltId").Exists())
            {
                Create.Index()
                .OnTable("TournamentLayoutSections")
                .OnColumn("AltId").Ascending();
            }

            /*newly added indexes*/

            if (Schema.Table("Cities").Exists())
            {
                Create.Index()
                    .OnTable("Cities")
                    .OnColumn("StateId").Ascending();
            }
            if (Schema.Table("CorporateRequests").Exists())
            {
                Create.Index()
                   .OnTable("CorporateRequests")
                   .OnColumn("SponsorId").Ascending();

            }

            if (Schema.Table("CorporateTicketAllocationDetails").Exists())
            {
                Create.Index()
                 .OnTable("CorporateTicketAllocationDetails")
                 .OnColumn("EventTicketAttributeId").Ascending()
                 .OnColumn("IsEnabled").Ascending()
                 .OnColumn("SponsorId").Ascending();
            }

            if (Schema.Table("CorporateTransactionDetails").Exists())
            {
                Create.Index()
                .OnTable("CorporateTransactionDetails")
                .OnColumn("TransactionId").Ascending()
                .OnColumn("EventTicketAttributeId").Ascending()
                .OnColumn("SponsorId").Ascending()
                .OnColumn("TransactingOptionId").Ascending()
                .OnColumn("IsEnabled").Ascending();
            }


            if (Schema.Table("EventAttributes").Exists())
            {
                Create.Index()
              .OnTable("EventAttributes")
              .OnColumn("EventDetailId");
            }
            if (Schema.Table("EventCategoryMappings").Exists())
            {
                Create.Index()
               .OnTable("EventCategoryMappings")
               .OnColumn("EventCategoryId").Ascending();
            }
            if (Schema.Table("EventDetails").Exists())
            {
                Create.Index()
               .OnTable("EventDetails")
               .OnColumn("AltId").Ascending()
               .OnColumn("EventId").Ascending()
               .OnColumn("IsEnabled").Ascending()
               .OnColumn("VenueId").Ascending();
            }
            if (Schema.Table("EventGalleryImages").Exists())
            {
                Create.Index()
             .OnTable("EventGalleryImages")
             .OnColumn("EventId").Ascending()
             .OnColumn("IsEnabled").Ascending();
            }
            if (Schema.Table("Events").Exists())
            {
                Create.Index()
                .OnTable("Events")
                .OnColumn("IsFeel").Ascending()
                .OnColumn("EventCategoryId").Ascending();
            }

            if (Schema.Table("EventSiteIdMappings").Exists())
            {
                Create.Index()
              .OnTable("EventSiteIdMappings")
               .OnColumn("EventId").Ascending();
            }

            if (Schema.Table("EventSponsorMappings").Exists())
            {
                Create.Index()
               .OnTable("EventSponsorMappings")
               .OnColumn("EventDetailId").Ascending()
               .OnColumn("SponsorId").Ascending()
               .OnColumn("IsEnabled").Ascending();

            }

            if (Schema.Table("EventsUserMappings").Exists())
            {
                Create.Index()
              .OnTable("EventsUserMappings")
               .OnColumn("EventId").Ascending()
                       .OnColumn("UserId").Ascending()
                       .OnColumn("EventDetailId").Ascending()
                      .OnColumn("IsEnabled").Ascending();
            }

            if (Schema.Table("EventTicketDetails").Exists())
            {
                Create.Index()
               .OnTable("EventTicketDetails")
               .OnColumn("IsEnabled").Ascending();
            }

            if (Schema.Table("HandoverSheets").Exists())
            {
                Create.Index()
               .OnTable("HandoverSheets")
               .OnColumn("TransactionId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }
            if (Schema.Table("MatchLayoutSectionSeats").Exists())
            {
                Create.Index()
               .OnTable("MatchLayoutSectionSeats")
               .OnColumn("MatchLayoutSectionId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }

            if (Schema.Table("MatchSeatTicketDetails").Exists())
            {
                Create.Index()
               .OnTable("MatchSeatTicketDetails")
               .OnColumn("EventTicketDetailId").Ascending()
               .OnColumn("SeatStatusId").Ascending()
               .OnColumn("TransactionId").Ascending()
               .OnColumn("BarcodeNumber").Ascending()
               .OnColumn("SponsorId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }

            if (Schema.Table("PlaceHolidayDates").Exists())
            {
                Create.Index()
               .OnTable("PlaceHolidayDates")
               .OnColumn("EventId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }
            if (Schema.Table("PlaceWeekOffs").Exists())
            {
                Create.Index()
               .OnTable("PlaceWeekOffs")
               .OnColumn("EventId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }
            if (Schema.Table("RePrintRequestDetails").Exists())
            {
                Create.Index()
               .OnTable("RePrintRequestDetails")
               .OnColumn("IsApproved").Ascending();
            }

            if (Schema.Table("TransactionDetails").Exists())
            {
                Create.Index()
               .OnTable("TransactionDetails")
               .OnColumn("EventTicketAttributeId").Ascending()
               .OnColumn("TicketTypeId").Ascending();
            }
            if (Schema.Table("Transactions").Exists())
            {
                Create.Index()
               .OnTable("Transactions")
               .OnColumn("CurrencyId").Ascending()
               .OnColumn("TransactionStatusId").Ascending()
               .OnColumn("DiscountCode").Ascending()
               .OnColumn("EmailId").Ascending();
            }

            if (Schema.Table("UserAddressDetails").Exists())
            {
                Create.Index()
               .OnTable("UserAddressDetails")
               .OnColumn("UserId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }
            if (Schema.Table("UserCardDetails").Exists())
            {
                Create.Index()
               .OnTable("UserCardDetails")
               .OnColumn("UserId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }

            if (Schema.Table("Venues").Exists())
            {
                Create.Index()
               .OnTable("Venues")
               .OnColumn("CityId").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }
            if (Schema.Table("Zipcodes").Exists())
            {
                Create.Index()
               .OnTable("Zipcodes")
               .OnColumn("Postalcode").Ascending()
               .OnColumn("IsEnabled").Ascending();
            }
        }
    }
}
