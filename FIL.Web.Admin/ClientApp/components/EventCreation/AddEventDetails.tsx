import { autobind } from "core-decorators";
import * as React from "React";
import Yup from "yup";
import EventDetailForm from "../form/EventCreation/EventDetailForm";
import { Field } from "formik";
import Select from "react-select";
import { AddEventDetailViewModel } from "../../models/EventCreation/AddEventDetailViewModel";
import VenueCreationViewModel from "../../models/Venues/VenueCreation"
import GetEventsResponseViewModel from "../../models/EventCreation/GetEventsResponseViewModel";
import * as Datetime from "react-datetime";
import "./DatePicker.css";

interface ISaveEventDetails {
    onSubmit: (values: AddEventDetailViewModel) => void;
    options: VenueCreationViewModel[],
    selected: any,
    changeVal: any,
    selectEvent: any,
    selectEventId: any,
    name: any,
    startdatetime: any,
    enddatetime: any,
    metaDetails: any,
    id: any
    event: GetEventsResponseViewModel[];
    onCancle: () => void;
   EventStartDate:any;
   OnChangeStartDatetime:()=>void;
   OnChangeEndtime:()=>void;
   EventEndDate:any
}

export default class AddEventDetails extends React.Component<ISaveEventDetails, any> {
    constructor(props) {
        super(props)
        this.state = {
            id: this.props.id,
            name: this.props.name,
            metaDetails: this.props.metaDetails,
            startdatetime: this.props.startdatetime,
            enddatetime: this.props.enddatetime
        }
    }

    @autobind
    public saveEventData(e) {
        this.setState({
            name: e.target.value
        })
    }

    public render() {
        const schema = this.getSchema();
        var venueSelect;
        var Eventselect;
        if (this.props.options !== undefined) {
            venueSelect = this.props.options.map(function (venue) {
                return { "id": venue.altId, "label": venue.name };
            });
        }

        if (this.props.event !== undefined) {
            Eventselect = this.props.event.map(function (event) {
                return { "id": event.id, "label": event.name };
            });
        }

        return (
            <EventDetailForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="form-group mb-2 position-relative">
                    <Field type="name" name="name" className="form-control" placeholder={this.state.name == "" ? "Enter sub-place name" : this.state.name} required />
                </div>
                
                <div className="form-group mb-2 position-relative">
                    <Select
                        value={this.props.selectEvent}
                        onChange={this.props.selectEventId}
                        name="form-field-name"
                        options={Eventselect}
                        placeholder="Select place">
                        <option value='1' disabled>Select place</option>
                    </Select>
                </div>

                <div className="form-group mb-2 position-relative">
                    <Select
                        name="form-field-name"
                        value={this.props.selected}
                        onChange={this.props.changeVal}
                        options={venueSelect}
                        placeholder="Select venue">
                        <option value='1' disabled>Select venue</option>
                    </Select>
                </div>
                <div className="form-row">
                    <div className="form-group mb-2 position-relative col-6">
                        <Datetime inputProps={{ placeholder: 'Select place open date & time', disabled: false }} onChange={this.props.OnChangeStartDatetime} value={this.props.EventStartDate} dateFormat="YYYY-MM-DD" timeFormat={true}/>
                    </div>

                    <div className="form-group mb-2 position-relative col-6">
                        <Datetime inputProps={{ placeholder: 'Select place close date & time', disabled: false }} onChange={this.props.OnChangeEndtime} value={this.props.EventEndDate} dateFormat="YYYY-MM-DD" timeFormat={true}/>
                    </div>
                </div>
            </EventDetailForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({
            //name: Yup.string().required(),
            //metaDetails: Yup.string().required(),
            //startDateTime: Yup.string().required(),
            //endDateTime: Yup.string().required()
        });
    }
}
