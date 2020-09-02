import * as React from "react";
import { connect } from "react-redux";
import {
    actionCreators as sessionActionCreators,
    ISessionProps,
    ISessionState
} from "shared/stores/Session";
import { IApplicationState } from "../../stores";
import {
    actionCreators as signUpActionCreators,
    ISignUpProps
} from "../../stores/SignUp";
import { bindActionCreators } from "redux";
import SignUp from './SignUp';
import { Modal } from 'antd';
import 'antd/dist/antd.css';
import './SignInSignUp.scss';
import SignIn from "./SignIn";

interface ISignInSignUp {
    isSignUp: boolean;
    history: any;
    closeSignInSignUp: (e: any) => void;
    showSignInModal: boolean;
    isCheckout: boolean;
    checkoutUser?: () => void;
    redirectUrl?: string;
    modalFlag?: boolean;
}

type SignInSignUpProps = ISignInSignUp &
    ISessionProps &
    typeof sessionActionCreators &
    ISignUpProps &
    typeof signUpActionCreators;

class SignInSignUp extends React.PureComponent<SignInSignUpProps, any> {

    state = {
        tittle: '',
        isSignUp: true,
        showSignInModal: true,
        resetState: false
    }

    componentDidMount = () => {
        this.props.requestCountryData();
        this.setState({
            tittle: this.props.isSignUp ? "Sign Up" : "Log in",
            isSignUp: this.props.isSignUp
        })

    }

    componentDidUpdate = (prevProps) => {
        if (this.props && prevProps.isSignUp !== this.props.isSignUp) {
            this.setState({
                tittle: this.props.isSignUp ? "Sign Up" : "Log in",
                isSignUp: this.props.isSignUp,
            })
        }
    }

    onChangeView = (pageName) => {
        let tittle = "Sign up";
        if (pageName == "login") {
            tittle = "Log in";
        } else if (pageName == "forget") {
            tittle = "Forget Password";
        }
        this.setState({
            isSignUp: (pageName == "login" || pageName == "forget") ? false : true,
            tittle: tittle,
        })
    }

    onCloseSignInSingUp = (e) => {
        this.props && this.props.closeSignInSignUp(e);
        this.setState({ resetState: true })
        // this.setState((state) => ({ isSignUp: !state.isSignUp }))
    }

    resetStateChanged = (e) => {
        this.setState({ resetState: false })
    }

    redirectOnLogin = () => {
        //admin redirect
        if (this.props.session.isAuthenticated && this.props.redirectUrl && this.props.redirectUrl.indexOf("admin") >= -1) {

            let popUpBlocked = window.open(
                this.props.redirectUrl.replace("userAltId", this.props.session.user.altId),
                '_blank' // <- This makes it open in a new window.
            );
            if (popUpBlocked == null) {
                alert("Please allow pop-ups for this page to continue ")
            }
        }
        this.onCloseSignInSingUp(this);
    }

    removeLoginSignupHash = () => {
        this.setState({ isSignUp: !this.props.modalFlag });
        window.history.pushState('', null, window.location.href.split('#')[0])
    }

    public render() {
        if (!this.props.session.isAuthenticated && this.state.showSignInModal) {
            return (<>
                <Modal
                    title={this.state.tittle}
                    centered
                    footer={null}
                    wrapClassName="signup-form"
                    visible={this.props.showSignInModal}
                    onCancel={(e) => this.onCloseSignInSingUp(e)}
                    maskClosable={false}
                    afterClose={() => this.removeLoginSignupHash()}
                >
                    {
                        this.state.isSignUp ?
                            <SignUp signUp={this.props.signUp} onChangeView={(pageName) =>
                                this.onChangeView(pageName)} handleSignUp={this.props.register} isCheckout={this.props.isCheckout}
                                resetState={this.state.resetState} resetStateChanged={(e) => { this.resetStateChanged(e) }}
                                isLoading={this.props.signUp.isLoading} redirectOnLogin={() => this.redirectOnLogin()}
                                checkoutUser={() => { this.props.checkoutUser() }}
                            />
                            :
                            <SignIn onChangeView={(pageName) => this.onChangeView(pageName)}
                                resetState={this.state.resetState} resetStateChanged={(e) => { this.resetStateChanged(e) }}
                                isCheckout={this.props.isCheckout} checkoutUser={() => { this.props.checkoutUser() }}
                                redirectOnLogin={() => this.redirectOnLogin()}
                            />
                    }
                </Modal >
            </>
            );
        } else {
            return <></>
        }
    }
}

export default connect(
    (state: IApplicationState, ownProps) => ({
        session: state.session,
        signUp: state.signUp,
        ...ownProps
    }),
    dispatch =>
        bindActionCreators(
            { ...sessionActionCreators, ...signUpActionCreators },
            dispatch
        )
)(SignInSignUp);