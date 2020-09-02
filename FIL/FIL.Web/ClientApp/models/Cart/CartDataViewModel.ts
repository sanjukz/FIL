import { MasterEventTypes } from "../../Enum/MasterEventTypes";
import { Eventstatuses } from "../../Enum/Eventstatuses";

export class CartDataViewModel {
    cartItems: CartItem[];
}

export class GuestUserDetails {
    firstName: string;
    lastName: string;
    email: string;
    phoneCode: string;
    phoneNumber: string;
    age: number;
    gender: number;
    identityType: number;
    identityNumber: string;
    country: string;
}

export class CartItem {
    altId: string;
    eventDetailId: number;
    name: string;
    eventStartDate: string;
    venue: string;
    city: string;
    ticketCategoryId: number;
    ticketCategoryName: string;
    eventTicketAttributeId: number;
    currencyId: number;
    currencyName: string;
    quantity: number;
    pricePerTicket: number;
    ticketCategoryDescription: string;
    eventTermsAndConditions: string;
    selectedDate?: string;
    isTimeSelection?: boolean;
    guestList: GuestData[];
    isAddOn?: boolean;
    ticketSubCategoryId?: number;
    ticketCategoryTypeId?: number;
    placeVisitTime?: string;
    deliveryOptions?: string[];
    journeyType?: number;
    pickupLocation?: PickupLocation;
    pickupTime?: string;
    returnLocation?: string;
    returnTime?: string;
    waitingTime?: number;
    eventVenueMappingTimeId?: number;
    isItinerary?: boolean;
    visitStartTime?: string;
    visitEndTime?: string;
    isAdult?: boolean;
    isTiqetsPlace?: boolean;
    timeSlot?: string;
    timeStamp?: Date;
    isHohoPlace?: boolean;
    parentCategory?: string;
    subCategory?: string;
    subCategoryId?: number;
    categoryId?: number;
    donationAmount?: number;
    transactionType?: number;
    eventTicketDetail?: number;
    additionalInfo?: string;
    discountAmount?: number;
    masterEventTypeId?: MasterEventTypes;
    eventStatusId?: Eventstatuses;
    specialPrice?: number;
    overridedAmount?: number;
    isBSPUpgrade?: boolean;
    scheduleDetailId?: number;
}

export class PickupLocation {
    address1?: string;
    address2?: string;
    town?: string;
    region?: string;
    postalCode?: number;
}

export class GuestData {
    guestId: number;
    firstName: string;
    lastName: string;
    nationality: string;
    documentTypeId: string;
    documentNumber: string;
}
