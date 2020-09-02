using FIL.Api.Integrations;
using FIL.Api.Repositories;
using FIL.Configuration;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations;
using FIL.Logging;
using System;

namespace FIL.Api.Providers.Transaction
{
    public interface ISaveIPProvider
    {
        IPDetail SaveIp(string ipAddress);
    }

    public class SaveIPProvider : ISaveIPProvider
    {
        private readonly IIPDetailRepository _iPDetailRepository;
        private readonly IIpApi _ipApi;
        private readonly FIL.Logging.ILogger _logger;

        public SaveIPProvider(ILogger logger, ISettings settings,
                 IIpApi ipApi,
                 IIPDetailRepository iPDetailRepository
            )
        {
            _iPDetailRepository = iPDetailRepository;
            _ipApi = ipApi;
            _logger = logger;
        }

        public IPDetail SaveIp(string ipAddress)
        {
            try
            {
                IPDetail ipDetail = new IPDetail();
                if (!string.IsNullOrWhiteSpace(ipAddress))
                {
                    ipDetail = _iPDetailRepository.GetByIpAddress(ipAddress);
                    if (ipDetail == null)
                    {
                        IResponse<IpApiResponse> ipApiResponse = _ipApi.GetUserLocationByIp(ipAddress).Result;
                        if (ipApiResponse.Result != null)
                        {
                            ipDetail = _iPDetailRepository.Save(new IPDetail
                            {
                                IPAddress = ipAddress,
                                CountryCode = ipApiResponse.Result.CountryCode,
                                CountryName = ipApiResponse.Result.Country,
                                RegionCode = ipApiResponse.Result.Region,
                                RegionName = ipApiResponse.Result.RegionName,
                                City = ipApiResponse.Result.City,
                                Zipcode = ipApiResponse.Result.ZipCode,
                                TimeZone = ipApiResponse.Result.Timezone,
                                Latitude = Convert.ToDecimal(ipApiResponse.Result.Latitude),
                                Longitude = Convert.ToDecimal(ipApiResponse.Result.Longitude)
                            });
                        }
                    }
                }
                return ipDetail;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return null;
            }
        }
    }
}