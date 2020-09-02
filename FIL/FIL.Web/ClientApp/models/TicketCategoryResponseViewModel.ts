// This file was generated from the Models.tst template
//

import EventDetail from '../models/Comman/EventDetailViewModel';
import Venue from '../models/Comman/VenueViewModel';
import Event from '../models/Comman/EventViewModel';
import City from '../models/Comman/CityViewModel';
import TicketCategory from '../models/TicketCategory/TicketCategory';
import EventTicketAttribute from '../models/Comman/EventTicketAttributeViewModel';
import EventTicketDetail from '../models/Comman/EventTicketDetailViewModel';
import CurrencyType from '../models/TicketCategory/CurrencyType';
import PlaceCustomerDocumentTypeMapping from '../models/PlaceCustomerDocumentTypeMapping';
import { PlaceHolidayDateResponseViewModel } from '../models/PlaceHolidayDateResponseViewModel';
import { PlaceWeekOffResponseViewModel } from '../models/PlaceWeekOffResponseViewModel';
import TicketCategorySubTypesViewModel from "./TicketCategoryType/TicketCategorySubTypesViewModel";
import TicketCategoryTypesViewModel from "./TicketCategoryType/TicketCategoryTypesViewModel";
import ETDTicketCategoryTypeMappingsViewModel from "./TicketCategoryType/ETDTicketCategoryTypeMappingsViewModel";
import TiqetsProductCheckoutModel from "./Comman/TiqetsProductCheckoutModel";
import CitySightSeeingTicketDetail from "./Comman/CitySightSeeingTicketDetail";
import { CategoryViewModel } from './CategoryViewModel';
import EventHostMapping from './Comman/EventHostMapping';
import EventAttributeViewModel from './Comman/EventAttributeViewModel';

class CustomerDocumentType {
    id: number;
    documentType: string;
}

export class TicketCategoryResponseViewModel {
    event: Event;
    eventDetail: EventDetail[];
    venue: Venue[];
    city: City[];
    ticketCategory: TicketCategory[];
    eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    currencyType: CurrencyType;
    eventCategory: string;
    eventCategoryName: string;
    PlaceCustomerDocumentTypeMappings: PlaceCustomerDocumentTypeMapping[];
    PlaceHolidayDates: PlaceHolidayDateResponseViewModel[];
    PlaceWeekOffs: PlaceWeekOffResponseViewModel[];
    customerDocumentTypes: CustomerDocumentType[];
    ticketCategoryTypes: TicketCategoryTypesViewModel[];
    ticketCategorySubTypes: TicketCategorySubTypesViewModel[];
    eventTicketDetailTicketCategoryTypeMappings: ETDTicketCategoryTypeMappingsViewModel[];
    regularTimeModel: regularViewModel;
    seasonTimeModel: seasonViewModel[];
    specialDayModel: specialDayViewModel[];
    eventDeliveryTypeDetails: EventTicketDetail[];
    deliveryOptions: string[];
    eventVenueMappings: EventVenueMapping;
    eventVenueMappingTimes: EventVenueMappingTime[];
    tiqetsCheckoutDetails: TiqetsProductCheckoutModel[];
    validWithVariantModel: ValidWithVariantModel[];
    citySightSeeingTicketDetail: CitySightSeeingTicketDetail;
    category: CategoryViewModel;
    subCategory: CategoryViewModel;
    formattedDateString?: string;
    eventHostMapping?: EventHostMapping[];
    eventRecurranceScheduleModels: EventRecurranceScheduleModel[];
    EventAttributes?: EventAttributeViewModel[];
}

class EventDeliveryTypeDetail {
    id: number;
    eventDetailId: number;
    deliveryTypeId: string;
    notes: string;
    endDate: string;
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

export class EventVenueMappingTime {
    id: number;
    eventVenueMappingId: number;
    pickupTime: string;
    pickupLocation: string;
    returnTime: string;
    returnLocation: string;
    journeyType: number;
    waitingTime: string;
}

export class EventVenueMapping {
    id: number;
    eventId: number;
    venueId: number;
}


export class ValidWithVariantModel {
    eventTicketDetailId: number;
    validWithEventTicketDetailId: number[];
}


export class EventRecurranceScheduleModel {
    eventScheduleId: number;
    scheduleDetailId: number;
    dayIds: string;
    startDateTime: string;
    endDateTime: string;
    localStartDateTime: string;
    localEndDateTime: string;
    eventScheduleStartDateTime: string;
    eventScheduleEndDateTime: string;
    localStartTime: string;
    localEndTime: string;
    localStartDateString: string;
    localEndDateString: string;
    localEventScheduleStartDateTimeString: string;
    localEventScheduleEndDateTimeString: string;
    isEnabled: boolean
}