using FIL.Api.Repositories;
using FIL.Contracts.Models;
using FIL.Contracts.Queries.Zipcode;
using FIL.Contracts.QueryResults;
using System;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.Zipcodes
{
    public class ZipcodeQueryHandler : IQueryHandler<ZipcodeQuery, ZipcodeQueryResult>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IZipcodeRepository _zipRepository;
        private readonly FIL.Logging.ILogger _logger;

        public ZipcodeQueryHandler(ICityRepository cityRepository, IZipcodeRepository zipRepository, FIL.Logging.ILogger logger)
        {
            _cityRepository = cityRepository;
            _zipRepository = zipRepository;
            _logger = logger;
        }

        public ZipcodeQueryResult Handle(ZipcodeQuery query)
        {
            List<Zipcode> zipcodes = new List<Zipcode>();
            var city = _cityRepository.GetByAltId(query.CityAltId);
            if (city == null)
            {
                return new ZipcodeQueryResult
                {
                    Zipcodes = new List<Zipcode>(),
                };
            }
            else
            {
                var zipcode = _zipRepository.GetAllByCityId(city.Id);
                try
                {
                    foreach (var item in zipcode)
                    {
                        zipcodes.Add(new Zipcode
                        {
                            AltId = item.AltId,
                            Postalcode = item.Postalcode,
                            Region = item.Region,
                            CityId = city.Id
                        });
                    }
                    return new ZipcodeQueryResult
                    {
                        Zipcodes = zipcodes
                    };
                }
                catch (Exception ex)
                {
                    _logger.Log(Logging.Enums.LogCategory.Error, ex);
                    return new ZipcodeQueryResult
                    {
                        Zipcodes = null
                    };
                }
            }
        }
    }
}