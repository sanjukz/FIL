using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers.Integrations.ExOz
{
    public class ExOzCountryController : Controller
    {
        private readonly IExOzCountryRepository _exOzCountryRepository;

        public ExOzCountryController(IExOzCountryRepository exOzCountryRepository)
        {
            _exOzCountryRepository = exOzCountryRepository;
        }

        [HttpGet]
        [Route("api/exozcountry/all")]
        public IEnumerable<ExOzCountry> GetAll()
        {
            return _exOzCountryRepository.GetAll();
        }

        [HttpGet]
        public ExOzCountry Get(int id)
        {
            return _exOzCountryRepository.Get(id);
        }

        [HttpPost]
        public ExOzCountry Save(ExOzCountry exOzCountry)
        {
            return _exOzCountryRepository.Save(exOzCountry);
        }

        [HttpPost]
        [Route("api/exozcountry/delete")]
        public void Delete(ExOzCountry exOzCountry)
        {
            _exOzCountryRepository.Delete(exOzCountry);
        }
    }
}