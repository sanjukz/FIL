import * as React from "react";
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { EventLearnPageViewModel } from "../../../models/EventLearnPageViewModel";
import ReactStars from "react-stars";
import { formatDate, formatCurrency, getBasePrice } from "../../../utils/currencyFormatter";
import * as getSymbolFromCurrency from "currency-symbol-map";
import SocialShare, { ISocialShareProps } from "../../SocialShare/SocialShare";
import { Link } from "react-router-dom";
import BespokeWishListViewModel from "../../../models/BespokeWishList/BespokeWishListViewModel";
import * as PubSub from "pubsub-js";
import ImageAndVideo from "./ImageAndVideo";
import { Rate } from "antd";
import { EventFrequencyType } from '../../../Enum/EventFrequencyType';

interface Iprops {
  eventData: EventLearnPageViewModel;
  isAthenticated: boolean;
}
export default class Banner extends React.PureComponent<Iprops, any> {
  constructor(props) {
    super(props);
    this.state = {
      s3BaseUrl: gets3BaseUrl(),
      wishlistData: null,
      wishListAdded: false,
    };
  }
  public componentDidMount() {
    if (
      localStorage.getItem("currentPlace") != null &&
      localStorage.getItem("currentPlace") != "0"
    ) {
      var data;
      if (localStorage.getItem("bespokeItems") !== null) {
        var data = JSON.parse(localStorage.getItem("bespokeItems")).filter(
          function (val) {
            return val.userAltId == localStorage.getItem("userToken");
          }
        );
      }

      if (
        localStorage.getItem("currentPlace") != null &&
        localStorage.getItem("currentPlace") != "0"
      ) {
        this.setState({
          wishlistData: JSON.parse(localStorage.getItem("currentPlace")),
        });
        var isPresentInBeskeData = [];
        if (localStorage.getItem("bespokeItems") !== null) {
          isPresentInBeskeData = data.filter(function (item) {
            return (
              item.cartItems.altId ===
              JSON.parse(localStorage.getItem("currentPlace")).categoryEvent
                .altId
            );
          });
        }

        if (isPresentInBeskeData.length > 0) {
          this.setState({ isPresentInBespokeData: true });
        }
      }
    }
    PubSub.subscribe(
      "UPDATE_EVENTLEARNPAGE_WISHLIST_DATA_EVENT",
      this.subscriberTitleData.bind(this)
    );
  }

  public subscriberTitleData(msg, data) {
    var that = this;
    if (
      localStorage.getItem("currentPlace") != null &&
      localStorage.getItem("currentPlace") != "0"
    ) {
      var data;
      if (localStorage.getItem("bespokeItems") !== null) {
        var data = JSON.parse(localStorage.getItem("bespokeItems")).filter(
          function (val) {
            return val.userAltId == localStorage.getItem("userToken");
          }
        );
      }

      if (
        localStorage.getItem("currentPlace") != null &&
        localStorage.getItem("currentPlace") != "0"
      ) {
        this.setState({
          wishlistData: JSON.parse(localStorage.getItem("currentPlace")),
        });
        var isPresentInBeskeData = [];
        if (localStorage.getItem("bespokeItems") !== null) {
          isPresentInBeskeData = data.filter(function (item) {
            return (
              item.cartItems.categoryEvent.altId ===
              JSON.parse(localStorage.getItem("currentPlace")).categoryEvent
                .altId
            );
          });
        }

        if (isPresentInBeskeData.length === 0) {
          this.setState({ isPresentInBespokeData: false });
        }
      }
    }
  }

  public addToBespokeItinerary(item, e) {
    if (!this.props.isAthenticated) {
      PubSub.publish("SHOW_LOGIN", 1);
    } else {
      this.setState({ wishListAdded: !this.state.wishListAdded });
      PubSub.publish("UPDATE_NAV_WISHLIST_DATA_EVENT", 1);
    }
  }
  render() {
    const { s3BaseUrl } = this.state;
    let review = this.props.eventData.rating;
    let reviewLength = 1;
    if (review.length !== 0) {
      reviewLength = review.filter(function (item) {
        return item.points > 0;
      }).length;
    }
    let reviewAvg =
      review.length == 0
        ? 0
        : review
          .map((item) => {
            return item.points;
          })
          .reduce((a, b) => {
            return a + b;
          }) / reviewLength;

    let onlineStreamDateString = formatDate(
      this.props.eventData.onlineStreamStartTime
    );
    let onlineStreamDate = onlineStreamDateString.split("~");
    let minPrice = getBasePrice(this.props.eventData.eventTicketAttribute);
    let symbol = getSymbolFromCurrency(this.props.eventData.currencyType.code);
    let SOCIAL_SHARE_LINKS: ISocialShareProps = {
      facebook: {
        quote: "Hey, check out this event.",
        hashtag: "YourWay",
        url: window.location.href,
        className: "mr-2",
      },
      linkedIn: {
        url: window.location.href,
        title: "Hey, check out this event.",
        summary: "Hey, check out this event.",
        source: "www.feelitlive.com",
        className: "mr-2",
      },
      twitter: {
        url: window.location.href,
        title: "Hey, check out this event.",
        hashtags: ["YourWay"],
        related: ["@feelaplace"],
        className: "mr-2",
      },
      whatsApp: {
        url: window.location.href,
        title: "Hey, check out this really nice online event.",
      },
    };

    return (
      <section className="py-4 fil-inner-banner">
        <div className="container position-relative">
          <nav
            aria-label="breadcrumb"
            className="fil-cust-breadcrumb d-none d-md-block"
          >
            <ol className="breadcrumb rounded-0">
              <li className="breadcrumb-item">
                <a href="/">Home</a>
              </li>
              <li className="breadcrumb-item">
                <a
                  href={`c/${this.props.eventData.category.slug}?category=${
                    this.props.eventData.category.id
                    }`}
                >
                  {this.props.eventData.category.displayName}
                </a>
              </li>
              <li className="breadcrumb-item">
                <a
                  href={`/c/${this.props.eventData.category.slug}/${
                    this.props.eventData.subCategory.slug
                    }?category=${this.props.eventData.category.id}&subcategory=${
                    this.props.eventData.subCategory.id
                    }`}
                >
                  {this.props.eventData.subCategory.displayName}
                </a>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                {this.props.eventData.event.name}
              </li>
            </ol>
          </nav>

          <ImageAndVideo eventData={this.props.eventData} />

          <div className="card-deck pt-4 fil-event-detail">
            <div className="card">
              <h2 className="mb-2">{this.props.eventData.event.name}</h2>
              <div className="fil-exp-head">
                <div className="fil-exp-rating d-inline-block">
                  <div className="tils-text">
                    Ratings:
                    <span className="star-rating px-2">
                      <Rate
                        value={reviewAvg}
                        disabled={true}
                        style={{ color: "#572483" }}
                      />
                    </span>
                    | {reviewAvg}+ Reviews
                  </div>
                </div>
                <div className="d-inline-block ml-5">
                  <a href="javascript:void(0)" className="text-body mr-3">
                    <SocialShare
                      socialShareProps={SOCIAL_SHARE_LINKS}
                      isOnlineExperience={true}
                    />
                  </a>
                  <a
                    href="javascript:void(0)"
                    onClick={(e) =>
                      this.addToBespokeItinerary(this.state.wishlistData, this)
                    }
                    className="text-body"
                  >
                    <img
                      src={`${
                        (this.state.isPresentInBespokeData ||
                          this.state.wishListAdded) &&
                          this.props.isAthenticated
                          ? `${s3BaseUrl}/icons/live-tag.svg`
                          : `${s3BaseUrl}/fil-images/icon/bugvibe-inverse.svg`
                        }`}
                      alt=""
                      width="18"
                      className="mr-2"
                    />
                    <u>{!this.state.wishListAdded ? "Save" : "Remove"}</u>
                  </a>
                </div>
              </div>
            </div>
            <div className="card my-auto">
              <div className="fil-event-detail-right">
                {this.props.eventData.eventDetail.eventFrequencyType != EventFrequencyType.OnDemand && <div className="d-sm-inline-block d-block p-o pr-sm-4">
                  <img
                    src={`${s3BaseUrl}/fil-images/fil-exp-schedule.svg`}
                    alt=""
                    className="mr-2"
                  />
                  {this.props.eventData.eventDetail.eventFrequencyType == EventFrequencyType.Single && <span className="align-top d-inline-block">
                    {onlineStreamDate[0]} <br />
                    {onlineStreamDate[1]}{" "}
                    {this.props.eventData.onlineEventTimeZone}
                  </span>}
                  {this.props.eventData.eventDetail.eventFrequencyType == EventFrequencyType.Recurring && <span className="align-top d-inline-block">
                    Recurrent experience <br />
                    <Link to={
                      this.props.eventData.event.eventTypeId != 6 ?
                        `/ticket-purchase-selection/${this.props.eventData.event.altId}` : `/ticket-alert/${this.props.eventData.event.slug}`
                    } className="font-weight-bold"><u>See dates</u></Link>
                  </span>}
                </div>}
                {this.props.eventData.eventDetail.eventFrequencyType == EventFrequencyType.OnDemand && <span className="mr-5 red-tag-ticket-cat rounded px-3 py-1">On Demand</span>}
                {this.props.eventData.event.eventTypeId != 6 && <div className="d-sm-inline-block d-block">
                  <img
                    src={`${s3BaseUrl}/fil-images/fil-exp-price.svg`}
                    alt=""
                    className="mr-2"
                  />
                  <span className="align-top d-inline-block">
                    from <br />{" "}
                    <span className="font-weight-bold">
                      {`${symbol}${formatCurrency(minPrice)}`}
                    </span>{" "}
                    {this.props.eventData.currencyType.code} per person
                  </span>
                </div>}
                <div className="fil-exp-btn d-inline-block float-sm-right text-right">
                  {this.props.eventData.event.eventTypeId != 6 && (
                    <Link
                      to={
                        "/ticket-purchase-selection/" +
                        this.props.eventData.event.altId
                      }
                      className={
                        !this.props.eventData.event.isEnabled
                          ? "btn btn-primary fil-btn disabled"
                          : "btn btn-primary fil-btn"
                      }
                    >
                      {" "}
                      {this.props.eventData.event.isEnabled
                        ? "Book Now"
                        : "Booking Closed"}
                      <img
                        src={`${s3BaseUrl}/fil-images/icon/arrow-right.svg`}
                        className="ml-2"
                        alt=""
                      />
                    </Link>
                  )}
                  {this.props.eventData.event.eventTypeId == 6 && (
                    <Link
                      to={`/ticket-alert/${this.props.eventData.event.slug}`}
                      className={"btn btn-primary fil-btn"}
                    >
                      {" "}
                      Ticket Alert
                    </Link>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    );
  }
}
