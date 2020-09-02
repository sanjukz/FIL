using FIL.Api.Repositories;
using FIL.Contracts.Commands.EventSchedule;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.EventSchedule
{
    public class SaveEventScheduleCommandHandler : BaseCommandHandler<SaveEventScheduleCommand>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IMatchAttributeRepository _matchAttributeRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketFeeDetailRepository _ticketFeeDetailRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly ITeamRepository _teamRepository;

        public SaveEventScheduleCommandHandler(IEventDetailRepository eventDetailRepository, IMediator mediator,
            IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository, IMatchAttributeRepository matchAttributeRepository,
            IEventTicketDetailRepository eventTicketDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketFeeDetailRepository ticketFeeDetailRepository, ITicketCategoryRepository ticketCategoryRepository, ITeamRepository teamRepository)
            : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _matchAttributeRepository = matchAttributeRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketFeeDetailRepository = ticketFeeDetailRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _teamRepository = teamRepository;
        }

        protected override async Task Handle(SaveEventScheduleCommand command)
        {
            foreach (var eventDetail in command.SubEventList)
            {
                var eventDetailData = new EventDetail();
                if (eventDetail.id == 0) // check for create or edit insert if 0
                {
                    eventDetailData = new FIL.Contracts.DataModels.EventDetail
                    {
                        Name = eventDetail.name,
                        EventId = eventDetail.eventId,
                        AltId = Guid.NewGuid(),
                        VenueId = eventDetail.venueId,
                        MetaDetails = null,
                        Description = eventDetail.Description,
                        GroupId = 1,
                        StartDateTime = DateTime.Parse(eventDetail.startDateTime),
                        EndDateTime = DateTime.Parse(eventDetail.endDateTime),
                        IsEnabled = false,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = command.userAltId
                    };
                    _eventDetailRepository.Save(eventDetailData);
                }
                else // update sub event
                {
                    eventDetailData = _eventDetailRepository.Get(eventDetail.id);
                    eventDetailData.Name = eventDetail.name;
                    eventDetailData.EventId = eventDetail.eventId;
                    eventDetailData.VenueId = eventDetail.venueId;
                    eventDetailData.Description = eventDetail.Description;
                    eventDetailData.StartDateTime = DateTime.Parse(eventDetail.startDateTime);
                    eventDetailData.EndDateTime = DateTime.Parse(eventDetail.endDateTime);
                    eventDetailData.ModifiedBy = command.userAltId;
                    _eventDetailRepository.Save(eventDetailData);
                }

                //check for existing delivery type details
                var deliveryTypeData = _eventDeliveryTypeDetailRepository.GetByEventDetailId(eventDetailData.Id);
                if (deliveryTypeData.Count() > 0)
                {
                    foreach (var newDeliveryType in deliveryTypeData)
                    {
                        //update existing delivery type details
                        if (Convert.ToInt16(newDeliveryType.DeliveryTypeId) != command.DeliveryValue)
                        {
                            var deliveryValue = _eventDeliveryTypeDetailRepository.Get(newDeliveryType.Id);
                            deliveryValue.DeliveryTypeId = (DeliveryTypes)command.DeliveryValue;
                            _eventDeliveryTypeDetailRepository.Save(deliveryValue);
                        }
                    }
                }
                else // insert new deliveryType
                {
                    var deliveryType = new FIL.Contracts.DataModels.EventDeliveryTypeDetail
                    {
                        EventDetailId = eventDetailData.Id,
                        DeliveryTypeId = (DeliveryTypes)command.DeliveryValue,
                        Notes = "<p><strong>Delivery<br /></strong>Ticket packages are shipped to your preferred address (signature upon receival required), or are arranged for a secure pickup location at or near the circuit.</p>",
                        EndDate = DateTime.UtcNow,
                        IsEnabled = true,
                        CreatedUtc = DateTime.UtcNow,
                        ModifiedBy = command.userAltId,
                    };
                    _eventDeliveryTypeDetailRepository.Save(deliveryType);
                }
                var matchAttributeData = _matchAttributeRepository.GetByEventDetailId(eventDetail.id);
                if (matchAttributeData.Count() > 0)
                {
                    foreach (var oldMatchAttribute in matchAttributeData)
                    {
                        foreach (var newMatchAttribute in eventDetail.matches)
                        {
                            if (oldMatchAttribute.Id == newMatchAttribute.id)
                            {
                                var updatedMatchAttribute = _matchAttributeRepository.Get(newMatchAttribute.id);
                                updatedMatchAttribute.TeamA = newMatchAttribute.teamA;
                                updatedMatchAttribute.TeamB = newMatchAttribute.teamB;
                                updatedMatchAttribute.MatchNo = newMatchAttribute.matchNo;
                                updatedMatchAttribute.MatchDay = newMatchAttribute.matchDay;
                                updatedMatchAttribute.MatchStartTime = DateTime.Parse(newMatchAttribute.startDateTime);
                                _matchAttributeRepository.Save(updatedMatchAttribute);
                            }
                        }
                    }
                }
                else
                {
                    //check for not null
                    if (eventDetail.matches.Count > 0)
                    {
                        foreach (var match in eventDetail.matches)
                        {
                            //check for if team is exits if not insert new
                            var teamA = match.teamA != 0 ? _teamRepository.Get(match.teamA) :
                                 _teamRepository.Save(
                                     new Team
                                     {
                                         Name = match.teamAName,
                                         AltId = Guid.NewGuid(),
                                         IsEnabled = true,
                                         Description = "",
                                         CreatedUtc = DateTime.UtcNow,
                                         UpdatedUtc = DateTime.UtcNow,
                                         CreatedBy = command.userAltId,
                                         UpdatedBy = command.userAltId
                                     }
                             );

                            var teamB = match.teamB != 0 ? _teamRepository.Get(match.teamB) :
                                 _teamRepository.Save(
                                     new Team
                                     {
                                         Name = match.teamBName,
                                         AltId = Guid.NewGuid(),
                                         IsEnabled = true,
                                         Description = "",
                                         CreatedUtc = DateTime.UtcNow,
                                         UpdatedUtc = DateTime.UtcNow,
                                         CreatedBy = command.userAltId,
                                         UpdatedBy = command.userAltId
                                     }
                             );

                            var matchAttribute = new FIL.Contracts.DataModels.MatchAttribute
                            {
                                EventDetailId = eventDetailData.Id,
                                TeamA = teamA.Id,
                                TeamB = teamB.Id,
                                MatchNo = match.matchNo,
                                MatchDay = match.matchDay,
                                IsEnabled = true,
                                MatchStartTime = DateTime.Parse(match.startDateTime),
                                CreatedUtc = DateTime.UtcNow,
                                CreatedBy = command.userAltId,
                                ModifiedBy = command.userAltId
                            };
                            _matchAttributeRepository.Save(matchAttribute);
                        }
                    }
                }

                var TicketDetailData = _eventTicketDetailRepository.GetByEventDetailId(eventDetail.id);
                if (TicketDetailData.Count() > 0)
                {
                    foreach (var oldTicketCategory in TicketDetailData)
                    {
                        var newEventTicketDetail = eventDetail.ticketCategories.Where(s => s.id == oldTicketCategory.Id).FirstOrDefault();
                        if (newEventTicketDetail != null)
                        {
                            //update existing eventTicketDetail
                            var updatedEventTicketDetail = _eventTicketDetailRepository.Get(newEventTicketDetail.id);
                            updatedEventTicketDetail.TicketCategoryId = newEventTicketDetail.ticketCategoryId;
                            _eventTicketDetailRepository.Save(updatedEventTicketDetail);

                            //update event ticket attributes
                            var updatedTicketAttibutesData = _eventTicketAttributeRepository.GetByEventTicketDetailId(updatedEventTicketDetail.Id);
                            updatedTicketAttibutesData.AvailableTicketForSale = newEventTicketDetail.capacity;
                            updatedTicketAttibutesData.RemainingTicketForSale = newEventTicketDetail.capacity;
                            updatedTicketAttibutesData.Price = newEventTicketDetail.price;
                            updatedTicketAttibutesData.LocalPrice = newEventTicketDetail.price;
                            updatedTicketAttibutesData.CurrencyId = newEventTicketDetail.currencyId;
                            updatedTicketAttibutesData.LocalCurrencyId = newEventTicketDetail.currencyId;
                            _eventTicketAttributeRepository.Save(updatedTicketAttibutesData);

                            //get exisiting ticket fee details
                            var oldticketFeeDetailData = _ticketFeeDetailRepository.GetAllByEventTicketAttributeId(updatedTicketAttibutesData.Id).ToList();

                            foreach (var feeType in command.FeeTypes)
                            {
                                if (feeType.id == 0)
                                {
                                    var ticketFeeDetailData = new FIL.Contracts.DataModels.TicketFeeDetail
                                    {
                                        EventTicketAttributeId = updatedTicketAttibutesData.Id,
                                        FeeId = (Int16)feeType.feeId,
                                        DisplayName = feeType.displayName,
                                        ValueTypeId = (Int16)feeType.valueTypeId,
                                        Value = feeType.value,
                                        IsEnabled = true,
                                        CreatedUtc = DateTime.UtcNow,
                                        CreatedBy = command.userAltId
                                    };
                                    _ticketFeeDetailRepository.Save(ticketFeeDetailData);
                                }
                            }

                            if (oldticketFeeDetailData.Count() > 0)
                            {
                                foreach (var oldTicketFeeDetail in oldticketFeeDetailData)
                                {
                                    var newTicketFeeDetail = command.FeeTypes.Where(s => s.id == oldTicketFeeDetail.Id).FirstOrDefault();

                                    if (newTicketFeeDetail != null)
                                    {
                                        var updatedTicketFeeDetail = _ticketFeeDetailRepository.Get(oldTicketFeeDetail.Id);
                                        updatedTicketFeeDetail.FeeId = Convert.ToInt16(newTicketFeeDetail.feeId);
                                        updatedTicketFeeDetail.ValueTypeId = Convert.ToInt16(newTicketFeeDetail.valueTypeId);
                                        updatedTicketFeeDetail.Value = newTicketFeeDetail.value;
                                        _ticketFeeDetailRepository.Save(updatedTicketFeeDetail);
                                    }
                                    else
                                    {
                                        var updatedTicketFeeDetail = _ticketFeeDetailRepository.Get(oldTicketFeeDetail.Id);
                                        updatedTicketFeeDetail.IsEnabled = false;
                                        _ticketFeeDetailRepository.Save(updatedTicketFeeDetail);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var updatedEventTicketDetail = _eventTicketDetailRepository.Get(oldTicketCategory.Id);
                            updatedEventTicketDetail.IsEnabled = false;
                            _eventTicketDetailRepository.Save(updatedEventTicketDetail);
                        }
                    }
                }

                if (eventDetail.ticketCategories.Count > 0)
                {
                    foreach (var eventTicketDetail in eventDetail.ticketCategories)
                    {
                        if (eventTicketDetail.id == 0) // insert new category
                        {
                            //check for if ticket category is exits if not insert new
                            var ticketCategory = eventTicketDetail.ticketCategoryId != 0 ? _ticketCategoryRepository.Get(eventTicketDetail.ticketCategoryId) :
                                 _ticketCategoryRepository.Save(
                                     new TicketCategory
                                     {
                                         Name = eventTicketDetail.ticketCategoryName,
                                         IsEnabled = true,
                                         CreatedUtc = DateTime.UtcNow,
                                         UpdatedUtc = DateTime.UtcNow,
                                         CreatedBy = command.userAltId,
                                         UpdatedBy = command.userAltId
                                     }
                             );
                            //insert new ticket detail data
                            var eventTicketDetailData = new FIL.Contracts.DataModels.EventTicketDetail
                            {
                                EventDetailId = eventDetailData.Id,
                                TicketCategoryId = ticketCategory.Id,
                                IsEnabled = true,
                                CreatedUtc = DateTime.UtcNow,
                                CreatedBy = command.userAltId,
                                ModifiedBy = command.userAltId,
                                IsBOEnabled = false
                            };
                            _eventTicketDetailRepository.Save(eventTicketDetailData);

                            //save event ticket attributes
                            var eventTicketAttibutesData = new FIL.Contracts.DataModels.EventTicketAttribute
                            {
                                EventTicketDetailId = eventTicketDetailData.Id,
                                CurrencyId = eventTicketDetail.currencyId,
                                TicketTypeId = FIL.Contracts.Enums.TicketType.Adult,
                                ChannelId = FIL.Contracts.Enums.Channels.Website,
                                AvailableTicketForSale = eventTicketDetail.capacity,
                                RemainingTicketForSale = eventTicketDetail.capacity,
                                Price = eventTicketDetail.price,
                                LocalPrice = eventTicketDetail.price,
                                CreatedUtc = DateTime.UtcNow,
                                CreatedBy = command.userAltId,
                                IsEnabled = true,
                                IsEMIApplicable = false,
                                IsInternationalCardAllowed = false,
                                IsSeatSelection = false,
                                ViewFromStand = "",
                                TicketCategoryDescription = "",
                                LocalCurrencyId = eventTicketDetail.currencyId,
                                SalesStartDateTime = DateTime.UtcNow,
                                SalesEndDatetime = DateTime.UtcNow,
                            };
                            _eventTicketAttributeRepository.Save(eventTicketAttibutesData);

                            if (command.FeeTypes.Count > 0)
                            {
                                foreach (var feeType in command.FeeTypes)
                                {
                                    //save ticket fee details
                                    var ticketFeeDetailData = new FIL.Contracts.DataModels.TicketFeeDetail
                                    {
                                        EventTicketAttributeId = eventTicketAttibutesData.Id,
                                        FeeId = (Int16)feeType.feeId,
                                        DisplayName = feeType.displayName,
                                        ValueTypeId = (Int16)feeType.valueTypeId,
                                        Value = feeType.value,
                                        IsEnabled = true,
                                        CreatedUtc = DateTime.UtcNow,
                                        CreatedBy = command.userAltId
                                    };
                                    _ticketFeeDetailRepository.Save(ticketFeeDetailData);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}