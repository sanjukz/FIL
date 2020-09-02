import * as React from 'react';

import { View, UserReportColumn, Experience, TransactionLocatorEnum } from '../../utils';
import { transactionlocatorModel } from '../../../../models/TransactionLocator/TransactionLocatorResponseModel';

const getHeader = (view: View, props) => {
  if (view == View.ApproveModerate) {
    return <>
      <th scope="col" style={{ width: '35%' }}>Event<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(Experience.name, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Owner<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(Experience.userEmail, e.target.value) }} placeholder="search" /></th>
      <th scope="col" style={{ minWidth: '170px' }}>Created Date & Time<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(Experience.eventCreatedDateTime, e.target.value) }} placeholder="search" /></th>
      <th scope="col" style={{ minWidth: '150px' }}>Created By<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(Experience.roleId, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Status<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(Experience.eventStatusId, e.target.value) }} placeholder="search" /></th>
      <th scope="col"></th>
      <th></th>
    </>;
  } else if (view == View.UserReport) {
    return <>
      <th scope="col">First Name<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.firstName, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Last Name<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.lastName, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Email<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.email, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Phone Number<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.phoneNumber, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Sign Up Method<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.signUpMethod, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Opt In<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.isOptIn, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Created Date & Time<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.createdDate, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP Address<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.ipAddress, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP Based City<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.ipCity, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP Based State<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.ipState, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP Based Country<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(UserReportColumn.ipCountry, e.target.value) }} placeholder="search" /></th>
    </>;
  } else if (view == View.MyFeels) {
    return <>
      <th scope="col" style={{ width: '35%' }}>Event<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(Experience.name, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Sold</th>
      <th scope="col">Status<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(Experience.eventStatusId, e.target.value) }} placeholder="search" /></th>
      <th scope="col"></th>
      <th></th>
    </>;
  } else if (view == View.TicketAlert) {
    return <>
      <th scope="col">First Name</th>
      <th scope="col">Last Name</th>
      <th scope="col">Event</th>
      <th scope="col">Ticket Count</th>
      <th scope="col">Phone</th>
      <th scope="col">Email</th>
      <th scope="col"></th>
      <th></th>
    </>;
  } else if (view == View.TransactionLocator) {
    return <>
      <th scope="col">Confirmation Id<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.transactionId, e.target.value) }} placeholder="search" /></th>
      <th scope="col" style={{ minWidth: '200px' }}>Event<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.eventName, e.target.value) }} placeholder="search" /></th>
      <th scope="col">First Name<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.firstName, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Last Name<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.lastName, e.target.value) }} placeholder="search" /></th>
      <th scope="col" style={{ minWidth: '200px' }}>Phone<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.phoneNumber, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Email<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.email, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Status<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.transactionStatus, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Total Tickets<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.totalTicket, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Transaction Date & Time<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.transactionCreatedUtc, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Ticket Category<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.ticketCategoryName, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Currency Code<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.currencyCode, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Promo Code<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.promoCode, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Discount Amount<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.discountAmount, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Gross Ticket Amount<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.grossTicketAmount, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Net Ticket Amount<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.netTicketAmount, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Pay Config Number<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.payConfNumber, e.target.value) }} placeholder="search" /></th>
      <th scope="col">Stream Link<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.streamAltId, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP Address<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.ipAddress, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP City<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.ipCity, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP State<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.ipState, e.target.value) }} placeholder="search" /></th>
      <th scope="col">IP Country<br /> <input className="form-control" style={{ minWidth: "150px", border: "0" }} onChange={(e) => { props.onChangeColumnSearch(TransactionLocatorEnum.ipCountry, e.target.value) }} placeholder="search" /></th>
      <th></th>
    </>;
  } else {
    return <></>
  }
}

function HeaderProvider(props: any) {
  return (
    <>
      {getHeader(props.props.view, props.props)}
    </>
  );
}

export default HeaderProvider;
