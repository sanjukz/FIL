import * as React from "React";
import axios from "axios";
import {
  withScriptjs,
  withGoogleMap,
  GoogleMap,
  Marker,
} from "react-google-maps";
import { compose, withProps, withStateHandlers } from "recompose";
const { InfoBox } = require("react-google-maps/lib/components/addons/InfoBox");
import { gets3BaseUrl } from "../../../utils/imageCdn";
import ReactStars from "react-stars";
import * as debounce from "lodash/debounce";

// Cancellation Token for Axios request
const CancelToken = axios.CancelToken;
const source = CancelToken.source();

const StyledMapWithAnInfoBox = compose(
  withProps({
    googleMapURL:
      "https://maps.googleapis.com/maps/api/js?key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU&v=3.exp&libraries=geometry,drawing,places",
    loadingElement: <div style={{ height: `100%` }} />,
    containerElement: <div style={{ height: `300px` }} />,
    mapElement: <div style={{ height: `100%` }} />,
  }),
  withStateHandlers(
    () => ({
      isOpen: false,
      markerid: 0,
    }),
    {
      onToggleOpen: () => (marker) => ({
        isOpen: true,
        markerid: marker.eventDetails.altId,
      }),
      onToggleClose: () => () => ({
        isOpen: false,
      }),
    }
  ),
  withScriptjs,
  withGoogleMap
)((props: any) => (
  <GoogleMap
    defaultZoom={13}
    defaultCenter={
      new google.maps.LatLng(props.defaultCenter.lat, props.defaultCenter.lng)
    }
  >
    {props.marks.map((mark, index) => {
      return (
        <Marker
          key={index}
          position={{
            lat: parseFloat(mark.locData.lat),
            lng: parseFloat(mark.locData.lng),
          }}
          icon={`${gets3BaseUrl()}/logos/fap-pin-map.svg`}
          onMouseOver={() => props.onToggleOpen(mark)}
          onMouseOut={props.onToggleClose}
          onClick={() => window.open(`${mark.eventDetails.url}`, "_blank")}
        >
          {props.isOpen && props.markerid == mark.eventDetails.altId && (
            <InfoBox
              options={{ closeBoxURL: ``, enableEventPropagation: true }}
            >
              <div
                className="border shadow-sm media bg-white"
                style={{ width: "250px" }}
              >
                <img
                  src={`${gets3BaseUrl()}/places/tiles/${mark.eventDetails.altId.toUpperCase()}-ht-c1.jpg`}
                  onError={(e) =>
                    (e.currentTarget.src = `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`)
                  }
                  width="100"
                />
                <div
                  className="media-body p-1"
                  style={{ whiteSpace: "initial" }}
                >
                  <p className="m-0 font-weight-bold">
                    {mark.eventDetails.name}
                  </p>
                  <div>
                    <ReactStars
                      className="rating d-inline-block"
                      color2={"#572483"}
                      edit={false}
                      value={mark.eventDetails.rating}
                      count={5}
                    />
                    <div
                      className="d-inline position-relative ml-2"
                      style={{
                        top: "-5px",
                      }}
                    >{`${Math.round(mark.eventDetails.rating)}+ reviews`}</div>
                  </div>
                </div>
              </div>
            </InfoBox>
          )}
        </Marker>
      );
    })}
  </GoogleMap>
));

export default class NearByEvent extends React.PureComponent<any, any> {
  state = {
    tabValue: "see&do",
    nearMePlaces: [],
    lat: 0,
    lng: 0,
    isGetPlaceMarkers: true,
    marks: [],
  };
  componentDidMount() {
    this.props.getDataByCategory({
      category: 29,
      city: this.props.eventLearnPage.eventDetails.city.id || 0,
      country: 0,
      pagePath: this.props.match.url || "/",
      pageType: 1,
      state: 0,
      subCategory: 0,
    });
    this.getGeocode(this.props.venue.name);
  }

  setTabValue = (value, id, subCategoryId) => {
    this.props.getDataByCategory({
      category: id,
      city: this.props.eventLearnPage.eventDetails.city.id || 0,
      country: 0,
      pagePath: this.props.match.url || "",
      pageType: 1,
      state: 0,
      subCategory: subCategoryId,
    });
    this.setState({ isGetPlaceMarkers: false, marks: [] });
  };

  getGeocode = async (placeAddress) => {
    let geocode = await axios.get(
      `https://maps.googleapis.com/maps/api/geocode/json?address=${placeAddress}&key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU`,
      { cancelToken: source.token }
    );
    let lat = geocode.data.results.length
      ? geocode.data.results[0].geometry.location.lat
      : 0;
    let lng = geocode.data.results.length
      ? geocode.data.results[0].geometry.location.lng
      : 0;
    this.setState({ lat, lng });
  };

  getPlacesMarkers = async () => {
    var allPlace =
      (this.props.feelUserJourney &&
        this.props.feelUserJourney.dynamicSections &&
        this.props.feelUserJourney.dynamicSections.allPlaceTiles &&
        this.props.feelUserJourney.dynamicSections.allPlaceTiles
          .placeDetails) ||
      [];
    let filterPLaces = allPlace.filter(
      (item) => item.latitude && item.longitude
    );
    let places = filterPLaces.map(async (item) => {
      let latlng = await axios.get(
        `https://maps.googleapis.com/maps/api/geocode/json?&address=${
          item.name
        },${item.cityName}&key=AIzaSyDZ4Ik7ENzLWY1tLh1ul8NxhWBdWGK6tQU`
      );
      let locData =
        latlng &&
        latlng.data &&
        latlng.data.results.length &&
        latlng.data.results[0].geometry &&
        latlng.data.results[0].geometry.location;
      let eventDetails = item;
      return { locData, eventDetails };
    });
    let marksList = await Promise.all(places.map(async (marks) => await marks));
    if (this.state.marks.length === 0 && marksList.length > 0)
      this.setState({ marks: marksList, isGetPlaceMarkers: false });
  };

  public render() {
    if (
      this.props.feelUserJourney &&
      this.props.feelUserJourney.dynamicSections &&
      this.props.feelUserJourney.dynamicSections.allPlaceTiles &&
      this.state.marks.length === 0
    ) {
      this.getPlacesMarkers();
    }
    return (
      <>
        <ul
          className="nav nav-pills mb-3 text-uppercase"
          id="pills-tab"
          role="tablist"
        >
          <li
            className="nav-item"
            onClick={debounce(() => this.setTabValue("see&do", 29, 0), 500)}
          >
            <a
              className="nav-link active"
              id="pills-profile-tab"
              data-toggle="pill"
              href="#pills-profile"
              role="tab"
              aria-controls="pills-profile"
              aria-selected="false"
            >
              See & Do
            </a>
          </li>
          <li
            className="nav-item"
            onClick={debounce(() => this.setTabValue("eat&drink", 30, 0), 500)}
          >
            <a
              className="nav-link"
              id="pills-home-tab"
              data-toggle="pill"
              href="#pills-home"
              role="tab"
              aria-controls="pills-home"
              aria-selected="true"
            >
              Eat & Drink
            </a>
          </li>
          <li
            className="nav-item"
            onClick={debounce(
              () => this.setTabValue("experiences&activities", 32, 0),
              500
            )}
          >
            <a
              className="nav-link"
              id="pills-contact-tab"
              data-toggle="pill"
              href="#pills-contact"
              role="tab"
              aria-controls="pills-contact"
              aria-selected="false"
            >
              Experiences & Activities
            </a>
          </li>
          <li
            className="nav-item"
            onClick={debounce(() => this.setTabValue("events", 33, 0), 500)}
          >
            <a
              className="nav-link"
              id="pills-contact-tab"
              data-toggle="pill"
              href="#pills-event"
              role="tab"
              aria-controls="pills-contact"
              aria-selected="false"
            >
              Events
            </a>
          </li>
          <li
            className="nav-item"
            onClick={debounce(() => this.setTabValue("stayAt", 35, 0), 500)}
          >
            <a
              className="nav-link"
              id="pills-contact-tab"
              data-toggle="pill"
              href="#pills-food-drink"
              role="tab"
              aria-controls="pills-contact"
              aria-selected="false"
            >
              Stay At
            </a>
          </li>
        </ul>
        <div className="tab-content" id="pills-tabContent">
          <div
            className="tab-pane fade show active"
            id="pills-home"
            role="tabpanel"
            aria-labelledby="pills-home-tab"
          >
            <div style={{ height: "50vh", width: "100%" }}>
              {this.state.lat ? (
                <StyledMapWithAnInfoBox
                  marks={this.state.marks}
                  history={this.props.history}
                  defaultCenter={{ lat: this.state.lat, lng: this.state.lng }}
                />
              ) : (
                <iframe
                  src={`https://www.google.com/maps/embed/v1/place?key=AIzaSyCc3zoz5TZaG3w2oF7IeR - fhxNXi8uywNk&q=${this.props.venue.name.replace(
                    /&/g,
                    "and"
                  )}`}
                  width="100%"
                  height="100%"
                />
              )}
            </div>
          </div>
        </div>
      </>
    );
  }
}
