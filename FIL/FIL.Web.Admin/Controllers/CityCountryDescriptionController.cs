using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIL.Foundation.Senders;
using FIL.Web.Core.Providers;
using Microsoft.Extensions.Caching.Memory;
using FIL.Web.Kitms.Feel.ViewModels.CityCountryDescription;
using FIL.Contracts.Commands.Description;
using FIL.Contracts.Queries.Description;
using Microsoft.AspNetCore.Authorization;

namespace FIL.Web.Kitms.Feel.Controllers
{
    public class CityCountryDescriptionController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly ICommandSender _commandSender;
        private readonly IMemoryCache _memoryCache;
        private readonly ISessionProvider _sessionProvider;
        private readonly ISiteIdProvider _siteIdProvider;

        public CityCountryDescriptionController(ICommandSender commandSender, IQuerySender querySender, ISessionProvider sessionProvider, IMemoryCache memoryCache, ISiteIdProvider siteIdProvider)
        {
            _querySender = querySender;
            _memoryCache = memoryCache;
            _siteIdProvider = siteIdProvider;
            _sessionProvider = sessionProvider;
            _commandSender = commandSender;
        }

        [HttpGet]
        [Route("api/active/cities")]
        public async Task<SearchResponseViewModel> Get()
        {
            var queryResult = await _querySender.Send(new Contracts.Queries.Itinerary.ItineraryQuery
            {

            });
            return new SearchResponseViewModel
            {
                ItinerarySerchData = queryResult.ItinerarySearchData,
                FeelStateData = queryResult.FeelStateData
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/cityCountryDescription/save")]
        public async Task<DescriptionResponseViewModel> SaveDescription([FromBody]DescriptionInputViewModel model)
        {
            var session = await _sessionProvider.Get();
            try
            {
                if (!model.IsStateDescription)
                {
                    var queryResult = await _querySender.Send(new DescriptionQuery
                    {
                        IsCountryDescription = model.IsCountryDescription,
                        Name = model.Name,
                        IsCityDescription = model.IsCityDescription,
                        IsStateDescription = model.IsStateDescription,
                        StateId = model.StateId
                    });


                    DescriptionCommandResult EventData = await _commandSender.Send<DescriptionCommand, DescriptionCommandResult>(new DescriptionCommand
                    {
                        CreatedBy = session.User.AltId,
                        Description = model.Description,
                        IsCountryDescription = model.IsCountryDescription,
                        IsStateDescription = model.IsStateDescription,
                        IsCityDescription = model.IsCityDescription,
                        Name = model.Name,
                        City = queryResult.City,
                        Country = queryResult.Country
                    });
                }
                else
                {
                    DescriptionCommandResult EventData = await _commandSender.Send<DescriptionCommand, DescriptionCommandResult>(new DescriptionCommand
                    {
                        CreatedBy = session.User.AltId,
                        Description = model.Description,
                        IsCountryDescription = model.IsCountryDescription,
                        IsStateDescription = model.IsStateDescription,
                        IsCityDescription = model.IsCityDescription,
                        Name = model.Name,
                        StateId = model.StateId
                    });
                }


                return new DescriptionResponseViewModel
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new DescriptionResponseViewModel
                {
                    Success = false
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/get/cityCountryDescription")]
        public async Task<CityCountryDescriptionResponseViewModel> getDescription([FromBody]DescriptionInputViewModel model)
        {
            var session = await _sessionProvider.Get();
            try
            {
                var queryResult = await _querySender.Send(new DescriptionQuery
                {
                    IsCountryDescription = model.IsCountryDescription,
                    IsCityDescription = model.IsCityDescription,
                    StateId = model.StateId,
                    IsStateDescription = model.IsStateDescription,
                    Name = model.Name
                });
                if (queryResult.CityDescription != null && !model.IsCountryDescription && !model.IsStateDescription)
                {
                    return new CityCountryDescriptionResponseViewModel
                    {
                        Success = true,
                        Description = queryResult.CityDescription.Description
                    };
                }
                else if (queryResult.CountryDescription != null && !model.IsStateDescription && !model.IsCityDescription)
                {
                    return new CityCountryDescriptionResponseViewModel
                    {
                        Success = true,
                        Description = queryResult.CountryDescription.Description
                    };
                }
                else if (queryResult.StateDescription != null && !model.IsCountryDescription && !model.IsCityDescription)
                {
                    return new CityCountryDescriptionResponseViewModel
                    {
                        Success = true,
                        Description = queryResult.StateDescription.Description
                    };
                }
                else
                {
                    return new CityCountryDescriptionResponseViewModel
                    {
                        Success = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new CityCountryDescriptionResponseViewModel
                {
                    Success = false
                };
            }
        }

    }
}
