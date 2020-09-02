import * as React from 'react';

class ChauffeurServiceBooking extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            pickupTime: "00:00",
            journeyType: 2,
            waitingTime: 1
        };
    };

    render() {
        return (
            <div className="my-3 row">
                <div className="col-sm-6">
                    <h5 className="mb-3 pink-color text-center">Select Pickup Time</h5>
                    <div className="time-select d-flex justify-content-center">
                        <input className="form-control" onChange={this.props.onTimeSelection} type="time" name="appt-time" min="00:00" max="23:59" value={this.props.pickupTime} required />
                    </div>
                </div>
                <div className="col-sm-6">
                    <h5 className="mb-3 pink-color text-center">Waiting Time</h5>
                    <div className="d-flex justify-content-center">
                        <select className="form-control" id="waitingTime" onChange={this.props.onWaitingTimeChange} required>
                            <option value="" selected disabled>Select Waiting Time</option>
                            <option value="1">1 hr</option>
                            <option value="2">2 hr</option>
                            <option value="3">3 hr</option>
                            <option value="4">4 hr</option>
                            <option value="5">5 hr</option>
                            <option value="6">6 hr</option>
                            <option value="7">7 hr</option>
                            <option value="8">8 hr</option>
                        </select>
                    </div>
                </div>
                <div className="col-sm-12 mt-4">
                    <h5 className="mb-3 pink-color text-center">Where would you like picking up from?</h5>
                    <div className="row">
                        <div className="col-sm-4">
                            <input className="form-control" onChange={this.props.onDepartureAddressChange} name="address1" type="text" placeholder="Address Line 1" required />
                        </div>
                        <div className="col-sm-4">
                            <input className="form-control" onChange={this.props.onDepartureAddressChange} name="address2" type="text" placeholder="Address Line 2" required />
                        </div>
                        <div className="col-sm-4">
                            <input className="form-control align-self-center" onChange={this.props.onDepartureAddressChange} name="postalCode" type="text" placeholder="Postal Code" required />
                        </div>
                    </div>
                </div>
            </div>);
    }
}

export default ChauffeurServiceBooking;