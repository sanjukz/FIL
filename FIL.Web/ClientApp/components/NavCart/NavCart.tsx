import * as numeral from "numeral";
import * as React from "react";
import { Link } from "react-router-dom";
import * as PubSub from "pubsub-js";
import * as getSymbolFromCurrency from "currency-symbol-map";
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class NavCart extends React.Component<any, any> {
  public constructor(props) {
    super(props);
    this.state = {
      cartData: "",
      isRedirect: true,
      isSort: 0,
      isOrderConfirmation: false,
      s3BaseUrl: gets3BaseUrl(),
    };
  }

  public UNSAFE_componentWillMount() {
    this.setState({ cartData: "" });
    PubSub.subscribe(
      "UPDATE_NAV_CART_DATA_EVENT",
      this.subscriberData.bind(this)
    );
    PubSub.subscribe(
      "UPDATE_NAV_CART_DATA_ON_CART_REMOVE_EVENT",
      this.subscribeNavCartData.bind(this)
    );
  }

  public subscriberData(msg, data) {
    var that = this;
    if (
      localStorage.getItem("cartItems") != null &&
      localStorage.getItem("cartItems") != "0"
    ) {
      var data = JSON.parse(localStorage.getItem("cartItems"));
      this.setState({ cartData: data });
    }
  }

  public subscribeNavCartData(msg, data) {
    var that = this;
    if (
      localStorage.getItem("cartItems") != null &&
      localStorage.getItem("cartItems") != "0"
    ) {
      var data = JSON.parse(localStorage.getItem("cartItems"));
      this.setState({ cartData: data });
    } else {
      localStorage.removeItem("cartItems");
      this.setState({ cartData: null });
    }
  }

  public componentDidMount() {
    if ((window as any).location.href.indexOf("order-confirmation") > -1) {
      this.setState({ isOrderConfirmation: true });
    }
    if (
      localStorage.getItem("cartItems") != null &&
      localStorage.getItem("cartItems") != "0" &&
      localStorage.getItem("cartItems") !== "undefined"
    ) {
      var data = JSON.parse(localStorage.getItem("cartItems"));
      this.loadCartItems(data);
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

  public removeItemFromCart(item, e) {
    if (
      localStorage.getItem("cartItems") != null &&
      localStorage.getItem("cartItems") != "0"
    ) {
      var cartData = JSON.parse(localStorage.getItem("cartItems"));
      var cartItems;
      for (var k = 0; k < cartData.length; k++) {
        if (cartData[k].eventDetailId == item.eventDetailId) {
          cartData[k].quantity = 0;
          localStorage.setItem("cartItems", JSON.stringify(cartData));
        }
      }
      localStorage.setItem(
        "cartItems",
        JSON.stringify(
          JSON.parse(localStorage.getItem("cartItems")).filter(function(item) {
            return item.quantity > 0;
          })
        )
      );
      this.loadCartItems(cartData);

      PubSub.publish("UPDATE_CART_COUNT_EVENT", 1);
      PubSub.publish("UPDATE_CART_DATA_EVENT", 1);
    }
  }

  public getUnique(data) {
    return data.filter(function(x, i) {
      return data.indexOf(x) === i;
    });
  }

  public showOrders() {
    localStorage.setItem("isShow", "true");
  }

  public render() {
    if (
      this.state.cartData !== "" &&
      this.state.cartData !== "[]" &&
      this.state.cartData !== null &&
      this.state.isOrderConfirmation === false
    ) {
      var data = this.state.cartData.filter(function(item) {
        return item.quantity > 0;
      });
      var quantity = 0;
      var totalquantity = this.state.cartData.map(function(item) {
        return (quantity += item.quantity);
      });
      var that = this;
      var unique = Array.from(new Set(data.map((item) => item.eventDetailId)));
      var eventDetailsIds = data.map((item) => item.eventDetailId);
      unique = that.getUnique(eventDetailsIds);
      var cartItems = unique.map(function(eventdetailId) {
        var categories = data.filter(function(cat) {
          return cat.eventDetailId == eventdetailId;
        });
        var symbol = getSymbolFromCurrency(categories[0].currencyName);
        var categoryData = categories.map(function(val) {
          return (
            <small>
              <div>
                {val.ticketCategoryName +
                  " (" +
                  val.quantity +
                  " x " +
                  symbol +
                  numeral(val.pricePerTicket).format("0.00") +
                  ")"}
                <span className="pull-right">
                  {symbol +
                    numeral(val.quantity * val.pricePerTicket).format("0.00") +
                    " " +
                    categories[0].currencyName}
                </span>
              </div>
            </small>
          );
        });
        var imageSrc = categories[0].altId
          ? `${that.state.s3BaseUrl}/places/tiles/` +
            categories[0].altId.toUpperCase() +
            `-ht-c1.jpg`
          : "";
        return (
          <li className="clearfix">
            {(window as any).location.href.indexOf("payment") == -1 && (
              <a
                href="javascript:void(0)"
                onClick={that.removeItemFromCart.bind(that, categories[0])}
                className="close"
              >
                x
              </a>
            )}
            <div className="float-left">
              <img
                className="rounded"
                src={imageSrc}
                onError={(e) => {
                  e.currentTarget.src = `${that.state.s3BaseUrl}/places/tiles/${
                    categories[0].subCategory
                  }-placeholder.jpg`;
                }}
                alt="Generic placeholder image"
                width="120"
              />
            </div>
            <div className="float-right">
              <span>
                <span className="mr-3 d-inline-block">
                  {categories[0].name}
                </span>
                <br />
                {categoryData}
              </span>
            </div>
          </li>
        );
      });
      return (
        <>
          <a
            href="#"
            className="btn btn-link p-0 ml-4"
            id="feelitembag"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            <img
              src={`${this.state.s3BaseUrl}/fil-images/icon/shopping-bag.svg`}
              alt="FIL Shoping Bag"
              width="18"
            />
          </a>

          {/* <Link to="/itinerary" data-toggle="dropdown" className="nav-link" role="button" aria-expanded="false">
                    {(quantity > 0) && <img src={`${this.state.s3BaseUrl}/header/cart-icon-fill-v1.png`} width="23" alt="Feel Cart Icon" />}
                    {(quantity === 0) && <img src={`${this.state.s3BaseUrl}/header/cart-icon-blank-v1.png`} width="23" alt="Feel Cart Icon" />}
                </Link> */}
          <ul className="dropdown-menu dropdown-menu-right dropdown-cart p-4">
            <div className="cart-items">{cartItems}</div>
            <li>
              {quantity > 0 && (
                <Link
                  to="/itinerary"
                  className="btn btn-block btn-primary mt-4"
                  aria-label="checkout"
                >
                  Checkout
                </Link>
              )}
              {quantity == 0 && <div>No items in your cart</div>}
            </li>
            <li className="header-list">
              <i>
                <img
                  src={`${
                    this.state.s3BaseUrl
                  }/fil-images/icon/shopping-bag.svg`}
                  alt="Feel Cart Icon"
                  width="16"
                  className="mr-2"
                />
              </i>
              <Link to="/itinerary">Bag ({quantity})</Link>
            </li>
            {this.props.isAuth === false && (
              <li className="header-list">
                <i className="fa fa-inbox" aria-hidden="true" />
                <a
                  onClick={(e) => {
                    this.props.showSignInSignUp(true, null);
                  }}
                  href="javascript:void(0)"
                >
                  Orders
                </a>
              </li>
            )}
            {this.props.isAuth === true && (
              <li className="header-list">
                <i className="fa fa-inbox" aria-hidden="true" />
                <Link to="/account/orders">Orders</Link>
              </li>
            )}
            {this.props.isAuth === false && (
              <li className="header-list">
                <i className="fa fa-cog" aria-hidden="true" />
                <a
                  onClick={(e) => {
                    this.props.showSignInSignUp(true, null);
                  }}
                  href="javascript:void(0)"
                >
                  Account
                </a>
              </li>
            )}
            {this.props.isAuth === true && (
              <li className="header-list">
                <i className="fa fa-cog" aria-hidden="true" />
                <Link to="/account">Account</Link>
              </li>
            )}
          </ul>
        </>
      );
    } else {
      return (
        <>
          <a
            href="#"
            className="btn btn-link p-0 ml-4"
            id="feelitembag"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            <img
              src={`${this.state.s3BaseUrl}/fil-images/icon/shopping-bag.svg`}
              alt="FIL Shoping Bag"
              width="18"
            />
          </a>
          {/* <Link to="/itinerary" data-toggle="dropdown" className="nav-link" role="button" aria-expanded="false">
                    <img src={`${this.state.s3BaseUrl}/header/cart-icon-blank-v1.png`} width="23" alt="Feel Cart Icon" />
                </Link> */}
          <ul className="dropdown-menu dropdown-menu-right dropdown-cart p-4">
            <div>
              <li>
                <div>No items in your cart</div>
              </li>
              <li className="header-list">
                <img
                  src={`${
                    this.state.s3BaseUrl
                  }/fil-images/icon/shopping-bag.svg`}
                  alt="Feel Cart Icon"
                  width="16"
                  className="mr-2"
                />
                <Link to="/itinerary">Bag (0)</Link>
              </li>
              {this.props.isAuth === false && (
                <li className="header-list">
                  <i className="fa fa-inbox" aria-hidden="true" />
                  <a
                    onClick={(e) => {
                      this.props.showSignInSignUp(true, null);
                    }}
                    href="javascript:void(0)"
                  >
                    Orders
                  </a>
                </li>
              )}
              {this.props.isAuth === true && (
                <li className="header-list">
                  <i className="fa fa-inbox" aria-hidden="true" />
                  <Link to="/account/orders" onClick={this.showOrders}>
                    Orders
                  </Link>
                </li>
              )}
              {this.props.isAuth === false && (
                <li className="header-list">
                  <i className="fa fa-cog" aria-hidden="true" />
                  <a
                    onClick={(e) => {
                      this.props.showSignInSignUp(true, null);
                    }}
                    href="javascript:void(0)"
                  >
                    Account
                  </a>
                </li>
              )}
              {this.props.isAuth === true && (
                <li className="header-list">
                  <i className="fa fa-cog" aria-hidden="true" />
                  <Link to="/account">Account</Link>
                </li>
              )}
            </div>
          </ul>
        </>
      );
    }
  }
}
