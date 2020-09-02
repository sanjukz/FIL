using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.Models;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IFeelNearbyRepository : IOrmRepository<Venue, Venue>
    {
        List<Venue> GetNearbyPlaces(decimal latitude, decimal longitude, decimal distanceFilter);
    }

    public class FeelNearbyRepository : BaseOrmRepository<Venue>, IFeelNearbyRepository
    {
        public FeelNearbyRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public List<Venue> GetNearbyPlaces(decimal latitude, decimal longitude, decimal distanceFilter = 100)
        {
            var count = GetCurrentConnection().Query<Venue>("Select Id, Name, CityId from Venues Where ACOS(COS(RADIANS(90 - @Lat)) * COS(RADIANS(90 - TRY_CONVERT(decimal(9, 6), latitude))) + SIN(RADIANS(90 - @Lat)) * SIN(RADIANS(90 - TRY_CONVERT(decimal(9, 6), latitude))) * COS(RADIANS(@Lon - TRY_CONVERT(decimal(9, 6), longitude)))) * 3959 <= @Distance", new
            {
                Lat = latitude,
                Lon = longitude,
                Distance = distanceFilter
            });
            return count.ToList();
        }
    }
}