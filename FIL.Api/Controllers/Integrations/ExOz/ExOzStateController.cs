using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers.Integrations.ExOz
{
    public class ExOzStateController : Controller
    {
        private readonly IExOzStateRepository _exOzStateRepository;

        public ExOzStateController(IExOzStateRepository exOzStateRepository)
        {
            _exOzStateRepository = exOzStateRepository;
        }

        [HttpGet]
        [Route("api/exozstate/all")]
        public IEnumerable<ExOzState> GetAll()
        {
            return _exOzStateRepository.GetAll();
        }

        [HttpGet]
        public ExOzState Get(int id)
        {
            return _exOzStateRepository.Get(id);
        }

        [HttpPost]
        public ExOzState Save(ExOzState exOzState)
        {
            return _exOzStateRepository.Save(exOzState);
        }

        [HttpPost]
        [Route("api/exozstate/delete")]
        public void Delete(ExOzState exOzState)
        {
            _exOzStateRepository.Delete(exOzState);
        }

        [HttpPost]
        [Route("api/exozstate/details")]
        public ExOzState GetDetail(string urlSegment)
        {
            return _exOzStateRepository.GetByUrlSegment(urlSegment);
        }
    }
}