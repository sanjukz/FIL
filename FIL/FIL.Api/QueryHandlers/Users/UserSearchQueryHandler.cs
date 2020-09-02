using AutoMapper;
using FIL.Api.Repositories;
using FIL.Contracts.DataModels;
using FIL.Contracts.Queries.User;
using FIL.Contracts.QueryResults.User;

namespace FIL.Api.QueryHandlers.Users
{
    public class UserSearchQueryHandler : IQueryHandler<UserSearchQuery, UserSearchQueryResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;

        public UserSearchQueryHandler(IUserRepository userRepository, ICountryRepository countryRepository)
        {
            _userRepository = userRepository;
            _countryRepository = countryRepository;
        }

        public UserSearchQueryResult Handle(UserSearchQuery query)
        {
            var response = new UserSearchQueryResult();
            var user = new User();
            if (query.SignUpMethodId == Contracts.Enums.SignUpMethods.Facebook)
            {
                user = _userRepository.GetBySocialIdAndChannel(query.SocialLoginId, query.ChannelId);
            }
            else
            {
                if (query.ChannelId == Contracts.Enums.Channels.Feel)
                {
                    user = _userRepository.GetByEmailAndChannel(query.Email, query.ChannelId, null);
                }
                else
                {
                    user = query.ChannelId == null && query.SignUpMethodId == null ? _userRepository.GetByEmail(query.Email) : _userRepository.GetByEmailAndChannel(query.Email, query.ChannelId, query.SignUpMethodId);
                }
            }
            if (user != null)
            {
                response.Success = true;
                response.User = Mapper.Map<Contracts.Models.User>(user);

                if (query.PhoneCode != null)
                {
                    var country = _countryRepository.GetByPhoneCode(query.PhoneCode.Split("~")[0]);
                    response.Country = country.CountryName;
                }
                else if (!string.IsNullOrEmpty(user.PhoneCode))
                {
                    var country = _countryRepository.GetByPhoneCode(user.PhoneCode);
                    response.Country = country.CountryName;
                }
            }
            else
            {
                if (query.PhoneCode != null)
                {
                    var country = _countryRepository.GetByPhoneCode(query.PhoneCode.Split("~")[0]);
                    response.Country = country.CountryName;
                }
                else if (user != null && !string.IsNullOrEmpty(user.PhoneCode))
                {
                    var country = _countryRepository.GetByPhoneCode(user.PhoneCode);
                    response.Country = country.CountryName;
                }
                response.Success = false;
            }
            return response;
        }
    }
}