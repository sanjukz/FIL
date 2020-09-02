using System.Collections.Generic;

namespace FIL.Contracts.Models.MasterLayout
{
    public class MasterLayoutLevelContainer
    {
        public List<MasterLayout> BlockUnderLevel { get; set; }
        public List<MasterLayout> SectionUnderLevel { get; set; }
        public List<MasterLayoutBlockContainer> MasterLayoutBlockContainers { get; set; }
    }
}