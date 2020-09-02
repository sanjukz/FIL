using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIL.Api.Providers
{
    public interface IDynamicSectionProvider
    {
        List<DynamicPlaceSections> GetDynamicSections(List<FIL.Contracts.Models.PlaceDetail> placeDetails,
            FIL.Contracts.Enums.MasterEventType masterEventType,
            PageDetail pageDetail);
    }

    public class DynamicSectionProvider : IDynamicSectionProvider
    {
        public DynamicSectionProvider()
        {
        }

        public DynamicPlaceSections GetDynamicPlaceSection(List<PlaceDetail> placeDetails,
            FIL.Contracts.Enums.MasterEventType masterEventType,
            string Key, PageDetail pageDetail, bool IsMainCategory = false)
        {
            DynamicPlaceSections DynamicPlaceSection = new DynamicPlaceSections();
            DynamicPlaceSection.PlaceDetails = new List<PlaceDetail>();
            var placeDetailList = placeDetails;
            DynamicPlaceSection.SectionDetails = new SectionDetail();
            DynamicPlaceSection.PlaceDetails = placeDetailList.OrderBy(a => Guid.NewGuid()).ToList();

            if (masterEventType == Contracts.Enums.MasterEventType.Online)
            {
                DynamicPlaceSection.SectionDetails.Heading = "Top " + placeDetails.FirstOrDefault().ParentCategory + " from around the World";
            }
            else if (pageDetail.PageType == Contracts.Enums.PageType.Category)
            {
                DynamicPlaceSection.SectionDetails.Heading = ((Key.ToLower().Contains("top") || placeDetails.First().Category.Contains("top")) ? "" : ((Key.ToLower().Contains("museums") || placeDetails.First().Category.Contains("museums"))) ? "Iconic " : "Top ")
                + (pageDetail.IsCategoryLevel ? Key : placeDetails.First().Category) + (placeDetails.FirstOrDefault().MasterEventTypeId == FIL.Contracts.Enums.MasterEventType.Online ? " from around " : " around ") +
            (pageDetail.IsCategoryLevel ? (pageDetail.IsCountryLevel ? placeDetails.First().CountryName : pageDetail.IsStateLevel ?
            placeDetails.First().StateName : pageDetail.IsCityLevel ? placeDetails.First().CityName : "the World") : Key);
            }
            else
            {
                StringBuilder sbTicketSummary = new StringBuilder();
                sbTicketSummary.Append("Top ");
                if ((!pageDetail.IsCategoryLevel && !pageDetail.IsSubCategoryLevel) || pageDetail.IsCategoryLevel)
                {
                    sbTicketSummary.Append(Key);
                }
                else
                {
                    sbTicketSummary.Append(placeDetails.First().Category);
                }
                sbTicketSummary.Append(IsMainCategory ? " feels around " : (placeDetails.FirstOrDefault().MasterEventTypeId == FIL.Contracts.Enums.MasterEventType.Online ? " from around " : " around "));
                if ((!pageDetail.IsCategoryLevel && !pageDetail.IsSubCategoryLevel) || pageDetail.IsCategoryLevel)
                {
                    if (pageDetail.IsCityLevel)
                    {
                        sbTicketSummary.Append(placeDetailList.First().CityName);
                    }
                    else if (pageDetail.IsStateLevel)
                    {
                        sbTicketSummary.Append(placeDetailList.First().StateName);
                    }
                    else
                    {
                        sbTicketSummary.Append(placeDetailList.First().CountryName);
                    }
                }
                else
                {
                    sbTicketSummary.Append(Key);
                }
                DynamicPlaceSection.SectionDetails.Heading = sbTicketSummary.ToString();
            }

            DynamicPlaceSection.SectionDetails.IsShowMore = true;

            if (masterEventType == Contracts.Enums.MasterEventType.Online)
            {
                DynamicPlaceSection.SectionDetails.Url = "/c/" + placeDetails.First().ParentCategorySlug;
            }
            else if (pageDetail.PageType == Contracts.Enums.PageType.Category)
            {
                DynamicPlaceSection.SectionDetails.Url = "/c/" + placeDetails.First().ParentCategorySlug + "/" + DynamicPlaceSection.PlaceDetails.First().SubCategorySlug +
                   (pageDetail.IsCountryLevel ? "/" + placeDetailList.First().StateName :
                   (pageDetail.IsStateLevel || pageDetail.IsCityLevel) ? "/" + placeDetailList.First().CityName : "");
            }
            else
            {
                StringBuilder sbTicketSummary = new StringBuilder();
                sbTicketSummary.Append(pageDetail.PagePath);
                if (IsMainCategory && !pageDetail.IsSubCategoryLevel)
                {
                    sbTicketSummary.Append("/" + DynamicPlaceSection.PlaceDetails.First().ParentCategorySlug);
                }
                if (!IsMainCategory && !pageDetail.IsSubCategoryLevel)
                {
                    sbTicketSummary.Append("/" + DynamicPlaceSection.PlaceDetails.First().SubCategorySlug);
                }
                if (pageDetail.IsSubCategoryLevel && pageDetail.IsCountryLevel)
                {
                    sbTicketSummary.Append("/" + Key);
                }
                else if (!pageDetail.IsCategoryLevel && !pageDetail.IsSubCategoryLevel && pageDetail.IsStateLevel)
                {
                    sbTicketSummary.Append("/" + placeDetailList.First().StateName.Replace(" ", "-").Replace(".", "").ToLower());
                }
                else if (pageDetail.IsCityLevel || pageDetail.IsStateLevel)
                {
                    sbTicketSummary.Append("/" + placeDetailList.First().CityName.Replace(" ", "-").Replace(".", "").ToLower());
                }
                DynamicPlaceSection.SectionDetails.Url = sbTicketSummary.ToString();
            }

            if (masterEventType == Contracts.Enums.MasterEventType.Online)
            {
                DynamicPlaceSection.SectionDetails.Query = "?category=" + placeDetailList.First().ParentCategoryId;
            }
            else if (pageDetail.PageType == Contracts.Enums.PageType.Category)
            {
                DynamicPlaceSection.SectionDetails.Query =
                (pageDetail.IsCategoryLevel ? "?category=" + placeDetailList.First().ParentCategoryId + (pageDetail.IsCountryLevel ? "&country=" + placeDetailList.First().CountryId : pageDetail.IsStateLevel ? "&state=" + placeDetailList.First().StateId :
                pageDetail.IsCityLevel ? "&city=" + placeDetailList.First().CityId : "") :
                pageDetail.IsCountryLevel ? "?category=" + placeDetailList.First().ParentCategoryId + "&subcategory=" + placeDetailList.First().EventCategoryId + "&state=" + placeDetailList.First().StateId :
                (pageDetail.IsStateLevel || pageDetail.IsCityLevel) ? "?category=" + placeDetailList.First().ParentCategoryId + "&subcategory=" + placeDetailList.First().EventCategoryId + "&city=" + placeDetailList.First().CityId :
                "?category=" + placeDetailList.First().ParentCategoryId + "&subcategory=" + placeDetailList.First().EventCategoryId + "&country=" + placeDetailList.First().CountryId
                );
            }
            else
            {
                StringBuilder sbTicketSummary = new StringBuilder();
                sbTicketSummary.Append("?country=" + placeDetailList.First().CountryId);
                if (IsMainCategory)
                {
                    sbTicketSummary.Append("&category=" + placeDetailList.First().ParentCategoryId);
                }
                if (!IsMainCategory)
                {
                    sbTicketSummary.Append("&category=" + placeDetailList.First().ParentCategoryId + "&subcategory=" + placeDetailList.First().EventCategoryId);
                }
                if (pageDetail.IsSubCategoryLevel && pageDetail.IsCountryLevel)
                {
                    sbTicketSummary.Append("&state=" + placeDetailList.First().StateId);
                }
                if (!pageDetail.IsCategoryLevel && !pageDetail.IsSubCategoryLevel && pageDetail.IsStateLevel)
                {
                    sbTicketSummary.Append("&state=" + placeDetailList.First().StateId);
                }
                if (((pageDetail.IsStateLevel || pageDetail.IsCityLevel) && pageDetail.PageType == Contracts.Enums.PageType.Category) ||
                    pageDetail.PageType == Contracts.Enums.PageType.Country && pageDetail.IsCityLevel)
                {
                    sbTicketSummary.Append("&city=" + placeDetailList.First().CityId);
                }
                DynamicPlaceSection.SectionDetails.Query = sbTicketSummary.ToString();
            }
            return DynamicPlaceSection;
        }

        public List<DynamicPlaceSections> GetAllDynamicSections(IOrderedEnumerable<IGrouping<string, PlaceDetail>> categoryGroup,
            FIL.Contracts.Enums.MasterEventType masterEventType,
           PageDetail pageDetail, bool IsMainCategory = false)
        {
            List<DynamicPlaceSections> DynamicPlaceSections = new List<DynamicPlaceSections>();
            foreach (var currentCatGroup in categoryGroup)
            {
                List<PlaceDetail> placeDetailList = new List<PlaceDetail>();
                if ((pageDetail.IsCategoryLevel == true
                    && !pageDetail.IsCityLevel
                    && !pageDetail.IsStateLevel
                    && !pageDetail.IsCountryLevel)
                    && pageDetail.PageType == Contracts.Enums.PageType.Category
                    && (masterEventType != Contracts.Enums.MasterEventType.Online && masterEventType != Contracts.Enums.MasterEventType.InRealLife))
                {
                    var groupPlaceByCountry = currentCatGroup.GroupBy(s => s.CountryName).Take(4);
                    foreach (var currentPlaceGroup in groupPlaceByCountry)
                    {
                        placeDetailList.AddRange(currentPlaceGroup.OrderBy(a => Guid.NewGuid()).Take(4).ToList());
                    }
                }
                else
                {
                    placeDetailList.AddRange(currentCatGroup.OrderBy(a => Guid.NewGuid()).Take(16).ToList());
                }

                var DynamicPlaceSection = GetDynamicPlaceSection(placeDetailList, masterEventType, currentCatGroup.Key, pageDetail, IsMainCategory);

                DynamicPlaceSections.Add(DynamicPlaceSection);
            }
            return DynamicPlaceSections;
        }

        public List<DynamicPlaceSections> GetDynamicSections(List<FIL.Contracts.Models.PlaceDetail> placeDetails,
            FIL.Contracts.Enums.MasterEventType masterEventType,
            PageDetail pageDetail)
        {
            List<DynamicPlaceSections> DynamicPlaceSections = new List<DynamicPlaceSections>();
            if (masterEventType == Contracts.Enums.MasterEventType.Online || masterEventType == Contracts.Enums.MasterEventType.InRealLife)
            {
                var categoryGroup = placeDetails.GroupBy(s => s.ParentCategoryId.ToString()).OrderByDescending(x => x.Count());
                DynamicPlaceSections = GetAllDynamicSections(categoryGroup, masterEventType, pageDetail);
            }
            else if (pageDetail.IsCategoryLevel)
            {
                var categoryGroup = placeDetails.GroupBy(s => s.Category).Take(4).OrderByDescending(x => x.Count());
                DynamicPlaceSections = GetAllDynamicSections(categoryGroup, masterEventType, pageDetail);
            }
            else if (pageDetail.IsSubCategoryLevel && !pageDetail.IsCountryLevel && !pageDetail.IsStateLevel && masterEventType != Contracts.Enums.MasterEventType.Online)
            {
                var categoryGroup = placeDetails.GroupBy(s => s.CountryName).Take(4).OrderByDescending(x => x.Count());
                DynamicPlaceSections = GetAllDynamicSections(categoryGroup, masterEventType, pageDetail);
            }
            else if (pageDetail.IsSubCategoryLevel && pageDetail.IsCountryLevel && masterEventType != Contracts.Enums.MasterEventType.Online)
            {
                var categoryGroup = placeDetails.GroupBy(s => s.StateName).Take(4).OrderByDescending(x => x.Count());
                DynamicPlaceSections = GetAllDynamicSections(categoryGroup, masterEventType, pageDetail);
            }
            else if (pageDetail.IsSubCategoryLevel && pageDetail.IsStateLevel && masterEventType != Contracts.Enums.MasterEventType.Online)
            {
                var categoryGroup = placeDetails.GroupBy(s => s.CityName).Take(4).OrderByDescending(x => x.Count());
                DynamicPlaceSections = GetAllDynamicSections(categoryGroup, masterEventType, pageDetail);
            }
            else if (pageDetail.PageType == Contracts.Enums.PageType.Country && !pageDetail.IsCategoryLevel && !pageDetail.IsSubCategoryLevel)
            {
                var parentCategoryGroup = placeDetails.GroupBy(s => s.ParentCategory).Take(3).OrderByDescending(x => x.Count());
                var subCategoryGroup = placeDetails.GroupBy(s => s.Category).Take(3).OrderByDescending(x => x.Count());
                var parentDynamicSections = GetAllDynamicSections(parentCategoryGroup, masterEventType, pageDetail, true);
                var subCategoryDynamicPlaceSections = GetAllDynamicSections(subCategoryGroup, masterEventType, pageDetail, false);
                DynamicPlaceSections.AddRange(parentDynamicSections.ToList());
                DynamicPlaceSections.AddRange(subCategoryDynamicPlaceSections.ToList());
            }
            return DynamicPlaceSections;
        }
    }
}