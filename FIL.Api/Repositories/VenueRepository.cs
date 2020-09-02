using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IVenueRepository : IOrmRepository<Venue, Venue>
    {
        Venue Get(int id);

        IEnumerable<Venue> GetByVenueIds(IEnumerable<int> venueIds);

        Venue GetByVenueId(int venueId);

        Venue GetByName(string name);

        Venue GetByAltId(Guid altId);

        IEnumerable<Venue> GetByNames(List<string> names);

        IEnumerable<Venue> GetByVenueName(string venueName);

        IEnumerable<Venue> GetByCityIds(List<int> cityIds);

        IEnumerable<Venue> GetBySiteId(int siteId);

        IEnumerable<Venue> GetAllVenueByCity(string cityName);

        Venue GetByVenueNameAndCityId(string venueName, int cityId);

        Venue GetByLatLon(string latitute, string longitutde);

        IEnumerable<Venue> GetAllVenues();

        Venue GetByNameAndCityId(string name, int cityId);
    }

    public class VenueRepository : BaseOrmRepository<Venue>, IVenueRepository
    {
        public VenueRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public Venue Get(int id)
        {
            return Get(new Venue { Id = id });
        }

        public IEnumerable<Venue> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteVenue(Venue venue)
        {
            Delete(venue);
        }

        public Venue SaveVenue(Venue venue)
        {
            return Save(venue);
        }

        public IEnumerable<Venue> GetByVenueIds(IEnumerable<int> venueIds)
        {
            var venueList = GetAll(statement => statement
                                    .Where($"{nameof(Venue.Id):C} IN @Ids")
                                    .WithParameters(new { Ids = venueIds }));
            return venueList;
        }

        public Venue GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(Venue.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public Venue GetByNameAndCityId(string name, int cityId)
        {
            return GetAll(s => s.Where($"{nameof(Venue.Name):C} = @Name AND {nameof(Venue.CityId)} = @CityId")
                .WithParameters(new { Name = name, CityId = cityId })
            ).FirstOrDefault();
        }

        public IEnumerable<Venue> GetByVenueName(string venueName)
        {
            var venueList = (GetAll(s => s.Where($"{nameof(Venue.Name):C} LIKE '%'+@Name+'%' AND IsEnabled=1")
                   .WithParameters(new { Name = venueName })
            ));
            return venueList;
        }

        public Venue GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Venue.AltId):C} = @AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public Venue GetByVenueId(int venueId)
        {
            return GetAll(statement => statement
                                    .Where($"{nameof(Venue.Id):C} = @Ids")
                                    .WithParameters(new { Ids = venueId })).FirstOrDefault();
        }

        public IEnumerable<Venue> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(Venue.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }

        public IEnumerable<Venue> GetByCityIds(List<int> cityIds)
        {
            return GetAll(s => s.Where($"{nameof(Venue.CityId):C} in @City")
                .WithParameters(new { City = cityIds })
            );
        }

        public IEnumerable<Venue> GetAllVenueByCity(string cityName)
        {
            return GetCurrentConnection().Query<Venue>("select DISTINCT v.* from events e  WITH (NOLOCK) " +
                                                       " inner join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " inner join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " inner join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " inner join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " inner join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE ct.name LIKE '%'+@CityName+'%' AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           CityName = cityName,
                                                       });
        }

        public IEnumerable<Venue> GetBySiteId(int siteId)
        {
            return GetCurrentConnection().Query<Venue>("SELECT DISTINCT V.* FROM EventSiteIdMappings ESIM WITH (NOLOCK) " +
                                                        "INNER JOIN EventDetails ED WITH(NOLOCK) ON ESIM.EventId = ED.EventId " +
                                                        "INNER JOIN Venues V WITH(NOLOCK) ON V.Id = ED.VenueId " +
                                                        "WHERE ESIM.IsEnabled = 1 " +
                                                        "AND ED.IsEnabled = 1 " +
                                                        "AND ESIM.SiteId = @SiteId", new
                                                        {
                                                            SiteId = siteId
                                                        });
        }

        public Venue GetByVenueNameAndCityId(string venueName, int cityId)
        {
            return GetAll(s => s.Where($"{nameof(Venue.Name):C} LIKE '%'+@Name+'%' AND {nameof(Venue.CityId):C} =@CityId")
                      .WithParameters(new { Name = venueName, CityId = cityId })).FirstOrDefault();
        }

        public Venue GetByLatLon(string latitude, string longitutde)
        {
            if (latitude != "" || latitude != null || longitutde != "" || longitutde != null)
            {
                return GetAll(s => s.Where($"{nameof(Venue.Latitude):C} = @LatitudeParam AND {nameof(Venue.Longitude):C} =@LongitudeParam").WithParameters(new { LatitudeParam = latitude.Trim(), LongitudeParam = longitutde.Trim() })).FirstOrDefault();
            }
            return new Venue();
        }

        public IEnumerable<Venue> GetAllVenues()
        {
            return GetCurrentConnection().Query<Venue>("select DISTINCT v.* FROM events e  WITH (NOLOCK) " +
            "inner join eventdetails ed WITH(NOLOCK) on e.id = ed.eventId " +
            "inner join venues v WITH(NOLOCK) on ed.venueId = v.id " +
            "inner join cities ct WITH(NOLOCK) on ct.id = v.cityid " +
            "inner join states st WITH(NOLOCK) on st.id = ct.stateid " +
            "inner join countries country WITH(NOLOCK) on country.id = st.countryId " +
            "WHERE e.EventCategoryId In(2, 3) and VenueId Not in (39,134,155,158,193,292,293,294,295,296,297,298,307,311,388) " +
            "Union " +
            "Select * From Venues where CreatedUtc >= '2019-06-05 00:00:00.000'");
        }
    }
}