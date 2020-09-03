import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import { IApplicationState } from "../stores";
import * as OrderConfirmationStore from "../stores/OrderConfirmation";
import { OrderConfirmationFormDataViewModel } from "../models/OrderConfirmationFormDataViewModel";
import FilLoader from "../components/Loader/FilLoader";
import "./OrderConfirmation.scss";
import * as numeral from "numeral";
import * as PubSub from "pubsub-js";
import * as Moment from "moment";
import {
    withScriptjs,
    withGoogleMap,
    GoogleMap,
    Marker
} from "react-google-maps";
import { compose, withProps, withStateHandlers } from "recompose";
import * as parse from "url-parse";
import { gets3BaseUrl } from "../utils/imageCdn";
import { MasterEventTypes } from "../Enum/MasterEventTypes";
import EcommercePurchaseGtm from "../components/analytics/EcommercePurchaseGtm";

const StyledMap = compose(
    withProps({
        googleMapURL:
            "https://maps.googleapis.com/maps/api/js?key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU&v=3.exp&libraries=geometry,drawing,places",
        loadingElement: <div style={{ height: `100%` }} />,
        containerElement: <div style={{ height: `300px` }} />,
        mapElement: <div style={{ height: `100%` }} />
    }),
    withStateHandlers(
        () => ({
            isOpen: false,
            markerid: 0
        }),
        {
            onToggleOpen: () => marker => ({
                isOpen: true,
                markerid: marker.eventDetails.altId
            }),
            onToggleClose: () => () => ({
                isOpen: false
            })
        }
    ),
    withScriptjs,
    withGoogleMap
)((props: any) => (
    <GoogleMap
        defaultZoom={2}
        defaultCenter={{ lat: 17.9638031, lng: 1.8769136 }}
    >
        {props.marks.map((mark, index) => {
            return (
                <Marker
                    key={index}
                    position={mark}
                    icon={`${gets3BaseUrl()}/logos/fap-pin-map.svg`}
                ></Marker>
            );
        })}
    </GoogleMap>
));

type OrderConfirmationComponentProps = OrderConfirmationStore.IOrderConfirmationState &
    typeof OrderConfirmationStore.actionCreators &
    RouteComponentProps<{ transactionAltId: string }>;

class OrderConfirmation extends React.PureComponent<
    OrderConfirmationComponentProps,
    any
    > {
    constructor(props) {
        super(props);
    }

    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }
        const data: any = parse(location.search, location, true);
        let OrderConfirmationFormDataViewModel: OrderConfirmationFormDataViewModel = {
            transactionId: this.props.match.params.transactionAltId,
            confirmationFromMyOrders: false
        };
        if (data.query.confirmation_from_orders && data.query.confirmation_from_orders == "true") {
            OrderConfirmationFormDataViewModel: OrderConfirmationFormDataViewModel = {
                transactionId: this.props.match.params.transactionAltId,
                confirmationFromMyOrders: true
            };
        }
        this.props.requestConfirmationData(OrderConfirmationFormDataViewModel);
        localStorage.removeItem('cartItems');
        localStorage.removeItem('currentCartItems');
        PubSub.publish("UPDATE_NAV_CART_DATA_EVENT", 1);
        PubSub.publish("UPDATE_CART_COUNT_EVENT", 1);
    }

    render() {
        let isLiveOnlineEvent = false, eventAltId, hideDayClass = "", isTicket = false;
        if (this.props.fetchOrderConfirmationSuccess) {
            var orderConfirmationData = this.props.orderconfirmations;
            //check if it is live online event
            if (orderConfirmationData.orderConfirmationSubContainer.length > 0) {
                if (orderConfirmationData.orderConfirmationSubContainer[0].subEventContainer.length > 0) {
                    if (orderConfirmationData.orderConfirmationSubContainer[0].subEventContainer[0].event.masterEventTypeId == MasterEventTypes.Online) {
                        isLiveOnlineEvent = true;
                        orderConfirmationData.orderConfirmationSubContainer.map(
                            (item, index) => {
                                let orderSubContainer = item.subEventContainer.map((val, currentIndex) => {
                                    let ticketCats = val.ticketCategory.filter((currentTicket) => { return (currentTicket.id != 19452 && currentTicket.id != 12259) });
                                    let ticket = ticketCats.map((currentTicketCat, currentTicketIndex) => { isTicket = true });
                                })
                            })
                        eventAltId = orderConfirmationData.orderConfirmationSubContainer[0].subEventContainer[0].event.altId;
                        hideDayClass = "before-none";
                    }
                }
            }
            var marks =
                orderConfirmationData &&
                orderConfirmationData.orderConfirmationSubContainer.map(item => ({
                    lat: parseInt(item.subEventContainer[0].venue.latitude || 0),
                    lng: parseInt(item.subEventContainer[0].venue.longitude || 0)
                }));

            return (
                <div className="confirmPageNew">
                    <EcommercePurchaseGtm orderConfirmationData={orderConfirmationData} />
                    <div className="inner-banner">
                        {!isLiveOnlineEvent ?
                            <StyledMap marks={marks} />
                            : <img src={`${gets3BaseUrl()}/places/InnerBanner/` + eventAltId.toUpperCase() + `.jpg`} alt="" className="card-img" />
                        }
                    </div>
                    <div className="container pt-5 pb-4">
                        <div className="row topSecConfirm pb-5">
                            <div className="col-sm-7">
                                <h1 className="m-0 text-purple">
                                    {`Thank You ${orderConfirmationData.transaction.firstName} ${orderConfirmationData.transaction.lastName}`}
                                </h1>
                                <div className="mb-3">
                                    <h4 className="m-0">
                                        Your Order Is Confirmed <br />
                                        <span className="font-weight-normal">
                                            Confirmation ID:
                      {` FAP ${orderConfirmationData.transaction.id * 6 +
                                                "0019" +
                                                orderConfirmationData.transaction.id}`}
                                        </span>
                                    </h4>
                                </div>
                                <div>
                                <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/mail.svg" width="14" className="mr-2" alt=""/>
                                    Emai ID: {orderConfirmationData.transaction.emailId}
                                </div>
                                {(orderConfirmationData.transaction.phoneNumber && orderConfirmationData.transaction.phoneNumber != "") && <div>
                                <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/phone-icon.svg" width="14" className="mr-2" alt=""/>
                                    Mobile No:
                  {` +${orderConfirmationData.transaction.phoneCode}-${orderConfirmationData.transaction.phoneNumber}`}
                                </div>}
                            </div>
                            <div className="col-sm-5 confirm-qr-box text-center">
                                {!isLiveOnlineEvent &&
                                    <>
                                        <img
                                            src={orderConfirmationData.transactionQrcode}
                                            className="img-fluid confirm-qr"
                                            alt="Bar Code"
                                        />
                                        <p>Scan the QR Code to Redeem the Ticket</p>
                                    </>}
                                <div>
                                    <div className="font-weight-bold">Booking Date & Time:</div>
                                    {`${Moment(
                                        orderConfirmationData.transaction.createdUtc
                                    ).format("ddd, MMM DD, YYYY")} ${Moment(
                                        orderConfirmationData.transaction.createdUtc
                                    ).format("h:mm A")}`}
                                </div>
                                {orderConfirmationData && orderConfirmationData.orderConfirmationSubContainer[0].event.id == 15645 && <div className="mt-2 py-2 bg-light rounded">
                                    <div className="font-weight-bold text-purple">YAKOV SOCIAL MEDIA</div>
                                    <a href="https://twitter.com/Yakov_Smirnoff" target="_blank" className="mr-2"> <img src="https://static6.feelitlive.com/images/fil-images/social/twitter.svg" alt="FIL Twitter" width="20" /></a>
                                    <a href="https://m.facebook.com/Yakov.Smirnoff.Comedian/" target="_blank" className="mr-2"> <img src="https://static6.feelitlive.com/images/fil-images/social/facebook.svg" alt="FIL Facebook" width="20" /></a>
                                    <a href="https://www.instagram.com/yakov_smirnoff/" target="_blank" > <img src="https://static6.feelitlive.com/images/fil-images/social/instagram.svg" alt="FIL Instagram" width="20" /></a>
                                </div>}
                            </div>
                            {(isLiveOnlineEvent) && <div className="col-12 pt-3"><b>Your online streaming link is included below.</b> You can use this to view your event. We have also emailed the link to you, but <b>please do not close this page without first confirming that you have received your email with the streaming link.</b> If you do not see the email in your inbox, please check all the email tabs such as Updates, Forums, Promotions etc, and/or your spam/junk/bulk or folder and mark the email as not spam so as to receive our emails in your inbox.</div>}
                        </div>

                        {/* <h4>7 Days In India, 18th Nov 2019 to 24 Nov 2019</h4> */}
                        <div className="itinerary-places pb-4">
                            {orderConfirmationData &&
                                orderConfirmationData.orderConfirmationSubContainer.map(
                                    (item, index) => {
                                        let orderSubContainer = item.subEventContainer.map((val, currentIndex) => {
                                            let ticketCats = val.ticketCategory.filter((currentTicket) => { return (currentTicket.id != 19452 && currentTicket.id != 12259) });
                                            let ticket = ticketCats.map((currentTicketCat, currentTicketIndex) => {
                                                let etd = val.eventTicketDetail.filter((currentETD) => { return currentETD.ticketCategoryId == currentTicketCat.id })[0];
                                                let eta = val.eventTicketAttribute.filter((currentETA) => { return currentETA.eventTicketDetailId == etd.id })[0];
                                                let td = val.transactionDetail.filter((transactionDetail) => { return transactionDetail.eventTicketAttributeId == eta.id })[0];
                                                return <div className={`card shadow-sm mt-4 mb-5 ${hideDayClass}`} key={currentTicketIndex} >
                                                    <div className="row no-gutters">
                                                        <div className="col-sm-4">
                                                            <img
                                                                src={td.transactionType != 8 ? `${gets3BaseUrl()}/places/tiles/${item.event.altId.toUpperCase()}-ht-c1.jpg` : `${gets3BaseUrl()}/add-ons/${etd.altId ? etd.altId.toUpperCase() : ''}-add-ons.jpg`}
                                                                className="card-img"
                                                                alt=""
                                                                onError={e => {
                                                                    e.currentTarget.src = 'https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/add-ons/addons-placeholder.jpg';
                                                                }}
                                                            />
                                                        </div>
                                                        <div className="col-sm-8">
                                                            <div className="card-body">
                                                                <h5 className="card-title m-0">
                                                                    {item.event.name}
                                                                    {!isLiveOnlineEvent && <span className="text-muted ml-2">
                                                                        (
                                                                         {val.transactionDetail[0].visitDate ==
                                                                            val.transactionDetail[0]
                                                                                .visitEndDate
                                                                            ? `${Moment(
                                                                                val
                                                                                    .transactionDetail[0].visitDate
                                                                            ).format("DD MMM, h:mm a")}`
                                                                            : `${Moment(
                                                                                val
                                                                                    .transactionDetail[0].visitDate
                                                                            ).format("DD MMM, h:mm a")} - ${Moment(
                                                                                val
                                                                                    .transactionDetail[0].visitEndDate
                                                                            ).format("DD MMM, h:mm a")}`}
                                                                    )
                                                                        </span>}
                                                                </h5>
                                                                <div>
                                                                    {val.eventCategory && <span className="badge badge-primary mr-3">
                                                                        {td.transactionType == 8 ? "Add-Ons" : val.eventCategory.displayName}
                                                                    </span>}
                                                                    {!isLiveOnlineEvent && <img
                                                                        src={`${gets3BaseUrl()}/header/cart-icon-fill-v1.png`}
                                                                        alt="Feel Cart Icon"
                                                                        width="18"
                                                                    />}
                                                                </div>
                                                                {!isLiveOnlineEvent &&
                                                                    <div>
                                                                        <b>Venue Pickup:</b>
                                                                        {` ${val.venue.addressLineOne}, ${val.city.name},
                                 ${val.state.name}, ${val.country.name}`}
                                                                    </div>
                                                                }
                                                                <div className="font-weight-bold">
                                                                    {currentTicketCat.name}
                                                                    {` (${td.totalTickets} x
                                ${orderConfirmationData.currencyType.code} ${numeral(td.pricePerTicket).format("00.00")})`}
                                                                </div>
                                                                {(isLiveOnlineEvent && isTicket) && <div className="py-3"><a target="_blank" href={this.props.orderconfirmations.streamLink} className="btn site-primery-btn px-5 text-white">Online Stream Link</a></div>}
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            });
                                            return ticket;
                                        })
                                        return orderSubContainer;
                                    }
                                )}
                        </div>

                        <div className="row">
                            <div className="col-sm-8">
                            </div>
                            <div className="col-sm-4">
                                <h5>Order Summary</h5>
                                <div>
                                    Ticket Amount:{" "}
                                    <span className="pull-right">{`${
                                        orderConfirmationData.currencyType.code
                                        } ${numeral(
                                            orderConfirmationData.transaction.grossTicketAmount - ((orderConfirmationData.transaction.donationAmount && orderConfirmationData.transaction.donationAmount != null && orderConfirmationData.transaction.donationAmount > 0) ? orderConfirmationData.transaction.donationAmount : 0)
                                        ).format("00.00")}`}</span>
                                </div>
                                <div>
                                    Booking Fee:
                  <span className="pull-right">
                                        {`${orderConfirmationData.currencyType.code} ${numeral(
                                            orderConfirmationData.transaction.convenienceCharges +
                                            orderConfirmationData.transaction.serviceCharge
                                        ).format("00.00")}`}
                                    </span>
                                </div>

                                <div>
                                    Delivery Fee:
                  <span className="pull-right">
                                        {`${orderConfirmationData.currencyType.code} ${numeral(
                                            orderConfirmationData.transaction.deliveryCharges
                                        ).format("00.00")}`}
                                    </span>
                                </div>
                                {(orderConfirmationData.transaction.discountAmount != 0) && < div >
                                    Discount Amount:
                  <span className="pull-right">
                                        {`${orderConfirmationData.currencyType.code} ${numeral(
                                            orderConfirmationData.transaction.discountAmount
                                        ).format("00.00")}`}
                                    </span>
                                </div>}

                                {(orderConfirmationData.transaction.donationAmount != 0) && < div >
                                    Donation Amount:
                  <span className="pull-right">
                                        {`${orderConfirmationData.currencyType.code} ${numeral(
                                            orderConfirmationData.transaction.donationAmount
                                        ).format("00.00")}`}
                                    </span>
                                </div>}

                                <div>
                                    Total Amount Paid:
                  <span className="pull-right">{`${
                                        orderConfirmationData.currencyType.code
                                        } ${numeral(
                                            orderConfirmationData.transaction.netTicketAmount
                                        ).format("00.00")}`}</span>
                                </div>
                                <hr />
                                <h5 className="text-right">
                                    Payment Type <br />
                                    <small>
                                        {`${orderConfirmationData.paymentOption}`}
                                    </small>
                                </h5>
                            </div>
                        </div>
                    </div>
                </div>
            );
        } else {
            return <FilLoader />;
        }
    }
}

export default connect(
    (state: IApplicationState) => state.OrderConfirmation,
    OrderConfirmationStore.actionCreators
)(OrderConfirmation);
