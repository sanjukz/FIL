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
    public class ManageSponsorOrderCommandHandler : BaseCommandHandlerWithResult<ManageSponsorOrderCommand, ManageSponsorOrderCommandResult>
    {
        private readonly ICorporateOrderRequestRepository _corporateOrderRequestRepository;
        private readonly IEventTicketDetailRepository _eventTicketDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly ISponsorRepository _sponsorRepository;
        private readonly IEventSponsorMappingRepository _eventSponsorMappingRepository;
        private readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;
        private readonly ICorporateTicketAllocationDetailLogRepository _corporateTicketAllocationDetailLogRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly Logging.ILogger _logger;

        public ManageSponsorOrderCommandHandler(
        ICorporateOrderRequestRepository corporateOrderRequestRepository,
        IEventTicketDetailRepository eventTicketDetailRepository,
        IEventTicketAttributeRepository eventTicketAttributeRepository,
        ISponsorRepository sponsorRepository,
        IEventSponsorMappingRepository eventSponsorMappingRepository,
        ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository,
        ICorporateTicketAllocationDetailLogRepository corporateTicketAllocationDetailLogRepository,
        IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
        IMediator mediator, Logging.ILogger logger)
        : base(mediator)
        {
            _corporateOrderRequestRepository = corporateOrderRequestRepository;
            _eventTicketDetailRepository = eventTicketDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _sponsorRepository = sponsorRepository;
            _eventSponsorMappingRepository = eventSponsorMappingRepository;
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
            _corporateTicketAllocationDetailLogRepository = corporateTicketAllocationDetailLogRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(ManageSponsorOrderCommand command)
        {
            ManageSponsorOrderCommandResult manageSponsorOrderCommandResult = new ManageSponsorOrderCommandResult();
            try
            {
                int remainingSeats = 0;
                List<Contracts.Models.TMS.SeatDetail> seats = new List<Contracts.Models.TMS.SeatDetail>();
                CorporateTicketAllocationDetail corporateTicketAllocationDetail = new CorporateTicketAllocationDetail();
                CorporateOrderRequest corporateOrderRequests = _corporateOrderRequestRepository.Get(command.OrderId);
                if (command.IsApprove)
                {
                    var eventTicketAttributes = _eventTicketAttributeRepository.Get(corporateOrderRequests.EventTicketAttributeId);
                    var eventDetailId = _eventTicketDetailRepository.Get(eventTicketAttributes.EventTicketDetailId).EventDetailId;
                    var eventSponsorMappingResult = _eventSponsorMappingRepository.GetByEventDetailIdandSponsorId(eventDetailId, corporateOrderRequests.SponsorId).FirstOrDefault();
                    if (eventSponsorMappingResult==null) {
                        var eventSponsorMapping = new EventSponsorMapping
                        {
                            SponsorId = corporateOrderRequests.SponsorId,
                            EventDetailId = eventDetailId,
                            IsEnabled = true,
                            ModifiedBy = command.ModifiedBy
                        };
                        eventSponsorMappingResult =_eventSponsorMappingRepository.Save(eventSponsorMapping);
                    }                  
                    if (eventSponsorMappingResult.Id != 0)
                    {
                        corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsorId(corporateOrderRequests.EventTicketAttributeId, corporateOrderRequests.SponsorId);
                        if (corporateTicketAllocationDetail == null)
                        {
                            var CorporateTicketAllocationDetail = new CorporateTicketAllocationDetail
                            {
                                AltId = new Guid(),
                                EventTicketAttributeId = corporateOrderRequests.EventTicketAttributeId,
                                SponsorId = corporateOrderRequests.SponsorId,
                                Price = 0,
                                ModifiedBy = command.ModifiedBy,
                                IsEnabled = true
                            };
                            corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.Save(CorporateTicketAllocationDetail);
                        }
                        if (command.AllocationOption == AllocationOption.Block)
                        {
                            if (corporateTicketAllocationDetail != null)
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
                                                corporateOrderRequests.OrderStatusId = OrderStatus.Approve;
                                                var result = _corporateOrderRequestRepository.Save(corporateOrderRequests);
                                                if (result.Id > 0)
                                                {
                                                    manageSponsorOrderCommandResult.Id = result.Id;
                                                    manageSponsorOrderCommandResult.Success = true;
                                                    manageSponsorOrderCommandResult.Message = "Order apporved Successfully!";
                                                }
                                                else
                                                {
                                                    manageSponsorOrderCommandResult.Id = -1;
                                                    manageSponsorOrderCommandResult.Success = false;
                                                    manageSponsorOrderCommandResult.Message = "Selected seats are transfered succesfully";
                                                }
                                            }
                                            else
                                            {
                                                manageSponsorOrderCommandResult.Id = -1;
                                                manageSponsorOrderCommandResult.Success = false;
                                                manageSponsorOrderCommandResult.Message = "Only " + remainingSeats + " seats are available for sale from selected seats";
                                            }
                                        }
                                        else
                                        {
                                            manageSponsorOrderCommandResult.Id = -1;
                                            manageSponsorOrderCommandResult.Success = false;
                                            manageSponsorOrderCommandResult.Message = "Selected seats are no longer available for sale";
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
                                        corporateOrderRequests.OrderStatusId = OrderStatus.Approve;
                                        var result = _corporateOrderRequestRepository.Save(corporateOrderRequests);
                                        if (result.Id > 0)
                                        {
                                            manageSponsorOrderCommandResult.Id = result.Id;
                                            manageSponsorOrderCommandResult.Success = true;
                                            manageSponsorOrderCommandResult.Message = "Order apporved Successfully!";
                                        }
                                        else
                                        {
                                            manageSponsorOrderCommandResult.Id = -1;
                                            manageSponsorOrderCommandResult.Success = false;
                                            manageSponsorOrderCommandResult.Message = "Something went worng!";
                                        }
                                    }
                                }
                                else
                                {
                                    manageSponsorOrderCommandResult.Id = -1;
                                    manageSponsorOrderCommandResult.Success = false;
                                    manageSponsorOrderCommandResult.Message = "Only " + corporateTicketAllocationDetail.RemainingTickets + " tickets are avaialble for sale";
                                }
                            }
                            else
                            {
                                manageSponsorOrderCommandResult.Id = -1;
                                manageSponsorOrderCommandResult.Success = false;
                                manageSponsorOrderCommandResult.Message = "Something went worng!";
                            }
                            manageSponsorOrderCommandResult.SeatDetails = seats;
                        }
                        else if (command.AllocationOption == AllocationOption.Transfer)
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
                                                CorporateTicketAllocationDetail corporateTicketAllocationDetailForSponsor = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor(corporateOrderRequests.EventTicketAttributeId, corporateOrderRequests.SponsorId));
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
                                                            AllocationOptionId = AllocationOption.Transfer,
                                                            TotalTickets = 1,
                                                            Price = eventTicketAttributes.LocalPrice,
                                                            ModifiedBy = command.ModifiedBy
                                                        };
                                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                                    }
                                                }
                                                seatitem.SponsorId = corporateOrderRequests.SponsorId;
                                                _matchLayoutSectionSeatRepository.Save(seatitem);
                                                remainingSeats++;
                                            }
                                        }
                                        if (command.Quantity == remainingSeats)
                                        {
                                            corporateOrderRequests.OrderStatusId = OrderStatus.Approve;
                                            var result = _corporateOrderRequestRepository.Save(corporateOrderRequests);
                                            if (result.Id > 0)
                                            {
                                                manageSponsorOrderCommandResult.Id = result.Id;
                                                manageSponsorOrderCommandResult.Success = true;
                                                manageSponsorOrderCommandResult.Message = "Order apporved Successfully!";
                                            }
                                            else
                                            {
                                                manageSponsorOrderCommandResult.Id = -1;
                                                manageSponsorOrderCommandResult.Success = false;
                                                manageSponsorOrderCommandResult.Message = "Selected seats are transfered succesfully";
                                            }
                                        }
                                        else
                                        {
                                            manageSponsorOrderCommandResult.Id = -1;
                                            manageSponsorOrderCommandResult.Success = false;
                                            manageSponsorOrderCommandResult.Message = "Only " + remainingSeats + " are transfered and the other " + (command.Quantity - remainingSeats) + "are consumed";
                                        }
                                    }
                                    else
                                    {
                                        manageSponsorOrderCommandResult.Id = -1;
                                        manageSponsorOrderCommandResult.Success = false;
                                        manageSponsorOrderCommandResult.Message = "Selected seats are no longer available for sale";
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

                                    CorporateTicketAllocationDetail corporateTicketAllocationDetailForSponsor = AutoMapper.Mapper.Map<CorporateTicketAllocationDetail>(_corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsor(corporateOrderRequests.EventTicketAttributeId, corporateOrderRequests.SponsorId));
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
                                                AllocationOptionId = AllocationOption.Transfer,
                                                TotalTickets = (short)command.Quantity,
                                                Price = eventTicketAttributes.LocalPrice,
                                                ModifiedBy = command.ModifiedBy
                                            };
                                            _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                        }
                                    }
                                    corporateOrderRequests.OrderStatusId = OrderStatus.Approve;
                                    var result = _corporateOrderRequestRepository.Save(corporateOrderRequests);
                                    if (result.Id > 0)
                                    {
                                        manageSponsorOrderCommandResult.Id = result.Id;
                                        manageSponsorOrderCommandResult.Success = true;
                                        manageSponsorOrderCommandResult.Message = "Order apporved Successfully!";
                                    }
                                    else
                                    {
                                        manageSponsorOrderCommandResult.Id = -1;
                                        manageSponsorOrderCommandResult.Success = false;
                                        manageSponsorOrderCommandResult.Message = "Selected seats are transfered succesfully";
                                    }
                                }
                            }
                            else
                            {
                                manageSponsorOrderCommandResult.Id = -1;
                                manageSponsorOrderCommandResult.Success = false;
                                manageSponsorOrderCommandResult.Message = "Only" + corporateTicketAllocationDetail.RemainingTickets + " tickets are avaialble to transfer";
                            }
                        }
                    }
                    else {
                        manageSponsorOrderCommandResult.Id = -1;
                        manageSponsorOrderCommandResult.Success = false;
                        manageSponsorOrderCommandResult.Message = "Something went wrong";
                    }
                }
                else /* Reject order request */
                {
                    corporateOrderRequests.OrderStatusId = OrderStatus.Reject;
                    var result = _corporateOrderRequestRepository.Save(corporateOrderRequests);
                    if (result.Id > 0)
                    {
                        manageSponsorOrderCommandResult.Id = result.Id;
                        manageSponsorOrderCommandResult.Success = true;
                        manageSponsorOrderCommandResult.Message = "Order rejected Successfully!";
                    }
                    else
                    {
                        manageSponsorOrderCommandResult.Id = result.Id;
                        manageSponsorOrderCommandResult.Success = false;
                        manageSponsorOrderCommandResult.Message = "Something went worng!";
                    }
                }
                return Task.FromResult<ICommandResult>(manageSponsorOrderCommandResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);

                return Task.FromResult<ICommandResult>(manageSponsorOrderCommandResult);
            }
        }
    }
}
