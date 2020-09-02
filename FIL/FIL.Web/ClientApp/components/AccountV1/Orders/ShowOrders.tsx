import * as React from 'react';
import {
    Link, RouteComponentProps, Route, Switch
} from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import * as numeral from "numeral";
import Transaction from '../../../models/Comman/TransactionViewModel';

interface Iprops {
    gets3BaseUrl: string;
}
type AccountProps = Iprops & AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

function ShowOrders(props: AccountProps) {
    const [isUpcoming, setIsUpcoming] = React.useState(true);

    const getPlaceHolderImg = (eventId: number) => {
        let eventCategoryMappings = props.account.userOrders.eventCategoryMappings.filter((item) => {
            return item.eventId == eventId
        })
        let slug = "";
        if (eventCategoryMappings.length > 0) {
            let eventCategory = props.account.userOrders.eventCategories.filter((item) => {
                return item.eventCategoryId == eventCategoryMappings[0].eventCategoryId
            })
            if (eventCategory.length > 0) {
                slug = eventCategory[0].slug;
            } else {
                slug = "music"
            }
        } else {
            slug = "music"
        }
        return `https://feelitlive.imgix.net/images/places/tiles/${slug}-placeholder.jpg`;
    }
    const getOrderData = (transaction: Transaction[], isUpcoming: boolean) => {

        transaction.sort((a, b) => {
            return new Date(b.createdUtc).getTime() -
                new Date(a.createdUtc).getTime()
        })
        let filtredTransactionData = transaction.map((val, index) => {
            let symbol;
            if (val.currencyId == 7) {
                symbol = "₹";
            } else if (val.currencyId == 16) {
                symbol = "£";
            } else if (val.currencyId == 11) {
                symbol = "$";
            } else if (val.currencyId == 15) {
                symbol = "€";
            } else {
                symbol = "₹";
            }
            let tramsactionDetail = props.account.userOrders.transactionDetail.filter((item) => {
                return item.transactionId == val.id
            })
            let eventticketAttr = [], eventticketDetails = [], eventDetail = [], eventId = [];
            if (tramsactionDetail.length > 0) {
                eventticketAttr = props.account.userOrders.eventTicketAttribute.filter((item) => {
                    return item.id == tramsactionDetail[0].eventTicketAttributeId
                })
            }
            if (eventticketAttr.length > 0) {
                eventticketDetails = props.account.userOrders.eventTicketDetail.filter((item) => {
                    return item.id == eventticketAttr[0].eventTicketDetailId;
                })
            }
            if (eventticketDetails.length > 0) {
                eventDetail = props.account.userOrders.eventDetail.filter((item) => {
                    return item.id == eventticketDetails[0].eventDetailId
                })
            }
            if (eventDetail.length > 0) {
                eventId = props.account.userOrders.event.filter((item) => {
                    return item.id == eventDetail[0].eventId;
                });
            }
            if (eventId.length > 0) {
                return <>
                    <div className="media mt-4">
                        <img
                            className="mr-3"
                            src={`https://feelitlive.imgix.net/images/places/tiles/${eventId[0].altId.toLocaleUpperCase()}-ht-c1.jpg`}
                            onError={e => {
                                e.currentTarget.src = getPlaceHolderImg(eventId[0].id);
                            }}
                            alt={eventId[0].name}
                            width="150"
                            height="96"
                        />
                        <div className="media-body">
                            <div className="row">
                                <div className="col">
                                    <h5 className="mt-0">{eventId[0].name}</h5>
                                    <div>
                                        Date and time of booking: {new Date(val.createdUtc)
                                            .toDateString()
                                            .split(" ")
                                            .join(" ")}{" "}
                                        {numeral(new Date(val.createdUtc).getHours()).format(
                                            "00"
                                        ) +
                                            ":" +
                                            numeral(new Date(val.createdUtc).getMinutes()).format(
                                                "00"
                                            )}
                                    </div>
                                    <div>Transaction Id: {val.id}</div>
                                    <div>Total Amount: {symbol}{numeral(val.netTicketAmount).format("00.00")}
                                    </div>
                                    <div className="mt-3">
                                        <Link
                                            to={`/order-confirmation/${val.altId}?confirmation_from_orders=true`}
                                            className="btn px-3 rounded-0 btn-primary"
                                        >
                                            View Details
                                </Link>
                                    </div>
                                </div>
                                {/* <div className="col" style={{ maxWidth: "200px" }}>
                                <button
                                    type="button"
                                    className="btn rounded-0 btn-outline-primary btn-block"
                                    disabled={true}
                                >
                                    Write a review
                        </button>
                                {isUpcoming &&
                                    <button
                                        type="button"
                                        className="btn my-3 rounded-0 btn-outline-primary btn-block"
                                        disabled={true}
                                    >
                                        Track Shipment
                        </button>
                                }
                                <button
                                    type="button"
                                    className="btn rounded-0 btn-outline-primary btn-block"
                                >
                                    View invoice
                        </button>
                            </div> */}
                            </div>
                        </div>
                    </div>
                    {index < props.account.userOrders.transaction.length - 1 && <hr />}
                </>
            }
        });
        if (filtredTransactionData && filtredTransactionData.length > 0) {
            filtredTransactionData = filtredTransactionData.filter((item) => {
                return item != undefined
            })
        }
        return filtredTransactionData;
    }

    let filteredPastEventDetails = props.account.userOrders.eventDetail.filter((item) => {
        return item.startDateTime > props.account.userOrders.currentDateTime
    })
    let filtredPastEventticketDetails = props.account.userOrders.eventTicketDetail.filter((item) => {
        let currentEventDetail = filteredPastEventDetails.filter((val) => {
            return item.eventDetailId == val.id
        });
        if (currentEventDetail.length > 0) {
            return item;
        }
    });
    let filtredPastEventTicketAttributes = props.account.userOrders.eventTicketAttribute.filter((item) => {
        let currentEventTicketDetail = filtredPastEventticketDetails.filter((val) => {
            return item.eventTicketDetailId == val.id
        });
        if (currentEventTicketDetail.length > 0) {
            return item;
        }
    })
    let filtredPastTransactionDetails = props.account.userOrders.transactionDetail.filter((item) => {
        let currentEventTicketAttr = filtredPastEventTicketAttributes.filter((val) => {
            return item.eventTicketAttributeId == val.id
        });
        if (currentEventTicketAttr.length > 0) {
            return item;
        }
    })
    let filtredUpcomingTransactions = props.account.userOrders.transaction;
    filtredUpcomingTransactions = filtredUpcomingTransactions.filter((item) => {
        let currentTransactionDetailId = filtredPastTransactionDetails.filter((val) => {
            return item.id == val.transactionId
        });
        if (currentTransactionDetailId.length > 0) {
            return item;
        }
    });
    let filtredPastTransactions = props.account.userOrders.transaction;

    filtredPastTransactions = filtredPastTransactions.filter((item) => {
        let currentTransaction = filtredUpcomingTransactions.filter((val) => {
            return item.id == val.id
        });
        if (currentTransaction.length == 0) {
            return item;
        }
    });

    return <>
        <div className="row">
            <div className="col-sm-8">
                <ul
                    className="nav nav-tabs border-bottom text-uppercase"
                    id="my-acc-tab"
                    role="tablist"
                >
                    <li className="nav-item" role="presentation" onClick={() => setIsUpcoming(true)}>
                        <a
                            className="nav-link active"
                            id="upcoming"
                            data-toggle="pill"
                            href="#pills-home"
                            role="tab"
                            aria-controls="pills-home"
                            aria-selected="true"
                        >UPCOMING</a>
                    </li>
                    <li className="nav-item" role="presentation" onClick={() => setIsUpcoming(false)}>
                        <a
                            className="nav-link"
                            id="past"
                            data-toggle="pill"
                            href="#pills-profile"
                            role="tab"
                            aria-controls="pills-profile"
                            aria-selected="false"
                        >PAST</a>
                    </li>
                </ul>
                <div className="tab-content orders-tab" id="my-acc-tabContent">
                    <div
                        className="tab-pane fade show active"
                        id="pills-home"
                        role="tabpanel"
                        aria-labelledby="upcoming">

                        {getOrderData(filtredUpcomingTransactions, true).length > 0 ? getOrderData(filtredUpcomingTransactions, true) :
                            <h5 className="text-center mt-5">You have no upcoming orders</h5>}
                    </div>
                    <div
                        className="tab-pane fade"
                        id="pills-profile"
                        role="tabpanel"
                        aria-labelledby="past"
                    >
                        {getOrderData(filtredPastTransactions, false).length > 0 ? getOrderData(filtredPastTransactions, false) :
                            <h5 className="text-center mt-5">You have no orders in past</h5>}
                    </div>
                </div>
            </div>
            <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
                <div className="border p-4">
                    <img
                        src={`${props.gets3BaseUrl}/my-account/right-bar-images/orders-${isUpcoming ? 'upcoming' : 'past'}.svg`}
                        className="img-fluid mb-4"
                        alt=""
                    />
                    <div>
                        <h5 className="mb-3">{isUpcoming ? 'Your upcoming orders' : 'Your past orders'}</h5>
                        <p className="m-0">
                            {isUpcoming ? 'In here you can view the details and track shipment -if applicable - of all your upcoming experiences/orders.'
                                :
                                'In here you can find all the details of all your past orders.'
                            }
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </>
}
export default ShowOrders;