using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IZsuiteUserFeeDetailRepository : IOrmRepository<ZsuiteUserFeeDetail, ZsuiteUserFeeDetail>
    {
        ZsuiteUserFeeDetail Get(long id);

        IEnumerable<ZsuiteUserFeeDetail> GetByBoxOfficeUserAdditionalDetailId(long boxOfficeUserAdditionalDetailId);
    }

    public class ZsuiteUserFeeDetailRepository : BaseLongOrmRepository<ZsuiteUserFeeDetail>, IZsuiteUserFeeDetailRepository
    {
        public ZsuiteUserFeeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ZsuiteUserFeeDetail Get(long id)
        {
            return Get(new ZsuiteUserFeeDetail { Id = id });
        }

        public IEnumerable<ZsuiteUserFeeDetail> GetByBoxOfficeUserAdditionalDetailId(long boxOfficeUserAdditionalDetailId)
        {
            var zsuiteUserFeeDetailList = GetAll(statement => statement
                                 .Where($"{nameof(ZsuiteUserFeeDetail.BoxOfficeUserAdditionalDetailId):C} = @BoxOfficeUserAdditionalDetailId AND IsEnabled=1")
                                 .WithParameters(new { BoxOfficeUserAdditionalDetailId = boxOfficeUserAdditionalDetailId }));
            return zsuiteUserFeeDetailList;
        }
    }
}