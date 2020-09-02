import { EventFrequencyType } from "../../Enums/EventFrequencyType";
import { MasterEventTypes } from "../../Enums/MasterEventTypes";
import { Eventstatuses } from "../../Enums/Eventstatuses";

export interface TransactionLocatorResponseViewModel {
    filTransactionLocator: FilTransactionLocator;
    success: boolean;
}

export interface FilTransactionLocator {
    transactionData: TransactionData[];
}

export interface TransactionData {
    transactionId: number;
    transactionAltId: string;
    transactionCreatedUtc: string;
    email: string;
    phoneCode: string;
    phoneNumber: string;
    firstName: string;
    lastName: string;
    eventName: string;
    eventStartDate: string;
    eventEndDate: string;
    eventFrequencyType: EventFrequencyType;
    isEventEnabled: boolean;
    isEventDetailEnabled: boolean;
    eventId: number;
    eventAltId: string;
    masterEventTypeId: MasterEventTypes;
    eventStatusId: Eventstatuses;
    ticketCategoryName: string;
    totalTicket: number;
    timeZone: string;
    timeZoneAbbreviation: string;
    currencyCode: string;
    grossTicketAmount: number;
    netTicketAmount: number;
    payConfNumber: string;
    channel: string;
    transactionStatus: string;
    ipAddress: string;
    ipCity: string;
    ipState: string;
    ipCountry: string;
    streamAltId?: string;
    discountAmount: number;
    promoCode: number;
    localStartDateString: string;
    localEndDateString: string;
}