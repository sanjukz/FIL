import * as React from "react";
import { connect } from "react-redux";
import { Route, Switch, Redirect } from "react-router-dom";
import { RouteComponentProps } from "react-router-dom";
import { bindActionCreators } from "redux";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import "shared/styles/globalStyles/main.scss";
import Layout from "./components/Layout";
import { IApplicationState } from "./stores";
import {
    actionCreators as categoryActionCreators,
    ICategoryProps
} from "./stores/Category";
import DeliveryOptionsView from "./views/DeliveryOptions";
import PaymentView from "./views/Payment";
import PaymentForm from "./views/PaymentForm";
import PaymentResponse from "./views/PaymentResponse";
import PaymentError from "./views/PaymentError";
import CheckoutView from "./views/Checkout";	
import HomeView from "./views/HomeV1";
import AccountView from "./views/AccountV1";
import TicketPurchaseSelectionView from "./views/TicketPurchaseSelection";
import ItineraryView from "./views/Itinerary";
import EventLearnPage from "./views/EventLearnPage";
import ActiveReview from "./views/ActiveReview";
import OrderConfirmation from "./views/OrderConfirmation";
import ActivateUser from "./views/Activate";
import ResetPassword from "./components/SignInSignUp/ResetPassword";
import ComingSoon from "./views/ComingSoon";
import TermsOfUse from "../ClientApp/components/Footer/TermsOfUse";
import AboutUs from "../ClientApp/components/Footer/AboutUs";
import PrivacyPolicy from "../ClientApp/components/Footer/PrivacyPolicy";
import ItineraryInputForm from "./views/ItineraryInputForm";
import ItineraryResult from "./views/ItineraryResult";
import PrintAtHome from "./views/PrintAtHome";
import Careers from "./views/Careers";
import FeelForBusiness from "./views/FeelForBusiness";
import FeelForAdvertise from "./views/FeelForAdvertise";
import * as PubSub from "pubsub-js";
import ItineraryGuestDetails from "./views/ItineraryGuestDetails";
import CategoryContainer from "./views/CategoryContainer";
import CountryContainer from "./views/CountryContainer";
import HostAFeel from "./views/HostAFeel";
import LiveOnline from "../ClientApp/components/LiveOnline/LiveOnline";
import LiveStream from "./views/LiveStream";
import StreamOnline from "../ClientApp/components/LiveOnline/StreamOnline";
import NoMatchPage from "./views/NoMatchPage";
import { GetLanguage } from "./utils/GetLanguage";
import TicketAlertV1 from "./views/TicketAlertV1";

interface IAppProps {
    gtmId: string;
}

type AppProps = IAppProps &
    ISessionProps &
    ICategoryProps &
    typeof sessionActionCreators &
    typeof categoryActionCreators &
    RouteComponentProps<IAppProps>;

class App extends React.Component<AppProps, {}> {
    pad(value) {
        return value < 10 ? '0' + value : value;
    }
    createOffset(date) {
        var sign = (date.getTimezoneOffset() > 0) ? "-" : "+";
        var offset = Math.abs(date.getTimezoneOffset());
        var hours = this.pad(Math.floor(offset / 60));
        var minutes = this.pad(offset % 60);
        return sign + hours + ":" + minutes;
    }
    public componentDidMount() {
        if (!this.props.session.isLoading) {
            this.props.getSession();
        }
        if (window) {
            document.cookie = `utcTimeOffset = ${this.createOffset(new Date())}`
        }
        //setting global language for regional sites
        GetLanguage();
    }

    public render() {
        PubSub.publish("UPDATE_CURRENCY_TAB");
        if (this.props.location.pathname.includes("live-online")) {
            return <Route path="/live-online" component={LiveOnline} />;
        }
        if (this.props.location.pathname.includes("stream-online")) {
            return <Route path="/stream-online" component={StreamOnline} />;
        }
        if (this.props.location.pathname.includes("test-stream")) {
            return <Route path="/test-stream" component={StreamOnline} />;
        }
        else if (this.props.location.pathname.includes("pah")) {
            return <Route path="/pah/:transactionId" component={PrintAtHome} />;
        } else {
            return (
                <Layout
                    gtmId={this.props.gtmId}
                    categories={this.props.category.categories}
                    url={this.props.location.pathname}
                >
                    <Switch>
                        <Route exact path="/" component={HomeView} />
                        <Route exact path="/careers" component={Careers} />
                        <Route exact path="/feelforbusiness" component={FeelForBusiness} />
                        <Route
                            path="/delivery-options/:transactionAltId"
                            component={DeliveryOptionsView}
                        />
                        <Route path="/payment/:transactionAltId" component={PaymentView} />
                        <Route exact path="/paymentform" component={PaymentForm} />
                        <Route path="/response" component={PaymentResponse} />
                        <Route path="/advertiseafeel" component={FeelForAdvertise} />
                        <Route path="/pgerror" component={PaymentError} />
                        <Route
                            exact
                            path="/ticket-purchase-selection/:eventId"
                            component={TicketPurchaseSelectionView}
                        />
                        <Route exact path="/itinerary" component={ItineraryView} />
                        <Route
                            exact
                            path="/place/:slug/:category/:parent"
                            component={EventLearnPage}
                        />
                        <Route
                            exact
                            path="/event/:slug/:altId"
                            component={EventLearnPage}
                        />
                        <Route exact path="/review/:ratingAltId" component={ActiveReview} />
                        <Route path="/checkout" component={CheckoutView} />
                        <Route
                            path="/order-confirmation/:transactionAltId"
                            component={OrderConfirmation}
                        />
                        <Route path="/reset-password" component={ResetPassword} />
                        <Route path="/account" component={AccountView} />
                        <Route exact path="/activate/:altId" component={ActivateUser} />
                        <Route exact path="/coming-soon" component={ComingSoon} />
                        <Route exact path="/terms" component={TermsOfUse} />
                        <Route exact path="/about-us" component={AboutUs} />
                        <Route exact path="/privacy-policy" component={PrivacyPolicy} />
                        <Route
                            path="/itineraryplanner-feelr"
                            component={ItineraryInputForm}
                        />
                        <Route
                            path="/feel-itineraryplanner"
                            component={ItineraryInputForm}
                        />
                        <Route exact path="/itinerary-result" component={ItineraryResult} />
                        <Route
                            path="/customerDetails/:transactionAltId"
                            component={ItineraryGuestDetails}
                        />
                        <Route path="/c/*" component={CategoryContainer} />
                        <Route path="/country/*" component={CountryContainer} />
                        <Route path="/host-a-feel" component={HostAFeel} />
                        <Route path="/create-online-experience" component={LiveStream} />
                        <Route
                            path="/ticket-alert"
                            component={TicketAlertV1}
                        />
                        <Route component={NoMatchPage} />
                    </Switch>
                </Layout>
            );
        }
    }
}

export default connect(
    (state: IApplicationState, ownProps) => ({
        session: state.session,
        category: state.category,
        ...ownProps
    }),
    dispatch =>
        bindActionCreators(
            { ...sessionActionCreators, ...categoryActionCreators },
            dispatch
        )
)(App);
