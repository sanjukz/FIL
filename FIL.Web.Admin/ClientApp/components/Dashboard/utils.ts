import * as moment from 'moment';

export enum View {
  ApproveModerate = 1,
  UserReport,
  MyFeels,
  TicketAlert,
  TransactionLocator
}

export interface IBodyRowData {
  id?: number;
  createdAt?: string;
  eventName?: string;
  altId?: string;
  isOnlineEvent?: boolean;
  eventDateTime?: string;
  ticketSold?: number;
  totalTickets?: number;
  grossAmountSold?: number;
  approvalStatus?: string;
}

export function getTableBodyData(sellerData: any): IBodyRowData[] {
  let bodyRowData: IBodyRowData[] = [];
  sellerData.events.forEach((e) => {
    let rowData: IBodyRowData = {};
    rowData.id = e.id;
    rowData.createdAt = e.createdUtc;
    rowData.eventName = e.name;
    rowData.altId = e.altId;
    rowData.isOnlineEvent = e.eventTypeId === 97 ? true : false;
    let ed = sellerData.eventDetails.filter((ed) => ed.eventId === e.id)[0];
    if (ed !== undefined) {
      rowData.eventDateTime = ed.startDateTime;
      rowData.ticketSold = 20;
      rowData.totalTickets = 200;
      rowData.grossAmountSold = 200;
      rowData.approvalStatus = 'Draft';
    }
    bodyRowData.push(rowData);
  });
  return bodyRowData;
}

export function parseDateTime(dateTime) {
  return {
    date: moment(dateTime).get('date'),
    month: moment.months(moment(dateTime).get('month')),
    year: moment(dateTime).get('year'),
    hour: moment(dateTime).get('hour'),
    minute: moment(dateTime).get('minute'),
    second: moment(dateTime).get('second'),
    day: moment.weekdays(moment(dateTime).get('day'))
  };
}

export enum UserReportColumn {
  firstName = 1,
  lastName,
  email,
  phoneNumber,
  signUpMethod,
  isOptIn,
  createdDate,
  ipAddress,
  ipCity,
  ipState,
  ipCountry,
}

export enum Experience {
  name = 1,
  userEmail,
  eventCreatedDateTime,
  roleId,
  eventStatusId
}

export enum TransactionLocatorEnum {
  transactionId = 1,
  eventName,
  firstName,
  lastName,
  phoneNumber,
  email,
  transactionStatus,
  totalTicket,
  transactionCreatedUtc,
  ticketCategoryName,
  currencyCode,
  promoCode,
  discountAmount,
  grossTicketAmount,
  netTicketAmount,
  payConfNumber,
  streamAltId,
  ipAddress,
  ipCity,
  ipState,
  ipCountry,
}