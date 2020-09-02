import { autobind } from "core-decorators";
import * as React from "react";
import { actionCreators as sessionActionCreators, ISessionProps } from "shared/stores/Session";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Link, RouteComponentProps } from "react-router-dom";
import { IApplicationState } from "../stores";
import * as Checkoutstore from "../stores/Checkout";
import { UserCheckoutResponseViewModel } from "../models/UserCheckoutResponseViewModel";
import * as getCheckoutModel from "../utils/CheckoutProvider";
import FeelLoader from "../components/Loader/FeelLoader";
import "../scss/_checkout.scss";
import Metatag from "../components/Metatags/Metatag";
import { checkIsLiveOnlineEvents } from "../../ClientApp/utils/TicketCategory/ItineraryProvider";
import { gets3BaseUrl } from "../utils/imageCdn";
import * as parse from "url-parse";

type CheckoutProps =
    Checkoutstore.ICheckoutLoginProps
    & typeof Checkoutstore.actionCreators
    & ISessionProps
    & typeof sessionActionCreators
    & RouteComponentProps<{}>;

class Checkout extends React.Component<CheckoutProps, any> {
    public constructor(props) {
        super(props);
        this.state = {
            currentPage: "checkout",
            default: 0
        };
    }

    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }
        let urlData = parse(location.search, location, true);
        if (urlData.query.user) {
            this.setState({ userAltId: urlData.query.user }, () => {
                this.validateUser(this.props.session, true);
            })
        } else {
            this.validateUser(this.props.session);
        }
    }

    public showPage(page, e) {
        e.preventDefault();
        this.props.history.push("/login/forgot-password");
    }

    public render() {
        if (this.props.checkout.fetchCountriesSuccess) {
            return <div>
                <Metatag url={this.props.location.pathname} title="Checkout Page" />
                <div className="container sign-form pt-3 pb-5 inner-banner">
                    <div className="card">
                        <div className="card-header h5 bg-white">
                            Please Sign In <small className="float-right text-muted"><i className="fa fa-lock text-success"></i> Secure</small>
                        </div>
                        <div className="card-footer">
                            <Link to="/itinerary"><button type="button" className="btn btn-outline-dark">Cancel</button></Link>
                            <small className="float-right">Questions? Write to us at <a href="mailto:support@feelitLIVE.com" className="btn btn-link"><img alt="Feel Mail Icon" src={`${gets3BaseUrl()}/feel-mail-icon.png`} /></a></small>
                        </div>
                    </div>
                </div>
            </div >
        } else {
            return <div>
                <FeelLoader />
            </div>;
        }
    }

    @autobind
    private checkoutResponseAction(response: UserCheckoutResponseViewModel) {
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
                this.props.requestCountryData();
            }
        }
    }

    validateUser = (session, isAuth = false) => {
        if (session.isAuthenticated || isAuth) {
            if (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') {
                let userCheckoutData = getCheckoutModel.getCheckoutModel(null, isAuth ? this.state.userAltId : this.props.session.user.altId);
                this.props.checkoutLoginToDeliveryOption(userCheckoutData, (response: UserCheckoutResponseViewModel) => {
                    this.checkoutResponseAction(response);
                });
            } else {
                this.props.history.replace("/");
            }
        } else {
            this.props.requestCountryData();
        }
    }
}

export default connect(
    (state: IApplicationState) => ({ session: state.session, checkout: state.checkout }),
    (dispatch) => bindActionCreators({ ...sessionActionCreators, ...Checkoutstore.actionCreators }, dispatch)
)(Checkout);