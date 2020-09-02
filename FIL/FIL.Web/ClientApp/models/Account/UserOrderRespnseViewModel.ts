// This file was generated from the Models.tst template
//

import Transaction from '../../models/Comman/TransactionViewModel';
import TransactionDetail from '../../models/Comman/TransactionDetailViewModel';
import EventTicketAttribute from '../../models/Comman/EventTicketAttributeViewModel';
import EventTicketDetail from '../../models/Comman/EventTicketDetailViewModel';
import TicketCategory from '../../models/TicketCategory/TicketCategory';
import TransactionPaymentDetail from '../../models/COmman/TransactionPaymentDetailViewModel';
import Event from '../../models/Comman/EventViewModel';
import CurrencyType from '../../models/Comman/CurrencyTypeViewModel';
import EventDetail from '../Comman/EventDetailViewModel';
import { CategoryViewModel } from '../CategoryViewModel';
import { EventCategoryMappingModel } from '../Comman/EventCategoryMappingModel';

export class UserOrderRespnseViewModel {
    transaction: Transaction[];
    transactionDetail: TransactionDetail[];
    eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    ticketCategory: TicketCategory[];
    transactionPaymentDetail: TransactionPaymentDetail[];
    event: Event[];
    currenctType: CurrencyType[];
    currentDateTime?: string;
    eventDetail?: EventDetail[];
    eventCategories?: CategoryViewModel[];
    eventCategoryMappings?: EventCategoryMappingModel[];
}
