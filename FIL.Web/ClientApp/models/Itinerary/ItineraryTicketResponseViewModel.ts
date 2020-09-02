


import EventDetail from '../../models/Comman/EventDetailViewModel';
import TicketCategory from '../../models/TicketCategory/TicketCategory';
import EventTicketAttribute from '../../models/Comman/EventTicketAttributeViewModel';
import EventTicketDetail from '../../models/Comman/EventTicketDetailViewModel';


export default class ItineraryTicketResponseViewModel {
    itineraryTicketDetails: TicketResponse[];
}


export class TicketResponse {
    eventId: number;
    eventDetails: EventDetail[];
    ticketCategories: TicketCategory[];
    eventTicketAttributes: EventTicketAttribute[];
    eventTicketDetails: EventTicketDetail[];
}
