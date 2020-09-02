export class EventTicketDetailDataViewModel {
    eventTickets: EventTicketAttribute[];
}

export class EventTicketAttribute {
    id: number;
    totalTickets: number;
    ticketType: number;
    visitDate: Date;
    guestDetails: GuestData[];
    eventVenueMappingTimeId?: number;
    purchaserAddress?: PurchaserAddress;
    visitStartTime?: string;
    visitEndTime?: string;
    price?: string;
    isAdult?: boolean;
    timeSlot?: string;
    reserveHohoBook?: boolean;
    transactionType?: number;
    discountedPrice?: number;
    donationAmount?: number;
    overridedAmount?: number;
    scheduleDetailId?: number;
}

export class GuestData {
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

class PurchaserAddress {
    address1?: string;
    address2?: string;
    town?: string;
    region?: string;
    postalCode?: string;
}
