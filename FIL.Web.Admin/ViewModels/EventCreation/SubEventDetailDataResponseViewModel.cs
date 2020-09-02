using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIL.Contracts.Models;
namespace FIL.Web.Kitms.Feel.ViewModels.EventCreation
{
    public class SubEventDetailDataResponseViewModel
    {
        public IEnumerable<EventDetail> EventDetail { get; set; }
    }
}
