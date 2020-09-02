using FIL.Api.Repositories;
using FIL.Contracts.Queries.User;
using FIL.Contracts.QueryResults.User;
using System;

namespace FIL.Api.QueryHandlers.Users
{
    public class UserTokenQueryHandler : IQueryHandler<UserTokenQuery, UserTokenQueryResult>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserTokenMappingRepository _userTokenMappingRepository;
        private readonly IUserRepository _userRepository;
        private readonly FIL.Logging.ILogger _logger;

        public UserTokenQueryHandler(ITokenRepository tokenRepository, IUserTokenMappingRepository userTokenMappingRepository, IUserRepository userRepository, FIL.Logging.ILogger logger)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _userTokenMappingRepository = userTokenMappingRepository;
            _logger = logger;
        }

        public UserTokenQueryResult Handle(UserTokenQuery query)
        {
            try
            {
                var token = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.Token>(_tokenRepository.GetByTokenId(query.AccessToken));
                if (token != null)
                {
                    var userMapping = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.UserTokenMapping>(_userTokenMappingRepository.GetByTokenId(token.Id));
                    if (userMapping != null)
                    {
                        var users = _userRepository.Get(userMapping.UserId);
                        if (users != null)
                        {
                            return new UserTokenQueryResult
                            {
                                IsValid = true,
                                UserAltId = users.AltId
                            };
                        }
                        else
                        {
                            return new UserTokenQueryResult
                            {
                                IsValid = false,
                            };
                        }
                    }
                    else
                    {
                        return new UserTokenQueryResult
                        {
                            IsValid = false
                        };
                    }
                }
                else
                {
                    return new UserTokenQueryResult
                    {
                        IsValid = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new UserTokenQueryResult
                {
                    IsValid = false
                };
            }
        }
    }
}