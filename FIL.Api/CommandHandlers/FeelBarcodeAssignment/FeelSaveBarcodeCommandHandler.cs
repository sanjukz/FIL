using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Feel;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.FeelBarcodeAssignment
{
    public class FeelSaveBarcodeCommandHandler : BaseCommandHandlerWithResult<SaveFeelBarcodeCommand, SaveFeelBarcodeCommandResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly IFeelBarcodeMappingRepository _feelBarcodeMappingRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly ICitySightSeeingTransactionDetailRepository _citySightSeeingTransactionDetailRepository;
        private readonly ISettings _settings;

        public FeelSaveBarcodeCommandHandler(
            IEventDetailRepository eventDetailRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            IFeelBarcodeMappingRepository feelBarcodeMappingRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventAttributeRepository eventAttributeRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            IStateRepository stateRepository,
            ICountryRepository countryRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            Logging.ILogger logger,
            IMatchLayoutSectionRepository matchLayoutSectionRepository, ISettings settings,
            ITeamRepository teamRepository, IMediator mediator, ICitySightSeeingTransactionDetailRepository citySightSeeingTransactionDetailRepository) : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _feelBarcodeMappingRepository = feelBarcodeMappingRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _logger = logger;
            _teamRepository = teamRepository;
            _settings = settings;
            _citySightSeeingTransactionDetailRepository = citySightSeeingTransactionDetailRepository;
        }

        protected override async Task<ICommandResult> Handle(SaveFeelBarcodeCommand command)
        {
            SaveFeelBarcodeCommandResult assignBarcodeCommandResult = new SaveFeelBarcodeCommandResult();
            try
            {
                var transaction = command.TransactionAltId.HasValue
                             ? _transactionRepository.GetByAltId(command.TransactionAltId.Value)
                             : _transactionRepository.Get(command.TransactionId);

                var transactionDetail = _transactionDetailRepository.GetByTransactionId(command.TransactionId);
                if (transaction != null)
                {
                    var pahdata = GetPAHDetails(transaction.Id);
                    if (pahdata != null)
                    {
                        assignBarcodeCommandResult.Id = transaction.Id;
                        assignBarcodeCommandResult.Success = true;
                        assignBarcodeCommandResult.PahDetail = pahdata;
                    }
                    else
                    {
                        assignBarcodeCommandResult.Id = -1;
                        assignBarcodeCommandResult.Success = false;
                    }
                }
                else
                {
                    assignBarcodeCommandResult.Id = -1;
                    assignBarcodeCommandResult.Success = false;
                }
            }
            catch (Exception ex)
            {
                assignBarcodeCommandResult.Id = -1;
                assignBarcodeCommandResult.Success = false;
            }

            return await Task.FromResult<ICommandResult>(assignBarcodeCommandResult);
        }

        public List<PAHDetail> GetPAHDetails(long transactionId)
        {
            var transactionDetail = _transactionDetailRepository.GetByTransactionId(transactionId);

            try
            {
                var feelBarcodeMapping = _feelBarcodeMappingRepository.GetByTransactionDetailIds(transactionDetail.Select(s => s.Id)).ToList();
                List<PAHDetail> listPAHDetails = new List<PAHDetail>();
                List<CategoryWiseTickets> categoryWiseTicketsList = new List<CategoryWiseTickets>();
                long[] transactionDetailIds = new long[feelBarcodeMapping.Count];
                if (feelBarcodeMapping.Count() > 0)
                {
                    int i = 0, totalTickets = 0; decimal grptotalPrice = 0;
                    foreach (FeelBarcodeMapping currentBarcode in feelBarcodeMapping)
                    {
                        i++;
                        PAHDetail pAHDetail = new PAHDetail();
                        CategoryWiseTickets categoryWiseTickets = new CategoryWiseTickets();
                        var currentTransaction = _transactionRepository.Get(transactionId);
                        // check for hoho timeslot
                        var citySightSeeingTransaction = _citySightSeeingTransactionDetailRepository.GetByTransactionId(currentTransaction.Id);
                        if (citySightSeeingTransaction.HasTimeSlot && citySightSeeingTransaction.TimeSlot != null)
                        {
                            var formatTimeSlot = citySightSeeingTransaction.TimeSlot.Split(":");
                            pAHDetail.TimeSlot = formatTimeSlot[0] + ":" + formatTimeSlot[1];
                        }
                        var currentTransactionDetail = _transactionDetailRepository.Get(currentBarcode.TransactionDetailId);
                        var currentEventTicketAttributes = _eventTicketAttributeRepository.Get((int)currentTransactionDetail.EventTicketAttributeId);
                        var currentEventTicketDetail = _eventTicketDetailRepository.Get(currentEventTicketAttributes.EventTicketDetailId);
                        var currentTicketCategory = _ticketCategoryRepository.Get((int)currentEventTicketDetail.TicketCategoryId);
                        var currentEventDetails = _eventDetailRepository.Get(currentEventTicketDetail.EventDetailId);
                        var curretVenue = _venueRepository.Get(currentEventDetails.VenueId);
                        var currentCity = _cityRepository.Get(curretVenue.CityId);
                        var currentstate = _stateRepository.Get(currentCity.StateId);
                        var currentcountry = _countryRepository.Get(currentstate.CountryId);
                        var currentEvent = _eventRepository.Get(currentEventDetails.EventId);
                        var currentCurrencyType = _currencyTypeRepository.Get(currentEventTicketAttributes.CurrencyId);
                        var flag = true; var ticketCatName = currentTicketCategory.Name;
                        grptotalPrice += currentEventTicketAttributes.Price;
                        totalTickets += currentTransactionDetail.TotalTickets;
                        categoryWiseTickets.CategoryName = currentTicketCategory.Name;
                        categoryWiseTickets.TotalTickets = currentTransactionDetail.TotalTickets;
                        if (currentBarcode.GroupCodeExist)
                        {
                            flag = false;
                            if (!transactionDetailIds.Contains(currentBarcode.TransactionDetailId))
                            {
                                transactionDetailIds[i - 1] = currentBarcode.TransactionDetailId;
                                categoryWiseTicketsList.Add(categoryWiseTickets);
                            }
                        }
                        if (currentBarcode.GroupCodeExist && feelBarcodeMapping.Count == i)
                        {
                            flag = true;
                            ticketCatName = "Group";
                        }
                        if (flag)
                        {
                            pAHDetail.EventId = currentEvent.Id;
                            pAHDetail.EventName = currentEvent.Name;
                            pAHDetail.EventStartTime = (DateTime)currentTransactionDetail.VisitDate;
                            pAHDetail.EventsourceId = (long)(EventSource)Enum.Parse(typeof(EventSource), currentEvent.EventSourceId.ToString());
                            pAHDetail.Price = currentBarcode.GroupCodeExist ? grptotalPrice : currentEventTicketAttributes.Price;
                            pAHDetail.TicketCategoryName = ticketCatName;
                            pAHDetail.TotalTickets = currentBarcode.GroupCodeExist ? totalTickets : currentTransactionDetail.TotalTickets;
                            pAHDetail.EventDetailsName = currentEventDetails.Name;
                            pAHDetail.VenueId = curretVenue.Id;
                            pAHDetail.VenueName = curretVenue.Name;
                            pAHDetail.CityName = currentCity.Name;
                            pAHDetail.CountryName = currentcountry.Name;
                            pAHDetail.BarcodeNumber = currentBarcode.Barcode;
                            pAHDetail.CurrencyName = currentCurrencyType.Code;
                            pAHDetail.EventDeatilId = currentEventDetails.Id;
                            pAHDetail.FirstName = currentTransaction.FirstName;
                            pAHDetail.LastName = currentTransaction.LastName;
                            pAHDetail.EmailId = currentTransaction.EmailId;
                            pAHDetail.PhoneNumber = currentTransaction.PhoneNumber;
                            pAHDetail.TransactionId = currentTransaction.Id;
                            pAHDetail.CategoryWiseTickets = AutoMapper.Mapper.Map<List<CategoryWiseTickets>>(categoryWiseTicketsList);
                            listPAHDetails.Add(pAHDetail);
                        }
                    }
                    return listPAHDetails;
                }
                else
                {
                    return listPAHDetails;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, new Exception(ex.Message));
                return null;
            }
        }
    }
}