import * as React from "react";
import { connect } from "react-redux";
import { GoogleSignInResponseViewModel } from "../../../models/SocialSignIn/GoogleSignInResponseViewModel";
import { GoogleSignInFormDataViewModel } from "../../../models/SocialSignIn/GoogleSignInFormDataViewModel";
import { IApplicationState } from "../../../stores";
import * as SocialLoginStore from "../../../stores/SocialLogin";
import { Modal } from 'antd';
import { GoogleLogin } from 'react-google-login';


interface IProps {
    redirectOnLogin: () => void;
    isCheckout: boolean;
    checkoutUser: () => void;
    isSignUp?: boolean;
}
type SocialLoginProps = SocialLoginStore.ISocialLoginState &
    typeof SocialLoginStore.actionCreators &
    IProps;

const LoginGoogle: React.FC<SocialLoginProps> = (props: SocialLoginProps) => {

    const [isLoading, setLoading] = React.useState(false);

    const responseGoogle = (googleUser) => {
        setLoading(true);
        console.log(googleUser);
        var res = new GoogleSignInFormDataViewModel();
        res.email = googleUser.profileObj.email;
        res.firstName = googleUser.profileObj.givenName;
        res.lastName = googleUser.profileObj.familyName;
        res.SocialLoginId = googleUser.profileObj.googleId;
        res.referralId = sessionStorage.getItem('referralId') != null ? sessionStorage.getItem('referralId') : null;
        props.googleLogin(res, (response: GoogleSignInResponseViewModel) => {
            if (response.success) {
                localStorage.setItem("userToken", response.session.user.altId);
                if (props.isCheckout) {
                    props.checkoutUser();
                } else {
                    props.redirectOnLogin();
                }
            }
            setLoading(false);

        });
    }

    return (
        <div>
            <GoogleLogin
                clientId="757107720206-q87mgtl574s1jla8d9l7hfm918ohsjo6.apps.googleusercontent.com"
                render={renderProps => (
                    <button className={isLoading ? "btn btn-outline-danger btn-block mt-2 disabled" : "btn btn-outline-danger btn-block fa-google-cus mt-2"}
                        onClick={renderProps.onClick} disabled={renderProps.disabled}>{isLoading ? "Please wait.." : props.isSignUp ? "Sign up with Google" : "Log in with Google"}</button>
                )}
                onSuccess={responseGoogle}
                cookiePolicy={'single_host_origin'}
                icon={false}
            />
        </div>
    );
}

export default connect(
    (state: IApplicationState) => state.socialLogin,
    SocialLoginStore.actionCreators
)(LoginGoogle);
