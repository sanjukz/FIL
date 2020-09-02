using System;

namespace FIL.Contracts.Models
{
    public class ZsuitePaymentOption
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}