using FIL.Logging;
using FIL.Configuration;
using System;
using FIL.Contracts.DataModels;
using FIL.Contracts.Models.Integrations;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Web.Feel.Providers
{
    public interface IPlacePriceProvider
    {
        SearchVenue GetPlacePrice(SearchVenue searchVenue,
            List<EventDetail> eventDetails,
            List<EventTicketDetail> eventTicketDetails,
            List<TicketCategory> ticketCategories,
            List<EventTicketAttribute> eventTicketAttributes
            );
    }

    public class PriceModel
    {
        public decimal Price { get; set; }
        public long EtaId { get; set; }
    }

    public class PlacePriceProvider : IPlacePriceProvider
    {
        private readonly FIL.Logging.ILogger _logger;
        public PlacePriceProvider(ILogger logger, ISettings settings
            )
        {
            _logger = logger;
        }

        public PriceModel filterEtabyPrice(List<EventTicketAttribute> eventTicketAttributes)
        {
            var minPrice = int.MaxValue;
            decimal min = 0;
            long currentETAId = 0;
            var isMinPriceExists = false;
            foreach (var eventTicketAttribute in eventTicketAttributes)
            {
                currentETAId = eventTicketAttribute.Id;
                if (eventTicketAttribute.Price < minPrice && eventTicketAttribute.Price != 0)
                {
                    isMinPriceExists = true;
                    min = eventTicketAttribute.Price;
                    currentETAId = eventTicketAttribute.Id;
                }
            }
            if (!isMinPriceExists)
            {
                min = 0;
            }
            var priceModel = new PriceModel();
            priceModel.EtaId = currentETAId;
            priceModel.Price = min;
            return priceModel;
        }

        public PriceModel filterPrices(
            List<TicketCategory> ticketCategories,
            List<EventTicketDetail> eventTicketDetails,
            List<EventTicketAttribute> eventTicketAttributes)
        {
            var eventTicketDetailIds = eventTicketDetails.Where((item) => ticketCategories.Any(s => s.Id == item.TicketCategoryId)).Select(s => s.Id);
            var eventTicketAttributes1 = eventTicketAttributes.Where((item) => eventTicketDetailIds.Any(s => s == item.EventTicketDetailId));
            var priceModel = filterEtabyPrice(eventTicketAttributes1.ToList());
            priceModel.EtaId = priceModel.EtaId;
            priceModel.Price = priceModel.Price;
            return priceModel;
        }

        public SearchVenue GetPlacePrice(SearchVenue searchVenue,
            List<EventDetail> eventDetails,
            List<EventTicketDetail> eventTicketDetails,
            List<TicketCategory> ticketCategories,
            List<EventTicketAttribute> eventTicketAttributes
            )
        {
            try
            {
                var adultCategories = ticketCategories.Where((item) =>
                { // check is adult exists
                    return item.Name.ToLower().Contains("adult");
                });
                var childCategories = ticketCategories.Where((item) =>
                { // check if child exists
                    return item.Name.ToLower().Contains("child");
                });
                var adultPriceModel = new PriceModel();
                var childPriceModel = new PriceModel();
                if (adultCategories.Count() > 0)
                { // if adult exists then pick base price of adult
                    adultPriceModel = filterPrices(adultCategories.ToList(), eventTicketDetails, eventTicketAttributes);
                }
                if (childCategories.Count() > 0)
                { // if child exists then pick base price of child
                    childPriceModel = filterPrices(childCategories.ToList(), eventTicketDetails, eventTicketAttributes);
                }

                if (adultCategories.Count() == 0 || childCategories.Count() == 0)
                { // if adult or child not exists then pick the base price of place
                    var currentPriceModel = filterEtabyPrice(eventTicketAttributes);
                    if (childCategories.Count() == 0)
                    {
                        childPriceModel = currentPriceModel;
                    }
                    if (adultCategories.Count() == 0)
                    {
                        adultPriceModel = currentPriceModel;
                    }
                }
                if (eventDetails.Count() == 0 || eventTicketDetails.Count() == 0 || eventTicketAttributes.Count() == 0)
                {
                    return searchVenue;
                }
                else
                {
                    searchVenue.AdultETAId = adultPriceModel.EtaId;
                    searchVenue.ChildETAId = childPriceModel.EtaId;
                    searchVenue.AdultPrice = Decimal.Parse(adultPriceModel.Price.ToString("0.00"));
                    searchVenue.ChildPrice = Decimal.Parse(childPriceModel.Price.ToString("0.00"));
                    searchVenue.Price = searchVenue.AdultPrice + searchVenue.ChildPrice;
                }
                return searchVenue;
            }
            catch (Exception e)
            {
                _logger.Log(Logging.Enums.LogCategory.Error, e);
                return searchVenue;
            }
        }
    }
}
