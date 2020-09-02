import * as React from "react";
import StandardFeelCard from "./StandardFeelCard";
import PlaceColSection from "./PlaceColSection";

export default class DynamicSections extends React.Component<any, any> {

    constructor(props) {
        super(props);
        this.state = {};
    }

    render() {
        let title = this.GetTitle();
        var that = this; let bgClass = "";
        let dynamicSections = this.props.dynamicSections.map((item, index) => {
            let noOfSlides = 3;
            if (index % 2 != 0) {
                noOfSlides = 4;
            }
            if (item.placeDetails.length < noOfSlides) {
                noOfSlides = item.placeDetails.length;
            }
            let placeLength = item.placeDetails.map((val) => { return val.name }).filter(function (value, index, self) {
                return self.indexOf(value) === index;
            }).length;
            let isOnline = item.placeDetails.filter((val) => { return val.masterEventTypeId == 4 }).length;
            if ((placeLength > 2 || (isOnline && placeLength > 1)) && !(that.props.pageType == 2 && (index == 1 || index == 3))) {
                bgClass = "";
                if (this.props.isLiveOnine && (this.props.currentUrl.query.subcategory == undefined || this.props.currentUrl.query.subcategory == 0)) {
                    if (index + 1 % 2 == 0) {
                        bgClass = "bg-light"
                    }
                } else {
                    if (index % 2 == 0) {
                        bgClass = "bg-light"
                    }
                }
                return <section className={`recommended-feels fil-tiles-sec one-col ${bgClass}`}>
                    <div className="container">
                        {(item.sectionDetails.heading && item.sectionDetails.heading != "") && <h3 title={title} className="text-purple">{item.sectionDetails.heading}</h3>}
                        <div id="RecommendedFeels">
                            <div className="nav-tab-content">
                                <StandardFeelCard
                                    display={true}
                                    placeCardsData={item.placeDetails}
                                    isRecommendedSection={true}
                                    slidesId={"topFeels"}
                                    isSlide={true}
                                    noOfSlides={noOfSlides}
                                    session={that.props.session}
                                />
                                {(item.sectionDetails.isShowMore) &&
                                    <a className="show-all-link" href={item.sectionDetails.url + item.sectionDetails.query}>
                                        Show All
                                </a>
                                }
                            </div>
                        </div>
                    </div>
                </section>
            } else if (that.props.pageType == 2 && index == 1 && placeLength >= 2) {
                return <PlaceColSection noOfSlide={2} placeCardsData={item.placeDetails} heading={item.sectionDetails.heading} bgClass={bgClass} />
            } else if (that.props.pageType == 2 && index == 3 && placeLength >= 2) {
                return <PlaceColSection noOfSlide={item.placeDetails.length >= 3 ? 3 : item.placeDetails.length} placeCardsData={item.placeDetails} heading={item.sectionDetails.heading}
                    bgClass={bgClass} />
            }
        }).filter((item) => {
            return item != undefined
        });
        return (
            <div>{dynamicSections}</div>
        );
    }
    GetTitle() {
        let title = "";
        if (this.props.generic_subcategory_name == null) {
            if (this.props.generic_category == "See & Do") {
                if (this.props.generic_location_name != null) {
                    let location_name = this.props.generic_countryName != null ? this.props.generic_location_name + ", " + this.props.generic_countryName : this.props.generic_location_name;
                    title = "Things to See & Do around " + location_name + " - history, tickets, souvenirs, architecture, reviews & ratings, tips";
                } else {
                    title = "Things to See & Do around the world- history, tickets, souvenirs, architecture, reviews & ratings, tips ";
                }
            }
            if (this.props.generic_category == "Eat & Drink") {
                if (this.props.generic_location_name != null) {
                    let location_name = this.props.generic_countryName != null ? this.props.generic_location_name + ", " + this.props.generic_countryName : this.props.generic_location_name;
                    title = "Visit the best restaurants, cafes, bars and eateries for local delicacies in " + location_name + "";
                } else {
                    title = "Visit the best restaurants, cafes, bars and eateries for local delicacies around the world";
                }
            }
            if (this.props.generic_category == "Shop Local") {
                if (this.props.generic_location_name != null) {
                    let location_name = this.props.generic_countryName != null ? this.props.generic_location_name + ", " + this.props.generic_countryName : this.props.generic_location_name;
                    title = "Shop locally while visiting " + location_name + "";
                } else {
                    title = "Shop locally while traveling around the world";
                }
            }
            if (this.props.generic_category == "Experiences & Activities") {
                if (this.props.generic_location_name != null) {
                    let location_name = this.props.generic_countryName != null ? this.props.generic_location_name + ", " + this.props.generic_countryName : this.props.generic_location_name;
                    title = "Feel the experiences, activities and adventures while visiting " + location_name + " ";
                } else {
                    title = "Feel the experiences, activities and adventures while traveling around the world";
                }
            }
            if (this.props.generic_category == "Live Stream") {
                title = "Feel it LIVE on feelitLIVE";
            }
        } else {
            if (this.props.generic_subcategory_name != null && (this.props.generic_category == "See & Do" || this.props.generic_category == "Eat & Drink")) {
                if (this.props.generic_location_name != null) {
                    let location_name = this.props.generic_countryName != null ? this.props.generic_location_name + ", " + this.props.generic_countryName : this.props.generic_location_name;
                    title = this.props.generic_subcategory_name + " in " + location_name + "";
                } else {
                    title = this.props.generic_subcategory_name + " around the world";
                }
            }
            if (this.props.generic_subcategory_name != null && (this.props.generic_category == "Shop Local")) {
                if (this.props.generic_location_name != null) {
                    let location_name = this.props.generic_countryName != null ? this.props.generic_location_name + ", " + this.props.generic_countryName : this.props.generic_location_name;
                    title = "Shop locally for " + this.props.generic_subcategory_name + " while visiting " + location_name + ".";
                } else {
                    title = "Shop locally for " + this.props.generic_subcategory_name + " while traveling around the world";
                }
            }
            if (this.props.generic_subcategory_name != null && (this.props.generic_category == "Live Stream")) {
                title = "Feel " + this.props.generic_subcategory_name + " Live on feelitLIVE";
            }

        }

        return title;
    }
}

