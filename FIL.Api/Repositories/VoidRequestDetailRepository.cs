using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IVoidRequestDetailRepository : IOrmRepository<VoidRequestDetail, VoidRequestDetail>
    {
        VoidRequestDetail Get(long id);

        IEnumerable<VoidRequestDetail> GetByAltIds(IEnumerable<Guid> guids);

        VoidRequestDetail GetByTransId(long transId);
    }

    public class VoidRequestDetailRepository : BaseLongOrmRepository<VoidRequestDetail>, IVoidRequestDetailRepository
    {
        public VoidRequestDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public VoidRequestDetail Get(long id)
        {
            return Get(new VoidRequestDetail { Id = id });
        }

        public IEnumerable<VoidRequestDetail> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<VoidRequestDetail> GetByAltIds(IEnumerable<Guid> guids)
        {
            return GetAll(s => s.Where($"{nameof(VoidRequestDetail.CreatedBy):C} IN @altids AND  {nameof(VoidRequestDetail.IsVoid):C} = 0")
               .WithParameters(new { altids = guids })
           );
        }

        public VoidRequestDetail GetByTransId(long transId)
        {
            return GetAll(s => s.Where($"{nameof(VoidRequestDetail.TransactionId):C} = @TransactionId")
             .WithParameters(new { TransactionId = transId })
             ).FirstOrDefault();
        }
    }
}