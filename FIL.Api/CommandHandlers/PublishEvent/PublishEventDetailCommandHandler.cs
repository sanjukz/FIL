using FIL.Api.Repositories;
using FIL.Contracts.Commands.PublishEvent;
using FIL.Contracts.DataModels;
using MediatR;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.PublishEvent
{
    public class PublishEventDetailCommandHandler : BaseCommandHandler<PublishEventDetailCommand>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IFeaturedEventRepository _featuredEventRepository;

        public PublishEventDetailCommandHandler(IEventRepository eventRepository, IEventDetailRepository eventDetailRepository, IFeaturedEventRepository featuredEventRepository, IMediator mediator)
            : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventRepository = eventRepository;
            _featuredEventRepository = featuredEventRepository;
        }

        protected override async Task Handle(PublishEventDetailCommand command)
        {
            //get event to publish
            var eventModel = _eventRepository.Get(command.Event.Id);
            if (eventModel.IsEnabled != command.Event.IsEnabled)
            {
                eventModel.IsEnabled = command.Event.IsEnabled;
                eventModel.UpdatedBy = command.ModifiedBy;
                eventModel.ModifiedBy = command.ModifiedBy;
                _eventRepository.Save(eventModel);
            }
            //update all subevents
            var allSubEvents = _eventDetailRepository.GetAllByEventId(command.Event.Id);
            foreach (var item in command.SubEvents)
            {
                var currentED = allSubEvents.Where(s => s.Id == item.Id).FirstOrDefault();
                currentED.IsEnabled = item.IsEnabled;
                currentED.UpdatedBy = command.ModifiedBy;
                currentED.ModifiedBy = command.ModifiedBy;
                _eventDetailRepository.Save(currentED);
            }

            if (command.isHotTicket)
            {
                //Get event from featured events
                var featureEventModel = _featuredEventRepository.GetByEventIdAndSiteId(command.Event.Id, command.Site);

                //if item not exists insert with new sort order and site id
                if (featureEventModel == null)
                {
                    var featureEventData = new FIL.Contracts.DataModels.FeaturedEvent
                    {
                        EventId = command.Event.Id,
                        SortOrder = short.MaxValue,
                        SiteId = command.Site,
                        IsAllowedInFooter = true,
                        CreatedUtc = DateTime.UtcNow,
                        IsEnabled = true,
                        UpdatedBy = command.ModifiedBy,
                        CreatedBy = command.ModifiedBy,
                        ModifiedBy = command.ModifiedBy
                    };
                    _featuredEventRepository.Save(featureEventData);
                }
                //update existing with new sort order
                else
                {
                    var enabledModel = _featuredEventRepository.GetByEventIdAndSiteId(command.Event.Id, command.Site);
                    enabledModel.SortOrder = short.MaxValue;
                    enabledModel.IsEnabled = true;
                    enabledModel.UpdatedBy = command.ModifiedBy;
                    enabledModel.ModifiedBy = command.ModifiedBy;
                    _featuredEventRepository.Save(enabledModel);
                }

                //Get all enabled event by site from featured events
                var allEnabledEvents = _featuredEventRepository.GetBySiteIds(command.Site).OrderBy(o => o.SortOrder).ToList();
                ObservableCollection<FeaturedEvent> updatedSortOrderedEvents = new ObservableCollection<FeaturedEvent>(allEnabledEvents);
                updatedSortOrderedEvents.Move(updatedSortOrderedEvents.Count - 1, command.SortOrder - 1);

                for (var i = 0; i < updatedSortOrderedEvents.Count; i++)
                {
                    var updatedFeaturedEnvet = updatedSortOrderedEvents[i];
                    updatedFeaturedEnvet.SortOrder = (Int16)(i + 1);
                    updatedFeaturedEnvet.UpdatedBy = command.ModifiedBy;
                    updatedFeaturedEnvet.ModifiedBy = command.ModifiedBy;
                    _featuredEventRepository.Save(updatedFeaturedEnvet);
                }
            }
            else
            {
                //disable from hot ticket
                var disabledModel = _featuredEventRepository.GetByEventIdAndSiteId(command.Event.Id, command.Site);
                if (disabledModel != null)
                {
                    disabledModel.IsEnabled = false;
                    disabledModel.UpdatedBy = command.ModifiedBy;
                    disabledModel.ModifiedBy = command.ModifiedBy;
                    _featuredEventRepository.Save(disabledModel);
                }
            }
        }
    }
}