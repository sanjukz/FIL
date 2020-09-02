using AutoMapper;
using FIL.Foundation.Repositories;
using FIL.Web.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FIL.Web.Core.Providers
{
    public interface ISessionProvider
    {
        Task<SessionViewModel> Get();
    }

    public class SessionProvider : ISessionProvider
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;

        public SessionProvider(IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository,
            IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Meant as an extension point for custom SessionViewModels
        /// ie. override in implementing class and add other values
        /// </summary>
        /// <returns></returns>
        public async Task<SessionViewModel> Get()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            return new SessionViewModel
            {
                AltId = Guid.NewGuid(),
                Success = true,
                HubspotUserToken = GetHubspotUserToken(),
                IsAuthenticated = principal?.Identity.IsAuthenticated ?? false,
                User = await GetUserViewModel()
            };
        }

        protected string GetHubspotUserToken()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var cookies = request?.Cookies;
            if (cookies != null && cookies.ContainsKey("hubspotutk"))
            {
                return cookies["hubspotutk"];
            }
            return null;
        }

        /// <summary>
        /// Meant to allow customizing the user view model in each module
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<UserViewModel> GetUserViewModel()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var claim = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
                var altId = Guid.Parse(claim.Value);

                if (!_memoryCache.TryGetValue($"user_{altId}", out Contracts.Models.User user))
                {
                    user = await _userRepository.Get(altId);
                    _memoryCache.Set($"user_{altId}", user, DateTime.Now.AddMinutes(15));
                }

                if (user != null)
                {
                    var userViewModel = Mapper.Map<UserViewModel>(user);
                    return userViewModel;
                }
            }
            return null;
        }
    }
}
