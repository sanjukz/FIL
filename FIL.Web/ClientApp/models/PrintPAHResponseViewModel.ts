export class PrintPAHResponseViewModel {
    pahDetail: PAHDetail[];
    success: boolean;
}

export class PAHDetail {
    venueId: number;
    venueName: string;
    cityName: string;
    stateName: string;
    countryName: string;
    firstName: string;
    lastName: string;
    emailId: string;
    eventId: number;
    eventsourceId: number;
    eventName: string;
    eventDeatilId: number;
    eventDetailsName: string;
    eventStartTime: string;
    ticketHtml: string;
    ticketCategoryId: number;
    ticketCategoryName: string;
    totalTickets: number;
    phoneNumber: string;
    price: string;
    barcodeNumber: string;
    currencyName: string;
    transactionId: number;
    categoryWiseTickets: CategoryWiseTickets[];
    timeSlot: string;
}

export class CategoryWiseTickets {
    categoryName: string;
    totalTickets: number;
}