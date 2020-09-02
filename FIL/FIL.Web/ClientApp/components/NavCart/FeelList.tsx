import * as React from "react";
import { bindActionCreators } from "redux";
import { actionCreators as sessionActionCreators } from "shared/stores/Session";
import { IApplicationState } from "../../stores";
import { connect } from "react-redux";
import { gets3BaseUrl } from "../../utils/imageCdn";
import * as PubSub from "pubsub-js";

class FeelList extends React.Component<any, any> {
  public constructor(props) {
    super(props);
    this.state = {
      cartData: "",
      isRedirect: true,
      isSort: 0,
      isPaymentPage: false,
      isOrderConfirmation: false,
      s3BaseUrl: gets3BaseUrl(),
    };
  }

  public UNSAFE_componentWillMount() {
    this.setState({ cartData: "" });
    PubSub.subscribe(
      "UPDATE_NAV_WISHLIST_DATA_EVENT",
      this.subscriberData.bind(this)
    );
  }

  public subscriberData(msg, data) {
    if (
      localStorage.getItem("bespokeItems") != null &&
      localStorage.getItem("bespokeItems") != "0"
    ) {
      var data = JSON.parse(localStorage.getItem("bespokeItems"));

      this.setState({ cartData: data });
    }
    this.setState({ cartError: false });
    if ((window as any).location.href.indexOf("payment") > -1) {
      this.setState({ isPaymentPage: true });
    }
  }

  public subscribeNavCartData(msg, data) {
    var that = this;
    if (
      localStorage.getItem("bespokeItems") != null &&
      localStorage.getItem("bespokeItems") != "0"
    ) {
      var data = JSON.parse(localStorage.getItem("bespokeItems"));
      this.setState({ cartData: data });
    } else {
      localStorage.removeItem("bespokeItems");
      this.setState({ cartData: null });
    }
    this.setState({ cartError: false });
    if ((window as any).location.href.indexOf("payment") > -1) {
      this.setState({ isPaymentPage: true });
    }
  }

  public componentDidMount() {
    if ((window as any).location.href.indexOf("payment") > -1) {
      this.setState({ isPaymentPage: true });
    }
    if ((window as any).location.href.indexOf("order-confirmation") > -1) {
      this.setState({ isOrderConfirmation: true });
    }
    if (
      localStorage.getItem("bespokeItems") != null &&
      localStorage.getItem("bespokeItems") != "0" &&
      localStorage.getItem("bespokeItems") !== "undefined"
    ) {
      var data = JSON.parse(localStorage.getItem("bespokeItems"));
      this.setState({ cartData: data, cartError: false });
    }
  }

  public loadCartItems(data) {
    var that = this;
    if (data != "" && this.state.isOrderConfirmation == false) {
      var cartData;
      if (localStorage.getItem("cartItems") === "") {
        cartData = null;
        localStorage.removeItem("cartItems");
      } else {
        cartData = JSON.parse(localStorage.getItem("cartItems"));
      }
      data.map(function(val) {
        var eventTicketAttributeId = val.eventTicketAttributeId;
        var quantity = val.quantity;
        that.setState({
          [eventTicketAttributeId]: quantity,
          isSort: that.state.isSort + 1,
          cartData: cartData,
        });
      });
    } else {
      this.setState({ cartData: "" });
    }
  }

  public getUnique(data) {
    return data.filter(function(x, i) {
      return data.indexOf(x) === i;
    });
  }

  public addToBespokeItinerary(item, e) {
    if (!this.props.session.isAuthenticated) {
      this.props.goToLogin();
    } else {
      var that = this;
      var bespokeData =
        localStorage.getItem("bespokeItems") != null &&
        localStorage.getItem("bespokeItems") != "0"
          ? JSON.parse(localStorage.getItem("bespokeItems"))
          : [];

      if (bespokeData.length > 0) {
        bespokeData = bespokeData.filter(function(val) {
          return val.userAltId == that.props.session.user.altId;
        });
      }

      var i = -1;
      if (bespokeData && bespokeData.length == 1) {
        var allWishlistexceptCurrent;
        if (
          localStorage.getItem("bespokeItems") !== null &&
          localStorage.getItem("bespokeItems") != "0"
        ) {
          allWishlistexceptCurrent = JSON.parse(
            localStorage.getItem("bespokeItems")
          ).filter(function(val) {
            return val.userAltId !== that.props.session.user.altId;
          });
        }
        var data = [];
        if (allWishlistexceptCurrent.length > 0) {
          allWishlistexceptCurrent.forEach(function(val) {
            data.push(val);
          });
          localStorage.setItem("bespokeItems", JSON.stringify(data));
        } else {
          localStorage.removeItem("bespokeItems");
        }
        that.setState({
          [item.altId]: false,
        });
      } else {
        bespokeData.forEach(function(val) {
          i = i + 1;
          if (val.cartItems.altId == item.altId) {
            bespokeData.splice(i, 1);
          }
        });
        if (
          localStorage.getItem("bespokeItems") !== null &&
          localStorage.getItem("bespokeItems") != "0"
        ) {
          allWishlistexceptCurrent = JSON.parse(
            localStorage.getItem("bespokeItems")
          ).filter(function(val) {
            return val.userAltId !== that.props.session.user.altId;
          });
          allWishlistexceptCurrent.forEach(function(val) {
            bespokeData.push(val);
          });
        }
        localStorage.setItem("bespokeItems", JSON.stringify(bespokeData));
        that.setState({
          [item.altId]: false,
        });
      }
      PubSub.publish("UPDATE_WISHLIST_DATA_EVENT", 1);
      PubSub.publish("UPDATE_EVENTLEARNPAGE_WISHLIST_DATA_EVENT", 1);
      PubSub.publish("UPDATE_WISHLIST_DATA_EVENT_ON_LANDING_PAGES", 1);
      PubSub.publish("UPDATE_WISHLIST_PAGE_DATA_EVENT", 1);
    }
  }
  removeDuplicateItems = (bespokeData) => {
    let items = bespokeData.filter(
      (item, index, self) =>
        index ===
        self.findIndex(
          (value) => value.cartItems.altId === item.cartItems.altId
        )
    );
    return items;
  };
  public render() {
    var that = this;
    if (
      this.state.cartData !== "" &&
      this.state.cartData !== null &&
      this.state.isOrderConfirmation === false
    ) {
      var bespokeData =
        localStorage.getItem("bespokeItems") != null &&
        localStorage.getItem("bespokeItems") != "0"
          ? JSON.parse(localStorage.getItem("bespokeItems"))
          : [];
      if (bespokeData.length > 0) {
        bespokeData = this.removeDuplicateItems(bespokeData);
        bespokeData = bespokeData.filter(function(val) {
          return val.userAltId == that.props.session.user.altId;
        });
      }
      var data = bespokeData.map(function(item) {
        return item.cartItems;
      });
      var cartItems = data.map(function(val) {
        debugger;
        var imageSrc =
          `${gets3BaseUrl()}/places/tiles/` +
          val.altId.toUpperCase() +
          `-ht-c1.jpg`;
        return (
          <li className="clearfix">
            {!that.state.isPaymentPage && (
              <a
                href="javascript:void(0)"
                onClick={that.addToBespokeItinerary.bind(that, val)}
                className="close"
              >
                x
              </a>
            )}
            <div className="float-left">
              <img
                className="rounded"
                src={imageSrc}
                alt=""
                onError={(e) => {
                  e.currentTarget.src = `https://feelitlive.imgix.net/images/places/tiles/${
                    val.subCategorySlug
                  }-placeholder.jpg?auto=format&fit=crop&h=270&w=360&crop=entropy&q=55`;
                }}
              />
            </div>
            <div className="float-right">
              <span>
                {val.name}
                <br />
              </span>
            </div>
          </li>
        );
      });
      return (
        <>
          <a
            href="#"
            className="btn btn-link p-0 ml-3"
            id="feelitembag"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            <img
              src={`${this.state.s3BaseUrl}/icons/feellist.svg`}
              alt="FIL Shoping Bag"
              width="21"
            />
          </a>
          <ul className="dropdown-menu dropdown-cart p-4 dropdown-menu-right">
            {cartItems}
            {data.length == 0 && (
              <li>
                Your feelList is empty! Browse some of our featured
                destinations, or check out our blog for inspiration for your
                next travel experience.
              </li>
            )}
          </ul>
        </>
      );
    } else {
      return (
        <>
          {" "}
          <a
            href="#"
            className="btn btn-link p-0 ml-4"
            id="feelitembag"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            <img
              src={`${this.state.s3BaseUrl}/icons/feellist.svg`}
              alt="FIL Shoping Bag"
              width="21"
            />
          </a>{" "}
          <ul className="dropdown-menu dropdown-cart p-4 dropdown-menu-right">
            <li>
              Your feelList is empty! Browse some of our featured destinations,
              or check out our blog for inspiration for your next travel
              experience.
            </li>
          </ul>
        </>
      );
    }
  }
}
export default connect(
  (state: IApplicationState) => ({ session: state.session }),
  (dispatch) => bindActionCreators({ ...sessionActionCreators }, dispatch)
)(FeelList);
