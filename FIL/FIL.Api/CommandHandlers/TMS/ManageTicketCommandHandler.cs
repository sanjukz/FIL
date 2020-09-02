using FIL.Api.Repositories;
using FIL.Contracts.Commands.TMS;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Interfaces.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Api.CommandHandlers.TMS
{
    public class ManageTicketCommandHandler : BaseCommandHandlerWithResult<ManageTicketCommand, ManageTicketCommandResult>
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;
        private readonly ICorporateTicketAllocationDetailLogRepository _corporateTicketAllocationDetailLogRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly Logging.ILogger _logger;

        public ManageTicketCommandHandler(Logging.ILogger logger,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            IEventDetailRepository eventDetailRepository,
            IEventTicketDetailRepository eventTicketDetailRepository,
            IEventTicketAttributeRepository eventTicketAttributeRepository,
            ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository,
            ICorporateTicketAllocationDetailLogRepository corporateTicketAllocationDetailLogRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IMediator mediator) : base(mediator)
        {
            _eventDetailRepository = eventDetailRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
            _corporateTicketAllocationDetailLogRepository = corporateTicketAllocationDetailLogRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(ManageTicketCommand command)
        {
            int remainingSeats = 0;
            List<Contracts.Models.TMS.SeatDetail> seats = new List<Contracts.Models.TMS.SeatDetail>();
            ManageTicketCommandResult manageTicketCommandResult = new ManageTicketCommandResult();
            try
            {
                if (command.AllocationType == AllocationType.Match)
                {
                    CorporateTicketAllocationDetail corporateTicketAllocationDetail = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByCorporateTicketAllocationDetailId((long)command.CorporateTicketAllocationDetailId));
                    if (corporateTicketAllocationDetail.Id != 0)
                    {
                        var eventTicketAttributes = _eventTicketAttributeRepository.Get(corporateTicketAllocationDetail.EventTicketAttributeId);
                        if (command.AllocationOptionId == AllocationOption.Block)
                        {
                            if ((eventTicketAttributes.RemainingTicketForSale + command.Quantity) >= 0)
                            {
                                if (command.SeatDetails != null)
                                {
                                    var seatDetails = command.SeatDetails.Where(W => W.EventTicketDetailId == eventTicketAttributes.EventTicketDetailId);
                                    var matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(seatDetails.Select(s => s.MatchLayoutSectionSeatId));
                                    if (matchLayoutSectionSeats.Any())
                                    {
                                        foreach (var seatitem in matchLayoutSectionSeats)
                                        {
                                            if (seatitem.SeatStatusId == SeatStatus.UnSold && (seatitem.SeatTypeId == SeatType.Available || seatitem.SeatTypeId == SeatType.WheelChair || seatitem.SeatTypeId == SeatType.Companion))
                                            {
                                                seatitem.SeatTypeId = SeatType.Blocked;
                                                seatitem.SponsorId = corporateTicketAllocationDetail.SponsorId;
                                                seatitem.SeatStatusId = SeatStatus.BlockedforSponsor;
                                                _matchLayoutSectionSeatRepository.Save(seatitem);

                                                eventTicketAttributes.RemainingTicketForSale -= 1;
                                                eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                                _eventTicketAttributeRepository.Save(eventTicketAttributes);

                                                corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                                corporateTicketAllocationDetail.AllocatedTickets += 1;
                                                corporateTicketAllocationDetail.RemainingTickets += 1;
                                                corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                                corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                                CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                                if (corporateTicketAllocatioresult.Id > 0)
                                                {
                                                    var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                    {
                                                        CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                                        AllocationOptionId = AllocationOption.Block,
                                                        TotalTickets = 1,
                                                        Price = eventTicketAttributes.LocalPrice,
                                                        ModifiedBy = command.ModifiedBy
                                                    };
                                                    _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                }
                                                remainingSeats++;
                                            }
                                            else
                                            {
                                                var seat = new Contracts.Models.TMS.SeatDetail
                                                {
                                                    EventTicketDetailId = (long)seatitem.EventTicketDetailId,
                                                    MatchLayoutSectionSeatId = seatitem.MatchLayoutSectionId,
                                                    SeatTag = seatitem.SeatTag,
                                                    SeatTypeId = (short)seatitem.SeatTypeId
                                                };
                                                seats.Add(seat);
                                            }
                                        }
                                        if (command.Quantity == remainingSeats)
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            manageTicketCommandResult.Message = "Selected seats are blocked succesfully";
                                        }
                                        else
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            manageTicketCommandResult.Message = "Only " + remainingSeats + " seats are available for sale from selected seats";
                                        }
                                    }
                                    else
                                    {
                                        manageTicketCommandResult.Id = -1;
                                        manageTicketCommandResult.Success = false;
                                        manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                        manageTicketCommandResult.Message = "Selected seats are no longer available for sale";
                                    }
                                }
                                else
                                {
                                    eventTicketAttributes.RemainingTicketForSale -= command.Quantity;
                                    eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                    _eventTicketAttributeRepository.Save(eventTicketAttributes);
                                    corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                    corporateTicketAllocationDetail.AllocatedTickets += command.Quantity;
                                    corporateTicketAllocationDetail.RemainingTickets += command.Quantity;
                                    corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                    corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                    CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                    if (corporateTicketAllocatioresult.Id > 0)
                                    {
                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                        {
                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                            AllocationOptionId = AllocationOption.Block,
                                            TotalTickets = (short)command.Quantity,
                                            Price = eventTicketAttributes.LocalPrice,
                                            ModifiedBy = command.ModifiedBy
                                        };
                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                    }
                                    manageTicketCommandResult.Id = corporateTicketAllocatioresult.Id;
                                    manageTicketCommandResult.Success = true;
                                    manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocatioresult.EventTicketAttributeId;
                                    manageTicketCommandResult.Message = "Tickets blocked successfully";
                                }
                            }
                            else
                            {
                                manageTicketCommandResult.Id = -1;
                                manageTicketCommandResult.Success = false;
                                manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocationDetail.EventTicketAttributeId;
                                manageTicketCommandResult.Message = "Only " + corporateTicketAllocationDetail.RemainingTickets + " tickets are avaialble for sale";
                            }
                            manageTicketCommandResult.SeatDetails = seats;
                        }
                        else if (command.AllocationOptionId == AllocationOption.Release)
                        {
                            if (corporateTicketAllocationDetail.RemainingTickets >= command.Quantity)
                            {
                                if (command.SeatDetails != null)
                                {
                                    var seatDetails = command.SeatDetails.Where(W => W.EventTicketDetailId == eventTicketAttributes.EventTicketDetailId);
                                    var matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(seatDetails.Select(s => s.MatchLayoutSectionSeatId));
                                    if (matchLayoutSectionSeats.Any())
                                    {
                                        foreach (var seatitem in matchLayoutSectionSeats)
                                        {
                                            if (seatitem.SeatStatusId == SeatStatus.UnSold && seatitem.SeatTypeId == SeatType.Blocked)
                                            {
                                                seatitem.SeatTypeId = SeatType.Available;
                                                seatitem.SponsorId = 0;
                                                seatitem.SeatStatusId = SeatStatus.UnSold;
                                                _matchLayoutSectionSeatRepository.Save(seatitem);

                                                corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                                corporateTicketAllocationDetail.AllocatedTickets -= 1;
                                                corporateTicketAllocationDetail.RemainingTickets -= 1;
                                                corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                                corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                                CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                                eventTicketAttributes.RemainingTicketForSale += 1;
                                                eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                                _eventTicketAttributeRepository.Save(eventTicketAttributes);

                                                if (corporateTicketAllocatioresult.Id > 0)
                                                {
                                                    var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                    {
                                                        CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                                        AllocationOptionId = AllocationOption.Release,
                                                        TotalTickets = 1,
                                                        Price = eventTicketAttributes.LocalPrice,
                                                        ModifiedBy = command.ModifiedBy
                                                    };
                                                    _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                }
                                                remainingSeats++;
                                            }
                                        }

                                        if (command.Quantity == remainingSeats)
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            manageTicketCommandResult.Message = "Selected seats are released succesfully";
                                        }
                                        else
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            manageTicketCommandResult.Message = "Only " + remainingSeats + " are released and the other " + (command.Quantity - remainingSeats) + "are consumed";
                                        }
                                    }
                                    else
                                    {
                                        manageTicketCommandResult.Id = -1;
                                        manageTicketCommandResult.Success = false;
                                        manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                        manageTicketCommandResult.Message = "Selected seats are no longer available to release";
                                    }
                                }
                                else
                                {
                                    corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                    corporateTicketAllocationDetail.AllocatedTickets -= command.Quantity;
                                    corporateTicketAllocationDetail.RemainingTickets -= command.Quantity;
                                    corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                    corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                    CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                    eventTicketAttributes.RemainingTicketForSale += command.Quantity;
                                    eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                    _eventTicketAttributeRepository.Save(eventTicketAttributes);

                                    if (corporateTicketAllocatioresult.Id > 0)
                                    {
                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                        {
                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                            AllocationOptionId = AllocationOption.Release,
                                            TotalTickets = (short)command.Quantity,
                                            Price = eventTicketAttributes.LocalPrice,
                                            ModifiedBy = command.ModifiedBy
                                        };
                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                    }
                                    manageTicketCommandResult.Id = corporateTicketAllocatioresult.Id;
                                    manageTicketCommandResult.Success = true;
                                    manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocatioresult.EventTicketAttributeId;
                                    manageTicketCommandResult.Message = command.Quantity + " Tickets released successfully";
                                }
                            }
                            else
                            {
                                manageTicketCommandResult.Id = -1;
                                manageTicketCommandResult.Success = false;
                                manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocationDetail.EventTicketAttributeId;
                                manageTicketCommandResult.Message = "Only" + corporateTicketAllocationDetail.RemainingTickets + " tickets are avaialble to released";
                            }
                        }
                        else if (command.AllocationOptionId == AllocationOption.Transfer)
                        {
                            if (corporateTicketAllocationDetail.RemainingTickets >= command.Quantity)
                            {
                                if (command.SeatDetails != null)
                                {
                                    var seatDetails = command.SeatDetails.Where(W => W.EventTicketDetailId == eventTicketAttributes.EventTicketDetailId);
                                    var matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(seatDetails.Select(s => s.MatchLayoutSectionSeatId));
                                    if (matchLayoutSectionSeats.Any())
                                    {
                                        foreach (var seatitem in matchLayoutSectionSeats)
                                        {
                                            if (seatitem.SeatStatusId == SeatStatus.UnSold && seatitem.SeatTypeId == SeatType.Blocked)
                                            {
                                                corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                                corporateTicketAllocationDetail.AllocatedTickets -= 1;
                                                corporateTicketAllocationDetail.RemainingTickets -= 1;
                                                corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                                corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                                CorporateTicketAllocationDetail corporateTicketAllocatioResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                                if (corporateTicketAllocatioResult.Id > 0)
                                                {
                                                    var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                    {
                                                        CorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                                        AllocationOptionId = AllocationOption.Release,
                                                        TotalTickets = (short)command.Quantity,
                                                        Price = eventTicketAttributes.LocalPrice,
                                                        ModifiedBy = command.ModifiedBy
                                                    };
                                                    _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                }
                                                CorporateTicketAllocationDetail corporateTicketAllocationDetailForSponsor = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor((long)command.EventTicketAttributeId, (long)command.TransfertoSponsorId));
                                                if (corporateTicketAllocationDetailForSponsor.Id != 0)
                                                {
                                                    corporateTicketAllocationDetailForSponsor.Id = corporateTicketAllocationDetailForSponsor.Id;
                                                    corporateTicketAllocationDetailForSponsor.AllocatedTickets += 1;
                                                    corporateTicketAllocationDetailForSponsor.RemainingTickets += 1;
                                                    corporateTicketAllocationDetailForSponsor.Price = corporateTicketAllocationDetailForSponsor.Price;
                                                    corporateTicketAllocationDetailForSponsor.ModifiedBy = command.ModifiedBy;
                                                    CorporateTicketAllocationDetail corporateTicketAllocatioTransferResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetailForSponsor);
                                                    if (corporateTicketAllocatioTransferResult.Id > 0)
                                                    {
                                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                        {
                                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioTransferResult.Id,
                                                            TransferToCorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                                            AllocationOptionId = AllocationOption.Transfer,
                                                            TotalTickets = 1,
                                                            Price = eventTicketAttributes.LocalPrice,
                                                            ModifiedBy = command.ModifiedBy
                                                        };
                                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                    }
                                                }
                                                seatitem.SponsorId = command.TransfertoSponsorId;
                                                _matchLayoutSectionSeatRepository.Save(seatitem);
                                                remainingSeats++;
                                            }
                                        }
                                        if (command.Quantity == remainingSeats)
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            manageTicketCommandResult.Message = "Selected seats are transfered succesfully";
                                        }
                                        else
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            manageTicketCommandResult.Message = "Only " + remainingSeats + " are transfered and the other " + (command.Quantity - remainingSeats) + "are consumed";
                                        }
                                    }
                                    else
                                    {
                                        manageTicketCommandResult.Id = -1;
                                        manageTicketCommandResult.Success = false;
                                        manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                        manageTicketCommandResult.Message = "Selected seats are no longer available for sale";
                                    }
                                }
                                else
                                {
                                    corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                    corporateTicketAllocationDetail.AllocatedTickets -= command.Quantity;
                                    corporateTicketAllocationDetail.RemainingTickets -= command.Quantity;
                                    corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                    corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                    CorporateTicketAllocationDetail corporateTicketAllocatioResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                    if (corporateTicketAllocatioResult.Id > 0)
                                    {
                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                        {
                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                            AllocationOptionId = AllocationOption.Release,
                                            TotalTickets = (short)command.Quantity,
                                            Price = eventTicketAttributes.LocalPrice,
                                            ModifiedBy = command.ModifiedBy
                                        };
                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                    }

                                    CorporateTicketAllocationDetail corporateTicketAllocationDetailForSponsor = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor((long)command.EventTicketAttributeId, (long)command.TransfertoSponsorId));
                                    if (corporateTicketAllocationDetailForSponsor.Id != 0)
                                    {
                                        corporateTicketAllocationDetailForSponsor.Id = corporateTicketAllocationDetailForSponsor.Id;
                                        corporateTicketAllocationDetailForSponsor.AllocatedTickets += command.Quantity;
                                        corporateTicketAllocationDetailForSponsor.RemainingTickets += command.Quantity;
                                        corporateTicketAllocationDetailForSponsor.Price = corporateTicketAllocationDetailForSponsor.Price;
                                        corporateTicketAllocationDetailForSponsor.ModifiedBy = command.ModifiedBy;
                                        CorporateTicketAllocationDetail corporateTicketAllocatioTransferResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetailForSponsor);
                                        if (corporateTicketAllocatioTransferResult.Id > 0)
                                        {
                                            var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                            {
                                                CorporateTicketAllocationDetailId = corporateTicketAllocatioTransferResult.Id,
                                                TransferToCorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                                AllocationOptionId = AllocationOption.Transfer,
                                                TotalTickets = (short)command.Quantity,
                                                Price = eventTicketAttributes.LocalPrice,
                                                ModifiedBy = command.ModifiedBy
                                            };
                                            _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                        }
                                    }

                                    manageTicketCommandResult.Id = corporateTicketAllocatioResult.Id;
                                    manageTicketCommandResult.Success = true;
                                    manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocatioResult.EventTicketAttributeId;
                                    manageTicketCommandResult.Message = command.Quantity + " Tickets transfered successfully";
                                }
                            }
                            else
                            {
                                manageTicketCommandResult.Id = -1;
                                manageTicketCommandResult.Success = false;
                                manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocationDetail.EventTicketAttributeId;
                                manageTicketCommandResult.Message = "Only" + corporateTicketAllocationDetail.RemainingTickets + " tickets are avaialble to transfer";
                            }
                        }
                    }
                    else
                    {
                        manageTicketCommandResult.Id = -1;
                        manageTicketCommandResult.Success = false;
                        manageTicketCommandResult.EventTicketAttributeId = (long)command.EventTicketAttributeId;
                        manageTicketCommandResult.Message = "Sponsor not added for the selected ticket category";
                    }
                }

                if (command.AllocationType == AllocationType.Venue)
                {
                    CorporateTicketAllocationDetail corporateTicketAllocationDetail = new CorporateTicketAllocationDetail();
                    var eventDetailList = _eventDetailRepository.GetByIds(command.EventDetailIds);
                    EventSponsorMapping eventSponsorMappingResult = new EventSponsorMapping();
                    if (eventDetailList.Any())
                    {
                        string message = string.Empty;
                        foreach (var item in eventDetailList)
                        {
                            var eventTicketDetails = _eventTicketDetailRepository.GetAllByTicketCategoryIdAndEventDetailId((long)command.TicketCategoryId, item.Id);
                            var eventTicketAttributes = _eventTicketAttributeRepository.GetByEventTicketDetailId(eventTicketDetails.Id);
                            corporateTicketAllocationDetail = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsorId(eventTicketAttributes.Id, command.SponsorId));
                            if (corporateTicketAllocationDetail == null)
                            {
                                var CorporateTicketAllocationDetail = new CorporateTicketAllocationDetail
                                {
                                    AltId = new Guid(),
                                    EventTicketAttributeId = (long)eventTicketAttributes.Id,
                                    SponsorId = command.SponsorId,
                                    Price = 0,
                                    ModifiedBy = command.ModifiedBy,
                                    IsEnabled = true
                                };
                                corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.Save(CorporateTicketAllocationDetail);
                            }
                            if (command.AllocationOptionId == AllocationOption.Block)
                            {
                                if ((eventTicketAttributes.RemainingTicketForSale + command.Quantity) >= 0)
                                {
                                    if (command.SeatDetails != null)
                                    {
                                        var seatDetails = command.SeatDetails.Where(W => W.EventTicketDetailId == eventTicketAttributes.EventTicketDetailId);
                                        var matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(seatDetails.Select(s => s.MatchLayoutSectionSeatId));
                                        if (matchLayoutSectionSeats.Any())
                                        {
                                            foreach (var seatitem in matchLayoutSectionSeats)
                                            {
                                                if (seatitem.SeatStatusId == SeatStatus.UnSold && (seatitem.SeatTypeId == SeatType.Available || seatitem.SeatTypeId == SeatType.WheelChair || seatitem.SeatTypeId == SeatType.Companion))
                                                {
                                                    seatitem.SeatTypeId = SeatType.Blocked;
                                                    seatitem.SponsorId = corporateTicketAllocationDetail.SponsorId;
                                                    seatitem.SeatStatusId = SeatStatus.BlockedforSponsor;
                                                    _matchLayoutSectionSeatRepository.Save(seatitem);

                                                    eventTicketAttributes.RemainingTicketForSale -= 1;
                                                    eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                                    _eventTicketAttributeRepository.Save(eventTicketAttributes);

                                                    corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                                    corporateTicketAllocationDetail.AllocatedTickets += 1;
                                                    corporateTicketAllocationDetail.RemainingTickets += 1;
                                                    corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                                    corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                                    CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                                    if (corporateTicketAllocatioresult.Id > 0)
                                                    {
                                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                        {
                                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                                            AllocationOptionId = AllocationOption.Block,
                                                            TotalTickets = 1,
                                                            Price = eventTicketAttributes.LocalPrice,
                                                            ModifiedBy = command.ModifiedBy
                                                        };
                                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                    }
                                                    remainingSeats++;
                                                }
                                                else
                                                {
                                                    var seat = new Contracts.Models.TMS.SeatDetail
                                                    {
                                                        EventTicketDetailId = (long)seatitem.EventTicketDetailId,
                                                        MatchLayoutSectionSeatId = seatitem.MatchLayoutSectionId,
                                                        SeatTag = seatitem.SeatTag,
                                                        SeatTypeId = (short)seatitem.SeatTypeId
                                                    };
                                                    seats.Add(seat);
                                                }
                                            }
                                            if (command.Quantity == remainingSeats)
                                            {
                                                manageTicketCommandResult.Id = -1;
                                                manageTicketCommandResult.Success = false;
                                                manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                                message += command.Quantity.ToString() + " seats are blocked successfully for " + item.Name + "\n";
                                            }
                                            else
                                            {
                                                manageTicketCommandResult.Id = -1;
                                                manageTicketCommandResult.Success = false;
                                                manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                                message += "Only" + remainingSeats + " seats are available for block from the selected seats for " + item.Name + "\n";
                                            }
                                        }
                                        else
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            message = "Selected seats are no longer available for " + item.Name + "\n";
                                        }
                                    }
                                    else
                                    {
                                        corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                        corporateTicketAllocationDetail.AllocatedTickets += command.Quantity;
                                        corporateTicketAllocationDetail.RemainingTickets += command.Quantity;
                                        corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                        corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                        CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                        if (corporateTicketAllocatioresult.Id > 0)
                                        {
                                            var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                            {
                                                CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                                AllocationOptionId = AllocationOption.Block,
                                                TotalTickets = (short)command.Quantity,
                                                Price = eventTicketAttributes.LocalPrice,
                                                ModifiedBy = command.ModifiedBy
                                            };
                                            _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                        }
                                        manageTicketCommandResult.Id = corporateTicketAllocatioresult.Id;
                                        manageTicketCommandResult.Success = true;
                                        manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocatioresult.EventTicketAttributeId;
                                        message += command.Quantity.ToString() + " tickets blocked successfully for " + item.Name + "\n";
                                    }
                                }
                                else
                                {
                                    manageTicketCommandResult.Id = -1;
                                    manageTicketCommandResult.Success = false;
                                    manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocationDetail.EventTicketAttributeId;
                                    message += "Only" + eventTicketAttributes.RemainingTicketForSale.ToString() + " tickets are availble for " + item.Name + "\n";
                                }
                            }
                            if (command.AllocationOptionId == AllocationOption.Release)
                            {
                                if (corporateTicketAllocationDetail.RemainingTickets >= command.Quantity)
                                {
                                    if (command.SeatDetails != null)
                                    {
                                        var seatDetails = command.SeatDetails.Where(W => W.EventTicketDetailId == eventTicketAttributes.EventTicketDetailId);
                                        var matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(seatDetails.Select(s => s.MatchLayoutSectionSeatId));
                                        if (matchLayoutSectionSeats.Any())
                                        {
                                            foreach (var seatitem in matchLayoutSectionSeats)
                                            {
                                                if (seatitem.SeatStatusId == SeatStatus.UnSold && seatitem.SeatTypeId == SeatType.Blocked)
                                                {
                                                    seatitem.SeatTypeId = SeatType.Available;
                                                    seatitem.SponsorId = 0;
                                                    seatitem.SeatStatusId = SeatStatus.UnSold;
                                                    _matchLayoutSectionSeatRepository.Save(seatitem);

                                                    corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                                    corporateTicketAllocationDetail.AllocatedTickets -= 1;
                                                    corporateTicketAllocationDetail.RemainingTickets -= 1;
                                                    corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                                    corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                                    CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                                    eventTicketAttributes.RemainingTicketForSale += 1;
                                                    eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                                    _eventTicketAttributeRepository.Save(eventTicketAttributes);

                                                    if (corporateTicketAllocatioresult.Id > 0)
                                                    {
                                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                        {
                                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                                            AllocationOptionId = AllocationOption.Release,
                                                            TotalTickets = 1,
                                                            Price = eventTicketAttributes.LocalPrice,
                                                            ModifiedBy = command.ModifiedBy
                                                        };
                                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                    }
                                                    remainingSeats++;
                                                }
                                            }
                                            if (command.Quantity == remainingSeats)
                                            {
                                                manageTicketCommandResult.Id = -1;
                                                manageTicketCommandResult.Success = false;
                                                manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                                message += command.Quantity.ToString() + " seats are released successfully for " + item.Name + "\n";
                                            }
                                            else
                                            {
                                                manageTicketCommandResult.Id = -1;
                                                manageTicketCommandResult.Success = false;
                                                manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                                message += "Only" + remainingSeats + " seats are released and other " + (command.Quantity - remainingSeats) + " seats are consumed for " + item.Name + "\n";
                                            }
                                        }
                                        else
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            message = "Selected seats are no longer available for " + item.Name + "\n";
                                        }
                                    }
                                    else
                                    {
                                        corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                        corporateTicketAllocationDetail.AllocatedTickets -= command.Quantity;
                                        corporateTicketAllocationDetail.RemainingTickets -= command.Quantity;
                                        corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                        corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                        CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                        eventTicketAttributes.RemainingTicketForSale += command.Quantity;
                                        eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                        _eventTicketAttributeRepository.Save(eventTicketAttributes);

                                        if (corporateTicketAllocatioresult.Id > 0)
                                        {
                                            var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                            {
                                                CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                                AllocationOptionId = AllocationOption.Release,
                                                TotalTickets = (short)command.Quantity,
                                                Price = eventTicketAttributes.LocalPrice,
                                                ModifiedBy = command.ModifiedBy
                                            };
                                            _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                        }
                                        manageTicketCommandResult.Id = corporateTicketAllocatioresult.Id;
                                        manageTicketCommandResult.Success = true;
                                        manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocatioresult.EventTicketAttributeId;
                                        message += command.Quantity.ToString() + " tickets released successfully for " + item.Name + "\n";
                                    }
                                }
                                else
                                {
                                    manageTicketCommandResult.Id = -1;
                                    manageTicketCommandResult.Success = false;
                                    manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocationDetail.EventTicketAttributeId;
                                    message += "Only" + eventTicketAttributes.RemainingTicketForSale.ToString() + " tickets are avaialble to release for " + item.Name + "\n";
                                }
                            }
                            if (command.AllocationOptionId == AllocationOption.Transfer)
                            {
                                if (corporateTicketAllocationDetail.RemainingTickets >= command.Quantity)
                                {
                                    if (command.SeatDetails != null)
                                    {
                                        var seatDetails = command.SeatDetails.Where(W => W.EventTicketDetailId == eventTicketAttributes.EventTicketDetailId);
                                        var matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.GetByIds(seatDetails.Select(s => s.MatchLayoutSectionSeatId));
                                        if (matchLayoutSectionSeats.Any())
                                        {
                                            foreach (var seatitem in matchLayoutSectionSeats)
                                            {
                                                if (seatitem.SeatStatusId == SeatStatus.UnSold && seatitem.SeatTypeId == SeatType.Blocked)
                                                {
                                                    corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                                    corporateTicketAllocationDetail.AllocatedTickets -= 1;
                                                    corporateTicketAllocationDetail.RemainingTickets -= 1;
                                                    corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                                    corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                                    CorporateTicketAllocationDetail corporateTicketAllocatioResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                                    if (corporateTicketAllocatioResult.Id > 0)
                                                    {
                                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                        {
                                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                                            AllocationOptionId = AllocationOption.Release,
                                                            TotalTickets = 1,
                                                            Price = eventTicketAttributes.LocalPrice,
                                                            ModifiedBy = command.ModifiedBy
                                                        };
                                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                    }
                                                    CorporateTicketAllocationDetail corporateTicketAllocationDetailForSponsor = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor((long)corporateTicketAllocationDetail.EventTicketAttributeId, (long)command.TransfertoSponsorId));
                                                    if (corporateTicketAllocationDetailForSponsor.Id != 0)
                                                    {
                                                        corporateTicketAllocationDetailForSponsor.Id = corporateTicketAllocationDetailForSponsor.Id;
                                                        corporateTicketAllocationDetailForSponsor.AllocatedTickets += 1;
                                                        corporateTicketAllocationDetailForSponsor.RemainingTickets += 1;
                                                        corporateTicketAllocationDetailForSponsor.Price = corporateTicketAllocationDetailForSponsor.Price;
                                                        corporateTicketAllocationDetailForSponsor.ModifiedBy = command.ModifiedBy;
                                                        CorporateTicketAllocationDetail corporateTicketAllocatioTransferResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetailForSponsor);
                                                        if (corporateTicketAllocatioTransferResult.Id > 0)
                                                        {
                                                            var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                            {
                                                                CorporateTicketAllocationDetailId = corporateTicketAllocatioTransferResult.Id,
                                                                TransferToCorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                                                AllocationOptionId = AllocationOption.Transfer,
                                                                TotalTickets = (short)command.Quantity,
                                                                Price = eventTicketAttributes.LocalPrice,
                                                                ModifiedBy = command.ModifiedBy
                                                            };
                                                            _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                        }
                                                    }
                                                    seatitem.SponsorId = command.TransfertoSponsorId;
                                                    _matchLayoutSectionSeatRepository.Save(seatitem);
                                                    remainingSeats++;
                                                }
                                            }
                                            if (command.Quantity == remainingSeats)
                                            {
                                                manageTicketCommandResult.Id = -1;
                                                manageTicketCommandResult.Success = false;
                                                manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                                message += command.Quantity.ToString() + " seats are transfered successfully for " + item.Name + "\n";
                                            }
                                            else
                                            {
                                                manageTicketCommandResult.Id = -1;
                                                manageTicketCommandResult.Success = false;
                                                manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                                message += "Only" + remainingSeats + " seats are transfered and other " + (command.Quantity - remainingSeats) + " seats are consumed for " + item.Name + "\n";
                                            }
                                        }
                                        else
                                        {
                                            manageTicketCommandResult.Id = -1;
                                            manageTicketCommandResult.Success = false;
                                            manageTicketCommandResult.EventTicketAttributeId = eventTicketAttributes.Id;
                                            message = "Selected seats are no longer available for " + item.Name + " \n";
                                        }
                                    }
                                    else
                                    {
                                        corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                        corporateTicketAllocationDetail.AllocatedTickets -= command.Quantity;
                                        corporateTicketAllocationDetail.RemainingTickets -= command.Quantity;
                                        corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                        corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                        CorporateTicketAllocationDetail corporateTicketAllocatioResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);

                                        if (corporateTicketAllocatioResult.Id > 0)
                                        {
                                            var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                            {
                                                CorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                                AllocationOptionId = AllocationOption.Release,
                                                TotalTickets = (short)command.Quantity,
                                                Price = eventTicketAttributes.LocalPrice,
                                                ModifiedBy = command.ModifiedBy
                                            };
                                            _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                        }

                                        CorporateTicketAllocationDetail corporateTicketAllocationDetailForSponsor = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor((long)corporateTicketAllocatioResult.EventTicketAttributeId, (long)command.TransfertoSponsorId));
                                        if (corporateTicketAllocationDetailForSponsor.Id != 0)
                                        {
                                            corporateTicketAllocationDetailForSponsor.Id = corporateTicketAllocationDetailForSponsor.Id;
                                            corporateTicketAllocationDetailForSponsor.AllocatedTickets += command.Quantity;
                                            corporateTicketAllocationDetailForSponsor.RemainingTickets += command.Quantity;
                                            corporateTicketAllocationDetailForSponsor.Price = corporateTicketAllocationDetailForSponsor.Price;
                                            corporateTicketAllocationDetailForSponsor.ModifiedBy = command.ModifiedBy;
                                            CorporateTicketAllocationDetail corporateTicketAllocatioTransferResult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetailForSponsor);
                                            if (corporateTicketAllocatioTransferResult.Id > 0)
                                            {
                                                var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                                {
                                                    CorporateTicketAllocationDetailId = corporateTicketAllocatioTransferResult.Id,
                                                    TransferToCorporateTicketAllocationDetailId = corporateTicketAllocatioResult.Id,
                                                    AllocationOptionId = AllocationOption.Transfer,
                                                    TotalTickets = (short)command.Quantity,
                                                    Price = eventTicketAttributes.LocalPrice,
                                                    ModifiedBy = command.ModifiedBy
                                                };
                                                _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                            }
                                        }
                                        manageTicketCommandResult.Id = corporateTicketAllocatioResult.Id;
                                        manageTicketCommandResult.Success = true;
                                        manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocatioResult.EventTicketAttributeId;
                                        message += command.Quantity.ToString() + " tickets transfered successfully for " + item.Name + "\n";
                                    }
                                }
                                else
                                {
                                    manageTicketCommandResult.Id = -1;
                                    manageTicketCommandResult.Success = false;
                                    manageTicketCommandResult.EventTicketAttributeId = corporateTicketAllocationDetail.EventTicketAttributeId;
                                    message += "Only" + command.Quantity.ToString() + " tickets are avaialble to transfer for " + item.Name + "\n";
                                }
                            }
                        }
                        manageTicketCommandResult.Message = message;
                    }
                    else
                    {
                        manageTicketCommandResult.Id = -1;
                        manageTicketCommandResult.Success = false;
                        manageTicketCommandResult.EventTicketAttributeId = (long)command.EventTicketAttributeId;
                        manageTicketCommandResult.Message = "Something went wrong with the current selection";
                    }
                }
                if (command.AllocationType == AllocationType.Sponsor)
                {
                }
                return Task.FromResult<ICommandResult>(manageTicketCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                manageTicketCommandResult.Id = -1;
                manageTicketCommandResult.Success = false;
                manageTicketCommandResult.EventTicketAttributeId = (long)command.EventTicketAttributeId;
                manageTicketCommandResult.Message = "Somethig went wrong, please try again!";
                return Task.FromResult<ICommandResult>(manageTicketCommandResult);
            }
        }
    }
}