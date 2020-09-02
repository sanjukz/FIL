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
    public class SaveBarcodeCommandHandler : BaseCommandHandlerWithResult<SaveBarcodeCommand, SaveBarcodeCommandResult>
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

        public SaveBarcodeCommandHandler(

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
            ITeamRepository teamRepository, IMediator mediator) : base(mediator)
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

        protected override Task<ICommandResult> Handle(SaveBarcodeCommand command)
        {
            SaveBarcodeCommandResult assignBarcodeCommandResult = new SaveBarcodeCommandResult();
            try
            {
                var transaction = command.TransactionAltId.HasValue
                             ? _transactionRepository.GetByAltId(command.TransactionAltId.Value)
                             : _transactionRepository.Get(command.TransactionId);
                if (transaction != null)
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

                        var pahdata = GetPAHDetails(transaction.Id);
                        if (pahdata != null)
                        {
                            assignBarcodeCommandResult.Id = transaction.Id;
                            assignBarcodeCommandResult.Success = true;
                            assignBarcodeCommandResult.PAHDetail = pahdata;
                        }
                        else
                        {
                            assignBarcodeCommandResult.Id = -1;
                            assignBarcodeCommandResult.Success = false;
                        }
                    }
                    else
                    {
                        var pahdata = GetPAHDetails(transaction.Id);
                        if (pahdata != null)
                        {
                            assignBarcodeCommandResult.Id = transaction.Id;
                            assignBarcodeCommandResult.Success = true;
                            assignBarcodeCommandResult.PAHDetail = pahdata;
                        }
                        else
                        {
                            assignBarcodeCommandResult.Id = -1;
                            assignBarcodeCommandResult.Success = false;
                        }
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
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
            }

            return Task.FromResult<ICommandResult>(assignBarcodeCommandResult);
        }

        public List<PAHDetail> GetPAHDetails(long transactionId)
        {
            try
            {
                var matchSeatDetailDataModels =
                     _matchSeatTicketDetailRepository.GetbyTransactionId(transactionId);
                var matchSeatTicketDetail =
                    AutoMapper.Mapper.Map<IEnumerable<MatchSeatTicketDetail>>(matchSeatDetailDataModels);

                var ticketDetails = _matchSeatTicketDetailRepository.GetbyTransactionId(transactionId)
                    .ToList();
                if (ticketDetails.Any())
                {
                    var transaction = _transactionRepository.Get((long)ticketDetails.Select(td => td.TransactionId).First());
                    var eventTicketDetails = _eventTicketDetailRepository
                        .GetByIds(ticketDetails.Select(msd => msd.EventTicketDetailId).Distinct())
                        .ToDictionary(etd => etd.Id);
                    var eventTicketAttributes =
                        _eventTicketAttributeRepository.GetByEventTicketDetailIds(eventTicketDetails.Values
                            .Select(etd => etd.Id).Distinct())
                            .ToDictionary(eta => eta.EventTicketDetailId);
                    var eventDetails = _eventDetailRepository.Get(eventTicketDetails.Values.First().EventDetailId);
                    var events = _eventRepository.Get(eventDetails.EventId);
                    var ticketCategory = _ticketCategoryRepository.GetByTicketCategoryIds(eventTicketDetails.Values
                        .Select(etd => etd.TicketCategoryId).Distinct())
                        .ToDictionary(tc => (long)tc.Id);
                    var currencyType = _currencyTypeRepository.Get(transaction.CurrencyId);
                    var transcationDetails =
                        _transactionDetailRepository
                            .GetByTransactionId(transactionId);
                    var matchAttributes =
                        AutoMapper.Mapper.Map<List<MatchAttribute>>(
                            _matchAttributeRepository.GetByEventDetailId(eventDetails.Id));
                    var venues = _venueRepository.Get(eventDetails.VenueId);
                    var cities = _cityRepository.Get(venues.CityId);
                    var eventAttributes = _eventAttributeRepository.GetByEventDetailId(eventDetails.Id);

                    // TODO: Refactor to be more dynamic
                    string match1teamA = "",
                        match1teamB = "",
                        match2teamA = "",
                        match2teamB = "",
                        match3teamA = "",
                        match3teamB = "";
                    string match1Time = "", match2Time = "", match3Time = "";
                    if (matchAttributes.Count > 1)
                    {
                        for (int i = 0; i <= matchAttributes.Count; i++)
                        {
                            if (i == 0)
                            {
                                match1teamA = _teamRepository.Get(Convert.ToInt64(matchAttributes.Select(s => s.TeamA)))
                                    .Name;
                                match1teamB = _teamRepository.Get(Convert.ToInt64(matchAttributes.Select(s => s.TeamB)))
                                    .Name;
                                match1Time = matchAttributes.Select(s => s.MatchStartTime).ToString();
                            }
                            else if (i == 1)
                            {
                                match2teamA = _teamRepository.Get(Convert.ToInt64(matchAttributes.Select(s => s.TeamA)))
                                    .Name;
                                match2teamB = _teamRepository.Get(Convert.ToInt64(matchAttributes.Select(s => s.TeamB)))
                                    .Name;
                                match2Time = matchAttributes.Select(s => s.MatchStartTime).ToString();
                            }
                            else if (i == 2)
                            {
                                match3teamA = _teamRepository.Get(Convert.ToInt64(matchAttributes.Select(s => s.TeamA)))
                                    .Name;
                                match3teamB = _teamRepository.Get(Convert.ToInt64(matchAttributes.Select(s => s.TeamB)))
                                    .Name;
                                match3Time = matchAttributes.Select(s => s.MatchStartTime).ToString();
                            }
                        }
                    }
                    else
                    {
                        match1teamA = "";
                        match1teamB = "";
                        match1Time = "";
                    }

                    var matchLayoutSectionCache = new Dictionary<int, MatchLayoutSection>();
                    MatchLayoutSection GetMatchLayoutSection(int matchLayoutSectionId)
                    {
                        if (matchLayoutSectionCache.ContainsKey(matchLayoutSectionId))
                        {
                            return matchLayoutSectionCache[matchLayoutSectionId];
                        }

                        var matchLayoutSection = _matchLayoutSectionRepository.Get(matchLayoutSectionId);
                        matchLayoutSectionCache.Add(matchLayoutSectionId, matchLayoutSection);
                        return matchLayoutSection;
                    }

                    var entryGateCache = new Dictionary<int, EntryGate>();
                    EntryGate GetEntryGate(int entryGateId)
                    {
                        if (entryGateCache.ContainsKey(entryGateId))
                        {
                            return entryGateCache[entryGateId];
                        }

                        var entryGate = _entryGateRepository.Get(entryGateId);
                        entryGateCache.Add(entryGateId, entryGate);
                        return entryGate;
                    }

                    var ticketdata = ticketDetails.Select(matchSeatdetail =>
                    {
                        var matchlayoutSectionSeats =
                            _matchLayoutSectionSeatRepository.Get((long)matchSeatdetail.MatchLayoutSectionSeatId);

                        string standName = "", levelName = "", blockName = "";
                        var matchlayoutSection = GetMatchLayoutSection(matchlayoutSectionSeats.MatchLayoutSectionId);
                        var entryGate = GetEntryGate(matchlayoutSection.EntryGateId);

                        if (matchlayoutSection.VenueLayoutAreaId == VenueLayoutArea.Block)
                        {
                            blockName = matchlayoutSection.SectionName;
                            if (matchlayoutSection.MatchLayoutSectionId != 0)
                            {
                                var tempStanddetails = GetMatchLayoutSection(matchlayoutSection.MatchLayoutSectionId);
                                if (tempStanddetails.VenueLayoutAreaId == VenueLayoutArea.Level)
                                {
                                    levelName = tempStanddetails.SectionName;
                                    if (tempStanddetails.MatchLayoutSectionId != 0)
                                    {
                                        var tempStanddetails1 = GetMatchLayoutSection(tempStanddetails.MatchLayoutSectionId);
                                        if (tempStanddetails1.VenueLayoutAreaId == VenueLayoutArea.Stand)
                                        {
                                            standName = tempStanddetails1.SectionName;
                                        }
                                    }
                                }

                                if (tempStanddetails.VenueLayoutAreaId == VenueLayoutArea.Stand)
                                {
                                    standName = tempStanddetails.SectionName;
                                }
                            }
                        }

                        if (matchlayoutSection.VenueLayoutAreaId == VenueLayoutArea.Level)
                        {
                            levelName = matchlayoutSection.SectionName;
                            if (matchlayoutSection.MatchLayoutSectionId != 0)
                            {
                                var tempStanddetails = GetMatchLayoutSection(matchlayoutSection.MatchLayoutSectionId);
                                standName = tempStanddetails.SectionName;
                            }
                        }

                        if (matchlayoutSection.VenueLayoutAreaId == VenueLayoutArea.Stand)
                        {
                            standName = matchlayoutSection.SectionName;
                        }

                        var eventTicketDetail = eventTicketDetails[matchSeatdetail.EventTicketDetailId];
                        var eventTicketAttribute = eventTicketAttributes[eventTicketDetail.Id];
                        var transactionDetail = transcationDetails.FirstOrDefault(td =>
                            td.EventTicketAttributeId == eventTicketAttribute.Id
                            && td.TicketTypeId.HasValue
                            && (TicketType)td.TicketTypeId.Value == matchSeatdetail.TicketTypeId);
                        return new PAHDetail
                        {
                            VenueId = venues.Id,
                            VenueName = venues.Name,
                            CityName = cities.Name,
                            EventId = events.Id,
                            EventName = events.Name,
                            EventDeatilId = eventDetails.Id,
                            EventDetailsName = eventDetails.Name,
                            EventStartTime = eventDetails.StartDateTime,
                            TicketHtml = eventAttributes.TicketHtml,
                            GateOpenTime = eventAttributes.GateOpenTime,
                            MatchNo = eventAttributes.MatchNo.ToString(),
                            MatchDay = eventAttributes.MatchDay.ToString(),
                            MatchAdditionalInfo = eventAttributes.MatchAdditionalInfo.ToString(),
                            SponsorOrCustomerName = transaction.FirstName + " " + transaction.LastName,
                            TicketCategoryId = ticketCategory[eventTicketDetail.TicketCategoryId].Id,
                            TicketCategoryName = ticketCategory[eventTicketDetail.TicketCategoryId].Name,
                            Price = transactionDetail?.PricePerTicket ?? (decimal)0.0,
                            BarcodeNumber = matchSeatdetail.BarcodeNumber,
                            CurrencyName = currencyType.Code,
                            StandName = standName,
                            LevelName = levelName,
                            BlockName = blockName,
                            SectionName = matchlayoutSection.SectionName,
                            GateName = entryGate.Name,
                            RowNumber = matchlayoutSectionSeats.RowNumber,
                            TicketNumber = matchlayoutSectionSeats.ColumnNumber,
                            TicketType = matchSeatdetail.TicketTypeId.ToString(),
                            IsWheelChair = 0,
                            IsSeatSelection = eventTicketAttribute.IsSeatSelection,
                            match1teamA = match1teamA,
                            match1teamB = match1teamB,
                            match2teamA = match2teamA,
                            match2teamB = match2teamB,
                            match3teamA = match3teamA,
                            match3teamB = match3teamB,
                            match1Time = match1Time,
                            match2Time = match2Time,
                            match3Time = match3Time
                        };
                    });

                    foreach (var item in matchSeatTicketDetail)
                    {
                        if (item.PrintStatusId != PrintStatus.Printed)
                        {
                            item.PrintStatusId = PrintStatus.Printed;
                            item.PrintDateTime = DateTime.UtcNow;
                            _matchSeatTicketDetailRepository.Save(item);
                        }
                    }
                    return ticketdata.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}