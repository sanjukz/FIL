using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IValueRetailVillageRepository : IOrmRepository<ValueRetailVillage, ValueRetailVillage>
    {
        ValueRetailVillage Get(int id);

        ValueRetailVillage GetByName(string name);

        ValueRetailVillage GetByVillageCode(string villageCode);
    }

    public class ValueRetailVillageRepository : BaseOrmRepository<ValueRetailVillage>, IValueRetailVillageRepository
    {
        public ValueRetailVillageRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ValueRetailVillage Get(int id)
        {
            return Get(new ValueRetailVillage { Id = id });
        }

        public ValueRetailVillage GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailVillage.VillageName):C} = @VillageNameParam").WithParameters(new { VillageNameParam = name })).FirstOrDefault();
        }

        public ValueRetailVillage GetByVillageCode(string villageCode)
        {
            return GetAll(s => s.Where($"{nameof(ValueRetailVillage.VillageCode):C} = @VillageCodeParam").WithParameters(new { VillageCodeParam = villageCode })).FirstOrDefault();
        }

        public IEnumerable<ValueRetailVillage> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteValueRetailVillage(ValueRetailVillage valueRetailVillage)
        {
            Delete(valueRetailVillage);
        }

        public ValueRetailVillage SaveValueRetailVillage(ValueRetailVillage valueRetailVillage)
        {
            return Save(valueRetailVillage);
        }
    }
}