import * as React from "react";
import SubEvents from "./SubEvents";
import { ReactHTMLConverter } from "react-html-converter/browser";
import * as Moment from "moment";

export default class EventItems extends React.Component<any, any> {

    public render() {
        var orderConfirmationData = this.props.eventContainer;
        var eventContainer = this.props.eventContainer.orderConfirmationSubContainer;
        var currency = this.props.currency;
        const converter = ReactHTMLConverter();
        var eventDetails = eventContainer.map((item) => {
            return <div>
                <hr />
                <div className="row">
                    <div className="col-sm-6"><h5>Items to be Picked By {orderConfirmationData.transaction.firstName} {orderConfirmationData.transaction.lastName}</h5></div>
                    <div className="col-sm-6"><h5>Your visit date and time: {Moment(item.subEventContainer[0].transactionDetail[0].visitDate).format("ddd, MMM DD, YYYY")} &nbsp;&nbsp; {Moment(item.subEventContainer[0].transactionDetail[0].visitDate).format('HH:mm')}</h5></div>
                </div>
                <hr />
                <div className="row">
                    <div className="col-sm-6">
                        <p>
                            <span className="h5 d-block">Location: </span>
                            <i className="fa fa-map-marker-alt" aria-hidden="true"></i>{item.subEventContainer[0].venue.name},{item.subEventContainer[0].city.name}
                        </p>
                        <p>
                            <span className="h5 d-block">Contact: </span>
                            <i className="fa fa-mobile-alt"></i>{orderConfirmationData.transaction.firstName} {orderConfirmationData.transaction.lastName}  <br />
                            {orderConfirmationData.transaction.emailId} <br />
                            {orderConfirmationData.transaction.phoneNumber != "" &&
                                "+" + orderConfirmationData.transaction.phoneCode + " - " + orderConfirmationData.transaction.phoneNumber}
                        </p>
                        <h5>Policy</h5>
                        <ul>
                            {item.subEventContainer[0].eventDeliveryTypeDetail != '' ? converter.convert(item.subEventContainer[0].eventDeliveryTypeDetail[0].notes) : ''}
                        </ul>
                    </div>
                    <div className="col-md-6">
                        <SubEvents subEventContainer={item.subEventContainer} currency={currency} />
                    </div>
                </div>
            </div>
        })
        return <>
            {eventDetails}
        </>
    }
}