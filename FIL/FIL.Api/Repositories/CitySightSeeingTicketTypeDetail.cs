using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICitySightSeeingTicketTypeDetailRepository : IOrmRepository<CitySightSeeingTicketTypeDetail, CitySightSeeingTicketTypeDetail>
    {
        CitySightSeeingTicketTypeDetail Get(int id);

        CitySightSeeingTicketTypeDetail GetByTicketId(string ticketId, string ticketType);

        IEnumerable<CitySightSeeingTicketTypeDetail> GetAllByTicketId(string ticketId);

        IEnumerable<CitySightSeeingTicketTypeDetail> GetAllDisabledByTicketId(string ticketId);
    }

    public class CitySightSeeingTicketTypeDetailRepository : BaseOrmRepository<CitySightSeeingTicketTypeDetail>, ICitySightSeeingTicketTypeDetailRepository
    {
        public CitySightSeeingTicketTypeDetailRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public CitySightSeeingTicketTypeDetail Get(int id)
        {
            return Get(new CitySightSeeingTicketTypeDetail { Id = id });
        }

        public IEnumerable<CitySightSeeingTicketTypeDetail> GetAll()
        {
            return GetAll(null);
        }

        public CitySightSeeingTicketTypeDetail GetByTicketId(string ticketId, string ticketType)
        {
            return GetAll(s => s.Where($"{nameof(CitySightSeeingTicketTypeDetail.TicketId):C} = @TicketId AND {nameof(CitySightSeeingTicketTypeDetail.TicketType):C} = @TicketType ")
                .WithParameters(new { TicketId = ticketId, TicketType = ticketType })
            ).FirstOrDefault();
        }

        public void DeleteCitySightSeeingTicketTypeDetail(CitySightSeeingTicketTypeDetail CitySightSeeingTicketTypeDetail)
        {
            Delete(CitySightSeeingTicketTypeDetail);
        }

        public CitySightSeeingTicketTypeDetail SaveCitySightSeeingTicketTypeDetail(CitySightSeeingTicketTypeDetail CitySightSeeingTicketTypeDetail)
        {
            return Save(CitySightSeeingTicketTypeDetail);
        }

        public IEnumerable<CitySightSeeingTicketTypeDetail> GetAllByTicketId(string ticketId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(CitySightSeeingTicketTypeDetail.TicketId):C}=@Ids")
                .WithParameters(new { Ids = ticketId }));
        }

        public IEnumerable<CitySightSeeingTicketTypeDetail> GetAllDisabledByTicketId(string ticketId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(CitySightSeeingTicketTypeDetail.TicketId):C}=@Ids AND IsEnabled=0")
                .WithParameters(new { Ids = ticketId }));
        }
    }
}