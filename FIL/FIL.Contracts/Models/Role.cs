using System;

namespace FIL.Contracts.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime? UpdatedUtc { get; set; }
    }
}