import * as React from "react";
import { IApplicationState } from "../stores";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import { bindActionCreators } from "redux";
import * as TicketCategoryPageStore from "../stores/TicketCategory";
import * as RegisterStore from "../stores/Register";
import { GuestUserDetails } from "../models/Cart/CartDataViewModel";

const SAARCNations = ['Afghanistan', 'Bangladesh', 'Bhutan', 'Maldives', 'Nepal', 'Pakistan', 'Sri Lanka'];
const BIMSTECNations = [' Bangladesh', 'Bhutan', 'Myanmar', 'Nepal', 'Sri Lanka', 'Thailand'];

type ItineraryGuestDetailsProps =
    RegisterStore.IRegisterState
    & typeof RegisterStore.actionCreators
    & RouteComponentProps<{ transactionAltId: string; }>;

class ItineraryGuestDetails extends React.Component<ItineraryGuestDetailsProps, any> {
    state = {
        guestDetails: [],
        currentguestIndex: 0,
        ageExceedError: "",
        childQuantity: 0,
        adultQuantity: 0,
        guestType: [],
        countryList: []
    }

    componentDidMount() {
        this.props.requestCountryData();
        var that = this;
        var guestData = [];
        var guestType = [];
        var cartItems = JSON.parse(localStorage.getItem("cartItems"));
        var adultQty = this.state.adultQuantity;
        var childQty = this.state.childQuantity;
        if (cartItems.length > 0) {
            for (var i = 0; i < cartItems.length; i++) {
                if (cartItems[i].guestList.length == 0) {
                    for (var j = 0; j < cartItems[i].quantity; j++) {
                        var guest1 = this.getDefaultModel("", false);
                        guestData.push(guest1)
                        guestType.push(cartItems[i].ticketCategoryName)
                    }

                    if (cartItems[i].isItinerary && cartItems[i].ticketCategoryName.indexOf("Adult") > -1) {
                        adultQty = cartItems[i].quantity;
                    }

                    if (cartItems[i].isItinerary && cartItems[i].ticketCategoryName.indexOf("Child") > -1) {
                        childQty = cartItems[i].quantity;
                    }

                } else {
                    for (var j = 0; j < cartItems[i].quantity; j++) {
                        var guest1 = this.getDefaultModel(cartItems[i].guestList[j], true);
                        guestData.push(guest1)
                        guestType.push(cartItems[i].ticketCategoryName)
                    }

                    if (cartItems[i].isItinerary && cartItems[i].ticketCategoryName.indexOf("Adult")) {
                        adultQty = cartItems[i].quantity;
                    }

                    if (cartItems[i].isItinerary && cartItems[i].ticketCategoryName.indexOf("Child")) {
                        childQty = cartItems[i].quantity;
                    }
                }
            }
        }

        if (cartItems[0].isItinerary) {
            guestData.splice(adultQty + childQty),
                guestType.splice(adultQty + childQty)
        }

        that.setState({
            adultQuantity: adultQty,
            childQuantity: childQty,
            guestDetails: guestData,
            guestType: guestType
        })

    }



    onChangeGuest = (e) => {
        e.preventDefault();
        if (this.state.guestDetails.length == this.state.currentguestIndex + 1) {
            this.saveToLocalStorage();
        } else {
            this.setState({ currentguestIndex: this.state.currentguestIndex + 1, ageExceedError: "" });
        }
    }

    saveToLocalStorage = () => {
        var cartItems = JSON.parse(localStorage.getItem("cartItems"));
        var guest = this.state.guestDetails
        var childGuest = []
        var countryGust = []
        if (cartItems[0].isItinerary) {
            childGuest = guest.splice(this.state.adultQuantity)
        }

        for (var i = 0; i < cartItems.length; i++) {
            if (cartItems[i].isItinerary) {
                if (cartItems[i].ticketCategoryName.indexOf("Adult") > -1) {
                    cartItems[i].guestList = guest;
                } else {
                    cartItems[i].guestList = childGuest;
                }
            } else {
                countryGust = guest.splice(cartItems[i].quantity)
                cartItems[i].guestList = guest;
                guest = countryGust
            }
        }

        localStorage.setItem("cartItems", JSON.stringify(cartItems));
        this.props.history.push(`/delivery-options/${this.props.match.params.transactionAltId}`);
    }

    onChangeGender = (e) => {
        var guest = this.state.guestDetails;
        var that = this;
        guest.map(function (item, currentIndex) {
            if (that.state.currentguestIndex == currentIndex) {
                item.gender = +e.target.value;
            }
        });
        this.setState({ guestDetails: guest });

    }

    onChangeFirstName = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.firstName = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangeLastName = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.lastName = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangeAge(index, e) {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.age = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangePhoneCode = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.phoneCode = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangePhoneNumber = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.phoneNumber = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangeEmail = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.email = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangeCountry = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.country = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangeDocumentType = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.identityType = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    onChangeDocumentTypeNumber = (index, e) => {
        var guest = this.state.guestDetails;
        guest.map(function (item, currentIndex) {
            if (index == currentIndex) {
                item.identityNumber = e.target.value;
            }
        });
        this.setState({ guestDetails: guest });
    }

    getDefaultModel = (val, flag) => {
        var guest1: GuestUserDetails = {
            age: flag ? val.age : 0,
            email: flag ? val.email : "",
            firstName: flag ? val.firstName : "",
            gender: flag ? val.gender : 1,
            identityNumber: flag ? val.identityNumber : "",
            identityType: flag ? val.identityType : 1,
            lastName: flag ? val.lastName : "",
            phoneCode: flag ? val.phoneCode : "",
            phoneNumber: flag ? val.phoneNumber : "",
            country: flag ? val.country : ""
        };
        return guest1;
    }

    onChangePreviousGuest = (e) => {
        e.preventDefault();
        this.setState({ currentguestIndex: this.state.currentguestIndex - 1 });
    }

    render() {
        if (this.props.fetchCountriesSuccess && this.state.guestDetails.length > 0) {
            var countryList = this.props.countryList.countries
            var index = this.state.currentguestIndex;
            var phoneCode = countryList.filter(item => item.phonecode && item.name).map((item) => <option selected={this.state.guestDetails[index].phoneCode == item.altId ? true : false} value={item.altId}>{item.name + " (" + item.phonecode + ") "}</option>)
            var country = countryList.filter(item => item.name).map((item) => <option value={item.altId} selected={this.state.guestDetails[index].country == item.altId ? true : false}>{item.name}</option>)
            var indianCountry = countryList.filter(item => item.name == "India")
            var saarcCountry = countryList.filter(item => { return SAARCNations.indexOf(item.name) != -1 })
            var bimstecCountry = countryList.filter(item => { return BIMSTECNations.indexOf(item.name) != -1 })
            return (
                <div className="container">

                    <div className="card my-5">
                        <div className="card-header">
                            Customer details
                    </div>
                        <div className="card-body">

                            {(this.state.guestDetails.length > 0) && <div className="font-weight-bold mb-2">Guest {index + 1} of {this.state.guestDetails.length}
                                {(this.state.guestDetails.length > 0) && <div className="pull-right">{this.state.guestType[this.state.currentguestIndex]}</div>} </div>}
                            {this.state.guestDetails.length > 0 && <form onSubmit={this.onChangeGuest} className="form-group mb-2 needs-validation">
                                <div className="form-row">
                                    <div className="form-group col-sm-4 pb-2">
                                        First Name* <input
                                            type="text"
                                            className="form-control"
                                            value={this.state.guestDetails[index].firstName}
                                            onChange={this.onChangeFirstName.bind(this, index)}
                                            required />
                                    </div>
                                    <div className="form-group col-sm-4 pb-2">
                                        Last Name* <input
                                            type="text"
                                            className="form-control"
                                            value={this.state.guestDetails[index].lastName}
                                            onChange={this.onChangeLastName.bind(this, index)}
                                            required />
                                    </div>
                                    <div className="form-group col-sm-4 pb-2">
                                        <div>Phone Number*</div>
                                        <div className="input-group">
                                            <div className="input-group-prepend" style={{ maxWidth: "110px" }}>
                                                <select
                                                    className="form-control"
                                                    onChange={this.onChangePhoneCode.bind(this, index)} required>
                                                    <option value="" selected disabled>Code</option>
                                                    {(this.state.guestType[this.state.currentguestIndex].includes("Indian")) ?
                                                        indianCountry.map((item) => <option selected={this.state.guestDetails[index].phoneCode == item.altId ? true : false} value={item.altId}>{item.isoAlphaTwoCode + " (" + item.phonecode + ") "}</option>) : this.state.guestType[this.state.currentguestIndex].includes("SAARC") ? saarcCountry.map((item) => <option selected={this.state.guestDetails[index].phoneCode == item.altId ? true : false} value={item.altId}>{item.isoAlphaTwoCode + " (" + item.phonecode + ") "}</option>) : this.state.guestType[this.state.currentguestIndex].includes("BIMSTEC") ? bimstecCountry.map((item) => <option selected={this.state.guestDetails[index].phoneCode == item.altId ? true : false} value={item.altId}>{item.isoAlphaTwoCode + " (" + item.phonecode + ") "}</option>) : countryList.filter(item => item.name).map((item) => <option selected={this.state.guestDetails[index].phoneCode == item.altId ? true : false} value={item.altId}>{item.isoAlphaTwoCode + " (" + item.phonecode + ") "}</option>)}
                                                </select>
                                            </div>
                                            <input type="number" className="form-control" onChange={this.onChangePhoneNumber.bind(this, index)} value={this.state.guestDetails[index].phoneNumber} />
                                        </div>
                                    </div>
                                </div>
                                <div className="form-row">
                                    <div className="form-group col-sm-4">Email*
                                        <input type="email"
                                            required
                                            className="form-control"
                                            onChange={this.onChangeEmail.bind(this, index)}
                                            value={this.state.guestDetails[index].email} />
                                    </div>
                                    <div className="form-group col-sm-4">
                                        Country of origin
                                        <select
                                            className="form-control"
                                            onChange={this.onChangeCountry.bind(this, index)}
                                        >
                                            <option value="" selected disabled>Select country</option>
                                            {(this.state.guestType[this.state.currentguestIndex].includes("Indian")) ?
                                                indianCountry.map((item) => <option value={item.altId} selected={this.state.guestDetails[index].country == item.altId ? true : false}>{item.name}</option>) : this.state.guestType[this.state.currentguestIndex].includes("SAARC") ? saarcCountry.map((item) => <option value={item.altId} selected={this.state.guestDetails[index].country == item.altId ? true : false}>{item.name}</option>) : this.state.guestType[this.state.currentguestIndex].includes("BIMSTEC") ? bimstecCountry.map((item) => <option value={item.altId} selected={this.state.guestDetails[index].country == item.altId ? true : false}>{item.name}</option>) : countryList.filter(item => item.name).map((item) => <option value={item.altId} selected={this.state.guestDetails[index].country == item.altId ? true : false}>{item.name}</option>)}
                                        </select>
                                    </div>
                                </div>
                                <hr />
                                {(this.state.ageExceedError != "") && <div className="alert alert-danger small p-2 mt-2">{this.state.ageExceedError}</div>}
                                <div className="text-center">
                                    {(index > 0) && <input type="button" className="btn btn-secondary mx-2" name="action" onClick={this.onChangePreviousGuest} value="Previous" />}
                                    <input type="submit" className="btn btn-primary mx-2" value={(index + 1 == this.state.guestDetails.length) ? "Submit" : "Next"} />
                                </div>
                            </form>}
                        </div>
                    </div>
                </div>
            )
        } else {
            return (<div>
                <div className="pt-2 pb-3 text-center">
                    <div className="loader-content">
                        <i className="fa fa-circle-o-notch fa-spin" />
                        <p style={{ fontSize: '13px' }}> Please wait... </p>
                    </div>
                </div>
            </div>)
        }
    }

}

export default connect(
    (state: IApplicationState) => state.register,
    (dispatch) => bindActionCreators({
        ...RegisterStore.actionCreators
    }, dispatch)
)(ItineraryGuestDetails);