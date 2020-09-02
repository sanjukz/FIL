using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingEventDetailMappingRepository : IOrmRepository<CitySightSeeingEventDetailMapping, CitySightSeeingEventDetailMapping>
    {
        CitySightSeeingEventDetailMapping Get(int id);

        CitySightSeeingEventDetailMapping GetByCitySightSeeingTicketId(int CitySightSeeingTicketId);

        CitySightSeeingEventDetailMapping GetByEventDetailId(long EventDetailId);

        IEnumerable<CitySightSeeingEventDetailMapping> GetAllDisabledDetails();
    }

    public class CitySightSeeingEventDetailMappingRepository : BaseOrmRepository<CitySightSeeingEventDetailMapping>, ICitySightSeeingEventDetailMappingRepository
    {
        public CitySightSeeingEventDetailMappingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingEventDetailMapping Get(int id)
        {
            return Get(new CitySightSeeingEventDetailMapping { Id = id });
        }

        public CitySightSeeingEventDetailMapping GetByCitySightSeeingTicketId(int citySightSeeingTicketId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(CitySightSeeingEventDetailMapping.CitySightSeeingTicketId):C} = @CitySightSeeingTicketId")
                    .WithParameters(new { CitySightSeeingTicketId = citySightSeeingTicketId })).FirstOrDefault();
        }

        public CitySightSeeingEventDetailMapping GetByEventDetailId(long eventDetailId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(CitySightSeeingEventDetailMapping.EventDetailId):C} = @EventDetailId")
                    .WithParameters(new { EventDetailId = eventDetailId })).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingEventDetailMapping> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<CitySightSeeingEventDetailMapping> GetAllDisabledDetails()
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingEventDetailMapping.IsEnabled):C} = 0")
                   .WithParameters(new { }));
        }

        public void DeleteCitySightSeeingEventDetailMapping(CitySightSeeingEventDetailMapping CitySightSeeingEventDetailMapping)
        {
            Delete(CitySightSeeingEventDetailMapping);
        }

        public CitySightSeeingEventDetailMapping SaveCitySightSeeingEventDetailMapping(CitySightSeeingEventDetailMapping CitySightSeeingEventDetailMapping)
        {
            return Save(CitySightSeeingEventDetailMapping);
        }
    }
}