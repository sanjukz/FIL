using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IBoUserVenueRepository : IOrmRepository<BoUserVenue, BoUserVenue>
    {
        BoUserVenue Get(long id);

        IEnumerable<BoUserVenue> GetEventsByUserId(long userId);

        BoUserVenue GetEventByUserId(long userId);

        IEnumerable<BoUserVenue> GetByUserIdAndEventId(long eventid, long userId);
    }

    public class BoUserVenueRepository : BaseLongOrmRepository<BoUserVenue>, IBoUserVenueRepository
    {
        public BoUserVenueRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public BoUserVenue Get(long id)
        {
            return Get(new BoUserVenue { Id = id });
        }

        public IEnumerable<BoUserVenue> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<BoUserVenue> GetEventsByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(BoUserVenue.UserId):C} = @UserId AND IsEnabled=1")
                .WithParameters(new { UserId = userId })
            );
        }

        public IEnumerable<BoUserVenue> GetByUserIdAndEventId(long eventId, long userId)
        {
            return GetAll(s => s.Where($"{nameof(BoUserVenue.EventId):C}=@EventId AND {nameof(BoUserVenue.UserId):C} = @UserId AND IsEnabled=1 ")
            .WithParameters(new { EventId = eventId, UserId = userId })
            );
        }

        public BoUserVenue GetEventByUserId(long userId)
        {
            return GetAll(s => s.Where($"{nameof(BoUserVenue.UserId):C} = @UserId AND IsEnabled=1")
                .WithParameters(new { UserId = userId })
            ).FirstOrDefault();
        }
    }
}