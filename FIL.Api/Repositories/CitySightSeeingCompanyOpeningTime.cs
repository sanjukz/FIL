using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingCompanyOpeningTimeRepository : IOrmRepository<CitySightSeeingCompanyOpeningTime, CitySightSeeingCompanyOpeningTime>
    {
        CitySightSeeingCompanyOpeningTime Get(int id);

        CitySightSeeingCompanyOpeningTime GetByTicketId(string ticketId);
    }

    public class CitySightSeeingCompanyOpeningTimeRepository : BaseOrmRepository<CitySightSeeingCompanyOpeningTime>, ICitySightSeeingCompanyOpeningTimeRepository
    {
        public CitySightSeeingCompanyOpeningTimeRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingCompanyOpeningTime Get(int id)
        {
            return Get(new CitySightSeeingCompanyOpeningTime { Id = id });
        }

        public IEnumerable<CitySightSeeingCompanyOpeningTime> GetAll()
        {
            return GetAll(null);
        }

        public CitySightSeeingCompanyOpeningTime GetByTicketId(string ticketId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingCompanyOpeningTime.TicketId):C} = @TicketId")
                .WithParameters(new { TicketId = ticketId })
            ).FirstOrDefault();
        }

        public void DeleteCitySightSeeingCompanyOpeningTime(CitySightSeeingCompanyOpeningTime CitySightSeeingCompanyOpeningTime)
        {
            Delete(CitySightSeeingCompanyOpeningTime);
        }

        public CitySightSeeingCompanyOpeningTime SaveCitySightSeeingCompanyOpeningTime(CitySightSeeingCompanyOpeningTime CitySightSeeingCompanyOpeningTime)
        {
            return Save(CitySightSeeingCompanyOpeningTime);
        }
    }
}