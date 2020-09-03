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

        TransactionReport GetTransactionReportDataAsSingleDataModel(string EventAltId, Guid UserAltId, string EventDetailId, DateTime FromDate, DateTime ToDate, string currencyType);

        TransactionReport GetFAPTransactionReport(string EventAltId, DateTime FromDate, DateTime ToDate, string currencyType);

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