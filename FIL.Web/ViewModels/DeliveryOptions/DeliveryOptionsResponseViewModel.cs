using FIL.Contracts.Commands.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.DeliveryOptions
{
    public class DeliveryOptionsResponseViewModel
    {
        public List<FIL.Contracts.Models.EventDeliveryTypeDetail> EventDeliveryTypeDetails { get; set; }
        public FIL.Contracts.Models.User UserDetails { get; set; }
    }
}
