using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ITicketAlertUserMappingRepository : IOrmRepository<TicketAlertUserMapping, TicketAlertUserMapping>
    {
        TicketAlertUserMapping Get(int id);

        IEnumerable<TicketAlertUserMapping> GetByTicketAlertEventMapAndEmailId(int ticketAlertEventId, string emailId);
    }

    public class TicketAlertUserMappingRepository : BaseOrmRepository<TicketAlertUserMapping>, ITicketAlertUserMappingRepository
    {
        public TicketAlertUserMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public TicketAlertUserMapping Get(int id)
        {
            return Get(new TicketAlertUserMapping { Id = id });
        }

        public IEnumerable<TicketAlertUserMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCity(TicketAlertUserMapping ticketAlertUserMapping)
        {
            Delete(ticketAlertUserMapping);
        }

        public TicketAlertUserMapping SaveCity(TicketAlertUserMapping ticketAlertUserMapping)
        {
            return Save(ticketAlertUserMapping);
        }

        public IEnumerable<TicketAlertUserMapping> GetByTicketAlertEventMapAndEmailId(int ticketAlertEventId, string emailId)
        {
            return GetAll(s => s.Where($"{nameof(TicketAlertUserMapping.TicketAlertEventMappingId):C}=@TicketAlertEventId AND {nameof(TicketAlertUserMapping.Email):C}= @Email")
                .WithParameters(new { TicketAlertEventId = ticketAlertEventId, Email = emailId })
            );
        }
    }
}