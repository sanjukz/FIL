import Events from "../Comman/EventViewModel";
export default class Transaction {
    id: number;
    altId: string;
    totalTickets: number;
    grossTicketAmount: number;
    deliveryCharges: number;
    convenienceCharges: number;
    serviceCharge: number;
    discountAmount: number;
    netTicketAmount: number;
    donationAmount: number;
    transactionStatusId: string;
    firstName: string;
    lastName: string;
    phoneCode: string;
    phoneNumber: string;
    currencyId?: number;
    emailId: string;
    countryName: string;
    createdUtc: string;
    createdBy?: string;
    transactions: number[];
    events: Events[];
    currency?: string;
}