using Dapper;
using FIL.Api.Core.Repositories;
using FIL.Api.Core.Utilities;
using FIL.Contracts.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Repositories
{
    public interface IStateRepository : IOrmRepository<State, State>
    {
        State Get(int id);

        State GetByName(string name);

        State GetByAltId(Guid altId);

        IEnumerable<State> GetAllByCountryId(int countryId);

        State GetByNameAndCountryId(string name, long countryId);

        IEnumerable<State> GetByNames(List<string> names);

        IEnumerable<State> GetByStateIds(IEnumerable<int> stateIds);

        IEnumerable<State> SearchByStateName(string stateName);

        IEnumerable<State> SearchByStateNameAndIds(string stateName, IEnumerable<int> stateIds);

        State GetByStateId(long stateId);

        IEnumerable<FIL.Contracts.DataModels.CountryPlace> GetAllStatePlaceByCountry(string countryName);

        IEnumerable<FIL.Contracts.Models.Search> GetFeelSearchStateByName(string stateName);

        IEnumerable<FeelState> GetAllFeelStateAndCountries();
    }

    public class StateRepository : BaseOrmRepository<State>, IStateRepository
    {
        public StateRepository(IDataSettings dataSettings) : base(dataSettings)
        {
        }

        public State Get(int id)
        {
            return Get(new State { Id = id });
        }

        public IEnumerable<State> GetAll()
        {
            return GetAll(null);
        }

        public IEnumerable<State> GetAllByCountryId(int countryId)
        {
            return GetAll(s => s.Where($"{nameof(State.CountryId):C} = @Id")
                  .WithParameters(new { Id = countryId }));
        }

        public void DeleteState(State state)
        {
            Delete(state);
        }

        public State SaveState(State state)
        {
            return Save(state);
        }

        public State GetByName(string name)
        {
            return GetAll(s => s.Where($"{nameof(State.Name):C} = @Name")
                .WithParameters(new { Name = name })
            ).FirstOrDefault();
        }

        public State GetByNameAndCountryId(string name, long countryId)
        {
            return GetAll(s => s.Where($"{nameof(State.Name):C} = @Name AND {nameof(State.CountryId)} = @CountryId")
                .WithParameters(new { Name = name, CountryId = countryId })
            ).FirstOrDefault();
        }

        public State GetByAltId(Guid altId)
        {
            return GetAll(s => s.Where($"{nameof(State.AltId):C} = @AltId")
                .WithParameters(new { AltId = altId })
            ).FirstOrDefault();
        }

        public IEnumerable<State> GetByNames(List<string> names)
        {
            return GetAll(s => s.Where($"{nameof(State.Name):C} in @Name")
                .WithParameters(new { Name = names })
            );
        }

        public IEnumerable<State> GetByStateIds(IEnumerable<int> stateIds)
        {
            var stateList = GetAll(statement => statement.Where($"{nameof(State.Id):C} IN @Ids")
            .WithParameters(new { Ids = stateIds }));
            return stateList;
        }

        public IEnumerable<State> SearchByStateName(string stateName)
        {
            var stateList = GetAll(statement => statement
                                  .Where($"{nameof(State.Name):C} LIKE '%'+@Name+'%'")
                                  .WithParameters(new { Name = stateName }));
            return stateList;
        }

        public IEnumerable<State> SearchByStateNameAndIds(string stateName, IEnumerable<int> stateIds)
        {
            return GetAll(s => s.Where($"{nameof(State.Name):C} LIKE '%'+@Name+'%' AND {nameof(State.Id):C} IN @Id ")
            .WithParameters(new { Name = stateName, Id = stateIds })
            );
        }

        public State GetByStateId(long stateId)
        {
            return GetAll(s => s.Where($"{nameof(State.Id):C} = @StateId")
                .WithParameters(new { StateId = stateId })
            ).FirstOrDefault();
        }

        public IEnumerable<FeelState> GetAllFeelStateAndCountries()
        {
            return GetCurrentConnection().Query<FeelState>("SELECT DISTINCT(st.name) as StateName, st.Id as StateId ,country.name as CountryName from events e " +
                                                        " INNER JOIN eventdetails ed With(NOLOCK) on e.id = ed.eventId " +
                                                        " INNER JOIN venues v With(NOLOCK) on ed.venueId = v.id " +
                                                        " INNER JOIN cities ct With(NOLOCK) on ct.id = v.cityid " +
                                                        " INNER JOIN states st With(NOLOCK) on st.id = ct.stateid " +
                                                        " INNER JOIN countries country With(NOLOCK) on country.id = st.countryId  " +
                                                        "where e.isfeel =1 and e.isenabled =1 ");
        }

        public IEnumerable<CountryPlace> GetAllStatePlaceByCountry(string countryName)
        {
            return GetCurrentConnection().Query<CountryPlace>("select Distinct(st.name) as Name,st.altId, count(distinct(e.name)) as Count from events e " +
                                                       " inner join eventdetails ed on e.id = ed.eventId " +
                                                       " inner join venues v on ed.venueId = v.id " +
                                                       " inner join cities ct on ct.id = v.cityid " +
                                                       " inner join states st on st.id = ct.stateid " +
                                                       " inner join countries country on country.id = st.countryId " +
                                                       "WHERE country.name LIKE '%'+@CountryName+'%' AND e.IsEnabled=1 AND e.IsFeel=1 group by st.name, st.altid"
                                                       , new
                                                       {
                                                           CountryName = countryName,
                                                       });
        }

        public IEnumerable<FIL.Contracts.Models.Search> GetFeelSearchStateByName(string stateName)
        {
            return GetCurrentConnection().Query<FIL.Contracts.Models.Search>("select Distinct(st.name) as StateName,st.id as StateId, country.name as CountryName, country.id as CountryId from events e " +
                                                       " inner join eventdetails ed WITH (NOLOCK) on e.id = ed.eventId  " +
                                                       " inner join venues v WITH (NOLOCK) on ed.venueId = v.id  " +
                                                       " inner join cities ct WITH (NOLOCK) on ct.id = v.cityid  " +
                                                       " inner join states st WITH (NOLOCK) on st.id = ct.stateid  " +
                                                       " inner join countries country WITH (NOLOCK) on country.id = st.countryId " +
                                                       "WHERE st.name LIKE '%'+@state+'%' AND e.IsEnabled=1 AND e.IsFeel=1 "
                                                       , new
                                                       {
                                                           state = stateName,
                                                       });
        }
    }
}