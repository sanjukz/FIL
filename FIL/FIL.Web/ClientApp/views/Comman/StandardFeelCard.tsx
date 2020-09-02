import * as React from "react";
import { Link } from "react-router-dom";
import EventRatingsV1 from "../../components/Home/EventRatingV1";
import EventPriceV1 from "../../components/Home/EventPriceV1";
import EventImageV1 from "../../components/Home/EventImageV1";
import BespokeWishListViewModel from "../../models/BespokeWishList/BespokeWishListViewModel";
import * as PubSub from "pubsub-js";
import Slider from "react-slick";
import { gets3BaseUrl } from "../../utils/imageCdn";
import { autobind } from "core-decorators";
import { GetDuration } from "../../utils/currencyFormatter";
import { MasterEventTypes } from "../../Enum/MasterEventTypes";
import { EventFrequencyType } from "../../Enum/EventFrequencyType";

let DragSortableList;
export default class StandardFeelCard extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      placeIndex: 1,
      s3BaseUrl: gets3BaseUrl(),
      baseImgixUrl: "https://feelitlive.imgix.net/images",
      imgUrl: ``,
    };
  }

  public componentDidMount() {
    var that = this;
    DragSortableList = require("react-drag-sortable");
    var itemsData = this.props.placeCardsData;
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
    // TODO: make this not change state so much (one object instead of many)
    if (bespokeData.length > 0) {
      bespokeData.map(function(val) {
        if (that.props.session.isAuthenticated) {
          that.setState({
            [val.cartItems.altId]: true,
          });
        } else {
          that.setState({
            [val.cartItems.altId]: false,
          });
        }
      });
    }
    // set loaded
    PubSub.subscribe(
      "UPDATE_WISHLIST_DATA_EVENT_ON_LANDING_PAGES",
      this.subscriberData.bind(this)
    );
  }

  public subscriberData() {
    this.componentDidMount();
  }

  public addToBespokeItinerary(item) {
    if (this.props.session && this.props.session.user) {
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
      var isPlaceExists = bespokeData.filter(function(val) {
        return val.cartItems.altId == item.altId;
      });
      if (isPlaceExists.length == 0) {
        var userCheckoutData: BespokeWishListViewModel = {
          cartItems: item,
          userAltId: that.props.session.user.altId,
        };
        bespokeData.push(userCheckoutData);
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
          allWishlistexceptCurrent.forEach(function(val) {
            bespokeData.push(val);
          });
        }
        localStorage.setItem("bespokeItems", JSON.stringify(bespokeData));
        that.setState({
          [item.altId]: true,
        });
      } else {
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
      }
      PubSub.publish("UPDATE_NAV_WISHLIST_DATA_EVENT", 1);
    } else {
      PubSub.publish("SHOW_LOGIN", 1);
    }
  }

  @autobind
  onClickShowMore(e) {
    this.setState({ placeIndex: this.state.placeIndex + 1 });
  }
  public checkVenue(item) {
    return (
      <div className="iconlink">
        <Link
          className="text-dark"
          to={"https://www.google.com/maps/search/" + item.name}
          target="_blank"
        >
          {item.cityName + ", " + item.countryName}
        </Link>
      </div>
    );
  }

  public render() {
    const { s3BaseUrl, baseImgixUrl } = this.state;
    var onSort = function(sortedList) {};
    const settings = {
      className: "",
      infinite: true,
      slidesToShow: this.props.noOfSlides,
      slidesToScroll: 1,
      speed: 500,
      responsive: [
        {
          breakpoint: 991,
          settings: {
            slidesToShow: 4,
          },
        },
        {
          breakpoint: 768,
          settings: {
            slidesToShow: 3,
          },
        },
        {
          breakpoint: 570,
          settings: {
            slidesToShow: 2,
          },
        },
        {
          breakpoint: 430,
          settings: {
            slidesToShow: 1,
          },
        },
      ],
    };
    var that = this,
      isLiveOnline = false;
    var place_Data = [];
    var data = this.props.placeCardsData
      .slice(0, 16 * (this.state.placeIndex + 1))
      .map((item) => {
        // Get distinct places...
        if (
          place_Data.filter((val) => {
            return item.name == val.name;
          }).length == 0
        ) {
          place_Data.push(item);
        }
      });
    //Check if live online Category
    if (this.props.placeCardsData && this.props.placeCardsData.length > 0) {
      if (
        this.props.placeCardsData[0].masterEventTypeId ==
        MasterEventTypes.Online
      ) {
        isLiveOnline = true;
      }
    }
    var placeCardData = place_Data.slice(0, 16 * this.state.placeIndex);
    var placeCards = placeCardData.map((item) => {
      let endUrl = !isLiveOnline
        ? "icons/feelList-hear-new.png"
        : "icons/live-tag.svg";
      let logoWidth = !isLiveOnline ? "15" : "24";
      let duration;
      if (isLiveOnline) {
        duration = GetDuration(item.duration);
      }
      return (
        <div className={`${this.props.isSlide ? "" : "col-sm-3 mb-4"}`}>
          <div className="card fil-tils border-0 h-100">
            <a href="javascript:void(0)" className="feellist-tag">
              <img
                className="d-inline"
                src={
                  that.state && that.state[item.altId]
                    ? `${s3BaseUrl}/icons/live-tag.svg`
                    : `${s3BaseUrl}/fil-images/icon/bugvibe-inverse.svg`
                }
                alt="Add to your feelList"
                onClick={this.addToBespokeItinerary.bind(that, item)}
              />
            </a>

            <a
              href={
                item.eventTypeId == 6
                  ? `/ticket-alert/${item.slug}`
                  : `${item.url}`
              }
            >
              <EventImageV1
                altId={item.altId}
                noOfSlides={this.props.noOfSlides}
                parentCategory={item.parentCategory}
                category={item.category}
                key={item.altId}
              />
            </a>
            <div className="card-body px-0 pb-0">
              <a
                href={
                  item.eventTypeId == 6
                    ? `/ticket-alert/${item.slug}`
                    : `${item.url}`
                }
                className="tils-title m-0 text-body"
              >
                {item.name}
              </a>

              {!isLiveOnline && this.checkVenue(item)}

              {item.eventTypeId != 6 && (
                <EventPriceV1
                  currentItem={item}
                  currency={item.currency}
                  minPrice={item.minPrice}
                  maxPrice={item.maxPrice}
                  duration={GetDuration(item.duration)}
                />
              )}
              <div className="fil-tils-tag">
                {item.eventFrequencyType == EventFrequencyType.OnDemand && (
                  <span className="mr-2 red-tag">On Demand</span>
                )}
                <span>{item.category}</span>
              </div>

              {item.eventTypeId != 6 && (
                <EventRatingsV1
                  rating={item.rating}
                  eventAltId={item.altId}
                  isInnerPage={true}
                />
              )}
            </div>
          </div>
        </div>
      );
    });
    if (!this.props.isWishList) {
      return (
        <>
          {this.props.isSlide ? (
            <div className="card-columns">
              <Slider {...settings}>{placeCards}</Slider>
            </div>
          ) : (
            <div className="row">{placeCards}</div>
          )}
          {this.props.isShowMore &&
            this.props.placeCardsData.length > 16 &&
            16 * this.state.placeIndex <= this.props.placeCardsData.length && (
              <a
                className="show-all-link"
                onClick={this.onClickShowMore}
                href="javascript:Void(0)"
              >
                Show More...
              </a>
            )}
        </>
      );
    } else {
      var list = [];
      placeCards.forEach(function(val) {
        list.push({
          content: val,
        });
      });
      return (
        <div className="card-columns">
          <DragSortableList
            items={list}
            moveTransitionDuration={0.3}
            onSort={onSort}
            type="horizontal"
          />
        </div>
      );
    }
  }
}
