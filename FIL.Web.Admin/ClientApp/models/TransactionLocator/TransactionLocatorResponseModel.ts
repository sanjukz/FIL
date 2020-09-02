export class TransactionLocatorResponseModel {
    transactionData: transactionlocatorModel[]
}
export class transactionlocatorModel {
    srNo: number;
    transactionDetailId: number;
    transactionDetailAltId: string;
    confirmationNumber: number;
    createdUtc: Date;
    userEmailId: string;
    userMobileNumber: number;
    buyerName: string;
    eventName: string;
    eventDate: Date;
    ticketCategoryName: string;
    totalTicket: number;
    grossTicketAmount: number;
    payConfNumber: number;
    paymentGateway: string;
    channel: string;
    transactionStatus: string;
    countryName: string;
    phoneCode: string;
}
