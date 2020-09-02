import Transaction from "../Common/TransactionViewModel";
import TransactionDetail from "../Common/TransactionDetailViewModel";
import TicketCategory from "../Common/TicketCategoryViewModel";
import Event from "../Common/EventViewModel";
import EventDetail from "../Common/EventDetailViewModel";
import EventTicketDetail from "../Common/EventTicketDetailViewModel";
import EventTicketAttribute from "../Common/EventTicketAttributeViewModel";
import currencyType from "../Common/CurrencyTypeViewModel";
import Venue from "../Common/VenueViewModel";
import City from "../Common/CityViewModel";
import CurrencyType from "../Common/CurrencyTypeViewModel";
import CorporateTicketAllocationDetail from "../Common/CorporateTicketAllocationDetailViewModel";
import CorporateTransactionDetail from "../Common/CorporateTransactionDetailViewModel";
import Sponsor from "../Common/SponsorViewModel";
import ReportColumn from "../Common/ReportingColumn";

export class InventoryReportResponseDataViewModel {
    ticketCategory: TicketCategory[];
    eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    corporateTicketAllocationDetail: CorporateTicketAllocationDetail[];
    corporateTransactionDetail: CorporateTransactionDetail[];
    sponsor: Sponsor[];
    transaction: Transaction[];
    transactionDetail: TransactionDetail[];
    reportColumns: ReportColumn[];
}
