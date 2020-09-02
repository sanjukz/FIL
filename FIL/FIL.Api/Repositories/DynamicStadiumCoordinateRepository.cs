using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Api.Repositories
{
    public interface IDynamicStadiumCoordinateRepository : IOrmRepository<DynamicStadiumCoordinate, DynamicStadiumCoordinate>
    {
        DynamicStadiumCoordinate Get(int id);

        IEnumerable<DynamicStadiumCoordinate> GetByVenueId(int venueId);
    }

    public class DynamicStadiumCoordinateRepository : BaseOrmRepository<DynamicStadiumCoordinate>, IDynamicStadiumCoordinateRepository
    {
        public DynamicStadiumCoordinateRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public DynamicStadiumCoordinate Get(int id)
        {
            return Get(new DynamicStadiumCoordinate { Id = id });
        }

        public IEnumerable<DynamicStadiumCoordinate> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteDynamicStadiumCoordinate(DynamicStadiumCoordinate dynamicStadiumCoordinate)
        {
            Delete(dynamicStadiumCoordinate);
        }

        public DynamicStadiumCoordinate SaveDynamicStadiumCoordinate(DynamicStadiumCoordinate dynamicStadiumCoordinate)
        {
            return Save(dynamicStadiumCoordinate);
        }

        public IEnumerable<DynamicStadiumCoordinate> GetByVenueId(int venueId)
        {
            return (GetAll(s => s.Where($"{nameof(DynamicStadiumCoordinate.VenueId):C}=@VenueId AND {nameof(DynamicStadiumCoordinate.IsEnabled):C}=@isEnabled")
                           .WithParameters(new { VenueId = venueId, isEnabled = true })));
        }
    }
}