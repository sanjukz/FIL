using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Contracts.Models.Integrations.InfiniteAnalytics.Recommendation;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.InfiniteAnalytics.Recommendation
{
    public interface IGetRecommendation : IService
    {
        Task<IResponse<RecommendationResponse>> GetRecommendations(RecommendationModel query);
    }

    public class GetRecommendation : Service<RecommendationResponse>, IGetRecommendation
    {
        public GetRecommendation(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<RecommendationResponse>> GetRecommendations(RecommendationModel query)
        {
            Urls.BaseUrl = "https://feelaplace.infiniteanalytics.com/SocialGenomix";

            IRequestCreateOptions<GetRequestCreateOption> requestCreateOptions = new RequestCreateOptions<GetRequestCreateOption>();
            IHttpResponse httpResponse = await HttpWebRequestProviders<GetRequestCreateOption>.GetWebRequestProviderAsync(string.Format(Urls.InfiniteAnalytics.Recommendation, query.SessionId, query.ClientType, query.SitePageType, query.siteProductId, query.Count), requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                RecommendationResponse recommendations = Mapper<RecommendationResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, recommendations);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to fetch recommendations", httpResponse.WebException));
                return GetResponse(false, null);
            }
        }
    }
}