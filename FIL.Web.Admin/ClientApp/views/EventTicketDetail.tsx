import { autobind } from "core-decorators";
import * as React from "react";
import { Button } from "react-bootstrap";
import AddEventTicketDetails from "../components/EventCreation/AddEventTicketDetails";
import GetTicketDetailViewModel from "../models/eventCreation/GetTicketDetailViewModel";
import { GetTicketDetailResponseViewModel } from "../models/EventCreation/GetTicketDetailResponseViewModel";
import FILLoader from "../components/Loader/FILLoader";

export default class EventTicketDetail extends React.Component<any, any> {
    constructor(props) {
        super(props)
        this.state = {
            valEvent: "",
            valCurrency: "",
            setView: true,
            ticketType: [],
            ChannelType: [],
            tickets: [],
            valueType: [],
            currencyType: [],
        }
    }

    public componentDidMount() {
        this.props.requesttickets();
        this.props.requestTicketType();
        this.props.requestChannelType();
        this.props.requestValueType();
        this.props.requestCurrencyType();
        var eventDetailId: GetTicketDetailViewModel = {
            eventDetailId: this.props.eventDetailId,
        };
        this.props.geEventTicketDetailData(eventDetailId, (response: GetTicketDetailResponseViewModel) => {
            if (response.eventTicketAttribute.length > 0 && response.eventTicketDetail.length > 0 && response.ticketCategory.length > 0 && response.ticketFeeDetail.length > 0) {
                this.setState({
                    IsShowcomponent: false,
                    Isaddmore:true
                })
            }
        });
    }

    @autobind
    public changeVal(e) {
        this.setState({ valEvent: e });
    }

    @autobind
    public changecurrencyVal(e) {
        this.setState({ valCurrency: e });
    }
   
    public render() {
        var tickets = this.state.tickets;
        var ChannelType = this.state.ChannelType;
        var ticketType = this.state.ticketType;
        var valueType = this.state.valueType;
        var currencyType = this.state.currencyType;
        var TicketDetail = this.props.getTicketDetailData;
        var ticketList = [];
        var TicketDataList;
        if (TicketDetail != null && TicketDetail !=undefined) {
            var eventTicketDetailList = TicketDetail.eventTicketDetail;
            var eventTicketAttributeList = TicketDetail.eventTicketAttribute;
            var ticketCategoryList = TicketDetail.ticketCategory;
            var ticketfeeDetail = TicketDetail.ticketFeeDetail;

            eventTicketDetailList.forEach(function (val) {
                var ticketCategory = ticketCategoryList.filter(function (et5) {
                    return et5.id == val.ticketCategoryId;
                });
                var eventTicketAttribute = eventTicketAttributeList.filter(function (et3) {
                    return et3.eventTicketDetailId == val.id;
                });

                var ticketFeeDetails = ticketfeeDetail.filter(function (et4) {
                    return et4.eventTicketAttributeId == eventTicketAttribute[0].id;
                });

                let newName = {
                    eventTicketAttribute: eventTicketAttribute[0].eventTicketDetailId,
                    ticketCategory: ticketCategory[0].name,
                    pricePerTicket: eventTicketAttribute[0].price,
                    ticketforsale: eventTicketAttribute[0].availableTicketForSale,
                    ticketDescription: eventTicketAttribute[0].ticketCategoryDescription,
                    convCharge: ticketFeeDetails[0].feeId
                };
                ticketList.push(newName);
            });
        }
        if (ticketList != null) {
            var that = this;
            TicketDataList = ticketList.map(function (val) {
                return <div className="form-group mb-2 position-relative">
                 <div className="form-control form-control-sm readonly-input mb-3">{val.ticketCategory}
                    <div className="form-group collapse myaccount-error" id={"collapseName" + val.eventTicketAttribute}>
                            <AddEventTicketDetails channels={that.props.channels} eventdetailId={that.props.eventDetailId} valueType={valueType} ticketType={ticketType} ChannelType={ChannelType} onSubmit={that.props.onSubmitEventDetails} options={tickets} selected={that.state.valEvent} selectedcurrency={that.state.valCurrency} currencyType={currencyType} changecurrency={that.changecurrencyVal} changeVal={that.changeVal} eventdetail={that.props.eventdetailData}  selectEventDetail={that.props.selectEventDetail}  selectEventdetailId={that.props.selectEventdetailId}/>
                    </div>
                    <a className="form-icon form-icon-edit form-icon-sm d-none" role="button" data-toggle="collapse"  data-target={"#collapseName" + val.eventTicketAttribute} aria-expanded="false" aria-controls="collapseExample">
                        <i className="fa fa-pencil" aria-hidden="true"></i>Edit </a>
                </div>
           </div>
            });
        }

        if (this.props.ticketfetchsuccess && this.props.eventTicketTypefetchsuccess && this.props.eventChannelTypefetchsuccess && this.props.eventvalueTypeFetchsuccess && this.props.currencyFetchSuccess) {
            tickets = this.props.ticektCategories

            ticketType = this.props.ticketTypes.map((item) => {
                return <option >{item}</option>
            })
            ChannelType = this.props.channels.map((item) => {
                return <option >{item}</option>
            })
            valueType = this.props.valueTypes.map((item) => {
                return <option >{item}</option>
            })
            currencyType = this.props.currencyType

            return <div>
                <h4 className="mb-3">Ticket Details</h4>
                {this.props.Isaddmore == true &&
                    <div> <Button type="submit" onClick={this.props.getComponent} className="btn btn-sm btn-outline-secondary">Add Another Ticket+</Button>
                        <div>{TicketDataList}</div>
                    </div>}
                {this.props.IsShowcomponent == true && 
                    <div>
                    <AddEventTicketDetails channels={this.props.channels} eventdetailId={this.props.eventDetailId} valueType={valueType} ticketType={ticketType} ChannelType={ChannelType} onSubmit={this.props.onSubmitEventDetails} options={tickets} selected={this.props.valTicketcategory} selectedcurrency={this.props.valCurrency} currencyType={currencyType} changecurrency={this.props.changecurrencyVal} changeVal={this.props.changeValTicketCatgory} eventdetail={this.props.eventdetailData} selectEventDetail={this.props.selectEventDetail} selectEventdetailId={this.props.selectEventdetailId}/>
                    </div>
                }
            </div>
        } else {
            return (<div><FILLoader /></div>);
        }
    }   
}
