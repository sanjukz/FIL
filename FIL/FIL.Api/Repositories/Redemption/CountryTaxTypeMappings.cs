using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels.Redemption;
using System.Collections.Generic;

namespace FIL.Api.Repositories.Redemption
{
    public interface ICountryTaxTypeMappingsRepository : IOrmRepository<Redemption_CountryTaxTypeMappings, Redemption_CountryTaxTypeMappings>
    {
        Redemption_CountryTaxTypeMappings Get(int Id);
    }

    public class CountryTaxTypeMappingsRepository : BaseLongOrmRepository<Redemption_CountryTaxTypeMappings>, ICountryTaxTypeMappingsRepository
    {
        public CountryTaxTypeMappingsRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Redemption_CountryTaxTypeMappings Get(int Id)
        {
            return Get(new Redemption_CountryTaxTypeMappings { Id = Id });
        }

        public IEnumerable<Redemption_CountryTaxTypeMappings> GetAll()
        {
            return GetAll(null);
        }
    }
}