import * as React from "react";
import Progress from "react-progressbar";

export default class WizardProgressBar extends React.Component<any, any> {
    // Generic progress bar component
    public render() {
        const progress = this.props.progress;
        return <div>
            <Progress completed={this.props.progress} color="blue" />
        </div>;
    }
}
