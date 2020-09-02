import * as React from "react";
import { Link } from "react-router-dom";
import * as numeral from "numeral";
import { ReactHTMLConverter } from "react-html-converter/browser";
import * as PubSub from 'pubsub-js';
import * as getSymbolFromCurrency from 'currency-symbol-map';
import { gets3BaseUrl } from "../../utils/imageCdn";

export default class ItineraryComponent extends React.Component<any, any> {
  public constructor(props) {
    super(props);
    this.state = {
      cartData: "",
      enableCheckout: false,
      s3BaseUrl: gets3BaseUrl()
    };
  }

  public componentDidMount() {
    if (
      localStorage.getItem("cartItems") != null &&
      localStorage.getItem("cartItems") != "0" && 
      localStorage.getItem("cartItems") != "undefined"
    ) {
      var data = JSON.parse(localStorage.getItem("cartItems"));
      //check if cart contains Tiqets Place if yes remove other places
      data.sort(function (a, b) {
        if (a.timeStamp < b.timeStamp) return -1;
        if (a.timeStamp > b.timeStamp) return 1;
        return 0;
      });
      let lastCartItemAdded = data[data.length - 1];
      if (lastCartItemAdded && lastCartItemAdded.isTiqetsPlace) {
        // last item added is Tiqets one
        data = data.filter(item => {
          return item.timeStamp == lastCartItemAdded.timeStamp;
        });
      } else {
        data = data.filter(item => {
          // remove if there is tiqets place in cart
          return item.isTiqetsPlace == false;
        });
      }
      this.loadCartItems(data);
      this.setState({ cartData: data, enableCheckout: true });
    }
    localStorage.setItem("cartItems", JSON.stringify(data));
    PubSub.subscribe("UPDATE_CART_DATA_EVENT", this.subscriberData.bind(this));
  }

  public subscriberData(msg, data) {
    var that = this;
    if (
      localStorage.getItem("cartItems") != null &&
      localStorage.getItem("cartItems") != "" &&
      localStorage.getItem("cartItems") != "0"
    ) {
      var data = JSON.parse(localStorage.getItem("cartItems"));
      this.setState({ cartData: data });
    } else {
      this.setState({ cartData: "" });
    }
    if ((window as any).location.href.indexOf("payment") > -1) {
      this.setState({ isPaymentPage: true });
    }
  }

  public loadCartItems(data) {
    var that = this;
    if (data != "") {
      this.setState({ cartData: data });
    } else {
      this.props.disableCheckout();
      this.setState({ cartData: "" });
    }
  }

  public removeItemFromCart(item, e) {
    e.preventDefault();
    var cartData = this.state.cartData;
    var cartItems = cartData.filter(function (val) {
      return val.altId != item;
    });
    localStorage.setItem("cartItems", JSON.stringify(cartItems));
    this.loadCartItems(cartItems);
    PubSub.publish("UPDATE_NAV_CART_DATA_ON_CART_REMOVE_EVENT", 1);
  }
  public removeCategory(eventTicketAttributeId, e) {
    e.preventDefault();
    var cartData = this.state.cartData;
    var cartItems = cartData.filter(function (val) {
      return val.eventTicketAttributeId != eventTicketAttributeId;
    });
    localStorage.setItem("cartItems", JSON.stringify(cartItems));
    this.loadCartItems(cartItems);
    PubSub.publish("UPDATE_NAV_CART_DATA_ON_CART_REMOVE_EVENT", 1);
  }

  public getTotalCartAmount() {
    var cartData = this.state.cartData;
    var currencies = cartData.map(val => val.currencyName);
    var currencyTypes = currencies.filter(function (value, index, self) {
      return self.indexOf(value) === index;
    });

    var totalCartAmount = 0;
    if (currencyTypes.length > 1) {
      return <p></p>;
    } else {
      for (var i = 0; i < cartData.length; i++) {
        totalCartAmount += cartData[i].quantity * cartData[i].pricePerTicket;
      }
      var symbol = getSymbolFromCurrency(cartData[0].currencyName);
      return (
        <div>
          <div className="col-xs-10">Cart Subtotal</div>{" "}
          <div className="col-xs-2">
            {" "}
            {symbol +
              numeral(totalCartAmount).format("0.00") +
              " " +
              cartData[0].currencyName}
          </div>
        </div>
      );
    }
  }

  public showTotal() {
    var cartData = this.state.cartData;
    var currencies = cartData.map(val => val.currencyName);
    var currencyTypes = currencies.filter(function (value, index, self) {
      return self.indexOf(value) === index;
    });
    var symbol = getSymbolFromCurrency(cartData[0].currencyName);
    var totalCartAmount = 0;
    if (currencyTypes.length > 1) {
      return (
        <b>
          Slow down there, globetrotter! Your cart currently contains multiple
          currencies. Please modify your purchase so that all items are in the
          same currency.
        </b>
      );
    } else {
      for (var i = 0; i < cartData.length; i++) {
        totalCartAmount += cartData[i].quantity * cartData[i].pricePerTicket;
      }
      return (
        <h4 className="m-0">
          Total:{" "}
          {symbol +
            numeral(totalCartAmount).format("0.00") +
            " " +
            cartData[0].currencyName}
        </h4>
      );
    }
  }

  public parseDateLocal(s) {
    var b = s.split(/\D/);
    return new Date(
      b[0],
      b[1] - 1,
      b[2],
      b[3] || 0,
      b[4] || 0,
      b[5] || 0,
      b[6] || 0
    );
  }

  public render() {
    var data = this.state.cartData;
    var that = this;
    var isMultipleCurrencies = false;
    const converter = ReactHTMLConverter();
    converter.registerComponent("Ticket", ItineraryComponent);
    if (data != "") {
      // To sort according to date and time to create itinerary
      data.sort(function (a, b) {
        var keyA = new Date(a.eventStartDate),
          keyB = new Date(b.eventStartDate);
        if (keyA < keyB) return -1;
        if (keyA > keyB) return 1;
        return 0;
      });
      var currencies = data.map(val => val.currencyId && val.currencyName);
      var currencyTypes = currencies.filter(function (value, index, self) {
        return self.indexOf(value) === index;
      });
      if (currencyTypes.length > 1) {
        isMultipleCurrencies = true;
      }
      var unique = Array.from(new Set(data.map(item => item.altId)));
      var cartItems = unique.map(function (altId) {
        var categories = data.filter(function (cat) {
          return cat.altId == altId;
        });
        var categoryCost = 0;
        var categoryData = categories.map(function (val) {
          categoryCost += val.quantity * val.pricePerTicket;
          var symbol = getSymbolFromCurrency(val.currencyName);
          return (
            <div className="row small">
              <div className="col-md-10 col-sm-12">
                {val.ticketCategoryName +
                  " (" +
                  val.quantity +
                  " x " +
                  symbol +
                  numeral(val.pricePerTicket).format("0.00") +
                  ")"}
              </div>
              <div className="col-md-2 col-sm-12 text-md-right">
                {!val.isItinerary && (
                  <span className="float-left">
                    <a
                      href="#"
                      className="pink-color"
                      onClick={that.removeCategory.bind(
                        that,
                        val.eventTicketAttributeId
                      )}
                    >
                      Remove
                    </a>
                  </span>
                )}
                <span className="float-right">
                  {symbol +
                    numeral(val.quantity * val.pricePerTicket).format("0.00") +
                    " " +
                    val.currencyName}
                </span>
              </div>
            </div>
          );
        });
        var eventInformation = categories[0];
        var symbol = getSymbolFromCurrency(eventInformation.currencyName);
        var date =
          new Date(eventInformation.selectedDate)
            .toDateString()
            .split(" ")
            .join(", ")
            .substring(0, 8) +
          " " +
          new Date(eventInformation.selectedDate)
            .toDateString()
            .split(" ")
            .join(", ")
            .substring(9);

        return (
          <div>
            <div className="media">
              <img
                className="mr-3 d-none d-sm-block"
                src={
                  `${that.state.s3BaseUrl}/places/tiles/` +
                  eventInformation.altId.toUpperCase() +
                  `-ht-c1.jpg`
                }
                onError={e => {
                  e.currentTarget.src = `${that.state.s3BaseUrl}/places/tiles/${eventInformation.subCategory}-placeholder.jpg`;
                }}
                alt="Generic placeholder image"
                width="120"
              />
              <div className="media-body">
                <div className="mb-2">
                  {eventInformation.name}
                  {!categories[0].isItinerary && eventInformation.selectedDate && (
                    <span className="pink-color pl-2">
                      <i className="fa fa-calendar"></i>{" "}
                      {date +
                        " | " +
                        numeral(
                          new Date(eventInformation.selectedDate).getHours()
                        ).format("00") +
                        ":" +
                        numeral(
                          new Date(eventInformation.selectedDate).getMinutes()
                        ).format("00")}
                    </span>
                  )}
                  {categories[0].isItinerary && (
                    <span className="pink-color pl-2">
                      <i className="fa fa-calendar"></i>{" "}
                      {date +
                        " | " +
                        (categories[0].visitStartTime.split(":").length > 2
                          ? categories[0].visitStartTime.split(":")[0] +
                          ":" +
                          categories[0].visitStartTime.split(":")[1]
                          : "") +
                        " - " +
                        (categories[0].visitEndTime.split(":").length > 2
                          ? categories[0].visitEndTime.split(":")[0] +
                          ":" +
                          categories[0].visitEndTime.split(":")[1]
                          : "")}
                    </span>
                  )}
                  <div className="float-md-right">
                    <a
                      href="#"
                      className="btn btn-sm pl-0 pink-color"
                      onClick={that.removeItemFromCart.bind(that, altId)}
                    >
                      Remove
                    </a>
                    {symbol +
                      numeral(categoryCost).format("0.00") +
                      " " +
                      eventInformation.currencyName}
                  </div>
                </div>
                <div className="text-muted">
                  {eventInformation.venue != undefined &&
                    eventInformation.city != undefined && eventInformation.categoryId != 98 && (
                      <p className="m-0">
                        <small>
                          <i className="fa fa-map-marker pink-color"></i>{" "}
                          {eventInformation.venue +
                            ", " +
                            eventInformation.city}{" "}
                          -
                          <Link
                            to={
                              "https://www.google.com/maps/search/" +
                              eventInformation.venue
                            }
                            target="_blank"
                          >
                            <i className="fa fa-map-marker ml-1"></i>View on Map
                          </Link>
                        </small>
                      </p>
                    )}
                  {categoryData}
                  {converter.convert(
                    categories[0].eventTermsAndConditions != "NA"
                  ) &&
                    converter.convert(
                      categories[0].eventTermsAndConditions != ""
                    ) &&
                    converter.convert(
                      categories[0].eventTermsAndConditions != null
                    ) && (
                      <p className="small text-info">
                        <i className="fa fa-file-text" aria-hidden="true"></i>{" "}
                        <Link
                          data-toggle="modal"
                          data-target="#exampleModalLong"
                          to="#tnc"
                        >
                          Terms &amp; Conditions
                        </Link>
                      </p>
                    )}

                  <div
                    className="modal fade"
                    id="exampleModalLong"
                    role="dialog"
                    aria-labelledby="exampleModalLongTitle"
                    aria-hidden="true"
                  >
                    <div className="modal-dialog" role="document">
                      <div className="modal-content">
                        <div className="modal-header">
                          <h5
                            className="modal-title"
                            id="exampleModalLongTitle"
                          >
                            Terms and Conditions
                          </h5>
                          <button
                            type="button"
                            className="close"
                            data-dismiss="modal"
                            aria-label="Close"
                          >
                            <span aria-hidden="true">&times;</span>
                          </button>
                        </div>
                        <div className="modal-body">
                          {converter.convert(
                            categories[0].eventTermsAndConditions
                          )}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            {altId != unique[unique.length - 1] && <hr />}
          </div>
        );
      });
      var isItinerary = JSON.parse(localStorage.getItem("cartItems")).filter(
        function (item) {
          return item.isItinerary == true;
        }
      );
      return (
        <div>
          <div className="card-body">{cartItems}</div>
          <div className="card-body border-top text-right">
            {this.showTotal()}
          </div>
          {this.state.enableCheckout && (
            <div className="card-footer gradient-bg text-right">
              <Link
                to="/"
                className="btn btn-outline-primary text-uppercase mr-1"
              >
                Feel More
              </Link>
              {isMultipleCurrencies === false && (
                <a
                  href="javascript:void(0)"
                  onClick={(e) => this.props.checkoutUser(e)}
                  className="btn site-primery-btn text-uppercase"
                >
                  Place Order
                </a>
              )}
            </div>
          )}
        </div>
      );
    } else {
      return (
        <div className="card-body">
          There are no items in your itinerary! Browse some of our featured
          experiences.
        </div>
      );
    }
  }
}