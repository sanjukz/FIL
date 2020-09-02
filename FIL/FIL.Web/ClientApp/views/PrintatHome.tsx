import { autobind } from "core-decorators";
import * as numeral from "numeral";
import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import { IApplicationState } from "../stores";
import * as PrinntPAHStore from "../../ClientApp/stores/PrintPAH";
import "../views/PAH.scss";

var months = ['Janaury', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
var daysInWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
var ticketCount = 0, iflag = 0, imageDataUrl = [];

type PrinntPAHComponentProps =
    PrinntPAHStore.IPrintPAHComponentPropsState
    & typeof PrinntPAHStore.actionCreators
    & RouteComponentProps<{ transactionId: string; }>;
var transsactionId = "0";

class PrintAtHome extends React.Component<PrinntPAHComponentProps, PrinntPAHStore.IPrintPAHComponentPropsState> {

    public componentDidMount() {
        this.props.printPAHData(this.props.match.params.transactionId);
        transsactionId = this.props.match.params.transactionId;
    }

    public parseDateLocal(s) {
        var b = s.split(/\D/);
        return new Date(b[0], b[1] - 1, b[2], (b[3] || 0),
            (b[4] || 0), (b[5] || 0), (b[6] || 0));
    }

    @autobind
    public exportToPDF() {
        var jsPDF = require('jspdf');
        var that = this;
        const doc = new jsPDF("p", "pt", "a4");
        iflag = iflag + 1;
        if (iflag <= ticketCount) {
            var id = "PAH" + iflag;
            var node = document.getElementById(id);
            var domtoimage = require('dom-to-image');
            domtoimage.toPng(node)
                .then(function (dataUrl) {
                    imageDataUrl.push(dataUrl);
                    that.exportToPDF();
                })
                .catch(function (error) {
                    console.error('oops, something went wrong!', error);
                });
        }
        else {
            for (var i = 0; i < imageDataUrl.length; i++) {
                doc.addImage(imageDataUrl[i], 'PNG', 3, 12);
                if (i < imageDataUrl.length - 1)
                    doc.addPage();
            }
            doc.save("printathome.pdf");
            iflag = 0;
            imageDataUrl = [];
        }
    }
    public render() {
        var ticket, catgoryWiseTickets, categoryWiseData;
        var that = this;
        if (this.props.pahDetail != undefined) {
            if (this.props.pahDetail.pahDetail.length > 0) {
                categoryWiseData = this.props.pahDetail.pahDetail[0].categoryWiseTickets;
                if (categoryWiseData != undefined && categoryWiseData.length > 0) {
                    catgoryWiseTickets = categoryWiseData.map(function (item, index) {
                        return <span style={{ "marginRight": "10px", "display": "inline-block" }}>{item.categoryName} {item.totalTickets}{categoryWiseData.length - 1 > index ? ", " : ""}</span>
                    });
                }
            }
            ticket = this.props.pahDetail.pahDetail.map(function (item, index) {
                ticketCount = that.props.pahDetail.pahDetail.length;
                var targetRef = 'localhost:53000/pah/' + item.transactionId;
                var currentdate = new Date(item.eventStartTime);
                currentdate.setHours(currentdate.getHours() + 5);
                currentdate.setMinutes(currentdate.getMinutes() + 30);

                return <div className="content px-4 py-3">
                    <div className="row">
                        <div className="col-6">
                            <img src={require("../images/logo-feel.png")} className="img-fluid f-logo" alt="feel logo" />
                        </div>
                        <div className="col-6 text-right text-etkt">
                            e-ticket
                    </div>
                    </div>

                    <div className="ticket-bg border border-prpl rounded-lg mt-2 py-2 px-3 py-md-3 px-md-4">
                        <span className="place mb-2 font-weight-bold">{item.eventName}</span>
                        <span className="address mb-1 mb-md-2">
                            {item.venueName}	<br />
                            {item.cityName} {item.countryName}
                        </span>
                        <span className="date mb-2 mb-md-3">{daysInWeek[currentdate.getDay()]} | {months[currentdate.getMonth()]} {currentdate.getDate()}, {currentdate.getFullYear()}
                            -{item.timeSlot && item.timeSlot != "" && item.timeSlot != null ? item.timeSlot : (currentdate.getHours()) + ":" + numeral(currentdate.getMinutes()).format('00')}  </span>
                        <span className="category font-weight-bold">{item.ticketCategoryName}</span>
                        {catgoryWiseTickets != undefined && <span className="date mb-2 mb-md-3">{catgoryWiseTickets} </span>}
                        <span className="price font-weight-bold">{item.currencyName} {numeral(item.price).format('0.00')}
                        </span>
                    </div>

                    <div className="qr-code">
                        <div className="row">
                            <div className="col-sm-5 text-uppercase font-weight-bold">
                                This is your ticket	<br />
                                Do Not Duplicate	<br />
                                {catgoryWiseTickets != undefined ? "" : "One Entry Per Barcode"}
                            </div>
                            <div className="col-sm-2 text-center">
                                <img src={`https://chart.googleapis.com/chart?cht=qr&chl=${item.barcodeNumber}&chs=170x170&chld=L|0`} className="img-fluid" alt="barcode" />
                                <h6 className="mt-2">{item.barcodeNumber}</h6>
                            </div>
                            <div className="col-sm-5 text-right">
                                <span className="text-capitalize font-weight-bold g-name">{item.firstName} {item.lastName}</span>
                                <span>10500183250032-F{item.transactionId}</span>
                                <span className="font-weight-bold">Ticket {index + 1}/{that.props.pahDetail.pahDetail.length}</span>
                            </div>
                        </div>
                    </div>
                    <div className="qr-code-mob">
                        <div className="text-center qr-img">
                            <img src={`https://chart.googleapis.com/chart?cht=qr&chl=${item.barcodeNumber}&chs=170x170&chld=L|0`} className="img-fluid" alt="barcode" />
                        </div>
                        <div className="text-uppercase text-center">
                            <b>This is your ticket. Do Not Duplicate</b> <a href="#tnc" data-toggle="collapse" ><img src="images/tnc.png" className="tnc-icon" alt="tnc" /></a>
                        </div>
                        <hr />
                        <div className="row">
                            <div className="col-12 col-sm-5 text-capitalize font-weight-bold g-name">
                                {item.firstName} {item.lastName}
                            </div>
                            <div className="col-6 col-sm-4 text-uppercase text-left text-sm-right">
                                10500183250032-{item.transactionId}
                            </div>
                            <div className="col-6 col-sm-3 text-right">
                                Ticket {index + 1}/{that.props.pahDetail.pahDetail.length}
                            </div>
                        </div>
                    </div>


                    <div id="tnc" className="collapse">
                        <div className="tnc border border-secondary">
                            <p className="font-weight-bold mb-2">1. Entry on this ticket is valid only for one person unless specified.     <br /><br />
                                2.Tickets once purchased are non-transferable.     <br /><br />
                                3.The consumption and sale of illegal substances is strictly prohibited     <br /><br />
                                4.The rights of admission are reserved with the organizers.     <br /><br />
                                5.The organizers reserve the right to frisk the ticket holders at the entry point for security reasons. Your cooperation is solicited.     <br /><br />
                                6.No drugs, alcohol, cigarettes or lighters shall be allowed inside the venue. The venue is a no-smoking area.     <br /><br />
                                7.No food or beverage from outside is allowed     <br /><br />
                                8.Cameras, handbags, backpacks, bottles, cans or tins are not allowed inside the venue     <br /><br />
                                9.The organizers are not liable for any type of injury suffered by the ticket holder while attending the place.     <br /><br />
                                10.The organizers, the venue and ticketing company shall not be held liable for any difficulties caused by unauthorized copy or reproduction of this ticket.     <br /><br />
                                11.All pagers and cell phones from outside must be switched off.     <br /><br />
                                12.Entry will not be allowed for those holding a tampered ticket.     <br /><br />
                                13.The ticket is issued in accordance to the rules and regulations of the place.     <br /><br />
                                14.The starting time of the place may change without prior intimation.     <br /><br />
                                15.The organizers, the venue or ticketing company shall not be held liable on cancellation of the place due to calamities/or as directed by the Government Authorities.     <br /><br /></p>
                            <p className="text-justify mb-0">

                            </p>
                        </div>
                    </div>

                    <div className="bottom">



                        <div className="row">
                            <div className="col-sm-5">
                                <p className="mb-2">
                                    <span className="font-weight-bold">{item.firstName} {item.lastName}</span>
                                    <span>10500183250032-F{item.transactionId}</span>
                                    <span className="font-weight-bold">Ticket {index + 1}/{that.props.pahDetail.pahDetail.length}</span>
                                </p>
                            </div>
                            <div className="col-sm-5">
                                <p className="mb-2">
                                    <span className="font-weight-bold">{item.eventName}</span>
                                    <span className="address mb-1">
                                        {item.venueName}	<br />{item.cityName} {item.countryName}
                                    </span>
                                    <span>{daysInWeek[currentdate.getDay()]} | {months[currentdate.getMonth()]} {currentdate.getDate()}, {currentdate.getFullYear()} - {item.timeSlot && item.timeSlot != "" && item.timeSlot != null ? item.timeSlot : (currentdate.getHours()) + ":" + numeral(currentdate.getMinutes()).format('00')}  </span>
                                </p>
                                <span className="text-capitalize font-weight-bold">{item.ticketCategoryName}</span>
                                {catgoryWiseTickets != undefined && <span className="text-capitalize">{catgoryWiseTickets} </span>}
                            </div>
                            <div className="col-sm-2 text-right">
                                <img src="https://chart.googleapis.com/chart?cht=qr&chl=${item.barcodeNumber}&chs=170x170&chld=L|0" style={{ width: "70px", height: "70px" }} />
                            </div>
                        </div>
                        <div className="cutter">
                            <p className="icon">&#9986;</p>
                        </div>
                    </div>
                </div>
            });
        }


        return <div>

            <div id="PAH">
                <div className="container">
                    <div className="border border-secondary">
                        {ticket}
                    </div>
                    <button className="export-btn" id="download" onClick={this.exportToPDF.bind(this)}> Download Pdf </button>

                </div>
            </div>
        </div>
    }
}
export default connect(
    (state: IApplicationState) => state.reprintRequest,
    PrinntPAHStore.actionCreators
)(PrintAtHome);