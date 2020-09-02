using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingExtraSubOptionRepository : IOrmRepository<CitySightSeeingExtraSubOption, CitySightSeeingExtraSubOption>
    {
        CitySightSeeingExtraSubOption Get(int id);

        CitySightSeeingExtraSubOption GetByExtraOptionId(int citySightSeeingExtraOptionId);
    }

    public class CitySightSeeingExtraSubOptionRepository : BaseOrmRepository<CitySightSeeingExtraSubOption>, ICitySightSeeingExtraSubOptionRepository
    {
        public CitySightSeeingExtraSubOptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingExtraSubOption Get(int id)
        {
            return Get(new CitySightSeeingExtraSubOption { Id = id });
        }

        public CitySightSeeingExtraSubOption GetByExtraOptionId(int citySightSeeingExtraOptionId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingExtraSubOption.CitySightSeeingExtraOptionId):C} = @ExtraOptionId")
                .WithParameters(new { ExtraOptionId = citySightSeeingExtraOptionId })
            ).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingExtraSubOption> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCitySightSeeingExtraSubOption(CitySightSeeingExtraSubOption CitySightSeeingExtraSubOption)
        {
            Delete(CitySightSeeingExtraSubOption);
        }

        public CitySightSeeingExtraSubOption SaveCitySightSeeingExtraSubOption(CitySightSeeingExtraSubOption CitySightSeeingExtraSubOption)
        {
            return Save(CitySightSeeingExtraSubOption);
        }
    }
}