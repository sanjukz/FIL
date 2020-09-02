import * as React from "react";
import "shared/styles/globalStyles/main.scss";
import WizardProgressBar from "./WizardProgressBar";
export class CreateEventHome extends React.Component<any, any> {
    public render() {
        return <div className="container bg-white pb-20">
            <div className="row pt-15 pb-20">
                <div className="col-sm-8">
                    <h2 className="m-0">Create Event</h2>
                </div>
                <div className="col-sm-4 pt-5">
                    <div className="progress m-0">
                        <WizardProgressBar color="blue" />
                    </div>
                </div>
            </div>
            <div className="row pt-20 pb-20">
                <div className="col-md-4 col-md-offset-4">
                    <h4>Event Details</h4>
                    <div className="form-group">
                        Name
                        <input type="text" className="form-control" id="name" placeholder="Name" />
                    </div>
                    <div className="form-group">
                        Date
                        <input type="text" className="form-control" id="name" placeholder="Date" />
                    </div>
                    <div className="form-group">
                        Event Capacity (if non-seated venue)
                        <input type="text" className="form-control" id="name" placeholder="Placeholder" />
                    </div>
                    <div className="form-group">
                        Lable
                        <input type="text" className="form-control" id="name" placeholder="Placeholder" />
                    </div>
                    <div className="form-group">
                        Lable
                        <input type="text" className="form-control" id="name" placeholder="Placeholder" />
                    </div>
                    <div className="checkbox">
                        <input type="checkbox" /> This event requires a seated venue
                    </div>
                </div>
            </div>
        </div>;
    }
}
