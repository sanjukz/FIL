import * as React from "react";
import { Link, RouteComponentProps } from "react-router-dom";
import {
  actionCreators as sessionActionCreators,
  ISessionProps,
} from "shared/stores/Session";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import Metatag from "../components/Metatags/Metatag";
import * as PubSub from "pubsub-js";
import ItineraryComponent from "../components/ReviewItinerary/ItineraryComponent";
import BespokeItinerary from "../components/ReviewItinerary/BespokeItinerary";
import KzLoader from "../components/Loader/KzLoader";
import { gets3BaseUrl } from "../utils/imageCdn";
import "../scss/_buttons.scss";
import "../scss/_custom.scss";
import CheckoutComponent from "../components/CheckOut/CheckoutComponent";

type sessionProps = ISessionProps &
  typeof sessionActionCreators &
  RouteComponentProps<{}>;

export class Itinerary extends React.Component<sessionProps, any> {
  public constructor(props) {
    super(props);
    this.state = {
      enableCheckout: false,
      loaded: false,
      checkoutUser: false,
    };
  }

  public componentDidMount() {
    PubSub.publish("UPDATE_NAV_CART_DATA_EVENT", 1);
    if (
      localStorage.getItem("cartItems") != null &&
      localStorage.getItem("cartItems") != "0" &&
      localStorage.getItem("cartItems") != "[]"
    ) {
      this.setState({ enableCheckout: true, loaded: true });
    } else {
      this.setState({ loaded: true });
    }
  }

  public disableCheckout() {
    this.setState({ enableCheckout: false });
  }
  checkoutUser = (e) => {
    this.setState({ checkoutUser: true });
  };
  onCloseSignInSignUp = () => {
    this.setState({ checkoutUser: false });
  };
  public render() {
    if (this.state.loaded) {
      return (
        <div>
          <Metatag title="Review your itineraries" url="itinerary" />
          <div className="container forgot-password pt-3 pb-5 inner-banner">
            <div className="card">
              <div className="card-header h5 bg-white">
                <img
                  src={`${gets3BaseUrl()}/logos/feelr-logo-1.png`}
                  alt="feelr logo"
                  width="50"
                  className="mr-2"
                />{" "}
                Your Bespoke Itinerary
                <small className="float-right text-muted">
                  <i className="fa fa-lock text-success" /> Secure
                </small>
              </div>
              <ItineraryComponent
                disableCheckout={this.disableCheckout.bind(this)}
                checkoutUser={(e) => this.checkoutUser(e)}
              />

              {!this.state.enableCheckout && (
                <div className="card-footer gradient-bg text-right">
                  <Link
                    to="/"
                    className="btn btn-outline-primary text-uppercase"
                  >
                    Go to Home
                  </Link>
                </div>
              )}
            </div>
            <div className="nav-tab-content">
              {this.props.session.isAuthenticated && (
                <BespokeItinerary session={this.props.session} />
              )}
            </div>
          </div>
          {this.state.checkoutUser && (
            <CheckoutComponent
              session={this.props.session}
              history={this.props.history}
              onCloseSignInSignUp={() => this.onCloseSignInSignUp()}
            />
          )}
        </div>
      );
    } else {
      return (
        <div>
          <KzLoader />
        </div>
      );
    }
  }
}
export default connect(
  (state: IApplicationState) => ({ session: state.session }),
  (dispatch) => bindActionCreators({ ...sessionActionCreators }, dispatch)
)(Itinerary);
