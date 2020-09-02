using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIL.Web.Core.Providers
{
    public class ImageFilePathProvider
    {
        public static string GetImageFilePath(string altId, ImageType imageType)
        {
            string keyPath = string.Empty;
            switch (imageType)
            {
                case ImageType.HomePageBanner: return $"Images/HomePageBanners/{altId}.jpg";
                case ImageType.CategoryPageBanner: return $"Images/CategoryPageBanners/{altId}.jpg";
                case ImageType.HotTicket: return $"Images/HotTicket/{altId}-ht.jpg";
                case ImageType.EventHome: return $"Images/EventHome/{altId}-home.jpg";
                case ImageType.AboutEvent: return $"Images/AboutEvent/{altId}-about.jpg";
                case ImageType.EventTile: return $"Images/EventTile/{altId}-tile.jpg";
                case ImageType.QrCode: return $"Images/QrCodes/{altId}.png";
                case ImageType.ProfilePic: return $"Images/ProfilePictures/{altId}.jpg";
                case ImageType.FeelHotTicketSlider: return $"images/places/tiles/{altId}.jpg";
                case ImageType.FeelDescriptionPage: return $"images/places/about/{altId}.jpg";
                case ImageType.FeelInventoryPage: return $"images/places/InnerBanner/{ altId}.jpg";
                case ImageType.FeelGallery: return $"images/places/about/photo-gallery/{altId}.jpg";
                case ImageType.FeelPlacePlan: return $"images/places/about/in-depth/{altId}.jpg";
                case ImageType.FeelTimeline: return $"images/places/about/in-depth/{altId}.jpg";
                case ImageType.FeelImmersiveExperience: return $"images/places/about/in-depth/{altId}.jpg";
                case ImageType.FeelArchitecturalDetail: return $"images/places/about/in-depth/{altId}.jpg";
                case ImageType.FeelTest: return $"test/{altId}.jpg";
                case ImageType.TicketPDF: return $"images/ticketpdf/{altId}.pdf";
                case ImageType.RecommendedSectionImage: return $"Images/zoonga2019/Z-Recom/{altId}-recom.jpg";
                default: return "";
            }
        }       
    }
}
