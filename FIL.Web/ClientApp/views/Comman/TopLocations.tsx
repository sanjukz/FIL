import * as React from "react";
import Slider from "react-slick";
import * as shuffle from "lodash/shuffle";
import { gets3BaseUrl } from "../../utils/imageCdn";
import ImageComponent from "../Comman/ImageComponent";

export default class TopLocations extends React.Component<any, any> {

    public constructor(props) {
        super(props);
        this.state = {
            s3BaseUrl: gets3BaseUrl(),
            baseImgixUrl: 'https://feelitlive.imgix.net/images'
        };
    }

    getLocationModel() {
        var locationModel = {
            name: "",
            url: "",
            query: "",
            imgUrl: "",
            parentCategorySlug: "",
            subCategorySlug: ""
        };
        return locationModel;
    }

    getLocationDataModel(name, url, query, imgURL, parentCategorySlug, subCategorySlug) {
        let locationModel = this.getLocationModel();
        locationModel.name = name;
        locationModel.url = url;
        locationModel.query = query;
        locationModel.imgUrl = imgURL;
        locationModel.parentCategorySlug = parentCategorySlug;
        locationModel.subCategorySlug = subCategorySlug;
        return locationModel;
    }

    render() {
        var that = this;
        const settings = {
            className: "",
            infinite: true,
            slidesToShow: 5,
            slidesToScroll: 1,
            speed: 500,
            responsive: [
                {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 3
                    }
                },
                {
                    breakpoint: 570,
                    settings: {
                        slidesToShow: 2
                    }
                },
                {
                    breakpoint: 430,
                    settings: {
                        slidesToShow: 2
                    }
                }
            ]
        };
        var locations = [];
        var citiesNames = this.props.dynamicSections.geoData.cities.map((item) => { return item.name });
        var filteredStates = this.props.dynamicSections.geoData.states.map((item) => {
            if (citiesNames.indexOf(item.name) == -1) {
                return item;
            }
        }).filter((val) => { return val != undefined });
        if (this.props.dynamicSections.geoData.countries && this.props.dynamicSections.geoData.countries.length > 1) {
            this.props.dynamicSections.geoData.countries.map((item) => {
                var imgUrl = "";
                let parentCategorySlug = "see-and-do";
                let subCategorySlug = "museums";
                for (var i = 0; i < that.props.dynamicSections.allPlaceTiles.placeDetails.length; i++) {
                    if (that.props.dynamicSections.allPlaceTiles.placeDetails[i].countryId == item.id) {
                        imgUrl = that.state.baseImgixUrl + "/places/tiles/" + that.props.dynamicSections.allPlaceTiles.placeDetails[i].altId.toUpperCase() + "-ht-c1.jpg?auto=format&fit=crop&h=159&w=212&crop=entropy&q=55";
                        parentCategorySlug = that.props.dynamicSections.allPlaceTiles.placeDetails[i].parentCategorySlug;
                        subCategorySlug = that.props.dynamicSections.allPlaceTiles.placeDetails[i].subCategorySlug;
                        break;
                    }
                }
                var locationModel = that.getLocationDataModel(item.name, item.url, item.query, imgUrl, parentCategorySlug, subCategorySlug);
                locations.push(locationModel);
            });
        }
        if (this.props.dynamicSections.geoData.cities && this.props.dynamicSections.geoData.cities.length > 1) {
            this.props.dynamicSections.geoData.cities.map((item) => {
                var imgUrl = "";
                let parentCategorySlug = "see-and-do";
                let subCategorySlug = "museums";
                let randomNumber = 0;
                var cityFilterArray = that.props.dynamicSections.allPlaceTiles.placeDetails.filter((val) => {
                    return val.cityName == item.name
                });
                if (cityFilterArray.length > 0) {
                    randomNumber = Math.floor(Math.random() * ((cityFilterArray.length - 1) - 0 + 1) + 0);
                    imgUrl = that.state.baseImgixUrl + "/places/tiles/" + cityFilterArray[randomNumber].altId.toUpperCase() + "-ht-c1.jpg?auto=format&fit=crop&h=159&w=212&crop=entropy&q=55";
                    parentCategorySlug = cityFilterArray[randomNumber].parentCategorySlug;
                    subCategorySlug = cityFilterArray[randomNumber].subCategorySlug;
                }
                var locationModel = that.getLocationDataModel(item.name, item.url, item.query, imgUrl, parentCategorySlug, subCategorySlug);
                var isExists = locations.filter((val) => {
                    return val.name == item.name
                });
                if (isExists.length == 0) {
                    locations.push(locationModel);
                }
            });
        }
        if (filteredStates.length > 1) {
            filteredStates.map((item) => {
                var imgUrl = "";
                let randomNumber = 0;
                let parentCategorySlug = "see-and-do";
                let subCategorySlug = "museums";
                var stateFilterArray = that.props.dynamicSections.allPlaceTiles.placeDetails.filter((val) => {
                    return val.stateName == item.name
                });
                if (stateFilterArray.length > 0) {
                    randomNumber = Math.floor(Math.random() * ((stateFilterArray.length - 1) - 0 + 1) + 0);
                    imgUrl = that.state.baseImgixUrl + "/places/tiles/" + stateFilterArray[randomNumber].altId.toUpperCase() + "-ht-c1.jpg?auto=format&fit=crop&h=159&w=212&crop=entropy&q=55";
                    parentCategorySlug = stateFilterArray[randomNumber].parentCategorySlug;
                    subCategorySlug = stateFilterArray[randomNumber].subCategorySlug;
                }
                var locationModel = that.getLocationDataModel(item.name, item.url, item.query, imgUrl, parentCategorySlug, subCategorySlug);
                var isExists = locations.filter((val) => {
                    return val.name == item.name
                });
                if (isExists.length == 0) {
                    locations.push(locationModel);
                }
            });
        }
        var location_data = [];
        location_data = shuffle(locations.filter(item => item.imgUrl));
        var all_locations = location_data.map((item) => {
            return <a href={item.url + item.query} className="text-decoration-none d-block">
                <div className="card">
                    <ImageComponent
                        parentCategorySlug={item.parentCategorySlug}
                        subCategorySlug={item.subCategorySlug}
                        s3BaseUrl={that.state.baseImgixUrl}
                        imgUrl={item.imgUrl}
                    />
                    <div className="card-body p-0 position-relative">
                        <p className="card-title pt-0 m-0">{item.name}</p>
                    </div>
                </div>
            </a>
        });

        return (
            <section className="feelsBlogHome one-col placesTab top-locations sec-spacing" >
                <div className="container">
                    <Slider {...settings}>
                        {all_locations}
                    </Slider>
                </div>
            </section>
        );
    }
};