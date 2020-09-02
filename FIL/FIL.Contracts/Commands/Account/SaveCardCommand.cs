using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Commands.Account
{
    public class SaveCardCommand : BaseCommand
    {
        public Guid UserAltId { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public short? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public CardType? CardTypeId { get; set; }
    }
}