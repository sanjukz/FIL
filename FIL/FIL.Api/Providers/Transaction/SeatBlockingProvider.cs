using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.Commands.Transaction;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using FIL.Contracts.Models.TMS;
using FIL.Logging;
using System;
using System.Collections.Generic;

namespace FIL.Api.Providers.Transaction
{
    public interface ISeatBlockingProvider
    {
        CheckoutCommandResult BlockSeat(List<SeatDetail> seatData, TransactionDetail transactionDetail, FIL.Contracts.DataModels.EventTicketAttribute eventTicketAttribute, EventTicketDetail eventTicketDetail, Guid userAltId, Channels channelId);
    }

    public class SeatBlockingProvider : ISeatBlockingProvider
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IMatchSeatTicketDetailRepository _matchSeatTicketDetailRepository;
        private readonly IMatchLayoutSectionSeatRepository _matchLayoutSectionSeatRepository;
        private readonly IMatchLayoutCompanionSeatMappingRepository _masterLayoutCompanionSeatMappingRepository;
        private readonly ILogger _logger;

        public SeatBlockingProvider(

            IEventDetailRepository eventDetailRepository,
            IMatchSeatTicketDetailRepository matchSeatTicketDetailRepository,
            IMatchLayoutSectionSeatRepository matchLayoutSectionSeatRepository,
            IMatchLayoutCompanionSeatMappingRepository matchLayoutCompanionSeatMappingRepository,
            ILogger logger, ISettings settings)
        {
            _matchSeatTicketDetailRepository = matchSeatTicketDetailRepository;
            _matchLayoutSectionSeatRepository = matchLayoutSectionSeatRepository;
            _eventDetailRepository = eventDetailRepository;
            _logger = logger;
            _masterLayoutCompanionSeatMappingRepository = matchLayoutCompanionSeatMappingRepository;
        }

        public CheckoutCommandResult BlockSeat(List<SeatDetail> seatData, TransactionDetail transactionDetail, FIL.Contracts.DataModels.EventTicketAttribute eventTicketAttribute, EventTicketDetail eventTicketDetail, Guid userAltId, Channels channelId)
        {
            try
            {
                CheckoutCommandResult checkoutCommandResult = new CheckoutCommandResult();
                FIL.Contracts.DataModels.EventDetail eventDetails = _eventDetailRepository.Get(eventTicketDetail.EventDetailId);
                var BarcodeNumber = "";
                int seatCounter = 0;
                if (eventTicketDetail.InventoryTypeId == InventoryType.Seated || eventTicketDetail.InventoryTypeId == InventoryType.SeatedWithSeatSelection)
                {
                    if (seatData != null)
                    {
                        foreach (var seat in seatData)
                        {
                            FIL.Contracts.DataModels.MatchLayoutSectionSeat matchLayoutSectionSeats = _matchLayoutSectionSeatRepository.Get(seat.MatchLayoutSectionSeatId);
                            if (matchLayoutSectionSeats != null && matchLayoutSectionSeats.SeatStatusId != SeatStatus.Sold)
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
                                matchLayoutSectionSeats.Id = matchLayoutSectionSeats.Id;
                                matchLayoutSectionSeats.SeatStatusId = channelId == Channels.Corporate ? SeatStatus.BlockedforSponsor : SeatStatus.BlockedByCustomer;
                                matchLayoutSectionSeats.UpdatedUtc = DateTime.UtcNow;
                                matchLayoutSectionSeats.UpdatedBy = userAltId;
                                _matchLayoutSectionSeatRepository.Save(matchLayoutSectionSeats);

                                _matchSeatTicketDetailRepository.Save(new MatchSeatTicketDetail
                                {
                                    AltId = Guid.NewGuid(),
                                    EventTicketDetailId = eventTicketDetail.Id,
                                    MatchLayoutSectionSeatId = matchLayoutSectionSeats.Id,
                                    BarcodeNumber = BarcodeNumber + "0" + (short)channelId + matchLayoutSectionSeats.Id + uniqueId,
                                    Price = transactionDetail.PricePerTicket,
                                    TransactionId = transactionDetail.TransactionId,
                                    ModifiedBy = userAltId,
                                    TicketTypeId = (TicketType)transactionDetail.TicketTypeId,
                                    SeatStatusId = SeatStatus.Sold,
                                    PrintStatusId = PrintStatus.NotPrinted,
                                    IsEnabled = true,
                                    EntryCountAllowed = 1,
                                    ChannelId = channelId,
                                });

                                seatCounter++;
                            }
                        }
                        if (seatData.Count == seatCounter)
                        {
                            checkoutCommandResult.Success = true;
                            checkoutCommandResult.IsSeatSoldOut = false;
                        }
                        else
                        {
                            checkoutCommandResult.Success = false;
                            checkoutCommandResult.IsSeatSoldOut = true;
                        }
                    }
                    else
                    {
                        List<FIL.Contracts.DataModels.MatchLayoutSectionSeat> matchLayoutSectionSeats = AutoMapper.Mapper.Map<List<FIL.Contracts.DataModels.MatchLayoutSectionSeat>>(_matchLayoutSectionSeatRepository.GetByEventTicketDetails(eventTicketDetail.Id, transactionDetail.TotalTickets));
                        if (matchLayoutSectionSeats.Count == transactionDetail.TotalTickets)
                        {
                            foreach (var seatItem in matchLayoutSectionSeats)
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

                                if (seatItem.SeatStatusId == SeatStatus.UnSold && (seatItem.SeatTypeId == SeatType.Available || seatItem.SeatTypeId == SeatType.WheelChair))
                                {
                                    if (seatItem.SeatTypeId == SeatType.WheelChair)
                                    {
                                        var wheelChairSeat = _masterLayoutCompanionSeatMappingRepository.GetByWheelChairSeatId(seatItem.Id);
                                        if (wheelChairSeat != null)
                                        {
                                            seatItem.Id = wheelChairSeat.CompanionSeatId;
                                            seatItem.SeatStatusId = channelId == Channels.Corporate ? SeatStatus.BlockedforSponsor : SeatStatus.BlockedByCustomer;
                                            seatItem.ModifiedBy = userAltId;
                                            _matchLayoutSectionSeatRepository.Save(seatItem);

                                            _matchSeatTicketDetailRepository.Save(new MatchSeatTicketDetail
                                            {
                                                AltId = Guid.NewGuid(),
                                                EventTicketDetailId = eventTicketDetail.Id,
                                                MatchLayoutSectionSeatId = wheelChairSeat.CompanionSeatId,
                                                BarcodeNumber = BarcodeNumber + "0" + (short)channelId + wheelChairSeat.CompanionSeatId + uniqueId,
                                                Price = transactionDetail.PricePerTicket,
                                                TransactionId = transactionDetail.TransactionId,
                                                ModifiedBy = userAltId,
                                                TicketTypeId = (TicketType)transactionDetail.TicketTypeId,
                                                SeatStatusId = SeatStatus.Sold,
                                                PrintStatusId = PrintStatus.NotPrinted,
                                                IsEnabled = true,
                                                EntryCountAllowed = 1,
                                                ChannelId = channelId,
                                            });
                                        }

                                        seatItem.Id = seatItem.Id;
                                        seatItem.SeatStatusId = channelId == Channels.Corporate ? SeatStatus.BlockedforSponsor : SeatStatus.BlockedByCustomer;
                                        seatItem.ModifiedBy = userAltId;
                                        _matchLayoutSectionSeatRepository.Save(seatItem);

                                        _matchSeatTicketDetailRepository.Save(new MatchSeatTicketDetail
                                        {
                                            AltId = Guid.NewGuid(),
                                            EventTicketDetailId = eventTicketDetail.Id,
                                            MatchLayoutSectionSeatId = seatItem.Id,
                                            BarcodeNumber = BarcodeNumber + "0" + (short)channelId + seatItem.Id + uniqueId,
                                            Price = transactionDetail.PricePerTicket,
                                            TransactionId = transactionDetail.TransactionId,
                                            ModifiedBy = userAltId,
                                            TicketTypeId = (TicketType)transactionDetail.TicketTypeId,
                                            SeatStatusId = SeatStatus.Sold,
                                            PrintStatusId = PrintStatus.NotPrinted,
                                            IsEnabled = true,
                                            EntryCountAllowed = 1,
                                            ChannelId = channelId,
                                        });
                                    }
                                    else
                                    {
                                        seatItem.Id = seatItem.Id;
                                        seatItem.SeatStatusId = channelId == Channels.Corporate ? SeatStatus.BlockedforSponsor : SeatStatus.BlockedByCustomer;
                                        seatItem.ModifiedBy = userAltId;
                                        _matchLayoutSectionSeatRepository.Save(seatItem);

                                        _matchSeatTicketDetailRepository.Save(new MatchSeatTicketDetail
                                        {
                                            AltId = Guid.NewGuid(),
                                            EventTicketDetailId = eventTicketDetail.Id,
                                            MatchLayoutSectionSeatId = seatItem.Id,
                                            BarcodeNumber = BarcodeNumber + "0" + (short)channelId + seatItem.Id + uniqueId,
                                            Price = transactionDetail.PricePerTicket,
                                            TransactionId = transactionDetail.TransactionId,
                                            ModifiedBy = userAltId,
                                            TicketTypeId = (TicketType)transactionDetail.TicketTypeId,
                                            SeatStatusId = SeatStatus.Sold,
                                            PrintStatusId = PrintStatus.NotPrinted,
                                            IsEnabled = true,
                                            EntryCountAllowed = 1,
                                            ChannelId = channelId,
                                        });
                                    }
                                    seatCounter++;
                                }
                            }
                            if (seatCounter == transactionDetail.TotalTickets)
                            {
                                checkoutCommandResult.Success = true;
                                checkoutCommandResult.IsSeatSoldOut = false;
                            }
                            else
                            {
                                checkoutCommandResult.Success = false;
                                checkoutCommandResult.IsSeatSoldOut = true;
                            }
                        }
                        else
                        {
                            checkoutCommandResult.Success = false;
                            checkoutCommandResult.IsSeatSoldOut = true;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= transactionDetail.TotalTickets * 1; i++)
                    {
                        Random rd = new Random();
                        BarcodeNumber = eventDetails.StartDateTime.ToString("ddMM");

                        int uniqueIdLength = 14;
                        string uniqueIdChars = "abcdefghijklmnopqrstuvwxyz1234567890";
                        char[] uniqueIdChar = new char[uniqueIdLength];
                        for (int j = 0; j < uniqueIdLength; j++)
                        {
                            uniqueIdChar[j] = uniqueIdChars[rd.Next(0, uniqueIdChars.Length)];
                        }
                        string uniqueId = new string(uniqueIdChar);

                        _matchSeatTicketDetailRepository.Save(new MatchSeatTicketDetail
                        {
                            AltId = Guid.NewGuid(),
                            EventTicketDetailId = eventTicketDetail.Id,
                            BarcodeNumber = BarcodeNumber + "0" + (short)channelId + uniqueId,
                            Price = transactionDetail.PricePerTicket,
                            TransactionId = transactionDetail.TransactionId,
                            ModifiedBy = userAltId,
                            TicketTypeId = (TicketType)transactionDetail.TicketTypeId,
                            SeatStatusId = SeatStatus.Sold,
                            PrintStatusId = PrintStatus.NotPrinted,
                            IsEnabled = true,
                            EntryCountAllowed = 1,
                            ChannelId = channelId,
                        });
                        seatCounter++;
                    }
                    if (seatCounter == transactionDetail.TotalTickets)
                    {
                        checkoutCommandResult.Success = true;
                        checkoutCommandResult.IsSeatSoldOut = false;
                    }
                    else
                    {
                        checkoutCommandResult.Success = false;
                        checkoutCommandResult.IsSeatSoldOut = true;
                    }
                }
                return checkoutCommandResult;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return new CheckoutCommandResult
                {
                    Success = false,
                    IsSeatSoldOut = true
                };
            }
        }
    }
}