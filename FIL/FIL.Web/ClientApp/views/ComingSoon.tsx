import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import NotifyMe from "../components/Footer/Notifyme";
import { IApplicationState } from "../stores";
import * as FooterStore from "../stores/Footer";
import { NewsLetterSignupFooterDataViewModel } from "../models/Footer/NewsLetterSignupFooterDataViewModel";
import { NewsLetterSignupFooterResponseDataViewModel } from "../models/Footer/NewsLetterSignupFooterResponseDataViewModel";
import KzAlert from "../components/Alert/KzAlert";
import Metatag from "../components/Metatags/Metatag";

type FooterProps = FooterStore.INewsLetterState & typeof FooterStore.actionCreators;

class ComingSoon extends React.Component<FooterProps, any> {
    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }
    }

    public render() {
        return <div>
            <Metatag url={null} title="Coming Soon" />
            <NotifyMe onSubmit={this.onSubmitSignUp} />
            {this.props.registered && <KzAlert visible={this.props.alertVisible}>Successfully signed up to our newsletters.</KzAlert>}
            {this.props.subscriptionExists && <KzAlert visible={this.props.alertVisible}>Already signed up to our newsletters.</KzAlert>}
        </div>;
    }

    @autobind
    private onSubmitSignUp(values: NewsLetterSignupFooterDataViewModel) {
        this.props.newsLetterSignUp(values, (response: NewsLetterSignupFooterResponseDataViewModel) => {
            if (response.success || response.isExisting) {
                if (response.success) {
                    alert("Successfully signed up to our newsletters.");
                }
                else if (response.isExisting) {
                    alert("Already signed up to our newsletters");
                }
                setTimeout(() => {
                    this.props.hideAlertAction();
                }, 3000);
            }
        });
    }
}

export default connect(
    (state: IApplicationState) => state.footer,
    FooterStore.actionCreators
)(ComingSoon);

