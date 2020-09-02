import * as React from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import { CartDataViewModel, CartItem, GuestData } from "../../models/Cart/CartDataViewModel";
import Calendar from 'react-calendar';
import * as numeral from "numeral";
import * as PubSub from 'pubsub-js';
import NavCart from "../NavCart/NavCart";
import * as ReactTooltip from 'react-tooltip';
import "./TicketCategoryDateSelection.scss";
import { StickyContainer, Sticky } from 'react-sticky';
import { ReactHTMLConverter } from "react-html-converter/browser";
import * as getSymbolFromCurrency from 'currency-symbol-map';
import { IApplicationState } from "../../stores";
import ModalDialog from "../ModalDialog/ModalDialog";
import { autobind } from "core-decorators";
import KzLoader from "../../components/Loader/KzLoader";
import ChauffeurServiceBooking from "./ChauffeurServiceBooking";
import { getDepartureTimes, getDepatureLocations, getFilteredLocation, getReturnTimes } from "../../utils/valueRetail";

const daysInWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
var seasonDays = [];
const SAARCNations = ['Afghanistan', 'Bangladesh', 'Bhutan', 'Maldives', 'Nepal', 'Pakistan', 'Sri Lanka'];
const BIMSTECNations = [' Bangladesh', 'Bhutan', 'Myanmar', 'Nepal', 'Sri Lanka', 'Thailand'];

export default class TicketCategorySelectionMoveAround extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            selectedDate: null,
            timeModel: [],
            isShowTiming: false,
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
            crrJourneyType: 2,
            currentPickupLocation: "",
            selectedPickupTime: "",
            selectedPickupTimeIndex: -1,
            selectedReturnTimeIndex: -1,
            returnTimeOptions: []
        }
    }

    public componentDidMount() {
        this.setState({ className: "" });
        //window.addEventListener('scroll', this.hideBar);
    }

    hideBar = () => {
        if (window.pageYOffset >= 450 && window.pageXOffset === 0) {
            this.setState({ className: "learn-head-stiky" });
        }
        else {
            this.setState({ className: "" });
        }
    }

    UNSAFE_componentWillMount() {
        var nextProps = this.props.ticketCategoryData;
        var that = this;
        var ticketCategoryDataModel: CartDataViewModel = {
            cartItems: [],
        };
        var venueData = nextProps.venue;
        var ticketCategories = nextProps.eventTicketDetail;

        var currentDateCategories = nextProps.eventTicketDetail.filter((val) => {
            return val.eventDetailId == nextProps.eventDetail[0].id
        });

        currentDateCategories.forEach((item) => {
            let categoryName = nextProps.ticketCategory.filter((val) => {
                return val.id == item.ticketCategoryId
            });

            let price = nextProps.eventTicketAttribute.filter(val => val.eventTicketDetailId == item.id);
            let isAddOn = nextProps.eventTicketDetailTicketCategoryTypeMappings.filter((val) => {
                if (val.ticketCategoryTypeId == 2 && val.eventTicketDetailId == item.id) {
                    return val.eventTicketDetailId == item.id
                }
            });

            let ticketCategoryMapping = nextProps.eventTicketDetailTicketCategoryTypeMappings.filter((val) => {
                if (val.eventTicketDetailId == item.id) {
                    return val.eventTicketDetailId == item.id
                }
            });

            let ticketCategoryDescription = nextProps.eventTicketAttribute.filter((val) => {
                return val.eventTicketDetailId == item.id
            });

            let eventTermsAndConditions = nextProps.event.termsAndConditions;
            var deliveryOptions = [];
            nextProps.eventDeliveryTypeDetails.filter((val) => {
                deliveryOptions.push(val.deliveryTypeId)
            });

            let cartJson: CartItem = {
                altId: nextProps.event.altId,
                eventDetailId: nextProps.eventDetail[0].id,
                name: nextProps.eventDetail[0].name,
                eventStartDate: nextProps.eventDetail[0].startDateTime,
                venue: venueData[0].name,
                city: nextProps.city[0].name,
                ticketCategoryId: categoryName[0].id,
                ticketCategoryName: categoryName[0].name,
                eventTicketAttributeId: price[0].id,
                currencyId: nextProps.currencyType.id,
                currencyName: nextProps.currencyType.code,
                quantity: 0,
                pricePerTicket: price[0].price,
                ticketCategoryDescription: ticketCategoryDescription[0].ticketCategoryDescription,
                eventTermsAndConditions: eventTermsAndConditions,
                selectedDate: "",
                isTimeSelection: false,
                placeVisitTime: "",
                guestList: [],
                isAddOn: (isAddOn.length > 0 ? true : false),
                ticketSubCategoryId: ((ticketCategoryMapping.length > 0) ? ticketCategoryMapping[0].ticketCategorySubTypeId : -1),
                ticketCategoryTypeId: ((ticketCategoryMapping.length > 0) ? ticketCategoryMapping[0].ticketCategoryTypeId : -1),
                deliveryOptions: deliveryOptions,
                journeyType: null,
                pickupLocation: {
                    address1: "",
                    address2: "",
                    town: "",
                    region: "",
                    postalCode: -1
                },
                pickupTime: "",
                returnLocation: null,
                returnTime: "",
                waitingTime: 0,
                eventVenueMappingTimeId: -1,
                parentCategory: nextProps.category.slug,
                subCategory: nextProps.subCategory.slug,
                isTiqetsPlace: false
            }
            ticketCategoryDataModel.cartItems.push(cartJson);
        });
        this.loadCartItems(ticketCategoryDataModel.cartItems);
        this.setState({ cartData: ticketCategoryDataModel.cartItems });
    }

    public toggleCalender = (e) => {
        this.setState({ calenderVisible: (!this.state.calenderVisible) });
    }

    public showTime = (date) => {
        var that = this;
        var isShowTiming = false;
        var timeArray = [];
        var currentDate = date;
        var data = that.state.cartData;
        var ticketCategoryDataModel: CartDataViewModel = {
            cartItems: [],
        };
        var venueData = this.props.ticketCategoryData.venue;
        var ticketCategories = this.props.ticketCategoryData.eventTicketDetail;
        var isTimeTypeExists = false;
        var timeModel = [];
        var isSeasonDate = false;
        var isSpecialDay = false;

        /*-------- Regular Time Type -------------*/
        if (!isSeasonDate && !isSpecialDay) {
            if (this.props.ticketCategoryData.eventDetail.length > 1) {
                this.props.ticketCategoryData.eventDetail.map((item) => {
                    var startDate = new Date(item.startDateTime);
                    var endDate = new Date(item.endDateTime);
                    var timeModel = {
                        eventDetailId: item.id,
                        fromTime: (startDate.getHours().toString() + ":" + (startDate.getMinutes().toString() == "0" ? "00" : startDate.getMinutes().toString())),
                        toTime: (endDate.getHours().toString() + ":" + (endDate.getMinutes().toString() == "0" ? "00" : endDate.getMinutes().toString())),
                    }
                    timeArray.push(timeModel);
                });
            } else {
                var startDate = new Date(this.props.ticketCategoryData.eventDetail[0].startDateTime);
                currentDate = new Date(date);
                currentDate.setHours(startDate.getHours(), startDate.getMinutes(), 0);
            }

            data.forEach((item) => {
                var itemData = item;
                itemData.isTimeSelection = isShowTiming,
                    itemData.selectedDate = currentDate.toString()
            });

            this.setState({
                cartData: data,
                selectedTime: 0,
                selectedDate: date,
                calenderVisible: true,
                dateSelected: true,
                showTicketType: isTimeTypeExists ? false : true,
                isTimeTypeExists: isTimeTypeExists,
            });
        }
    }

    public loadCartItems = (data) => {
        var that = this;
        data.map((val) => {
            var eventTicketAttributeId = val.eventTicketAttributeId;
            var quantity = val.quantity;
            that.setState({
                [eventTicketAttributeId]: quantity
            });
        });
    }

    public addTicket = (item, e) => {
        e.preventDefault();
        if (this.state[item.eventTicketAttributeId] < 10) {
            var cartData = this.state.cartData;
            for (var i = 0; i < cartData.length; i++) {
                if (cartData[i].eventTicketAttributeId == item.eventTicketAttributeId) {
                    cartData[i].quantity = ++(item.quantity);
                    /*var newGuest: GuestData = {
                        guestId: cartData[i].guestList.length + 1,
                        firstName: "",
                        lastName: "",
                        nationality: "",
                        documentTypeId: "",
                        documentNumber: "",
                    };
                    cartData[i].guestList.push(newGuest);*/
                    var cartItems = cartData.filter((val) => {
                        return val.quantity > 0
                    });
                    localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
                    this.loadCartItems(cartData);
                    break;
                }
            }
        }
    }

    public removeTicket = (item, e) => {
        e.preventDefault();
        if (this.state[item.eventTicketAttributeId] > 0) {
            var cartData = this.state.cartData;
            for (var i = 0; i < cartData.length; i++) {
                if (cartData[i].eventTicketAttributeId == item.eventTicketAttributeId) {
                    cartData[i].quantity = --(item.quantity);
                    cartData[i].guestList.pop();
                    var cartItems = cartData.filter((val) => {
                        return val.quantity > 0
                    });
                    localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
                    this.loadCartItems(cartData);
                    break;
                }
            }
        }
    }

    /*public setGuestInformation = (event, guestId, info, e) => {
        if (info === "nationality") {
            this.setState({
                currentCountry: e.target.value
            });
        }
        var cartData = this.state.cartData;
        for (var i = 0; i < cartData.length; i++) {
            if (cartData[i].eventTicketAttributeId == event.eventTicketAttributeId) {

                cartData[i].guestList[guestId][info] = e.target.value;
                var cartItems = cartData.filter((val) => {
                    return val.quantity > 0
                });
                localStorage.setItem('currentCartItems', JSON.stringify(cartItems));
                break;
            }
        }
    }*/

    public continueToItinerary = () => {
        let isPrivateChauffeuredVehicle = true;
        if (this.props.ticketCategoryData.eventCategory == "Private Chauffeured Vehicle") {
            let data = this.state.cartData.filter(item => item.journeyType && item.waitingTime && item.pickupLocation.address2 && item.pickupLocation.postalCode != -1)
            isPrivateChauffeuredVehicle = data.length === 0
        } else {
            let data = this.state.cartData.filter(item => item.journeyType && item.pickupTime && item.pickupLocation.address1 && item.returnTime)
            isPrivateChauffeuredVehicle = data.length === 0
        }
        if (isPrivateChauffeuredVehicle) {
            alert('Please fill the required details');
        } else {
            var data = this.state.cartData.filter((item) => {
                return item.ticketCategoryTypeId == 1 || item.ticketCategoryTypeId == -1
            });

            if (!this.state.dateSelected) {
                alert("Please select date");
            } else {
                if (data.length === 0) {
                    alert("Please add tickets");
                } else {

                    /*data.forEach((val) => {
                        if (val.quantity <= 0) {
                            alert("Please add tickets");
                        }
                    });
        
                    var guestList = [];
                    data.forEach((val) => {
                        if (val.guestList.length > 0) {
                            val.guestList.forEach((guest) => {
                                if ((guest.firstName != "" && guest.lastName != "" && guest.nationality != "" && guest.documentTypeId != "" && guest.documentNumber != "")) {
                                    guestList.push(guest);
                                }
                            })
                        }
                    });
                    if (guestList.length < categories.length) {
                        alert("Please fill in all guests' details to continue.");
                    } else {*/
                    var allCartItems = (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') ? JSON.parse(localStorage.getItem("cartItems")) : [];
                    var currentCartItems = (localStorage.getItem('currentCartItems') != null && localStorage.getItem('currentCartItems') != '0') ? JSON.parse(localStorage.getItem("currentCartItems")) : [];
                    if (currentCartItems.pickupTime == "") alert("Please Select Pickup Time");
                    allCartItems = allCartItems.filter((item) => { /* remove already added cart items for same day and event */
                        var selectedDate = new Date(item.selectedDate);
                        var isAlreadyExistsForPlace = false;
                        currentCartItems.map((val) => {
                            var currentSelectedDate = new Date(val.selectedDate);
                            if (val.altId == item.altId) {
                                isAlreadyExistsForPlace = true;
                            }
                        });
                        if (!isAlreadyExistsForPlace) {
                            return item;
                        }
                    })
                    currentCartItems.forEach((val) => {
                        allCartItems.push(val);
                    });
                    localStorage.setItem('cartItems', JSON.stringify(allCartItems));
                    PubSub.publish('UPDATE_NAV_CART_DATA_EVENT', 1);
                    this.props.goToItinerary();
                }
            }
        }
    }

    public onTimeClick = (timeType, selectedTime, index, e) => {
        let returnTimeForSelectDeparture = [];
        if (this.props.ticketCategoryData.eventVenueMappingTimes.length > 0) {
            returnTimeForSelectDeparture = getReturnTimes(getFilteredLocation(this.props.ticketCategoryData.eventVenueMappingTimes, this.state.currentPickupLocation), selectedTime.pickupTime);
        }
        let fromTime = selectedTime.pickupTime.split(":");
        let isTimExistsForCurrentDate = false;
        let cartData = this.state.cartData.map((item) => {
            var date = new Date(item.selectedDate);
            if (timeType == "departureTime") {
                item.pickupTime = selectedTime.pickupTime;
                this.setState({
                    selectedPickupTimeIndex: index,
                    selectedPickupTime: selectedTime.pickupTime
                });
            }

            if (timeType == "returnTime") {
                item.returnTime = selectedTime;
                this.setState({
                    selectedReturnTimeIndex: index
                });
            }
            // if (((date.getDate() == that.state.selectedDate.getDate()) && (date.getMonth() == that.state.selectedDate.getMonth()) && (date.getFullYear() == that.state.selectedDate.getFullYear()) && item.placeVisitTime == time) || item.placeVisitTime == "") {
            isTimExistsForCurrentDate = true;
            date.setHours(+fromTime[0], +fromTime[1], 0);
            item.placeVisitTime = item.pickupTime + " - " + item.returnTime;
            item.selectedDate = date.toString();
            return item
        });
        this.setState({ cartData: cartData, showTicketType: true, returnTimeOptions: returnTimeForSelectDeparture });
    }

    @autobind
    public onClickAddOns(id) {
        this.setState({ isAddOns: true, addOnId: id });
    }

    @autobind
    public onClickTicketCategory() {
        this.setState({ isAddOns: false });
    }

    @autobind
    getDayExistsInSeasonDate(currentDate, startDate, endDate) {
        var currentDateDate = currentDate.getDate();
        var currentDateMonth = currentDate.getMonth();

        var seasonStartDate = startDate.getDate();
        var seasonStartDateMonth = startDate.getMonth();

        var seasonEndDate = endDate.getDate();
        var seasonEndMonth = endDate.getMonth();

        if ((currentDateDate >= seasonStartDate && currentDateDate <= seasonEndDate) && (currentDateMonth >= seasonStartDateMonth && currentDateMonth <= seasonEndMonth)) {
            return true;
        } else {
            false
        }
    }

    public setPickupLocation = (value, e) => {
        var timeModel = [];
        if (!this.props.ticketCategoryData.regularTimeModel.isSameTime) {
            timeModel = getDepartureTimes(getFilteredLocation(this.props.ticketCategoryData.eventVenueMappingTimes, value.pickupLocation));
        }

        let cartData = this.state.cartData.map(item => {
            return {
                ...item,
                pickupLocation: {
                    ...item.pickupLocation,
                    address1: value.pickupLocation
                },
                eventVenueMappingTimeId: value.id
            };
        });
        this.setState({
            cartData: cartData,
            currentPickupLocation: value.pickupLocation,
            selectedDateTimeModel: timeModel,
            isShowTiming: timeModel.length ? true : false,
            selectedTimeIndex: -1,
            selectedPickupTimeIndex: -1,
            selectedPickupTime: ""
        });
    };

    public setReturnLocation = (value, e) => {
        let cartData = this.state.cartData.map(item => {
            return item.returnLocation = value;
        });
        this.setState({
            cartData: cartData,
            returnLocation: value
        });
    };

    onJourneyTypeChange = (e) => {
        let cartData = this.state.cartData.map(item => {
            return {
                ...item, journeyType: +e.target.value
            };
        });
        this.setState({
            cartData: cartData,
        });
    };

    getJourneyTypeName = (journeyType: number) => {
        if (journeyType == 0) {
            return "From Village";
        } else if (journeyType == 1) {
            return "To Village";
        } else if (journeyType == 2) {
            return "Return";
        } else {
            return "None";
        }
    };

    onChauffeurTimeSelection = (e) => {
        let cartData = this.state.cartData.map(item => {
            return {
                ...item, pickupTime: e.target.value
            };
        });
        this.setState({
            cartData: cartData,
        });
    };



    onChauffeurWaitingTimeChange = (e) => {
        let cartData = this.state.cartData.map(item => {
            return {
                ...item, waitingTime: +e.target.value
            };
        });
        this.setState({
            cartData: cartData,
        });
    };

    onChauffeurDepartureAddressChange = (e) => {
        let cartData = this.state.cartData.map(item => {
            return {
                ...item, pickupLocation: { ...item.pickupLocation, [e.target.name]: e.target.value }
            };
        });
        this.setState({
            cartData: cartData,
            showTicketType: true
        });
    };

    public render() {
        let filteredLocations = getDepatureLocations(this.props.ticketCategoryData.eventVenueMappingTimes);
        var data = this.state.cartData;
        var Quantity = 0;
        var that = this;
        var i = 0;
        var departureTiming = [];
        var returnTiming = [];
        var that = this;
        var disabledDates = [];
        seasonDays = [];
        var weekOff = [];
        const converter = ReactHTMLConverter();
        converter.registerComponent('Ticket', TicketCategorySelectionMoveAround);

        this.props.ticketCategoryData.placeHolidayDates.map((item) => {
            var holidayDates = item.leaveDateTime.split("-");
            var holidatDay = holidayDates[2].split("T");
            disabledDates.push(new Date(+holidayDates[0], (+holidayDates[1] - 1), +holidatDay[0]));
        });
        if (this.state.dateSelected && this.state.currentPickupLocation) {
            departureTiming = this.state.selectedDateTimeModel.map((item, index) => {
                var className = "badge badge-secondary mr-1";
                if (index == that.state.selectedPickupTimeIndex) {
                    className = "badge badge-primary mr-1";
                }
                return <a href="javascript:void(0)" className={className} onClick={(e) => this.onTimeClick("departureTime", item, index, e)}>{item.pickupTime}</a>
            });

            if (this.state.selectedPickupTimeIndex !== -1) {
                returnTiming = this.state.returnTimeOptions.map((item, index) => {
                    var className = "badge badge-secondary mr-1";
                    if (index == that.state.selectedReturnTimeIndex) {
                        className = "badge badge-primary mr-1";
                    }
                    return <a href="javascript:void(0)" className={className} onClick={(e) => this.onTimeClick("returnTime", item, index, e)}>{item.returnTime}</a>
                });
            }
        }

        var documentType = this.props.ticketCategoryData.placeCustomerDocumentTypeMappings.map((item, index) => {
            var isExist = that.props.ticketCategoryData.customerDocumentTypes.filter((val) => {
                if (item.customerDocumentType == val.id) {
                    return val;
                }
            });
            if (isExist.length > 0) {
                return <option value="${index}">{isExist[0].documentType}</option>
            }
        });
        var ticketCategory;
        if (!this.state.isAddOns) {
            data = data.filter((val) => {
                if ((val.ticketCategoryTypeId == 1 || val.ticketCategoryTypeId == -1) && val.pricePerTicket != 0) {
                    return val
                }
            });
            ticketCategory = data.map((val) => {
                var category = val.eventTicketAttributeId;
                i = i + 1;
                var ticketQuantity = that.state[category];
                Quantity += ticketQuantity;
                var symbol = getSymbolFromCurrency(val.currencyName);
                let filteredCountries = null;
                if (val.ticketCategoryName.includes("Indian")) {
                    filteredCountries = that.props.countries.filter((item) => {
                        return item.name == "India";
                    });
                }
                if (val.ticketCategoryName.includes("SAARC")) {
                    filteredCountries = that.props.countries.filter((item) => {
                        return SAARCNations.indexOf(item.name) != -1;
                    });
                }
                if (val.ticketCategoryName.includes("BIMSTEC")) {
                    filteredCountries = that.props.countries.filter((item) => {
                        return BIMSTECNations.indexOf(item.name) != -1;
                    });
                }
                if (filteredCountries == null) {
                    filteredCountries = that.props.countries;
                }
                var countries = filteredCountries.map((item) => {
                    return <option value={item.phonecode}>{item.name}</option>
                });

                /*var guestInformation = val.guestList.map((guest) => {
                    return <div className="col-sm-12 pt-3">
                        <form>
                            <div className="form-row mt-2 pl-2 pr-2 mb-2">
                                <div style={{ lineHeight: "26px" }}>
                                    Guest {guest.guestId}:
                            </div>
                                <div className="form-group col-md-2 mb-2">
                                    <input type="text" className="form-control form-control-sm" onChange={(e) => this.setGuestInformation(val, (guest.guestId - 1), "firstName", e)} placeholder="First Name" required />
                                </div>
                                <div className="form-group col-md-2 mb-2">
                                    <input type="text" className="form-control form-control-sm" onChange={(e) => this.setGuestInformation(val, (guest.guestId - 1), "lastName", e)} placeholder="Last Name" required />
                                </div>
                                <div className="form-group col-md-2 mb-2">
                                    <select id="inputState" className="form-control form-control-sm" onChange={(e) => this.setGuestInformation(val, (guest.guestId - 1), "nationality", e)} required >
                                        <option>Nationality</option>
                                        {countries}
                                    </select>
                                </div>
                                <div className="form-group col-md-2 mb-2">
                                    {(that.props.ticketCategoryData.placeCustomerDocumentTypeMappings.length == 0) &&
                                        <select id="inputState" className="form-control form-control-sm" onChange={that.setGuestInformation.bind(that, val, (guest.guestId - 1), "documentTypeId")}
                                            required>												<option>Document Type</option>
                                            <option value="1">Passport</option>
                                            <option value="2">Driving Licence</option>
                                            {that.state.currentCountry == "India" ?
                                                <option value="3">Aadhar</option> :
                                                <option value="3">National Id</option>
                                            }
                                            {that.state.currentCountry == "India" ?
                                                <option value="4">Voter Id</option> : null
                                            }

                                        </select>}
                                    {(that.props.ticketCategoryData.placeCustomerDocumentTypeMappings.length > 0) &&
                                        <select id="inputState" className="form-control form-control-sm" onChange={(e) => this.setGuestInformation(val, (guest.guestId - 1), "documentTypeId", e)} required>
                                            <option>Document Type</option>
                                            {documentType}
                                        </select>}
                                </div>
                                <div className="form-group col-md-2 mb-2">
                                    <input type="text" pattern="[a-zA-Z0-9-]+" className="form-control form-control-sm" onChange={(e) => this.setGuestInformation(val, (guest.guestId - 1), "documentNumber", e)} placeholder="ID Number" required />
                                </div>
                            </div>
                        </form>
                    </div>
                });*/

                return <div>
                    <hr />
                    <div className="row">
                        <div className="col-sm-6 col-lg-7 text-sm-left text-center pb-2 pb-sm-0">
                            <span className="h6">{val.ticketCategoryName}
                                {(converter.convert(val.ticketCategoryDescription) != "" && converter.convert(val.ticketCategoryDescription) != null) && <span>
                                    <i data-tip={"React-tooltip" + i} data-for={val.ticketCategoryName + i} className="fa fa-info-circle text-primary ml-2" />
                                    <ReactTooltip place="bottom" id={val.ticketCategoryName + i} type="info" effect="float">
                                        <span style={{ maxWidth: "230px", display: "block" }}>{converter.convert(val.ticketCategoryDescription)}</span>
                                    </ReactTooltip>
                                </span>}
                            </span>
                        </div>

                        <div className="col-sm-6 col-lg-5 text-sm-right text-center pb-2 pb-sm-0">
                            <span className="pr-2"></span>
                            <span className="pink-color pr-3 h5">{symbol + numeral(val.pricePerTicket).format('0.00')}</span>
                            <div className="add-to-cart-btns">
                                <a href="javascript:void(0)" className="rounded-circle border decrease-iten" onClick={(e) => this.removeTicket(val, e)}>-</a>
                                <input className="form-control velue-item mr-1 ml-1" placeholder={ticketQuantity} type="text" />
                                <a href="javascript:void(0)" className="rounded-circle border increase-item" onClick={(e) => this.addTicket(val, e)}>+</a>
                            </div>
                        </div>
                        {/*guestInformation*/}
                    </div>
                </div>;
            });
        } else {
            data = data.filter((val) => {
                if (val.ticketSubCategoryId == that.state.addOnId) {
                    return val
                }
            });
            ticketCategory = data.map((item) => {
                var category = item.eventTicketAttributeId;
                i = i + 1;
                var ticketQuantity = that.state[category];
                Quantity += ticketQuantity;
                return <div className="col-sm-6">
                    <div className="card">
                        <div className="card-body">
                            <h5 className="card-title">{item.ticketCategoryName}</h5>
                            <div className="add-to-cart-btns">
                                <a href="javascript:void(0)" className="rounded-circle border decrease-iten" onClick={e => this.removeTicket(item, e)}>-</a>
                                <input className="form-control velue-item mr-1 ml-1" placeholder={ticketQuantity} type="text" />
                                <a href="javascript:void(0)" className="rounded-circle border increase-item" onClick={e => this.addTicket(item, e)}>+</a>
                            </div>
                        </div>
                    </div>
                </div>
            })
        }

        var subEventData = this.props.ticketCategoryData.eventDetail.filter((val) => {
            return val.name == that.props.eventName;
        })
        if (subEventData.length == 0) {
            return <div></div>;
        }
        let maxDate = subEventData.reduce((a, b) => { return a.startDateTime > b.startDateTime ? a : b; });
        let minDate = subEventData.reduce((a, b) => { return a.startDateTime < b.startDateTime ? a : b; });
        var etdTicketCategoryMappings = this.props.ticketCategoryData.eventTicketDetailTicketCategoryTypeMappings.filter((item) => {
            return item.ticketCategoryTypeId == 2
        });
        var ticketCatSubType = etdTicketCategoryMappings.map((item) => { return item.ticketCategorySubTypeId });
        ticketCatSubType = ticketCatSubType.filter((value, index, self) => {
            return self.indexOf(value) === index;
        });
        var addOns = ticketCatSubType.map((item) => {
            var subType = that.props.ticketCategoryData.ticketCategorySubTypes.filter((val) => {
                return val.id == item
            });
            return <li className="nav-item">
                <a className="nav-link" href="javascript:void(0)" onClick={() => that.onClickAddOns(subType[0].id)}>{subType[0].name}</a>
            </li>
        });

        return <div>
            <StickyContainer>
                <div className="container tickating-sub-tabs text-center pb-3">
                    <Sticky topOffset={400}>
                        {({
                            style,
                            isSticky
                        }) => {
                            return (
                                <header className={this.state.className}>
                                    <ul className="nav justify-content-center pb-4 ticket-flow">
                                        <li className="nav-item">
                                            <a className="nav-link active" onClick={() => this.onClickTicketCategory()}>{this.props.ticketCategoryData.eventDetail[0].name}</a>
                                        </li>
                                        {addOns}
                                    </ul>
                                </header>
                            )
                        }
                        }
                    </Sticky>
                    {(!this.state.isAddOns) && <h2>Choose Your Ticket</h2>}
                </div>
            </StickyContainer>

            {(!this.state.isAddOns) && < section className="ticket-details bg-light pt-5 pb-5">
                <div className="container">
                    <h5 onClick={this.toggleCalender.bind(this)} className="mb-3 pink-color text-center"><i className="fa fa-calendar-alt"></i> <a href="javascript:void(0)" className="pink-color btn-lg btn-link p-0"> Select Date</a></h5>
                    <div className="calendar-picker">
                        <Calendar className="m-auto"
                            onChange={this.showTime}
                            tileDisabled={({ date, view }) =>
                                (view === 'month') && // Block day tiles only
                                disabledDates.some(disabledDate =>
                                    date.getFullYear() === disabledDate.getFullYear() &&
                                    date.getMonth() === disabledDate.getMonth() &&
                                    date.getDate() === disabledDate.getDate()
                                )
                            }
                            value={this.state.selectedDate}
                            minDate={(this.props.ticketCategoryData != undefined
                                && this.props.ticketCategoryData.eventDetail != undefined &&
                                this.props.ticketCategoryData.eventDetail.length > 0) ?
                                ((new Date(this.props.ticketCategoryData.eventDetail[0].startDateTime) > new Date()) ?
                                    new Date(new Date(this.props.ticketCategoryData.eventDetail[0].startDateTime).getFullYear(), new Date(this.props.ticketCategoryData.eventDetail[0].startDateTime).getMonth(), new Date(this.props.ticketCategoryData.eventDetail[0].startDateTime).getDate())
                                    : new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate())
                                )
                                : new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate())}
                            maxDate={new Date(new Date(maxDate.endDateTime).getFullYear(), new Date(maxDate.endDateTime).getMonth(), new Date(maxDate.endDateTime).getDate())} /> <br />
                    </div>

                    {/*{this.state.dateSelected && this.state.isTimeTypeExists && <div className="text-center"><h5>Select Time: {timing}</h5></div>}
                        {this.state.showTicketType && <div>
                            <h5 className="mb-3 pink-color">Select Ticket Category</h5>
                            {ticketCategory}
                        </div>}
                        <hr />
                        <div className="footer-bottom text-center pt-4">
                            {(Quantity <= 0) && <a href="javascript:void(0)" className="btn-lg btn btn-light opacity-disabled">Continue</a>}
                            {(Quantity > 0) && <a href="javascript:void(0)" onClick={this.continueToItinerary.bind(this)} className="btn-lg site-primery-btn">Continue</a>}
                        </div>*/}

                    {this.state.dateSelected && <div className="container">
                        <div className="mb-3 pb-3 border-bottom"><h5 className="mb-3 pink-color text-center">Select Journey Type</h5>
                            <div className="form-check form-check-inline d-flex justify-content-center">
                                {this.props.ticketCategoryData.eventVenueMappingTimes.map(item => +item.journeyType).filter((v, i, a) => a.indexOf(v) === i).map((item, index) => <div className="form-check form-check-inline d-flex justify-content-center">
                                    <input className="form-check-input" onChange={this.onJourneyTypeChange} type="radio" id="journeyType" name="journeyType" value={item} />
                                    <label className="form-check-label" htmlFor="journeyType">{this.getJourneyTypeName(item)}</label>
                                </div>)}
                            </div>
                        </div>
                        {this.props.ticketCategoryData.eventCategory == "Private Chauffeured Vehicle" ? <ChauffeurServiceBooking onTimeSelection={this.onChauffeurTimeSelection} onDepartureAddressChange={this.onChauffeurDepartureAddressChange} onWaitingTimeChange={this.onChauffeurWaitingTimeChange} waitingTime={this.state.cartData[0].waitingTime} pickupTime={this.state.cartData[0].pickupTime} /> : <div><h5 className="mb-3 pink-color text-center">Select Pickup Location</h5>
                            <div className="calendar-picker">
                                <ul className="bg-light tab-pane fade show active p-3 text-center">
                                    {filteredLocations.map((item, index) =>
                                        <li key={index}
                                            data-id={item.id}
                                            onClick={(e) => this.setPickupLocation(item, e)} className={this.state.currentPickupLocation == item.pickupLocation ? "btn btn-sm badge-primary mr-1" : "btn btn-sm btn-outline-secondary mr-1 ml-1"}>
                                            {item.pickupLocation}
                                        </li>)}
                                </ul>
                            </div>
                            {this.state.currentPickupLocation && departureTiming.length > 0 && <div className="text-center pb-4"><h5>Select Pickup Time: <div className="d-sm-inline d-block pb-4">{departureTiming}</div> </h5></div>}
                        </div>}

                        {this.state.dateSelected && returnTiming.length > 0 && <div>
                            {/*<h5 className="mb-3 pink-color text-center"><a href="javascript:void(0)" className="pink-color btn-lg btn-link p-0"> Select Return Location</a></h5>
                                <div className="calendar-picker">
                                    <ul className="bg-light tab-pane fade show active p-3 text-center">
                                        {this.props.ticketCategoryData.eventVenueMappingTimes
                                            .map((item, index) => <li key={item.name + index}
                                                onClick={(e) => this.setReturnLocation(item.returnLocation, e)}
                                                className="btn btn-sm btn-outline-secondary mr-1 ml-1">
                                                {item.returnLocation}
                                            </li>)}
                                    </ul>
                                </div>*/}

                            {this.state.dateSelected && returnTiming.length > 0 && <div className="text-center pb-4"><h5>Select Return Time: <div className="d-sm-inline d-block">{returnTiming}</div></h5></div>}
                        </div>}

                        {this.state.showTicketType && <div>
                            <h5 className="mb-3 pink-color text-sm-left text-center">Select Ticket Category</h5>
                            {ticketCategory}
                        </div>}
                        <hr />
                        <div className="footer-bottom text-center pt-4">
                            {(Quantity <= 0) && <a href="javascript:void(0)" className="btn-lg btn btn-light opacity-disabled">Continue</a>}
                            {(Quantity > 0) && <a href="javascript:void(0)" onClick={this.continueToItinerary} className="btn-lg site-primery-btn">Continue</a>}
                        </div>
                    </div>}
                </div>
            </section>}
            {(this.state.isAddOns) && < section className="ticket-details bg-light pb-5">
                <div className="container">
                    <h5 onClick={this.toggleCalender.bind(this)} className="mb-3 pink-color text-center"><i className="fa fa-calendar-alt"></i> <a href="javascript:void(0)" className="pink-color btn-lg btn-link p-0"> Select Add-ons</a></h5>

                    <div className="row">
                        {ticketCategory}
                    </div>

                    <hr />
                    <div className="footer-bottom text-center pt-4">
                        {(Quantity <= 0) && <a href="javascript:void(0)" className="btn-lg btn btn-light opacity-disabled">Continue</a>}
                        {(Quantity > 0) && <a href="javascript:void(0)" onClick={this.continueToItinerary} className="btn-lg site-primery-btn">Continue</a>}
                    </div>
                </div>
            </section>}
        </div>;
    }
}