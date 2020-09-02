import * as React from "react";
import { connect } from "react-redux";

export class WizardNavigator extends React.Component<any, any> {
    public render() {
        return <div className="bg-gray-light pt-15 pb-15 navbar navbar-fixed-bottom">
            <div className="container text-center">
                <div className="col-sm-4">
                    <button onClick={this.props.decreaseProgress} className="btn btn-default">Back</button>
                </div>
                <div className="col-sm-4 white-txt pt-5">
                    Saved as draft!
                </div>
                <div className="col-sm-4">
                    <button onClick={this.props.increaseProgress} className="btn btn-default">{this.props.showProgress()}</button>
                </div>
            </div>
        </div>;
    }
}
