export default class TransactionDetail {
    id: number;
    transactionId: number;
    eventTicketAttributeId: number;
    totalTickets: number;
    pricePerTicket: number;
    deliveryCharges?: number;
    convenienceCharges: number;
    serviceCharge?: number;
    discountAmount?: number;
    ticketTypeId: number;
}
