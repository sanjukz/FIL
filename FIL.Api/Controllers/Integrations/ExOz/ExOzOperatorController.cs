using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers.Integrations.ExOz
{
    public class ExOzOperatorController : Controller
    {
        private readonly IExOzOperatorRepository _exOzOperatorRepository;

        public ExOzOperatorController(IExOzOperatorRepository exOzOperatorRepository)
        {
            _exOzOperatorRepository = exOzOperatorRepository;
        }

        [HttpGet]
        [Route("api/exozoperator/all")]
        public IEnumerable<ExOzOperator> GetAll()
        {
            return _exOzOperatorRepository.GetAll();
        }

        [HttpGet]
        public ExOzOperator Get(int id)
        {
            return _exOzOperatorRepository.Get(id);
        }

        [HttpPost]
        public ExOzOperator Save(ExOzOperator exOzOperator)
        {
            return _exOzOperatorRepository.Save(exOzOperator);
        }

        [HttpPost]
        [Route("api/exozoperator/delete")]
        public void Delete(ExOzOperator exOzOperator)
        {
            _exOzOperatorRepository.Delete(exOzOperator);
        }

        [HttpPost]
        [Route("api/exozoperator/details")]
        public ExOzOperator GetDetail(string urlSegment)
        {
            return _exOzOperatorRepository.GetByUrlSegment(urlSegment);
        }
    }
}