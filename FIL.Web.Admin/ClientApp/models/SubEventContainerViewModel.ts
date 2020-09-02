import EventDetail from "./Common/EventDetailViewModel";
import Venue from "./Common/VenueViewModel";
import City from "./Common/CityViewModel";
import EventDeliveryTypeDetail from "./Common/EventDeliveryTypeDetailViewModel";
import EventTicketDetail from "./Common/EventTicketDetailViewModel";
import State from "./Common/StateViewModel";
import Country from "./Common/CountryViewModel";
import TicketCategory from "./Common/TicketCategoryViewModel";
import TransactionDeliveryDetail from "./Common/TransactionDeliveryDetailViewModel";
import TransactionDetail from "./Common/TransactionDetailViewModel";
import TransactionPaymentDetail from "./Common/TransactionPaymentDetailViewModel";
import TransactionSeatDetail from "./Common/TransactionSeatDetailViewModel";
import MatchLayoutSectionSeat from "./Common/MatchLayoutSectionSeatViewModel";
import MatchSeatTicketDetailContainer from "../models/TicketLookup/MatchSeatTicketDetailContainerViewModel"

export default class SubEventContainer {
    eventDetail: EventDetail;
    eventTicketDetail: EventTicketDetail[];
    venue: Venue;
    city: City;
    state: State;
    country: Country;
    transactionDetail: TransactionDetail[];
    ticketCategory: TicketCategory[];
    transactionDeliveryDetail: TransactionDeliveryDetail[];
    transactionSeatDetail: TransactionSeatDetail[];
    transactionPaymentDetail: TransactionPaymentDetail[];
    eventDeliveryTypeDetail: EventDeliveryTypeDetail[];
    matchLayoutSectionSeat: MatchLayoutSectionSeat[];
    matchSeatTicketDetail: MatchSeatTicketDetailContainer[]
}