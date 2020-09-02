using FIL.Api.Integrations.HttpHelpers;
using FIL.Configuration;
using FIL.Contracts.Models.Integrations;
using FIL.Logging;
using FIL.Logging.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FIL.Api.Integrations
{
    public interface ICountryAlphaCode : IService
    {
        Task<IResponse<CountryDetailResult>> GetCountryCodeByName(string countryName);
    }

    public class CountryAlphaCode : Service<CountryDetailResult>, ICountryAlphaCode
    {
        private Uri uri = new Uri("https://restcountries.eu/rest/v2/all");

        public CountryAlphaCode(ILogger logger, ISettings settings)
            : base(logger, settings)
        {
        }

        public async Task<IResponse<CountryDetailResult>> GetCountryCodeByName(string countryName)
        {
            string responseString = string.Empty;
            try
            {
                using (HttpClient webClient = new HttpClient())
                {
                    HttpResponseMessage response = await webClient.GetAsync(uri);
                    responseString = await response.Content.ReadAsStringAsync();
                }

                var _Data = Mapper<IList<CountryDetailResponse>>.MapJsonStringToObject(responseString);
                var filteredCountry = _Data.Where(s => s.name == countryName || s.nativeName == countryName);
                var countryDetailResult = new CountryDetailResult
                {
                    IsoAlphaTwoCode = filteredCountry.ToList()[0].alpha2Code,
                    IsoAlphaThreeCode = filteredCountry.ToList()[0].alpha2Code,
                    CurrecyName = filteredCountry.ToList()[0].currencies[0].name,
                    CurrencyCode = filteredCountry.ToList()[0].currencies[0].code,
                    CurrencySymbol = filteredCountry.ToList()[0].currencies[0].symbol
                };

                return GetResponse(true, countryDetailResult);
            }
            catch (Exception)
            {
                _logger.Log(LogCategory.Error, new Exception($"Failed to get country details from Country Rest API for {countryName}"));
                return GetResponse(false, new CountryDetailResult());
            }
        }
    }
}