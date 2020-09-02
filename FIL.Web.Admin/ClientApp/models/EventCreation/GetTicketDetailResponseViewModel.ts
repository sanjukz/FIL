import TicketCategory from '../TicketCategory/TicketCategory';
import EventTicketAttribute from '../Common/EventTicketAttributeViewModel';
import TicketFeeDetail from '../TicketCategory/TicketFeeDetail';
import EventTicketDetail from '../Common/EventTicketDetailViewModel';

export class GetTicketDetailResponseViewModel  { 
    ticketCategory: TicketCategory[];
    eventTicketAttribute: EventTicketAttribute[];
    ticketFeeDetail: TicketFeeDetail[];
    eventTicketDetail: EventTicketDetail[];
}
