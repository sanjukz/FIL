import * as React from 'react';
import LoginFacebook from "../SocialLogin/LoginFacebook";
import LoginGoogle from "../SocialLogin/LoginGoogle";
import { ISignUpState } from "../../../stores/SignUp";
import SignUpComponent from "./SignUpComponent";
import GetEmailForFacebook from "../SocialLogin/GetEmailForFacebook";
import FacebookSignInFormDataViewModel from "../../../models/SocialSignIn/FacebookSignInFormDataViewModel";
import { ValidateSignUpForm, GetDefaultSignupValidationValues, IsValidSignUp } from "../../../utils/SignInSignUp/Validation";
import {
    defaultValidateEmail, defaultValidatePassword, defaultValidateConfirmPassword,
    defaultValidateFirstName, defaultValidateLastName, defaultValidatePhoneNumber, defaultValidatePhoneCode,
    ISignUpValidationResponse
} from "../../../models/Auth/ValidationResponse";
import { RegistrationResponseViewModel } from "shared/models/RegistrationResponseViewModel";
import { RegistrationFormDataViewModel } from "shared/models/RegistrationFormDataViewModel";
import OTPLogin from '../OTPLogin';

interface ISignUpProps {
    onChangeView: (typeName: string) => void;
    signUp: ISignUpState;
    handleSignUp: (user: RegistrationFormDataViewModel, callback: (RegistrationResponseViewModel: any) => void) => void;
    resetState: boolean;
    isLoading: boolean;
    resetStateChanged: (e: any) => void;
    redirectOnLogin: () => void;
    isCheckout: boolean;
    checkoutUser: () => void;
}

class SignUp extends React.Component<ISignUpProps, any>{
    state = {
        phoneCodeOptions: [],
        user: {
            firstName: '',
            lastName: '',
            email: '',
            phoneCode: '',
            phoneNumber: '',
            password: '',
            confirmPassword: '',
            userName: '',
            referralId: null,
            isMailOpt: false
        },
        validateResponse: null,
        resetFlag: true,
        errorMessage: '',
        isSignedUp: false,
        getEmailForFacebook: false,
        facebookUser: null,
        signUpOTP: false
    }

    componentDidMount = () => {
        let validationResponse: ISignUpValidationResponse = {
            email: defaultValidateEmail,
            password: defaultValidatePassword,
            confirmPassword: defaultValidateConfirmPassword,
            firstName: defaultValidateFirstName,
            lastName: defaultValidateLastName,
            phoneNumber: defaultValidatePhoneNumber,
            phoneCode: defaultValidatePhoneCode
        }
        this.setState({ validateResponse: validationResponse })
    }



    handleInputChange = (e) => {
        const { user, validateResponse } = this.state;
        //Check for updates mail opt
        if (e.target.name == "mailOpt") {
            user.isMailOpt = !user.isMailOpt;
        } else {
            user[e.target.name] = e.target.value;
        }
        let validationResponse = ValidateSignUpForm(user, validateResponse, false);
        this.setState({ user: user, validateResponse: validationResponse, errorMessage: '', isSignedUp: false })
    }

    handleSignup = (e) => {
        e.preventDefault();
        const { user, validateResponse } = this.state;
        let validationResponse = ValidateSignUpForm(user, validateResponse, true);
        this.setState({ validateResponse: validationResponse, isSignedUp: false, errorMessage: '' })

        if (IsValidSignUp(validationResponse)) {
            user.userName = user.firstName;
            user.referralId = sessionStorage.getItem('referralId') != null ? sessionStorage.getItem('referralId') : null;
            this.props.handleSignUp(user, (response: RegistrationResponseViewModel) => {
                let errorMessage = '';
                if (response.success) {
                    errorMessage = `Thank you for registering with us! Please use the account activation link sent to you at ${user.email} in order to log in`;
                    this.setState({ isSignedUp: true, errorMessage: errorMessage })
                    if (this.props.isCheckout) {
                        sessionStorage.setItem("checkoutRedirect", "1")
                    } else {
                        sessionStorage.removeItem("checkoutRedirect");
                    }
                }
                else if (response.isExisting && !response.success) {
                    errorMessage = `This email is already registered with an account. Please use a different email address and try again.`;
                }
                else {
                    errorMessage = "Oops!, Something went wrong please try again";
                }
                this.setState({ errorMessage: errorMessage })
            })
        }
    }

    onChangeView = (pageName) => {
        this.props.onChangeView(pageName);
        this.resetState();
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
        // this.checkoutClassExistence();
        const { user, phoneCodeOptions, validateResponse, resetFlag, isSignedUp, errorMessage, getEmailForFacebook,
            facebookUser, signUpOTP } = this.state;

        if (this.props.signUp.countryList.countries.length > 0 && phoneCodeOptions.length == 0) {
            this.getPhoneCodeOptions(this.props.signUp.countryList);
        }
        return (<>
            <div className="form-row">
                {!getEmailForFacebook && !signUpOTP &&
                    <SignUpComponent user={user} phoneCodeOptions={phoneCodeOptions}
                        handleInputChange={(e) => this.handleInputChange(e)}
                        onChangeView={(pageName) => this.onChangeView(pageName)} validateResponse={validateResponse}
                        isSignedUp={isSignedUp} errorMessage={errorMessage}
                        isLoading={this.props.isLoading} handleSignup={(e) => this.handleSignup(e)}
                    />
                }
                {getEmailForFacebook &&
                    <GetEmailForFacebook onChangeView={(pageName) => this.onChangeView(pageName)}
                        isSignIn={false} user={facebookUser}
                    />
                }

                {signUpOTP &&
                    <OTPLogin isCheckout={this.props.isCheckout}
                        redirectOnLogin={() => this.props.redirectOnLogin()} checkoutUser={() => { this.props.checkoutUser() }}
                        onChangeView={(pageName) => this.onChangeView(pageName)} isSignUp={true} resetState={this.props.resetState} />
                }

                <div className="col-sm-6 signup-form-right">
                    <div className="sign-up-inner-form">
                        {!signUpOTP ?
                            <button className="btn btn-outline-primary btn-block mb-2" onClick={() => { this.resetState(); this.setState({ signUpOTP: true }) }}><i className="fa fa-mobile fa-lg mr-2" aria-hidden="true"></i>
                            Sign up with Mobile
                            </button>
                            :
                            <button className="btn btn-outline-primary btn-block mb-2" onClick={() => { this.resetState(); this.setState({ signUpOTP: false }) }}><i className="fa fa-envelope-o  mr-2" aria-hidden="true"></i>
                            Sign up with Email
                            </button>
                        }
                        <LoginFacebook getEmail={(user) => this.getEmailForFacebook(user)}
                            redirectOnLogin={() => this.props.redirectOnLogin()} isCheckout={this.props.isCheckout}
                            checkoutUser={() => { this.props.checkoutUser() }} isSignUp={true} />

                        <LoginGoogle redirectOnLogin={() => this.props.redirectOnLogin()} isCheckout={this.props.isCheckout}
                            checkoutUser={() => { this.props.checkoutUser() }} isSignUp={true} />
                    </div>
                </div>
            </div>
        </>);
    }

    resetState = () => {
        GetDefaultSignupValidationValues(this.state.validateResponse);
        const { user } = this.state;
        user.email = "";
        user.password = "";
        user.confirmPassword = "";
        user.phoneCode = "";
        user.phoneNumber = "";
        user.firstName = "";
        user.lastName = "";
        user.isMailOpt = false;

        this.setState({ user: user, resetFlag: false, signUpOTP: false }, () => {
            (e) =>
                this.props.resetStateChanged(e);
        })
    }

    getPhoneCodeOptions(countriesOptions) {
        if (countriesOptions && countriesOptions.countries) {
            let phoneCodeOptions = countriesOptions.countries
                .filter(item => item.phonecode && item.name)
                .map(item => {
                    return <>
                        <option value={item.phonecode + "~" + item.altId}>
                            {item.name + " (" + item.phonecode + ")"}
                        </option>
                    </>
                });
            this.setState({ phoneCodeOptions: phoneCodeOptions })
        }
    }
}

export default SignUp;