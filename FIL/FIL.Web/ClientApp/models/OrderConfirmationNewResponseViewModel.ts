import CurrencyType from "./Comman/CurrencyTypeViewModel";
import Transaction from "./Comman/TransactionViewModel";
import UserCardDetail from "./Comman/UserCardDetailViewModel"
import TransactionPaymentDetail from "./Comman/TransactionPaymentDetailViewModel";
import OrderConfirmationSubContainer from "./OrderConfirmationSubContainer";

export class OrderConfirmationNewResponseViewModel {
    transactionQrcode: string;
    currencyType: CurrencyType;
    transaction: Transaction;
    userCardDetail: UserCardDetail;
    paymentOption: string;
    cardTypes: string;
    transactionPaymentDetail: TransactionPaymentDetail;
    orderConfirmationSubContainer: OrderConfirmationSubContainer[];
    streamLink?: string;
}
