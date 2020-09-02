using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IZoomMeetingRepository : IOrmRepository<ZoomMeeting, ZoomMeeting>
    {
        ZoomMeeting Get(long id);

        ZoomMeeting GetByEventId(long eventId);
    }

    public class ZoomMeetingRepository : BaseLongOrmRepository<ZoomMeeting>, IZoomMeetingRepository
    {
        public ZoomMeetingRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ZoomMeeting Get(long id)
        {
            return Get(new ZoomMeeting { Id = id });
        }

        public ZoomMeeting GetByEventId(long eventId)
        {
            return GetAll(statement => statement
                    .Where($"{nameof(ZoomMeeting.EventId):C} = @EventId")
                    .WithParameters(new { EventId = eventId })).FirstOrDefault();
        }
    }
}