using FIL.Api.Repositories;
using FIL.Contracts.Queries.FeelAdminPlaces;
using FIL.Contracts.QueryResults.FeelAdminPlaces;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.FeelAdminPlace
{
    public class FeelAdminPlaceQueryHandler : IQueryHandler<FeelAdminPlacesQuery, FeelAdminPlacesQueryResult>
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;

        public FeelAdminPlaceQueryHandler(
            IEventRepository eventRepository,
            IEventDetailRepository eventDetailRepository,
            IPlaceWeekOpenDaysRepository placeWeekOpenDaysRepository,
            IEventAttributeRepository eventAttributeRepository,
            IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _eventDetailRepository = eventDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
        }

        public FeelAdminPlacesQueryResult Handle(FeelAdminPlacesQuery query)
        {
            List<FIL.Contracts.DataModels.Event> eventList = new List<Contracts.DataModels.Event>();
            if (query.IsFeelExists)
            {
                if (_eventRepository.GetAllByUserAltId(query.UserAltId).Any())
                {
                    return new FeelAdminPlacesQueryResult { IsFeelExists = true };
                }
                else
                {
                    return new FeelAdminPlacesQueryResult { };
                }
            }
            if (query.IsMyFeel)
            {
                eventList = _eventRepository.GetAllByUserAltId(query.UserAltId).OrderByDescending(s => s.CreatedUtc).ToList();
            }
            else if (query.IsDeactivateFeels)
            {
                eventList = _eventRepository.GetAllFeels(true).Where(s => s.IsCreatedFromFeelAdmin == true).OrderByDescending(s => s.CreatedUtc).ToList();
                eventList = eventList.Take(1000).ToList();
            }
            else
            {
                eventList = _eventRepository.GetAllFeelAdminEvents().OrderByDescending(s => s.CreatedUtc).ToList();
            }
            var users = _userRepository.GetAllByAltIds((eventList.Select(s => s.CreatedBy)));
            var eventDetails = _eventDetailRepository.GetByEventIds(eventList.Select(s => s.Id));
            var eventsModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.Event>>(eventList);
            var userModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.User>>(users);
            var eventDetailModel = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.EventDetail>>(eventDetails);
            var myFeelDetails = _eventRepository.GetMyFeelDetails(eventList.Select(s => s.Id).ToList()).ToList();
            var eventAttributes = _eventAttributeRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id).ToList()).ToList();
            return new FeelAdminPlacesQueryResult
            {
                IsFeelExists = eventsModel.Any() ? true : false,
                Events = eventsModel,
                EventDetails = eventDetailModel,
                Users = userModel,
                MyFeelDetails = myFeelDetails,
                EventAttributes = eventAttributes
            };
        }
    }
}