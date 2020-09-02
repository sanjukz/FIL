using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.Reporting;
using System;
using System.Collections.Generic;
using System.Data;

namespace FIL.Api.Repositories
{
    public interface IReportingRepository : IOrmRepository<Reporting, Reporting>
    {
        Reporting GetTransactionReportData(TransactionReportZsuiteQuery query);

        Reporting GetBOTransactionReportData(FIL.Contracts.Queries.BoxOffice.TransactionReportQuery query);

        TransactionReport GetTransactionReportDataAsSingleDataModel(string EventAltId, Guid UserAltId, string EventDetailId, DateTime FromDate, DateTime ToDate, string currencyType);

        TransactionReport GetFAPTransactionReport(string EventAltId, DateTime FromDate, DateTime ToDate, string currencyType);

        Reporting GetScanningReportData(ScanningReportQuery query);

        Reporting GetRASVScanningReportData(RASVScanningReportQuery query);

        Reporting GetTransactionReportByDateData(TransactionReportQueryByDate query);

        IEnumerable<FIL.Contracts.Models.TransactionReport.FAPAllPlacesResponseModel> GetAllReportEvents(bool isFeel, List<long> EventIds, bool isSuperUser);

        TransactionReport GetTransactionData(string EventAltId, Guid UserAltId, string VenueId, string EventDetailId, string TicketCategoryId, string ChannelId, string CurrencyTypes, string TicketTypes, string TransactionTypes, string PaymentGatewayes, DateTime FromDate, DateTime ToDate);

        IEnumerable<FIL.Contracts.Models.Venue> GetVenuesByEvents(List<Guid> eventAltIds);

        IEnumerable<FIL.Contracts.Models.EventDetail> GetSubeventByVenues(List<Guid> eventAltIds, List<Guid> venueAltIds);

        IEnumerable<FIL.Contracts.Models.TicketCategory> GetTicketCategoryBySubevents(List<long> eventDetailIds);

        IEnumerable<FIL.Contracts.Models.CurrencyType> GetCurrencyBySubevents(List<long> eventDetailIds);
    }

    public class ReportingRepository : BaseOrmRepository<Reporting>, IReportingRepository
    {
        public ReportingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Reporting GetTransactionReportData(TransactionReportZsuiteQuery query)
        {
            Reporting reporting = new Reporting();
            var reportData = GetCurrentConnection().QueryMultiple("spTransactionReport", new { EventAltId = query.EventAltId, UserAltId = query.UserAltId, EventDetailId = query.EventDetailId, FromDate = query.FromDate, ToDate = query.ToDate }, commandType: CommandType.StoredProcedure);

            reporting.Transaction = reportData.Read<Transaction>();
            reporting.TransactionDetail = reportData.Read<TransactionDetail>();
            reporting.TransactionDeliveryDetail = reportData.Read<TransactionDeliveryDetail>();
            reporting.TransactionPaymentDetail = reportData.Read<TransactionPaymentDetail>();
            reporting.CurrencyType = reportData.Read<CurrencyType>();
            reporting.EventTicketAttribute = reportData.Read<EventTicketAttribute>();
            reporting.EventTicketDetail = reportData.Read<EventTicketDetail>();
            reporting.TicketCategory = reportData.Read<TicketCategory>();
            reporting.EventDetail = reportData.Read<EventDetail>();
            reporting.EventAttribute = reportData.Read<EventAttribute>();
            reporting.Venue = reportData.Read<Venue>();
            reporting.City = reportData.Read<City>();
            reporting.State = reportData.Read<State>();
            reporting.Country = reportData.Read<Country>();
            reporting.Event = reportData.Read<Event>();
            reporting.User = reportData.Read<User>();
            reporting.UserCardDetail = reportData.Read<UserCardDetail>();
            reporting.IPDetail = reportData.Read<IPDetail>();
            reporting.TicketFeeDetail = reportData.Read<TicketFeeDetail>();
            reporting.ReportingColumnsUserMapping = reportData.Read<ReportingColumnsUserMapping>();
            reporting.ReportingColumnsMenuMapping = reportData.Read<ReportingColumnsMenuMapping>();
            reporting.ReportColumns = reportData.Read<ReportingColumn>();
            return reporting;
        }

        public TransactionReport GetTransactionReportDataAsSingleDataModel(string EventAltId, Guid UserAltId, string EventDetailId, DateTime FromDate, DateTime ToDate, string currencyType)
        {
            TransactionReport reporting = new TransactionReport();
            var reportData = GetCurrentConnection().QueryMultiple("CR_GetTransactionReport_MultiSelection", new { EventAltId = EventAltId, UserAltId = UserAltId, EventDetailId = EventDetailId, FromDate = FromDate, ToDate = ToDate, Currency = currencyType }, commandType: CommandType.StoredProcedure);

            reporting.TransactionData = reportData.Read<TransactionData>();
            reporting.ChannelWiseSummary = reportData.Read<TransactionData>();
            reporting.CurrencyWiseSummary = reportData.Read<TransactionData>();
            reporting.TicketTypeWiseSummary = reportData.Read<TransactionData>();
            reporting.VenueWiseSummary = reportData.Read<TransactionData>();
            reporting.EventWiseSummary = reportData.Read<TransactionData>();
            reporting.ReportColumns = reportData.Read<ReportingColumn>();
            reporting.SummaryColumns = reportData.Read<ReportingColumn>();
            reporting.DynamicSummaryColumns = reportData.Read<ReportingColumn>();
            reporting.DynamicSummaryInfoColumns = reportData.Read<ReportingColumn>();
            return reporting;
        }

        public TransactionReport GetFAPTransactionReport(string EventAltId, DateTime FromDate, DateTime ToDate, string currencyType)
        {
            TransactionReport reporting = new TransactionReport();
            var reportData = GetCurrentConnection().QueryMultiple("FAP_GetTransactionReport", new { EventAltId = EventAltId, FromDate = FromDate, ToDate = ToDate, Currency = currencyType }, commandType: CommandType.StoredProcedure);

            reporting.TransactionData = reportData.Read<TransactionData>();
            reporting.ChannelWiseSummary = reportData.Read<TransactionData>();
            reporting.CurrencyWiseSummary = reportData.Read<TransactionData>();
            reporting.TicketTypeWiseSummary = reportData.Read<TransactionData>();
            reporting.VenueWiseSummary = reportData.Read<TransactionData>();
            reporting.EventWiseSummary = reportData.Read<TransactionData>();
            reporting.ReportColumns = reportData.Read<ReportingColumn>();
            reporting.SummaryColumns = reportData.Read<ReportingColumn>();
            reporting.DynamicSummaryColumns = reportData.Read<ReportingColumn>();
            reporting.DynamicSummaryInfoColumns = reportData.Read<ReportingColumn>();
            return reporting;
        }

        public Reporting GetBOTransactionReportData(FIL.Contracts.Queries.BoxOffice.TransactionReportQuery query)
        {
            Reporting reporting = new Reporting();
            var reportData = GetCurrentConnection().QueryMultiple("spBoxOfficeTransactionReport", new { UserAltId = query.UserAltId, EventAltId = query.EventAltId, EventDetailId = query.EventDetailId, FromDate = query.FromDate, ToDate = query.ToDate }, commandType: CommandType.StoredProcedure);
            reporting.Transaction = reportData.Read<Transaction>();
            reporting.TransactionDetail = reportData.Read<TransactionDetail>();
            reporting.TransactionDeliveryDetail = reportData.Read<TransactionDeliveryDetail>();
            reporting.TransactionPaymentDetail = reportData.Read<TransactionPaymentDetail>();
            reporting.CurrencyType = reportData.Read<CurrencyType>();
            reporting.EventTicketAttribute = reportData.Read<EventTicketAttribute>();
            reporting.EventTicketDetail = reportData.Read<EventTicketDetail>();
            reporting.TicketCategory = reportData.Read<TicketCategory>();
            reporting.EventDetail = reportData.Read<EventDetail>();
            reporting.EventAttribute = reportData.Read<EventAttribute>();
            reporting.Venue = reportData.Read<Venue>();
            reporting.City = reportData.Read<City>();
            reporting.State = reportData.Read<State>();
            reporting.Country = reportData.Read<Country>();
            reporting.Event = reportData.Read<Event>();
            reporting.User = reportData.Read<User>();
            reporting.UserCardDetail = reportData.Read<UserCardDetail>();
            reporting.IPDetail = reportData.Read<IPDetail>();
            reporting.TicketFeeDetail = reportData.Read<TicketFeeDetail>();
            reporting.ReportingColumnsUserMapping = reportData.Read<ReportingColumnsUserMapping>();
            reporting.ReportingColumnsMenuMapping = reportData.Read<ReportingColumnsMenuMapping>();
            reporting.ReportColumns = reportData.Read<ReportingColumn>();
            reporting.BOCustomerDetail = reportData.Read<FIL.Contracts.Models.BOCustomerDetail>();
            return reporting;
        }

        public Reporting GetTransactionReportByDateData(TransactionReportQueryByDate query)
        {
            Reporting reporting = new Reporting();
            var reportData = GetCurrentConnection().QueryMultiple("spTransactionReportByDateRange", new { UserAltId = query.UserAltId, FromDate = query.FromDate, ToDate = query.ToDate }, commandType: CommandType.StoredProcedure);

            reporting.Transaction = reportData.Read<Transaction>();
            reporting.TransactionDetail = reportData.Read<TransactionDetail>();
            reporting.TransactionDeliveryDetail = reportData.Read<TransactionDeliveryDetail>();
            reporting.TransactionPaymentDetail = reportData.Read<TransactionPaymentDetail>();
            reporting.CurrencyType = reportData.Read<CurrencyType>();
            reporting.EventTicketAttribute = reportData.Read<EventTicketAttribute>();
            reporting.EventTicketDetail = reportData.Read<EventTicketDetail>();
            reporting.TicketCategory = reportData.Read<TicketCategory>();
            reporting.EventDetail = reportData.Read<EventDetail>();
            reporting.EventAttribute = reportData.Read<EventAttribute>();
            reporting.Venue = reportData.Read<Venue>();
            reporting.City = reportData.Read<City>();
            reporting.State = reportData.Read<State>();
            reporting.Country = reportData.Read<Country>();
            reporting.Event = reportData.Read<Event>();
            reporting.User = reportData.Read<User>();
            reporting.UserCardDetail = reportData.Read<UserCardDetail>();
            reporting.IPDetail = reportData.Read<IPDetail>();
            reporting.TicketFeeDetail = reportData.Read<TicketFeeDetail>();
            reporting.ReportingColumnsUserMapping = reportData.Read<ReportingColumnsUserMapping>();
            reporting.ReportingColumnsMenuMapping = reportData.Read<ReportingColumnsMenuMapping>();
            reporting.ReportColumns = reportData.Read<ReportingColumn>();
            return reporting;
        }

        public Reporting GetScanningReportData(ScanningReportQuery query)
        {
            Reporting reporting = new Reporting();
            if (query.EventAltId.ToString().ToUpper() == "1F0257FA-EEA6-4469-A7BC-B878A215C8A9")
            {
                var reportData = GetCurrentConnection().QueryMultiple("spScanningReportRASV2", new { UserAltId = query.UserAltId, EventAltId = query.EventAltId, EventDetailId = query.EventDetailId, FromDate = query.FromDate, ToDate = query.ToDate }, commandType: CommandType.StoredProcedure);
                reporting.Transaction = reportData.Read<Transaction>();
                reporting.TransactionDetail = reportData.Read<TransactionDetail>();
                reporting.MatchSeatTicketDetail = reportData.Read<FIL.Contracts.Models.ScanningDetailModel>();
                reporting.EventTicketDetail = reportData.Read<EventTicketDetail>();
                reporting.EventDetail = reportData.Read<EventDetail>();
                reporting.EventAttribute = reportData.Read<EventAttribute>();
                reporting.Event = reportData.Read<Event>();
                reporting.TicketCategory = reportData.Read<TicketCategory>();
                reporting.ReportingColumnsUserMapping = reportData.Read<ReportingColumnsUserMapping>();
                reporting.ReportingColumnsMenuMapping = reportData.Read<ReportingColumnsMenuMapping>();
                reporting.ReportColumns = reportData.Read<ReportingColumn>();
            }
            else
            {
                var reportData = GetCurrentConnection().QueryMultiple("spScanningReportTemp", new { UserAltId = query.UserAltId, EventAltId = query.EventAltId, EventDetailId = query.EventDetailId, FromDate = query.FromDate, ToDate = query.ToDate }, commandType: CommandType.StoredProcedure);
                reporting.Transaction = reportData.Read<Transaction>();
                reporting.TransactionDetail = reportData.Read<TransactionDetail>();
                reporting.MatchSeatTicketDetail = reportData.Read<FIL.Contracts.Models.ScanningDetailModel>();
                reporting.EventTicketDetail = reportData.Read<EventTicketDetail>();
                reporting.EventDetail = reportData.Read<EventDetail>();
                reporting.EventAttribute = reportData.Read<EventAttribute>();
                reporting.Event = reportData.Read<Event>();
                reporting.TicketCategory = reportData.Read<TicketCategory>();
                reporting.ReportingColumnsUserMapping = reportData.Read<ReportingColumnsUserMapping>();
                reporting.ReportingColumnsMenuMapping = reportData.Read<ReportingColumnsMenuMapping>();
                reporting.ReportColumns = reportData.Read<ReportingColumn>();
            }
            return reporting;
        }

        public Reporting GetRASVScanningReportData(RASVScanningReportQuery query)
        {
            Reporting reporting = new Reporting();
            if (query.SearchBarcode)
            {
                var reportData = GetCurrentConnection().QueryMultiple("spRideRedemptionBarcodeReport", new { UserAltId = query.UserAltId, EventAltId = query.EventAltId, EventDetailId = query.EventDetailId, FromDate = query.FromDate, ToDate = query.ToDate, query.Barcode }, commandType: CommandType.StoredProcedure);
                reporting.Transaction = reportData.Read<Transaction>();
                reporting.TransactionDetail = reportData.Read<TransactionDetail>();
                reporting.MatchSeatTicketDetail = reportData.Read<FIL.Contracts.Models.ScanningDetailModel>();
                reporting.EventTicketDetail = reportData.Read<EventTicketDetail>();
                reporting.EventDetail = reportData.Read<EventDetail>();
                reporting.EventAttribute = reportData.Read<EventAttribute>();
                reporting.Event = reportData.Read<Event>();
                reporting.TicketCategory = reportData.Read<TicketCategory>();
                reporting.ReportingColumnsUserMapping = reportData.Read<ReportingColumnsUserMapping>();
                reporting.ReportingColumnsMenuMapping = reportData.Read<ReportingColumnsMenuMapping>();
                reporting.ReportColumns = reportData.Read<ReportingColumn>();
                return reporting;
            }
            else
            {
                var reportData = GetCurrentConnection().QueryMultiple("spRideRedemptionReport", new { UserAltId = query.UserAltId, EventAltId = query.EventAltId, EventDetailId = query.EventDetailId, FromDate = query.FromDate, ToDate = query.ToDate }, commandType: CommandType.StoredProcedure);
                reporting.Transaction = reportData.Read<Transaction>();
                reporting.TransactionDetail = reportData.Read<TransactionDetail>();
                reporting.MatchSeatTicketDetail = reportData.Read<FIL.Contracts.Models.ScanningDetailModel>();
                reporting.EventTicketDetail = reportData.Read<EventTicketDetail>();
                reporting.EventDetail = reportData.Read<EventDetail>();
                reporting.EventAttribute = reportData.Read<EventAttribute>();
                reporting.Event = reportData.Read<Event>();
                reporting.TicketCategory = reportData.Read<TicketCategory>();
                reporting.ReportingColumnsUserMapping = reportData.Read<ReportingColumnsUserMapping>();
                reporting.ReportingColumnsMenuMapping = reportData.Read<ReportingColumnsMenuMapping>();
                reporting.ReportColumns = reportData.Read<ReportingColumn>();
                return reporting;
            }
        }

        public IEnumerable<FIL.Contracts.Models.TransactionReport.FAPAllPlacesResponseModel> GetAllReportEvents(bool isFeel, List<long> eventIds, bool isSuperUser)
        {
            var eventIdsQuery = !isSuperUser ? " AND E.Id in @EventIds " : " ";
            return GetCurrentConnection().Query<FIL.Contracts.Models.TransactionReport.FAPAllPlacesResponseModel>("SELECT Distinct(E.Id),E.AltId,E.Name " +
         "from Transactions T WITH (NOLOCK) inner join TransactionDetails TD WITH (NOLOCK) ON TD.TransactionId=T.Id " +
        "inner join EventTicketAttributes ETA WITH (NOLOCK) ON ETA.Id=TD.EventTicketAttributeId " +
        "inner join EventTicketDetails ETD WITH (NOLOCK) on ETD.Id=ETA.EventTicketDetailId " +
        "inner join EventDetails ED WITH (NOLOCK) on ED.Id = ETD.EventDetailId " +
        "inner join Events E WITH (NOLOCK) on E.Id = Ed.EventId " +
        "Where T.TransactionStatusId = 8 AND " +
        " e.IsFeel = @IsFeel " + eventIdsQuery, new
        {
            IsFeel = isFeel,
            EventIds = eventIds
        });
        }

        public TransactionReport GetTransactionData(string EventAltId, Guid UserAltId, string VenueId, string EventDetailId, string TicketCategoryId, string ChannelId, string CurrencyTypes, string TicketTypes, string TransactionTypes, string PaymentGatewayes, DateTime FromDate, DateTime ToDate)
        {
            TransactionReport reporting = new TransactionReport();
            var reportData = GetCurrentConnection().QueryMultiple("CR_GetTransactionData_MultiSelection",
               new
               {
                   EventAltId = EventAltId,
                   UserAltId = UserAltId,
                   VenueId = VenueId,
                   EventDetailId = EventDetailId,
                   TicketCategoryId = TicketCategoryId,
                   ChannelId = ChannelId,
                   CurrencyTypes = CurrencyTypes,
                   TicketTypes = TicketTypes,
                   TransactionTypes = TransactionTypes,
                   PaymentGatewayes = PaymentGatewayes,
                   FromDate = FromDate,
                   ToDate = ToDate
               },
               commandType: CommandType.StoredProcedure);
            reporting.TransactionData = reportData.Read<TransactionData>();
            reporting.ReportColumns = reportData.Read<ReportingColumn>();
            return reporting;
        }

        public IEnumerable<FIL.Contracts.Models.Venue> GetVenuesByEvents(List<Guid> eventAltIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.Venue>("SELECT Distinct V.* FROM Events E WITH(NOLOCK) INNER JOIN EventDetails ED WITH(NOLOCK) ON E.Id = ED.EventId INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id Where E.AltId In(@EventAltIds)", new
            {
                EventAltIds = eventAltIds
            });
        }

        public IEnumerable<FIL.Contracts.Models.EventDetail> GetSubeventByVenues(List<Guid> eventAltIds, List<Guid> venueAltIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.EventDetail>("SELECT Distinct ED.* FROM Events E WITH(NOLOCK) INNER JOIN EventDetails ED WITH(NOLOCK) ON E.Id = ED.EventId INNER JOIN Venues V WITH(NOLOCK) ON ED.VenueId = V.Id Where V.AltId In(@VenueAltIds) And E.AltId In(@EventAltIds)", new
            {
                EventAltIds = eventAltIds,
                VenueAltIds = venueAltIds
            });
        }

        public IEnumerable<FIL.Contracts.Models.TicketCategory> GetTicketCategoryBySubevents(List<long> eventDetailIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.TicketCategory>("SELECT Distinct TC.* FROM EventDetails ED WITH(NOLOCK) INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id = ETD.EventDetailId Inner Join TicketCategories TC WITH(NOLOCK) ON ETD.TicketCategoryId = TC.Id Where Ed.Id in (SELECT Keyword FROM SplitString(@EventDetailsIds, ','))", new
            {
                EventDetailsIds = eventDetailIds
            });
        }

        public IEnumerable<FIL.Contracts.Models.CurrencyType> GetCurrencyBySubevents(List<long> eventDetailIds)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.CurrencyType>("SELECT Distinct CT.* FROM EventDetails ED WITH(NOLOCK) INNER JOIN EventTicketDetails ETD WITH(NOLOCK) ON ED.Id = ETD.EventDetailId INNER JOIN EventTicketAttributes ETA WITH(NOLOCK) ON ETA.EventTicketDetailId = ETD.Id Inner Join CurrencyTypes CT WITH(NOLOCK) ON(CT.Id = ETA.CurrencyId OR CT.Id = ETA.LocalCurrencyId)Where Ed.Id in (SELECT Keyword FROM SplitString(@EventDetailsIds, ','))", new
            {
                EventDetailsIds = eventDetailIds
            });
        }
    }
}