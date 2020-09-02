using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICashCardDetailRepository : IOrmRepository<CashCardDetail, CashCardDetail>
    {
        CashCardDetail Get(int id);

        CashCardDetail GetByAltId(Guid altId);
    }

    public class CashCardDetailRepository : BaseLongOrmRepository<CashCardDetail>, ICashCardDetailRepository
    {
        public CashCardDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CashCardDetail Get(int id)
        {
            return Get(new CashCardDetail { Id = id });
        }

        public IEnumerable<CashCardDetail> GetAll()
        {
            return GetAll(null);
        }

        public CashCardDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(CashCardDetail.AltId):C}=@AltId")
                    .WithParameters(new { AltId = altId })
                ).FirstOrDefault();
        }
    }
}