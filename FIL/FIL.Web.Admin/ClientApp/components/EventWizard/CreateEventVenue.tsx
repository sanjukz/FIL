import * as React from "react";
import Select from "react-select";

export class CreateEventVenue extends React.Component<any, any> {
    public render() {
        const venueSelect = this.props.options.map(function (venue) {
            return { "id": venue.altId, "label": venue.name };
        });
        return <div className="container bg-white pb-20">
            <div className="row pt-15 pb-20">
                <div className="col-sm-8">
                    <h2 className="m-0">Create Event  Venue</h2>
                </div>
                <div className="col-sm-4 pt-5">
                    <div className="progress m-0">
                        <div className="progress-bar" role="progressbar">
                            <span className="sr-only">30% Complete</span>
                        </div>
                    </div>
                </div>
            </div>
            <div className="pt-20 pb-20">
                <div className="create-event-venue clearfix">
                    <div className="col-md-4 col-md-offset-4">
                        <div className="form-group">
                            <div id="imaginary_container">
                                <Select
                                    name="form-field-name"
                                    value={this.props.selected}
                                    onChange={this.props.changeVal}
                                    options={venueSelect}
                                />
                            </div>
                        </div>
                        <div className="form-group text-center">
                            or
                       </div>
                        <div className="form-group text-center">
                            <button type="button" className="btn" data-toggle="modal" data-target=".bs-example-modal-lg">New Venue Layout</button>
                        </div>
                        <div className="form-group text-center">
                            or
                        </div>
                        <div className="form-group text-center">
                            <a href="#">Manage your Master Venues</a>
                        </div>
                    </div>
                    <div className="modal fade bs-example-modal-lg" >
                        <a href="#" className="btn-black">Add New Stand</a>
                    </div>
                </div>
            </div>
        </div>;
    }
}
