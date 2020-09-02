import * as React from "react";
import { CartDataViewModel, CartItem, GuestData } from "../../models/Cart/CartDataViewModel";
import { getTimeSlot, } from "../../utils/TicketCategory/TimeSlotProvider";
import {
    checkIsTiqetCategoryValid,
    getTicketCategoryRemoveValidationModel,
    checkIsLiveOnlineCategoryValid,
    isDonationExists,
    getOnlyTickets,
    getOnlyAddOns,
    getOnlyDonation
} from "../../utils/TicketCategory/TiqetsCategoryValidationProvider";
import { checkIsValidItinerary } from "../../utils/TicketCategory/ItineraryProvider";
import { getDisabledDates, getActiveDate } from "../../utils/TicketCategory/DateProvider";
import { GetCartData } from "../../utils/TicketCategory/CartDataStateProvider";
import { getContent } from "./Tiqets/ContentProvider";
import * as numeral from "numeral";
import * as PubSub from 'pubsub-js';
import * as ReactTooltip from 'react-tooltip';
import "./TicketCategoryDateSelection.scss";
import { ReactHTMLConverter } from "react-html-converter/browser";
import { autobind } from "core-decorators";
import * as getSymbolFromCurrency from 'currency-symbol-map';
import { gets3BaseUrl } from "../../utils/imageCdn";
import * as moment from 'moment';
import { Calendar } from 'react-date-range';
import 'react-date-range/dist/styles.css'; // main style file
import 'react-date-range/dist/theme/default.css'; // theme css file
import { polyfill } from 'react-lifecycles-compat';
import { MasterEventTypes } from "../../Enum/MasterEventTypes";

let isTiqetsEvent = false, hasTiqetsTimeSlots = false, hasHohoTimeSlots = false, isHohoPlace = false;
let isAttendEvent = false;

export enum Display {
    Tickets = 1,
    AddOn,
    Donate
}
class TicketCategorySelection extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            selectedDate: null,
            timeModel: [],
            cartData: [],
            isAddOnAdded: false,
            isAddOns: false,
            addOnId: 0,
            className: "",
            selectedTimeIndex: -1,
            currentCountry: null,
            isFirstTimeClick: true,
            isTimeTypeExists: false,
            selectedDateTimeModel: [],
            selectedTimeSlot: "",
            validTicketCatError: '',
            isTiqetsEvent: false,
            isHohoPlace: false,
            currentDisplay: Display.Tickets,
            s3BaseUrl: gets3BaseUrl()
        }
    }

    public componentDidMount() {
        if (this.props.ticketCategoryData.event.masterEventTypeId == MasterEventTypes.Online) {
            isAttendEvent = true;
            this.setState({
                showTicketType: true
            });
        }
        var ticketCategoryDataModel = GetCartData(this.props);
        if (this.props.isDonate) {
            ticketCategoryDataModel.cartItems = getOnlyDonation(ticketCategoryDataModel.cartItems);
        }
        this.loadCartItems(ticketCategoryDataModel.cartItems);
        ticketCategoryDataModel.cartItems = ticketCategoryDataModel.cartItems.sort((a, b) => Number(a.pricePerTicket) - Number(b.pricePerTicket));

        this.setState({
            cartData: ticketCategoryDataModel.cartItems,
            isTiqetsEvent: this.props.ticketCategoryData.event.eventSourceId == 6 ? true : false,
            isHohoPlace: this.props.ticketCategoryData.event.eventSourceId == 3 ? true : false,
            currentDisplay: this.props.isDonate ? Display.Donate : Display.Tickets,
            className: ""
        });
    }

    public toggleCalender(e) {
        this.setState({ calenderVisible: (!this.state.calenderVisible) });
    }

    showTime = newDate => {
        let date = newDate && newDate._d ? newDate._d : newDate;
        var timeArray = [];
        var currentDate = date;
        var data = this.state.cartData;
        let timSlotModel = getTimeSlot(date, this.props.ticketCategoryData.eventDetail[0], this.props.ticketCategoryData); // Get Timeslot Model
        var startDate = new Date(this.props.ticketCategoryData.eventDetail[0].startDateTime);
        currentDate = new Date(date);
        currentDate.setHours(startDate.getHours(), startDate.getMinutes(), 0);
        data.forEach(function (item) {
            var itemData = item;
            itemData.isTimeSelection = false,
                itemData.selectedDate = currentDate.toString()
        });
        this.setState({
            cartData: data,
            selectedTime: 0,
            selectedDateTimeModel: timSlotModel.timeSlot,
            timeModel: timeArray,
            selectedDate: date,
            calenderVisible: true,
            dateSelected: true,
            showTicketType: timSlotModel.isTimeTypeExists ? false : true,
            isTimeTypeExists: timSlotModel.isTimeTypeExists,
            selectedTimeIndex: -1
        });
        // Checking if requested event is Tiqets 
        if (this.props.ticketCategoryData.event.eventSourceId == 6) {
            isTiqetsEvent = true;
            this.setState({
                showTicketType: this.props.ticketCategoryData.tiqetsCheckoutDetails.hasTimeSlot ? false : true
            });
            if (this.props.ticketCategoryData.tiqetsCheckoutDetails.hasTimeSlot) {
                hasTiqetsTimeSlots = true;
                var d = new Date(date),
                    month = '' + (d.getMonth() + 1),
                    day = '' + d.getDate(),
                    year = d.getFullYear();

                if (month.length < 2)
                    month = '0' + month;
                if (day.length < 2)
                    day = '0' + day;
                this.props.onChangeDate(date);
                this.props.getTiqetsTimeSlots(this.props.ticketCategoryData.tiqetsCheckoutDetails.productId, [year, month, day].join('-'));
            }
        }
        //check if place is Hoho one
        if (this.props.ticketCategoryData.event.eventSourceId == 3) {
            isHohoPlace = true;
            if (this.props.ticketCategoryData.citySightSeeingTicketDetail.ticketClass != 1) {
                hasHohoTimeSlots = true;
                var d = new Date(date),
                    month = '' + (d.getMonth() + 1),
                    day = '' + d.getDate(),
                    year = d.getFullYear();

                if (month.length < 2)
                    month = '0' + month;
                if (day.length < 2)
                    day = '0' + day;
                this.props.onChangeDate(date);
                this.props.getHohoTimeSlots(this.props.ticketCategoryData.citySightSeeingTicketDetail.ticketId, [year, month, day].join('-'));
            }
        }
    }

    public loadCartItems(data) {
        var that = this;
        data.map(function (val) {
            var eventTicketAttributeId = val.eventTicketAttributeId;
            var quantity = val.quantity;
            that.setState({
                [eventTicketAttributeId]: quantity
            })
        });
    }

    addTicket = (e, item, isContinue = false) => {
        let tiqetsValidationModel = checkIsTiqetCategoryValid(this.props.ticketCategoryData, item); // this checks if this ticketCategory can be bought alone
        //Checkfor Live Online Category
        if (item.masterEventTypeId == MasterEventTypes.Online && item.ticketCategoryTypeId != 2) {
            if (!this.state.isItemAddedInCart) {
                localStorage.removeItem("cartItems");
                localStorage.removeItem("currentCartItems");
                this.setState({ isItemAddedInCart: true });
            }
            tiqetsValidationModel = checkIsLiveOnlineCategoryValid(item.altId);
        }
        if (tiqetsValidationModel.catError != "") {
            this.setState({ validTicketCatError: tiqetsValidationModel.catError });
        }
        if ((this.state[item.eventTicketAttributeId] < 10 && tiqetsValidationModel.isValid) || isContinue) {
            var cartData = this.state.cartData;
            for (var i = 0; i < cartData.length; i++) {
                if (cartData[i].eventTicketAttributeId == item.eventTicketAttributeId) {
                    cartData[i].quantity = ++(item.quantity);
                    var newGuest: GuestData = {
                        guestId: cartData[i].guestList.length + 1,
                        firstName: "",
                        lastName: "",
                        nationality: "",
                        documentTypeId: "",
                        documentNumber: "",
                    };
                    cartData[i].isTiqetsPlace = this.state.isTiqetsEvent;
                    cartData[i].guestList.push(newGuest);
                    cartData[i].timeSlot = this.state.selectedTimeSlot;
                    cartData[i].timeStamp = new Date();
                    cartData[i].isHohoPlace = this.state.isHohoPlace;
                    var cartItems = cartData.filter(function (val) {
                        return val.quantity > 0
                    });
                    localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
                    this.loadCartItems(cartData);
                    break;
                }
            }
            this.setState({ validTicketCatError: '' }, () => {
                if (isContinue) {
                    this.continue();
                }
            })
        }
    }

    removeTicket = (e, item) => {
        let tiqetsValidationModel = getTicketCategoryRemoveValidationModel(this.props.ticketCategoryData, item, this.state.isTiqetsEvent);
        if (this.state[item.eventTicketAttributeId] > 0 && tiqetsValidationModel.isValid) {
            var cartData = this.state.cartData;
            for (var i = 0; i < cartData.length; i++) {
                if (cartData[i].eventTicketAttributeId == item.eventTicketAttributeId) {
                    cartData[i].quantity = --(item.quantity);
                    var cartItems = cartData.filter(function (val) {
                        return val.quantity > 0
                    });
                    localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
                    this.loadCartItems(cartData);
                    break;
                }
            }
            this.setState({ validTicketCatError: '' })
        }
    }

    checkSkirtingTheBoundaryDiscount = (cartItems) => {
        let isAddons = cartItems.filter((val) => { return val.ticketCategoryTypeId == 2 });
        if (isAddons.length > 0) {
            let bsp = cartItems.filter((val) => { return val.ticketCategoryId == 19350 });
            if (bsp.length == 0) {
                cartItems = cartItems.map((val) => {
                    if (val.discountAmount) {
                        val.discountAmount = 0;
                    }
                    return val;
                });
                return cartItems;
            }
            return cartItems;
        } else {
            cartItems = cartItems.map((val) => {
                if (val.discountAmount) {
                    val.discountAmount = 0;
                }
                return val;
            });
            return cartItems;
        }
    }

    continue = () => {
        var cartItems = this.state.cartData.filter(function (val) {
            return val.quantity > 0
        });
        if (cartItems.length > 0 && cartItems[0].altId == "FB59C733-A430-4C64-B5D8-E70D4ACAA131") {
            cartItems = this.checkSkirtingTheBoundaryDiscount(cartItems);
        }
        localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
        let isValidItinerary = checkIsValidItinerary(this.props.ticketCategoryData, this.state, hasHohoTimeSlots, isAttendEvent, this.props.selectedDate);
        if (isValidItinerary) {
            PubSub.publish('UPDATE_NAV_CART_DATA_EVENT', 1);
            this.props.goToItinerary();
        }
    }

    public continueToItinerary() {
        if (this.state.donationAmount && this.state.donationAmount > 0) { // if donation state is set then add donation category in the cart
            this.addTicket(this, getOnlyDonation(this.state.cartData)[0], true)
        } else {
            this.continue();
        }
    }

    public onTimeClick(model, e) {
        var fromTime = model.fromTime.split(":");
        var cartData = this.state.cartData.map(function (item) {
            var date = new Date(item.selectedDate);
            date.setHours(+fromTime[0], +fromTime[1], 0);
            item.placeVisitTime = model.fromTime + " - " + model.toTime;
            item.selectedDate = date.toString();
            return item
        });
        this.setState({ cartData: cartData, showTicketType: true, selectedTimeIndex: model.index });
    }

    @autobind
    public onClickAddOns(id) {
        this.setState({ isAddOns: true, addOnId: id });
    }

    @autobind
    public onClickTicketCategory() {
        this.setState({ isAddOns: false, isShowDonate: false });
    }

    changeTimeSlot(e) {
        this.setState({ selectedTimeSlot: e.target.value, showTicketType: true })
    }

    onChangeDonateAmount = (amount, isContinueToItinerary = false) => {
        var data = this.state.cartData;
        data.forEach(function (item) {
            var itemData = item;
            itemData.donationAmount = +amount
        });
        this.setState({ cartData: data, isDonationSelected: true }, () => {
            if (isContinueToItinerary) {
                this.continueToItinerary();
            }
        });
    }

    public render() {
        var that = this, isLiveOnlineEvent = false;
        var data = this.state.cartData;
        var Quantity = 0, ticketCount = 0;
        var i = 0;
        var timing;
        const converter = ReactHTMLConverter();
        converter.registerComponent('Ticket', TicketCategorySelection);
        let disabledDates = getDisabledDates(this.props);
        if (this.state.dateSelected) {
            timing = this.state.selectedDateTimeModel.map(function (item) {
                var className = "badge badge-secondary mr-1";
                if (item.index == that.state.selectedTimeIndex) {
                    className = "badge badge-primary mr-1";
                }
                return <a href="javascript:void(0)" className={className} onClick={that.onTimeClick.bind(that, item)}>{item.fromTime} - {item.toTime}</a>

            })
        }
        //Check if it's live online event
        if (this.props.ticketCategoryData.event.masterEventTypeId == MasterEventTypes.Online) {
            isLiveOnlineEvent = true;
        }
        var ticketCategory;
        data.filter(function (val) {
            if (val.ticketCategoryTypeId == 1 || val.ticketCategoryTypeId == -1) {
                return val
            }
        }).map((val) => { var ticketQuantity = that.state[val.eventTicketAttributeId]; ticketCount += ticketQuantity; });
        if (this.state.currentDisplay == Display.Tickets) {
            data = getOnlyTickets(data);
            ticketCategory = data.map(function (val) {
                var category = val.eventTicketAttributeId;
                i = i + 1;
                var ticketQuantity = that.state[category];
                Quantity += ticketQuantity;
                var symbol = getSymbolFromCurrency(val.currencyName);
                let currentETA = that.props.ticketCategoryData.eventTicketAttribute.filter((eta) => {
                    return eta.id == val.eventTicketAttributeId
                })
                let isSoldOut = false;
                if (currentETA) {
                    if (currentETA[0].remainingTicketForSale == 0) {
                        isSoldOut = true;
                    }
                }
                return <div>
                    <hr />
                    <div className="row">
                        <div className="col-sm-6 col-lg-7 text-sm-left text-center pb-2 pb-sm-0">
                            <span className="h6 mt-2">{val.ticketCategoryName}
                                {(converter.convert(val.ticketCategoryDescription) != "" && converter.convert(val.ticketCategoryDescription) != null) && <span>
                                    <i data-tip={"React-tooltip" + i} data-for={val.ticketCategoryName + i} className="fa fa-info-circle text-primary ml-2" />
                                    <ReactTooltip place="bottom" id={val.ticketCategoryName + i} type="info" effect="float">
                                        <span style={{ maxWidth: "230px", display: "block" }}>{converter.convert(val.ticketCategoryDescription)}</span>
                                    </ReactTooltip>
                                </span>}
                            </span>
                            {(converter.convert(val.additionalInfo) != "" && converter.convert(val.additionalInfo) != null) && <span><p>
                                ({converter.convert(val.additionalInfo.replace("<currency>", symbol))})
                            </p></span>}
                        </div>
                        <div className="col-sm-6 col-lg-5 text-sm-right text-center pb-2 pb-sm-0">
                            {(val.specialPrice > 0) && <span className="mr-20 text-danger"><del>{symbol + numeral(val.specialPrice).format('0.00') + ' ' + val.currencyName}</del></span>}
                            <span className="mt-2 pr-3 h5">{symbol + numeral(val.pricePerTicket).format('0.00') + ' ' + val.currencyName}</span>
                            {!isSoldOut ? <> <div className="input-group pull-right" style={{ maxWidth: "150px" }}>
                                <div className="input-group-prepend">
                                    <button className="btn btn-outline-secondary border" type="button" onClick={() => that.removeTicket(that, val)}>-</button>
                                </div>
                                <input type="text" className="form-control text-center border-left-0 border-right-0 bg-white" placeholder={ticketQuantity} readOnly={true} />
                                <div className="input-group-append">
                                    <button className="btn btn-outline-secondary border" type="button" onClick={() => that.addTicket(that, val)}>+</button>
                                </div>
                            </div></> : <span className="text-danger">Sold out</span>}
                        </div>
                    </div>
                </div >;
            });
        } else if (this.state.currentDisplay == Display.AddOn) {
            if (this.state.currentDisplay == Display.AddOn) {
                data = getOnlyAddOns(data);
                ticketCategory = data.map(function (item) {
                    var category = item.eventTicketAttributeId;
                    i = i + 1;
                    var ticketQuantity = that.state[category];
                    Quantity += ticketQuantity;
                    var symbol = getSymbolFromCurrency(item.currencyName);
                    let isSoldOut = false;
                    let currentETA = that.props.ticketCategoryData.eventTicketAttribute.filter((eta) => {
                        return eta.id == item.eventTicketAttributeId
                    })
                    if (currentETA) {
                        if (currentETA[0].remainingTicketForSale == 0) {
                            isSoldOut = true;
                        }
                    }
                    return <div className="col-sm-3 text-center">
                        <div className="card">
                            <img src={`${that.state.s3BaseUrl}/add-ons/${item.eventTicketDetail ? item.eventTicketDetail.toUpperCase() : ""}-add-ons.jpg`} className="card-img-top"
                                onError={e => {
                                    e.currentTarget.src = 'https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/add-ons/addons-placeholder.jpg';
                                }} />
                            <div className="card-body">
                                <p className="card-title" style={{ fontSize: "16px" }} >{item.ticketCategoryName}</p>
                                {(item.specialPrice > 0) && <span className="mr-20 text-danger"><del>{symbol + numeral(item.specialPrice).format('0.00') + ' ' + item.currencyName}</del></span>}
                                <h4 className="text-purple">{symbol + numeral(item.pricePerTicket).format('0.00') + ' ' + item.currencyName}</h4>
                                {!isSoldOut ? <div className="input-group m-auto" style={{ maxWidth: "150px" }}>
                                    <div className="input-group-prepend">
                                        <button className="btn btn-outline-secondary border" type="button" onClick={() => that.removeTicket(that, item)}>-</button>
                                    </div>
                                    <input type="text" className="form-control text-center border-left-0 border-right-0 bg-white" placeholder={ticketQuantity} readOnly={true} />
                                    <div className="input-group-append">
                                        <button className="btn btn-outline-secondary border" type="button" onClick={() => that.addTicket(that, item)}>+</button>
                                    </div>
                                </div> : <span className="text-danger">Sold out</span>}
                            </div>
                        </div>
                    </div>
                });
            }
        } else if (this.state.currentDisplay == Display.Donate) {
            var symbol = getSymbolFromCurrency(this.props.ticketCategoryData.currencyType.code);
            ticketCategory = <div className="col-sm-5 text-center">
                <div className="card">
                    <div className="card-body">
                        <div className="row m-0">
                            <div className="col-sm-6 mb-2 mb-sm-0 p-0">
                                <button type="button" onClick={(e) => {
                                    this.setState({ btnId: 1, donationAmount: "25" }, () => {
                                        this.onChangeDonateAmount(25, false);
                                    })
                                }} className={`btn ${this.state.btnId == 1 ? 'btn-primary' : 'btn-outline-secondary'}`}>{symbol}25</button>
                                <button type="button" onClick={(e) => {
                                    this.setState({ btnId: 2, donationAmount: "50" }, () => {
                                        this.onChangeDonateAmount(50, false);
                                    })
                                }} className={`btn mx-2 ${this.state.btnId == 2 ? 'btn-primary' : 'btn-outline-secondary'}`}>{symbol}50</button>
                                <button type="button" onClick={(e) => {
                                    this.setState({ btnId: 3, donationAmount: "100" }, () => {
                                        this.onChangeDonateAmount(100, false);
                                    })
                                }} className={`btn ${this.state.btnId == 3 ? 'btn-primary' : 'btn-outline-secondary'}`}>{symbol}100</button>
                                <span className="d-none d-sm-inline-block px-3">|</span>
                            </div>
                            <div className="col-sm-6 p-0">
                                <div className="input-group">
                                    <div className="input-group-prepend">
                                        <span className="input-group-text">{symbol}</span>
                                    </div>
                                    <input
                                        value={this.state.donationAmount}
                                        type="number"
                                        placeholder="Enter amount"
                                        className="form-control"
                                        onChange={(e) => {
                                            let amountArray = e.target.value.split(".");
                                            let amount = amountArray.length > 0 ? amountArray[0] : e.target.value;
                                            this.setState({ donationAmount: amount, btnId: 4 }, () => {
                                                this.onChangeDonateAmount(this.state.donationAmount, false)
                                            });
                                        }} aria-label="Enter amount" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        }
        if (this.props.ticketCategoryData.eventDetail.length == 0) {
            return <div></div>;
        }
        let maxDate = this.props.ticketCategoryData.eventDetail[0];
        var etdTicketCategoryMappings = this.props.ticketCategoryData.eventTicketDetailTicketCategoryTypeMappings.filter(function (item) {
            return item.ticketCategoryTypeId == 2
        });
        var ticketCatSubType = etdTicketCategoryMappings.map(function (item) { return item.ticketCategorySubTypeId });
        ticketCatSubType = ticketCatSubType.filter(function (value, index, self) {
            return self.indexOf(value) === index;
        });
        var addOns = ticketCatSubType.map(function (item) {
            var subType = that.props.ticketCategoryData.ticketCategorySubTypes.filter(function (val) {
                return val.id == item
            });
            return <li className="nav-item">
                <a className={that.state.currentDisplay == Display.AddOn ? "nav-link active" : "nav-link"} href="javascript:void(0)" onClick={() => { that.setState({ currentDisplay: Display.AddOn }, () => { that.onClickAddOns(subType[0].id) }) }}>Add-ons</a>
            </li>
        })
        // Tiqets Time Slots goes here ...
        let timeSlots = [];
        if (this.props.fetchTiqetsTimeSlots && this.props.tiqetsTimeSlots.timeSlots != undefined) {
            timeSlots = this.props.tiqetsTimeSlots.timeSlots.map((item) => {
                return <option value={item.timeslot}>{item.timeslot}</option>
            })
        }
        if (that.props.fetchHohoTimeSlots) {
            timeSlots = that.props.hohoTimeSlots.timeSlots.map((item) => {
                return <option value={item}>{item}</option>
            })
        }
        return <div>
            <div className="container tickating-sub-tabs text-center pb-3">
                <header className={this.state.className}>
                    <ul className="nav justify-content-center pb-4 ticket-flow pt-4">
                        {(!this.props.isDonate) && < li className="nav-item">
                            <a className={that.state.currentDisplay == Display.Tickets ? "nav-link active" : "nav-link"} onClick={() => { this.setState({ currentDisplay: Display.Tickets }, () => { this.onClickTicketCategory() }) }}>Select your tickets</a>
                        </li>}
                        {!this.props.isDonate && <>{addOns}</>}
                        {(isDonationExists(this.state.cartData)) && <li className="nav-item">
                            <a className={that.state.currentDisplay == Display.Donate ? "nav-link active" : "nav-link"} href="javascript:void(0)" onClick={() => { this.setState({ currentDisplay: Display.Donate, isShowDonate: true }) }}>Donate</a>
                        </li>}
                    </ul>
                </header>
            </div>
            {(!this.state.isAddOns && !this.state.isShowDonate && !this.props.isDonate) && <section className="ticket-details pt-sm-5 pb-5">
                <div className="container">
                    <div className="row">
                        {!isAttendEvent && <div className={`calendar-picker col-sm-${this.state.showTicketType ? '4' : '12'}`}>
                            <h5 onClick={this.toggleCalender.bind(this)} className="mb-3 pink-color text-center">
                                Select Date
                        </h5>
                            <div className={`calendar-picker ${this.state.showTicketType ? '' : 'text-center'}`}>
                                <Calendar
                                    className="shadow border mx-auto mb-3"
                                    date={this.state.selectedDate}
                                    disabledDates={disabledDates}
                                    onChange={this.showTime.bind(this)}
                                    shownDate={getActiveDate(disabledDates, this.props)}
                                    minDate={getActiveDate(disabledDates, this.props)}
                                    maxDate={new Date(new Date(maxDate.endDateTime).getFullYear(), new Date(maxDate.endDateTime).getMonth(), new Date(maxDate.endDateTime).getDate())}
                                />
                            </div>
                            {((isTiqetsEvent && hasTiqetsTimeSlots) || (isHohoPlace && hasHohoTimeSlots)) &&
                                <select className="form-control shadow-sm mx-auto mb-4" onChange={this.changeTimeSlot.bind(this)} style={{ maxWidth: "350px" }}>
                                    <option value="" disabled selected>Select Time</option>
                                    {timeSlots}
                                </select>
                            }
                            {(this.props.fetchTiqetsTimeSlots && timeSlots.length == 0) &&
                                <div className="text-center pb-4">
                                    <h5>Timeslot not found or the selected date. Please try different date, Thanks!</h5>
                                </div>
                            }
                            {this.state.dateSelected && this.state.isTimeTypeExists &&
                                <div className="text-center pb-4">
                                    <h5>Select Time: <div className="d-sm-inline d-block pb-4">{timing}</div></h5>
                                </div>}
                        </div>}
                        {this.state.showTicketType &&
                            <div className={`col-sm-${isAttendEvent ? '12' : '8'}`}>
                                {ticketCategory}
                                <hr />
                                {this.state.validTicketCatError != '' && <p className="text-center text-danger">{this.state.validTicketCatError}</p>}
                                <div className="footer-bottom text-center pt-4">
                                    {(Quantity <= 0) && <a href="javascript:void(0)" className="btn btn-lg btn-light opacity-disabled disabled border">Add to bag</a>}
                                    {(Quantity > 0) && <a href="javascript:void(0)" onClick={() => {
                                        let isAddOns = this.state.cartData.filter(function (val) {
                                            return val.ticketCategoryTypeId == 2
                                        });
                                        if (this.state.currentDisplay == Display.Tickets && isAddOns.length > 0) {
                                            this.setState({ isAddOns: true, currentDisplay: Display.AddOn });
                                        } else if (isDonationExists(this.state.cartData) && this.state.currentDisplay == Display.AddOn) {
                                            this.setState({ isShowDonate: true, currentDisplay: Display.Donate });
                                        } else {
                                            this.continueToItinerary()
                                        }
                                    }} className="btn-lg site-primery-btn px-5 text-white">Add to bag</a>}
                                </div>
                            </div>
                        }
                    </div>
                    <div className="row ShowTicketSec text-center my-5 py-5">
                        <div className={isLiveOnlineEvent ? "col-sm-12" : "col-sm-6"}>
                            <p>{isLiveOnlineEvent ? "The link to the live-stream will be provided in the ticket purchase confirmation email. The link will become active a few minutes before the start of the event." : "Receive your tickets via email, SMS and our app"}</p>
                        </div>
                        {!isLiveOnlineEvent &&
                            <div className="col-sm-6">
                                <img src={`${gets3BaseUrl()}/icons/show-tickets-icon.png`} alt="" width="104" className="mb-4" />
                                <p>Show tickets on your smartphone</p>
                            </div>
                        }
                    </div>
                    {
                        this.props.ticketCategoryData.tiqetsCheckoutDetails && this.props.ticketCategoryData.tiqetsCheckoutDetails != undefined &&
                        this.props.ticketCategoryData.tiqetsCheckoutDetails != null &&
                        <div>
                            {getContent(this.props.ticketCategoryData)}
                        </div>
                    }
                </div>
            </section>}
            {(this.state.isAddOns || this.state.isShowDonate || this.props.isDonate) && < section className="ticket-details pb-5">
                <div className="container">
                    <h5 onClick={this.toggleCalender.bind(this)} className="mb-3 pink-color text-center"><i className="fa fa-calendar-alt"></i> <a href="javascript:void(0)" className="pink-color btn-lg btn-link p-0">{(that.state.currentDisplay != Display.Donate) ? '' : 'Choose Amount'}</a></h5>
                    <div className="row justify-content-center">
                        {ticketCategory}
                    </div>
                    <div className="footer-bottom text-center pt-4">
                        {(ticketCount > 0 && !this.props.isDonate) && <button type="button" onClick={() => {
                            let isAddOns = this.state.cartData.filter(function (val) {
                                return val.ticketCategoryTypeId == 2
                            });
                            if (this.state.currentDisplay == Display.Tickets && isAddOns.length > 0) {
                                this.setState({ isAddOns: true, currentDisplay: Display.AddOn });
                            } else if (isDonationExists(this.state.cartData) && this.state.currentDisplay == Display.AddOn) {
                                this.setState({ isShowDonate: true, currentDisplay: Display.Donate });
                            } else {
                                this.onChangeDonateAmount(0, true);
                            }
                        }} className="btn btn-link">Skip</button>}
                        {((Quantity > 0 || this.state.isShowDonate || this.props.isDonate) && (ticketCount > 0 || (this.props.isDonate && this.state.isDonationSelected))) && <a href="javascript:void(0)" onClick={() => {
                            let isAddOns = this.state.cartData.filter(function (val) {
                                return val.ticketCategoryTypeId == 2
                            });
                            if (this.state.currentDisplay == Display.Tickets && isAddOns.length > 0) {
                                this.setState({ isAddOns: true, currentDisplay: Display.AddOn });
                            } else if (isDonationExists(this.state.cartData) && this.state.currentDisplay == Display.AddOn) {
                                this.setState({ isShowDonate: true, currentDisplay: Display.Donate });
                            } else {
                                this.continueToItinerary()
                            }
                        }} className="btn-lg site-primery-btn px-5 text-white">Add to bag</a>}
                    </div>
                </div>
            </section>
            }
        </div>;
    }
}

polyfill(TicketCategorySelection);

export default TicketCategorySelection;