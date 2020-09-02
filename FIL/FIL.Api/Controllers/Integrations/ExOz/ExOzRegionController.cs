using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers.Integrations.ExOz
{
    public class ExOzRegionController : Controller
    {
        private readonly IExOzRegionRepository _exOzRegionRepository;

        public ExOzRegionController(IExOzRegionRepository exOzRegionRepository)
        {
            _exOzRegionRepository = exOzRegionRepository;
        }

        [HttpGet]
        [Route("api/exozregion/all")]
        public IEnumerable<ExOzRegion> GetAll()
        {
            return _exOzRegionRepository.GetAll();
        }

        [HttpGet]
        public ExOzRegion Get(int id)
        {
            return _exOzRegionRepository.Get(id);
        }

        [HttpPost]
        public ExOzRegion Save(ExOzRegion exOzRegion)
        {
            return _exOzRegionRepository.Save(exOzRegion);
        }

        [HttpPost]
        [Route("api/exozRegion/delete")]
        public void Delete(ExOzRegion exOzRegion)
        {
            _exOzRegionRepository.Delete(exOzRegion);
        }

        [HttpPost]
        [Route("api/exozRegion/details")]
        public ExOzRegion GetDetail(string urlSegment)
        {
            return _exOzRegionRepository.GetByUrlSegment(urlSegment);
        }
    }
}