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
import CountryDetailBlock from "./Comman/CountryDetailBlock";
import CategoryTab from "./Comman/CategoryTab";
import * as FeelUserJourneyStore from "../stores/FeelUserJourney";
import FeelUserJourneyRequestViewModel from "../models/FeelUserJourney/FeelUserJourneyRequestViewModel";
import * as parse from "url-parse";
import TimeLine from '../components/CityCountryLanding/TimeLine';
import * as PubSub from 'pubsub-js';
import { Element, scroller } from 'react-scroll';
import { gets3BaseUrl } from "../utils/imageCdn";
import Metatag from "../components/Metatags/Metatag";

type CategoryContainerComponentProps = FeelUserJourneyStore.IFeelUserJourneyProps
    & typeof FeelUserJourneyStore.actionCreators
    & ISessionProps
    & typeof sessionActionCreators
    & RouteComponentProps<{}>;

class CountryContainer extends React.Component<CategoryContainerComponentProps, any> {
    constructor(props) {
        super(props);
        this.state = {};
    }

    getRequestMode(data) {
        var requestModel: FeelUserJourneyRequestViewModel = {
            category: data.query.category ? +data.query.category : 0,
            city: data.query.city ? +data.query.city : 0,
            country: data.query.country ? +data.query.country : 0,
            pageType: 2,
            pagePath: window.location.pathname,
            state: data.query.state ? +data.query.state : 0,
            subCategory: data.query.subcategory ? +data.query.subcategory : 0
        };
        return requestModel;
    }

    componentDidMount() {
        const data: any = parse(location.search, location, true);
        var requestModel = this.getRequestMode(data);
        this.props.requestDyanamicSectionResponseData(requestModel);

    }
    ScrollToFeels() {
        scroller.scrollTo('scroll-container', {
            duration: 800,
            delay: 0,
            smooth: 'easeInOutQuart'
        });
    }

    public UNSAFE_componentWillReceiveProps(newProps) {
        if (newProps.location.pathname != this.props.location.pathname) {
            const updatedProps: any = parse(newProps.location.search, location, true);
            var requestModel = this.getRequestMode(updatedProps);
            this.props.requestDyanamicSectionResponseData(requestModel);
        }
    }

    render() {
        if (this.props.feelUserJourney.fetchSuccess) {
            PubSub.publish("UPDATE_SEARCH_CATEGORY", this.props.feelUserJourney.dynamicSections.searchValue);
            let imgURL = "";
            if (this.props.feelUserJourney.dynamicSections.allPlaceTiles && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) {
                let randomNumber = Math.floor(Math.random() * ((this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length - 1) - 0 + 1) + 0);
                imgURL = gets3BaseUrl() + "/places/about/" + this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails[randomNumber].altId.toUpperCase() + "-about.jpg";
            }
            let countryName;
            if (this.props.feelUserJourney.dynamicSections && this.props.feelUserJourney.dynamicSections.contryPageDetails && this.props.feelUserJourney.dynamicSections.contryPageDetails.sectionTitle) {
                countryName = this.props.feelUserJourney.dynamicSections.contryPageDetails.sectionTitle;
            }
            let bgClass = "";
            if (this.props.feelUserJourney.dynamicSections.dynamicPlaceSections && this.props.feelUserJourney.dynamicSections.dynamicPlaceSections.length % 2 == 0) {
                bgClass = " bg-light"
            }
            return (
                <div>
                    <Metatag url={this.props.location.pathname} metaContent={null} title={null} isDescriptionAvailable={false} categoryName={null} countryName={countryName} />
                    {(this.props.feelUserJourney.dynamicSections.allPlaceTiles && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) && <TopBanner
                        img={imgURL}
                        fallbackImage={`${gets3BaseUrl()}/country/` + this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails[0].countryName.split(/[\d .-]+/).join("").toLowerCase() + `.jpg`}
                    />}
                    <div className="content-area pt-0 pb-0">
                        {(this.props.feelUserJourney.dynamicSections.subCategories && this.props.feelUserJourney.dynamicSections.subCategories.length > 1 &&
                            !this.props.feelUserJourney.dynamicSections.subCategories[0].isMainCategory) &&
                            <SubCategoryTab subCategories={this.props.feelUserJourney.dynamicSections.subCategories} />}
                        {(this.props.feelUserJourney.dynamicSections.subCategories && this.props.feelUserJourney.dynamicSections.subCategories.length > 1 &&
                            this.props.feelUserJourney.dynamicSections.subCategories[0].isMainCategory) &&
                            <CategoryTab subCategories={this.props.feelUserJourney.dynamicSections.subCategories} />}
                        {(this.props.feelUserJourney.dynamicSections.geoData && this.props.feelUserJourney.dynamicSections.geoData.cities.length > 1) &&
                            <div className="fil-site"> <TopLocations dynamicSections={this.props.feelUserJourney.dynamicSections} />                            </div>
                        }
                        <CountryDetailBlock sectionDetails={this.props.feelUserJourney.dynamicSections.contryPageDetails} dynamicSections={this.props.feelUserJourney.dynamicSections} ScrollToFeels={this.ScrollToFeels} />
                        {(this.props.feelUserJourney.dynamicSections.dynamicPlaceSections && this.props.feelUserJourney.dynamicSections.dynamicPlaceSections.length > 0 && this.props.feelUserJourney.dynamicSections.dynamicPlaceSections[0].placeDetails[0].countryId == 101) &&
                            <TimeLine titleName={"Timeline"} />}
                        {(this.props.feelUserJourney.dynamicSections.dynamicPlaceSections && this.props.feelUserJourney.dynamicSections.dynamicPlaceSections.length > 0) &&
                            <div className="fil-site">
                                <DynamicSections pageType={2} session={this.props.session} dynamicSections={this.props.feelUserJourney.dynamicSections.dynamicPlaceSections} />
                            </div>
                        }
                        {(this.props.feelUserJourney.dynamicSections.allPlaceTiles && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails && this.props.feelUserJourney.dynamicSections.allPlaceTiles.placeDetails.length > 0) &&
                            <div className="fil-site">
                                <section className={`fil-tiles-sec ${bgClass}`}>
                                    <Element name="scroll-containe" className="element" id="scroll-container">
                                        <AllFeelSection allPlaceTiles={this.props.feelUserJourney.dynamicSections.allPlaceTiles} session={this.props.session} />
                                    </Element>
                                </section>
                            </div>
                        }
                    </div>
                </div>
            );
        } else if (!this.props.feelUserJourney.fetchSuccess && this.props.feelUserJourney.isError) {
            return <div className="home-view-wrapper text-center container py-5 my-5">
                <Metatag url={this.props.location.pathname} metaContent={null} title={null} isDescriptionAvailable={false} categoryName={null} />
                <h3>Hey! We’re working on bringing you amazing experiences in this category. Meanwhile, please explore some of our other exciting categories.</h3>
                <a href="/" className="btn bnt-lg site-primery-btn mt-4">Go To Home</a>
            </div>;
        } else {
            return <div>
                <FeelLoader />
            </div>;
        }
    }
}
export default connect(
    (state: IApplicationState) => ({
        feelUserJourney: state.feelUserJourney,
        session: state.session
    }),
    (dispatch) => bindActionCreators({ ...FeelUserJourneyStore.actionCreators, ...sessionActionCreators }, dispatch)
)(CountryContainer);
