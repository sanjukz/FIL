import * as React from "react";
import { IOTPLoginValidationResponse } from "../../../models/Auth/ValidationResponse";
import { LoginWithOTPFormModel } from "shared/models/LoginWithOTPFormModel";
import { Alert } from 'antd';

interface IProps {
    user: LoginWithOTPFormModel;
    handleInputChange: (event: any) => void;
    handleSignUp: (e) => void;
    isLoading: boolean;
    validateResponse: IOTPLoginValidationResponse;
    isEmailAlreadyRegistered: boolean;
}
const OTPSignUpForm: React.FC<IProps> = (props: IProps) => {

    const { user, handleInputChange, isLoading, validateResponse, handleSignUp, isEmailAlreadyRegistered } = props;


    return (
        <>
            {
                isEmailAlreadyRegistered &&
                < Alert
                    message={"Error"}
                    description={`Already ${user.email} is registered with another phone number, please use another email`}
                    type={"error"}
                    showIcon
                    className="mb-3 py-1"
                />
            }
            <h5>Finish Signing up</h5>
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
            </div>

            <div className="form-btn-submit mt-2">
                <button disabled={isLoading} className="btn btn-primary btn-block btn-lg" type="button" onClick={(e) => handleSignUp(e)}>
                    {isLoading ?
                        <>
                            Please wait ...
                            </> :
                        "Continue"}
                </button>
            </div>
        </>
    );

}

export default OTPSignUpForm;