using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [Route("api/country/all")]
        public IEnumerable<Country> GetAll()
        {
            return _countryRepository.GetAll();
        }

        [HttpGet]
        [Route("api/country/get/{id}")]
        public Country Get(int id)
        {
            return _countryRepository.Get(id);
        }

        [HttpPost]
        [Route("api/country/save")]
        public Country Save([FromBody] Country country)
        {
            return _countryRepository.Save(country);
        }

        [HttpPost]
        [Route("api/country/delete")]
        public void Delete([FromBody] Country country)
        {
            _countryRepository.Delete(country);
        }
    }
}