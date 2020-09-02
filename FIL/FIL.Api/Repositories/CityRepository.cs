using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICityRepository : IOrmRepository<City, City>
    {
        City Get(int id);

        IEnumerable<City> GetByCityIds(IEnumerable<int> cityIds);

        City GetByName(string name);

        City GetByAltId(Guid altId);

        IEnumerable<City> GetAllByStateId(int stateId);

        IEnumerable<City> SearchByCityName(string cityName);

        City GetByNameAndStateId(string name, long stateId);

        IEnumerable<City> GetByNames(List<string> names);

        IEnumerable<City> SearchByCityNameAndIds(string cityName, IEnumerable<int> cityIds);

        City GetByCityId(long cityId);

        IEnumerable<FIL.Contracts.DataModels.Itinerary> GetAllFeelCityAndCountries();

        IEnumerable<City> GetFeelCityByName(string cityName);

        City GetByNameAndStateIds(string name, IEnumerable<int> ids);

        IEnumerable<City> GetAllByName(string name);

        IEnumerable<FIL.Contracts.Models.Search> GetFeelSearchCityByName(string cityName);
    }

    public class CityRepository : BaseOrmRepository<City>, ICityRepository
    {
        public CityRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public City Get(int id)
        {
            return Get(new City { Id = id });
        }

        public IEnumerable<City> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<City> GetAllByStateId(int stateId)
        {
            return GetAll(s => s.Where($"{nameof(City.StateId):C} = @Id")
                  .WithParameters(new { Id = stateId }));
        }

        public void DeleteCity(City city)
        {
            Delete(city);
        }

        public City SaveCity(City city)
        {
            return Save(city);
        }

        public IEnumerable<City> GetByCityIds(IEnumerable<int> cityIds)
        {
            var cityList = GetAll(statement => statement
                                  .Where($"{nameof(City.Id):C} IN @Ids")
                                  .WithParameters(new { Ids = cityIds }));
            return cityList;
        }

        public City GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(City.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).LastOrDefault();
        }

        public City GetByNameAndStateIds(string name, IEnumerable<int> ids)
        {
            return GetAll(s => s.Where($"{nameof(City.Name):C} = @Name  AND {nameof(City.StateId)} IN @StateIds")
                .WithParameters(new { Name = name, StateIds = ids })
            ).LastOrDefault();
        }

        public IEnumerable<City> GetAllByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(City.Name):C} = @Name")
                .WithParameters(new { Name = name })
            );
        }

        public City GetByNameAndStateId(string name, long stateId)
        {
            return GetAll(s => s.Where($"{nameof(City.Name):C} = @Name AND {nameof(City.StateId)} = @StateId")
                .WithParameters(new { Name = name, StateId = stateId })
            ).FirstOrDefault();
        }

        public City GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(City.AltId):C} = @AltId")
                .WithParameters(new { AltId = altId })
            ).LastOrDefault();
        }

        public IEnumerable<City> SearchByCityName(string cityName)
        {
            var cityList = GetAll(statement => statement
                                  .Where($"{nameof(City.Name):C} LIKE '%'+@Name+'%'")
                                  .WithParameters(new { Name = cityName }));
            return cityList;
        }

        public IEnumerable<City> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(City.Name):C} in @Name")
                .WithParameters(new { Name = names })
                );
        }

        public IEnumerable<City> SearchByCityNameAndIds(string cityName, IEnumerable<int> cityIds)
        {
            return GetAll(s => s.Where($"{nameof(City.Name):C} LIKE '%'+@Name+'%' AND {nameof(City.Id):C} IN @Id ")
            .WithParameters(new { Name = cityName, Id = cityIds })
            );
        }

        public City GetByCityId(long cityId)
        {
            return GetAll(s => s.Where($"{nameof(City.Id):C} = @CityId")
                .WithParameters(new { CityId = cityId })
            ).LastOrDefault();
        }

        public IEnumerable<Itinerary> GetAllFeelCityAndCountries()
        {
            return GetCurrentConnection().Query<Itinerary>("SELECT DISTINCT(ct.name) as CityName ,country.name as CountryName, ct.Id as CityId,st.Id as StateId,st.Name as StateName, country.Id as CountryId from events e " +
                                                        " INNER JOIN eventdetails ed With(NOLOCK) on e.id = ed.eventId " +
                                                        " INNER JOIN venues v With(NOLOCK) on ed.venueId = v.id " +
                                                        " INNER JOIN cities ct With(NOLOCK) on ct.id = v.cityid " +
                                                        " INNER JOIN states st With(NOLOCK) on st.id = ct.stateid " +
                                                        " INNER JOIN countries country With(NOLOCK) on country.id = st.countryId  " +
                                                        "where e.isfeel =1 and e.isenabled =1 ");
        }

        public IEnumerable<City> GetFeelCityByName(string cityName)
        {
            return GetCurrentConnection().Query<City>("select DISTINCT ct.* from events e  WITH (NOLOCK) " +
                                                       " inner join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " inner join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " inner join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " inner join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " inner join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE ct.name LIKE '%'+@CityName+'%' AND e.IsEnabled=1 AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           CityName = cityName,
                                                       });
        }

        public IEnumerable<FIL.Contracts.Models.Search> GetFeelSearchCityByName(string cityName)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.Search>("select Distinct(ct.name) as CityName, ct.id as CityId, country.name as CountryName, country.id as CountryId from events e " +
                                                       " inner join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " inner join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " inner join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " inner join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " inner join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE ct.name LIKE '%'+@CityName+'%' AND e.IsEnabled=1 AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           CityName = cityName,
                                                       });
        }
    }
}