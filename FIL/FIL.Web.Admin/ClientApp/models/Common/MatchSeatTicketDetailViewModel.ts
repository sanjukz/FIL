export default class MatchSeatTicketDetail {
    price: number;
    seatStatusId: number;
    transactionId: number;
    matchLayoutSectionSeatId: number;
    eventTicketDetailId: number;
    barcodeNumber: string;
    entryCount?: number;
    entryStatus: boolean;
    entryDateTime?: string;
    checkedDateTime?: string;
    entryCountAllowed?: number;
    entryGateName: string;
    ticketTypeId: string;
}
