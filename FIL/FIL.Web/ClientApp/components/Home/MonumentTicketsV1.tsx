import * as React from 'react';
import * as PubSub from "pubsub-js";
import { gets3BaseUrl } from "../../utils/imageCdn";
import { GetDuration } from "../../utils/currencyFormatter";
import { CategoryPageResponseViewModel } from "../../models/CategoryPageResponseViewModel";
import EventRatingsV1 from './EventRatingV1';
import EventPriceV1 from './EventPriceV1';
import { Link } from "react-router-dom";
import { ISessionState } from 'shared/stores/Session';
import { PlaceDetail } from "../../models/FeelUserJourney/FeelUserJourneyResponseViewModel";
import BespokeWishListViewModel from "../../models/BespokeWishList/BespokeWishListViewModel";
import EventImageV1 from './EventImageV1';
import { EventFrequencyType } from '../../Enum/EventFrequencyType';

interface IProps {
    categoryData: CategoryPageResponseViewModel[];
    count: number;
    session: ISessionState;
    isLiveStreamSection?: boolean;
}
class MonumentTicketsV1 extends React.PureComponent<IProps, any>{
    constructor(props) {
        super(props);
        this.state = {
            s3BaseUrl: gets3BaseUrl(),
            baseImgixUrl: 'https://feelitlive.imgix.net/images'
        }
    }

    public checkVenue(item) {
        var venues = item.venue.map((item) => item.name);
        if (venues.length === 1) {
            return (
                <div className="iconlink">
                    <Link
                        className="text-dark"
                        to={"https://www.google.com/maps/search/" + venues[0]}
                        target="_blank"
                    >
                        {item.city[0].name + ", " + item.country[0].name}
                    </Link>
                </div>
            );
        } else {
            var uniqueVenues = venues.filter(function (value, index, self) {
                return self.indexOf(value) === index;
            });
            if (uniqueVenues.length == 1) {
                return (
                    <div className="iconlink">
                        <Link
                            to={"https://www.google.com/maps/search/" + uniqueVenues[0]}
                            target="_blank"
                        >
                            {item.city[0].name + ", " + item.country[0].name}
                        </Link>
                    </div>
                );
            } else {
                var cities = item.city.map((item, index) => item.name);
                return (
                    <div className="iconlink">
                        <Link
                            to={"https://www.google.com/maps/search/" + item[0]}
                            target="_blank"
                        >
                            {cities.toString()}
                        </Link>
                    </div>
                );
            }
        }
    }

    public componentDidMount() {
        var that = this;
        var itemsData = this.props.categoryData;
        itemsData.map(function (val) {
            that.setState({
                [val.event && val.event.altId]: false,
            });
        });
        var bespokeData =
            localStorage.getItem("bespokeItems") != null &&
                localStorage.getItem("bespokeItems") != "0"
                ? JSON.parse(localStorage.getItem("bespokeItems"))
                : [];
        if (bespokeData.length > 0) {
            bespokeData = bespokeData.filter(function (val) {
                return (
                    val.userAltId == that.props.session.user &&
                    that.props.session.user.altId
                );
            });
        }
        // TODO: make this not change state so much (one object instead of many)
        if (bespokeData.length > 0) {
            bespokeData.map(function (val) {
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
            "UPDATE_WISHLIST_DATA_EVENT",
            this.subscriberData.bind(this)
        );
    }


    public subscriberData() {
        this.componentDidMount();
    }

    public getMinPrice(item) {
        var prices = item.map((val) => val.price);
        if (prices.length == 1) {
            return prices[0];
        } else {
            return Math.min(...prices);
        }
    }

    public getMaxPrice(item) {
        var prices = item.map((val) => val.price);
        if (prices.length == 1) {
            return prices[0];
        } else {
            return Math.max(...prices);
        }
    }
    public addToBespokeItinerary(item) {
        var that = this;
        if (that.props.session && that.props.session.user) {
            var bespokeData =
                localStorage.getItem("bespokeItems") != null &&
                    localStorage.getItem("bespokeItems") != "0"
                    ? JSON.parse(localStorage.getItem("bespokeItems"))
                    : [];

            if (bespokeData.length > 0) {
                bespokeData = bespokeData.filter(function (val) {
                    return val.userAltId == that.props.session.user.altId;
                });
            }
            var isPlaceExists = bespokeData.filter(function (val) {
                return val.cartItems.altId == item.categoryEvent.altId;
            });
            let placeItems: PlaceDetail = {
                altId: item.event.altId,
                cityName: item.city[0].name,
                countryName: item.country[0].name,
                currency: item.currencyType.code,
                currencyId: item.eventTicketAttribute[0].currencyId,
                maxPrice: this.getMaxPrice(item.eventTicketAttribute),
                minPrice: this.getMinPrice(item.eventTicketAttribute),
                name: item.event.name,
                category: item.eventCategories[0],
                parentCategory: item.parentCategory,
                slug: item.event.slug,
                subCategorySlug: item.eventCategory,
                parentCategorySlug: item.parentCategory,
                categorySlug: item.parentCategory,
                url:
                    "/place/" +
                    item.parentCategory +
                    "/" +
                    item.event.slug +
                    "/" +
                    item.eventCategory.toLocaleLowerCase(),
                rating: 0,
                cityId: item.city[0].id,
                countryAltId: item.country[0].altId,
                countryId: item.country[0].id,
                eventCategoryId: 29,
                eventDetailId: item.event.id,
                id: item.event.id,
                parentCategoryId: 29,
                stateId: item.state[0].id,
                stateName: item.state[0].name,
                subCategories: [],
                duration: null,
            };
            if (isPlaceExists.length == 0) {
                var userCheckoutData: BespokeWishListViewModel = {
                    cartItems: placeItems,
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
                    ).filter(function (val) {
                        return (
                            val.userAltId !== that.props.session.user &&
                            that.props.session.user.altId
                        );
                    });
                    allWishlistexceptCurrent.forEach(function (val) {
                        bespokeData.push(val);
                    });
                }
                localStorage.setItem("bespokeItems", JSON.stringify(bespokeData));
                that.setState({
                    [item.categoryEvent.altId]: true,
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
                        ).filter(function (val) {
                            return (
                                val.userAltId !== that.props.session.user &&
                                that.props.session.user.altId
                            );
                        });
                    }
                    var data = [];
                    if (allWishlistexceptCurrent.length > 0) {
                        allWishlistexceptCurrent.forEach(function (val) {
                            data.push(val);
                        });
                        localStorage.setItem("bespokeItems", JSON.stringify(data));
                    } else {
                        localStorage.removeItem("bespokeItems");
                    }
                    that.setState({
                        [item.categoryEvent.altId]: false,
                    });
                } else {
                    bespokeData.forEach(function (val) {
                        i = i + 1;
                        if (val.cartItems.altId == item.categoryEvent.altId) {
                            bespokeData.splice(i, 1);
                        }
                    });
                    if (
                        localStorage.getItem("bespokeItems") !== null &&
                        localStorage.getItem("bespokeItems") != "0"
                    ) {
                        allWishlistexceptCurrent = JSON.parse(
                            localStorage.getItem("bespokeItems")
                        ).filter(function (val) {
                            return (
                                val.userAltId !== that.props.session.user &&
                                that.props.session.user.altId
                            );
                        });
                        allWishlistexceptCurrent.forEach(function (val) {
                            bespokeData.push(val);
                        });
                    }
                    localStorage.setItem("bespokeItems", JSON.stringify(bespokeData));
                    that.setState({
                        [item.categoryEvent.altId]: false,
                    });
                }
            }
            PubSub.publish("UPDATE_NAV_WISHLIST_DATA_EVENT", 1);
        } else {
            PubSub.publish("SHOW_LOGIN", 1);
        }
        this.setState({ temp: 45 })
    }

    public getCurrentItem(that) {
        localStorage.setItem("currentPlace", JSON.stringify(that));
    }
    isAddedtoFeelList = (cartItems, currentItemId) => {
        if (cartItems && cartItems != null && cartItems.length > 0) {
            let isPresent = cartItems.filter((item) => {
                return item.cartItems.id == currentItemId
            });
            if (isPresent && isPresent != null && isPresent.length > 0) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }
    render() {
        const { categoryData, count } = this.props;
        const { s3BaseUrl, baseImgixUrl } = this.state;
        let that = this;
        let bespokeData = localStorage.getItem("bespokeItems") != null &&
            localStorage.getItem("bespokeItems") != "0"
            ? JSON.parse(localStorage.getItem("bespokeItems"))
            : [];
        if (this.props.session && this.props.session.user) {
            if (bespokeData.length > 0) {
                bespokeData = bespokeData.filter(function (val) {
                    return (
                        val.userAltId == that.props.session.user.altId
                    );
                });
            }
        }
        return (
            <div className="row">
                {
                    categoryData.slice(0, count).map((item) => {
                        let isAddedToFeelList = that.isAddedtoFeelList(bespokeData, item.event.id);

                        let currentEventCategories = item.eventCategories.map((cat) => {
                            return <span>{cat}</span>
                        })

                        return <div className="col-sm-3 mb-4">
                            <div className="card fil-tils border-0 h-100">
                                <a href="javascript:void(0)" className="feellist-tag">
                                    <img src={isAddedToFeelList ? `${s3BaseUrl}/icons/live-tag.svg` : `${s3BaseUrl}/fil-images/icon/bugvibe-inverse.svg`} alt="Add to your feelList"
                                        onClick={this.addToBespokeItinerary.bind(this, item)}
                                    />
                                </a>
                                <a href={item.event.eventTypeId == 6 ? `/ticket-alert/${item.event.slug}` : `/place/${item.parentCategory}/${item.event.slug}/${item.eventCategory.toLocaleLowerCase()}`}>
                                    <EventImageV1 altId={item.event.altId}
                                        parentCategory={item.parentCategory} category={item.eventCategories[0]}
                                        key={item.event.altId} />
                                </a>
                                <div className="card-body px-0 pb-0">
                                    <a href={item.event.eventTypeId == 6 ? `/ticket-alert/${item.event.slug}` : `/place/${item.parentCategory}/${item.event.slug}/${item.eventCategory.toLocaleLowerCase()}`}
                                        className="tils-title m-0 text-body">
                                        {item.event.name}
                                    </a>
                                    {!this.props.isLiveStreamSection && this.checkVenue(item)}

                                    {item.event.eventTypeId != 6 && <EventPriceV1 currency={item.currencyType && item.currencyType.code}
                                        currentItem={item}
                                        minPrice={this.getMinPrice(item.eventTicketAttribute)}
                                        maxPrice={this.getMaxPrice(item.eventTicketAttribute)}
                                        duration={GetDuration(item.duration)}
                                    />}
                                    {(item.event.eventTypeId == 6 && !item.event.isDelete) && <div className="tils-text">
                                        {item.localStartDateTime} {item.timeZoneAbbrivation}
                                    </div>}
                                    <div className="fil-tils-tag">
                                        {currentEventCategories}
                                        {item.eventFrequencyType == EventFrequencyType.OnDemand && <span className="ml-2 red-tag">On Demand</span>}
                                    </div>

                                    {item.event.eventTypeId != 6 && <EventRatingsV1 rating={item.rating} eventAltId={item.event.altId} />}

                                </div>
                            </div>
                        </div>
                    })
                }
            </div>
        )
    }
}

export default MonumentTicketsV1;