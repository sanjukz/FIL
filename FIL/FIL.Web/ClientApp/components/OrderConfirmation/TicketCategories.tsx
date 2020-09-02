import * as React from "react";
import * as numeral from "numeral";

export default class TicketCategories extends React.Component<any, any> {

    public render() {
        var transactionDetails = this.props.transactionDetails;
        var ticketCategories = this.props.ticketCategories;
        var currency = this.props.currency;
        var i = -1;
        var ticketCategories = ticketCategories.map((item) => {
            i = i + 1;
            return <p className="m-0">
                <small>
                    {item.name} ({transactionDetails[i].totalTickets + " x " + currency.code} {numeral(transactionDetails[i].pricePerTicket).format('00.00')})
                    <span className="d-block border-bottom pb-2 mb-2">
                        {currency.code} {numeral(transactionDetails[i].pricePerTicket * transactionDetails[i].totalTickets).format('00.00')}
                    </span>
                </small>
            </p>
        })
        return <>
                {ticketCategories}
              </>
    }
}