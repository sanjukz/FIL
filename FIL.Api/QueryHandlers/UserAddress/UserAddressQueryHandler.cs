using FIL.Api.Repositories;
using FIL.Contracts.Queries.UserAddress;
using FIL.Contracts.QueryResults.UserAddress;
using System.Collections.Generic;

namespace FIL.Api.QueryHandlers.UserAddress
{
    public class UserAddressQueryHandler : IQueryHandler<UserAddressQuery, UserAddressQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressDetailRepository _userAddressDetailRepository;
        private readonly IZipcodeRepository _zipcodeRepository;
        private readonly FIL.Logging.ILogger _logger;

        public UserAddressQueryHandler(IUserRepository userRepository, IUserAddressDetailRepository userAddressDetailRepository, IZipcodeRepository zipcodeRepository, FIL.Logging.ILogger logger)
        {
            _userRepository = userRepository;
            _userAddressDetailRepository = userAddressDetailRepository;
            _zipcodeRepository = zipcodeRepository;
            _logger = logger;
        }

        public UserAddressQueryResult Handle(UserAddressQuery query)
        {
            List<FIL.Contracts.Models.UserAddress> userAddresses = new List<FIL.Contracts.Models.UserAddress>();
            var user = _userRepository.GetByAltId(query.UserAltId);
            if (user == null)
            {
                return new UserAddressQueryResult();
            }
            else
            {
                var addressList = _userAddressDetailRepository.GetByUserId(user.Id);

                if (query.AddressTypeId == Contracts.Enums.AddressTypes.None)
                {
                    return new UserAddressQueryResult
                    {
                        UserAddresses = AutoMapper.Mapper.Map<List<FIL.Contracts.Models.UserAddress>>(addressList)
                    };
                }
                else
                {
                    foreach (var item in addressList)
                    {
                        if (item.AddressTypeId == query.AddressTypeId)
                        {
                            userAddresses.Add(new FIL.Contracts.Models.UserAddress
                            {
                                AltId = item.AltId,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                PhoneCode = item.PhoneCode,
                                PhoneNumber = item.PhoneNumber,
                                AddressLine1 = item.AddressLine1,
                                Zipcode = _zipcodeRepository.Get(item.Zipcode).Postalcode, // XXX: TODO: how do we use automapper here
                                AddressTypeId = item.AddressTypeId,
                                IsDefault = item.IsDefault,
                            });
                        }
                    }

                    return new UserAddressQueryResult
                    {
                        UserAddresses = userAddresses
                    };
                }
            }
        }
    }
}