using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IReprintRequestDetailRepository : IOrmRepository<ReprintRequestDetail, ReprintRequestDetail>
    {
        ReprintRequestDetail Get(long id);

        IEnumerable<ReprintRequestDetail> GetByReprintRequestId(IEnumerable<long> reprintid);

        IEnumerable<ReprintRequestDetail> GetByReprintRequestIdForApprove(IEnumerable<long> reprintid);

        ReprintRequestDetail GetRequestDetailByMatchSeatTicketDetailIdaAndUserAltId(long matchSeatTicketDetailId, Guid userAltId);

        ReprintRequestDetail GetRequestDetailByBarcodeAndUserAltId(string barcodeNumber, Guid userAltId);
    }

    public class ReprintRequestDetailRepository : BaseLongOrmRepository<ReprintRequestDetail>, IReprintRequestDetailRepository
    {
        public ReprintRequestDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ReprintRequestDetail Get(long id)
        {
            return Get(new ReprintRequestDetail { Id = id });
        }

        public IEnumerable<ReprintRequestDetail> GetByReprintRequestId(IEnumerable<long> reprintId)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequestDetail.RePrintRequestId):C} IN @ReprintRequestIds")
            .WithParameters(new { ReprintRequestIds = reprintId })
            );
        }

        public IEnumerable<ReprintRequestDetail> GetByReprintRequestIdForApprove(IEnumerable<long> reprintId)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequestDetail.RePrintRequestId):C} IN @ReprintRequestIds AND {nameof(ReprintRequestDetail.IsApproved):C}=0")
            .WithParameters(new { ReprintRequestIds = reprintId })
            );
        }

        public ReprintRequestDetail GetRequestDetailByMatchSeatTicketDetailIdaAndUserAltId(long matchSeatTicketDetailId, Guid userAltId)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequestDetail.MatchSeatTicketDetaildId):C} = @MatchSeatTicketDetailId AND {nameof(ReprintRequestDetail.CreatedBy):C}=@CreatedBy")
            .WithParameters(new { MatchSeatTicketDetailId = matchSeatTicketDetailId, CreatedBy = userAltId })
             ).FirstOrDefault();
        }

        public ReprintRequestDetail GetRequestDetailByBarcodeAndUserAltId(string barcodeNumber, Guid userAltId)
        {
            return GetAll(s => s.Where($"{nameof(ReprintRequestDetail.BarcodeNumber):C} = @BarcodeNumber AND {nameof(ReprintRequestDetail.CreatedBy):C}=@CreatedBy")
            .WithParameters(new { BarcodeNumber = barcodeNumber, CreatedBy = userAltId })
             ).FirstOrDefault();
        }

        public IEnumerable<ReprintRequestDetail> GetAll()
        {
            return GetAll(null);
        }
    }
}