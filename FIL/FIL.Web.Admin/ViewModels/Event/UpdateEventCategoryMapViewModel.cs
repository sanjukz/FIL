using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.Event
{
    public class UpdateEventCategoryMapViewModel
    {
        public int Id { get; set; }
        public int Eventid { get; set; }
        public int Parentcategoryid { get; set; }
        public int Subcategoryid { get; set; }
        public bool Isenabled { get; set; }
    }
}
