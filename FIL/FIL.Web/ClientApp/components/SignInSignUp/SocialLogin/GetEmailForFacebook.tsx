import * as React from 'react';
import { isEmail, isEmpty } from 'validator';
import FacebookSignInFormDataViewModel from "../../../models/SocialSignIn/FacebookSignInFormDataViewModel";
import { FacebookSignInResponseViewModel } from "../../../models/SocialSignIn/FacebookSignInResponseViewModel";
import { IApplicationState } from "../../../stores";
import * as SocialLoginStore from "../../../stores/SocialLogin";
import { connect } from "react-redux";
import { Alert } from 'antd';

interface GetEmailProps {
    onChangeView: (typeName: string) => void;
    isSignIn: boolean;
    user: FacebookSignInFormDataViewModel;
}
type IProps = SocialLoginStore.ISocialLoginState &
    typeof SocialLoginStore.actionCreators & GetEmailProps;

const GetEmailForFacebook: React.FC<IProps> = (props: IProps) => {
    const { onChangeView, isSignIn, user } = props;
    const [email, setEmail] = React.useState("");
    const [error, setError] = React.useState(false);
    const [isResponseError, setResponseError] = React.useState(false);
    const [errorMessage, setErrorMessage] = React.useState("");

    const [isLoading, setLoading] = React.useState(false);

    const handleChange = (e) => {
        if (!isEmail(e.target.value)) {
            setError(true);
            setErrorMessage("Enter a valid email")
        } else {
            setError(false);
            setErrorMessage("")
        }
        setEmail(e.target.value)
        // if (!isEmpty(responseErrorMessage)) {
        //     resetError()
        // }
    }

    const handleSubmit = () => {
        if (isEmpty(email)) {
            setError(true);
            setErrorMessage("Email is required")
        }
        else if (!isEmail(email)) {
            setError(true);
            setErrorMessage("Enter a valid email")
        }
        setResponseError(false);

        if (!isEmpty(email) && isEmail(email)) {
            setLoading(true);
            user.email = email;
            props.facebookLogin(
                user,
                (response: FacebookSignInResponseViewModel) => {
                    if (response.success) {
                        localStorage.setItem("userToken", response.session.user.altId);
                        this.props.redirectOnLogin();
                    }
                    else {
                        setResponseError(true);
                    }
                    setLoading(false);

                })
            setError(false);
            setErrorMessage("")
        }

    }
    return (<>
        <div className="col-sm-6 signup-form-left">

            <div className="sign-up-inner-form">
                {
                    isResponseError &&
                    < Alert
                        message={"Error"}
                        description={"Oops!, something wrong with server, please go back and try with other" + isSignIn ? " sign in " : " sign up" + " options"}
                        type={"error"}
                        showIcon
                    />
                }
                <div className="form-group">
                    <p><b>Please confirm your email to continue </b></p>
                    <label >Email</label>
                    <input type="email" className={"form-control" + (error ? " is-invalid" : "")}
                        id="signup-email" name="email" value={email} required onChange={e => handleChange(e)} />
                    {
                        error &&
                        <div className="d-block invalid-feedback">{errorMessage} </div>
                    }
                </div>

                <div className="form-btn-submit">
                    <button disabled={isLoading} className="btn btn-primary btn-block btn-lg" onClick={handleSubmit} type="button">
                        {isLoading ? "Please wait.." : "Submit"}
                    </button>
                    <a href="javascript:void(0)" onClick={(e) => onChangeView(isSignIn ? "login" : "signup")} className="btn-link ml-2">
                        <p className="mt-3"><i className="fa fa-chevron-left mr-2" aria-hidden="true"></i> Back</p></a >

                </div>
            </div>
        </div>
    </>);
}

export default connect(
    (state: IApplicationState) => state.socialLogin,
    SocialLoginStore.actionCreators
)(GetEmailForFacebook);