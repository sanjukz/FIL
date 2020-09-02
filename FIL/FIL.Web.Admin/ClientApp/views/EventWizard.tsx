import * as React from "react";
import { connect } from "react-redux";
import * as IEventWizardState from "../stores/EventWizard";
import { CreateEventHome } from "../components/EventWizard/CreateEventHome";
import { CreateEventMasterVenue } from "../components/EventWizard/CreateEventMasterVenue";
import { CreateEventVenue } from "../components/EventWizard/CreateEventVenue";
import { CreateSeats } from "../components/EventWizard/CreateSeats";
import WizardProgressBar from "../components/EventWizard/WizardProgressBar";
import { WizardNavigator } from "../components/EventWizard/WizardNavigator";
import { IApplicationState } from "../stores";

type EventComponentStateProps = IEventWizardState.IEventWizardState & typeof IEventWizardState.actionCreators;

export class EventWizard extends React.Component<EventComponentStateProps, IEventWizardState.IEventWizardState> {
    //state and props are defaulted to type 'any' just for the start to get the layout working first
    public componentWillMount() {
        this.props.requestVenues();
        this.setState({ progress: 25 });
        this.changeVal("");
    }

    public increaseProgress() {
        this.setState({ progress: this.state.progress < 100 ? this.state.progress + 25 : 100 });
    }

    public decreaseProgress() {
        this.setState({ progress: this.state.progress > 25 ? this.state.progress - 25 : 25 });
    }

    public showProgress() {
        return this.state.progress === 100 ? "Finish" : "Next";
    }
    public changeVal(e) {
        this.setState({ val: e });
    }

    public render() {
        const venue = this.props.venues.venues;
        return <div>
            <WizardProgressBar progress={this.state.progress} />
            {this.state.progress === 25 && <CreateEventHome />}
            {this.state.progress === 50 && <CreateEventVenue options={venue} selected={this.state.val} changeValue={this.changeVal} />}
            {this.state.progress === 75 && <CreateEventMasterVenue />}
            {this.state.progress === 100 && <CreateSeats />}
            <WizardNavigator increaseProgress={this.increaseProgress.bind(this)} decreaseProgress={this.decreaseProgress.bind(this)} showProgress={this.showProgress.bind(this)} />
        </div>;
    }
}
export default connect(
    (state: IApplicationState) => state.eventWizard,
    IEventWizardState.actionCreators
)(EventWizard);
