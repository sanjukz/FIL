using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingTicketDetailImageRepository : IOrmRepository<CitySightSeeingTicketDetailImage, CitySightSeeingTicketDetailImage>
    {
        CitySightSeeingTicketDetailImage Get(int id);

        CitySightSeeingTicketDetailImage GetByTicketId(string ticketId);
    }

    public class CitySightSeeingTicketDetailImageRepository : BaseOrmRepository<CitySightSeeingTicketDetailImage>, ICitySightSeeingTicketDetailImageRepository
    {
        public CitySightSeeingTicketDetailImageRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingTicketDetailImage Get(int id)
        {
            return Get(new CitySightSeeingTicketDetailImage { Id = id });
        }

        public IEnumerable<CitySightSeeingTicketDetailImage> GetAll()
        {
            return GetAll(null);
        }

        public CitySightSeeingTicketDetailImage GetByTicketId(string ticketId)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingTicketDetailImage.TicketId):C} = @TicketId")
                .WithParameters(new { TicketId = ticketId })
            ).FirstOrDefault();
        }

        public void DeleteCitySightSeeingTicketDetailImage(CitySightSeeingTicketDetailImage CitySightSeeingTicketDetailImage)
        {
            Delete(CitySightSeeingTicketDetailImage);
        }

        public CitySightSeeingTicketDetailImage SaveCitySightSeeingTicketDetailImage(CitySightSeeingTicketDetailImage CitySightSeeingTicketDetailImage)
        {
            return Save(CitySightSeeingTicketDetailImage);
        }
    }
}