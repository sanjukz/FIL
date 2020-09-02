using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.BoxOffice;
using FIL.Contracts.QueryResults.BoxOffice;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.BoxOffice
{
    public class BOCLosingReportQueryHandler : IQueryHandler<BOClosingReportQuery, BOClosingReportQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBoxofficeUserAdditionalDetailRepository _boxofficeUserAdditionalDetailRepository;
        private readonly IBoClosingDetailRepository _boClosingDetailRepository;

        public BOCLosingReportQueryHandler(IUserRepository userRepository, IFloatDetailRepository floatDetailRepository, IBoxofficeUserAdditionalDetailRepository boxofficeUserAdditionalDetailRepository, IBoClosingDetailRepository boClosingDetailRepository)
        {
            _userRepository = userRepository;
            _boClosingDetailRepository = boClosingDetailRepository;
            _boxofficeUserAdditionalDetailRepository = boxofficeUserAdditionalDetailRepository;
        }

        public BOClosingReportQueryResult Handle(BOClosingReportQuery query)
        {
            List<BOClosingDetail> closingDetails = new List<BOClosingDetail>();
            List<User> usernames = new List<User>();
            var user = _userRepository.GetByAltId(query.AltId);
            var boxOfficeUserAdditionalDetails = _boxofficeUserAdditionalDetailRepository.GetAllById(user.Id);
            closingDetails = AutoMapper.Mapper.Map<List<BOClosingDetail>>(_boClosingDetailRepository.GetByUserIds(boxOfficeUserAdditionalDetails.Select(s => s.UserId)).OrderByDescending(O => O.Id));
            usernames = AutoMapper.Mapper.Map<List<User>>(_userRepository.GetByUserIds(closingDetails.Select(s => s.UserId))).OrderByDescending(O => O.Id).ToList();

            return new BOClosingReportQueryResult
            {
                ClosingReportDetails = closingDetails,
                Users = usernames
            };
        }
    }
}