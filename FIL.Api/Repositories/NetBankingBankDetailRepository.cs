using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface INetBankingBankDetailRepository : IOrmRepository<NetBankingBankDetail, NetBankingBankDetail>
    {
        NetBankingBankDetail Get(int id);

        NetBankingBankDetail GetByAltId(Guid altId);
    }

    public class NetBankingBankDetailRepository : BaseLongOrmRepository<NetBankingBankDetail>, INetBankingBankDetailRepository
    {
        public NetBankingBankDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public NetBankingBankDetail Get(int id)
        {
            return Get(new NetBankingBankDetail { Id = id });
        }

        public IEnumerable<NetBankingBankDetail> GetAll()
        {
            return GetAll(null);
        }

        public NetBankingBankDetail GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(NetBankingBankDetail.AltId):C}=@AltId")
                    .WithParameters(new { AltId = altId })
                ).FirstOrDefault();
        }
    }
}