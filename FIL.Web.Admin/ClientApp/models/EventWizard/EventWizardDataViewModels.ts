export class EventFormDataViewModel {
    eventCategoryId: number;
    eventTypeId: number;
    name: string;
    description: string;
    clientPointOfContactId: number;
    fbEventId: number;
    metaDetails: string;
    termsAndConditions: string;
    isPublishedOnSite: boolean;
}

export class EventDetailFormDataViewModel {
    venueId: number;
    startDateTime: Date;
    endDateTime: Date;
    groupId: number;
}

export class EventTicketAttributeFormDataViewModel {
    salesStartDateTime: Date;
    salesEndDatetime: Date;
    ticketTypeId: number;
    channelId: number;
    currencyId: number;
    sharedInventoryGroupId: number;
    availableTicketForSale: number;
    remainingTicketForSale: number;
    ticketCategoryDescription: string;
    viewFromStand: string;
    isSeatSelection: boolean;
    price: number;
    isInternationalCardAllowed: boolean;
    isEMIApplicable: boolean;
}

export class EventAttributeFormDataViewModel {
    matchNo: number;
    matchDay: number;
    gateOpenTime: string;
    timeZone: string;
    timeZoneAbbreviation: string;
    ticketHtml: string;
}

export class EventWizardDataViewModel {
    eventForm: EventFormDataViewModel;
    eventDetail: EventDetailFormDataViewModel;
    eventTicketAttribute: EventTicketAttributeFormDataViewModel;
    eventAttribute: EventAttributeFormDataViewModel;
}

export class EventWizardResponseModel {
    success: boolean;
}