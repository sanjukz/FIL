using FIL.Api.CommandHandlers;
using FIL.Api.Events.Event.HubSpot;
using FIL.Api.Providers.ASI;
using FIL.Api.Providers.Transaction;
using FIL.Api.Repositories;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.DataModels.ASI;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using FIL.Contracts.Models.ASI;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.QueryHandlers.Transaction
{
    public class CheckoutCommandHandler : BaseCommandHandlerWithResult<CheckoutCommand, CheckoutCommandResult>
    {
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMediator _mediator;
        private readonly IASIBookingProvider _iASIBookingProvider;
        private readonly ITicketLimitProvider _ticketLimitProvider;
        private readonly IUserProvider _userProvider;
        private readonly ISaveTransactionProvider _saveTransactionProvider;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IASIMonumentTimeSlotMappingRepository _aSIMonumentTimeSlotMappingRepository;
        private readonly IEventTimeSlotMappingRepository _eventTimeSlotMappingRepository;
        private readonly IEventRepository _eventRepository;

        public CheckoutCommandHandler(
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICountryRepository countryRepository,
            IASIBookingProvider iASIBookingProvider,
            FIL.Logging.ILogger logger,
            IMediator mediator,
            ITicketLimitProvider ticketLimitProvider,
            IUserProvider userProvider,
            ISaveTransactionProvider saveTransactionProvider,
            IASIMonumentTimeSlotMappingRepository aSIMonumentTimeSlotMappingRepository,
            IEventTimeSlotMappingRepository eventTimeSlotMappingRepository,
            IEventRepository eventRepository) : base(mediator)
        {
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _countryRepository = countryRepository;
            _mediator = mediator;
            _iASIBookingProvider = iASIBookingProvider;
            _ticketLimitProvider = ticketLimitProvider;
            _saveTransactionProvider = saveTransactionProvider;
            _userProvider = userProvider;
            _logger = logger;
            _aSIMonumentTimeSlotMappingRepository = aSIMonumentTimeSlotMappingRepository;
            _eventTimeSlotMappingRepository = eventTimeSlotMappingRepository;
            _eventRepository = eventRepository;
        }

        protected override async Task<ICommandResult> Handle(CheckoutCommand command)
        {
            CheckoutCommandResult GuestCheckoutCommandResult = new CheckoutCommandResult();

            IEnumerable<long> eventTicketAttributeIds = command.EventTicketAttributeList.Select(s => s.Id).Distinct();
            IEnumerable<Contracts.Models.EventTicketAttribute> eventTicketAttributes = AutoMapper.Mapper.Map<IEnumerable<Contracts.Models.EventTicketAttribute>>(_eventTicketAttributeRepository.GetByIds(eventTicketAttributeIds));
            try
            {
                // Ticket Limit Check Provider
                var ticketLimit = _ticketLimitProvider.CheckTicketLimit(command);
                if (!ticketLimit.Success)
                {
                    return ticketLimit;
                }

                Contracts.Models.Country country = new Contracts.Models.Country();
                if (!command.IsLoginCheckout)
                {
                    country = AutoMapper.Mapper.Map<Contracts.Models.Country>(_countryRepository.GetByAltId(command.GuestUser.PhoneCode));
                }

                // User Provider
                var user = _userProvider.GetCheckoutUser(command, country);
                if (command.SiteId == Site.RASVSite)
                {
                    await _mediator.Publish(new FIL.Api.Events.Event.HubSpot.VisitorInfoEvent(user));
                }

                // Save Transaction Provider
                var transactionResult = _saveTransactionProvider.SaveTransaction(command, eventTicketAttributes, user);
                if (!transactionResult.Success && transactionResult.IsTransactionLimitExceed)
                {
                    return transactionResult;
                }

                //// TODO: XXX: HUB: This can still be called
                //// but the front end should call API when things are added to the cart (or removed)
                //// where if they are logged in, or HubspotUtk is not empty, it posts to a command to save to DB
                //// That there is a temp cart (with the hubspot vid saved).  If they buy, delete the row, if not, its checked with
                //// ticket release as well, marked as cart abandoned and then deleted.
                if (command.ISRasv)
                {
                    try
                    {
                        await _mediator.Publish(new CartInfoEvent(new Contracts.Models.CartTrack
                        {
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Phone = user.PhoneNumber,
                            EventDetailId = transactionResult.CartBookingModel,
                            TicketDateOfPurchase = transactionResult.Transaction.CreatedUtc,
                            IsMailOpt = command.GuestUser.IsMailOpt != null ? (bool)command.GuestUser.IsMailOpt : false,
                        }));
                    }
                    catch (Exception e)
                    {
                        _logger.Log(Logging.Enums.LogCategory.Error, e);
                    }
                }

                var aSIBookingResponse = new ASIBookingResponse();
                if (command.IsASI != null && (bool)command.IsASI) // IF ASI CART
                {
                    foreach (var eta in command.EventTicketAttributeList)
                    {
                        var timeSlot = _aSIMonumentTimeSlotMappingRepository.GetByASIMonumentIdAndTimeId(eta.ASIAvailability.MonumentId, eta.ASIAvailability.Id);
                        if (timeSlot == null)
                        {
                            var ASIMonumentTimeSlotMapping = new ASIMonumentTimeSlotMapping
                            {
                                ASIMonumentId = eta.ASIAvailability.MonumentId,
                                TimeSlotId = eta.ASIAvailability.Id,
                                Name = eta.ASIAvailability.Name,
                                StartTime = eta.ASIAvailability.StartTime,
                                EndTime = eta.ASIAvailability.EndTime,
                                IsEnabled = true,
                                CreatedBy = command.ModifiedBy,
                                CreatedUtc = DateTime.UtcNow,
                                ModifiedBy = command.ModifiedBy,
                                UpdatedBy = command.ModifiedBy,
                                UpdatedUtc = DateTime.UtcNow
                            };
                            _aSIMonumentTimeSlotMappingRepository.Save(ASIMonumentTimeSlotMapping);
                        }
                        var @event = _eventRepository.GetByAltId(Guid.Parse(eta.eventAltId));

                        var eventTimeSlot = _eventTimeSlotMappingRepository.GetByEventIdandTimeSlotId(@event.Id, eta.ASIAvailability.Id);
                        if (eventTimeSlot == null)
                        {
                            eventTimeSlot = _eventTimeSlotMappingRepository.Save(new Contracts.DataModels.ASI.EventTimeSlotMapping
                            {
                                EventId = @event.Id,
                                TimeSlotId = eta.ASIAvailability.Id,
                                Name = eta.ASIAvailability.Name,
                                StartTime = eta.ASIAvailability.StartTime,
                                EndTime = eta.ASIAvailability.EndTime,
                                IsEnabled = true,
                                CreatedUtc = DateTime.UtcNow,
                                UpdatedUtc = DateTime.UtcNow,
                                CreatedBy = Guid.NewGuid()
                            });
                        }
                        eta.VisitTimeId = (int?)eventTimeSlot.Id;
                    }

                    aSIBookingResponse = await _iASIBookingProvider.saveASIData(new ASIBooking
                    {
                        EventTicketAttributeList = command.EventTicketAttributeList,
                        TransactionId = transactionResult.Id,
                    });
                }
                if (transactionResult != null && transactionResult.Transaction != null && user != null)
                {
                    return new CheckoutCommandResult
                    {
                        Success = true,
                        Id = transactionResult.Id,
                        TransactionAltId = transactionResult.Transaction.AltId,
                        Transaction = transactionResult.Transaction,
                        User = AutoMapper.Mapper.Map<FIL.Contracts.Models.User>(user),
                        IsTransactionLimitExceed = false,
                        IsTicketCategorySoldOut = false,
                        IsASI = command.IsASI == null ? false : command.IsASI,
                        ASIBookingResponse = aSIBookingResponse,
                        IsPaymentByPass = transactionResult.IsPaymentByPass,
                        StripeAccount = transactionResult.StripeAccount
                    };
                }
                else
                {
                    return new CheckoutCommandResult
                    {
                        Success = false,
                        Id = 0,
                        TransactionAltId = Guid.NewGuid(),
                        IsTransactionLimitExceed = false,
                        IsTicketCategorySoldOut = false,
                        IsASI = command.IsASI == null ? false : command.IsASI,
                    };
                }
            }
            catch (Exception exception)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, exception);
                return new CheckoutCommandResult
                {
                    Success = false,
                    Id = 0,
                    TransactionAltId = Guid.NewGuid(),
                    IsTransactionLimitExceed = false,
                    IsTicketCategorySoldOut = false,
                    IsASI = command.IsASI == null ? false : command.IsASI,
                };
            }
        }
    }
}