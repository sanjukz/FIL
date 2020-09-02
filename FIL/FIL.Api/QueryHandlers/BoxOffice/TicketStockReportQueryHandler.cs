using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class TicketStockReportQueryHandler : IQueryHandler<TicketStockReportQuery, TicketStockReportQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly ITicketStockDetailRepository _ticketStockDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public TicketStockReportQueryHandler(IUserRepository userRepository, ITicketStockDetailRepository ticketStockDetailRepository,
            IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository,
             IEventAttributeRepository eventAttributeRepository,
            IBoUserVenueRepository boUserVenueRepository,
            IEventDetailRepository eventDetailRepository)
        {
            _userRepository = userRepository;
            _ticketStockDetailRepository = ticketStockDetailRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public TicketStockReportQueryResult Handle(TicketStockReportQuery query)
        {
            List<TicketStockDetail> ticketStockDetails = new List<TicketStockDetail>();
            List<User> usernames = new List<User>();
            var user = _userRepository.GetByAltId(query.AltId);
            var eventId = _boUserVenueRepository.GetEventByUserId(user.Id).EventId;
            var userVenues = _boUserVenueRepository.GetByUserIdAndEventId(eventId, user.Id).Select(s => s.VenueId).Distinct();
            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueIds(eventId, userVenues);
            var eventAttributeList = _eventAttributeRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id).Distinct()).ToList();
            int timeZone = eventAttributeList.Count > 0 ? Convert.ToInt16(eventAttributeList[0].TimeZone) : 0;

            if (user.RolesId == 9 || user.RolesId == 7)
            {
                // ticketStockDetails = AutoMapper.Mapper.Map<List<TicketStockDetail>>(_ticketStockDetailRepository.GetByUserId(user.Id)).OrderByDescending(O => O.Id).ToList();

                var ticketStocklists = _ticketStockDetailRepository.GetByUserId(user.Id).OrderByDescending(O => O.Id).ToList();
                try
                {
                    foreach (var item in ticketStocklists)
                    {
                        ticketStockDetails.Add(new TicketStockDetail
                        {
                            Id = item.Id,
                            UserId = item.UserId,
                            TicketStockStartSrNo = item.TicketStockStartSrNo,
                            TicketStockEndSrNo = item.TicketStockEndSrNo,
                            IsEnabled = item.IsEnabled,
                            CreatedUtc = item.CreatedUtc.AddMinutes(timeZone),
                            CreatedBy = item.CreatedBy,
                        });
                    }
                }
                catch (Exception ex)
                {
                    ticketStockDetails = null;
                }

                usernames = AutoMapper.Mapper.Map<List<User>>(_userRepository.GetById(query.AltId)).OrderByDescending(O => O.Id).ToList();
            }
            else
            {
                var boxOfficeUserAdditionalDetails = _boxofficeUserAdditionalDetailRepository.GetAllById(user.Id).OrderByDescending(O => O.Id).ToList();
                // ticketStockDetails = AutoMapper.Mapper.Map<List<TicketStockDetail>>(_ticketStockDetailRepository.GetByUserIds(boxOfficeUserAdditionalDetails.Select(s => s.UserId))).OrderByDescending(O => O.Id).ToList();

                var ticketStocklists = _ticketStockDetailRepository.GetByUserIds(boxOfficeUserAdditionalDetails.Select(s => s.UserId)).OrderByDescending(O => O.Id).ToList();
                try
                {
                    foreach (var item in ticketStocklists)
                    {
                        ticketStockDetails.Add(new TicketStockDetail
                        {
                            Id = item.Id,
                            UserId = item.UserId,
                            TicketStockStartSrNo = item.TicketStockStartSrNo,
                            TicketStockEndSrNo = item.TicketStockEndSrNo,
                            IsEnabled = item.IsEnabled,
                            CreatedUtc = item.CreatedUtc.AddMinutes(timeZone),
                            CreatedBy = item.CreatedBy,
                        });
                    }
                }
                catch (Exception ex)
                {
                    ticketStockDetails = null;
                }

                usernames = AutoMapper.Mapper.Map<List<User>>(_userRepository.GetByUserIds(ticketStockDetails.Select(s => s.UserId))).OrderByDescending(O => O.Id).ToList();
            }

            return new TicketStockReportQueryResult
            {
                TicketStockDetails = ticketStockDetails,
                Users = usernames
            };
        }
    }
}