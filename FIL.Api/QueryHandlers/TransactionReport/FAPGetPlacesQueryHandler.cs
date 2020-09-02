using FIL.Api.Repositories;
using FIL.Contracts.Queries.TransactionReport;
using FIL.Contracts.QueryResults.TransactionReport;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.TicketAlert
{
    public class FAPGetPlacesQueryHandler : IQueryHandler<FAPGetPlacesQuery, FAPGetPlacesQueryResult>
    {
        private readonly IReportingRepository _reportingRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventsUserMappingRepository _eventsUserMappingRepository;
        private readonly ILogger _logger;

        public FAPGetPlacesQueryHandler(IReportingRepository reportingRepository,
            IUserRepository userRepository,
            IEventsUserMappingRepository eventsUserMappingRepository,
            ILogger logger,
            IEventRepository eventRepository)
        {
            _reportingRepository = reportingRepository;
            _userRepository = userRepository;
            _logger = logger;
            _eventRepository = eventRepository;
            _eventsUserMappingRepository = eventsUserMappingRepository;
        }

        public FAPGetPlacesQueryResult Handle(FAPGetPlacesQuery query)
        {
            List<FIL.Contracts.Models.TransactionReport.FAPAllPlacesResponseModel> places = new List<FIL.Contracts.Models.TransactionReport.FAPAllPlacesResponseModel>();

            try
            {
                List<long> EventIds = new List<long>(); bool isFeelAdminRole = true;
                var user = _userRepository.GetByAltId(query.UserAltId);
                if (user.RolesId != 10)                   //if not super admin
                {
                    var createdEvents = _eventRepository.GetAllByUserAltId(query.UserAltId);
                    var userEvents = _eventsUserMappingRepository.GetByUserId(user.Id);
                    EventIds.AddRange(createdEvents.Select(s => s.Id));
                    EventIds.AddRange(userEvents.Select(s => s.EventId));
                    isFeelAdminRole = false;
                }
                places = _reportingRepository.GetAllReportEvents(query.isFeel, EventIds, isFeelAdminRole).ToList();
                return new FAPGetPlacesQueryResult
                {
                    Places = places
                };
            }
            catch (Exception e)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to get places for reports", e));
                return new FAPGetPlacesQueryResult
                {
                    Places = places
                };
            }
        }
    }
}