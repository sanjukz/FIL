import ReportColumn from "../Common/ReportingColumn";
import TransactionDetail from "../Common/TransactionDetailViewModel";
import EventTicketDetail from "../Common/EventTicketDetailViewModel";
import EventTicketAttribute from "../Common/EventTicketAttributeViewModel";
import Transaction from "../Common/TransactionViewModel";
import Event from "../Common/EventViewModel";
import EventDetail from "../Common/EventDetailViewModel";
import EventAttribute from "../Common/EventAttributeViewModel";
import MatchSeatTicketDetail from "../Common/MatchSeatTicketDetailViewModel";
import TicketCategory from "../Common/TicketCategoryViewModel";

export class ScanningReportResponseViewModel {
    transaction: Transaction[];
    transactionDetail: TransactionDetail[];
    matchSeatTicketDetail: MatchSeatTicketDetail[];
    //eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    eventDetail: EventDetail[];
    eventAttribute: EventAttribute[];
    event: Event[];    
    ticketCategory: TicketCategory[];
    reportColumns: ReportColumn[];
}
