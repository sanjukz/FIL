using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingEventTicketDetailMappingRepository : IOrmRepository<CitySightSeeingEventTicketDetailMapping, CitySightSeeingEventTicketDetailMapping>
    {
        CitySightSeeingEventTicketDetailMapping Get(int id);

        CitySightSeeingEventTicketDetailMapping GetByCitySightSeeingTicketTypeDetailId(int CitySightSeeingTicketTypeDetailId);
    }

    public class CitySightSeeingEventTicketDetailMappingRepository : BaseOrmRepository<CitySightSeeingEventTicketDetailMapping>, ICitySightSeeingEventTicketDetailMappingRepository
    {
        public CitySightSeeingEventTicketDetailMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingEventTicketDetailMapping Get(int id)
        {
            return Get(new CitySightSeeingEventTicketDetailMapping { Id = id });
        }

        public CitySightSeeingEventTicketDetailMapping GetByCitySightSeeingTicketTypeDetailId(int citySightSeeingTicketTypeDetailId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(CitySightSeeingEventTicketDetailMapping.CitySightSeeingTicketTypeDetailId):C} = @CitySightSeeingTicketTypeDetailId")
                    .WithParameters(new { CitySightSeeingTicketTypeDetailId = citySightSeeingTicketTypeDetailId })).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingEventTicketDetailMapping> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCitySightSeeingEventTicketDetailMapping(CitySightSeeingEventTicketDetailMapping CitySightSeeingEventTicketDetailMapping)
        {
            Delete(CitySightSeeingEventTicketDetailMapping);
        }

        public CitySightSeeingEventTicketDetailMapping SaveCitySightSeeingEventTicketDetailMapping(CitySightSeeingEventTicketDetailMapping CitySightSeeingEventTicketDetailMapping)
        {
            return Save(CitySightSeeingEventTicketDetailMapping);
        }
    }
}