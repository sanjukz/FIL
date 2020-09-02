using HubSpot.NET.Api.Contact.Dto;
using System.Runtime.Serialization;

namespace FIL.Contracts.Models.Hubspot
{
    public class RasvHubspotContact : ContactHubSpotModel
    {
        /// <summary>
        /// Online Ticket Saver 2019 | RACV Member 2019 | After 5 2019 | Group Bookings 2019 | Ride Pass
        /// </summary>
        [DataMember(Name = "rms_ticket_type_2019")]
        public string RMSTicketType { get; set; }

        /// <summary>
        /// Yes or No
        /// </summary>
        [DataMember(Name = "rms_2019_ride_pass_purchased")]
        public string RMSRidePassPurchased { get; set; }

        /// <summary>
        /// Yes or No
        /// </summary>
        [DataMember(Name = "rms_2019_ticket_platform_login")]
        public string RMS2019TicketPlatformLogin { get; set; }

        /// <summary>
        /// Yes or No
        /// </summary>
        [DataMember(Name = "zoonga_cart_abandonment")]
        public string KyazoongaCartAbandonment { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [DataMember(Name = "rms_2019_ticket_date_of_purchase")]
        public string RMS2019TicketDateOfPurchase { get; set; }

        /// <summary>
        /// 2018
        /// </summary>
        [DataMember(Name = "ticket_purchased_rms")]
        public string RMSTicketPurchased { get; set; }

        [DataMember(Name = "rms_newsletter_subscriber_2019")]
        public string RMSNewsletterSubscriber2019 { get; set; }

        [DataMember(Name = "opted_in")]
        public string RMSMarketingOptIn { get; set; }

        [DataMember(Name = "rms_2019_date_scanned_ticket_at_gate")]
        public string RMS2019DateScannedTicketAtGate { get; set; }
    }
}