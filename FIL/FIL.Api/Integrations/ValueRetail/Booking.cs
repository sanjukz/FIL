using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Models.Integrations.ValueRetail;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FIL.Api.Integrations.ValueRetail
{
    public interface IBooking : IService
    {
        Task<IResponse<string>> BookCart(BookCartRequest bookCartRequest);
    }

    public class Booking : Service<string>, IBooking
    {
        private IValueRetailAuth _valueRetailAuth;

        public Booking(ILogger logger, ISettings settings, IValueRetailAuth valueRetailAuth) : base(logger, settings)
        {
            _valueRetailAuth = valueRetailAuth;
        }

        public async Task<IResponse<string>> BookCart(BookCartRequest bookCartRequest)
        {
            var _accessToken = await _valueRetailAuth.GetToken();
            var builder = new UriBuilder("https://data0integration0prep0neu.azure-api.net/opdconnect/api/Booking/BookCart");
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

                    var response = await httpClient.PostAsJsonAsync(endpoint, bookCartRequest);
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
    }
}