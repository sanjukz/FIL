import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import FacebookProvider, { Login } from "react-facebook";
import { FacebookSignInResponseViewModel } from "../../../models/SocialSignIn/FacebookSignInResponseViewModel";
import FacebookSignInFormDataViewModel from "../../../models/SocialSignIn/FacebookSignInFormDataViewModel";
import { IApplicationState } from "../../../stores";
import * as SocialLoginStore from "../../../stores/SocialLogin";

interface IProps {
    getEmail: (userData: FacebookSignInFormDataViewModel) => void;
    isCheckout?: boolean;
    checkoutUser?: () => void;
    redirectOnLogin: () => void;
    isSignUp?: boolean;
}

type SocialLoginProps = SocialLoginStore.ISocialLoginState &
    typeof SocialLoginStore.actionCreators & IProps;

const LoginFacebook: React.FC<SocialLoginProps> = (props: SocialLoginProps) => {
    const [isLoading, setLoading] = React.useState(false);

    const { getEmail, isSignUp } = props;
    const responseFacebook = (responseJson) => {
        setLoading(true);
        console.log(responseJson);
        var userData = new FacebookSignInFormDataViewModel();
        userData.email = responseJson.profile.email ? responseJson.profile.email : "";
        userData.firstName = responseJson.profile.first_name;
        userData.lastName = responseJson.profile.last_name;
        userData.socialLoginId = responseJson.profile.id;
        userData.referralId = sessionStorage.getItem('referralId') != null ? sessionStorage.getItem('referralId') : null;
        props.facebookLogin(
            userData,
            (response: FacebookSignInResponseViewModel) => {
                if (response.success) {
                    localStorage.setItem("userToken", response.session.user.altId);
                    if (props.isCheckout) {
                        props.checkoutUser();
                    } else {
                        props.redirectOnLogin();
                    }
                }
                else if (!response.success && response.isEmailReqd) {
                    getEmail(userData);
                }
                setLoading(false);

            }
        );
    }

    return (
        <div>
            <FacebookProvider appId="300711604617534">
                <Login scope="email,public_profile" onResponse={responseFacebook}>
                    <button disabled={isLoading} type="button" className="btn btn-primary btn-block fb-btn">
                        {isLoading ? "Please wait .." : <>
                            <i className="fa fa-facebook-f mr-2"></i>{isSignUp ? "Sign up with Facebook" : "Log in with Facebook"}</>}
                    </button>
                </Login>
            </FacebookProvider>
        </div>
    );
}

export default connect(
    (state: IApplicationState) => state.socialLogin,
    SocialLoginStore.actionCreators
)(LoginFacebook);