using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Configuration.Utilities;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.DTCM;
using FIL.Contracts.Models.Integrations.HttpHelpers;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.DTCM.CustomerServices
{
    public interface ICreateCustomer : IService
    {
        Task<IResponse<CustomerResponse>> CreateCustomerAsync(CustomerCreateOption customerCreateOption, AccessToken accessToken);
    }

    public class CreateCustomer : Service<CustomerResponse>, ICreateCustomer
    {
        public CreateCustomer(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<CustomerResponse>> CreateCustomerAsync(CustomerCreateOption customerCreateOption, AccessToken accessToken)
        {
            Urls.BaseUrl = _settings.GetConfigSetting<string>(SettingKeys.Integration.DTCM.BaseUrl);

            IRequestCreateOptions<CustomerCreateOption> requestCreateOptions = new RequestCreateOptions<CustomerCreateOption>();
            requestCreateOptions.Content = customerCreateOption;
            requestCreateOptions.Header = new Dictionary<string, string>()
            {
                { "Token", $"{accessToken.Token}" }
            };
            requestCreateOptions.UserAgent = "zoonga.com (http://www.zoonga.com/)";

            IHttpResponse httpResponse = await HttpWebRequestProviders<CustomerCreateOption>.PostBearerWebRequestProvider(Urls.CreateCustomer, requestCreateOptions).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(httpResponse.Response))
            {
                CustomerResponse customerResponse = Mapper<CustomerResponse>.MapJsonStringToObject(httpResponse.Response);
                return GetResponse(true, customerResponse);
            }
            else
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create customer", httpResponse.WebException));
            }
            return GetResponse(false, null);
        }
    }
}