export default class TransactionDetail {
    eventTicketAttributeId?: number;
    totalTickets: number;
    pricePerTicket: number;
    deliveryCharges: number;
    convenienceCharges: number;
    serviceCharge: number;
    discountAmount: number;
    visitDate: string;
    visitEndDate?: string;
    transactionType?: number;
    id?: number;
    transactionId?: number;
}
