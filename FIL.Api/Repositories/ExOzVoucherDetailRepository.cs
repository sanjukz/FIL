using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IExOzVoucherDetailRepository : IOrmRepository<ExOzVoucherDetail, ExOzVoucherDetail>
    {
        ExOzVoucherDetail Get(int id);
    }

    public class ExOzVoucherDetailRepository : BaseOrmRepository<ExOzVoucherDetail>, IExOzVoucherDetailRepository
    {
        public ExOzVoucherDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzVoucherDetail Get(int id)
        {
            return Get(new ExOzVoucherDetail { Id = id });
        }

        public IEnumerable<ExOzVoucherDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteVoucherDetail(ExOzVoucherDetail exOzVoucherDetail)
        {
            Delete(exOzVoucherDetail);
        }

        public ExOzVoucherDetail SaveVoucherDetail(ExOzVoucherDetail exOzVoucherDetail)
        {
            return Save(exOzVoucherDetail);
        }
    }
}