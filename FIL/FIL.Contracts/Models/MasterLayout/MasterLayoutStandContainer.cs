using System.Collections.Generic;

namespace FIL.Contracts.Models.MasterLayout
{
    public class MasterLayoutStandContainer
    {
        public MasterLayout stands { get; set; }
        public List<MasterLayout> levels { get; set; }
        public List<MasterLayout> blocks { get; set; }
        public List<MasterLayout> sections { get; set; }
        public List<MasterLayoutLevelContainer> MasterLayoutLevelContainer { get; set; }
        public List<MasterLayoutBlockContainer> MasterLayoutBlockContainer { get; set; }
    }
}