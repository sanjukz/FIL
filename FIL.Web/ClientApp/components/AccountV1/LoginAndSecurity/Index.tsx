import * as React from 'react';
import { RouteComponentProps } from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import KzLoader from "../../../components/Loader/KzLoader";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import { ChangePasswordFormDataViewModel } from "../../../models/Account/ChangePasswordFormDataViewModel";
import { ChangePasswordResponseViewModel } from '../../../models/Account/ChangePasswordResponseViewModel';
import { ValidateResetPassword } from "../../../utils/SignInSignUp/Validation";
import {
    defaultValidatePassword, defaultValidateConfirmPassword, defaultValidateOldPassword,
    IResetPasswordValidationResponse
} from "../../../models/Auth/ValidationResponse";
import ChangePasswordForm from "./ChangePasswordForm";
import BreadcrumbAndTitle from '../BreadcrumbAndTitle';
import { gets3BaseUrl } from '../../../utils/imageCdn';
import { Alert, message } from 'antd';

type AccountProps = AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

class LoginAndSecurity extends React.PureComponent<AccountProps, any>{
    constructor(props) {
        super(props);
        this.state = {
            gets3BaseUrl: gets3BaseUrl(),
            user: {
                oldPassword: "",
                password: "",
                confirmPassword: "",
                firstName: '',
                lastName: '',
                email: '',
            },
            isLoading: true,
            validateResponse: null,
            signupmethod: null,
            hasError: false
        }
    }

    componentDidMount = () => {
        if (this.props.session.isAuthenticated) {
            if (this.props.account.loginAndSecurityUser != null
                && this.props.account.loginAndSecurityUser.profile.altId.toLocaleLowerCase() ==
                this.props.session.user.altId.toLocaleLowerCase()) {
                this.setState({ isLoading: false })
            } else {
                this.requestAndUpdateAction();
            }
        }
        let validationResponse: IResetPasswordValidationResponse = {
            password: defaultValidatePassword,
            confirmPassword: defaultValidateConfirmPassword,
            oldPassword: defaultValidateOldPassword
        }
        this.setState({ validateResponse: validationResponse })
    }

    componentDidUpdate = (nextProps) => {
        if (nextProps.session.isAuthenticated !== this.props.session.isAuthenticated) {
            if (this.props.account.loginAndSecurityUser != null && this.props.account.loginAndSecurityUser.profile.altId.toLocaleLowerCase() ==
                this.props.session.user.altId.toLocaleLowerCase()) {
                this.setState({ isLoading: false })
            } else {
                this.requestAndUpdateAction();
            }
        }
    }
    requestAndUpdateAction = (isUpdate?: boolean) => {
        const { user } = this.state;
        let userModel: ChangePasswordFormDataViewModel = {
            altId: this.props.session.user.altId,
            oldPassword: user.oldPassword,
            newPassword: user.password,
            confirmPassword: user.confirmPassword
        }
        this.props.loginAndSecurityAction(userModel, (response: ChangePasswordResponseViewModel) => {
            user.email = response.profile.email;
            user.firstName = response.profile.firstName;
            user.lastName = response.profile.lastName;
            if (isUpdate && response.success) {
                user.oldPassword = "";
                user.password = "";
                user.confirmPassword = "";
            }
            this.setState({
                isLoading: false,
                user: user,
                signupmethod: response.profile.signupmethod,
                hasError: !response.success && response.wrongPassword
            })
            if (isUpdate) {
                this.showSuccessAlert(!response.success && response.wrongPassword);
            }
        });
    }

    onChangeValue = (e) => {
        const { validateResponse } = this.state;
        let user = Object.assign({}, this.state.user);
        user[e.target.name] = e.target.value;
        let validationResponse = ValidateResetPassword(user, validateResponse, false);
        this.setState({ validateResponse: validationResponse, user: user })
    }

    onSaveValue = () => {
        const { validateResponse } = this.state;
        let user = Object.assign({}, this.state.user);
        let validationResponse = ValidateResetPassword(user, validateResponse, true);
        if (!validationResponse.oldPassword.isInValid && !validationResponse.password.isInValid
            && !validationResponse.confirmPassword.isInValid) {
            this.requestAndUpdateAction(true);
        }
        this.setState({ validateResponse: validationResponse, user: user, hasError: false })
    }

    showSuccessAlert = (isError: boolean) => {
        if (isError) {
            message.error({
                content: 'Failed to save, please try again',
                duration: 5,
                className: 'mt-5'
            })
        } else {
            message.success({
                content: 'Password updated successfully',
                duration: 5,
                className: 'mt-5'
            })
        }
    }

    render() {
        const { user, validateResponse, hasError } = this.state;
        if (!this.props.session.isAuthenticated || this.props.account.isLoading || this.state.isLoading) {
            return <KzLoader />
        }
        return <>
            <div className="container">

                <BreadcrumbAndTitle title={'Login & Security'} />

                <div className="row">
                    <div className="col-sm-8">
                        <div>
                            <h4 className="mb-5">Login</h4>
                            <a
                                className="btn btn-outline-primary btn-sm float-right"
                                data-toggle="collapse"
                                href="#password"
                                role="button"
                                aria-expanded="false"
                                aria-controls="password"
                            >
                                Update</a>
                            <p>Password </p>

                            <div className={`collapse pt-3 ${hasError ? " show" : ""}`} id="password">
                                {hasError &&
                                    <div className="form-row">
                                        <div className="col-sm-6 my-2">
                                            < Alert
                                                message="Error"
                                                description={`Incorrect old password, please try again `}
                                                type="error"
                                                showIcon
                                                className="mb-3"
                                            />
                                        </div>
                                    </div>
                                }
                                <div className="form-row">
                                    <div className="col-sm-6">
                                        <label className="font-weight-bold">Old Password</label>
                                        <input
                                            type="password"
                                            className={"form-control" + ((validateResponse && validateResponse.oldPassword.isInValid) ? " is-invalid" : "")}
                                            placeholder="Old Password"
                                            name="oldPassword"
                                            value={user.oldPassword}
                                            autoComplete="old-password"
                                            onChange={(e) => this.onChangeValue(e)}
                                        />
                                        {
                                            validateResponse && validateResponse.oldPassword.isInValid &&
                                            <div className="d-block invalid-feedback">{validateResponse && validateResponse.oldPassword.message} </div>
                                        }
                                    </div>
                                </div>
                                <div className="form-row py-4">
                                    <div className="col-sm-6">
                                        <label className="font-weight-bold">New Password</label>
                                        <input
                                            type="password"
                                            className={"form-control" + ((validateResponse && validateResponse.password.isInValid) ? " is-invalid" : "")}
                                            placeholder="New Password"
                                            name="password"
                                            value={user.password}
                                            autoComplete="new-password"
                                            onChange={(e) => this.onChangeValue(e)}
                                        />
                                        {
                                            validateResponse && validateResponse.password.isInValid &&
                                            <div className="d-block invalid-feedback">{validateResponse && validateResponse.password.message} </div>
                                        }
                                    </div>
                                    <div className="col-sm-6">
                                        <label className="font-weight-bold">Confirm Password</label>
                                        <input
                                            type="password"
                                            className={"form-control" + ((validateResponse && validateResponse.confirmPassword.isInValid) ? " is-invalid" : "")}
                                            placeholder="Confirm Password"
                                            name="confirmPassword"
                                            autoComplete="confirm-password"
                                            value={user.confirmPassword}
                                            onChange={(e) => this.onChangeValue(e)}
                                        />
                                        {
                                            validateResponse && validateResponse.confirmPassword.isInValid &&
                                            <div className="d-block invalid-feedback">{validateResponse && validateResponse.confirmPassword.message} </div>
                                        }
                                    </div>
                                    <ChangePasswordForm user={user} validateResponse={validateResponse} />
                                </div>
                                <a
                                    className="btn btn-primary"
                                    data-toggle="collapse"
                                    href="#fullName"
                                    role="button"
                                    aria-expanded="false"
                                    aria-controls="fullName"
                                    onClick={() => this.onSaveValue()}
                                >
                                    Update</a>
                            </div>
                        </div>
                        {(this.state.signupmethod == 2 || this.state.signupmethod == 3) &&
                            <>
                                <hr />
                                <div>
                                    <h4 className="mb-5">Social accounts</h4>
                                    <p>{this.state.signupmethod == 2 ? "Google" : "Facebook"} </p>
                                </div>
                            </>
                        }
                    </div>
                    <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
                        <div className="border p-4">
                            <img
                                src={`${this.state.gets3BaseUrl}/my-account/right-bar-images/login-security.svg`}
                                className="img-fluid mb-4"
                                alt=""
                            />
                            <div>
                                <h5 className="mb-3">Let's make your account more secure</h5>
                                <p className="m-0">
                                    We’re always working on ways to increase safety in our community.
                                    That’s why we look at every account to make sure it’s as secure as possible.

                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    }
}
export default LoginAndSecurity;