using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingTicketRepository : IOrmRepository<CitySightSeeingTicket, CitySightSeeingTicket>
    {
        CitySightSeeingTicket Get(int id);

        CitySightSeeingTicket GetByTicketId(string ticketId);

        IEnumerable<CitySightSeeingTicket> GetByTicketIds(IEnumerable<string> ticketIds);
    }

    public class CitySightSeeingTicketRepository : BaseOrmRepository<CitySightSeeingTicket>, ICitySightSeeingTicketRepository
    {
        public CitySightSeeingTicketRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingTicket Get(int id)
        {
            return Get(new CitySightSeeingTicket { Id = id });
        }

        public CitySightSeeingTicket GetByTicketId(string ticketId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingTicket.TicketId):C} = @TicketId")
                .WithParameters(new { TicketId = ticketId })
            ).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingTicket> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCitySightSeeingTicket(CitySightSeeingTicket CitySightSeeingTicket)
        {
            Delete(CitySightSeeingTicket);
        }

        public CitySightSeeingTicket SaveCitySightSeeingTicket(CitySightSeeingTicket CitySightSeeingTicket)
        {
            return Save(CitySightSeeingTicket);
        }

        public IEnumerable<CitySightSeeingTicket> GetByTicketIds(IEnumerable<string> ticketIds)
        {
            var citySightSeeingTicketList = GetAll(statement => statement
                                  .Where($"{nameof(CitySightSeeingTicket.TicketId):C} IN @Ids")
                                  .WithParameters(new { Ids = ticketIds }));
            return citySightSeeingTicketList;
        }
    }
}