import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import { GuestCheckoutFormDataViewModel } from "./GuestCheckOutFormDataViewModel";
import { EventTicketAttribute } from "./EventTicketDetailDataViewModel"

export class LoginTransactionFormDataViewModel {
    userDetail?: LoginFormDataViewModel;
    userAltId?: string;
    eventTicketAttributeList: EventTicketAttribute[];
    isItinerary?: boolean;
    isTiqets?: boolean;
    transactionCurrency: string;
    donationAmount: number;
    referralId?: string;
    isBSPUpgrade?: boolean;
}