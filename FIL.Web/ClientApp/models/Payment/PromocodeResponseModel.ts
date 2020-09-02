export class PromocodeResponseModel {
    success: boolean;
    isLimitExceeds: boolean;
    transactionId: number;
    currencyId: number;
    grossTicketAmount: number;
    deliveryCharges: number;
    convenienceCharges: number;
    serviceCharge: number;
    discountAmount: number;
    netTicketAmount: number;
    isPaymentBypass?: number;
}