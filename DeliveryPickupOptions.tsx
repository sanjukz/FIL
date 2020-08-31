import * as React from "react";
import { Link, RouteComponentProps } from "react-router-dom";
import { Grid, Row, Col, Button, Alert } from "react-bootstrap";
import * as numeral from "numeral";
import { UserDeliveryDetailFormDataViewModel } from "../../models/DeliveryOptions/UserDeliveryDetailFormDataViewModel";
import { ReactHTMLConverter } from "react-html-converter/node";

export default class DeliveryPickupOptions extends React.Component<any, any> {
    public constructor() {
        super();
        this.state = {
            cartData: "",
            currentAltId: "",
            isStripShow: true,
            isSelectVenueShow: false,
            selectedVenue: false,
            isChecked1: false,
            isChecked2: false,
            isChecked3: false,
            isChecked4: false,
        };
    }

    public componentDidMount() {
        if (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') {
            var data = JSON.parse(localStorage.getItem("cartItems"));
            let that = this;
            data.forEach(function (val) {
                //that.setPickupOption(val.eventTicketAttributeId, "0");
                that.setState({ [val.eventTicketAttributeId]: "0" });
            });
            if (data[0].altId == "D1A3F409-ED38-4488-BA84-68AC54D23BB8" || data[0].altId == "9A3B3708-5FC5-4AB7-8E53-133602E0D1A0") {
                this.setState({ cartData: data, isSelectVenueShow: true, currentAltId: data[0].altId });
            } else {
                this.setState({ cartData: data, isSelectVenueShow: false });
            }

        }
    }

    public continueToPickupDetails() {
        let data = this.state.cartData;
        let that = this;
        let choosedPickup = data.filter(function (val) {
            return that.state[val.eventTicketAttributeId] == "2";
        });
        let choosedPah = data.filter(function (val) {
            return that.state[val.eventTicketAttributeId] == "3";
        });
        if (((choosedPickup.length == data.length || choosedPah.length == data.length)) || (this.state.isSelectVenueShow && this.state.selectedVenue)) {
            if (choosedPickup.length == data.length) {
                this.props.switchPage("pickupDetails");
            } else {
                var userDetails: UserDeliveryDetailFormDataViewModel;
                userDetails = JSON.parse(localStorage.getItem('userData'));
                this.props.onSubmitPickupDetails(userDetails);
            }
        } else {
            alert("Please select delivery option for each item in your shopping bag");
        }
    }

    public setPickupOption(item, value, j, e) {
        this.setState({
            [item]: value
        });
        if (this.state.isSelectVenueShow) {
            if (j == 1) {
                this.setState({ isChecked1: true, isChecked2: false, isChecked3: false, isChecked4: false, isChecked5: false, isChecked6: false, isChecked7: false, isChecked8: false })
            } else if (j == 2) {
                this.setState({ isChecked1: false, isChecked2: true, isChecked3: false, isChecked4: false, isChecked5: false, isChecked6: false, isChecked7: false, isChecked8: false })
            } else if (j == 3) {
                this.setState({ isChecked1: false, isChecked2: false, isChecked3: true, isChecked4: false, isChecked5: false, isChecked6: false, isChecked7: false, isChecked8: false })
            } else if (j == 4) {
                this.setState({ isChecked1: false, isChecked2: false, isChecked3: false, isChecked4: true, isChecked5: false, isChecked6: false, isChecked7: false, isChecked8: false })
            } else if (j == 5) {
                this.setState({ isChecked1: false, isChecked2: false, isChecked3: false, isChecked4: false, isChecked5: true, isChecked6: false, isChecked7: false, isChecked8: false })
            } else if (j == 6) {
                this.setState({ isChecked1: false, isChecked2: false, isChecked3: false, isChecked4: false, isChecked5: false, isChecked6: true, isChecked7: false, isChecked8: false })
            } else if (j == 7) {
                this.setState({ isChecked1: false, isChecked2: false, isChecked3: false, isChecked4: false, isChecked5: false, isChecked6: false, isChecked7: true, isChecked8: false })
            } else if (j == 8) {
                this.setState({ isChecked1: false, isChecked2: false, isChecked3: false, isChecked4: false, isChecked5: false, isChecked6: false, isChecked7: false, isChecked8: true })
            }
            else { }

            this.setState({ selectedVenue: true })
            this.props.selectedVenue(e.target.value);
        }
    }

    public parseDateLocal(s) {
        var b = s.split(/\D/);
        return new Date(b[0], b[1] - 1, b[2], (b[3] || 0),
            (b[4] || 0), (b[5] || 0), (b[6] || 0));
    }

    public render() {
        var data = this.state.cartData;
        var that = this;
        if (data != "") {
            let i = 1;
            var j = 0;
            var deliveryPickupData = data.map(function (val, index) {
                if (i == 4) {
                    i = 1;
                }
                j = j + 1;
                const converter = ReactHTMLConverter();
                converter.registerComponent('Event', DeliveryPickupOptions);

                if (val.currencyName == "USD") {
                    val.currencyName = "US$";
                }

                return <div className="media col-xs-12">
                    <div className="media-left">
                        <a href="#">
                            <img className="media-object" src={`https://static${i++}.zoonga.com/Images/HotTicket/` + val.altId.toUpperCase() + "-ht.jpg"} alt="..." style={{ maxWidth: "150px" }} />
                        </a>
                    </div>
                    <div className="media-body text-muted">
                        <div className="row">
                            <div className="col-sm-10 col-md-9">
                                <h5 className="text-black mt-0">{val.name} - <span className="text-danger"> <i className="fa fa-calendar-check-o" aria-hidden="true"></i> {that.parseDateLocal(val.eventStartDate).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + that.parseDateLocal(val.eventStartDate).toDateString().split(' ').join(', ').toUpperCase().substring(9) + " | " + numeral(that.parseDateLocal(val.eventStartDate).getHours()).format('00') + ":" + numeral(that.parseDateLocal(val.eventStartDate).getMinutes()).format('00')} </span> | <small><a href="javascript:void(0);" className="text-info" data-toggle="collapse" data-target={"#show-details" + val.eventTicketAttributeId}>Show Details</a></small>
                                </h5>
                            </div>
                            <div className="col-sm-2 col-md-3 text-right mb-10 text-black">
                                {(val.currencyName === "USD" ? "US$" : val.currencyName) + " " + numeral(val.quantity * val.pricePerTicket).format('0.00')}
                            </div>
                        </div>
                        <div className="options mb-10 small">
                            {(!that.state.isSelectVenueShow) && <div>
                                <div className="row radio mt-0">
                                    <div className="col-sm-2 col-md-1 text-black">
                                        E-Ticket
											</div>
                                    <div className="col-sm-10 col-md-11">
                                        <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "3", 0)} name={"optradio" + val.eventTicketAttributeId} disabled={val.currencyName == "INR" && val.altId != "2689CAEC-770D-46E5-8444-347C7E265D7E"} />ZPass<sup>TM</sup> (Your confirmation email will contain the link to generate your ticket. You can generate E-ticket to print and bring to the venue.)
												</label>
                                    </div>
                                </div>
                                <div className="row radio mt-0">
                                    <div className="col-sm-2 col-md-1 text-black">
                                        Ship
											</div>
                                    <div className="col-sm-10 col-md-11">
                                        <label><input type="radio" name={"optradio" + val.eventTicketAttributeId} disabled />Courier
												</label>
                                    </div>
                                </div>
                                <div className="row radio mt-0 mb-0">
                                    <div className="col-sm-2 col-md-1 text-black">
                                        Pickup
											 </div>
                                    <div className="col-sm-10 col-md-11">
                                        <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 0)} disabled={val.currencyName != "INR" || val.altId == "2689CAEC-770D-46E5-8444-347C7E265D7E"} name={"optradio" + val.eventTicketAttributeId} />At Venue &nbsp;<i className="fa fa-map-marker" aria-hidden="true"></i>  <Link to={"https://www.google.com/maps/search/" + val.venue} target="_blank">Map</Link>
                                        </label>
                                    </div>
                                </div>
                            </div>}
                            {(that.state.isSelectVenueShow) && (that.state.currentAltId == "D1A3F409-ED38-4488-BA84-68AC54D23BB8") && <div className="row radio mt-0 mb-0">
                                <div className="col-sm-2 col-md-1 text-black">
                                    Venue Pickup
											 </div>
                                <div className="col-sm-10 col-md-11">
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 1)} checked={that.state.isChecked1} value="Box Office at Salt Lake Stadium gate number 1 (11 am to 5 pm)" />Box Office at Salt Lake Stadium gate number 1 (11 am to 5 pm)</label><br />
                                    <label> <input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 2)} checked={that.state.isChecked2} value="East Bengal club (11 am to 5 pm)" />East Bengal club (11 am to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 3)} checked={that.state.isChecked3} value="Box office at Barasat stadium (12 pm to 5 pm)" />Box office at Barasat stadium (12 pm to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 4)} checked={that.state.isChecked4} value="Lake town milan sangha, Near Jaya cinema (12 pm to 5 pm)" />Lake town milan sangha, Near Jaya cinema (12 pm to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 5)} checked={that.state.isChecked5} value="Jawpur Durga puja committee, jawpur, Dum Dum. Word number -18 (12 pm to 5 pm)" />Jawpur Durga puja committee, jawpur, Dum Dum. Word number -18 (12 pm to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 6)} checked={that.state.isChecked6} value="Bosepukur Sitala Mandir committee, bosepukur, Kasba (12 pm to 5 pm)" />Bosepukur Sitala Mandir committee, bosepukur, Kasba (12 pm to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 7)} checked={that.state.isChecked7} value="Naktala Udayan sangha (12 pm to 5 pm)" />Naktala Udayan sangha (12 pm to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 8)} checked={that.state.isChecked8} value="Behala Club, Behala area (12 pm to 5 pm)" />Behala Club, Behala area (12 pm to 5 pm)</label>
                                </div>
                            </div>}
                            {(that.state.isSelectVenueShow) && (that.state.currentAltId == "9A3B3708-5FC5-4AB7-8E53-133602E0D1A0") && <div className="row radio mt-0 mb-0">
                                <div className="col-sm-2 col-md-1 text-black">
                                    Venue Pickup
											 </div>
                                {(val.eventDetailId === 246874) && <div className="col-sm-10 col-md-11">
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 1)} checked={that.state.isChecked1} value="Mohun bagan ground PWD box office (From 24th to 26th Jan: 11 am to 5 pm)" />Mohun bagan ground PWD box office (From 24th to 26th Jan: 11 am to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 2)} checked={that.state.isChecked2} value="Box office at VYBK stadium gate number -1 (From 24th to 26th Jan: 11 am to 5 pm)" />Box office at VYBK stadium gate number -1 (From 24th to 26th Jan: 11 am to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 3)} checked={that.state.isChecked3} value="JC block childrens park, near Amul building (On 27th jan: 11 am to 5 pm)" />JC block childrens park, near Amul building (On 27th jan: 11 am to 5 pm)</label>
                                </div>
                                }
                                {(val.eventDetailId === 506664) && <div className="col-sm-10 col-md-11">
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 1)} checked={that.state.isChecked1} value="East bengal club (From 24th to 26th Jan: 11 am to 5 pm)" />East bengal club (From 24th to 26th Jan: 11 am to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 2)} checked={that.state.isChecked2} value="Box office at VYBK stadium gate number -4 (From 24th to 26th Jan: 11 am to 5 pm)" />Box office at VYBK stadium gate number -4 (From 24th to 26th Jan: 11 am to 5 pm)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 3)} checked={that.state.isChecked3} value="JC block childrens park, near Amul building (On 27th jan: 11 am to 5 pm)" />JC block childrens park, near Amul building (On 27th jan: 11 am to 5 pm)</label>
                                </div>
                                }
                                {(val.eventDetailId != 246874 && val.eventDetailId != 506664) && <div className="col-sm-10 col-md-11">
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 1)} checked={that.state.isChecked1} value="Mohun bagan Ground (For MB fan)" />Mohun bagan Ground (For MB fan)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 2)} checked={that.state.isChecked2} value="Box office at Salt lake stadium gate # 4 (For MB fan)" />Box office at Salt lake stadium gate # 4 (For MB fan)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 3)} checked={that.state.isChecked3} value="Stadium Box Office # 1 (For MB fan)" />Stadium Box Office # 1 (For MB fan)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 4)} checked={that.state.isChecked4} value="Mohun Bagan Club (For MB fan)" />Mohun Bagan Club (For MB fan)</label><br />
                                    <label><input type="radio" onClick={that.setPickupOption.bind(that, val.eventTicketAttributeId, "2", 5)} checked={that.state.isChecked5} value="JC Block Children Park (For MB fan)" />JC Block Children Park (For MB fan)</label>
                                </div>}
                            </div>}
                        </div>
                        <div id={"show-details" + val.eventTicketAttributeId} className="collapse small">
                            <hr className="mt-0 mb-10" />
                            <p><i className="fa fa-angle-right text-pink" aria-hidden="true"></i> {val.venue}  <span className="text-info"><i className="fa fa-map-marker" aria-hidden="true"></i>  <Link to={"https://www.google.com/maps/search/" + val.venue} target="_blank">View on Map</Link></span>
                            </p>
                            <p>{val.ticketCategoryName + " (" + val.quantity + " x " + (val.currencyName === "USD" ? "US$" : val.currencyName) + " " + numeral(val.pricePerTicket).format('0.00') + " )"}</p>
                            <p>{converter.convert(val.ticketCategoryDescription)}</p>
                            <p className="tnc text-info">
                                <i className="fa fa-file-text" aria-hidden="true"></i> <Link data-toggle="modal" to="#tnc" >Terms &amp; Conditions</Link>
                            </p>
                            <div className="modal fade" id="tnc" role="dialog">
                                <div className="modal-dialog">

                                    <div className="modal-content">
                                        <div className="modal-header">
                                            <button type="button" className="close" data-dismiss="modal">&times;</button>
                                            <h4 className="modal-title">Terms and Conditions</h4>
                                        </div>
                                        <div className="modal-body">

                                            {converter.convert(val.eventTermsAndConditions)}

                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            });
            return <div>
                <div className="row">
                    {deliveryPickupData}
                </div>
                {(this.state.isStripShow) && <div className="bg-gray-lightest p-10 mt-10 clearfix">
                    <Link to="/review-cart" className="btn btn-info hidden-xs">Back to Bag</Link>
                    <Link to="/review-cart" className="btn btn-block btn-info visible-xs mb-5">Back to Bag</Link>
                    <a href="javascript:void(0);" onClick={this.continueToPickupDetails.bind(this)} className="btn btn-success hidden-xs pull-right">Continue</a>
                    <a href="javascript:void(0);" onClick={this.continueToPickupDetails.bind(this)} className="btn btn-block btn-success visible-xs">Continue</a>
                </div>}
            </div>;
        } else {
            return <div>
            </div>;
        }
    }
}