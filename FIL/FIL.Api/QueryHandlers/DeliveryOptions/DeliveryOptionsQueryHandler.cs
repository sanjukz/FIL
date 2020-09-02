using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.DeliveryOptions;
using FIL.Contracts.QueryResults.DeliveryOptions;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Cities
{
    public class DeliveryOptionsQueryHandler : IQueryHandler<DeliveryOptionsQuery, DeliveryOptionsQueryResult>
    {
        private readonly IEventDeliveryTypeDetailRepository _eventDeliveryTypeDetailRepository;
        private readonly IUserRepository _userRepository;

        public DeliveryOptionsQueryHandler(IEventDeliveryTypeDetailRepository eventDeliveryTypeDetailRepository, IUserRepository userRepository)
        {
            _eventDeliveryTypeDetailRepository = eventDeliveryTypeDetailRepository;
            _userRepository = userRepository;
        }

        public DeliveryOptionsQueryResult Handle(DeliveryOptionsQuery query)
        {
            var eventDeliveryTypeDetailsDataModel = _eventDeliveryTypeDetailRepository.GetByEventDetailId(query.EventDetailId);
            var eventDeliveryTypeDetailsModel = AutoMapper.Mapper.Map<List<Contracts.Models.EventDeliveryTypeDetail>>(eventDeliveryTypeDetailsDataModel).ToList();
            var user = _userRepository.GetByAltId(query.UserId);
            return new DeliveryOptionsQueryResult
            {
                EventDeliveryTypeDetails = eventDeliveryTypeDetailsModel,
                UserDetails = AutoMapper.Mapper.Map<User>(user)
            };
        }
    }
}