using FIL.Contracts.DataModels;
using System.Collections.Generic;

namespace FIL.Contracts.QueryResults
{
    public class FeelSiteDynamicLayoutSectionQueryResult
    {
        public string PageName { get; set; }
        public List<SectionView> Sections { get; set; }
    }
}