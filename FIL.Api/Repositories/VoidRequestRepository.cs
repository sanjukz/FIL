using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IVoidRequestRepository : IOrmRepository<VoidRequest, VoidRequest>
    {
        VoidRequest Get(long id);
    }

    public class VoidRequestRepository : BaseLongOrmRepository<VoidRequest>, IVoidRequestRepository
    {
        public VoidRequestRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public VoidRequest Get(long id)
        {
            return Get(new VoidRequest { Id = id });
        }

        public IEnumerable<VoidRequest> GetAll()
        {
            return GetAll(null);
        }
    }
}