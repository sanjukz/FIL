import * as React from 'react';
import { Alert } from 'antd';
import { SignInUserModel } from "../../../models/Auth/UserModel";
import { ILoginValidationResponse } from "../../../models/Auth/ValidationResponse";

interface SignInProps {
    user: SignInUserModel;
    onChangeView: (typeName: string) => void;
    submitLogin: (e: any) => void;
    handleInputChange: (event: any) => void;
    validateResponse: ILoginValidationResponse;
    errorMessage: string;
    isLoading: boolean;
}

const SignInComponent = (props: SignInProps) => {
    const { user, handleInputChange, validateResponse, errorMessage, isLoading, submitLogin } = props;

    const [showPassword, setshowPassword] = React.useState(false);

    React.useEffect(() => {
            window.location.hash = "#login";
        return  () => {
            window.history.pushState('', null, window.location.href.split('#')[0]);
        }
    },[])
    return <>
        <div className="col-sm-6 signup-form-left">
            <form className="needs-validation" onSubmit={(e) => submitLogin(e)} noValidate autoComplete="off">

                {
                    errorMessage != '' &&
                    < Alert
                        message="Error"
                        description={errorMessage}
                        type="error"
                        showIcon
                        className="mb-3"
                    />
                }

                <div className="sign-up-inner-form">

                    <div className="form-group">
                        <label >Email</label>
                        <input type="email" id="signup-email" name="email" value={user.email}
                            className={"form-control" + ((validateResponse && validateResponse.email.isInValid) ? " is-invalid" : "")}
                            required onChange={e => handleInputChange(e)} />
                        {
                            validateResponse && validateResponse.email.isInValid &&
                            <div className="d-block invalid-feedback">{validateResponse && validateResponse.email.message} </div>
                        }
                    </div>

                    <div className="form-group">
                        <label >Password</label>
                        <div className="position-relative">
                            <i onClick={(e) => setshowPassword(!showPassword)}
                                className={!showPassword ? "position-absolute show-pass text-purple fa fa-eye " : "position-absolute show-pass text-purple fa fa-eye-slash"} aria-hidden="true"></i>


                            <input type={showPassword ? "text" : "password"} id="signup-password" name="password" value={user.password} required
                                onChange={e => handleInputChange(e)}
                                className={"form-control" + ((validateResponse && validateResponse.password.isInValid) ?
                                    " is-invalid" : "")} />
                        </div>

                        {
                            validateResponse && validateResponse.password.isInValid &&
                            <div className="d-block invalid-feedback">{validateResponse && validateResponse.password.message}</div>
                        }
                    </div>

                    <div className="form-btn-submit">
                        <button disabled={isLoading} className="btn btn-primary btn-block btn-lg" type="submit">
                            {isLoading ?
                                <>
                                    Please wait ...
                            </> :
                                "Log in"}
                        </button>

                        <p className="mt-3">Don't have account? <a href="javascript:void(0)"
                            onClick={(e) => props.onChangeView("signup")} className="btn-link ml-2">Sign up
                        </a >
                            <a href="javascript:void(0)" onClick={(e) => props.onChangeView("forget")}
                                className="btn-link float-md-right d-block d-md-inline">Forgot Password?</a>
                        </p>
                    </div>

                </div>
            </form>
        </div>
    </>

}
export default SignInComponent;