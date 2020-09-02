using FIL.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Kitms.Feel.ViewModels.TicketCategoryTypes
{
    public class TicketCategoryTypesResponseViewModel
    {
        public List<FIL.Contracts.DataModels.TicketCategoryType> TicketCategoryTypes { get; set; }
        public List<FIL.Contracts.DataModels.TicketCategorySubType> TicketCategorySubTypes { get; set; }
    }
}
