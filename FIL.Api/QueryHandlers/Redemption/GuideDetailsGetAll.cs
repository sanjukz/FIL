using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class GuideDetailsGetAllQueryHandler : IQueryHandler<GuideDetailsGetAllQuery, GuideDetailsGetAllResult>
    {
        private readonly IGuideDetailsRepository _GuideDetailsRepository;
        private readonly IUserRepository _UserRepository;
        private readonly FIL.Logging.ILogger _logger;

        public GuideDetailsGetAllQueryHandler(IGuideDetailsRepository GuideDetailsRepository,
        IUserRepository UserRepository,
        FIL.Logging.ILogger logger)
        {
            _GuideDetailsRepository = GuideDetailsRepository;
            _UserRepository = UserRepository;
            _logger = logger;
        }

        public GuideDetailsGetAllResult Handle(GuideDetailsGetAllQuery query)
        {
            try
            {
                List<FIL.Contracts.DataModels.Redemption.GuideDetailsCustom> GuideDetails = _GuideDetailsRepository.GetRecent100GuidesByStatus(query.OrderStatusId).ToList();
                var approvedByUsers = _UserRepository.GetByAltIds(GuideDetails.Select(s => s.ApprovedBy).Where(s => s != null)).ToList();
                return new GuideDetailsGetAllResult()
                {
                    GuideDetails = GuideDetails,
                    ApprovedByUsers = approvedByUsers
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new GuideDetailsGetAllResult()
                {
                };
            }
        }
    }
}