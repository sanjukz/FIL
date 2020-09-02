using FIL.Api.Repositories;
using FIL.Contracts.Queries.UserCard;
using FIL.Contracts.QueryResults.UserCard;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.UserCard
{
    public class UserCardQueryHandler : IQueryHandler<UserCardQuery, UserCardQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserCardDetailRepository _userCardDetailRepository;
        private readonly FIL.Logging.ILogger _logger;

        public UserCardQueryHandler(IUserRepository userRepository, IUserCardDetailRepository userCardDetailRepository, FIL.Logging.ILogger logger)
        {
            _userRepository = userRepository;
            _userCardDetailRepository = userCardDetailRepository;
            _logger = logger;
        }

        public UserCardQueryResult Handle(UserCardQuery query)
        {
            List<FIL.Contracts.Models.UserCard> userCards = new List<FIL.Contracts.Models.UserCard>();
            var user = _userRepository.GetByAltId(query.UserAltId);
            if (user == null)
            {
                return new UserCardQueryResult();
            }
            else
            {
                var cardList = _userCardDetailRepository.GetByUserId(user.Id);
                try
                {
                    foreach (var item in cardList)
                    {
                        userCards.Add(new FIL.Contracts.Models.UserCard
                        {
                            AltId = item.AltId,
                            NameOnCard = item.NameOnCard,
                            CardNumber = item.CardNumber,
                            ExpiryMonth = item.ExpiryMonth,
                            ExpiryYear = item.ExpiryYear,
                            CardTypeId = item.CardTypeId,
                        });
                    }
                    return new UserCardQueryResult
                    {
                        UserCards = userCards
                    };
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new UserCardQueryResult();
                }
            }
        }
    }
}