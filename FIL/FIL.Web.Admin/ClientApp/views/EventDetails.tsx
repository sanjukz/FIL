import { autobind } from "core-decorators";
import * as React from "react";
import AddEventDetails from "../../ClientApp/components/EventCreation/AddEventDetails";
import GetsubeventViewModel from "../models/EventCreation/GetsubeventViewModel";
import SubEventDetailDataResponseViewModel from "../models/EventCreation/SubEventDetailDataResponseViewModel";

export default class EventDetails extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            name: ""
        }
    }

    public componentDidMount() {
        this.props.requestVenues();
        var eventId: GetsubeventViewModel = {
            eventId: this.props.eventId
        };
        this.props.getSubeventData(eventId, (response: SubEventDetailDataResponseViewModel) => {
            if (response.eventDetail.length > 0) {
                this.setState({
                    showcomponent: false,
                    showDetailList: true
                })
            }
        });
    }

    @autobind
    public SaveEventDetail(name, starttime, endtime, metadetails, venueAltId, s) {
        this.setState({
            name: name,
            starttime: starttime,
            endtime: endtime,
            metadetails: metadetails,
            venueid: venueAltId
        });
    }

    public render() {
        if (this.props.fetchvenueSuccess) {
            const venue = this.props.venues;
            var that = this;
            if (this.props.subEventFetchSuccess) {
                const subevents = this.props.eventDetail;
                var EventList = subevents.map(function (val) {
                    return <div className="form-group mb-2 position-relative">
                        <div className="form-control form-control-sm readonly-input" >{val.name}</div>
                        <div className="form-group collapse myaccount-error" id={"collapseName" + val.id}>
                            <AddEventDetails onSubmit={that.props.onSubmitEventDetails} options={venue} selected={that.props.valEvent} name={val.name} startdatetime={val.startDateTime} enddatetime={val.endDateTime} metaDetails={val.metaDetails} id={val.id} changeVal={that.props.changeVal} event={that.props.event} selectEventId={that.props.selectEventId} selectEvent={that.props.selectEvent} onCancle={that.props.onCancle} OnChangeStartDatetime={that.props.OnChangeStartDatetime} EventStartDate={that.props.EventStartDate} OnChangeEndtime={that.props.OnChangeEndtime} EventEndDate={that.props.EventEndDate}/>
                        </div>
                        <div className="float-right">
                            <a className="form-icon form-icon-sm form-icon-edit d-none" onClick={that.SaveEventDetail.bind(this, val.name, val.startDateTime, val.endDateTime, val.metaDetails)} role="button" data-toggle="collapse" data-target={"#collapseName" + val.id} aria-expanded="false" aria-controls="collapseExample">
                                <i className="fa fa-pencil" aria-hidden="true"></i></a>
                        </div>
                    </div>;
                });
            }

            return <div>
                <h4 className="mt-0 mb-3">Day/Session Details</h4>
                {this.props.showcomponent == true && <AddEventDetails onSubmit={this.props.onSubmitEventDetails} options={venue} selected={this.props.valEvent} changeVal={this.props.changeVal} name={""} startdatetime={""} enddatetime={""} metaDetails={""} id={""} event={this.props.event} selectEventId={this.props.selectEventId} selectEvent={this.props.selectEvent} onCancle={this.props.onCancle} OnChangeStartDatetime={this.props.OnChangeStartDatetime} EventStartDate={this.state.EventStartDate} OnChangeEndtime={this.props.OnChangeEndtime} EventEndDate={this.props.EventEndDate}/>}
                {this.props.showDetailList == true && <div className="user-address">
                    <div className="mb-3">
                        <p><button type="button" className="btn btn-sm btn-outline-secondary" onClick={this.props.showSubEventForm} data-toggle="collapse" data-target="#collapseExample" aria-expanded="false" aria-controls="collapseExample">+ Add New Subevent</button></p>
                        {EventList}
                        <hr />
                    </div>
                </div>
                }
            </div>
        } else {
            return <div></div>
        }
    }
} 
