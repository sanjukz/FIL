// This file was generated from the Models.tst template
//
import Event from "../Common/EventViewModel";
import EventDetail from "../Common/EventDetailViewModel";
import PlaceCustomerDocumentTypeMapping from "./PlaceCustomerDocumentTypeMapping";
import PlaceTicketRedemptionDetails from "./PlaceTicketRedemptionDetails";
import EventDeliveryTypeDetail from "../Common/EventDeliveryTypeDetailViewModel";
import TicketCategory from "../Common/TicketCategoryViewModel";
import EventTicketAttribute from "../Common/EventTicketAttributeViewModel";
import EventTicketDetail from "../Common/EventTicketDetailViewModel";
import { PlaceWeekOffResponseViewModel } from "./PlaceWeekOffResponseViewModel";
import { PlaceHolidayDateResponseViewModel } from "./PlaceHolidayDateResponseViewModel";
import CustomerInformationModel from "../CustomerInformation/CustomerInformationModel";
import EventCustomerInformationMappingsModel from "../CustomerInformation/EventCustomerInformationMappingsModel";
import { EventTicketDetailTicketCategoryTypeMappingViewModel } from "./EventTicketDetailTicketCategoryTypeMappingViewModel";
import TicketFeeDetail from "../Common/TicketFeeDetail";
import EventAttribute from "../Common/EventAttributeViewModel";

export class GetPlaceInventoryDataResponseViewModel {
    event: Event;
    ticketCategoryContainer: TicketCategoryInfo[];
    eventDetails: EventDetail[];
    ticketValidityTypes: string[];
    deliveryTypes: string[];
    customerDocumentTypes: CustomerDocumentType[];
    placeCustomerDocumentTypeMappings: PlaceCustomerDocumentTypeMapping[];
    placeTicketRedemptionDetails: PlaceTicketRedemptionDetails[];
    eventDeliveryTypeDetails: EventDeliveryTypeDetail[];
    placeHolidayDates: PlaceHolidayDateResponseViewModel[];
    placeWeekOffs: PlaceWeekOffResponseViewModel[];
    eventTicketDetailTicketCategoryTypeMappings: EventTicketDetailTicketCategoryTypeMappingViewModel[];
    customerInformations: CustomerInformationModel[];
    eventCustomerInformationMappings: EventCustomerInformationMappingsModel[];
    regularTimeModel: regularViewModel;
    seasonTimeModel: seasonViewModel[];
    specialDayModel: specialDayViewModel[];
    eventAttribute: EventAttribute;
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
class CustomerDocumentType {
    id: number;
    documentType: string;
}

class TicketCategoryInfo {
    eventTicketDetail: EventTicketDetail;
    ticketCategory: TicketCategory;
    eventTicketAttribute: EventTicketAttribute;
    ticketCategorySubTypeId: number;
    ticketCategoryTypeId: number;
    ticketFeeDetails: TicketFeeDetail[]
}
