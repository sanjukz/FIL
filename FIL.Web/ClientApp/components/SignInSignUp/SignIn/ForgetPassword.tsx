import * as React from 'react';
import { Alert } from 'antd';
import { isEmail, isEmpty } from 'validator';

interface IForgetProps {
    resetPassword: (email: string) => void;
    resetError: () => void;
    onChangeView: (typeName: string) => void;
    forgetPasswordSucess: boolean;
    isLoading: boolean;
    responseErrorMessage: string;
}
const ForgetPassword: React.FC<IForgetProps> = (props: IForgetProps) => {

    const { resetPassword, onChangeView, forgetPasswordSucess, responseErrorMessage, resetError, isLoading } = props;

    const [email, setEmail] = React.useState("");
    const [error, setError] = React.useState(false);
    const [errorMessage, setErrorMessage] = React.useState("");

    const handleSubmit = () => {
        if (isEmpty(email)) {
            setError(true);
            setErrorMessage("Email is required")
        }
        else if (!isEmail(email)) {
            setError(true);
            setErrorMessage("Enter a valid email")
        }

        if (!isEmpty(email) && isEmail(email)) {
            resetPassword(email);
        }
        if (!isEmpty(responseErrorMessage)) {
            resetError()
        }
    }

    const handleChange = (e) => {
        if (!isEmail(e.target.value)) {
            setError(true);
            setErrorMessage("Enter a valid email")
        } else {
            setError(false);
            setErrorMessage("")
        }
        setEmail(e.target.value)
        if (!isEmpty(responseErrorMessage)) {
            resetError()
        }
    }

    return <>
        <div className="col-sm-6 signup-form-left">
            {
                responseErrorMessage != '' &&
                < Alert
                    message={forgetPasswordSucess ? "success" : "Error"}
                    description={responseErrorMessage}
                    type={forgetPasswordSucess ? "success" : "error"}
                    showIcon
                    className="mb-3 py-1"
                />
            }
            <div className="sign-up-inner-form">

                <div className="form-group">
                    <p>Enter the email address associated with your account, and we will email a link to reset your password</p>
                    <input type="email" className={error ? "form-control is-invalid" : "form-control"}
                        id="signup-email" name="email" value={email} required onChange={e => handleChange(e)} />
                    {
                        error && <div className="d-block invalid-feedback">{errorMessage}</div>
                    }
                </div>

                <div className="form-btn-submit">
                    <button disabled={isLoading} className="btn btn-primary btn-block btn-lg"
                        onClick={() => handleSubmit()} type="button">
                        {isLoading ?
                            <>
                                <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                         Please wait ...
                            </> :
                            "Send reset link"}
                    </button>

                    <a href="javascript:void(0)" onClick={(e) => onChangeView("login")} className="btn-link ml-2">
                        <p className="mt-3"><i className="fa fa-chevron-left mr-2" aria-hidden="true"></i>Back to Login</p>
                    </a >

                </div>
            </div>
        </div>
    </>
}
export default ForgetPassword;