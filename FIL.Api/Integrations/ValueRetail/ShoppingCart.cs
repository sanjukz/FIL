using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.ValueRetail;
using FIL.Contracts.Models.ValueRetail;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.ValueRetail
{
    public interface IShoppingCart : IService
    {
        Task<IResponse<string>> CreateShoppingCart(Village village);

        Task<IResponse<string>> UpsertShoppingCart(UpsertShoppingCartRequest upsertShoppingCartRequest);
    }

    public class ShoppingCart : Service<string>, IShoppingCart
    {
        private IValueRetailAuth _valueRetailAuth;

        public ShoppingCart(ILogger logger, ISettings settings, IValueRetailAuth valueRetailAuth) : base(logger, settings)
        {
            _valueRetailAuth = valueRetailAuth;
        }

        public async Task<IResponse<string>> CreateShoppingCart(Village village)
        {
            var _accessToken = await _valueRetailAuth.GetToken();
            var builder = new UriBuilder("https://data0integration0prep0neu.azure-api.net/opdconnect/api/ShoppingCart/CreatCart");
            builder.Port = -1;
            string endpoint = builder.ToString();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", "8ace1302001848ed9a311fca09ef8909");

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Result);

                    var response = await httpClient.PostAsJsonAsync(endpoint, village);
                    var responseString = await response.Content.ReadAsStringAsync();
                    return GetResponse(true, responseString);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to create shopping cart", ex));
                return GetResponse(false, null);
            }
        }

        public async Task<IResponse<string>> UpsertShoppingCart(UpsertShoppingCartRequest upsertShoppingCartRequest)
        {
            var _accessToken = await _valueRetailAuth.GetToken();
            var builder = new UriBuilder("https://data0integration0prep0neu.azure-api.net/opdconnect/api/ShoppingCart/UpsertItem");
            builder.Port = -1;
            string endpoint = builder.ToString();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", "8ace1302001848ed9a311fca09ef8909");

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.Result);

                    var response = await httpClient.PutAsJsonAsync(endpoint, upsertShoppingCartRequest);
                    var responseString = await response.Content.ReadAsStringAsync();
                    return GetResponse(true, responseString);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogCategory.Error, new Exception("Failed to upsert Item in shopping cart", ex));
                return GetResponse(false, null);
            }
        }
    }
}