using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IPartialVoidRequestDetailRepository : IOrmRepository<PartialVoidRequestDetail, PartialVoidRequestDetail>
    {
        PartialVoidRequestDetail Get(long id);

        PartialVoidRequestDetail GetByBarcodeNumber(string barcodenumber);

        IEnumerable<PartialVoidRequestDetail> GetAllByUserAltId(IEnumerable<Guid> altids);
    }

    public class PartialVoidRequestDetailRepository : BaseLongOrmRepository<PartialVoidRequestDetail>, IPartialVoidRequestDetailRepository
    {
        public PartialVoidRequestDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public PartialVoidRequestDetail Get(long id)
        {
            return Get(new PartialVoidRequestDetail { Id = id });
        }

        public PartialVoidRequestDetail GetByBarcodeNumber(string barcodenumber)
        {
            return GetAll(s => s.Where($"{nameof(PartialVoidRequestDetail.BarcodeNumber):C} = @BarcodeNumber")
                .WithParameters(new { BarcodeNumber = barcodenumber })
            ).FirstOrDefault();
        }

        public IEnumerable<PartialVoidRequestDetail> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<PartialVoidRequestDetail> GetAllByUserAltId(IEnumerable<Guid> altids)
        {
            return GetAll(s => s.Where($"{nameof(PartialVoidRequestDetail.CreatedBy):C} IN @Altids AND {nameof(PartialVoidRequestDetail.IsPartialVoid):C}=0")
            .WithParameters(new { Altids = altids })
            );
        }
    }
}