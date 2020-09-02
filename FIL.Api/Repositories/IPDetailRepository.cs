using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IIPDetailRepository : IOrmRepository<IPDetail, IPDetail>
    {
        IPDetail Get(int id);

        IPDetail GetByIpAddress(string ipAddress);

        IEnumerable<IPDetail> GetByIds(IEnumerable<int?> ids);

        IEnumerable<IPDetail> GetAllByIds(List<int> ids);
    }

    public class IPDetailRepository : BaseOrmRepository<IPDetail>, IIPDetailRepository
    {
        public IPDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public IPDetail Get(int id)
        {
            return Get(new IPDetail { Id = id });
        }

        public IEnumerable<IPDetail> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteIPDetail(IPDetail iPDetail)
        {
            Delete(iPDetail);
        }

        public IPDetail SaveIPDetail(IPDetail iPDetail)
        {
            return Save(iPDetail);
        }

        public IPDetail GetByIpAddress(string ipAddress)
        {
            return GetAll(s => s.Where($"{nameof(IPDetail.IPAddress):C}=@IPAddress")
            .WithParameters(new { IPAddress = ipAddress })).FirstOrDefault();
        }

        public IEnumerable<IPDetail> GetByIds(IEnumerable<int?> ids)
        {
            return GetAll(s => s.Where($"{nameof(IPDetail.Id):C} IN @Ids")
                .WithParameters(new { Ids = ids })
            );
        }

        public IEnumerable<IPDetail> GetAllByIds(List<int> ids)
        {
            return GetAll(s => s.Where($"{nameof(IPDetail.Id):C} IN @Ids")
                .WithParameters(new { Ids = ids })
            );
        }
    }
}