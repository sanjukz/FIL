using System;

namespace FIL.Contracts.Models
{
    public class UserCardDetail
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public Guid AltId { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public short? ExpiryMonth { get; set; }
        public short? ExpiryYear { get; set; }
        public string CardTypeId { get; set; }
        public bool? IsDefault { get; set; }
        public bool IsEnabled { get; set; }
    }
}