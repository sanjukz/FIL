using System.Collections.Generic;

namespace FIL.Contracts.Models.Tiqets
{
    public class Tag
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type_name { get; set; }
        public string type_id { get; set; }
    }

    public class TagPagination
    {
        public int total { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }
    }

    public class TagResponseModel
    {
        public bool success { get; set; }
        public IList<Tag> tags { get; set; }
        public TagPagination pagination { get; set; }
    }
}