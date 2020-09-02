import * as React from "react";
import OtpInput from 'react-otp-input';
import CountryDataViewModel from "../../../models/Country/CountryDataViewModel";
import { SendAndValidateOTPFormModel } from "shared/models/SendAndValidateOTPFormModel";
import { IOTPLoginValidationResponse } from "../../../models/Auth/ValidationResponse";
import Countdown from "react-countdown-now";

interface IProps {
    countryOptions: CountryDataViewModel;
    user: SendAndValidateOTPFormModel;
    handleInputChange: (event: any) => void;
    sendOTP: (event: any, clearOTP: boolean) => void;
    isLoading: boolean;
    validateResponse: IOTPLoginValidationResponse;
    showOTPValidationForm: boolean;
    handleOTPChange: (otp: number) => void;
    isOTPInvalid: boolean;
    showInputForm: (event: any) => void;
    otpCountDown: number;
    onChangeView?: (typeName: string) => void;
    isSignUp?: boolean;
}
const OTPInputForm: React.FC<IProps> = (props: IProps) => {

    const { countryOptions, user, handleInputChange, isLoading, validateResponse, sendOTP, showOTPValidationForm,
        handleOTPChange, isOTPInvalid, showInputForm, otpCountDown, isSignUp } = props;
    const [phoneCodeOptions, setPhoneCodeOptions] = React.useState([]);
    const [isCountCompleted, setCountDownCompleted] = React.useState(false);


    React.useEffect(() => {
        getPhoneCodeOptions(countryOptions);
    }, [countryOptions])
    const getPhoneCodeOptions = (countriesOptions: CountryDataViewModel) => {
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
            setPhoneCodeOptions(phoneCodeOptions);
        }
    }

    const showCountDown = ({ minutes, seconds, completed }) => {
        if (completed) {
            setCountDownCompleted(true);
            return null;
        } else {
            return <span>{minutes}:{seconds}</span>;
        }
    }
    return (
        <>
            {
                (!showOTPValidationForm || isLoading) ?
                    <>
                        <div className="form-row">
                            <div className="form-group col-sm-12">
                                <label >Country/Region</label>
                                <select value={user.phoneCode} onChange={(e) => handleInputChange(e)}
                                    className={"form-control" + ((validateResponse && validateResponse.phoneCode.isInValid) ? " is-invalid" : "")}
                                    name="phoneCode">
                                    <option value="" disabled selected>
                                        Phone Code
                                    </option>
                                    {phoneCodeOptions}
                                </select>
                                {validateResponse && validateResponse.phoneCode.isInValid && <div className="d-block invalid-feedback">
                                    {validateResponse && validateResponse.phoneCode.message}</div>}
                            </div>
                            <div className="form-group col-sm-12">
                                <label >Mobile Number</label>
                                <input type="number" onChange={(e) => handleInputChange(e)} value={user.phoneNumber}
                                    className={"form-control" + ((validateResponse && validateResponse.phoneNumber.isInValid) ? " is-invalid" : "")}
                                    onWheelCapture={e => { e.target.addEventListener("mousewheel", (evt) => { evt.preventDefault() }) }}
                                    name="phoneNumber" id="signup-phoneNumber" required />
                                {validateResponse && validateResponse.phoneNumber.isInValid && <div className="d-block invalid-feedback">
                                    {validateResponse && validateResponse.phoneNumber.message}</div>}
                            </div>
                            <div className="form-group col-sm-12 mb-0">
                                <p>We"ll text you to confirm your number.</p>
                            </div>
                        </div>
                        <div className="form-btn-submit mt-2">
                            <button disabled={isLoading} className="btn btn-primary btn-block btn-lg" type="button" onClick={(e) => sendOTP(e, false)}>
                                {isLoading ?
                                    <>
                                        Please wait ...
                            </> :
                                    isSignUp ? "Sign up" : "Log in"}
                            </button>
                            {!isSignUp ?
                                <p className="mt-3">Don't have account? <a href="javascript:void(0)"
                                    onClick={(e) => props.onChangeView("signup")} className="btn-link ml-2">Sign up
                              </a >
                                </p>
                                :
                                <p className="mt-3">Already have an account? <a href="javascript:void(0)" onClick={(e) => props.onChangeView("login")} className="btn-link">Log in</a></p>
                            }
                        </div>
                    </> :
                    <>
                        <h5>Confirm your number</h5>
                        <p>Enter the code just sent to +{user.phoneCode.split("~")[0]} {user.phoneNumber}: </p>
                        <OtpInput
                            onChange={otp => { handleOTPChange(otp) }}
                            numInputs={6}
                            separator={<span>-</span>}
                            value={user.otp}
                            inputStyle="inputStyle"
                            errorStyle="inputStyle error"
                            hasErrored={isOTPInvalid}
                        />

                        {isOTPInvalid && <p className="text-danger mt-2">Incorrect code</p>}
                        {!isCountCompleted ?
                            <p className="mt-2">Expect OTP in {<Countdown
                                date={otpCountDown}
                                renderer={(e) => showCountDown(e)}
                            />}</p>
                            : <>
                                <p className="mt-3">Didn’t get a text? <a href="javascript:void(0)"
                                    onClick={(e) => { sendOTP(e, true); setCountDownCompleted(false) }} className="btn-link ml-2">Send again
                        </a >
                                </p>

                            </>
                        }
                        <p className="mt-3">
                            <a href="javascript:void(0)" onClick={(e) => showInputForm(e)} className="btn-link mr-2">
                                <i className="fa fa-chevron-left mr-2" aria-hidden="true"></i>Back
                        </a>
                        </p>
                    </>
            }
        </>
    );

}

export default OTPInputForm;