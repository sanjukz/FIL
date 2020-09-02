using System;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using System.Threading.Tasks;
using FIL.Logging;
using FIL.Web.Feel.ViewModels;
using FIL.Contracts.Queries;

namespace FIL.Web.Feel.Controllers
{
    public class FeelNearbyController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ILogger _logger;
        private readonly Guid ValueRetailUid = Guid.NewGuid();

        public FeelNearbyController(ICommandSender commandSender, IQuerySender querySender, ILogger logger)
        {
            _querySender = querySender;
            _logger = logger;
        }

        [HttpGet()]
        [Route("api/nearby")]
        public async Task<FeelNearbyViewModel> GetNearbyPlaces([FromQuery(Name = "lat")] decimal lat, [FromQuery(Name = "lon")] decimal lon, [FromQuery(Name = "distance")] decimal distance)
        {
            try
            {
                var queryResult = await _querySender.Send(new FeelNearbyQuery
                {
                    Latitude = lat,
                    Longitude = lon,
                    Distance = distance
                });

                if(queryResult.NearbyPlaces != null)
                {
                    return new FeelNearbyViewModel
                    {
                        Latitude = lat,
                        Longitude = lon,
                        NearbyPlaces = queryResult.NearbyPlaces
                    };
                }
                else
                {
                    throw new ArgumentNullException("Error finding nearby places.");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, ex);
                return new FeelNearbyViewModel();
            }
        }
    }
}
