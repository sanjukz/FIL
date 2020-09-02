/* Third party imports */
import * as React from 'react'
import * as numeral from "numeral";
import { Popover, Badge } from 'antd';

/* Local imports */
import { Confirmation } from "../Confirmation";

export const TicketListing: React.FC<any> = (props: any) => {
  return (props.ticketViewModel.tickets.filter((val) => { return val.ticketCategoryId != 19452 && val.ticketCategoryId != 12259 }).map((item, index) => {
    let currency = props.currencyList.filter((val: any) => { return val.id == item.currencyId });
    return (
      <div key={item.etdId} className="col-sm-6 p-0 mb-2">
        <div className="rounded p-2 bg-white border">
          <a
            className="text-muted"
            href="javascript:void(0)"
            onClick={() => {
              props.onClickTicket(item);
            }}
          > {`${item.name} - ${currency.length > 0 ? currency[0].code : "USD $"} ${numeral(item.price).format("00.00")}`}</a>
          {item.isEnabled && <span className="ml-2"><Popover content="Ticket is active & on sale" ><Badge color={'green'} /></Popover></span>}
          <Confirmation
            id={item.ticketAltId}
            item={props.item}
            onEdit={() => {
              props.onClickTicket(item);
            }}
            message={props.isAddOn ? 'Are you sure, you want to delete the add-on?' : 'Are you sure, you want to delete the ticket?'}
            onDelete={(ticketId: any) => {
              props.onDeleteTicket(ticketId)
            }} />
        </div>
      </div>
    )
  }))
}

