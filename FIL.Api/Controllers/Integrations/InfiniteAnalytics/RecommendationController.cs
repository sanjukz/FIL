using FIL.Api.Integrations.InfiniteAnalytics.Recommendation;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.InfiniteAnalytics.Recommendation;
using Microsoft.AspNetCore.Mvc;

namespace FIL.Api.Controllers
{
    public class RecommendationController : Controller
    {
        private readonly IGetRecommendation _getRecommendation;

        public RecommendationController(IGetRecommendation getRecommendation)
        {
            _getRecommendation = getRecommendation;
        }

        [HttpGet]
        [Route("api/infiniteanalytics/recommendation")]
        public IResponse<RecommendationResponse> GetRecommendation()
        {
            RecommendationModel rq = new RecommendationModel
            {
                ClientType = "web_site",
                Count = 4,
                SessionId = "c9f7f47db1c2d6faf423bd3e9fc4e956",
                SitePageType = "product_detail",
                siteProductId = "34"
            };
            return _getRecommendation.GetRecommendations(rq).Result;
        }
    }
}