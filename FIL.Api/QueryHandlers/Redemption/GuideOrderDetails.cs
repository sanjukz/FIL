using FIL.Api.Repositories;
using FIL.Api.Repositories.Redemption;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using System;
using System.Linq;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class GuideOrderDetailsQueryHandler : IQueryHandler<GuideOrderDetailsGetAllQuery, GuideOrderDetailsQueryResult>
    {
        private readonly IGuideDetailsRepository _GuideDetailsRepository;
        private readonly IUserRepository _UserRepository;
        private readonly FIL.Logging.ILogger _logger;

        public GuideOrderDetailsQueryHandler(IGuideDetailsRepository GuideDetailsRepository,
        IUserRepository UserRepository,
        FIL.Logging.ILogger logger)
        {
            _GuideDetailsRepository = GuideDetailsRepository;
            _UserRepository = UserRepository;
            _logger = logger;
        }

        public GuideOrderDetailsQueryResult Handle(GuideOrderDetailsGetAllQuery query)
        {
            try
            {
                if ((FIL.Contracts.Enums.ApproveStatus)query.OrderStatusId == Contracts.Enums.ApproveStatus.Pending)
                {
                    var getRecent100GuideOrderDetails = _GuideDetailsRepository.GetRecent100PendingGuideOrders(query.UserId, query.OrderStatusId, query.RolesId);
                    var approvedByUsers = _UserRepository.GetByAltIds(getRecent100GuideOrderDetails.Select(s => s.OrderApprovedBy).Where(s => s != null)).ToList();
                    return new GuideOrderDetailsQueryResult
                    {
                        OrderDetails = getRecent100GuideOrderDetails.ToList(),
                        ApprovedByUsers = approvedByUsers
                    };
                }
                else
                {
                    var getRecent100GuideOrderDetails = _GuideDetailsRepository.GetRecent100GuideOrders(query.UserId, query.OrderStatusId, query.RolesId);
                    var approvedByUsers = _UserRepository.GetByAltIds(getRecent100GuideOrderDetails.Select(s => s.OrderApprovedBy).Where(s => s != null)).ToList();
                    return new GuideOrderDetailsQueryResult
                    {
                        OrderDetails = getRecent100GuideOrderDetails.ToList(),
                        ApprovedByUsers = approvedByUsers
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new GuideOrderDetailsQueryResult();
            }
        }
    }
}