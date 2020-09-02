using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IEventGalleryImageRepository : IOrmRepository<EventGalleryImage, EventGalleryImage>
    {
        EventGalleryImage Get(int id);

        IEnumerable<EventGalleryImage> GetByEventId(long eventId);

        EventGalleryImage GetBySingleEventId(long eventId);

        EventGalleryImage SaveEventGalleryImage(EventGalleryImage eventObj);
    }

    public class EventGalleryImageRepository : BaseOrmRepository<EventGalleryImage>, IEventGalleryImageRepository
    {
        public EventGalleryImageRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public EventGalleryImage Get(int id)
        {
            return Get(new EventGalleryImage { Id = id });
        }

        public IEnumerable<EventGalleryImage> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<EventGalleryImage> GetByEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventGalleryImage.EventId):C} = @EventId AND {nameof(EventGalleryImage.IsEnabled):C} = @IsEnabled")
            .WithParameters(new { EventId = eventId, IsEnabled = 1 }));
        }

        public EventGalleryImage GetBySingleEventId(long eventId)
        {
            return GetAll(s => s.Where($"{nameof(EventGalleryImage.EventId):C} = @EventId AND {nameof(EventGalleryImage.IsEnabled):C} = @IsEnabled")
            .WithParameters(new { EventId = eventId, IsEnabled = 1 })).FirstOrDefault();
        }

        public EventGalleryImage SaveEventGalleryImage(EventGalleryImage eventObj)
        {
            return Save(eventObj);
        }
    }
}