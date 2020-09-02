using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventTicketDetailTicketCategoryTypeMappingRepository : IOrmRepository<EventTicketDetailTicketCategoryTypeMapping, EventTicketDetailTicketCategoryTypeMapping>
    {
        EventTicketDetailTicketCategoryTypeMapping Get(long id);

        IEnumerable<EventTicketDetailTicketCategoryTypeMapping> GetByEventTicketDetails(List<long> eventTicketDetails);

        EventTicketDetailTicketCategoryTypeMapping GetByEventTicketDetail(long eventTicketDetail);

        IEnumerable<EventTicketDetailTicketCategoryTypeMapping> GeAlltByEventTicketDetail(long eventTicketDetail);
    }

    public class EventTicketDetailTicketCategoryTypeMappingRepository : BaseLongOrmRepository<EventTicketDetailTicketCategoryTypeMapping>, IEventTicketDetailTicketCategoryTypeMappingRepository
    {
        public EventTicketDetailTicketCategoryTypeMappingRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public EventTicketDetailTicketCategoryTypeMapping Get(long id)
        {
            return Get(new EventTicketDetailTicketCategoryTypeMapping { Id = id });
        }

        public IEnumerable<EventTicketDetailTicketCategoryTypeMapping> GetByEventTicketDetails(List<long> eventTicketDetails)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetailTicketCategoryTypeMapping.EventTicketDetailId):C} in @EventTicketDetails")
                .WithParameters(new { EventTicketDetails = eventTicketDetails })
            );
        }

        public EventTicketDetailTicketCategoryTypeMapping GetByEventTicketDetail(long eventTicketDetail)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetailTicketCategoryTypeMapping.EventTicketDetailId):C} = @EventTicketDetails")
                .WithParameters(new { EventTicketDetails = eventTicketDetail })
            ).FirstOrDefault();
        }

        public IEnumerable<EventTicketDetailTicketCategoryTypeMapping> GeAlltByEventTicketDetail(long eventTicketDetail)
        {
            return GetAll(s => s.Where($"{nameof(EventTicketDetailTicketCategoryTypeMapping.EventTicketDetailId):C} = @EventTicketDetails")
                .WithParameters(new { EventTicketDetails = eventTicketDetail })
            );
        }
    }
}