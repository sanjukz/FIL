using FIL.Contracts.Interfaces;

namespace FIL.Contracts.DataModels
{
    public class PageView : IId<int>
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class SectionView : IId<int>
    {
        public int Id { get; set; }
        public int PageViewId { get; set; }
        public string SectionName { get; set; }
        public string ComponentName { get; set; }
        public string SectionHeading { get; set; }
        public string SectionSubHeading { get; set; }
        public string Endpoint { get; set; }
        public int SortOrder { get; set; }
        public bool IsEnabled { get; set; }
    }
}