import * as React from "react";
import StandardFeelCard from "./StandardFeelCard";

export default class AllFeelSection extends React.Component<any, any> {

    render() {
        let title = this.GetTitle();

        return (<div className="container">
            <h3 className="text-purple" title={title}>
                {this.props.allPlaceTiles.sectionDetails.heading}
            </h3>
            <StandardFeelCard
                display={true}
                placeCardsData={this.props.allPlaceTiles.placeDetails}
                isShowMore={true}
                session={this.props.session}
            />
        </div>
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
