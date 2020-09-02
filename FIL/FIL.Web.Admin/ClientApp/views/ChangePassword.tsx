import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import { ChangePasswordFormDataViewModel } from "../models/Account/ChangePasswordFormDataViewModel";
import { ChangePasswordResponseViewModel } from "../models/Account/ChangePasswordResponseViewModel";
import { IApplicationState } from "../stores";
import * as AccountStore from "../stores/Account";
import * as SessionStore from "shared/stores/Session";
import { bindActionCreators } from "redux";
import { ChangePasswordForm } from "../components/form/Account/ChangePasswordForm";

type AccountProps = AccountStore.IUserAccountProps & typeof AccountStore.actionCreators
    & SessionStore.ISessionProps & typeof SessionStore.actionCreators & RouteComponentProps<{}>;

export class ChangePassword extends React.Component<AccountProps, any> {
    constructor(props) {
        super(props);
        this.state = {
            userAltId: ""
        }
    }

    public componentDidMount() {
        if (localStorage.getItem('altId') != null && localStorage.getItem('altId') != '0') {
            this.setState({ userAltId: localStorage.getItem('altId') });
        }
    }

    public render() {
        return <div>
            <div className="col-sm-12">
                <div className="text-center form-box">
                    <h4>Change Password</h4>
                    <ChangePasswordForm onSubmit={this.onSubmitChangePassword} />
                    {(this.props.UserAccount.invalidPassword == true) && <span className="validate-txt"><p style={{ color: "#FF0000" }}>Please check provided password correctly</p></span>}
                </div>
            </div>
        </div>;
    }

    @autobind
    private onSubmitChangePassword(values: ChangePasswordFormDataViewModel) {
        values.altId = this.state.userAltId;
        if (values.newPassword != values.confirmPassword) {
            alert("New Password and confirmation Password must match!");
            values.oldPassword = "";
            values.newPassword = "";
            values.confirmPassword = "";
        }
        else {
            this.props.changePasswordAction(values, (response: ChangePasswordResponseViewModel) => {
                if (response.success) {
                    this.props.logout();
                    this.props.history.replace("/login");
                }
            });
        }
    }
}

export default connect(
    (state: IApplicationState) => ({ session: state.session, UserAccount: state.UserAccount }),
    (dispatch) => bindActionCreators({ ...AccountStore.actionCreators, ...SessionStore.actionCreators }, dispatch)
)(ChangePassword);
