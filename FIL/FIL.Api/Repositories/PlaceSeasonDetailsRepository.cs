using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IPlaceSeasonDetailsRepository : IOrmRepository<PlaceSeasonDetails, PlaceSeasonDetails>
    {
        PlaceSeasonDetails Get(long id);

        IEnumerable<PlaceSeasonDetails> GetByEventId(long eventId);
    }

    public class PlaceSeasonDetailsRepository : BaseLongOrmRepository<PlaceSeasonDetails>, IPlaceSeasonDetailsRepository
    {
        public PlaceSeasonDetailsRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public PlaceSeasonDetails Get(long id)
        {
            return Get(new PlaceSeasonDetails { Id = id });
        }

        public IEnumerable<PlaceSeasonDetails> GetByEventId(long eventId)
        {
            return GetAll(statement => statement
                .Where($"{nameof(PlaceSeasonDetails.EventId):C}=@EventId")
                .WithParameters(new { EventId = eventId }));
        }
    }
}