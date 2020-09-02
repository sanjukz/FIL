import * as React from 'react';
import { ForgetPaswordModel } from "../../../models/Auth/UserModel";
import { IResetPasswordValidationResponse } from "../../../models/Auth/ValidationResponse";

interface ResetPasswordProps {
    user: ForgetPaswordModel;
    handleInputChange: (event: any) => void;
    validateResponse: IResetPasswordValidationResponse;
}

const ResetPasswordForm: React.FC<ResetPasswordProps> = (props: ResetPasswordProps) => {
    const { user, handleInputChange, validateResponse } = props;
    const customValidations = (validateResponse != null && validateResponse.password &&
        validateResponse.password.customValidations) ? validateResponse.password.customValidations : null;
    let strongPassword = (customValidations != null && customValidations.length > 0 &&
        customValidations.filter((item) => { return item.isInvalid == false }).length >= 4)
    return (<>
        <div className="form-group">
            <label >Password</label>
            <input type="password" id="reset-password" name="password" value={user.password}
                className={"form-control" + ((validateResponse && validateResponse.password.isInValid) ? " is-invalid" : "")}
                required onChange={e => handleInputChange(e)} />
            {
                validateResponse && validateResponse.password.isInValid &&
                <div className="d-block invalid-feedback">{validateResponse && validateResponse.password.message} </div>
            }
            {strongPassword &&
                <>
                    <small className={customValidations[0].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                        <i className={customValidations[0].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>
                            Password strength:{customValidations[0].isInvalid ? " weak" : " strong"}
                    </small>
                </>
            }
        </div>

        <div className="form-group">
            <label >Confirm Password</label>
            <input type="password" id="reset-confirmPassword" name="confirmPassword" value={user.confirmPassword}
                className={"form-control" + ((validateResponse && validateResponse.confirmPassword.isInValid) ? " is-invalid" : "")}
                required onChange={e => handleInputChange(e)} />
            {
                validateResponse && validateResponse.confirmPassword.isInValid &&
                <div className="d-block invalid-feedback">{validateResponse && validateResponse.confirmPassword.message} </div>
            }

            {user.password != "" && customValidations &&
                <div className="py-1">

                    {!strongPassword &&
                        <>
                            <small className={customValidations[0].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                <i className={customValidations[0].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>
                            Password strength:{customValidations[0].isInvalid ? " weak" : " strong"}
                            </small>

                            <small className={customValidations[1].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                <i className={customValidations[1].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Can't contain your name or email
                        </small>

                            <small className={customValidations[2].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                <i className={customValidations[2].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Atleast 8 characters</small>

                            <small className={customValidations[3].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                <i className={customValidations[3].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Contains a number or symbol</small>

                            <small className={customValidations[4].isInvalid ? "d-block text-danger" : "d-block text-success"}>
                                <i className={customValidations[4].isInvalid ? "fa fa-times mr-2" : "fa fa-check mr-2"} aria-hidden="true"></i>Contains uppercase and lowercase letters</small>
                        </>
                    }
                </div>
            }
        </div>
        <div className="form-btn-submit">
            <button className="btn btn-primary btn-block btn-lg" type="submit">Reset Password</button>
        </div>
    </>)

}

export default ResetPasswordForm;