import { EventTicketAttribute } from "./EventTicketDetailDataViewModel"

export class CheckoutSignInDataViewModel {
    userAltId: string;
    eventTicketAttributeList: EventTicketAttribute[];
    isItinerary?: boolean;
    transactionCurrency?: string;
    isTiqets?: boolean;
}

