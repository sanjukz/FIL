using FIL.Contracts.Models.ValueRetail;
using System;
using System.Collections.Generic;

namespace FIL.Contracts.Models.Integrations.ValueRetail
{
    public class Participant
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class Item
    {
        public string type { get; set; }
        public int JourneyType { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public int RouteId { get; set; }
        public int StopId { get; set; }
        public string StopName { get; set; }
        public int ReturnRouteId { get; set; }
        public int ReturnStopId { get; set; }
        public string ReturnStopName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Action { get; set; }
        public int ServiceType { get; set; }
        public string ItemId { get; set; }
        public int ProductId { get; set; }
        public DateTime DateOfTravel { get; set; }
        public IList<Participant> Participants { get; set; }
        public object PromotionalCode { get; set; }
        public int JobId { get; set; }
        public Village Village { get; set; }
        public object QuotedPrice { get; set; }
        public bool AmendmentAllowed { get; set; }
        public bool CancelAllowed { get; set; }
        public object AdditionalProperties { get; set; }
    }

    public class UpsertShoppingCartRequest
    {
        public string BasketId { get; set; }
        public Item Item { get; set; }
        public string AggregatorId { get; set; }
        public string OtaId { get; set; }
    }

    public class ProductItemType
    {
        public string ShoppingExpress = "VR.Integration.Common.Cart.Model.ShoppingExpressBasketItem, VR.Integration.Common.Cart";
        public string ShoppingPackage = "VR.Integration.Common.Cart.Model.ShoppingPackageBasketItem, VR.Integration.Common.Cart";
        public string ChauffeurDrive = "VR.Integration.Common.Cart.Model.ChauffeurDrivenBasketItem, VR.Integration.Common.Cart";
        public string GiftCard = "VR.Integration.Common.Cart.Model.GiftCardBasketItem, VR.Integration.Common.Cart";
    }
}