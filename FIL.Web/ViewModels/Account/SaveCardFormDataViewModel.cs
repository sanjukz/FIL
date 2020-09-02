using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Feel.ViewModels.Account
{
    public class SaveCardFormDataViewModel
    {
        public Guid AltId { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public short? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public CardType? CardTypeId { get; set; }
    }
}
