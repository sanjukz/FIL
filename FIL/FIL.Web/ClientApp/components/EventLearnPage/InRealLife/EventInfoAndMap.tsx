import * as React from "React";
import { ReactHTMLConverter } from "react-html-converter/browser";
import { autobind } from "core-decorators";
import EventHostDetail from "./EventHostDetail";
var CountryLanguage = require('country-language');

export default class EventInfoAndMap extends React.Component<any, any> {
    constructor(props) {
        super(props);
        this.state = {
            isShowLess: true,
            weather: {
                mintemp: 0,
                maxtemp: 0,
                link: ''
            }
        };
    }

    formatTiqetsCheckoutDetails(detailList) {
        if (detailList != undefined) {
            let splitedList = detailList.split("*").filter((item) => { return item != "" });
            var formattedList = splitedList.map((item) => {
                if (item != "")
                    return <li>{item.trim()}</li>
            })
            return formattedList;
        } else {
            let returnArray = [];
            return returnArray;
        }
    }

    @autobind
    showLessClick(val) {
        this.setState({ isShowLess: !this.state.isShowLess });
    }
    public render() {
        var that = this, containRoutes = false;
        // let included = [], excluded = [], mustKnow = [], goodKnow = [], prePurchase = [], usage = [], postPurchase = [];
        if (this.props.citySightSeeingRoutes != undefined && this.props.citySightSeeingRoutes != null && this.props.citySightSeeingRoutes.length > 0) {
            containRoutes = true;
            var routes = this.props.citySightSeeingRoutes.map(function (item) {
                let filteredRouteLocations = that.props.citySightSeeingRouteDetails.filter(function (val) {
                    return val.citySightSeeingRouteId == item.id
                })
                if (filteredRouteLocations != null && filteredRouteLocations.length > 0) {
                    var routeLocations = filteredRouteLocations.map(function (location) {
                        return <li>{location.routeLocationName}</li>
                    })
                }
                let audLang = item.routeAudioLanguages.split(",");
                let audLangData = audLang.map(function (lang) {
                    let language = CountryLanguage.getLanguage(lang).name
                    if (CountryLanguage.countryCodeExists(lang))
                        return <li>{language[0]}</li>
                })
                return (<div className="accordion" id={"accordionExample" + item.id}>
                    <h5 className="mb-0" id="headingOne">
                        <a className="d-block bg-white mb-2 collapsed" data-toggle="collapse" href={"#" + item.id} aria-expanded="true" aria-controls="collapseOne">
                            <i className="fa fa-angle-right"></i> {item.routeName}
                        </a>
                    </h5>
                    <div id={item.id} className="collapse" aria-labelledby="headingOne" data-parent="#accordionExample">
                        <ul>
                            <li>Route Duration:{item.routeDuration}</li>
                            <li>Route Type:{item.routeType.toLowerCase()}</li>
                            <li>Route Start Time:{item.routeStartTime}</li>
                            <li>Route End Time: {item.routeEndTime}</li>
                            <li>Route Frequency: {item.routeFrequency}</li>
                            {audLangData != null && <li> <a data-toggle="collapse" href={"#Audio" + item.id} role="button" aria-expanded="false" aria-controls="collapseExample">
                                Route Audio Languages
      </a>
                                <div className="collapse" id={"Audio" + item.id}>
                                    <ol className="clearfix mt-2 mb-3">{audLangData}</ol>
                                </div>
                            </li>}
                            {routeLocations != null && <li><a data-toggle="collapse" href={"#Locations" + item.id} role="button" aria-expanded="false" aria-controls="collapseExample">
                                Route Locations
      </a>
                                <div className="collapse" id={"Locations" + item.id}>
                                    <ol className="clearfix mt-2 mb-3">{routeLocations}</ol>
                                </div>
                            </li>}
                        </ul>
                    </div>
                </div>)
            })
        }
        var eventInfo = this.props.eventInfo;
        var Description = eventInfo.description;
        const converter = ReactHTMLConverter();
        var searchString = eventInfo.name.replace("&", "");
        const MY_API = 'AIzaSyCc3zoz5TZaG3w2oF7IeR - fhxNXi8uywNk';
        let _url = `https://www.google.com/maps/embed/v1/place?key=${MY_API}&q=${searchString.split('&').join('and')},${this.props.city}`;
        if (this.props.eventInfo.eventSourceId && this.props.eventInfo.eventSourceId == 4) {
            _url = `https://www.google.com/maps/embed/v1/place?key=${MY_API}&q=${this.props.city}`
        } else if (this.props.subCategory.eventCategoryId == 33) {
            searchString = this.props.venue.addressLineOne + this.props.city;
            _url = `https://www.google.com/maps/embed/v1/place?key=${MY_API}&q=${searchString}`
        }
        converter.registerComponent('EventInfoAndMap', EventInfoAndMap);
        return <div className="row pb-3 hoho-content">
            <div className={(this.props.isLiveStream ?"col-12" :(!this.state.isShowLess) ? "col-xl-8 col-lg-7 col-sm-12" : "col-xl-8 col-lg-7 col-sm-12 page-content-text lean-more-txt")}>
                <div><p>{converter.convert(Description)}</p>
                {!this.props.isLiveStream ?
                    <div className="ShowLink">
                        <a href="JavaScript:Void(0)" onClick={this.showLessClick} className="stretched-link" >
                            {this.state.isShowLess ? "Show More" : "Show Less"}
                        </a>
                        
                    </div>:
                   <EventHostDetail  eventHostMappings={this.props.eventHostMappings}/>}
                </div>
                {containRoutes && <div> <h4 className="border-bottom pb-2">Routes</h4>
                    {routes}</div>}
            </div>
            {!this.props.isLiveStream && 
            <div className="col-xl-4 col-lg-5 col-sm-12">
                <div className="content-map">
                    <iframe
                        src={_url} width="100%" height="310" frameBorder="0" className="iframes" allowFullScreen></iframe >
                </div>
            </div>
              }
        </div>
    }
}