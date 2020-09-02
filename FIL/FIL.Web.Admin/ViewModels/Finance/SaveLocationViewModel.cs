using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Enums;

namespace FIL.Web.Kitms.Feel.ViewModels.Finance
{
    public class SaveLocationViewModel
    {
        public long Id { get; set; }
        public long EventDetailId { get; set; }
        public Guid AltId { get; set; }
        public string Placename { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public string TilesSliderImages { get; set; }
        public string DescpagebannerImages { get; set; }
        public string InventorypagebannerImage { get; set; }
        public string GalleryImages { get; set; }
        public string PlacemapImages { get; set; }
        public string TimelineImages { get; set; }
        public string ImmersiveexpImages { get; set; }
        public string ArchdetailImages { get; set; }
        public bool IsEdit { get; set; }
        public string Zip { get; set; }
        public string Lat { get; set; }
        public string Long { get; set; }
    }
}
