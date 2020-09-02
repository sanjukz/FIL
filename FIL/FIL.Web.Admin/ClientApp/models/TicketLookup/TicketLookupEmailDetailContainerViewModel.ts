import CurrencyType from "../Common/CurrencyTypeViewModel";
import Transaction from "../Common/TransactionViewModel";
import TicketLookupSubContainer from "./TicketLookupSubContainerViewModel";
export default class TicketLookupEmailDetailContainer {
    
    currencyType: CurrencyType;
    transaction: Transaction;
    paymentOption: string;
    payconfigNumber: string;
    ticketLookupSubContainer: TicketLookupSubContainer[];
}
