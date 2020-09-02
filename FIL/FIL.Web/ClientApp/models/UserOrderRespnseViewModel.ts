// This file was generated from the Models.tst template
//

import Transaction  from '../models/Transaction/Transaction';
import  TransactionDetail  from '../models/Transaction/TransactionDetail';
import  EventTicketAttribute  from '../models/Comman/EventTicketAttributeViewModel';
import EventTicketDetail  from '../models/Comman/EventTicketDetailViewModel';
import TicketCategory from '../models/TicketCategory/TicketCategory';
import TransactionPaymentDetail from '../models/Transaction/TransactionPaymentDetail';
import  Event  from '../models/Comman/EventViewModel';

export class UserOrderRespnseViewModel  { 
    transaction: Transaction[];
    transactionDetail: TransactionDetail[];
    eventTicketAttribute: EventTicketAttribute[];
    eventTicketDetail: EventTicketDetail[];
    ticketCategory: TicketCategory[];
    transactionPaymentDetail: TransactionPaymentDetail;
    event: Event[];
}
