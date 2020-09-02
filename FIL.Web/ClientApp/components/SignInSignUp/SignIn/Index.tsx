import * as React from 'react';
import { connect } from "react-redux";
import { IApplicationState } from "../../../stores";
import { bindActionCreators } from "redux";
import LoginFacebook from "../SocialLogin/LoginFacebook";
import LoginGoogle from "../SocialLogin/LoginGoogle";
import {
    actionCreators as signInActionCreators,
    ILoginProps
} from "../../../stores/Login";
import SignInComponent from "./SignInComponent";
import ForgetPassword from "./ForgetPassword";
import GetEmailForFacebook from "../SocialLogin/GetEmailForFacebook";
import FacebookSignInFormDataViewModel from "../../../models/SocialSignIn/FacebookSignInFormDataViewModel";
import { ValidateLoginForm, GetDefaultLoginValidationValues } from "../../../utils/SignInSignUp/Validation";
import { defaultValidateEmail, defaultValidatePassword, ILoginValidationResponse } from "../../../models/Auth/ValidationResponse";
import { LoginResponseViewModel } from "shared/models/LoginResponseViewModel";
import { ForgotPasswordResponseViewModel } from "shared/models/ForgotPasswordResponseViewModel";
import OTPLogin from "../OTPLogin"
interface ISignInProps {
    onChangeView: (pageName: string) => void;
    resetState: boolean;
    resetStateChanged: (e: any) => void;
    isCheckout: boolean;
    checkoutUser: () => void;
    redirectOnLogin: () => void;
}
type SignInProps = ISignInProps &
    ILoginProps &
    typeof signInActionCreators;

let resetFlag = true;
class SignIn extends React.Component<SignInProps, any>{
    state = {
        user: {
            email: '',
            password: ''
        },
        isForgetPassword: false,
        validateResponse: null,
        isLoggedIn: false,
        errorMessage: '',
        forgetPasswordSucess: false,
        resetFlag: true,
        facebookUser: null,
        getEmailForFacebook: false,
        isLoading: false,
        loginWithMobile: false
    }

    componentDidMount = () => {
        let validationResponse: ILoginValidationResponse = {
            email: defaultValidateEmail,
            password: defaultValidatePassword
        }
        this.setState({ validateResponse: validationResponse })
    }

    submitLogin = (e) => {
        e.preventDefault();
        const { user, validateResponse } = this.state;
        let validationResponse = ValidateLoginForm(user, validateResponse, true);
        if (!validateResponse.email.isInValid && !validationResponse.password.isInValid) {
            this.setState({ isLoading: true })
            this.props.login(user, (response: LoginResponseViewModel) => {
                if (response.success) {
                    this.setState({ isLoggedIn: true, isLoading: false })
                    if (this.props.isCheckout) {
                        this.props.checkoutUser();
                    } else {
                        this.props.redirectOnLogin();
                    }

                } else {
                    let error = '';
                    if (!response.success && response.isActivated && !response.isLockedOut) {
                        error = "Invalid email or password, please try again"
                    }
                    else if (response.isLockedOut) {
                        error = "Too many failed attempts, please try again after sometime or reset your password to continue"
                    }
                    else if (!response.isActivated) {
                        error = "Your account is not activated, please activate your account to continue"
                    }
                    this.setState({ isLoggedIn: false, errorMessage: error, isLoading: false })
                }
            });
        }
        this.setState({ validateResponse: validationResponse, errorMessage: '' })
    }

    handleInputChange = (e) => {
        const { user, validateResponse } = this.state;
        user[e.target.name] = e.target.value;
        let validationResponse = ValidateLoginForm(user, validateResponse, false);
        this.setState({ validateResponse: validationResponse, user: user, errorMessage: '' })
    }

    onChangeView = (pageName) => {
        const { validateResponse, user } = this.state;
        let isForgetPasword = false;
        let defaultValues = GetDefaultLoginValidationValues(validateResponse);
        this.props.onChangeView(pageName);
        if (pageName != "login") {
            isForgetPasword = true;
        }
        user.email = "";
        user.password = "";
        this.setState({
            validateResponse: defaultValues, isForgetPassword: isForgetPasword,
            isLoading: false, errorMessage: '', user: user
        });
    }

    resetPassword = (email) => {
        let formData = {
            email: email
        }
        this.setState({ isLoading: true })
        this.props.forgotPassword(formData, (response: ForgotPasswordResponseViewModel) => {
            if (response.success && response.isExisting) {
                if (this.props.isCheckout) {
                    sessionStorage.setItem("checkoutRedirect", "1")
                } else {
                    sessionStorage.removeItem("checkoutRedirect");
                }
                let msg = `A link to reset your password has been sent to ${email}`;
                this.setState({ forgetPasswordSucess: true, errorMessage: msg, isLoading: false })
            } else {
                let error = `No account exists for ${email}. Maybe you signed up using different/incorrect e-mail address`
                this.setState({
                    forgetPasswordSucess: false,
                    errorMessage: error,
                    isLoading: false
                })
            }
        });
    }
    resetError = () => {
        this.setState({ errorMessage: '' })
    }

    getEmailForFacebook = (user: FacebookSignInFormDataViewModel) => {
        this.setState({ getEmailForFacebook: true, facebookUser: user })
    }
    componentDidUpdate = (nextProps) => {
        //will reset state after modal close
        if (nextProps.resetState != this.props.resetState) {
            this.resetState();
            this.props.resetStateChanged(this);
        }
    }
    

    render() {
        // this.checkoutClassExistence()
        const { user, validateResponse, isForgetPassword, errorMessage, forgetPasswordSucess, facebookUser,
            getEmailForFacebook, isLoading, loginWithMobile } = this.state;
        return (<>
            <div className="form-row">
                {
                    !getEmailForFacebook && !loginWithMobile &&
                    <>
                        {!isForgetPassword ?
                            <SignInComponent user={user} onChangeView={(pageName) => this.onChangeView(pageName)}
                                handleInputChange={(e) => this.handleInputChange(e)} validateResponse={validateResponse}
                                errorMessage={errorMessage} isLoading={isLoading} submitLogin={(e) => this.submitLogin(e)}
                            />
                            :
                            <ForgetPassword onChangeView={(pageName) => this.onChangeView(pageName)}
                                resetPassword={(email) => this.resetPassword(email)} responseErrorMessage={errorMessage}
                                forgetPasswordSucess={forgetPasswordSucess} resetError={() => { this.resetError() }}
                                isLoading={isLoading}
                            />}
                    </>
                }

                {loginWithMobile &&
                    <OTPLogin isCheckout={this.props.isCheckout}
                        redirectOnLogin={() => this.props.redirectOnLogin()} checkoutUser={() => { this.props.checkoutUser() }}
                        onChangeView={(pageName) => this.onChangeView(pageName)} resetState={this.props.resetState} />
                }

                {getEmailForFacebook &&
                    <GetEmailForFacebook onChangeView={(pageName) => this.onChangeView(pageName)} isSignIn={true} user={facebookUser} />
                }

                <div className="col-sm-6 signup-form-right">
                    <div className="sign-up-inner-form">
                        {!loginWithMobile ?
                            <button className="btn btn-outline-primary btn-block mb-2" onClick={() => { this.resetState(); this.setState({ loginWithMobile: true }) }}><i className="fa fa-mobile fa-lg mr-2" aria-hidden="true"></i>
                            Log in with Mobile
                            </button>
                            :
                            <button className="btn btn-outline-primary btn-block mb-2" onClick={() => { this.resetState(); this.setState({ loginWithMobile: false }) }}><i className="fa fa-envelope-o  mr-2" aria-hidden="true"></i>
                            Log in with Email
                            </button>
                        }
                        <LoginFacebook getEmail={(user) => this.getEmailForFacebook(user)} isCheckout={this.props.isCheckout}
                            redirectOnLogin={() => this.props.redirectOnLogin()} checkoutUser={() => { this.props.checkoutUser() }} />
                        <LoginGoogle redirectOnLogin={() => this.props.redirectOnLogin()} isCheckout={this.props.isCheckout}
                            checkoutUser={() => { this.props.checkoutUser() }} />

                    </div>
                </div>
            </div>
        </>)
    }
    resetState = () => {
        GetDefaultLoginValidationValues(this.state.validateResponse);
        resetFlag = false
        const { user } = this.state;
        user.email = "";
        user.password = "";
        this.setState({ user: user, errorMessage: '', loginWithMobile: false }, () => {
            (e) =>
                this.props.resetStateChanged(e);
            resetFlag = true;
        })
    }
}
export default connect(
    (state: IApplicationState, ownProps) => ({
        loginState: state.login,
        ...ownProps
    }),
    dispatch =>
        bindActionCreators({ ...signInActionCreators }, dispatch)
)(SignIn);

