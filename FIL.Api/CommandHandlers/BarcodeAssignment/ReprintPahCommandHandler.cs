using FIL.Api.Repositories;
using FIL.Contracts.Commands.BoxOffice;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.BarcodeAssignment
{
    public class ReprintPahCommandHandler : BaseCommandHandlerWithResult<PahCommand, PahCommandResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionDetailRepository _transactionDetailRepository;
        private readonly ITransactionSeatDetailRepository _transactionSeatDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IEntryGateRepository _entryGateRepository;
        private readonly IMatchAttributeRepository _matchAttributeRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly FIL.Logging.ILogger _logger;
        private readonly IMatchLayoutSectionRepository _matchLayoutSectionRepository;

        public ReprintPahCommandHandler(
            IEventDetailRepository eventDetailRepository,
            ITransactionRepository transactionRepository,
            ITransactionDetailRepository transactionDetailRepository,
            ITransactionSeatDetailRepository transactionSeatDetailRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IEventAttributeRepository eventAttributeRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ICityRepository cityRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IEntryGateRepository entryGateRepository,
            IMatchAttributeRepository matchAttributeRepository,
            Logging.ILogger logger,
            IMatchLayoutSectionRepository matchLayoutSectionRepository,
            ITeamRepository teamRepository, IMediator mediator
           ) : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _transactionRepository = transactionRepository;
            _transactionDetailRepository = transactionDetailRepository;
            _transactionSeatDetailRepository = transactionSeatDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _eventRepository = eventRepository;
            _venueRepository = venueRepository;
            _cityRepository = cityRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _entryGateRepository = entryGateRepository;
            _matchAttributeRepository = matchAttributeRepository;
            _logger = logger;
            _matchLayoutSectionRepository = matchLayoutSectionRepository;
            _teamRepository = teamRepository;
        }

        protected override Task<ICommandResult> Handle(PahCommand command)
        {
            PahCommandResult pahCommand = new PahCommandResult();
            try
            {
                var transaction = command.TransactionAltId.HasValue
                             ? _transactionRepository.GetByAltId(command.TransactionAltId.Value)
                             : _transactionRepository.Get(command.TransactionId);
                if (transaction != null && !command.isASI)
                {
                    var matchSeatTicketDetails = _matchSeatTicketDetailRepository.GetByTransactionId(transaction.Id);
                    if (matchSeatTicketDetails == null)
                    {
                        var transactionDetailList = _transactionDetailRepository.GetByTransactionId(transaction.Id);
                        var transactionDetailModel = AutoMapper.Mapper.Map<IEnumerable<TransactionDetail>>(transactionDetailList);
                        var BarcodeNumber = "";
                        foreach (var transactionDetail in transactionDetailModel)
                        {
                            var eventTicketAttributes = _eventTicketAttributeRepository.Get(transactionDetail.EventTicketAttributeId);
                            var eventTicketDetails = _eventTicketDetailRepository.Get(eventTicketAttributes.EventTicketDetailId);
                            var eventDetails = _eventDetailRepository.Get(eventTicketDetails.EventDetailId);
                            var transactionSeatDetails = _transactionSeatDetailRepository.GetByTransactionDetailId(transactionDetail.Id);
                            var transactionSeatDeatilsList = AutoMapper.Mapper.Map<List<TransactionSeatDetail>>(transactionSeatDetails);
                            var ticketCategory = _ticketCategoryRepository.Get((int)eventTicketDetails.TicketCategoryId);

                            if (transactionSeatDeatilsList.Any())
                            {
                                foreach (var item in transactionSeatDeatilsList)
                                {
                                    Random rd = new Random();
                                    BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                    int uniqueIdLength = 4;
                                    string uniqueIdChars = "1234567890";
                                    char[] uniqueIdChar = new char[uniqueIdLength];
                                    for (int j = 0; j < uniqueIdLength; j++)
                                    {
                                        uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                    }
                                    string uniqueId = new string(uniqueIdChar);
                                    MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.Get(item.MatchSeatTicketDetailId);
                                    matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)transaction.ChannelId).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                    matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                    matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                    matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                    matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                    matchSeatTicketDetail.TicketTypeId = (TicketType)transactionDetail.TicketTypeId;
                                    matchSeatTicketDetail.ChannelId = transaction.ChannelId;
                                    _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                }
                            }
                            else
                            {
                                if (transactionDetail.TotalTickets > 0)
                                {
                                    if (ticketCategory.Id == 1063 || ticketCategory.Id == 1337 || ticketCategory.Id == 11690) // For 2 adults and 2 child
                                    {
                                        MatchSeatTicketDetail matchSeatTicketDetail = new MatchSeatTicketDetail();
                                        for (int i = 1; i <= transactionDetail.TotalTickets * 2; i++)
                                        {
                                            Random rd = new Random();
                                            BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                            int uniqueIdLength = 4;
                                            string uniqueIdChars = "1234567890";
                                            char[] uniqueIdChar = new char[uniqueIdLength];
                                            for (int j = 0; j < uniqueIdLength; j++)
                                            {
                                                uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                            }
                                            string uniqueId = new string(uniqueIdChar);
                                            matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                            matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                            matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                            matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                            matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                            matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                            matchSeatTicketDetail.TicketTypeId = TicketType.Regular;
                                            matchSeatTicketDetail.ChannelId = Channels.Website;
                                            _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                        }
                                        for (int i = 1; i <= transactionDetail.TotalTickets * 2; i++)
                                        {
                                            Random rd = new Random();
                                            BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                            int uniqueIdLength = 4;
                                            string uniqueIdChars = "1234567890";
                                            char[] uniqueIdChar = new char[uniqueIdLength];
                                            for (int j = 0; j < uniqueIdLength; j++)
                                            {
                                                uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                            }
                                            string uniqueId = new string(uniqueIdChar);
                                            matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                            matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                            matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                            matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                            matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                            matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                            matchSeatTicketDetail.TicketTypeId = TicketType.Child;
                                            matchSeatTicketDetail.ChannelId = Channels.Website;
                                            _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                        }
                                    }
                                    else if (ticketCategory.Id == 1338) //1 adult 3 child
                                    {
                                        for (int i = 1; i <= transactionDetail.TotalTickets * 1; i++)
                                        {
                                            Random rd = new Random();
                                            BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                            int uniqueIdLength = 4;
                                            string uniqueIdChars = "1234567890";
                                            char[] uniqueIdChar = new char[uniqueIdLength];
                                            for (int j = 0; j < uniqueIdLength; j++)
                                            {
                                                uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                            }
                                            string uniqueId = new string(uniqueIdChar);
                                            MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                            matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                            matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                            matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                            matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                            matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                            matchSeatTicketDetail.TicketTypeId = TicketType.Regular;
                                            matchSeatTicketDetail.ChannelId = Channels.Website;
                                            _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                        }
                                        for (int i = 1; i <= transactionDetail.TotalTickets * 3; i++)
                                        {
                                            Random rd = new Random();
                                            BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                            int uniqueIdLength = 4;
                                            string uniqueIdChars = "1234567890";
                                            char[] uniqueIdChar = new char[uniqueIdLength];
                                            for (int j = 0; j < uniqueIdLength; j++)
                                            {
                                                uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                            }
                                            string uniqueId = new string(uniqueIdChar);
                                            MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                            matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                            matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                            matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                            matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                            matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                            matchSeatTicketDetail.TicketTypeId = TicketType.Child;
                                            matchSeatTicketDetail.ChannelId = Channels.Website;
                                            _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                        }
                                    }
                                    else if (ticketCategory.Id == 1339) //2 day vip pass
                                    {
                                        for (int i = 1; i <= transactionDetail.TotalTickets * 4; i++)
                                        {
                                            Random rd = new Random();
                                            BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                            int uniqueIdLength = 4;
                                            string uniqueIdChars = "1234567890";
                                            char[] uniqueIdChar = new char[uniqueIdLength];
                                            for (int j = 0; j < uniqueIdLength; j++)
                                            {
                                                uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                            }
                                            string uniqueId = new string(uniqueIdChar);
                                            MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                            matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)Channels.Website).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                            matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                            matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                            matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                            matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                            matchSeatTicketDetail.TicketTypeId = TicketType.Regular;
                                            matchSeatTicketDetail.ChannelId = Channels.Website;
                                            _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 1; i <= transactionDetail.TotalTickets; i++)
                                        {
                                            Random rd = new Random();
                                            BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");
                                            int uniqueIdLength = 4;
                                            string uniqueIdChars = "1234567890";
                                            char[] uniqueIdChar = new char[uniqueIdLength];
                                            for (int j = 0; j < uniqueIdLength; j++)
                                            {
                                                uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                                            }
                                            string uniqueId = new string(uniqueIdChar);
                                            MatchSeatTicketDetail matchSeatTicketDetail = _matchSeatTicketDetailRepository.GetByEventTicketDetailsId(eventTicketDetails.Id);
                                            matchSeatTicketDetail.BarcodeNumber = BarcodeNumber + "0" + ((short)transaction.ChannelId).ToString() + matchSeatTicketDetail.MatchLayoutSectionSeatId + uniqueId;
                                            matchSeatTicketDetail.Price = transactionDetail.PricePerTicket;
                                            matchSeatTicketDetail.SeatStatusId = SeatStatus.Sold;
                                            matchSeatTicketDetail.TransactionId = transactionDetail.TransactionId;
                                            matchSeatTicketDetail.ModifiedBy = command.ModifiedBy;
                                            matchSeatTicketDetail.TicketTypeId = (TicketType)transactionDetail.TicketTypeId;
                                            matchSeatTicketDetail.ChannelId = transaction.ChannelId;
                                            _matchSeatTicketDetailRepository.Save(matchSeatTicketDetail);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                pahCommand.Id = transaction.Id;
                pahCommand.Success = true;
            }
            catch (Exception ex)
            {
                pahCommand.Id = -1;
                pahCommand.Success = false;
            }

            return Task.FromResult<ICommandResult>(pahCommand);
        }
    }
}