using FIL.Api.Integrations;
using FIL.Contracts.Models;
using FIL.Contracts.Models.Integrations;
using FIL.Contracts.Queries.CustomerIpDetails;
using FIL.Contracts.QueryResults.CustomerIpDetails;
using System;

namespace FIL.Api.QueryHandlers.CustomerIpDetails
{
    public class CustomerIpDetailQueryHandler : IQueryHandler<CustomerIpDetailQuery, CustomerIpDetailQueryResult>
    {
        private readonly IIpApi _ipApi;

        public CustomerIpDetailQueryHandler(IIpApi ipApi)
        {
            _ipApi = ipApi;
        }

        public CustomerIpDetailQueryResult Handle(CustomerIpDetailQuery query)
        {
            IPDetail ipDetail = new IPDetail();
            IResponse<IpApiResponse> ipDetails = _ipApi.GetUserLocationByIp(query.Ip).Result;
            if (ipDetails.Result != null)
            {
                ipDetail = new IPDetail
                {
                    IPAddress = query.Ip,
                    CountryCode = ipDetails.Result.CountryCode,
                    CountryName = ipDetails.Result.Country,
                    RegionCode = ipDetails.Result.Region,
                    RegionName = ipDetails.Result.RegionName,
                    City = ipDetails.Result.City,
                    Zipcode = ipDetails.Result.ZipCode,
                    TimeZone = ipDetails.Result.Timezone,
                    Latitude = Convert.ToDecimal(ipDetails.Result.Latitude),
                    Longitude = Convert.ToDecimal(ipDetails.Result.Longitude)
                };
            }
            return new CustomerIpDetailQueryResult
            {
                IpDetail = ipDetail
            };
        }
    }
}