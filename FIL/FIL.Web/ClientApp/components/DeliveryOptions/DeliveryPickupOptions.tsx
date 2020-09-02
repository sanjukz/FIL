import * as React from "react";
import { Link } from "react-router-dom";
import { Button } from "react-bootstrap";
import * as numeral from "numeral";
import * as getSymbolFromCurrency from 'currency-symbol-map'
import { gets3BaseUrl } from "../../utils/imageCdn";
import { DeliveryDetail } from "../../models/DeliveryOptions/DeliveryDetailFormDataViewModel";

var dict = {}; var copy = {}, venuePickup = {};
var userDetailModel = [];

export default class DeliveryPickupOptions extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            cartData: "",
            showPickupDetails: false,
            isDeliveryOptionSelected: false,
            selfpickupFlg: true,
            otherPickupFlg: false,
            s3BaseUrl: gets3BaseUrl()
        };
    }

    public componentDidMount() {
        if (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') {
            var data = JSON.parse(localStorage.getItem("cartItems"));
            let that = this;
            var unique = Array.from(new Set(data.map(item => item.altId)));
            unique.forEach(function (val) {
                that.setState({ [val.toString()]: "0" });
            });
            this.setState({ cartData: data });
            data.map(function (dataItem) {
                var DeliveryDetailModelData: DeliveryDetail = {
                    eventAltid: dataItem.altId,
                    firstName: "",
                    lastName: "",
                    email: "",
                    phoneCode: "",
                    phoneNumber: "",
                    deliveryTypeId: 0,
                    eventTicketAttributeId: dataItem.eventTicketAttributeId
                }
                userDetailModel.push(DeliveryDetailModelData);
                venuePickup[dataItem.altId] = false;
            })
        }
    }

    public continueToPickupDetails() {
        let data = this.state.cartData;
        let that = this;
        let flg = true;
        let userDetails = that.props.deliveryOptions.userDetails;
        userDetailModel.map(function (item) {
            if (item.deliveryTypeId == 2 && venuePickup[item.eventAltid] == true) {
                if (item.firstName == "" || item.lastName == "" || item.email == "" || item.phoneCode == "" || item.phoneNumber == "") {
                    flg = false;
                    return false;
                }
                if (item.phoneCode != "") {
                    let phoneCode = that.props.countryData.countries.filter((val) => {
                        return val.altId == item.phoneCode;
                    })
                    item.phoneCode = phoneCode[0].phonecode.toString();
                }

            }
            else if (item.deliveryTypeId == 2 && venuePickup[item.eventAltid] == false) {
                item.firstName = userDetails["firstName"];
                item.lastName = userDetails["lastName"];
                item.email = userDetails["email"];
                item.phoneCode = userDetails["phoneCode"];
                item.phoneNumber = userDetails["phoneNumber"];
            }
            else if (item.deliveryTypeId == 0) {
                flg = false;
            }
        })
        if (flg) {
            this.props.submitDetails(userDetailModel);
        }
        else {
            alert("Please fill all the required details to continue");
            return;
        }
    }

    public setPickupOption(item, value, e) {
        if (value == 2) {
            dict[item] = true;
        }
        else {
            dict[item] = false;
        }
        this.setState({
            [item]: value
        });
        userDetailModel.map(function (val) {
            if (val.eventAltid == item) {
                val.deliveryTypeId = value
            }
        });
    }

    public parseDateLocal(s) {
        var b = s.split(/\D/);
        return new Date(b[0], b[1] - 1, b[2], (b[3] || 0),
            (b[4] || 0), (b[5] || 0), (b[6] || 0));
    }

    handleFirstNameChange(eventAltId, e) {
        userDetailModel.map(function (item) {
            if (item.eventAltid == eventAltId) {
                item.firstName = e.target.value
            }
        });
        this.setState({
            temp: 0
        })
    }
    handleLastNameChange(eventAltId, e) {
        userDetailModel.map(function (item) {
            if (item.eventAltid == eventAltId) {
                item.lastName = e.target.value
            }
        });
        this.setState({
            temp: 0
        })
    }
    handleEmailNameChange(eventAltId, e) {
        userDetailModel.map(function (item) {
            if (item.eventAltid == eventAltId) {
                item.email = e.target.value
            }
        });
        this.setState({
            temp: 0
        })
    }
    handlePhoneChange(eventAltId, e) {
        userDetailModel.map(function (item) {
            if (item.eventAltid == eventAltId) {
                item.phoneNumber = e.target.value
            }
        });
        this.setState({
            temp: 0
        })
    }
    handleCountryChange(eventAltId, e) {
        userDetailModel.map(function (item) {
            if (item.eventAltid == eventAltId) {
                item.phoneCode = e.target.value
            }
        });
        this.setState({
            temp: 0
        })
    }
    CopyAbove(index, e) {
        if (e.target.checked) {
            userDetailModel[index].firstName = userDetailModel[index - 1].firstName
            userDetailModel[index].lastName = userDetailModel[index - 1].lastName
            userDetailModel[index].email = userDetailModel[index - 1].email
            userDetailModel[index].phoneCode = userDetailModel[index - 1].phoneCode
            userDetailModel[index].phoneNumber = userDetailModel[index - 1].phoneNumber
        }
        else {
            userDetailModel[index].firstName = ""
            userDetailModel[index].lastName = ""
            userDetailModel[index].email = ""
            userDetailModel[index].phoneCode = ""
            userDetailModel[index].phoneNumber = ""
        }
        this.setState({
            temp: 0
        })
    }

    venuePickupOptions(deliveryBy, eventAltId, e) {
        if (deliveryBy == 0) {
            this.setState({
                selfpickupFlg: true,
                otherPickupFlg: false
            })
            venuePickup[eventAltId] = false;
        }
        if (deliveryBy == 1) {
            this.setState({
                selfpickupFlg: false,
                otherPickupFlg: true
            });
            venuePickup[eventAltId] = true;
        }
    }
    public render() {
        var data = this.state.cartData;
        var that = this;
        if (data != "") {
            let i = 1;
            data.sort(function (a, b) {
                var keyA = new Date(a.eventStartDate),
                    keyB = new Date(b.eventStartDate);
                if (keyA < keyB) return -1;
                if (keyA > keyB) return 1;
                return 0;
            });
            var unique = Array.from(new Set(data.map(item => item.altId)));
            var deliveryPickupData = unique.map(function (altId, unqdex) {

                var categories = data.filter(function (cat) {
                    return cat.altId == altId;
                });
                var firstName = "", lastName = "", email = "", phoneCode = "", phoneNumber = "";
                var categoryCost = 0;
                var categoryData = categories.map(function (val) {
                    categoryCost += (val.quantity * val.pricePerTicket);
                    var symbol = getSymbolFromCurrency(val.currencyName);
                    return <p className="m-0"><small>{val.ticketCategoryName + " (" + val.quantity + " x " + symbol + numeral(val.pricePerTicket).format('0.00') + ")"} <span className="float-sm-right">{symbol + numeral((val.quantity * val.pricePerTicket)).format('0.00') + ' ' + val.currencyName}</span></small></p>;
                });
                var eventInformation = categories[0];
                copy[eventInformation.altId] = false;
                if (unqdex > 0) {
                    if (userDetailModel[unqdex - 1].deliveryTypeId == 2) {
                        if (userDetailModel[unqdex - 1].firstName != "" && userDetailModel[unqdex - 1].lastName != "" && userDetailModel[unqdex - 1].email != "" && userDetailModel[unqdex - 1].phoneNumber != "") {
                            copy[eventInformation.altId] = true;
                        }
                    }
                }
                var userData = userDetailModel.filter(function (item) {
                    return item.eventAltid == eventInformation.altId;
                })
                if (userData[0].firstName != "" && userData[0].firstName != undefined) {
                    firstName = userData[0].firstName;
                }
                if (userData[0].lastName != "" && userData[0].lastName != undefined) {
                    lastName = userData[0].lastName;
                }
                if (userData[0].email != "" && userData[0].email != undefined) {
                    email = userData[0].email;
                }
                if (userData[0].phoneCode != "" && userData[0].phoneCode != undefined) {
                    phoneCode = userData[0].phoneCode;
                }
                if (userData[0].phoneNumber != "" && userData[0].phoneNumber != undefined) {
                    phoneNumber = userData[0].phoneNumber;
                }
                var symbol = getSymbolFromCurrency(eventInformation.currencyName);
                if (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') {
                    var cartItems = JSON.parse(localStorage.getItem("cartItems"));
                    var deliveryOptions, defaultDelivery = true;
                    var CartData = cartItems.filter(function (item) {
                        return item.altId == eventInformation.altId
                    })
                    if (CartData[0].deliveryOptions.length == 0) {
                        deliveryOptions = <div><div className="custom-control custom-radio">
                            <input type="radio" id={"optradio" + eventInformation.altId + "1"} onClick={that.setPickupOption.bind(that, eventInformation.altId, 2)} name={eventInformation.altId} className="custom-control-input" /> <label className="custom-control-label" htmlFor={"optradio" + eventInformation.altId + "1"}> <small>Venue Pickup</small> </label>
                        </div >
                            <div className="custom-control custom-radio">
                                <input type="radio" id={"optradio" + eventInformation.altId + "2"} onClick={that.setPickupOption.bind(that, eventInformation.altId, 3)} name={eventInformation.altId} className="custom-control-input" /> <label className="custom-control-label" htmlFor={"optradio" + eventInformation.altId + "2"}> <small>PrintAtHome</small> </label>
                            </div >
                            <div className="custom-control custom-radio">
                                <input type="radio" id={"optradio" + eventInformation.altId + "3"} onClick={that.setPickupOption.bind(that, eventInformation.altId, 4)} name={eventInformation.altId} className="custom-control-input" /> <label className="custom-control-label" htmlFor={"optradio" + eventInformation.altId + "3"}> <small>MTicket</small> </label>
                            </div >
                        </div>
                    }
                    else {
                        deliveryOptions = CartData[0].deliveryOptions.map(function (item, indx) {
                            var deliveryId;
                            if (item == "Courier") {
                                deliveryId = 1;
                            }
                            if (item == "VenuePickup") {
                                deliveryId = 2;

                            }
                            if (item == "PrintAtHome") {
                                deliveryId = 3;

                            }
                            if (item == "MTicket") {
                                deliveryId = 4;
                            }
                            return <div className="custom-control custom-radio">
                                <input type="radio" id={"optradio" + eventInformation.altId + indx} onClick={that.setPickupOption.bind(that, eventInformation.altId, deliveryId)} name={eventInformation.altId} className="custom-control-input" />
                                <label className="custom-control-label" htmlFor={"optradio" + eventInformation.altId + indx}> <small>{item}</small> </label>
                            </div>
                        });

                    }


                    var date = new Date(eventInformation.selectedDate).toDateString().split(' ').join(', ').substring(0, 8) + " " + new Date(eventInformation.selectedDate).toDateString().split(' ').join(', ').substring(9);
                    return <div>
                        <div className="media">
                            <img className="mr-3 d-none d-sm-block" src={`${that.state.s3BaseUrl}/places/tiles/` + eventInformation.altId.toUpperCase() + `-ht-c1.jpg`}
                                onError={(e) => {
                                    e.currentTarget.src = `${that.state.s3BaseUrl}/places/tiles/${eventInformation.subCategory}-placeholder.jpg`
                                }} alt="Generic placeholder image" width="120" />
                            <div className="media-body">
                                <p className="mb-2">{eventInformation.name}
                                    {(!categories[0].isItinerary && eventInformation.selectedDate) && <span className="pink-color pl-2"><i className="fa fa-calendar"></i> {date + " | " + numeral(new Date(eventInformation.selectedDate).getHours()).format('00') + ":" + numeral(new Date(eventInformation.selectedDate).getMinutes()).format('00')}</span>}
                                    {(categories[0].isItinerary) && <span className="pink-color pl-2"><i className="fa fa-calendar"></i> {date + " | " + (categories[0].visitStartTime.split(":").length > 2 ? (categories[0].visitStartTime.split(":")[0] + ":" + categories[0].visitStartTime.split(":")[1]) : "") + " - " + (categories[0].visitEndTime.split(":").length > 2 ? (categories[0].visitEndTime.split(":")[0] + ":" + categories[0].visitEndTime.split(":")[1]) : "")}</span>}
                                    <a role="button" data-toggle="collapse" href={"#collapseExample" + eventInformation.altId.toUpperCase()} aria-expanded="false" aria-controls="collapseExample"><small className="blue-txt ml-2">Show Details</small></a> <span className="float-sm-right">{symbol + numeral(categoryCost).format('0.00') + ' ' + eventInformation.currencyName}</span></p>
                                <div className="gradient-bg rounded p-2 border">
                                    {(deliveryOptions != undefined) && <div>{deliveryOptions}</div>}
                                </div>
                                <div id={"collapseExample" + eventInformation.altId.toUpperCase()} className="collapse delivery-more-detail">
                                    <div id={"show-details" + eventInformation.eventTicketAttributeId} className="text-muted pt-3 pb-3">
                                        <p className="m-0"><small><i className="fa fa-map-marker pink-color"></i> {eventInformation.venue} -  <Link to={"https://www.google.com/maps/search/" + eventInformation.venue} target="_blank"><i className="fa fa-map-marker mr-1"></i>View on Map</Link> </small></p>
                                        {categoryData}
                                    </div>
                                </div>
                            </div>
                        </div>
                        {altId != unique[unique.length - 1] && < hr />}
                        <div> {dict[eventInformation.altId] === true &&
                            <div>
                                <section className="container pt-3 pb-3">
                                    <div className="row">
                                        <div className="col-md-6">
                                            <div className="custom-control custom-radio">
                                                <div className="mr-3 mb-2">
                                                    <input type="radio" id={"pickupradio" + eventInformation.altId + 1} onClick={that.venuePickupOptions.bind(that, 0, eventInformation.altId)} name={"pickupradio" + eventInformation.altId} className="custom-control-input" checked={!venuePickup[eventInformation.altId]} />
                                                    <label className="custom-control-label" htmlFor={"pickupradio" + eventInformation.altId + 1}> <small>Self Pickup</small> </label>
                                                </div>
                                                <div className="mr-3 mb-4">
                                                    <input type="radio" id={"pickupradio" + eventInformation.altId + 2} onClick={that.venuePickupOptions.bind(that, 1, eventInformation.altId)} name={"pickupradio" + eventInformation.altId} className="custom-control-input" checked={venuePickup[eventInformation.altId]} />
                                                    <label className="custom-control-label" htmlFor={"pickupradio" + eventInformation.altId + 2}> <small>Other Person</small> </label>
                                                </div>
                                            </div>

                                            {(venuePickup[eventInformation.altId]) && <div>
                                                <h6 >Pickup Contact</h6>
                                                <p>The billing contact for this order will pick up these items.
                Please provide a mobile number to receive Pickup Notification text messages.</p>
                                                <div className="form-row">
                                                    <div className="form-group col-md-6">
                                                        <input type="text" name="firstName" className="form-control" value={firstName} disabled={!venuePickup[eventInformation.altId]} placeholder="First Name" required onChange={that.handleFirstNameChange.bind(that, eventInformation.altId)} />
                                                    </div>
                                                    <div className="form-group col-md-6">
                                                        <input type="text" name="lastName" className="form-control" value={lastName} disabled={!venuePickup[eventInformation.altId]} placeholder="Last Name" required onChange={that.handleLastNameChange.bind(that, eventInformation.altId)} />
                                                    </div>
                                                </div>
                                                <div className="form-group">
                                                    <input type="email" name="email" className="form-control" value={email} disabled={!venuePickup[eventInformation.altId]} placeholder="Email" required onChange={that.handleEmailNameChange.bind(that, eventInformation.altId)} />
                                                </div>
                                                <div className="form-row">
                                                    <div className="form-group col-md-4">
                                                        <select onChange={that.handleCountryChange.bind(that, eventInformation.altId)} disabled={!venuePickup[eventInformation.altId]} value={phoneCode} name="carlist" className="form-control" >
                                                            <option>Select Country</option>
                                                            {that.props.countryData.countries.map((item) => {
                                                                return <option value={item.altId}>{item.name + " (" + item.phonecode + ")"}</option>
                                                            })}
                                                        </select>
                                                    </div>
                                                    <div className="form-group col-md-8">
                                                        <input type="number" name="phoneNumber" className="form-control" disabled={!venuePickup[eventInformation.altId]} value={phoneNumber} placeholder="Mobile Number" required onChange={that.handlePhoneChange.bind(that, eventInformation.altId)} />
                                                    </div>
                                                </div>{unqdex > 0 && copy[eventInformation.altId] == true &&
                                                    <div className="form-row">
                                                        <div className="form-group col-md-4">
                                                            <input type="checkbox" onChange={that.CopyAbove.bind(that, unqdex)} />
                                                            &nbsp; Copy above
                                                    </div>
                                                    </div>}
                                            </div>}
                                        </div>
                                        <div className="col-md-6">
                                            <h6>Pickup Instructions</h6>
                                            <ul>
                                                <li>Exact ticket pickup location at the venue and the timing will be emailed to you and will also be announced in the "Customer Update" section of website closer to the event. Please check that regularly.</li>
                                                <p className="mt-3"><b>The following documents are compulsory for ticket pickup:</b></p>
                                                <ul>
                                                    <li>A: The card / bank account owner's original Govt. issued photo ID, along with a clean, fully legible, photocopy of the same ID.</li>
                                                    <li> B: When a debit or credit card has been used for purchase, we also need the original debit/credit card, along with a clean, fully legible, photocopy of the same card.</li>
                                                    <li>If sending someone else on behalf of the card holder / bank account owner, then we need numbers A and B above (originals and photocopies as mentioned) along with the following below</li>
                                                    <li>This is required even if the representative's name has been entered into the system when buying.</li>
                                                    <li>An authorization letter with the name of the representative, signed by the card holder/bank account owner A Govt issued photo ID of the representative, along with a clean and legible photocopy of the same photo identification</li>
                                                </ul>
                                                <li className="mt-3">Please note, absence of any one of the documents above can result in the tickets being refused at the ticket pickup window.</li>
                                            </ul>
                                        </div>
                                    </div>
                                </section>{unqdex < unique.length - 1 &&
                                    < hr />}
                            </div>
                        }</div>
                    </div>
                }
            });
            return <div>
                <div className="card-body">
                    {deliveryPickupData}
                </div>

                {this.props.showButtons && <div className="gradient-bg border p-2 text-right">
                    <Link to="/itinerary" className="btn btn-primary float-left text-uppercase">Back to Bag</Link>
                    {/* <a href="javascript:void(0);" onClick={this.continueToPickupDetails.bind(this)} className="btn site-primery-btn text-uppercase">Continue</a> */}
                    <Button type="submit" onClick={this.continueToPickupDetails.bind(this)} className="btn site-primery-btn text-uppercase">Continue</Button>
                </div>}
            </div>;
        } else {
            return <div>
            </div>;
        }
    }
}