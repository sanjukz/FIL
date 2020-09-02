using FIL.Api.Repositories;
using FIL.Contracts.Models.CitySightSeeing;
using FIL.Contracts.Queries.CitySightSeeingLocation;
using FIL.Contracts.QueryResults.CitySightSeeingLocation;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.CitySearch
{
    public class GetExOzImagesQueryHandler : IQueryHandler<GetExOzImagesQuery, GetExOzImagesQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IExOzProductImageRepository _exOzProductImageRepository;

        public GetExOzImagesQueryHandler(IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IExOzProductImageRepository exOzProductImageRepository)
        {
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
            _exOzProductImageRepository = exOzProductImageRepository;
        }

        public GetExOzImagesQueryResult Handle(GetExOzImagesQuery query)
        {
            List<ExOzImageUploadModel> ExOzImageUploadModels = new List<ExOzImageUploadModel>();
            var events = _eventRepository.GetBySourceId(Contracts.Enums.EventSource.ExperienceOz).ToList().Skip(query.SkipIndex).Take(query.TakeIndex);
            var eventDetails = _eventDetailRepository.GetByEventIds(events.Select(s => s.Id));
            var expOzImages = _exOzProductImageRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id));
            foreach (var currentEvent in events)
            {
                ExOzImageUploadModel ExOzImageUploadModel = new ExOzImageUploadModel();
                List<string> imageLinks = new List<string>();
                var currentEventDetailIds = eventDetails.Where(s => s.EventId == currentEvent.Id).ToList();
                foreach (var currentEventDetailId in currentEventDetailIds)
                {
                    var currentExpOzImages = expOzImages.Where(s => s.EventDetailId == currentEventDetailId.Id).FirstOrDefault();
                    if (currentExpOzImages != null)
                    {
                        imageLinks.Add(currentExpOzImages.ImageURL);
                    }
                }
                if (imageLinks.Count == 1)
                {
                    imageLinks.Add(imageLinks.FirstOrDefault());
                    imageLinks.Add(imageLinks.FirstOrDefault());
                }
                if (imageLinks.Count == 2)
                {
                    imageLinks.Add(imageLinks.FirstOrDefault());
                }
                if (imageLinks.Count > 0)
                {
                    ExOzImageUploadModel.EventAltIds = currentEvent.AltId.ToString().ToUpper();
                    ExOzImageUploadModel.ImageLinks = imageLinks;
                    ExOzImageUploadModels.Add(ExOzImageUploadModel);
                }
            }
            return new GetExOzImagesQueryResult
            {
                ExOzImageUploadModels = ExOzImageUploadModels
            };
        }
    }
}