using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface ICountryRepository : IOrmRepository<Country, Country>
    {
        Country Get(int id);

        Country GetByName(string Name);

        Country GetByAltId(Guid altId);

        IEnumerable<Country> GetByNames(List<string> names);

        Country GetByPhoneCode(string phoneCode);

        IEnumerable<Country> GetByCountryIds(IEnumerable<int> countryIds);

        IEnumerable<Country> SearchByCountryNameAndIds(string countryName, IEnumerable<int> countryIds);

        Country GetByCountryId(long countryId);

        IEnumerable<FIL.Contracts.DataModels.CountryPlace> GetAllCountryPlace();

        IEnumerable<Country> GetFeelCountryByName(string countryName);

        IEnumerable<CountryPlace> GetAllCountryPlaceCountByEventCategoryId(int parentCategoryId);

        IEnumerable<FIL.Contracts.Models.Search> GetFeelSearchCountryByName(string countryName);
    }

    public class CountryRepository : BaseOrmRepository<Country>, ICountryRepository
    {
        public CountryRepository(IDataSettings dataSettings)
            : base(dataSettings)
        {
        }

        public Country Get(int id)
        {
            return Get(new Country { Id = id });
        }

        public IEnumerable<Country> GetAll()
        {
            return GetAll(null);
        }

        public void DeleteCountry(Country country)
        {
            Delete(country);
        }

        public Country SaveCountry(Country country)
        {
            return Save(country);
        }

        public Country GetByName(string countryName)
        {
            return GetAll(s => s.Where($"{nameof(Country.CountryName):C} = @Name")
                 .WithParameters(new { Name = countryName })
             ).FirstOrDefault();
        }

        public Country GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(Country.AltId):C}=@AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<Country> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(Country.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }

        public Country GetByPhoneCode(string phoneCode)
        {
            return GetAll(s => s.Where($"{nameof(Country.Phonecode):C}=@PhoneCode")
            .WithParameters(new { PhoneCode = phoneCode })).FirstOrDefault();
        }

        public IEnumerable<Country> GetByCountryIds(IEnumerable<int> countryIds)
        {
            var countryList = GetAll(statement => statement
                                  .Where($"{nameof(Country.Id):C} IN @Ids")
                                  .WithParameters(new { Ids = countryIds }));
            return countryList;
        }

        public IEnumerable<Country> SearchByCountryNameAndIds(string countryName, IEnumerable<int> countryIds)
        {
            return GetAll(s => s.Where($"{nameof(Country.Name):C} LIKE '%'+@Name+'%' AND {nameof(Country.Id):C} IN @Id ")
            .WithParameters(new { Name = countryName, Id = countryIds })
            );
        }

        public Country GetByCountryId(long countryId)
        {
            return GetAll(s => s.Where($"{nameof(Country.Id):C}=@CountryId")
                .WithParameters(new { CountryId = countryId })
            ).FirstOrDefault();
        }

        public IEnumerable<CountryPlace> GetAllCountryPlace()
        {
            return GetCurrentConnection().Query<CountryPlace>("select Distinct(country.name) as Name,country.AltId, country.Id, count(distinct(e.name)) as Count from events e " +
                                                        " inner join eventdetails ed With(NOLOCK) on e.id = ed.eventId " +
                                                        " Inner Join Eventcategorymappings A With(NoLOck) ON e.id = A.eventId " +
                                                        " inner join venues v With(NOLOCK) on ed.venueId = v.id " +
                                                        " inner join cities ct With(NOLOCK) on ct.id = v.cityid " +
                                                        " inner join states st With(NOLOCK) on st.id = ct.stateid " +
                                                        " inner join countries country With(NOLOCK) on country.id = st.countryId " +
                                                        "where e.isfeel =1 and e.isenabled =1 group by country.name, country.altId,  country.Id ");
        }

        public IEnumerable<CountryPlace> GetAllCountryPlaceCountByEventCategoryId(int parentCategoryId)
        {
            return GetCurrentConnection().Query<CountryPlace>("select Distinct(country.name) as Name,country.AltId, country.Id, count(distinct(e.id)) as Count from Eventcategorymappings A With(NoLOck) " +
                                                        " Left Outer Join Events E With(NoLOck) ON A.EventId=E.Id " +
                                                        " inner join eventdetails ed With(NOLOCK) on e.id = ed.eventId  " +
                                                        " inner join venues v With(NOLOCK) on ed.venueId = v.id  " +
                                                        " inner join cities ct With(NOLOCK) on ct.id = v.cityid " +
                                                        " inner join states st With(NOLOCK) on st.id = ct.stateid  " +
                                                        " inner join countries country With(NOLOCK) on country.id = st.countryId " +
                                                        "where e.isfeel =1 and ed.isenabled=1 and  A.isenabled=1 and e.isenabled =1 and   A.eventcategoryid in(select id from eventcategories where eventcategoryId=@eventCategoryId)  group by country.name, country.altId, country.Id "
                                                        , new
                                                        {
                                                            eventCategoryId = parentCategoryId
                                                        });
        }

        public IEnumerable<Country> GetFeelCountryByName(string countryName)
        {
            return GetCurrentConnection().Query<Country>("select DISTINCT country.* from events e  WITH (NOLOCK) " +
                                                       " inner join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " inner join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " inner join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " inner join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " inner join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE country.name LIKE '%'+@country+'%' AND e.IsEnabled=1 AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           country = countryName,
                                                       });
        }

        public IEnumerable<FIL.Contracts.Models.Search> GetFeelSearchCountryByName(string countryName)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.Search>("select Distinct(country.name) as CountryName, country.id as CountryId from events e WITH (NOLOCK) " +
                                                       " inner join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " inner join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " inner join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " inner join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " inner join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE country.name LIKE '%'+@country+'%' AND e.IsEnabled=1 AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           country = countryName,
                                                       });
        }
    }
}