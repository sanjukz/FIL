import * as React from "react";
import { gets3BaseUrl } from "../../../utils/imageCdn";
import { formatDate, formatCurrency } from "../../../utils/currencyFormatter";
import * as getSymbolFromCurrency from "currency-symbol-map";
import SocialShare, { ISocialShareProps } from "../../SocialShare/SocialShare";
import { Link } from "react-router-dom";
import * as PubSub from "pubsub-js";
import ImageAndVideoV1 from "./ImageAndVideoV1";
import { Rate } from "antd";

const InnerBannerV1 = (props: any) => {
  debugger;
  let review = props.eventData.rating;
  let reviewLength = 1;
  const s3BaseUrl = gets3BaseUrl();
  const [wishlistData, setWishlistData] = React.useState(null);
  const [isPresentInBespokeData, setIsPresentInBeskeData] = React.useState(
    false
  );
  const [wishListAdded, setWishListAdded] = React.useState(false);
  let symbol = getSymbolFromCurrency(props.eventData.currencyType.code);
  let minPrice = props.eventData.eventTicketAttribute
    ? props.eventData.eventTicketAttribute[0].price
    : Math.min.apply(
        null,
        props.eventData.eventTicketAttribute
          .map((item) => {
            return item.price;
          })
          .filter(Boolean)
      ) !== Infinity
    ? Math.min.apply(
        null,
        props.eventData.eventTicketAttribute
          .map((item) => {
            return item.price;
          })
          .filter(Boolean)
      )
    : 0;
  if (review.length !== 0) {
    reviewLength = review.filter(function(item) {
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
  React.useEffect(() => {
    if (
      localStorage.getItem("currentPlace") != null &&
      localStorage.getItem("currentPlace") != "0"
    ) {
      var data;
      if (localStorage.getItem("bespokeItems") !== null) {
        var data = JSON.parse(localStorage.getItem("bespokeItems")).filter(
          function(val) {
            return val.userAltId == localStorage.getItem("userToken");
          }
        );
      }

      if (
        localStorage.getItem("currentPlace") != null &&
        localStorage.getItem("currentPlace") != "0"
      ) {
        setWishlistData(JSON.parse(localStorage.getItem("currentPlace")));
        var isPresentInBeskeData = [];
        if (localStorage.getItem("bespokeItems") !== null) {
          isPresentInBeskeData = data.filter(function(item) {
            return (
              item.cartItems.altId ===
              JSON.parse(localStorage.getItem("currentPlace")).categoryEvent
                .altId
            );
          });
        }

        if (isPresentInBeskeData.length > 0) {
          setIsPresentInBeskeData(true);
        }
      }
    }
    PubSub.subscribe(
      "UPDATE_EVENTLEARNPAGE_WISHLIST_DATA_EVENT",
      subscriberTitleData(this)
    );
  }, []);

  function subscriberTitleData(data) {
    if (
      localStorage.getItem("currentPlace") != null &&
      localStorage.getItem("currentPlace") != "0"
    ) {
      var data;
      if (localStorage.getItem("bespokeItems") !== null) {
        var data = JSON.parse(localStorage.getItem("bespokeItems")).filter(
          function(val) {
            return val.userAltId == localStorage.getItem("userToken");
          }
        );
      }

      if (
        localStorage.getItem("currentPlace") != null &&
        localStorage.getItem("currentPlace") != "0"
      ) {
        setWishlistData(JSON.parse(localStorage.getItem("currentPlace")));
        var isPresentInBeskeData = [];
        if (localStorage.getItem("bespokeItems") !== null) {
          isPresentInBeskeData = data.filter(function(item) {
            return (
              item.cartItems.categoryEvent.altId ===
              JSON.parse(localStorage.getItem("currentPlace")).categoryEvent
                .altId
            );
          });
        }

        if (isPresentInBeskeData.length === 0) {
          setIsPresentInBeskeData(false);
        }
      }
    }
  }
  function addToBespokeItinerary(item) {
    if (!props.isAthenticated) {
      PubSub.publish("SHOW_LOGIN", 1);
    } else {
      setWishListAdded(!wishListAdded);
      PubSub.publish("UPDATE_NAV_WISHLIST_DATA_EVENT", 1);
    }
  }
  let SOCIAL_SHARE_LINKS: ISocialShareProps = {
    facebook: {
      quote: "Hey, check out this event!",
      hashtag: "YourWay",
      url: window.location.href,
      className: "mr-2",
    },
    linkedIn: {
      url: window.location.href,
      title: "Hey, check out this event!",
      summary: "Hey, check out this event!",
      source: "www.feelitlive.com",
      className: "mr-2",
    },
    twitter: {
      url: window.location.href,
      title: "Hey, check out this event!",
      hashtags: ["YourWay"],
      related: ["@feelaplace"],
      className: "mr-2",
    },
    whatsApp: {
      url: window.location.href,
      title: "Hey, check out this really nice online event!",
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
                href={`/c/${props.eventData.category.slug}?category=${
                  props.eventData.category.id
                }`}
              >
                {props.eventData.category.displayName}
              </a>
            </li>
            <li className="breadcrumb-item">
              <a
                href={`/c/${props.eventData.category.slug}/${
                  props.eventData.subCategory.slug
                }?category=${props.eventData.category.id}&subcategory=${
                  props.eventData.subCategory.id
                }`}
              >
                {props.eventData.subCategory.displayName}
              </a>
            </li>
            <li className="breadcrumb-item active" aria-current="page">
              {props.eventData.event.name}
            </li>
          </ol>
        </nav>

        <ImageAndVideoV1 eventData={props.eventData} />

        <div className="card-deck pt-4 fil-event-detail">
          <div className="card">
            <h2 className="mb-2">{props.eventData.event.name}</h2>
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
                  onClick={(e) => addToBespokeItinerary(wishlistData)}
                  className="text-body"
                >
                  <img
                    src={`${
                      (isPresentInBespokeData || wishListAdded) &&
                      props.isAthenticated
                        ? `${s3BaseUrl}/icons/live-tag.svg`
                        : `${s3BaseUrl}/fil-images/icon/bugvibe-inverse.svg`
                    }`}
                    alt=""
                    width="18"
                    className="mr-2"
                  />
                  <u>{!wishListAdded ? "Save" : "Remove"}</u>
                </a>
              </div>
            </div>
          </div>
          <div className="card my-auto">
            <div className="fil-event-detail-right">
              <div className="d-sm-inline-block d-block">
                <img
                  src={`https://static7.feelitlive.com/images/fil-images/fil-exp-price.svg`}
                  alt=""
                  className="mr-2"
                />
                <span className="align-top d-inline-block">
                  from <br />{" "}
                  <span className="font-weight-bold">{`${symbol}${formatCurrency(
                    minPrice
                  )} `}</span>
                  {props.eventData.currencyType.code} per person
                </span>
              </div>
              <div className="fil-exp-btn d-inline-block float-sm-right text-right">
                {props.eventData.event.eventTypeId != 6 && (
                  <Link
                    to={
                      "/ticket-purchase-selection/" +
                      props.eventData.event.altId
                    }
                    className={
                      !props.eventData.event.isEnabled
                        ? "btn btn-primary fil-btn disabled"
                        : "btn btn-primary fil-btn"
                    }
                  >
                    {" "}
                    {props.eventData.event.isEnabled
                      ? "Book Now"
                      : "Booking Closed"}
                    <img
                      src={`https://static7.feelitlive.com/images/fil-images/icon/arrow-right.svg`}
                      className="ml-2"
                      alt=""
                    />
                  </Link>
                )}
                {props.eventData.event.eventTypeId == 6 && (
                  <Link
                    to={`/ticket-alert/${props.eventData.event.slug}`}
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
};

export default InnerBannerV1;
