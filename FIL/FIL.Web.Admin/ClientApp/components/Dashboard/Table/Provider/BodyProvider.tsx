import * as React from 'react';
import { CopyToClipboard } from 'react-copy-to-clipboard';
import * as numeral from "numeral";
import { Popover, Badge } from 'antd';
import EventColumn from '../Column/Event';
import ActionColumn from '../Column/Action';
import { View } from '../../utils';
import { DollarCircleTwoTone, CopyOutlined } from '@ant-design/icons';
import { EventInformation } from '../../Modal/EventInformation';
import { TransactionInformation } from '../../Modal/TransactionInformation';

export const EVENT_STATUS = ['None', 'Pending', 'Draft', 'Waiting For Approval', 'Rejected', 'Published'];

export const parseDateLocal = (s) => {
  var b = s.split(/\D/);
  var date = new Date(s);
  return new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000)
}

const getBody = (props: any) => {
  const [isShowEventModal, setIsShowEventModal] = React.useState(false)
  const [isShowTransactionModal, setIsShowTransactionModal] = React.useState(false)
  let rowData = props.rowData;
  let onChangeEventStatus = props.onChangeEventStatus;
  if (props.view == View.ApproveModerate) {
    return <>
      <EventColumn row={rowData} />
      <td>{rowData.userEmail ? rowData.userEmail : 'Admin'}</td>
      <td>{parseDateLocal(rowData.eventCreatedDateTime).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + parseDateLocal(rowData.eventCreatedDateTime).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(parseDateLocal(rowData.eventCreatedDateTime).getHours()).format('00') + ":" + numeral(parseDateLocal(rowData.eventCreatedDateTime).getMinutes()).format('00')}</td>
      <td>{rowData.roleId == 10 ? 'Admin' : 'Seller'}</td>
      <td> {rowData.isEnabled && <Popover content="Event is live on site" ><Badge color={'green'} /></Popover>} {EVENT_STATUS[rowData.isEnabled ? 5 : rowData.eventStatusId ? rowData.eventStatusId - 1 : 2]}</td>
      <ActionColumn row={{ rowData, onChangeEventStatus, view: props.view }} />
    </>
  } else if (props.view == View.UserReport) {
    return <>
      <td>{(rowData.isTransacted && rowData.isCreator == "Yes") ? <>
        <Popover content="Creator on site" >
          <img className="mr-2" src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/feelAdmin/creator-icon.png" width="16" />
        </Popover>
        <Popover content="Successfully transacted on site" >
          <DollarCircleTwoTone />
        </Popover>
      </> : (rowData.isTransacted && rowData.isCreator == "No") ? <><Popover content="Successfully transacted on site" ><DollarCircleTwoTone /></Popover></> : <></>} {rowData.firstName}</td>
      <td>{rowData.lastName}</td>
      <td><a href={`mailto:${rowData.email}`}>{rowData.email}</a></td>
      <td><a href={`tel:${rowData.phoneCode ? rowData.phoneCode + rowData.phoneNumber : rowData.phoneNumber}`}>{rowData.phoneCode ? rowData.phoneCode + ' - ' + rowData.phoneNumber : rowData.phoneNumber}</a></td>
      <td>{rowData.signUpMethod == "Regular" ? "Email" : rowData.signUpMethod}</td>
      <td>{rowData.isOptIn}</td>
      <td>{parseDateLocal(rowData.createdDate).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + parseDateLocal(rowData.createdDate).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(parseDateLocal(rowData.createdDate).getHours()).format('00') + ":" + numeral(parseDateLocal(rowData.createdDate).getMinutes()).format('00')}</td>
      <td>{rowData.ipAddress != "::1" ? rowData.ipAddress : ""}</td>
      <td>{rowData.ipCity}</td>
      <td>{rowData.ipState}</td>
      <td>{rowData.ipCountry}</td>
    </>
  } else if (props.view == View.MyFeels) {
    return <>
      <EventColumn row={rowData} />
      <td>{`${rowData.soldTicket}/${rowData.ticketForSale}`}</td>
      <td> {rowData.isEnabled && <Popover content="Event is live on site" ><Badge color={'green'} /></Popover>} {rowData.isPastEvent ? 'Past' : EVENT_STATUS[rowData.isEnabled ? 5 : rowData.eventStatusId ? rowData.eventStatusId - 1 : 2]}</td>
      <ActionColumn row={{ rowData, onChangeEventStatus, view: props.view }} />
    </>
  } else if (props.view == View.TicketAlert) {
    return <>
      <td>{rowData.firstName}</td>
      <td>{rowData.lastName}</td>
      <td>{props.props.selectedEvent.split('X')[0]}</td>
      <td>{rowData.ticketCount}</td>
      <td><a href={`tel:${rowData.phoneCode ? rowData.phoneCode + rowData.phoneNumber : rowData.phoneNumber}`}>{rowData.phoneCode ? rowData.phoneCode + ' - ' + rowData.phoneNumber : rowData.phoneNumber}</a></td>
      <td><a href={`mailto:${rowData.email}`}>{rowData.email}</a></td>
    </>
  } else if (props.view == View.TransactionLocator) {
    return <>
      <EventInformation
        visible={isShowEventModal}
        rowData={rowData}
        onCloseModal={() => {
          setIsShowEventModal(false)
        }}
      />
      <TransactionInformation
        visible={isShowTransactionModal}
        rowData={rowData}
        onCloseModal={() => {
          setIsShowTransactionModal(false)
        }}
      />
      <td><a href="javascript:Void(0)" onClick={e => setIsShowTransactionModal(true)} >{rowData.transactionId}</a></td>
      <td><a href="javascript:Void(0)" onClick={e => setIsShowEventModal(true)} >{rowData.eventName}</a></td>
      <td>{rowData.firstName}</td>
      <td>{rowData.lastName}</td>
      <td><a href={`tel:${(!rowData.phoneCode && !rowData.phoneNumber) ? "None" : rowData.phoneCode ? '+' + rowData.phoneCode + rowData.phoneNumber : rowData.phoneNumber}`}>{(!rowData.phoneCode && !rowData.phoneNumber) ? "None" : rowData.phoneCode ? '+' + rowData.phoneCode + rowData.phoneNumber : rowData.phoneNumber}</a></td>
      <td><a href={`mailto:${rowData.email}`}>{rowData.email}</a></td>
      <td style={{ color: rowData.transactionStatus == 'Success' ? 'green' : rowData.transactionStatus == 'UnderPayment' ? 'crimson' : 'black' }} >{rowData.transactionStatus}</td>
      <td>{rowData.totalTicket}</td>
      <td>{parseDateLocal(rowData.transactionCreatedUtc).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + parseDateLocal(rowData.transactionCreatedUtc).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(parseDateLocal(rowData.transactionCreatedUtc).getHours()).format('00') + ":" + numeral(parseDateLocal(rowData.transactionCreatedUtc).getMinutes()).format('00')}</td>
      <td>{rowData.ticketCategoryName}</td>
      <td>{rowData.currencyCode}</td>
      <td>{rowData.promoCode ? rowData.promoCode : 'None'}</td>
      <td>{numeral(rowData.discountAmount).format('0.00')}</td>
      <td>{numeral(rowData.grossTicketAmount).format('0.00')}</td>
      <td>{numeral(rowData.netTicketAmount).format('0.00')}</td>
      <td>{rowData.payConfNumber ? rowData.payConfNumber : 'None'}</td>
      <td>{rowData.streamAltId ? <><Popover content="Copy to clipboard">
        <CopyToClipboard text={getStreamLink(rowData.streamAltId)}
          onCopy={() => { }}>
          <CopyOutlined onClick={(e: any) => {
          }} /></CopyToClipboard></Popover><a target="_blank" href={getStreamLink(rowData.streamAltId)} >{getStreamLink(rowData.streamAltId)}</a></>
        : 'None'}</td>
      <td>{rowData.ipAddress}</td>
      <td>{rowData.ipCity}</td>
      <td>{rowData.ipState}</td>
      <td>{rowData.ipCountry}</td>
    </>
  }
}

const getStreamLink = (streamLink) => {
  if (window.location.href.indexOf('dev') > -1) {
    return `https://dev.feelitlive.com/stream-online?token=${streamLink}`
  } else {
    return `https://www.feelitlive.com/stream-online?token=${streamLink}`
  }
}


function BodyProvider(props: any) {
  return (
    <>
      {getBody(props.props)}
    </>
  );
}

export default BodyProvider;
