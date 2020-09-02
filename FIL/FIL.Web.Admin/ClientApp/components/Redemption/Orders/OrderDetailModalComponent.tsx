import * as React from "react";
import Modal from 'react-awesome-modal';
import * as numeral from "numeral";
import { ParseDateToLocal } from "../../../utils/ParseDateToLocal";
import { ApproveStatus } from "../../../../ClientApp/models/Redemption/ApproveStatus";
import * as _ from "lodash";

export default class OrderDetailModalComponent extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            isShow: false
        }
    }
    render() {
        let that = this;
        return (<div>
            <h5 className={"card-header bg-transparent text-center" + (this.state.displayScreen == ApproveStatus.Pending ? " border-warning" : this.state.displayScreen == ApproveStatus.Approved ? " border-primary" : " border-success")} ><a href="javascript:void(0)">{that.props.currentOrder.placeName}</a></h5>
            <div className="card-body">
                <p className="card-text">Order Status: {that.props.currentOrder.orderStatus}</p>
                <p className="card-text">Transaction Id: {that.props.currentOrder.transactionId}</p>
                <p className="card-text">Visit Date: {that.props.currentOrder.visitDate ? (ParseDateToLocal(that.props.currentOrder.visitDate).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + ParseDateToLocal(that.props.currentOrder.visitDate).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(ParseDateToLocal(that.props.currentOrder.visitDate).getHours()).format('00') + ":" + numeral(ParseDateToLocal(that.props.currentOrder.visitDate).getMinutes()).format('00')) : "--"}</p>
                <p className="card-text">Customer Name: {that.props.currentOrder.customerFirstName + " " + that.props.currentOrder.customerLastName}</p>
                <p className="card-text">Phone Number: {(that.props.currentOrder.phonceCode && that.props.currentOrder.phoneNumber) ? ('+' + that.props.currentOrder.phonceCode + ' - ' + that.props.currentOrder.phoneNumber) : (!that.props.currentOrder.phonceCode && that.props.currentOrder.phoneNumber) ? that.props.currentOrder.phoneNumber : '--'}</p>
                <p className="card-text">Category: {that.props.currentOrder.ticketCategory}</p>
                <p className="card-text">Price: {that.props.currentOrder.currency + " " + that.props.currentOrder.ticketPrice}</p>
                {(this.props.screenStatus == ApproveStatus.Approved) && < p className="card-text">Approved Date: {(that.props.currentOrder.orderApprovedDate && new Date(that.props.currentOrder.orderApprovedDate).getFullYear() != 1) ? (ParseDateToLocal(that.props.currentOrder.orderApprovedDate).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + ParseDateToLocal(that.props.currentOrder.orderApprovedDate).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(ParseDateToLocal(that.props.currentOrder.orderApprovedDate).getHours()).format('00') + ":" + numeral(ParseDateToLocal(that.props.currentOrder.orderApprovedDate).getMinutes()).format('00')) : "--"}</p>}
                {(this.props.screenStatus == ApproveStatus.Success) && < p className="card-text">Completed Date: {(that.props.currentOrder.orderCompletedDate && new Date(that.props.currentOrder.orderCompletedDate).getFullYear() != 1) ? (ParseDateToLocal(that.props.currentOrder.orderCompletedDate).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + ParseDateToLocal(that.props.currentOrder.orderCompletedDate).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(ParseDateToLocal(that.props.currentOrder.orderCompletedDate).getHours()).format('00') + ":" + numeral(ParseDateToLocal(that.props.currentOrder.orderCompletedDate).getMinutes()).format('00')) : "--"}</p>}
                {(this.props.screenStatus == ApproveStatus.Approved || this.props.screenStatus == ApproveStatus.Success) && < p className="card-text">Approved By (Guide Name): {that.props.currentUser.length > 0 ? that.props.currentUser[0].firstName + " " + that.props.currentUser[0].lastName : "--"}</p>}
            </div>
        </div>
        )
    }
}
