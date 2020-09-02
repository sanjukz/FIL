using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingTicketDetailRepository : IOrmRepository<CitySightSeeingTicketDetail, CitySightSeeingTicketDetail>
    {
        CitySightSeeingTicketDetail Get(int id);

        CitySightSeeingTicketDetail GetByTicketId(string ticketId);
    }

    public class CitySightSeeingTicketDetailRepository : BaseOrmRepository<CitySightSeeingTicketDetail>, ICitySightSeeingTicketDetailRepository
    {
        public CitySightSeeingTicketDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingTicketDetail Get(int id)
        {
            return Get(new CitySightSeeingTicketDetail { Id = id });
        }

        public IEnumerable<CitySightSeeingTicketDetail> GetAll()
        {
            return GetAll(null);
        }

        public CitySightSeeingTicketDetail GetByTicketId(string ticketId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingTicketDetail.TicketId):C} = @TicketId")
                .WithParameters(new { TicketId = ticketId })
            ).FirstOrDefault();
        }

        public void DeleteCitySightSeeingTicketDetail(CitySightSeeingTicketDetail CitySightSeeingTicketDetail)
        {
            Delete(CitySightSeeingTicketDetail);
        }

        public CitySightSeeingTicketDetail SaveCitySightSeeingTicketDetail(CitySightSeeingTicketDetail CitySightSeeingTicketDetail)
        {
            return Save(CitySightSeeingTicketDetail);
        }
    }
}