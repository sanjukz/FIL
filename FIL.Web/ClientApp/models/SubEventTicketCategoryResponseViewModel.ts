// This file was generated from the Models.tst template
//

import EventDetail from '../models/Comman/EventDetailViewModel';
import Venue from '../models/Comman/VenueViewModel';
import City from '../models/Comman/CityViewModel';
import TicketCategory from '../models/TicketCategory/TicketCategory';
import EventTicketAttribute from '../models/Comman/EventTicketAttributeViewModel';
import EventTicketDetail from '../models/Comman/EventTicketDetailViewModel';
import CurrencyType from '../models/TicketCategory/CurrencyType';

export default class SubEventTicketCategoryResponseViewModel  { 
    eventDetail: EventDetail;
    venue: Venue;
    city: City;
    ticketCategory: TicketCategory[];
    eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    currencyType: CurrencyType;
}
