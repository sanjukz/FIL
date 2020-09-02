using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using FIL.Contracts.Enums;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface ISitePropertyRepository : IOrmRepository<SiteProperty, SiteProperty>
    {
        SiteProperty Get(int id);

        IEnumerable<SiteProperty> GetBySiteId(Site siteId);
    }

    public class SitePropertyRepository : BaseOrmRepository<SiteProperty>, ISitePropertyRepository
    {
        public SitePropertyRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public SiteProperty Get(int id)
        {
            return Get(new SiteProperty { Id = id });
        }

        public IEnumerable<SiteProperty> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCustomerUpdate(SiteProperty siteProperty)
        {
            Delete(siteProperty);
        }

        public SiteProperty SaveCustomerUpdate(SiteProperty siteProperty)
        {
            return Save(siteProperty);
        }

        public IEnumerable<SiteProperty> GetBySiteId(Site siteId)
        {
            var customerUpdateList = (GetAll(s => s.Where($"{nameof(SiteProperty.SiteId):C}=@SiteId AND IsEnabled = 1")
                                           .WithParameters(new { SiteId = siteId })
             ));
            return customerUpdateList;
        }
    }
}