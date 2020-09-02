export default class Transaction {
    id: number;
	altId: string;
    totalTickets: number;
    grossTicketAmount: number;
    deliveryCharges: number;
    convenienceCharges: number;
    serviceCharge: number;
    discountAmount: number;
    discountCode: string;
    netTicketAmount: number;
    transactionStatusId: string;
    firstName: string;
    lastName: string;
    phoneCode: string;
    phoneNumber: string;
    emailId: string;
    countryName: string;
    createdUtc: string;
    createdBy: string;
    channelId: string;
    currencyId: number;
    ipDetailId?: number;
}
