using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIL.ExOzConsoleApp.Entities.Classes
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string urlSegment { get; set; }
        public int catId { get; set; }
        public int regionId { get; set; }
        public string regionUrl { get; set; }
        public int quantity { get; set; }
        public int offset { get; set; }
        public string timestamp { get; set; }
        public List<Operator> operators {get; set;}
    }
}
