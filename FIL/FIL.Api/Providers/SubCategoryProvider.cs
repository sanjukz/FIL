using FIL.Contracts.Models;
using FIL.Contracts.Queries.FeelUserJourney;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Api.Providers
{
    public interface ISubCategoryProvider
    {
        List<SubCategory> GetSubCategories(FeelUserJourneyQuery query, List<FIL.Contracts.Models.PlaceDetail> placeDetails,
            List<FIL.Contracts.DataModels.EventCategory> eventCategories, PageDetail pageDetail);
    }

    public class SubCategoryProvider : ISubCategoryProvider
    {
        public SubCategoryProvider()
        {
        }

        public string GetQuery(FeelUserJourneyQuery query, FIL.Contracts.DataModels.EventCategory eventCategory, PlaceDetail placeDetail,
            int ParentEventCategoryId, PageDetail pageDetail)
        {
            if (eventCategory.MasterEventTypeId == Contracts.Enums.MasterEventType.Online || eventCategory.MasterEventTypeId == Contracts.Enums.MasterEventType.InRealLife)
            {
                return "?category=" + ParentEventCategoryId +
                    (pageDetail.IsCountryLevel ? "&country=" + placeDetail.CountryId : pageDetail.IsStateLevel ? "&state=" + placeDetail.StateId :
                    pageDetail.IsCityLevel ? "&city=" + placeDetail.CityId : "");
            }
            else if (query.PageType == Contracts.Enums.PageType.Category)
            {
                return "?category=" + ParentEventCategoryId + "&subcategory=" + eventCategory.Id +
                    (pageDetail.IsCountryLevel ? "&country=" + placeDetail.CountryId : pageDetail.IsStateLevel ? "&state=" + placeDetail.StateId :
                    pageDetail.IsCityLevel ? "&city=" + placeDetail.CityId : "");
            }
            else
            {
                return "?country=" + placeDetail.CountryId + (eventCategory.EventCategoryId == 0 ? "&category=" + eventCategory.Id : "&category=" + ParentEventCategoryId + "&subcategory=" + eventCategory.Id) +
                     (pageDetail.IsStateLevel ? "&state=" + placeDetail.StateId :
                    pageDetail.IsCityLevel ? "&city=" + placeDetail.CityId : "");
            }
        }

        public List<SubCategory> GetSubCategories(FeelUserJourneyQuery query, List<FIL.Contracts.Models.PlaceDetail> placeDetails,
            List<FIL.Contracts.DataModels.EventCategory> eventCategories, PageDetail pageDetail)
        {
            List<SubCategory> allSubCategories = new List<SubCategory>();
            foreach (var currectSubCategory in eventCategories)
            {
                SubCategory subCategory = new SubCategory();
                subCategory.DisplayName = currectSubCategory.DisplayName;
                subCategory.Slug = currectSubCategory.Slug;
                subCategory.Id = currectSubCategory.Id;
                subCategory.IsMainCategory = currectSubCategory.EventCategoryId == 0 ? true : false;
                subCategory.Url = query.PagePath + "/" + currectSubCategory.Slug;
                subCategory.Order = currectSubCategory.Order;
                subCategory.Query = GetQuery(query, currectSubCategory, placeDetails.First(),
                   currectSubCategory.MasterEventTypeId == Contracts.Enums.MasterEventType.Online ? currectSubCategory.Id : placeDetails.First().ParentCategoryId, pageDetail);
                allSubCategories.Add(subCategory);
            }
            return allSubCategories;
        }
    }
}