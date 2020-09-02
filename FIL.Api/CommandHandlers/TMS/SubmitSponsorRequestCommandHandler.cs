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
    public class SubmitSponsorRequestCommandHandler : BaseCommandHandlerWithResult<SubmitSponsorRequestCommand, SubmitSponsorRequestCommandResult>
    {
        private readonly ICorporateOrderRequestRepository _corporateOrderRequestRepository;
        private readonly ICorporateRepresentativeDetailRepository _corporateRepresentativeDetailRepository;
        private readonly IEventTicketAttributeRepository _eventTicketAttributeRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        public readonly ICorporateTicketAllocationDetailRepository _corporateTicketAllocationDetailRepository;
        public readonly ICorporateTicketAllocationDetailLogRepository _corporateTicketAllocationDetailLogRepository;
        private readonly Logging.ILogger _logger;

        public SubmitSponsorRequestCommandHandler(ICorporateTicketAllocationDetailLogRepository corporateTicketAllocationDetailLogRepository, ICorporateTicketAllocationDetailRepository corporateTicketAllocationDetailRepository, IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository, ICorporateOrderRequestRepository corporateOrderRequestRepository, IMediator mediator, ICorporateRepresentativeDetailRepository corporateRepresentativeDetailRepository, IEventTicketAttributeRepository eventTicketAttributeRepository, Logging.ILogger logger)
            : base(mediator)
        {
            _corporateOrderRequestRepository = corporateOrderRequestRepository;
            _corporateRepresentativeDetailRepository = corporateRepresentativeDetailRepository;
            _eventTicketAttributeRepository = eventTicketAttributeRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _corporateTicketAllocationDetailRepository = corporateTicketAllocationDetailRepository;
            _corporateTicketAllocationDetailLogRepository = corporateTicketAllocationDetailLogRepository;
            _logger = logger;
        }

        protected override Task<ICommandResult> Handle(SubmitSponsorRequestCommand command)
        {
            SubmitSponsorRequestCommandResult submitSponsorRequestResult = new SubmitSponsorRequestCommandResult();
            try
            {
                foreach (var ticketCategory in command.TicketCategories)
                {
                    var sponsorRequest = new CorporateOrderRequest
                    {
                        SponsorId = command.SponsorId,
                        AccountTypeId = command.AccountTypeId,
                        OrderTypeId = command.OrderTypeId,
                        EventTicketAttributeId = Convert.ToInt32(ticketCategory.EventTicketAttributeId),
                        RequestedTickets = ticketCategory.RequestedTickets,
                        AllocatedTickets = command.RolesId == 25 ? ticketCategory.RequestedTickets : 0,
                        OrderStatusId = command.RolesId == 25 ? Contracts.Enums.OrderStatus.Approve : Contracts.Enums.OrderStatus.Pending,
                        FirstName = command.FirstName,
                        LastName = command.LastName,
                        Email = command.Email,
                        PhoneCode = command.PhoneCode,
                        PhoneNumber = command.PhoneNumber,
                        IsEnabled = true,
                        ModifiedBy = command.ModifiedBy
                    };
                    _corporateOrderRequestRepository.Save(sponsorRequest);

                    if (command.IsRepresentative)
                    {
                        var representative = new CorporateRepresentativeDetail
                        {
                            FirstName = command.RepresentativeDetail.FirstName,
                            LastName = command.RepresentativeDetail.LastName,
                            Email = command.RepresentativeDetail.Email,
                            PhoneCode = command.RepresentativeDetail.PhoneCode,
                            PhoneNumber = command.RepresentativeDetail.PhoneNumber,
                            CorporateOrderRequestId = sponsorRequest.Id,
                            IsEnabled = true,
                            ModifiedBy = command.ModifiedBy
                        };
                        _corporateRepresentativeDetailRepository.Save(representative);
                    }
                    if (command.RolesId == 25)
                    {
                        int remainingSeats = 0;
                        List<Contracts.Models.TMS.SeatDetail> seats = new List<Contracts.Models.TMS.SeatDetail>();
                        CorporateTicketAllocationDetail corporateTicketAllocationDetail = new CorporateTicketAllocationDetail();

                        var eventTicketAttributes = _eventTicketAttributeRepository.Get(sponsorRequest.EventTicketAttributeId);
                        corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.GetByEventTicketAttributeIdandSponsorId(sponsorRequest.EventTicketAttributeId, sponsorRequest.SponsorId);
                        if (corporateTicketAllocationDetail == null)
                        {
                            var CorporateTicketAllocationDetail = new CorporateTicketAllocationDetail
                            {
                                AltId = new Guid(),
                                EventTicketAttributeId = sponsorRequest.EventTicketAttributeId,
                                SponsorId = sponsorRequest.SponsorId,
                                Price = 0,
                                ModifiedBy = command.ModifiedBy,
                                IsEnabled = true
                            };
                            corporateTicketAllocationDetail = _corporateTicketAllocationDetailRepository.Save(CorporateTicketAllocationDetail);
                        }

                        if (corporateTicketAllocationDetail != null)
                        {
                            if ((eventTicketAttributes.RemainingTicketForSale + ticketCategory.RequestedTickets) >= 0)
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
                                        if (ticketCategory.RequestedTickets == remainingSeats)
                                        {
                                            submitSponsorRequestResult.Success = true;
                                            submitSponsorRequestResult.CorporateTicketAllocationId = corporateTicketAllocationDetail.Id;
                                        }
                                        else
                                        {
                                            submitSponsorRequestResult.Id = -1;
                                            submitSponsorRequestResult.Success = false;
                                            submitSponsorRequestResult.Message = "Only " + remainingSeats + " seats are available for sale from selected seats";
                                        }
                                    }
                                    else
                                    {
                                        submitSponsorRequestResult.Id = -1;
                                        submitSponsorRequestResult.Success = false;
                                        submitSponsorRequestResult.Message = "Selected seats are no longer available for sale";
                                    }
                                }
                                else
                                {
                                    eventTicketAttributes.RemainingTicketForSale -= ticketCategory.RequestedTickets;
                                    eventTicketAttributes.ModifiedBy = command.ModifiedBy;
                                    _eventTicketAttributeRepository.Save(eventTicketAttributes);
                                    corporateTicketAllocationDetail.Id = corporateTicketAllocationDetail.Id;
                                    corporateTicketAllocationDetail.AllocatedTickets += ticketCategory.RequestedTickets;
                                    corporateTicketAllocationDetail.RemainingTickets += ticketCategory.RequestedTickets;
                                    corporateTicketAllocationDetail.Price = eventTicketAttributes.LocalPrice;
                                    corporateTicketAllocationDetail.ModifiedBy = command.ModifiedBy;
                                    CorporateTicketAllocationDetail corporateTicketAllocatioresult = _corporateTicketAllocationDetailRepository.Save(corporateTicketAllocationDetail);
                                    if (corporateTicketAllocatioresult.Id > 0)
                                    {
                                        var corporateTicketAllocationDetailLog = new CorporateTicketAllocationDetailLog
                                        {
                                            CorporateTicketAllocationDetailId = corporateTicketAllocatioresult.Id,
                                            AllocationOptionId = AllocationOption.Block,
                                            TotalTickets = (short)ticketCategory.RequestedTickets,
                                            Price = eventTicketAttributes.LocalPrice,
                                            ModifiedBy = command.ModifiedBy
                                        };
                                        _corporateTicketAllocationDetailLogRepository.Save(corporateTicketAllocationDetailLog);
                                    }
                                }
                                submitSponsorRequestResult.Success = true;
                                submitSponsorRequestResult.Message = "Transaction done successfuly ";
                                submitSponsorRequestResult.CorporateTicketAllocationId = corporateTicketAllocationDetail.Id;
                            }
                            else
                            {
                                submitSponsorRequestResult.Id = -1;
                                submitSponsorRequestResult.Success = false;
                                submitSponsorRequestResult.Message = "Only " + corporateTicketAllocationDetail.RemainingTickets + " tickets are avaialble for sale";
                            }
                        }
                        else
                        {
                            submitSponsorRequestResult.Id = -1;
                            submitSponsorRequestResult.Success = false;
                            submitSponsorRequestResult.Message = "Something went worng!";
                        }
                        submitSponsorRequestResult.SeatDetails = seats;
                    }
                    submitSponsorRequestResult.Success = true;
                    submitSponsorRequestResult.Message = "Sponsor order request submitted successfully";
                }

                return Task.FromResult<ICommandResult>(submitSponsorRequestResult);
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                submitSponsorRequestResult.Id = -1;
                submitSponsorRequestResult.Success = false;
                submitSponsorRequestResult.Message = "Something went wrong";
                return Task.FromResult<ICommandResult>(submitSponsorRequestResult);
            }
        }
    }
}