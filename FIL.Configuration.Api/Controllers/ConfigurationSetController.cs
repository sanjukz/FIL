using FIL.Configuration.Api.Repositories;
using FIL.Configuration.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FIL.Configuration.Api.Controllers
{
    public class ConfigurationSetController : Controller
    {
        private readonly IConfigurationSetRepository _configurationSetRepository;

        public ConfigurationSetController(IConfigurationSetRepository configurationSetRepository)
        {
            _configurationSetRepository = configurationSetRepository;
        }

        [Route("api/configurationset")]
        [HttpGet]
        public IEnumerable<ConfigurationSet> GetAll()
        {
            return _configurationSetRepository.GetAll();
        }
    }
}