import ReportingColumn from "../Common/ReportingColumn";

export default class TransactionReportResponseViewModel {
    transactionReports: TransactionReportViewModel;
}
export class TransactionReportViewModel {
    transactionData: TransactionReportModel[];
    currencyWiseSummary: TransactionReportModel[];
    channelWiseSummary: TransactionReportModel[];
    ticketTypeWiseSummary: TransactionReportModel[];
    venueWiseSummary: TransactionReportModel[];
    eventWiseSummary: TransactionReportModel[];
    reportColumns: ReportingColumn[];
    summaryColumns: ReportingColumn[];
    dynamicSummaryColumns: ReportingColumn[];
    dynamicSummaryInfoColumns: ReportingColumn[];
};

export class TransactionReportModel {
    sno: number;
    transactionId: number;
    transactionDate: string;
    transactionTime: string;
    customerName: string;
    customerPhone: string;
    outletBOName: string;
    channel: string;
    event: string;
    eventName: string;
    eventStartDateTime: string;
    venueAddress: string;
    venueCity: string;
    venueCountry: string;
    ticketCategory: string;
    seatNumber: string;
    ticketType: string;
    deliveryType: string;
    currency: string;
    pricePerTicket: number;
    numberOfTickets: number;
    grossTicketAmount: number;
    discountAmount: number;
    donationAmount?: number;
    promoCode: string;
    netTicketAmount: number;
    netTicketAmountUSD: number;
    serviceCharge: number;
    deliveryCharges: number;
    convenienceCharges: number;
    exchangeRate: number;
    totalTransactedAmount: number;
    totalTransactedAmountUSD: number;
    customerIP: string;
    iPBasedCountry: string;
    iPBasedState: string;
    iPBasedCity: string;
    saleStatus: string;
    payConfNumber: string;
    transactionType: string;
    cardNumber: string;
    nameOnCard: string;
    paymentGateway: string;
    cardType: string;
    modeOfPayment: string;
    referralName: string;
    streamLink?: string;
}