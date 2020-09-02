using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Api.Controllers
{
    public class StateController : Controller
    {
        private readonly IStateRepository _stateRepository;

        public StateController(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        [HttpGet]
        [Route("api/state/all")]
        public IEnumerable<State> GetAll()
        {
            return _stateRepository.GetAll();
        }

        [HttpGet]
        public State Get(int id)
        {
            return _stateRepository.Get(id);
        }

        [HttpPost]
        public State Save(State state)
        {
            return _stateRepository.Save(state);
        }

        [HttpPost]
        [Route("api/state/delete")]
        public void Delete(State state)
        {
            _stateRepository.Delete(state);
        }
    }
}