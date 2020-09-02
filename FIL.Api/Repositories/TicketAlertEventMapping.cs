using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITicketAlertEventMappingRepository : IOrmRepository<TicketAlertEventMapping, TicketAlertEventMapping>
    {
        TicketAlertEventMapping Get(int id);

        IEnumerable<TicketAlertEventMapping> GetByEventId(long eventId);

        List<TicketAlertReport> GetReportDataByEventId(long eventId);

        TicketAlertEventMapping GetByEventIdAndCountryId(long eventId, int countryId);
    }

    public class TicketAlertEventMappingRepository : BaseOrmRepository<TicketAlertEventMapping>, ITicketAlertEventMappingRepository
    {
        public TicketAlertEventMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TicketAlertEventMapping Get(int id)
        {
            return Get(new TicketAlertEventMapping { Id = id });
        }

        public IEnumerable<TicketAlertEventMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCity(TicketAlertEventMapping ticketAlertEventMapping)
        {
            Delete(ticketAlertEventMapping);
        }

        public TicketAlertEventMapping SaveCity(TicketAlertEventMapping ticketAlertEventMapping)
        {
            return Save(ticketAlertEventMapping);
        }

        public IEnumerable<TicketAlertEventMapping> GetByEventId(long eventId)
        {
            var ticketAlertEventMappingList = (GetAll(s => s.Where($"{nameof(TicketAlertEventMapping.EventId):C} = @EventId AND IsEnabled=1")
                                           .WithParameters(new { EventId = eventId })
             ));
            return ticketAlertEventMappingList;
        }

        public TicketAlertEventMapping GetByEventIdAndCountryId(long eventId, int countryId)
        {
            var TicketAlertEventMappingData = (GetAll(s => s.Where($"{nameof(TicketAlertEventMapping.EventId):C} = @EventId AND IsEnabled=1 AND {nameof(TicketAlertEventMapping.CountryId):C} = @CountryId")
                   .WithParameters(new { EventId = eventId, CountryId = countryId })
            )).FirstOrDefault();
            return TicketAlertEventMappingData;
        }

        public List<TicketAlertReport> GetReportDataByEventId(long eventId)
        {
            List<TicketAlertReport> reporting = new List<TicketAlertReport>();
            var reportData = GetCurrentConnection().QueryMultiple("spAllTicketAlertReport", new { eventId = eventId }, commandType: CommandType.StoredProcedure);

            reporting = reportData.Read<TicketAlertReport>().ToList();
            return reporting;
        }
    }
}