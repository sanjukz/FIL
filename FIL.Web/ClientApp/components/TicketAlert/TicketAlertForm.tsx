import * as React from "react";
import { TicketAlertRequestViewModel } from "../../models/TicketAlert/TicketAlertRequestViewModel";
import { TicketAlertUserMappingResponseViewModel } from "../../models/TicketAlert/TicketAlertUserMappingResponseViewModel";

export default class TicketAlertForm extends React.Component<any, any> {
    getDefaultObject = () => {
        let ticketAlert: TicketAlertRequestViewModel = {
            countriesAltIds: ['231'],
            ticketAlertEventMapping: [this.props.eventData.ticketAlertEventMapping ? this.props.eventData.ticketAlertEventMapping.id : 0],
            eventAltId: this.props.eventData.event.altId,
            eventName: this.props.eventData.event.name,
            firstName: '',
            lastName: '',
            phoneCode: '',
            email: '',
            phoneNumber: '',
            tourAndTavelPackage: 0,
            isStreamingEvent: false,
            noOfTickets: 1
        }
        return ticketAlert;
    }
    state = {
        ticketAlertForm: this.getDefaultObject()
    };

    getPhoneCodeOptions = () => {
        return this.props.countryList
            .filter(item => item.phonecode && item.name)
            .map(item => {
                return <>
                    <option value={item.phonecode + "~" + item.altId}>
                        {item.name + " (" + item.phonecode + ")"}
                    </option>
                </>
            });
    }

    isButtonDisabled = () => {
        var pattern = /^[a-zA-Z0-9\-_]+(\.[a-zA-Z0-9\-_]+)*@[a-z0-9]+(\-[a-z0-9]+)*(\.[a-z0-9]+(\-[a-z0-9]+)*)*\.[a-z]{2,4}$/;
        return !(this.state.ticketAlertForm.firstName && this.state.ticketAlertForm.lastName && pattern.test(this.state.ticketAlertForm.email) && this.state.ticketAlertForm.phoneCode && this.state.ticketAlertForm.phoneNumber)
    }

    render() {
        return (
            <div className="card bg-white p-4">
                <h3 className="text-purple">Save the date! Register to receive a notification when tickets go on sale.</h3>
                <div className="form-row">
                    <div className="form-group col-md-6">
                        <label>First Name</label>
                        <input type="input" placeholder="First Name" className="form-control" onChange={(e: any) => {
                            let ticketAlertForm = this.state.ticketAlertForm;
                            ticketAlertForm.firstName = e.target.value;
                            this.setState({ ticketAlertForm: ticketAlertForm })
                        }} />
                    </div>
                    <div className="form-group col-md-6">
                        <label>Last Name</label>
                        <input type="input" placeholder="Last Name" className="form-control" onChange={(e: any) => {
                            let ticketAlertForm = this.state.ticketAlertForm;
                            ticketAlertForm.lastName = e.target.value;
                            this.setState({ ticketAlertForm: ticketAlertForm })
                        }} />
                    </div>
                    <div className="form-group col-md-12">
                        <label >Email</label>
                        <input type="email" placeholder="Email" className="form-control" onChange={(e: any) => {
                            let ticketAlertForm = this.state.ticketAlertForm;
                            ticketAlertForm.email = e.target.value;
                            this.setState({ ticketAlertForm: ticketAlertForm })
                        }} />
                    </div>
                    <div className="form-group col-sm-6">
                        <label >Country/Region</label>
                        <select className="form-control" onChange={(e) => {
                            let ticketAlertForm = this.state.ticketAlertForm;
                            ticketAlertForm.phoneCode = e.target.value.split('~')[0];
                            this.setState({ ticketAlertForm: ticketAlertForm })
                        }}
                            name="phoneCode">
                            <option value="" disabled selected>
                                Phone Code
                            </option>
                            {this.getPhoneCodeOptions()}
                        </select>
                    </div>
                    <div className="form-group col-md-6">
                        <label >Mobile Number</label>
                        <input type="number" placeholder="Mobile Number" className="form-control" onChange={(e: any) => {
                            let ticketAlertForm = this.state.ticketAlertForm;
                            ticketAlertForm.phoneNumber = e.target.value;
                            this.setState({ ticketAlertForm: ticketAlertForm })
                        }} />
                    </div>
                    <div className="alert-btn col-md-12">
                        <button type="submit" onClick={(e: any) => {
                            console.log(this.state.ticketAlertForm);
                            this.props.onSubmit(this.state.ticketAlertForm)
                        }} className="btn btn-primary fil-btn p-2" disabled={this.isButtonDisabled()} >Please remind me!<img src="https://static7.feelitlive.com/images/fil-images/icon/arrow-right.svg" className="ml-2" alt="" style={{ width: "7px", marginTop: "-2px" }} /></button>
                    </div>
                </div>
            </div>
        );
    }
}