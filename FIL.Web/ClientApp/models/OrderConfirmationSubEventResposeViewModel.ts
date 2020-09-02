import Event from "./Comman/EventViewModel";
import EventDetail from "./Comman/EventDetailViewModel";
import Venue from "./Comman/VenueViewModel";
import City from "./Comman/CityViewModel";
import EventDeliveryTypeDetail from "./Comman/EventDeliveryTypeDetailViewModel";
import EventTicketDetail from "./Comman/EventTicketDetailViewModel";
import EventTicketAttributeViewModel from "./Comman/EventTicketAttributeViewModel";
import State from "./Comman/StateViewModel";
import Country from "./Comman/CountryViewModel";
import TicketCategory from "./Comman/TicketCategoryViewModel";
import TransactionDeliveryDetail from "./Comman/TransactionDeliveryDetailViewModel";
import TransactionDetail from "./Comman/TransactionDetailViewModel";
import TransactionPaymentDetail from "./Comman/TransactionPaymentDetailViewModel";
import TransactionSeatDetail from "./Comman/TransactionSeatDetailViewModel";
import MatchLayoutSectionSeat from "./Comman/MatchLayoutSectionSeatViewModel";
import MatchSeatTicketDetail from "./Comman/MatchSeatTicketDetailViewModel"

export default class OrderConfirmationSubEventResposeViewModel {
    event: Event;
    eventDetail: EventDetail;
    eventCategory: CategoryViewModel;
    eventTicketDetail: EventTicketDetail[];
    eventTicketAttribute?: EventTicketAttributeViewModel[];
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
    matchSeatTicketDetail: MatchSeatTicketDetail[]
}

export class CategoryViewModel {
    id: number;
    displayName: string;
    slug: string;
    eventCategory?: number;
    order: number;
    isHomePage: boolean;
    categoryId?: number;
    isFeel: boolean;
    eventCategoryId: number;
}