using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class TagType
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class TagTypeResponseModel
    {
        public bool success { get; set; }
        public IList<TagType> tag_types { get; set; }
    }
}