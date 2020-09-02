using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class FloatReportQueryHandler : IQueryHandler<FloatReportQuery, FloatReportQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IFloatDetailRepository _floatDetailRepository;
        private readonly IEventAttributeRepository _eventAttributeRepository;
        private readonly IBoUserVenueRepository _boUserVenueRepository;
        private readonly IEventDetailRepository _eventDetailRepository;

        public FloatReportQueryHandler(IUserRepository userRepository, IFloatDetailRepository floatDetailRepository, IBoxofficeUserAdditionalDetailRepository
            boxofficeUserAdditionalDetailRepository,
            IEventAttributeRepository eventAttributeRepository,
            IBoUserVenueRepository boUserVenueRepository,
            IEventDetailRepository eventDetailRepository
            )
        {
            _userRepository = userRepository;
            _floatDetailRepository = floatDetailRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
            _eventAttributeRepository = eventAttributeRepository;
            _boUserVenueRepository = boUserVenueRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public FloatReportQueryResult Handle(FloatReportQuery query)
        {
            List<FloatDetail> floatDetails = new List<FloatDetail>();
            List<User> usernames = new List<User>();
            var user = _userRepository.GetByAltId(query.AltId);

            var eventId = _boUserVenueRepository.GetEventByUserId(user.Id).EventId;
            var userVenues = _boUserVenueRepository.GetByUserIdAndEventId(eventId, user.Id).Select(s => s.VenueId).Distinct();
            var eventDetails = _eventDetailRepository.GetByEventIdAndVenueIds(eventId, userVenues);
            var eventAttributeList = _eventAttributeRepository.GetByEventDetailIds(eventDetails.Select(s => s.Id).Distinct()).ToList();
            int timeZone = eventAttributeList.Count > 0 ? Convert.ToInt16(eventAttributeList[0].TimeZone) : 0;

            if (user.RolesId == 9 || user.RolesId == 7)
            {
                var floatlists = _floatDetailRepository.GetByUserId(user.Id).OrderByDescending(O => O.Id);
                try
                {
                    foreach (var item in floatlists)
                    {
                        floatDetails.Add(new FloatDetail
                        {
                            Id = item.Id,
                            UserId = item.UserId,
                            CashInHand = item.CashInHand,
                            CashInHandLocal = item.CashInHandLocal,
                            IsEnabled = item.IsEnabled,
                            CreatedUtc = item.CreatedUtc.AddMinutes(timeZone),
                            CreatedBy = item.CreatedBy,
                        });
                    }
                }
                catch (Exception ex)
                {
                    floatDetails = null;
                }
                usernames = AutoMapper.Mapper.Map<List<User>>(_userRepository.GetById(query.AltId)).OrderByDescending(O => O.Id).ToList();
            }
            else
            {
                var boxOfficeUserAdditionalDetails = _boxofficeUserAdditionalDetailRepository.GetAllById(user.Id);
                //floatDetails = AutoMapper.Mapper.Map<List<FloatDetail>>(_floatDetailRepository.GetByUserIds(boxOfficeUserAdditionalDetails.Select(s => s.UserId)).OrderByDescending(O => O.Id));
                var floatlists = _floatDetailRepository.GetByUserIds(boxOfficeUserAdditionalDetails.Select(s => s.UserId)).OrderByDescending(O => O.Id);
                try
                {
                    foreach (var item in floatlists)
                    {
                        floatDetails.Add(new FloatDetail
                        {
                            Id = item.Id,
                            UserId = item.UserId,
                            CashInHand = item.CashInHand,
                            CashInHandLocal = item.CashInHandLocal,
                            IsEnabled = item.IsEnabled,
                            CreatedUtc = item.CreatedUtc.AddMinutes(timeZone),
                            CreatedBy = item.CreatedBy,
                        });
                    }
                }
                catch (Exception ex)
                {
                    floatDetails = null;
                }
                usernames = AutoMapper.Mapper.Map<List<User>>(_userRepository.GetByUserIds(floatDetails.Select(s => s.UserId))).OrderByDescending(O => O.Id).ToList();
            }
            return new FloatReportQueryResult
            {
                FloatDetails = floatDetails,
                Users = usernames
            };
        }
    }
}