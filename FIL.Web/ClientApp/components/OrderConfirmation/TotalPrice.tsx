import * as React from "react";
import * as numeral from "numeral";

export default class TotalPrice extends React.Component<any, any> {
    public render() {
        var transactionDetails = this.props.TransactionDetails;
        var TotalAmount = 0;
        for (var i = 0; i < transactionDetails.length; i++) {
            TotalAmount = TotalAmount + (transactionDetails[i].totalTickets * transactionDetails[i].pricePerTicket);
        }

        return <span className="float-sm-right">
                    {this.props.currency.code}{numeral(TotalAmount).format('00.00')}
                </span>
    }
}