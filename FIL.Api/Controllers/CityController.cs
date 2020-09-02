using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpGet]
        [Route("api/city/all")]
        public IEnumerable<City> GetAll()
        {
            return _cityRepository.GetAll();
        }

        [HttpGet]
        public City Get(int id)
        {
            return _cityRepository.Get(id);
        }

        [HttpPost]
        public City Save(City city)
        {
            return _cityRepository.Save(city);
        }

        [HttpPost]
        [Route("api/city/delete")]
        public void Delete(City city)
        {
            _cityRepository.Delete(city);
        }
    }
}