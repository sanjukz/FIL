export default class TransactionDetail {
    transactionId: number;
    eventTicketAttributeId: number;
    totalTickets: number;
    pricePerTicket: number;
    deliveryCharges: number;
    convenienceCharges: number;
    serviceCharge: number;
    discountAmount: number;
    referralId?: number;
}