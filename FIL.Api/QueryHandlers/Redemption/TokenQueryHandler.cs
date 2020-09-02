using FIL.Api.Repositories;
using FIL.Contracts.Queries.Redemption;
using FIL.Contracts.QueryResults.Redemption;
using System;

namespace FIL.Api.QueryHandlers.Redemption
{
    public class TokenQueryHandler : IQueryHandler<TokenQuery, TokenQueryResult>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly FIL.Logging.ILogger _logger;

        public TokenQueryHandler(ITokenRepository tokenRepository, FIL.Logging.ILogger logger)
        {
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        public TokenQueryResult Handle(TokenQuery query)
        {
            try
            {
                var token = AutoMapper.Mapper.Map<FIL.Contracts.DataModels.Token>(_tokenRepository.GetByTokenId(query.TokenId));
                if (token != null)
                {
                    return new TokenQueryResult
                    {
                        IsValid = true
                    };
                }
                else
                {
                    return new TokenQueryResult
                    {
                        IsValid = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new TokenQueryResult
                {
                    IsValid = false
                };
            }
        }
    }
}