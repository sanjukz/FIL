using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers.Integrations.ExOz
{
    public class ExOzProductController : Controller
    {
        private readonly IExOzProductRepository _exOzProductRepository;

        public ExOzProductController(IExOzProductRepository exOzProductRepository)
        {
            _exOzProductRepository = exOzProductRepository;
        }

        [HttpGet]
        [Route("api/exozproduct/all")]
        public IEnumerable<ExOzProduct> GetAll()
        {
            return _exOzProductRepository.GetAll();
        }

        [HttpGet]
        public ExOzProduct Get(int id)
        {
            return _exOzProductRepository.Get(id);
        }

        [HttpPost]
        public ExOzProduct Save(ExOzProduct exOzProduct)
        {
            return _exOzProductRepository.Save(exOzProduct);
        }

        [HttpPost]
        [Route("api/exozproduct/delete")]
        public void Delete(ExOzProduct exOzProduct)
        {
            _exOzProductRepository.Delete(exOzProduct);
        }

        [HttpPost]
        [Route("api/exozproduct/details")]
        public ExOzProduct GetDetail(string urlSegment)
        {
            return _exOzProductRepository.GetByUrlSegment(urlSegment);
        }
    }
}