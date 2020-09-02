import * as React from 'react';
import { IApplicationState } from "../../stores";
import * as Checkoutstore from "../../stores/Checkout";
import { UserCheckoutResponseViewModel } from "../../models/UserCheckoutResponseViewModel";
import * as getCheckoutModel from "../../utils/CheckoutProvider";
import FeelLoader from "../../components/Loader/FeelLoader";
import { checkIsLiveOnlineEvents } from "../../../ClientApp/utils/TicketCategory/ItineraryProvider";
import { connect } from "react-redux";
import { ISessionState } from 'shared/stores/Session';
import SignInSignUp from "../SignInSignUp"

interface CheckoutProps {
    history: any;
    session: ISessionState;
    onCloseSignInSignUp: () => void;
}
type IProps = Checkoutstore.ICheckoutLoginState &
    typeof Checkoutstore.actionCreators & CheckoutProps;

class CheckoutComponent extends React.Component<IProps, any>{
    state = {
        isSiginSignUpShow: false,
        isLoginPage: false,
        showSignInModal: true
    }

    checkoutResponseAction = (response: UserCheckoutResponseViewModel) => {
        if (response.success) {
            localStorage.setItem('transactionId', response.transactionId.toString());
            let isLiveOnlineEvents = checkIsLiveOnlineEvents();
            if (isLiveOnlineEvents) {
                this.props.history.push({
                    pathname: `/delivery-options/${response.transactionAltId}`, state: {
                        isPaymentByPass: response.isPaymentByPass ? true : false,
                        StripeAccount: response.stripeAccount,
                    }
                });
            } else {
                this.props.history.push(`/customerDetails/${response.transactionAltId}`);
            }
        } else {
            if (response.isTiqetsOrderFailure) {
                alert("Opps! Something wrong with your selection Please try again");
            } else {
                alert("Invalid credentials");
            }
        }
    }

    checkoutUser = () => {
        if (this.props.session.isAuthenticated) {
            if (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') {
                let userCheckoutData = getCheckoutModel.getCheckoutModel(null, this.props.session.user.altId);
                this.props.checkoutLoginToDeliveryOption(userCheckoutData, (response: UserCheckoutResponseViewModel) => {
                    this.checkoutResponseAction(response);
                });
            } else {
                this.props.history.replace("/");
            }
        } else {
        }
    }

    closeSignInSignUp = (e) => {
        this.setState({ showSignInModal: false })
        this.props.onCloseSignInSignUp();
    }

    componentDidMount = () => {
        if (this.props.session.isAuthenticated) {
            this.checkoutUser()
        }
    }

    render() {
        if (!this.props.session.isAuthenticated) {
            return <>
                <SignInSignUp isSignUp={false} history={null} showSignInModal={this.state.showSignInModal}
                    closeSignInSignUp={(e) => this.closeSignInSignUp(e)} isCheckout={true}
                    checkoutUser={() => this.checkoutUser()}
                />
            </>
        } else {
            return <FeelLoader />
        }
    }

}


export default connect(
    (state: IApplicationState) => state.checkout,
    Checkoutstore.actionCreators
)(CheckoutComponent);