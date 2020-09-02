using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ITokenRepository : IOrmRepository<Token, Token>
    {
        Token Get(int id);

        Token GetByTokenId(Guid tokenId);
    }

    public class TokenRepository : BaseOrmRepository<Token>, ITokenRepository
    {
        public TokenRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Token Get(int id)
        {
            return Get(new Token { Id = id });
        }

        public IEnumerable<Token> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteToken(Token token)
        {
            Delete(token);
        }

        public Token SaveToken(Token token)
        {
            return Save(token);
        }

        public Token GetByTokenId(Guid tokenId)
        {
            return GetAll(s => s.Where($"{nameof(Token.TokenId):C} = @TokenId")
                .WithParameters(new { TokenId = tokenId })
            ).FirstOrDefault();
        }
    }
}