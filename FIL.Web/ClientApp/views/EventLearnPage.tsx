import { autobind } from "core-decorators";
import * as React from "react";
import { RouteComponentProps } from "react-router-dom";
import {
    actionCreators as sessionActionCreators,
    ISessionProps,
} from "shared/stores/Session";
import { bindActionCreators } from "redux";
import { IApplicationState } from "../stores";
import InnerBannerV1 from "../../ClientApp/components/EventLearnPage/InRealLife/InnerBannerV1";
import EventLearnPageDataViewModel from "../models/EventLearnPageDataViewModel";
import ReviewsRatingDataViewModel from "../models/ReviewsRating/ReviewsRatingViewModel";
import ReviewsRatingResponseViewModel from "../models/ReviewsRating/ReviewsRatingResponseViewModel";
import * as EventLearnPageStore from "../stores/EventLearnPage";
import { connect } from "react-redux";
import Metatag from "../components/Metatags/Metatag";
import FilLoader from "../components/Loader/FilLoader";
import "./EventLearnPage.scss";
import * as numeral from "numeral";
import { StickyContainer, Sticky } from "react-sticky";
import * as PubSub from "pubsub-js";
import * as FeelUserJourneyStore from "../stores/FeelUserJourney";
import * as parse from "url-parse";
import { MasterEventTypes } from "../Enum/MasterEventTypes";
import OnlineExperiences from "../components/EventLearnPage/Online";
import ExperienceAndHostDetailV1 from "../components/EventLearnPage/InRealLife/ExperienceAndHostDetailV1";
import ImportantThingsAndReviews from "../components/EventLearnPage/Online/ImportantThingsAndReviews";
import "../scss/_irl-learn-more.scss";

type EventLearnPageComponentProps = FeelUserJourneyStore.IFeelUserJourneyProps &
    typeof FeelUserJourneyStore.actionCreators &
    EventLearnPageStore.IEventLearnPageComponentProps &
    typeof EventLearnPageStore.actionCreators &
    ISessionProps &
    typeof sessionActionCreators &
    RouteComponentProps<{}>;

class EventLearnPage extends React.Component<
    EventLearnPageComponentProps,
    any
    > {
    containerLine: HTMLDivElement;
    constructor(props) {
        super(props);
        this.state = {
            review: "",
            rating: -1,
            isCount: 1,
            isRedirectlogin: false,
            className: "",
        };
        this.hideBar = this.hideBar.bind(this);
    }

    private getEventId(props) {
        return (
            props.location.pathname.split("/")[3] ||
            props.location.pathname.split("/")[3]
        );
    }

    public componentWillUnmount() {
        if (
            this.props.history.action === "POP" &&
            (window.sessionStorage.getItem("searchKeyword") == "" ||
                window.sessionStorage.getItem("searchKeyword") == null)
        ) {
            // custom back button implementation
            var data = {
                parentCat: this.props.eventLearnPage.eventDetails.eventCategoryName,
                childCat: this.props.eventLearnPage.eventDetails.eventCategory,
                isMainCatClick: false,
            };
            localStorage.setItem("isBreadCrumClick", "true");
            localStorage.setItem("selectedBreadCrumCat", JSON.stringify(data));
            this.props.history.push("/");
            var b = 5;
        }
    }
    public componentDidMount() {
        if (this.props.history.action === "POP") {
            // custom back button implementation
            var b = 5;
        }
        if (window) {
            window.scrollTo(0, 0);
        }
        if (localStorage.getItem("isRedirectLogin") !== null) {
            this.setState({ isRedirectlogin: true });
            localStorage.removeItem("isRedirectLogin");
        }
        if (
            localStorage.getItem("rating") !== null &&
            localStorage.getItem("review") !== null
        ) {
            this.setState({ review: localStorage.getItem("review") });
            this.setState({ rating: localStorage.getItem("rating") });
            localStorage.removeItem("review");
            localStorage.removeItem("rating");
        }
        let splitPath = this.props.location.pathname.split("/");
        if (splitPath.length == 4 && splitPath[1] == "event") {
            var user: EventLearnPageDataViewModel = {
                slug: splitPath[2],
            };
            this.setState({
                isPrivate: true,
                eventAltId: splitPath[3].toLowerCase(),
            });
        } else if (this.props.location.pathname.split("/")[3] != undefined) {
            var user: EventLearnPageDataViewModel = {
                slug: this.props.location.pathname.split("/")[3],
            };
        } else {
            var user: EventLearnPageDataViewModel = {
                eventAltId: this.props.location.pathname.split("/")[2],
            };
        }

        this.props.requestEventLearnPageData(user);
        localStorage.removeItem("redirectPlaceAltId");
        if (
            localStorage.getItem("userToken") !== null &&
            localStorage.getItem("userToken") !== ""
        ) {
            //this.props.getUserOrdertData(localStorage.getItem('userToken'));
        }
        this.setState({ className: "" });
        window.addEventListener("scroll", this.hideBar);
        let urlData = parse(location.search, location, true);
        if (urlData.query.utm_referrer) {
            sessionStorage.setItem('referralId', urlData.query.utm_referrer)
        }
    }

    hideBar() {
        if (window.pageYOffset >= 450 && window.pageXOffset === 0) {
            this.setState({ className: "learn-head-stiky" });
        } else {
            this.setState({ className: "" });
        }
    }

    public UNSAFE_componentWillReceiveProps(newProps) {
        let newId = this.getEventId(newProps);
        let currId = this.getEventId(this.props);
        if (newId !== currId) {
            let splitPath = this.props.location.pathname.split("/");
            if (splitPath.length == 4 && splitPath[1] == "event") {
                var user: EventLearnPageDataViewModel = {
                    slug: splitPath[2],
                };
                this.setState({
                    isPrivate: true,
                    eventAltId: splitPath[3].toLowerCase(),
                });
            } else if (this.props.location.pathname.split("/")[3] != undefined) {
                var user: EventLearnPageDataViewModel = {
                    slug: newId,
                };
            } else {
                var user: EventLearnPageDataViewModel = {
                    eventAltId: this.props.location.pathname.split("/")[2],
                };
            }
            this.props.requestEventLearnPageData(user);
        }
    }

    @autobind
    public checkForAltId() {
        if (this.props.location.pathname.split("/")[3] === undefined) {
            var Data = this.props.eventLearnPage.eventDetails;
            var eventParentCategory = Data.eventCategoryName
                .split(" ")
                .join("-")
                .split("&")
                .join("and");
            var eventChildCategory = Data.eventCategory;
            var eventSlug = Data.event.slug;
            var placeURL =
                "/place/" +
                eventParentCategory.toLowerCase() +
                "/" +
                eventSlug.toLowerCase() +
                "/" +
                eventChildCategory.toLowerCase();
            this.props.history.push(placeURL);
        }
    }

    public getSocialVenue(Data) {
        var venues = Data.venue.name;
        return venues + ", " + Data.city.name + ", " + Data.country.name;
    }

    public goToLogin() {
        this.props.history.push("/login");
    }

    public gotToHome() {
        this.props.history.push("/");
    }

    public goToBookNow() {
        this.props.history.push(
            "/ticket-purchase-selection/" +
            this.props.eventLearnPage.eventDetails.event.altId
        );
    }

    public render() {
        PubSub.publish("UPDATE_NAV_MENUE", 1);
        if (this.props.eventLearnPage.fetchSuccessEventLeanMore) {
            if (
                this.state.isPrivate &&
                this.props.eventLearnPage.eventDetails.event.altId.toLowerCase() !=
                this.state.eventAltId
            ) {
                return <FilLoader />;
            }
            var isPurase = false,
                data,
                isLiveStream = false;
            if (this.props.eventLearnPage.userOrders != undefined) {
                data = this.props.eventLearnPage.userOrders.event.filter(function (
                    item
                ) {
                    return item.altId === this.props.match.params.eventId;
                });
                if (data.length > 0) {
                    isPurase = true;
                    if (
                        this.state.review !== "" &&
                        this.state.rating !== -1 &&
                        this.state.isCount === 1 &&
                        this.state.isRedirectlogin === true
                    ) {
                        this.onSubmitSignIn(this.state.review, parseInt(this.state.rating));
                    }
                }
            }
            var Data = this.props.eventLearnPage.eventDetails;
            this.checkForAltId();
            var minPrice =
                Data &&
                    Data.eventTicketAttribute &&
                    Data.eventTicketAttribute.length == 1
                    ? Data.eventTicketAttribute[0].price
                    : Math.min.apply(
                        null,
                        Data.eventTicketAttribute
                            .map((item) => {
                                return item.price;
                            })
                            .filter(Boolean)
                    ) !== Infinity
                        ? Math.min.apply(
                            null,
                            Data.eventTicketAttribute
                                .map((item) => {
                                    return item.price;
                                })
                                .filter(Boolean)
                        )
                        : 0;
            var maxPrice = Math.max(
                ...Data.eventTicketAttribute.map((item) => {
                    return item.price;
                })
            );
            var historyData = Data.eventLearnMoreAttributes.filter(function (val) {
                return val.learnMoreFeatureId == "1";
            });

            var highlightsData = Data.eventLearnMoreAttributes.filter(function (val) {
                return val.learnMoreFeatureId == "2";
            });
            var timelineData = Data.eventLearnMoreAttributes.filter(function (val) {
                return val.learnMoreFeatureId == "3";
            });
            var immersiveData = Data.eventLearnMoreAttributes.filter(function (val) {
                return val.learnMoreFeatureId == "4";
            });
            var architectureData = Data.eventLearnMoreAttributes.filter(function (
                val
            ) {
                return val.learnMoreFeatureId == "5";
            });
            var mapPlanData = Data.eventLearnMoreAttributes.filter(function (val) {
                return val.learnMoreFeatureId == "6";
            });
            let latitude = Data.venue.latitude;
            let longitude = Data.venue.longitude;

            //Check For Live Online Stream
            if (Data.event.masterEventTypeId == MasterEventTypes.Online) {
                isLiveStream = true;
            }
            return (
                <>
                    <StickyContainer>
                        {!isLiveStream ? (
                            <div className="fil-site fil-exp-landing-page irl-learn-more">
                                <Metatag
                                    url={this.props.location.pathname}
                                    metaContent={Data}
                                    title={Data.event.name}
                                    isDescriptionAvailable={
                                        Data.event.description == "" ? false : true
                                    }
                                    categoryName={Data.eventCategoryName}
                                />
                                <InnerBannerV1
                                    gotToHome={this.gotToHome}
                                    history={this.props.history}
                                    isCityCountryPage={false}
                                    eventData={Data}
                                    isAthenticated={this.props.session.isAuthenticated}
                                />
                                <ExperienceAndHostDetailV1
                                    historyData={historyData}
                                    highlightsData={highlightsData}
                                    timelineData={timelineData}
                                    architectureData={architectureData}
                                    mapPlanData={mapPlanData}
                                    immersiveData={immersiveData}
                                    eventData={Data}
                                    getDataByCategory={
                                        this.props.requestDyanamicSectionResponseData
                                    }
                                    feelUserJourney={this.props.feelUserJourney}
                                    {...this.props}
                                />
                                <ImportantThingsAndReviews
                                    onSubmit={this.onSubmitSignIn.bind(this)}
                                    eventData={Data}
                                    rating={Data.rating}
                                    user={Data.user}
                                    event={Data.event}
                                    UserImageMap={Data.userImageMap}
                                    isPurase={isPurase}
                                    isAthenticated={this.props.session.isAuthenticated}
                                    goToBookNowPage={this.goToBookNow.bind(this)}
                                    reDirectUrl={this.props.location.pathname
                                        .split("/")[2]
                                        .toUpperCase()}
                                    goToLoginPage={this.goToLogin.bind(this)}
                                    isLiveStream={isLiveStream}
                                />
                            </div>
                        ) : (
                                <OnlineExperiences
                                    eventData={Data}
                                    rating={Data.rating}
                                    user={Data.user}
                                    event={Data.event}
                                    UserImageMap={Data.userImageMap}
                                    isPurase={isPurase}
                                    isAthenticated={this.props.session.isAuthenticated}
                                    goToBookNowPage={this.goToBookNow.bind(this)}
                                    reDirectUrl={this.props.location.pathname
                                        .split("/")[2]
                                        .toUpperCase()}
                                    goToLoginPage={this.goToLogin.bind(this)}
                                    onSubmit={this.onSubmitSignIn.bind(this)}
                                />
                            )}
                    </StickyContainer>
                </>
            );
        } else {
            return (
                <div>
                    <FilLoader />
                </div>
            );
        }
    }

    private onSubmitSignIn(review, rating) {
        if (this.props.session.isAuthenticated) {
            this.setState({ isCount: 2 });
            var reviewRating = new ReviewsRatingDataViewModel();
            reviewRating.UserAltId = this.props.session.user.altId;
            reviewRating.EventAltId = this.props.eventLearnPage.eventDetails.event.altId;
            reviewRating.Points = rating;
            reviewRating.Comment = review;
            reviewRating.isEnabled = true;
            this.props.userRatingAndReviews(
                reviewRating,
                (response: ReviewsRatingResponseViewModel) => {
                    if (response.success) {
                        //this.props.requestEventLearnPageData(this.getEventId(this.props));
                        window.location.reload();
                    }
                }
            );
        } else {
            PubSub.publish("SHOW_LOGIN", 1);
        }
    }
}
export default connect(
    (state: IApplicationState) => ({
        session: state.session,
        eventLearnPage: state.eventLearnPage,
        feelUserJourney: state.feelUserJourney,
    }),
    (dispatch) =>
        bindActionCreators(
            {
                ...sessionActionCreators,
                ...EventLearnPageStore.actionCreators,
                ...FeelUserJourneyStore.actionCreators,
            },
            dispatch
        )
)(EventLearnPage);
