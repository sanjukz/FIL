import * as React from "React";
import EventTicketDetailForm from "../form/EventCreation/EventTicketDetailForm";
import Yup from "yup";
import { Field } from "formik";
import Select from "react-select";
import TicektResponseViewModel from "../../models/EventCreation/TicektResponseViewModel";
import SaveEventTicketDetailDataViewModel from "../../models/EventCreation/SaveEventTicketDetailDataViewModel";
import EventTicketTypeDataViewModel from "../../models/EventCreation/EventTicketTypeDataViewModel";
import ChannelTypeDataViewModel from "../../models/EventCreation/ChannelTypeDataViewModel";
import EventTypedDataViewMdel from "../../models/EventCreation/EventValueTypeViewModel";
import CurrencyRespnseViewModel from "../../models/EventCreation/CurrencyRespnseViewModel";
import SubEventDetailDataResponseViewModel from "../../models/EventCreation/SubEventDetailDataResponseViewModel";

interface ISaveEventTicketDetails {
    onSubmit: (values: SaveEventTicketDetailDataViewModel) => void;
    options: TicektResponseViewModel[],
    selected: any,
    changeVal: any,
    selectedcurrency: any,
    changecurrency: any,
    ticketType: EventTicketTypeDataViewModel,
    ChannelType: ChannelTypeDataViewModel,
    valueType: EventTypedDataViewMdel,
    currencyType: CurrencyRespnseViewModel[],
    eventdetailId: any,
    eventdetail: SubEventDetailDataResponseViewModel[],
    selectEventdetailId: any,
    selectEventDetail: any,
    channels: any
}
export default class AddEventTicketDetails extends React.Component<ISaveEventTicketDetails, any> {

    constructor(props) {
        super(props);
        this.state = {
            isPasswordNotMatch: false,
            selectedOption: null,
        }
    }

    handleChange = (selectedOption) => {
        this.setState({ selectedOption });
    }

    public render() {
        var cat, ticketType, channelType, valueType, eventdetailData;
        const schema = this.getSchema();
        var options = [];
        var i = 0;
        if (this.props.channels != undefined) {
            this.props.channels.map(function (item) {
                i = i + 1;
                var data = {
                    value: i,
                    label: item
                };
                options.push(data);
            })
        }
        
        const ticketSelect = this.props.options.map(function (tickets) {
            return { "id": tickets.id, "label": tickets.name };
        });
        const currencySelect = this.props.currencyType.map(function (currency) {
            return { "id": currency.id, "label": currency.name };
        });

        if (this.props.eventdetail != undefined) {
            eventdetailData = this.props.eventdetail.map(function (eventdetail) {
                return { "id": eventdetail.id, "label": eventdetail.name };
            });
        }

        if (this.props.ticketType != undefined) {
            ticketType = this.props.ticketType;
        }
        if (this.props.ChannelType != undefined) {
            channelType = this.props.ChannelType;
        }
        if (this.props.valueType != undefined) {
            valueType = this.props.valueType;
        }
        return (
            <EventTicketDetailForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="form-group mb-2 position-relative">
                    <Select
                        name="ticketCategoryId"
                        options={eventdetailData}
                        value={this.props.selectEventdetailId}
                        onChange={this.props.selectEventDetail}
                        placeholder="Select sub-event">
                        <option value='1' disabled>Select sub-event</option>
                    </Select>
                </div>
                <div className="form-group mb-2 position-relative">
                    <Select
                        name="ticketCategoryId"
                        value={this.props.selected}
                        onChange={this.props.changeVal}
                        options={ticketSelect}
                        placeholder="Select ticket category"
                    />
                </div>
                {/* <div className="form-group mb-2 position-relative ">
                    <Field component="select" name="ticketTypeId" className="form-control" Placeholder="Select ticket type">
                        <option>Select ticket type</option>
                        {ticketType}
                    </Field>
                </div> */}
                <div className="form-group mb-2 position-relative">
                    <Select
                        multi
                        options={options}
                        placeholder= "Select channel types"
                        onChange={this.handleChange}
                        value={this.state.selectedOption}
                    />
                </div>
                <div className="form-group mb-2 position-relative">
                    <Select
                        name="ticketCategoryId"
                        value={this.props.selectedcurrency}
                        onChange={this.props.changecurrency}
                        options={currencySelect}
                        placeholder="Select ticket currency"
                    />
                </div>
                <div className="form-group mb-2 position-relative">
                    <Field type="name" name="availableTicketForSale" className="form-control" placeholder="Enter number of tickets available for sale" />
                </div>
                <div className="form-group mb-2 position-relative">
                    <Field type="name" name="ticketCategoryDescription" className="form-control" placeholder=" Enter ticket category description" />
                </div>
                <div className="form-group mb-2 position-relative">
                    <Field type="name" name="price" className="form-control" placeholder="Enter price per ticket" />
                </div>

                <div className="form-group mb-2 position-relative">
                    <Field type="name" name="conv" className="form-control" placeholder="Enter Convenience Charge (Percentage)" />
                </div>

                <div className="form-group mb-2 position-relative">
                    <Field type="name" name="bank" className="form-control" placeholder="Enter Bank Charge (Percentage)" />
                </div>

                <div className="form-group mb-2 position-relative">
                    <Field type="name" name="serviceCharge" className="form-control" placeholder="Enter Service Charge(Percentage)" />
                </div>
            </EventTicketDetailForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({
        });
    }
}
