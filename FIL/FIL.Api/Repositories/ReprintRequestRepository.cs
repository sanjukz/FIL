using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IReprintRequestRepository : IOrmRepository<ReprintRequest, ReprintRequest>
    {
        ReprintRequest Get(long id);

        IEnumerable<ReprintRequest> GetByUserAltId(Guid useraltId);

        IEnumerable<ReprintRequest> GetAllByUserAltId(IEnumerable<Guid> altids);

        ReprintRequest GetByAltId(Guid altId);

        ReprintRequest GetByUserAltIdAndTransactionId(Guid userAltId, long transactionId);
    }

    public class ReprintRequestRepository : BaseLongOrmRepository<ReprintRequest>, IReprintRequestRepository
    {
        public ReprintRequestRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ReprintRequest Get(long id)
        {
            return Get(new ReprintRequest { Id = id });
        }

        public ReprintRequest GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequest.AltId):C} = @AltId")
                  .WithParameters(new { AltId = altId })
              ).FirstOrDefault();
        }

        public IEnumerable<ReprintRequest> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<ReprintRequest> GetByUserAltId(Guid useraltId)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequest.CreatedBy):C} = @CreatedById")
            .WithParameters(new { CreatedById = useraltId }));
        }

        public IEnumerable<ReprintRequest> GetAllByUserAltId(IEnumerable<Guid> altids)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequest.CreatedBy):C} IN @Altids")
               .WithParameters(new { Altids = altids })
           );
        }

        public ReprintRequest GetByUserAltIdAndTransactionId(Guid userAltId, long transactionId)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequest.CreatedBy):C}= @UserAltId and { nameof(ReprintRequest.TransactionId):C}=@TransactionId ")
           .WithParameters(new { UserAltId = userAltId, TransactionId = transactionId })).FirstOrDefault();
        }
    }
}