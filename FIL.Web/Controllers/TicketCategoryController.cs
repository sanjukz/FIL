using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Foundation.Senders;
using Microsoft.AspNetCore.Mvc;
using FIL.Web.Feel.ViewModels.TicketCategories;
using FIL.Contracts.Queries.TicketCategory;
using FIL.Web.Feel.Providers;
using FIL.Web.Feel.ViewModels.Category;
using FIL.Contracts.Enums;
using FIL.Contracts.Queries.Category;
using FIL.Contracts.Models;
using FIL.Web.Feel.Modules.SiteExtensions;
using Microsoft.AspNetCore.JsonPatch.Internal;

namespace FIL.Web.Feel.Controllers
{
    public class TicketCategoryController : Controller
    {
        private readonly IQuerySender _querySender;
        private readonly IGeoCurrency _GeoCurrency;

        public TicketCategoryController(IQuerySender querySender, IGeoCurrency geoCurrency)
        {
            _querySender = querySender;
            _GeoCurrency = geoCurrency;
        }

        [HttpGet]
        [Route("api/ticketcategories/{eventAltId}")]
        public async Task<TicketCategoryResponseViewModel> Get(Guid eventAltId)
        {
            var queryResult = await _querySender.Send(new TicketCategoryQuery
            {
                EventAltId = eventAltId
            });

            var categoryName = "";
            var displayName = "";
            try
            {
                if (queryResult.EventCategoryMappings != null)
                {
                    var eventCategoryResult = await _querySender.Send(new Contracts.Queries.Events.EventCategoryQuery
                    {
                        Id = 0
                    });

                    var eventCategory = eventCategoryResult.EventCategories.Where(w => w.Id == queryResult.EventCategoryMappings.EventCategoryId).FirstOrDefault();

                    var eventParent = eventCategoryResult.EventCategories.Where(w => w.Id == eventCategory.EventCategoryId).FirstOrDefault();
                    displayName = eventCategory.DisplayName.ToString();
                    categoryName = eventParent != null ? eventParent.DisplayName : string.Empty;
                }
            }
            catch (Exception e)
            {
                categoryName = string.Empty;
            }
            List<string> deliveryOptionList = new List<string>();
            foreach (FIL.Contracts.Enums.DeliveryTypes delivery in Enum.GetValues(typeof(FIL.Contracts.Enums.DeliveryTypes)))
            {
                var deliveryOptions = delivery.ToString();
                deliveryOptionList.Add(deliveryOptions);
            }

            foreach (FIL.Contracts.Models.EventTicketAttribute eventTicketAttribute in queryResult.EventTicketAttribute)
            {
                try
                {
                    if (!String.IsNullOrEmpty(eventTicketAttribute.AdditionalInfo) && !String.IsNullOrEmpty(eventTicketAttribute.SrCitizenDiscount) && eventTicketAttribute.AdditionalInfo.Contains("<discount>"))
                    {
                        var discountAmount = _GeoCurrency.GetConvertedDiscountAmount(Convert.ToDecimal(eventTicketAttribute.SrCitizenDiscount), eventTicketAttribute.CurrencyId, HttpContext);
                        eventTicketAttribute.AdditionalInfo = eventTicketAttribute.AdditionalInfo.Replace("<discount>", String.Format("{0:0.00}", (discountAmount)));
                    }
                }
                catch (Exception e)
                {
                }
            }

            _GeoCurrency.UpdateCategoriesCurrency_v3(queryResult, HttpContext);

            return new TicketCategoryResponseViewModel
            {
                City = queryResult.City,
                Venue = queryResult.Venue,
                CurrencyType = queryResult.CurrencyType,
                EventDetail = queryResult.EventDetail,
                Event = queryResult.Event,
                EventTicketAttribute = queryResult.EventTicketAttribute,
                EventTicketDetail = queryResult.EventTicketDetail,
                TicketCategory = queryResult.TicketCategory,
                EventCategory = displayName,
                EventCategoryName = categoryName,
                PlaceCustomerDocumentTypeMappings = queryResult.PlaceCustomerDocumentTypeMappings,
                PlaceHolidayDates = queryResult.PlaceHolidayDates,
                EventDeliveryTypeDetails = queryResult.EventDeliveryTypeDetails,
                PlaceWeekOffs = queryResult.PlaceWeekOffs,
                CustomerDocumentTypes = queryResult.CustomerDocumentTypes,
                TicketCategorySubTypes = queryResult.TicketCategorySubTypes,
                TicketCategoryTypes = queryResult.TicketCategoryTypes,
                EventTicketDetailTicketCategoryTypeMappings = queryResult.EventTicketDetailTicketCategoryTypeMappings,
                RegularTimeModel = queryResult.RegularTimeModel,
                SeasonTimeModel = queryResult.SeasonTimeModel,
                SpecialDayModel = queryResult.SpecialDayModel,
                DeliveryOptions = deliveryOptionList,
                EventVenueMappings = queryResult.EventVenueMappings,
                EventVenueMappingTimes = queryResult.EventVenueMappingTimes,
                TiqetsCheckoutDetails = queryResult.TiqetsCheckoutDetails,
                ValidWithVariantModel = queryResult.ValidWithVariantModel,
                Category = queryResult.Category,
                SubCategory = queryResult.SubCategory,
                CitySightSeeingTicketDetail = queryResult.CitySightSeeingTicketDetail,
                eventRecurranceScheduleModels = queryResult.eventRecurranceScheduleModels,
                EventAttributes = queryResult.EventAttributes,
                EventHostMapping = queryResult.EventHostMapping,
                FormattedDateString = queryResult.FormattedDateString
            };
        }

        [HttpGet]
        [Route("api/subevents/{eventDetailAltId}")]
        public async Task<SubEventTicketCategoryResponseViewModel> GetSubevent(int eventDetailAltId)
        {
            var queryResult = await _querySender.Send(new SubEventTicketCategoryQuery
            {
                EventDetailId = eventDetailAltId
            });

            return new SubEventTicketCategoryResponseViewModel
            {
                City = queryResult.City,
                Venue = queryResult.Venue,
                CurrencyType = queryResult.CurrencyType,
                EventDetail = queryResult.EventDetail,
                EventTicketAttribute = queryResult.EventTicketAttribute,
                EventTicketDetail = queryResult.EventTicketDetail,
                TicketCategory = queryResult.TicketCategory
            };
        }

    }
}
