using System.Collections.Generic;
using FIL.Contracts.DataModels;

namespace FIL.Web.Feel.ViewModels
{
    public class FeelSiteDynamicLayoutSectionViewModel
    {
        public string PageName { get; set; }
        public List<SectionView> Sections { get; set; }
    }
}
