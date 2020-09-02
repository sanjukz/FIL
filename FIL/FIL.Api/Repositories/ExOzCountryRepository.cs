using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IExOzCountryRepository : IOrmRepository<ExOzCountry, ExOzCountry>
    {
        ExOzCountry Get(int id);

        ExOzCountry GetByName(string name);

        List<ExOzCountry> DisableAllExOzCountries();
    }

    public class ExOzCountryRepository : BaseOrmRepository<ExOzCountry>, IExOzCountryRepository
    {
        public ExOzCountryRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public ExOzCountry Get(int id)
        {
            return Get(new ExOzCountry { Id = id });
        }

        public IEnumerable<ExOzCountry> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCountry(ExOzCountry exOzCountry)
        {
            Delete(exOzCountry);
        }

        public ExOzCountry SaveCountry(ExOzCountry exOzCountry)
        {
            return Save(exOzCountry);
        }

        public ExOzCountry GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(ExOzCountry.Name):C}=@Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public List<ExOzCountry> DisableAllExOzCountries()
        {
            List<ExOzCountry> allExOzCountries = this.GetAll().ToList();
            foreach (var country in allExOzCountries)
            {
                country.IsEnabled = false;
                Save(country);
            }
            return allExOzCountries;
        }
    }
}