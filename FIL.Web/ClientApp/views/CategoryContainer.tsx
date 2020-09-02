import * as React from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { IApplicationState } from "../stores";
import FeelLoader from "../components/Loader/FeelLoader";
import { RouteComponentProps } from "react-router-dom";
import { actionCreators as sessionActionCreators, ISessionProps } from "shared/stores/Session";
import TopBanner from "./Comman/TopBanner";
import SubCategoryTab from "./Comman/SubCategoryTab";
import DynamicSections from "./Comman/DynamicSections";
import AllFeelSection from "./Comman/AllFeelSection";
import TopLocations from "./Comman/TopLocations";
import StayMap from "../components/Home/StayMap";
import * as FeelUserJourneyStore from "../stores/FeelUserJourney";
import FeelUserJourneyRequestViewModel from "../models/FeelUserJourney/FeelUserJourneyRequestViewModel";
import * as parse from "url-parse";
import * as PubSub from 'pubsub-js';
import { gets3BaseUrl } from "../utils/imageCdn";
import "../scss/fil-style.scss";


type CategoryContainerComponentProps = FeelUserJourneyStore.IFeelUserJourneyProps
    & typeof FeelUserJourneyStore.actionCreators
    & ISessionProps
    & typeof sessionActionCreators
    & RouteComponentProps<{}>;

let pageType = null, landingPage = null;

class CategoryContainer extends React.Component<CategoryContainerComponentProps, any> {
    constructor(props) {
        super(props);
        this.state = {
            isStayAt: false,
            baseImgixUrl: 'https://feelitlive.imgix.net/images'
        };
    }

    componentDidMount() {
        const data: any = parse(location.search, location, true);
        var requestModel: FeelUserJourneyRequestViewModel = {
            category: data.query.category ? +data.query.category : 0,
            city: data.query.city ? +data.query.city : 0,
            country: data.query.country ? +data.query.country : 0,
            pageType: 1,
            pagePath: window.location.pathname,
            state: data.query.state ? +data.query.state : 0,
            subCategory: data.query.subcategory ? +data.query.subcategory : 0,
            masterEventType: data.query.eventType ? +data.query.eventType : 0,
        };
        //Check if category is StayAt.
        if (data.query.category == "35") {
            this.setState({ isStayAt: true })
        } else {
            this.props.requestDyanamicSectionResponseData(requestModel);
        }
        pageType = null;
        if (data.query.subcategory == undefined || data.query.subcategory == 0) {
            pageType = this.GetPageType(data, "category");
            landingPage = "category";
        } else if (data.query.subcategory != undefined || data.query.subcategory != 0) {
            pageType = this.GetPageType(data, "subcategory");
            landingPage = "subcategory";
        }
    }

    render() {
        if (this.state.isStayAt) {
            return <StayMap />
        } else {
            if (this.props.feelUserJourney.fetchSuccess) {
                const data: any = parse(location.search, location, true);
                PubSub.publish("UPDATE_SEARCH_CATEGORY", this.props.feelUserJourney.dynamicSections.searchValue);
                let imgURL = "";
                if (this.props.feelUserJourney.dynamicSections.allPlaceTiles && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) {
                    let randomNumber = Math.floor(Math.random() * ((this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length - 1) - 0 + 1) + 0);
                    imgURL = this.state.baseImgixUrl + "/places/about/" + this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails[randomNumber].altId.toUpperCase() + "-about.jpg?auto=format&fit=crop&h=435&w=1920&crop=entropy&q=55";
                }
                //for Metatag content
                let generic_location_name = null, generic_catgeoryName = null, generic_countryName = null, generic_subCategoryName = null;
                if (this.props.feelUserJourney.dynamicSections.allPlaceTiles != null && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) {
                    let meta_content = this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails[0];
                    if (pageType == "city") {
                        generic_location_name = meta_content.cityName;
                        generic_countryName = meta_content.countryName;
                    }
                    if (pageType == "state") {
                        generic_location_name = meta_content.stateName;
                        generic_countryName = meta_content.countryName;
                    }
                    if (pageType == "country") {
                        generic_location_name = meta_content.countryName;
                        generic_countryName = null;
                    }
                    generic_catgeoryName = meta_content.parentCategory;
                    if (landingPage == "subcategory") {
                        generic_subCategoryName = meta_content.category;
                    }
                }
                let isLiveOnine = false; let sectionClassName = "fil-tiles-sec";
                if (this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) {
                    isLiveOnine = this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails[0].masterEventTypeId == 4 ? true : false;
                    let filteredSections = [];
                    this.props.feelUserJourney.dynamicSections.dynamicPlaceSections.map((item) => {
                        let placeLength = item.placeDetails.map((val) => { return val.name }).filter(function (value, index, self) {
                            return self.indexOf(value) === index;
                        }).length;
                        if (placeLength > 2) {
                            filteredSections.push(1);
                        }
                    })
                    if (isLiveOnine && (data.query.subcategory == undefined || data.query.subcategory == 0)) {
                        if ((filteredSections.length + 1) % 2 == 0) {
                            sectionClassName = sectionClassName + " bg-light"
                        }
                    } else {
                        if (filteredSections.length % 2 == 0) {
                            sectionClassName = sectionClassName + " bg-light"
                        }
                    }
                }
                return <>
                    {(this.props.feelUserJourney.dynamicSections.allPlaceTiles && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) &&
                        <TopBanner
                            img={imgURL}
                            fallbackImage={`${this.state.baseImgixUrl}/country/` + this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails[0].countryName.split(/[\d .-]+/).join("").toLowerCase() + `.jpg?auto=format&fit=crop&h=435&w=1920&crop=entropy&q=55`}
                        />}
                    <div className="content-area fil-site">
                        {(this.props.feelUserJourney.dynamicSections.subCategories && this.props.feelUserJourney.dynamicSections.subCategories.length > 1) &&
                            <SubCategoryTab subCategories={this.props.feelUserJourney.dynamicSections.subCategories} />}
                        {(this.props.feelUserJourney.dynamicSections.geoData && this.props.feelUserJourney.dynamicSections.geoData.cities.length > 1) &&
                            <TopLocations dynamicSections={this.props.feelUserJourney.dynamicSections} />}
                        {(this.props.feelUserJourney.dynamicSections.dynamicPlaceSections && this.props.feelUserJourney.dynamicSections.dynamicPlaceSections.length > 0) &&
                            <DynamicSections session={this.props.session} pageType={1}
                                dynamicSections={this.props.feelUserJourney.dynamicSections.dynamicPlaceSections}
                                generic_category={generic_catgeoryName}
                                generic_location_name={generic_location_name}
                                generic_countryName={generic_countryName}
                                generic_subcategory_name={generic_subCategoryName}
                                isLiveOnine={isLiveOnine}
                                currentUrl={data}
                            />
                        }
                        {(this.props.feelUserJourney.dynamicSections.allPlaceTiles && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) &&
                            <>
                                <section className={`${sectionClassName}`}>
                                    <AllFeelSection session={this.props.session} allPlaceTiles={this.props.feelUserJourney.dynamicSections.allPlaceTiles}
                                        generic_category={generic_catgeoryName}
                                        generic_location_name={generic_location_name}
                                        generic_countryName={generic_countryName}
                                        generic_subcategory_name={generic_subCategoryName} />
                                </section>
                            </>
                        }
                    </div>
                </>
            } else if (!this.props.feelUserJourney.fetchSuccess && this.props.feelUserJourney.isError) {
                return <div className="home-view-wrapper text-center container py-5 my-5">
                    <h3>Hey! We’re working on bringing you amazing experiences in this category. Meanwhile, please explore some of our other exciting categories.</h3>
                    <a href="/" className="btn bnt-lg site-primery-btn mt-4">Go To Home</a>
                </div>;
            }
            else {
                return <div>
                    <FeelLoader />
                </div>;
            }
        }
    }
    GetPageType(data, typeName) {
        let pageType = null;
        if (data.query.country != undefined && data.query.country != 0) {
            pageType = "country";
        }
        if (data.query.state != undefined && data.query.state != 0) {
            pageType = "state";
        }
        if (data.query.city != undefined && data.query.city != 0) {
            pageType = "city";
        }
        if (pageType == null) {
            pageType = typeName;
        }
        return pageType;
    }
}
export default connect(
    (state: IApplicationState) => ({
        feelUserJourney: state.feelUserJourney,
        session: state.session
    }),
    (dispatch) => bindActionCreators({ ...FeelUserJourneyStore.actionCreators, ...sessionActionCreators }, dispatch)
)(CategoryContainer);
