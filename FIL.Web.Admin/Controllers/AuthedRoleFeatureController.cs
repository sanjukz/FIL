using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Commands.Common;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.AuthedRoleFeature;
using FIL.Contracts.Queries.City;
using FIL.Foundation.Senders;
using FIL.Web.Kitms.Feel.ViewModels.AuthedRoleFeature;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class AuthedRoleFeatureController : Controller
    {
        private readonly IQuerySender _querySender;
        public AuthedRoleFeatureController(IQuerySender querySender)
        {
            _querySender = querySender;
        }

        [HttpGet]
        [Route("api/feature/{altId}")]
        public async Task<AuthedFeatureResponseViewModel> GetAuthedRoleFeature(Guid altId)
        {
            var queryResult = await _querySender.Send(new AuthedRoleFeatureQuery
            {
                UserAltId = altId
            });
            return queryResult.Feature != null ? new AuthedFeatureResponseViewModel { Feature = queryResult.Feature } : null;
        }
    }
}
