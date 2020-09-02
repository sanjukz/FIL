using System.Collections.Generic;

namespace FIL.Contracts.Models.ValueRetail.ShoppingPackage
{
    public class ChauffeurJourney
    {
        public int JourneyType { get; set; }
        public int RouteId { get; set; }
        public string JourneyName { get; set; }
        public bool OneWayEnabled { get; set; }
        public object LinkedRouteId { get; set; }
    }

    public class ChauffeurDrivenJourneyResponse
    {
        public IList<ChauffeurJourney> Journeys { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}