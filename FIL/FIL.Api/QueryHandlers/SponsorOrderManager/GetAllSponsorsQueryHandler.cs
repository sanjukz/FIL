using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.SponsorOrderManager;
using FIL.Contracts.QueryResult.SponsorOrderManager;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.SponsorOrderManager
{
    public class GetAllSponsorsQueryHandler : IQueryHandler<GetAllSponsorsQuery, GetAllSponsorsQueryResult>
    {
        private readonly ISponsorRepository _sponsorRepository;

        public GetAllSponsorsQueryHandler(
            ISponsorRepository sponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
        }

        public GetAllSponsorsQueryResult Handle(GetAllSponsorsQuery query)
        {
            var SponsorList = AutoMapper.Mapper.Map<List<Sponsor>>(_sponsorRepository.GetAll());

            return new GetAllSponsorsQueryResult
            {
                sponsors = SponsorList
            };
        }
    }
}