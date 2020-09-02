using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingExtraOptionRepository : IOrmRepository<CitySightSeeingExtraOption, CitySightSeeingExtraOption>
    {
        CitySightSeeingExtraOption Get(int id);

        CitySightSeeingExtraOption GetByTicketTypeDetailId(int citySightSeeingTicketTypeDetailId);
    }

    public class CitySightSeeingExtraOptionRepository : BaseOrmRepository<CitySightSeeingExtraOption>, ICitySightSeeingExtraOptionRepository
    {
        public CitySightSeeingExtraOptionRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingExtraOption Get(int id)
        {
            return Get(new CitySightSeeingExtraOption { Id = id });
        }

        public CitySightSeeingExtraOption GetByTicketTypeDetailId(int citySightSeeingTicketTypeDetailId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingExtraOption.CitySightSeeingTicketTypeDetailId):C} = @ExtraOptionId")
                .WithParameters(new { ExtraOptionId = citySightSeeingTicketTypeDetailId })
            ).FirstOrDefault();
        }

        public IEnumerable<CitySightSeeingExtraOption> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCitySightSeeingExtraOption(CitySightSeeingExtraOption CitySightSeeingExtraOption)
        {
            Delete(CitySightSeeingExtraOption);
        }

        public CitySightSeeingExtraOption SaveCitySightSeeingExtraOption(CitySightSeeingExtraOption CitySightSeeingExtraOption)
        {
            return Save(CitySightSeeingExtraOption);
        }
    }
}