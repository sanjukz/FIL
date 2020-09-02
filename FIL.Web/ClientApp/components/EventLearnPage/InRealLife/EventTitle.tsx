import * as React from "React";
import ReactStars from 'react-stars';
import { Link, withRouter } from "react-router-dom";
import * as PubSub from 'pubsub-js';
import BespokeWishListViewModel from "../../../models/BespokeWishList/BespokeWishListViewModel";
import { formatCurrency, formatDate } from '../../../utils/currencyFormatter';
import * as getSymbolFromCurrency from 'currency-symbol-map';
import * as ReactTooltip from 'react-tooltip';
import { gets3BaseUrl } from "../../../utils/imageCdn";
import SocialShare, { ISocialShareProps } from "../../SocialShare/SocialShare";

class EventTitle extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = { isPresentInBespokeData: false };
    }

    public componentDidMount() {
        if (localStorage.getItem('currentPlace') != null && localStorage.getItem('currentPlace') != '0') {
            var data;
            if (localStorage.getItem("bespokeItems") !== null) {
                var data = JSON.parse(localStorage.getItem("bespokeItems")).filter(function (val) { return val.userAltId == localStorage.getItem('userToken') });
            }

            if (localStorage.getItem('currentPlace') != null && localStorage.getItem('currentPlace') != '0') {
                this.setState({ wishlistData: JSON.parse(localStorage.getItem('currentPlace')) });
                var isPresentInBeskeData = [];
                if (localStorage.getItem("bespokeItems") !== null) {
                    isPresentInBeskeData = data.filter(function (item) { return item.cartItems.altId === JSON.parse(localStorage.getItem("currentPlace")).categoryEvent.altId });
                }

                if (isPresentInBeskeData.length > 0) {
                    this.setState({ isPresentInBespokeData: true });
                }
            }
        }
        PubSub.subscribe('UPDATE_EVENTLEARNPAGE_WISHLIST_DATA_EVENT', this.subscriberTitleData.bind(this));
    }

    public subscriberTitleData(msg, data) {
        var that = this;
        if (localStorage.getItem('currentPlace') != null && localStorage.getItem('currentPlace') != '0') {
            var data;
            if (localStorage.getItem("bespokeItems") !== null) {
                var data = JSON.parse(localStorage.getItem("bespokeItems")).filter(function (val) { return val.userAltId == localStorage.getItem('userToken') });
            }

            if (localStorage.getItem('currentPlace') != null && localStorage.getItem('currentPlace') != '0') {
                this.setState({ wishlistData: JSON.parse(localStorage.getItem('currentPlace')) });
                var isPresentInBeskeData = [];
                if (localStorage.getItem("bespokeItems") !== null) {
                    isPresentInBeskeData = data.filter(function (item) { return item.cartItems.categoryEvent.altId === JSON.parse(localStorage.getItem("currentPlace")).categoryEvent.altId });
                }

                if (isPresentInBeskeData.length === 0) {
                    this.setState({ isPresentInBespokeData: false });
                }
            }
        }
    }

    public addToBespokeItinerary(item, e) {
        if (!this.props.isAthenticated) {
            localStorage.setItem("redirectPlaceAltId", this.props.eventId);
            this.props.goToLoginPage();
        } else {

            var that = this;
            var bespokeData = (localStorage.getItem('bespokeItems') != null && localStorage.getItem('bespokeItems') != '0') ? JSON.parse(localStorage.getItem("bespokeItems")) : [];

            if (bespokeData.length > 0) {
                bespokeData = bespokeData.filter(function (val) { return val.userAltId == localStorage.getItem('userToken') });
            }
            var isPlaceExists = bespokeData.filter(function (val) { return val.cartItems.categoryEvent.altId == item.categoryEvent.altId });

            if (isPlaceExists.length == 0) {
                var userCheckoutData: BespokeWishListViewModel = {
                    cartItems: item,
                    userAltId: localStorage.getItem('userToken')
                }
                bespokeData.push(userCheckoutData);
                var allWishlistexceptCurrent;
                if (localStorage.getItem("bespokeItems") !== null && localStorage.getItem('bespokeItems') != '0') {
                    allWishlistexceptCurrent = JSON.parse(localStorage.getItem("bespokeItems")).filter(function (val) { return val.userAltId !== localStorage.getItem('userToken') });
                    allWishlistexceptCurrent.forEach(function (val) {
                        bespokeData.push(val);
                    });
                }
                localStorage.setItem('bespokeItems', JSON.stringify(bespokeData));
                that.setState({
                    isPresentInBespokeData: true
                })
            } else {
                var i = -1;
                if (bespokeData.length == 1) {
                    var allWishlistexceptCurrent;
                    if (localStorage.getItem("bespokeItems") !== null && localStorage.getItem('bespokeItems') != '0') {
                        allWishlistexceptCurrent = JSON.parse(localStorage.getItem("bespokeItems")).filter(function (val) { return val.userAltId !== localStorage.getItem('userToken') });
                    }
                    var data = []
                    if (allWishlistexceptCurrent.length > 0) {
                        allWishlistexceptCurrent.forEach(function (val) {
                            data.push(val);
                        });
                        localStorage.setItem('bespokeItems', JSON.stringify(data));
                    } else {
                        localStorage.removeItem('bespokeItems');
                    }
                    that.setState({
                        isPresentInBespokeData: false
                    });
                } else {
                    var bespokeItems = bespokeData.forEach(function (val) {
                        i = i + 1;
                        if (val.cartItems.categoryEvent.altId == item.categoryEvent.altId) {
                            bespokeData.splice(i, 1);
                        }
                    });
                    if (localStorage.getItem("bespokeItems") !== null && localStorage.getItem('bespokeItems') != '0') {
                        allWishlistexceptCurrent = JSON.parse(localStorage.getItem("bespokeItems")).filter(function (val) { return val.userAltId !== localStorage.getItem('userToken') });
                        allWishlistexceptCurrent.forEach(function (val) {
                            bespokeData.push(val);
                        });
                    }
                    localStorage.setItem('bespokeItems', JSON.stringify(bespokeData));
                    that.setState({
                        isPresentInBespokeData: false
                    });
                }
            }
            PubSub.publish('UPDATE_NAV_WISHLIST_DATA_EVENT', 1);
        }
    }

    public render() {
        let SOCIAL_SHARE_LINKS: ISocialShareProps = {
            facebook: {
                quote: "Hey, check out this event.",
                hashtag: "YourWay",
                url: window.location.href,
                className: "mr-2"
            },
            linkedIn: {
                url: window.location.href,
                title: "Hey, check out this event.",
                summary: "Hey, check out this event.",
                source: "www.feelitlive.com",
                className: "mr-2"
            },
            twitter: {
                url: window.location.href,
                title: "Hey, check out this event.",
                hashtags: ["YourWay"],
                related: ["@feelaplace"],
                className: "mr-2"
            },
            whatsApp: {
                url: window.location.href,
                title: "Hey, check out this really nice online event.",
            }
        };
        var event = this.props.event;
        var review = this.props.reviews;
        var reviewLength = 1;
        if (review.length !== 0) {
            reviewLength = review.filter(function (item) { return item.points > 0 }).length;
        }
        var symbol = getSymbolFromCurrency(this.props.currencyCode);
        var reviewAvg = review.length == 0 ? 0 : review.map((item) => { return item.points }).reduce((a, b) => { return a + b }) / reviewLength;
        let title = this.GetTitle(event);
        let rowClassName1 = this.props.isLiveStream ? "col-xl-5 col-lg-5 " : "col-xl-7 col-lg-6";
        let rowClassName2 = this.props.isLiveStream ? "col-xl-7 col-lg-7" : "col-xl-5 col-lg-6";
        let onlineStreamDate;
        if (this.props.isLiveStream) {
            onlineStreamDate = formatDate(this.props.onlineStreamDate);
            onlineStreamDate = onlineStreamDate.split('~');
        }
        return <div className="row learn-head">
            <div className={`${rowClassName1} col-sm-12`}>
                <h1 title={title} className="m-0 h3">{event.name}</h1>
                <div className="rating">
                    <span>Ratings: <ReactStars className="rating d-inline-block" color2={"#572483"} edit={false} value={reviewAvg} count={5} /> </span>
                    <span className="review-text">{review.length}+ Reviews</span>
                </div>
            </div>

            <div className={`${rowClassName2} col-sm-12 right-head text-lg-right`}>
                {this.props.isLiveStream &&
                    <div className="d-inline-block align-middle">
                        <h5 className="text-left pr-4">
                            <div><i className="fa fa-clock-o mr-2 d-inline-block align-top mt-2" aria-hidden="true"></i><div className="d-inline-block">
                                <small>{onlineStreamDate[0]} <br />
                                    {onlineStreamDate[1]} {this.props.eventTimeZone}
                                </small>
                            </div></div></h5>
                    </div>
                }
                <div className="d-inline-block align-middle" data-tip data-for="priceTooltip">
                    <h5 className="text-left">
                        <small>From</small><br />
                        {`${symbol}${formatCurrency(this.props.minPrice)} ${this.props.currencyCode}`} <small>per person</small>
                    </h5>
                </div>
                <ReactTooltip id="priceTooltip" type="info" place="left" effect="solid" border={true} style={{ "background-color": "#633d4 !important" }}>
                    <span>{this.props.maxPrice === this.props.minPrice ? `${symbol}${formatCurrency(this.props.minPrice)} ${this.props.currencyCode}` : `${symbol}${formatCurrency(this.props.minPrice)}  ${this.props.currencyCode} - ${symbol}${formatCurrency(this.props.maxPrice)}  ${this.props.currencyCode}`}</span>
                </ReactTooltip>
                <SocialShare socialShareProps={SOCIAL_SHARE_LINKS} />
                <div className="d-inline-block align-middle pr-4">
                    <div className="tinerary-box" onClick={this.addToBespokeItinerary.bind(this, this.state.wishlistData)} data-toggle="tooltip" data-placement="top" title="Add to your feelList">
                        <a href="javascript:void(0)">
                            {(!this.state.isPresentInBespokeData || !this.props.isAthenticated) && <img width="22px" src={`${gets3BaseUrl()}/icons/feelList-hear-new.png`} alt="Add to your feelList" />}
                            {(this.state.isPresentInBespokeData && this.props.isAthenticated) && <img width="22px" src={`${gets3BaseUrl()}/icons/feelList-hear-filled-new.png`} alt="your feelList" />}
                        </a>
                    </div>
                </div>
                <div className="d-inline-block align-middle">
                    <Link to={"/ticket-purchase-selection/" + this.props.eventId} className={!this.props.isActive ? "btn site-primery-btn text-uppercase disabled" : "btn site-primery-btn text-uppercase"} > {this.props.isActive ? "Book Now" : "Booking Closed"}</Link>
                </div>
            </div>
        </div>
    }

    public GetTitle = (event) => {
        let title = "Feel " + event.name + " when you visit " + this.props.city + ", " + this.props.country + "";
        if (this.props.categoryName == "Experiences & Activities") {
            title = "Feel " + event.name + " " + this.props.city + ", " + this.props.country + " - book experiences, activities, & outdoor adventure";
        }
        if (this.props.categoryName == "Eat & Drink") {
            title = "Dine in, eat at or enjoy your favorite drink at " + event.name + ", when you travel to " + this.props.city + ", " + this.props.country + "";
        }
        if (this.props.categoryName == "Shop Local") {
            title = "Shop locally at " + event.name + ", while visiting " + this.props.city + ", " + this.props.country + " - buy souvenirs, certificates and vouchers online.";
        }
        if (this.props.categoryName == "Experiences & Activities") {
            title = " Feel " + event.name + " when you visit " + this.props.city + ", " + this.props.country + " - book experiences, activities, & outdoor adventures";
        }
        if (this.props.categoryName == "Live Stream") {
            title = "Feel " + event.name + " Live on FeelAPlace";
        }
        return title;
    }
}

export default withRouter(EventTitle);
