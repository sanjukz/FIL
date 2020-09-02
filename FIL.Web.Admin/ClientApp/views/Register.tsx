import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import { RegistrationForm } from "../components/form/RegistrationForm";
import { IApplicationState } from "../stores";
import * as RegisterStore from "../stores/Register";
import { RegistrationFormDataViewModel } from "shared/models/RegistrationFormDataViewModel";

type RegistrationProps = RegisterStore.IRegisterState & typeof RegisterStore.actionCreators & RouteComponentProps<{}>;


class Register extends React.Component<RegistrationProps, {}> {
    public render() {

        return <div className="vertical-center">

            <div className="text-center form-box">
                <img src="http://kitms.kyazoonga.com/images/site-logo-blk.png" alt="" />
                <h6 className="site-top-title pb-3">Kyazoonga Integrated Ticket Managment
                                    System (KITMS<sup>TM</sup>)</h6>
                <div className="card p-3 pt-4 pb-4">
                    <h6 className="pb-3">Create a new account</h6>
                    <RegistrationForm onSubmit={this.onSubmitRegister} />
                </div>
            </div>
        </div>;
    }

    @autobind
    private onSubmitRegister(values: RegistrationFormDataViewModel) {
        this.props.register(values, (response) => {
            if (response.success) {
                this.props.history.replace("/login");
            }
        });
    }
}


export default connect(
    (state: IApplicationState) => state.register,
    RegisterStore.actionCreators
)(Register);
