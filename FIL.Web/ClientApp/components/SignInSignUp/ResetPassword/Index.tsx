import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import ResetPasswordForm from "./ResetPasswordForm";
import { ResetPasswordFormDataViewModel } from "shared/models/ResetPasswordFormDataViewModel";
import { ResetPasswordResponseViewModel } from "shared/models/ResetPasswordResponseViewModel";
import { IApplicationState } from "../../../stores";
import * as ResetPasswordStore from "../../../stores/ResetPassword";
import { ValidateResetPassword } from "../../../utils/SignInSignUp/Validation";
import {
    defaultValidatePassword, defaultValidateConfirmPassword, IResetPasswordValidationResponse
} from "../../../models/Auth/ValidationResponse";
import FeelLoader from "../../Loader/FeelLoader";
import { Modal } from 'antd';


type ResetPasswordProps = ResetPasswordStore.IResetPasswordState
    & typeof ResetPasswordStore.actionCreators & RouteComponentProps<{}>;

class ResetPassword extends React.Component<ResetPasswordProps, any> {

    state = {
        user: {
            password: '',
            confirmPassword: '',
            firstName: '',
            lastName: '',
            email: '',
        },
        validateResponse: null,
        errorMessage: '',
        isLoading: true,
        resetPasswordSuccess: false
    }

    componentDidMount = () => {
        let userData: ResetPasswordFormDataViewModel = {
            password: "",
            confirmPassword: "",
            altId: this.props.location.search.split('?')[1],
            isRequestedUserDetails: true
        }
        let that = this;
        this.props.resetPassword(userData, (response: ResetPasswordResponseViewModel) => {
            if (response.success) {
                const { user } = that.state;
                user.email = response.user.email;
                user.firstName = response.user.firstName;
                user.lastName = response.user.lastName;

                that.setState({ isLoading: false, user: user })
            } else {
                alert("Internal server error, please try again");
                this.props.history.push("/");
            }
        });
        let validationResponse: IResetPasswordValidationResponse = {
            password: defaultValidatePassword,
            confirmPassword: defaultValidateConfirmPassword
        }
        this.setState({ validateResponse: validationResponse })
    }

    handleInputChange = (e) => {
        const { user, validateResponse } = this.state;
        user[e.target.name] = e.target.value;
        let validationResponse = ValidateResetPassword(user, validateResponse, false);
        this.setState({ validateResponse: validationResponse, user: user, errorMessage: '' })
    }

    showSuccessAlert = () => {
        let secondsToGo = 5;
        const modal = Modal.success({
            title: 'Reset Password',
            content: `Password Changed Successfully`,
            onOk: () => this.props.history.push("/")
        });
        setTimeout(() => {
            modal.destroy();
            this.props.history.push("/");
        }, secondsToGo * 1000);
    }

    public render() {
        const { user, validateResponse, errorMessage, isLoading } = this.state;

        if (!isLoading) {
            return <div className="container forgot-password pt-3 pb-5" style={{ maxWidth: "400px" }}>
                <div className="card">
                    <div className="card-header h5 bg-white">
                        Reset Password
                    </div>
                    <form className="needs-validation" onSubmit={(e) => this.onSubmitResetPassword(e)} noValidate autoComplete="off">
                        <div className="card-body">
                            <div className="row">
                                <div className="col-12 p-4">
                                    <p>It's a good idea to use a strong password that you're not using elsewhere</p>
                                    <ResetPasswordForm user={user} handleInputChange={(e) => this.handleInputChange(e)}
                                        validateResponse={validateResponse}
                                    />
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>;
        } else {
            return <FeelLoader />
        }
    }

    @autobind
    private onSubmitResetPassword(e) {
        e.preventDefault();
        const { user, validateResponse } = this.state;
        let validationResponse = ValidateResetPassword(user, validateResponse, true);
        this.setState({ validateResponse: validationResponse, errorMessage: '' });
        if (!validationResponse.password.isInValid && !validationResponse.confirmPassword.isInValid) {

            let userData: ResetPasswordFormDataViewModel = {
                password: user.password,
                confirmPassword: user.confirmPassword,
                altId: this.props.location.search.split('?')[1],
                isRequestedUserDetails: false
            }
            let that = this;
            this.setState({ isLoading: true })
            this.props.resetPassword(userData, (response: ResetPasswordResponseViewModel) => {
                if (response.success) {
                    that.setState({ isLoading: false })
                    this.showSuccessAlert();
                } else {
                    that.setState({ isLoading: false })
                    alert("Internal server error, please try again");
                    this.props.history.push("/");
                }
            });
        }
    }
}

export default connect(
    (state: IApplicationState) => state.ResetPassword,
    ResetPasswordStore.actionCreators
)(ResetPassword);
