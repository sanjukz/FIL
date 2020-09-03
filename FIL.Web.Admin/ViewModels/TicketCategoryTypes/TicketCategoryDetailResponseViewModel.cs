using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.Zipcode
{
    public class TicketCategoryDetailResponseViewModel
    {
        public List<FIL.Contracts.Models.TicketCategoryDetail> TicketCategoryDetails { get; set; }
        public List<FIL.Contracts.Models.TicketCategory> TicketCategories { get; set; }
    }
}
