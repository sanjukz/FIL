using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Redemption
{
    public interface IServicesRepository : IOrmRepository<Services, Services>
    {
        Services Get(int Id);

        IEnumerable<Services> GetAllByIds(List<int> serviceIds);
    }

    public class ServicesRepository : BaseOrmRepository<Services>, IServicesRepository
    {
        public ServicesRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Services Get(int Id)
        {
            return Get(new Services { Id = Id });
        }

        public IEnumerable<Services> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<Services> GetAllByIds(List<int> serviceIds)
        {
            return GetAll(s => s.Where($"{nameof(Services.Id):C} IN @Id").WithParameters(new { Id = serviceIds }));
        }
    }
}