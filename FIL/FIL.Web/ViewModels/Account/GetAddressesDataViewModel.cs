using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class GetAddressesDataViewModel
    {
        public Guid AltId { get; set; }
        public AddressTypes addressType { get; set; }
    }
}
