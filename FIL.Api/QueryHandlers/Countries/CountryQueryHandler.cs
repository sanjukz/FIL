using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Country;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.QueryHandlers.Countries
{
    public class CountryQueryHandler : IQueryHandler<CountryQuery, CountryQueryResult>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly FIL.Logging.ILogger _logger;

        public CountryQueryHandler(ICountryRepository countryRepository, FIL.Logging.ILogger logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        public CountryQueryResult Handle(CountryQuery query)
        {
            List<Country> countries = new List<Country>();
            var country = _countryRepository.GetAll(null);
            country = country.GroupBy(x => x.Name, (key, group) => group.First()).ToList();
            try
            {
                foreach (var item in country)
                {
                    if (item.IsEnabled == true && item.Phonecode != null)
                    {
                        countries.Add(new Country
                        {
                            Id = item.Id,
                            AltId = item.AltId,
                            Name = item.Name,
                            IsoAlphaTwoCode = item.IsoAlphaTwoCode,
                            IsoAlphaThreeCode = item.IsoAlphaThreeCode,
                            Numcode = item.Numcode,
                            Phonecode = item.Phonecode
                        });
                    }
                }
                return new CountryQueryResult
                {
                    Countries = countries
                };
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new CountryQueryResult
                {
                    Countries = null
                };
            }
        }
    }
}