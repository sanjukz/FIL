import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../../stores";
import * as SignUpStore from "../../../stores/SignUp";
import CountryDataViewModel from "../../../models/Country/CountryDataViewModel";
import OTPInputForm from "./OTPInputForm";
import OTPSignUpForm from "./OTPSignUpForm";
import { ValidateOTPLogin, ValidateOTPSignUp, GetDefaultOTPValidationValues } from "../../../utils/SignInSignUp/Validation";
import { SendAndValidateOTPResponseModel } from "shared/models/SendAndValidateOTPResponseModel";
import {
    defaultValidatePhoneCode, defaultValidatePhoneNumber, IOTPLoginValidationResponse, defaultValidateFirstName,
    defaultValidateEmail, defaultValidateLastName
} from "../../../models/Auth/ValidationResponse";
import { LoginWithOTPFormModel } from "shared/models/LoginWithOTPFormModel";
import { LoginWithOTPResponseModel } from "shared/models/LoginWithOTPResponseModel";

interface IProps {
    isCheckout?: boolean;
    checkoutUser?: () => void;
    redirectOnLogin: () => void;
    onChangeView?: (typeName: string) => void;
    isSignUp?: boolean;
    resetState: boolean;
}
type OTPLoginProps = SignUpStore.ISignUpState &
    typeof SignUpStore.actionCreators &
    IProps;

const OTPLogin: React.FC<OTPLoginProps> = (props: OTPLoginProps) => {

    const { isCheckout, checkoutUser, redirectOnLogin, onChangeView, isSignUp, resetState } = props;

    const [user, setUser] = React.useState({
        phoneCode: '',
        phoneNumber: '',
        sendOTP: true,
        token: '',
        otp: null,
        email: '',
        firstName: '',
        lastName: '',
        referralId: null
    });
    const [validateResponse, setValidateResponse] = React.useState(null);
    const [phoneCodeOptions, setPhoneCodeOptions] = React.useState(null);
    const [showOTPValidationForm, setShowOTPValidationForm] = React.useState(false);
    const [isOTPInvalid, setOTPValid] = React.useState(false);
    const [isOTPSignUp, setOTPSignUp] = React.useState(false);
    const [isEmailAlreadyRegistered, setEmailRegistered] = React.useState(false);
    const [otpCountDown, resetOtpCountDown] = React.useState(0);

    React.useEffect(() => {
        props.requestCountryData((response: CountryDataViewModel) => {
            setPhoneCodeOptions(response);
        });
        let validationResponse: IOTPLoginValidationResponse = {
            phoneCode: defaultValidatePhoneCode,
            phoneNumber: defaultValidatePhoneNumber,
            email: defaultValidateEmail,
            firstName: defaultValidateFirstName,
            lastName: defaultValidateLastName
        }
        setValidateResponse(validationResponse);
    }, []);

    React.useEffect(() => {
        let newData = { ...user }
        newData["phoneCode"] = '';
        newData["phoneNumber"] = "";
        newData["sendOTP"] = false;
        newData["otp"] = null;
        newData["email"] = "";
        newData["firstName"] = "";
        newData["lastName"] = "";
        setUser(newData);
        if (validateResponse) {
            let getDefaultValidatoion = GetDefaultOTPValidationValues(validateResponse);
            setValidateResponse(getDefaultValidatoion);
        }
    }, [resetState]);

    const handleInputChange = (e: any, isSignup?: boolean) => {
        let newData = { ...user } //copy the object
        newData[e.target.name] = e.target.value;
        setUser(newData);
        let validationResponse;
        if (isSignup) {
            validationResponse = ValidateOTPSignUp(newData, validateResponse, false);
        } else {
            validationResponse = ValidateOTPLogin(newData, validateResponse, false);
        }
        let newValidationResponseData = { ...validateResponse }
        setValidateResponse(newValidationResponseData);
    }

    const sendOTP = (e, clearOTP) => {
        let validationResponse = ValidateOTPLogin(user, validateResponse, true);
        let newData = { ...validateResponse }
        setValidateResponse(newData);
        setShowOTPValidationForm(false);
        setOTPValid(false);
        if (clearOTP) {
            let newUser = { ...user } //copy the object
            newUser["otp"] = null;
            setUser(newUser);
        }
        if (!validateResponse.phoneNumber.isInValid && !validateResponse.phoneCode.isInValid) {
            user.sendOTP = true;
            props.requestAndValidateOTP(user, (response: SendAndValidateOTPResponseModel) => {
                console.log(response);
                if (response.success && response.isOTPSent) {
                    let newUser = { ...user }
                    newUser.token = response.token;
                    setUser(newUser);
                    resetOtpCountDown(Date.now() + (new Date().getTime() + 30000 - new Date().getTime()));
                    setShowOTPValidationForm(true);
                }
            })
        }
    }

    const handleOTPChange = (otp) => {
        let newData = { ...user }
        newData["otp"] = otp;
        setUser(newData);
        setOTPValid(false);
        if (otp.toString().length == 6) {
            newData.sendOTP = false;
            props.requestAndValidateOTP(newData, (response: SendAndValidateOTPResponseModel) => {
                console.log(response);
                if (response.success && response.isOtpValid) {
                    handleLogin();
                } else if (!response.isOtpValid) {
                    setOTPValid(true);
                }
            })
        }
    }
    //Login with OTP once OTP is validated
    const handleLogin = () => {
        let loginUser: LoginWithOTPFormModel = {
            phoneCode: user.phoneCode,
            phoneNumber: user.phoneNumber
        }
        props.loginWithOtp(loginUser, (response: LoginWithOTPResponseModel) => {
            console.log(response);

            if (response.success) {
                localStorage.setItem("userToken", response.session.user.altId);
                if (isCheckout) {
                    checkoutUser();
                } else {
                    redirectOnLogin();
                }
            }
            else if (response.isAdditionalFieldsReqd) {
                setOTPSignUp(true);
            }
        });
    }

    const handleSignUp = (e) => {
        let validationResponse = ValidateOTPSignUp(user, validateResponse, true);
        let newData = { ...validateResponse }
        setValidateResponse(newData);

        if (!validateResponse.firstName.isInValid && !validateResponse.lastName.isInValid && !validateResponse.email.isInValid) {
            user.referralId = sessionStorage.getItem('referralId') != null ? sessionStorage.getItem('referralId') : null
            props.loginWithOtp(user, (response: LoginWithOTPResponseModel) => {
                console.log(response);

                if (response.success) {
                    localStorage.setItem("userToken", response.session.user.altId);
                    if (isCheckout) {
                        checkoutUser();
                    } else {
                        redirectOnLogin();
                    }
                }
                else if (response.emailAlreadyRegistered) {
                    setEmailRegistered(true);
                }
            });
        }
    }

    const showInputForm = () => {
        setShowOTPValidationForm(false);
        let newData = { ...user }
        newData["otp"] = null;
        setUser(newData);
    }

    return (
        <div className="col-sm-6 signup-form-left">
            {
                !isOTPSignUp ?
                    <OTPInputForm countryOptions={phoneCodeOptions} user={user} handleInputChange={(e) => handleInputChange(e)}
                        isLoading={props.isLoading} sendOTP={(e, clearOTP) => sendOTP(e, clearOTP)} validateResponse={validateResponse}
                        showOTPValidationForm={showOTPValidationForm} handleOTPChange={(otp) => handleOTPChange(otp)}
                        isOTPInvalid={isOTPInvalid} showInputForm={() => showInputForm()} otpCountDown={otpCountDown}
                        onChangeView={(typeName) => onChangeView(typeName)} isSignUp={isSignUp}
                    />
                    :
                    <OTPSignUpForm user={user} handleInputChange={(e) => handleInputChange(e, true)}
                        isLoading={props.isLoading} validateResponse={validateResponse} handleSignUp={(e) => handleSignUp(e)}
                        isEmailAlreadyRegistered={isEmailAlreadyRegistered}
                    />
            }
        </div>
    );

}

export default connect(
    (state: IApplicationState) => state.signUp,
    SignUpStore.actionCreators
)(OTPLogin);
