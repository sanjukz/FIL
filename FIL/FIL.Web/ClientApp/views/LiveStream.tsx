import * as React from "react";
import HostAFeelNavigation from "../components/HostAFeel/HostAFeelNavigation";
import HostAFeelLanding from "../components/HostAFeel/HostAFeelLanding";
import LiveStreamTileSection from "../components/HostAFeel/LiveStreamTileSection";
import LiveOnlineLanding from "../components/HostAFeel/LiveOnlineLanding";
import { RouteComponentProps } from "react-router-dom";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { IApplicationState } from "../stores";
import { ISessionProps, actionCreators as sessionActionCreators } from "shared/stores/Session";
import SignInSignUp from "../components/SignInSignUp"
type HostAFeelProps = ISessionProps &
    typeof sessionActionCreators &
    RouteComponentProps<{}>;

class LiveStream extends React.Component<HostAFeelProps, any> {
    state = {
        isSiginSignUpShow: false,
        isLoginPage: false,
        showSignInModal: true,
        url: ""
    }
    render() {
        return (
            <>
                {
                    this.state.isSiginSignUpShow &&
                    <SignInSignUp isSignUp={!this.state.isLoginPage} history={null} showSignInModal={this.state.showSignInModal}
                        closeSignInSignUp={(e) => this.closeSignInSignUp(e)} isCheckout={false} redirectUrl={this.state.url} />

                }
                <HostAFeelNavigation session={this.props.session} logoutAction={(e) => this.onLogout(e)} isLiveStream={true}
                    showSignInSignUp={(isLoginPage, url) => this.showSignInSignUp(isLoginPage, url)} />

                {/* <HostAFeelLanding history={this.props.history} session={this.props.session} isLiveStream={true} showSignInSignUp={(isLoginPage, url) => this.showSignInSignUp(isLoginPage, url)} /> */}
                <LiveOnlineLanding history={this.props.history} session={this.props.session} isLiveStream={true} showSignInSignUp={(isLoginPage, url) => this.showSignInSignUp(isLoginPage, url)} />

                <div className="hostAfeel-page fil-landing-page fil-site fil-home-page " data-aos="fade-up" data-aos-duration="1000">
                    <section className="container-fluid">
                        <LiveStreamTileSection />
                    </section>
                </div>
            </>
        );
    }
    onLogout = (e) => {
        this.setState({ isSiginSignUpShow: false })
        this.props.logout();
    }
    showSignInSignUp = (isLoginPage, url) => {
        this.setState({ isSiginSignUpShow: true, isLoginPage: isLoginPage, showSignInModal: true, url: url })
    }
    closeSignInSignUp = (e) => {
        this.setState({ showSignInModal: false })
    }
}

const mapState = (state: IApplicationState) => {
    return {
        session: state.session
    };
};

const mapDispatch = dispatch => bindActionCreators({ ...sessionActionCreators }, dispatch);

export default connect(mapState, mapDispatch)(LiveStream);