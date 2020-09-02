using HubSpot.NET.Api.Deal.Dto;
using System.Runtime.Serialization;

namespace FIL.Contracts.Models.Hubspot
{
    public class RasvHubspotDeal : DealHubSpotModel
    {
        /// <summary>
        /// Online Ticket Saver 2019 | RACV Member 2019 | After 5 2019 | Group Bookings 2019
        /// </summary>
        [DataMember(Name = "rms_ticket_type")]
        public string RMSTicketType { get; set; }
    }
}