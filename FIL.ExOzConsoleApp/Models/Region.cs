using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIL.ExOzConsoleApp.Entities.Classes
{
    public class Region
    {
        public int regionId { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string urlSegment { get; set; }
        public string stateUrlSegment { get; set; }
        public int stateId { get; set; }
        public int quantity { get; set; }
        public int offset { get; set; }
        public string timestamp { get; set; }
        public List<Category> categories { get; set; }
        public List<Operator> operators { get; set; }
        public int cityMapId { get; set; }
        public int stateMapId { get; set; }

    }
}
