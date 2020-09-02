using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ICardDetailRepository : IOrmRepository<CardDetail, CardDetail>
    {
        CardDetail Get(int id);
    }

    public class CardDetailRepository : BaseOrmRepository<CardDetail>, ICardDetailRepository
    {
        public CardDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CardDetail Get(int id)
        {
            return Get(new CardDetail { Id = id });
        }

        public IEnumerable<CardDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCardDetail(CardDetail cardDetail)
        {
            Delete(cardDetail);
        }

        public CardDetail SaveCardDetail(CardDetail cardDetail)
        {
            return Save(cardDetail);
        }
    }
}