using FIL.Contracts.Enums;
using System;

namespace FIL.Contracts.Models
{
    public class UserCard
    {
        public Guid AltId { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public short? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public CardType? CardTypeId { get; set; }
    }
}