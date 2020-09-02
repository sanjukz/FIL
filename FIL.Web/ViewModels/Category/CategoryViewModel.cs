using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Category
{
    public class CategoryViewModel
    {
        public string DisplayName { get; set; }
        public string Slug { get; set; }
        public int EventCategory { get; set; }
        public int Order { get; set; }
        public bool IsHomePage { get; set; }
        public int CategoryId { get; set; }
        public bool IsFeel { get; set; }
        public FIL.Contracts.Enums.MasterEventType MasterEventTypeId { get; set; }
        public List<CategoryViewModel> SubCategories { get; set; }
    }
}
