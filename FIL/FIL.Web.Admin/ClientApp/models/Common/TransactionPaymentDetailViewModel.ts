export default class TransactionPaymentDetail {
    id: number;
    transactionId: number;
    paymentOptionId: string;
    paymentGatewayId: string;
    requestType: string;
    amount: string;
    payConfNumber: string;
    paymentDetail: string;
    userCardDetailId: number;
}
