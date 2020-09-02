import * as React from 'react';
import { SignUpUserModel } from "../../../models/Auth/UserModel";
import { ISignUpValidationResponse } from "../../../models/Auth/ValidationResponse";
import { Alert } from 'antd';

interface SignUpProps {
    user: SignUpUserModel;
    phoneCodeOptions: string[];
    handleInputChange: (event: any) => void;
    onChangeView: (typeName: any) => void;
    handleSignup: (e: any) => void;
    validateResponse: ISignUpValidationResponse;
    errorMessage: string;
    isSignedUp: boolean;
    isLoading: boolean;
}

const checkoutClassExistence = () => {
    var cls = document.getElementsByClassName("ant-modal-mask");
    if (cls[0].className === "ant-modal-mask") {
        window.history.pushState('', null, window.location.href.split('#')[0]);
    } else if (cls[0].className === "ant-modal-mask ant-modal-mask-hidden") {
        window.location.hash = "#signup";
    }
}

const SignUpComponent = (props: SignUpProps) => {

    const { user, phoneCodeOptions, handleInputChange, validateResponse, errorMessage, isSignedUp, isLoading, handleSignup } = props;
    const customValidations = (validateResponse != null && validateResponse.password &&
        validateResponse.password.customValidations) ? validateResponse.password.customValidations : null;
    let strongPassword = (customValidations != null && customValidations.length > 0 &&
        customValidations.filter((item) => { return item.isInvalid == false }).length >= 4)
    React.useEffect(() => {
        window.location.hash = "#signup";
        return () => {
            window.history.pushState('', null, window.location.href.split('#')[0]);
        }
    }, [])
    return (<div className="col-sm-6 signup-form-left">
        <form className="needs-validation" onSubmit={(e) => handleSignup(e)} noValidate>

            {
                errorMessage != '' &&
                < Alert
                    message={isSignedUp ? "Success" : "Error"}
                    description={errorMessage}
                    type={isSignedUp ? "success" : "error"}
                    showIcon
                    className="mb-3 py-1"
                />
            }
            <div className="sign-up-inner-form">

                <div className="form-row">
                    <div className="form-group col-sm-6">
                        <label >First Name</label>
                        <input type="text" value={user.firstName} className={"form-control" + ((validateResponse && validateResponse.firstName.isInValid) ? " is-invalid" : "")}
                            name="firstName" id="signup-fname" required onChange={e => handleInputChange(e)} />
                        {validateResponse && validateResponse.firstName.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.firstName.message}</div>}
                    </div>

                    <div className="form-group col-sm-6">
                        <label >Last Name</label>
                        <input type="text" value={user.lastName} className={"form-control" + ((validateResponse && validateResponse.lastName.isInValid) ? " is-invalid" : "")} id="signup-lname"
                            name="lastName" required onChange={e => handleInputChange(e)} />
                        {validateResponse && validateResponse.lastName.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.lastName.message}</div>}
                    </div>

                    <div className="form-group col-sm-12">
                        <label >Email</label>
                        <input type="email" value={user.email} className={"form-control" + ((validateResponse && validateResponse.email.isInValid) ? " is-invalid" : "")} id="signup-email"
                            name="email" required onChange={e => handleInputChange(e)} />
                        {validateResponse && validateResponse.email.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.email.message}</div>}
                    </div>

                    <div className="form-group col-sm-6">
                        <label >Country/Region</label>
                        <select className={"form-control" + ((validateResponse && validateResponse.phoneCode.isInValid) ? " is-invalid" : "")} value={user.phoneCode} onChange={e => handleInputChange(e)}
                            name="phoneCode">
                            <option value="" disabled selected>
                                Phone Code
                      </option>
                            {phoneCodeOptions}
                        </select>
                        {validateResponse && validateResponse.phoneCode.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.phoneCode.message}</div>}
                    </div>
                    <div className="form-group col-sm-6">
                        <label >Mobile Number</label>
                        <input
                            type="number"
                            min="0"
                            pattern="[0-9]*"
                            value={user.phoneNumber}
                            className={"form-control" + ((validateResponse && validateResponse.phoneNumber.isInValid) ? " is-invalid" : "")}
                            onWheelCapture={e => { e.target.addEventListener("mousewheel", (evt) => { evt.preventDefault() }) }}
                            name="phoneNumber"
                            id="signup-phoneNumber"
                            required onChange={e => handleInputChange(e)}
                            onKeyPress={(e: any) => {
                                e = e || window.event;
                                var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
                                var charStr = String.fromCharCode(charCode);
                                if (!charStr.match(/^[0-9]+$/))
                                    e.preventDefault();
                            }}
                        />
                        {validateResponse && validateResponse.phoneNumber.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.phoneNumber.message}</div>}
                    </div>

                    <div className="form-group col-sm-6">
                        <label >Password</label>
                        <input type="password" value={user.password} className={"form-control" + ((validateResponse && validateResponse.password.isInValid) ? " is-invalid" : "")} id="signup-password"
                            name="password" required onChange={e => handleInputChange(e)} autoComplete="new-password" />
                        {validateResponse && validateResponse.password.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.password.message}</div>}
                        {user.password != "" && customValidations &&
                            <div className="pt-1">

                                <small className={customValidations[0].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                    <i className={customValidations[0].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>
                            Password strength:{customValidations[0].isInvalid ? " weak" : " strong"}
                                </small>
                                {!strongPassword &&
                                    <>
                                        <small className={customValidations[1].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                            <i className={customValidations[1].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Can't contain your name or email
                                </small>

                                        <small className={customValidations[2].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                            <i className={customValidations[2].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Atleast 8 characters</small>

                                        <small className={customValidations[3].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                            <i className={customValidations[3].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Contains a number or symbol</small>

                                        <small className={customValidations[4].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                            <i className={customValidations[4].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Contains uppercase and lowercase letters
                            </small>
                                    </>
                                }
                            </div>
                        }
                    </div>
                    <div className="form-group  col-sm-6">
                        <label >Confirm Password</label>
                        <input type="password" value={user.confirmPassword} className={"form-control" + ((validateResponse && validateResponse.confirmPassword.isInValid) ? " is-invalid" : "")}
                            name="confirmPassword" id="signup-confirmpassword" required onChange={e => handleInputChange(e)} />
                        {validateResponse && validateResponse.confirmPassword.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.confirmPassword.message}</div>}
                    </div>


                    <div className="col-sm-12">
                        <p className="small">We"ll send you marketing promotions, special offers, inspiration and policy updates via email</p>

                        <p className="form-group small">
                            <div className="form-check">
                                <input className="form-check-input" type="checkbox" checked={user.isMailOpt}
                                    onChange={e => handleInputChange(e)} name="mailOpt" id="invalidCheck" required />
                                <label className="form-check-label">
                                    I wish to receive marketing messages from FeelitLIVE. I can opt out of recieving these at any time in my account setting. </label>
                            </div>
                        </p>
                        <p className="small">By selecting <b>Agree and continue</b> below, I agree to the FeelitLIVE <a href="/terms" target="_blank" className="btn-link"> Terms of Service</a> and
                    <a href="/privacy-policy" target="_blank" className="btn-link"> Privacy Policy</a> </p>

                        <div className="form-btn-submit">
                            <button disabled={isLoading} className="btn btn-primary btn-block btn-lg" type="submit">
                                {isLoading ?
                                    <>
                                        <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                         Please wait ...
                            </> :
                                    "Agree and Continue"}
                            </button>
                            <p className="mt-3">Already have an account? <a href="javascript:void(0)" onClick={(e) => props.onChangeView("login")} className="btn-link">Log in</a></p>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div >)
}
export default SignUpComponent;