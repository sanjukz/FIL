using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models
{
    public class CartTrack
    {
        public string HubspotUTK { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime TicketDateOfPurchase { get; set; }
        public Guid UserAltId { get; set; }
        public List<CartItemModel> EventDetailId { get; set; }
        public bool IsMailOpt { get; set; }
    }

    public class CartItemModel
    {
        public long EventDetailId { get; set; }
    }
}