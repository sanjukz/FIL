﻿import ReportColumn from "../Common/ReportingColumn";
import TransactionDetail from "../Common/TransactionDetailViewModel";
import TicketCategory from "../Common/TicketCategoryViewModel";
import User from "../Common/UserViewModel";
import TransactionDeliveryDetail from "../Common/TransactionDeliveryDetailViewModel";
import currencyType from "../Common/CurrencyTypeViewModel";
import TransactionPaymentDetail from "../Common/TransactionPaymentDetailViewModel";
import EventTicketDetail from "../Common/EventTicketDetailViewModel";
import EventTicketAttribute from "../Common/EventTicketAttributeViewModel";
import Transaction from "../Common/TransactionViewModel";
import Event from "../Common/EventViewModel";
import EventDetail from "../Common/EventDetailViewModel";
import Venue from "../Common/VenueViewModel";
import City from "../Common/CityViewModel";
import State from "../Common/StateViewModel";
import Country from "../Common/CountryViewModel";
import EventAttribute from "../Common/EventAttributeViewModel";
import CurrencyType from "../Common/CurrencyTypeViewModel";
import IPDetail from "../Common/IPDetailViewModel";
import UserCardDetail from "../Common/UserCardDetailViewModel";
import TicketFeeDetail from "../Common/TicketFeeDetail";

export class ReportResponseDataViewModel {
    transaction: Transaction[];
    transactionDetail: TransactionDetail[];
    transactionDeliveryDetail: TransactionDeliveryDetail[];
    transactionPaymentDetail: TransactionPaymentDetail[];
    currencyType: CurrencyType[];
    eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    ticketCategory: TicketCategory[];
    event: Event[];
    eventDetail: EventDetail[];
    eventAttribute: EventAttribute[];
    venue: Venue[];
    city: City[];
    state: State[];
    country: Country[];
    user: User[];
    userCardDetail: UserCardDetail[];
    reportColumns: ReportColumn[];
    ipDetail: IPDetail[];
    ticketFeeDetail: TicketFeeDetail[];
}