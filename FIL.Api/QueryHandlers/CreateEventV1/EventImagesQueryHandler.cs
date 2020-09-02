using FIL.Api.Repositories;
using FIL.Contracts.Queries.CreateEventV1;
using FIL.Contracts.QueryResults.CreateEventV1;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.CreateEventV1
{
    public class EventImagesQueryHandler : IQueryHandler<EventImageQuery, EventImageQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventGalleryImageRepository _eventGalleryImageRepository;

        public EventImagesQueryHandler(IEventRepository eventRepository,
            IEventGalleryImageRepository eventGalleryImageRepository)
        {
            _eventRepository = eventRepository;
            _eventGalleryImageRepository = eventGalleryImageRepository;
        }

        public EventImageQueryResult Handle(EventImageQuery query)
        {
            try
            {
                var eventImageQueryResult = new EventImageQueryResult();
                var eventImageModel = new FIL.Contracts.Models.CreateEventV1.EventImageModel();
                var currentEvent = _eventRepository.Get(query.EventId);
                var eventGallaryImage = _eventGalleryImageRepository.GetByEventId(query.EventId).FirstOrDefault();
                eventImageModel.IsBannerImage = true;
                eventImageModel.IsHotTicketImage = true;
                eventImageModel.IsPortraitImage = eventGallaryImage != null ? eventGallaryImage.IsPortraitImage != null ? eventGallaryImage.IsPortraitImage : false : false;
                eventImageModel.IsVideoUploaded = eventGallaryImage != null ? eventGallaryImage.IsVideoUploaded != null ? eventGallaryImage.IsVideoUploaded : false : false;
                eventImageModel.EventAltId = currentEvent.AltId;
                eventImageModel.EventId = currentEvent.Id;
                eventImageQueryResult.EventImageModel = eventImageModel;
                eventImageQueryResult.Success = true;
                eventImageQueryResult.IsDraft = false;
                eventImageQueryResult.IsValidLink = true;
                return eventImageQueryResult;
            }
            catch (Exception e)
            {
                return new EventImageQueryResult { };
            }
        }
    }
}