import ReportColumn from "../Common/ReportingColumn";

export class TransactionReportResponseViewModel {
    transactionReport: TransactionReport[];
    reportColumns: ReportColumn[];
}

export class TransactionReport {
        eventTransId : number;
        emailId: string;
        ipAddress: string;
        phoneNumber: string;
        customerName: string;
        modeOfPayment: string;
        cardType: string;
        saleStatus: string;
        createdDate: string;
        createdTime: string;
        eventName: string;
        subEventName: string;
        eventCity: string;
        channels: string;
        currencyName: string;
        outletName: string;
        customerCountry: string;
        customerState: string;
        customerCity: string;
        cardIssuingCountry: string;
        suspectTransaction: string;
        eventDate: string;
        venueAddress: string;
        promocode: string;
        ticketCategoty: string;
        seatNumber: string;
        ticketType: string;
        numberOfTickets: number;
        pricePerTicket: number;
        grossTicketAmount: number;
        discountAmount: number;
        netTicketAmount: number;
        convenienceCharges: number;
        serviceTax: number;
        totalTransactedAmount: number;
        courierCharge: number;
        deliveryType: string;
        transactionType: string;
        paymentGateway: string;
        payConfNumber: string;
        entryCount: number;
        nameOnCard: string;
}
