using FIL.Contracts.Models;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Commands.CartTrack
{
    public class CartTrackCommand : BaseCommand
    {
        public string HubspotUTK { get; set; }
        public Guid UserAltId { get; set; }
        public List<CartItemModel> EventDetailId { get; set; }
    }
}