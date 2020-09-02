// This file was generated from the Models.tst template
//

import City from "./Comman/CityViewModel";
import Country from "./Comman/CountryViewModel";
import EventDetail from "./Comman/EventDetailViewModel";
import Event from "./Comman/EventViewModel";
import State from "./Comman/StateViewModel";
import Venue from "./Comman/VenueViewModel";
import CitySightSeeingRoutes from './Comman/CitySightSeeingRoutes';
import CitySightSeeingRouteDetails from './Comman/CitySightSeeingRouteDetails';
import UserProfileResponseViewModel from "./Account/UserProfileResponseViewModel";
import EventTicketAttributeViewModel from "./EventTicketAttributeViewModel";
import TicektCategoryViewModel from "./TicektCategoryViewModel";
import RatingViewModel from "./RatingViewModel";
import ClientPointOfContactViewModel from "./Comman/ClientPointOfContactViewModel";
import EventTicketDetail from '../models/Comman/EventTicketDetailViewModel';
import EventGalleryImage from "./Comman/EventGalleryImageViewModel";
import { CategoryViewModel } from './CategoryViewModel';
import CurrencyType from "./Comman/CurrencyTypeViewModel";
import UserImageMap from "./ReviewsRating/UserImageViewModel";
import EventLearnMoreAttributeViewModel from "./Comman/EventLearnMoreAttributeViewModel";
import TiqetsProductCheckoutModel from "./Comman/TiqetsProductCheckoutModel";
import EventHostMapping from "./Comman/EventHostMapping";
import TicketAlertEventMapping from "./Comman/TicketAlertEventMapping";

export class EventLearnPageViewModel {
    event: Event;
    eventType: string;
    eventCategory: string;
    eventDetail: EventDetail;
    venue: Venue;
    city: City;
    state: State;
    country: Country;
    user: UserProfileResponseViewModel[];
    eventTicketDetail: EventTicketDetail[];
    eventTicketAttribute: EventTicketAttributeViewModel[];
    ticketCategory: TicektCategoryViewModel[];
    currencyType: CurrencyType;
    rating: RatingViewModel[];
    eventAmenitiesList: string[];
    clientPointOfContact: ClientPointOfContactViewModel;
    categories: CategoryViewModel[];
    eventGalleryImage: EventGalleryImage[];
    eventCategoryName: string;
    eventLearnMoreAttributes: EventLearnMoreAttributeViewModel[];
    userImageMap: UserImageMap[];
    regularTimeModel: regularViewModel;
    seasonTimeModel: seasonViewModel[];
    specialDayModel: specialDayViewModel[];
    citySightSeeingRoutes: CitySightSeeingRoutes[];
    citySightSeeingRouteDetails: CitySightSeeingRouteDetails[];
    tiqetsCheckoutDetails: TiqetsProductCheckoutModel[];
    category: CategoryViewModel;
    subCategory: CategoryViewModel;
    eventHostMappings: EventHostMapping[];
    ticketAlertEventMapping?:TicketAlertEventMapping;
    onlineStreamStartTime: string;
    onlineEventTimeZone: string;
}

export class timeViewModel {
    id: number;
    from: string;
    to: string;
}

export class speecialDateSeasonTimeViewModel {
    day: string;
    time: timeViewModel[];
}

export class seasonViewModel {
    id: number;
    isSameTime: boolean;
    startDate: string;
    endDate: string;
    name: string;
    sameTime: timeViewModel[];
    time: speecialDateSeasonTimeViewModel[];
    daysOpen: boolean[]
}

export class specailDatimeViewModel {
    day: string;
    time: timeViewModel;
}
export class customTimeModelData {
    day: string;
    id: number;
    time: timeViewModel[];
}

export class regularViewModel {
    isSameTime: boolean;
    customTimeModel: customTimeModelData[];
    timeModel: timeViewModel[];
    daysOpen: boolean[];
}

export class specialDayViewModel {
    id: number;
    name: string;
    specialDate: string;
    from: string;
    to: string;
}