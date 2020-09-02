using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIL.ExOzConsoleApp.Entities.Classes
{
    public class ProductSession
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int EventId { get; set; }
        public int EventCatId { get; set; }
        public int VenueId { get; set; }
        public string SessionName { get; set; }
        public List<ExOzProductOption> ProductOptions { get; set; }
        public string HasPickups { get; set; }
        public string Levy { get; set; }
        public string IsExtra { get; set; }
        public string TourHour{ get; set; }
        public string TourMinute { get; set; }
        public string TourDuration { get; set; }
        public string EventDesc { get; set; }
        public string Timezone { get; set; }
    }

    public class ExOzProductOption
    {
        public int Id { get; set; }
        public int VenueCatId { get; set; }
        public int SessionId { get; set; }
        public int EventId { get; set; }
        public int VMCC_Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string RetailPrice { get; set; }
        public int MaxQty { get; set; }
        public int minQty { get; set; }
        public int defaultQty { get; set; }
        public int multiple { get; set; }
        public int weight { get; set; }
        public bool isFromPrice { get; set; }
        public double levy { get; set; }
    }
}
