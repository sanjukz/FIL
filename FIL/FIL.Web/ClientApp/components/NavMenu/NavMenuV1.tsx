import * as React from "react";
import { Link } from "react-router-dom";
import "../../scss/_header.scss";
import "../../scss/_custom.scss";
import "../../scss/_tiles.scss";
import "../../scss/_buttons.scss";
import "../../scss/_custome_Carousel.scss";
import "../../scss/fil-style.scss";
import NavCart from "../../components/NavCart/NavCart";
import * as PubSub from "pubsub-js";
import InnerPageSearch from "../../components/EventLearnPage/InnerPagesSearch";
import { CategoryViewModel } from "../../models/CategoryViewModel";
import { SearchResponseViewModel } from "../../models/Search/SearchResponseViewModel";
import {
  Content,
  DefaultCitySearchResult,
  DefaultStateSearchResult,
  DefaultCountrySearchResult,
} from "../../models/SiteContentViewModel";
import axios from "axios";
import * as ReactTooltip from "react-tooltip";
import { getCurrentSlug } from "../../utils/GetCurrentSlug";
import { isMobile, osName } from "react-device-detect";
import { IFeelNearMeState, KnownAction } from "../../stores/FeelNearMe";
import googleGeoCodingApi from "../../utils/googleGeocodingApi";
import { IAppThunkAction } from "../../stores/index";
import { gets3BaseUrl } from "../../utils/imageCdn";
import {
  setCookieAndReload,
  getCurrencyList,
} from "../../utils/currencyFormatter";
let geoCodingApi = new googleGeoCodingApi();
var customHeaderClass = "navbar navbar-expand-lg navbar-light fixed-top";

export interface INavMenuProps {
  getNearByPlacesByPagination: (
    index: number,
    slug: string,
    search: string
  ) => IAppThunkAction<KnownAction>;
  getNearByLocations: (lat: number, lon: number, distance: number) => void;
  nearMe: IFeelNearMeState;
  isOverlay: boolean;
  categories: CategoryViewModel[];
  requestSiteContent: () => void;
  clearSearchAction: () => void;
  searchSuccess: boolean;
  clearSearchSuccess: boolean;
  searchResult: SearchResponseViewModel;
  content: Content;
  defaultSearchCities: DefaultCitySearchResult[];
  defaultSearchStates: DefaultStateSearchResult[];
  defaultSearchCountries: DefaultCountrySearchResult[];
  searchAction: (value1: any, value2: any) => void;
  getCategoryEventsByPaginationidex: (
    index: number,
    slug: string,
    search: string
  ) => IAppThunkAction<any>;
  changeNearMeData: (val: any) => void;
}

class NavMenuV1 extends React.PureComponent<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      pathName: "/",
      isNearMeClicked: false,
      currencyDropdown: "hidden",
      selectCurrency: "Change Currency",
      defaultSearchValue: "",
      logoName: "aplace",
      cartCount: 0,
      citySearchKeyword: "",
      stateSearchKeyword: "",
      countrySearchKeyword: "",
      siteName: "feelaplace",
      s3BaseUrl: gets3BaseUrl(),
      isMobileScreen: false,
    };
  }

  // TODO: XXX: need to make logoName dynamic
  public UNSAFE_componentWillMount() {
    PubSub.subscribe("UPDATE_CART_COUNT_EVENT", this.subscriber);

    PubSub.subscribe("UPDATE_NAV_MENUE", this.subscriber);
    if (typeof window != "undefined" && window.screen.width <= 576) {
      customHeaderClass = "navbar navbar-expand-lg navbar-light";
    }
  }

  public componentDidMount() {
    var logoName = "aplace";
    var origin = (window as any).location.origin;
    var siteName = origin.split(".")[1];
    if (origin.indexOf("feel") > -1) {
      if (siteName == "feelaplace") {
        logoName = "aplace";
      } else {
        logoName = siteName.slice(4).toLowerCase();
      }
    }
    if (
      typeof window !== "undefined" &&
      localStorage.getItem("currentCartItems") != null &&
      localStorage.getItem("currentCartItems") != "0" &&
      localStorage.getItem("currentCartItems") !== "undefined"
    ) {
      var data = JSON.parse(localStorage.getItem("currentCartItems"));
      this.setState({ cartCount: data.length });
    }
    if ((window as any).location.href.indexOf("order-confirmation") > -1) {
      this.setState({ cartCount: 0 });
    }
    this.setState({ logoName: logoName });
    if (this.state.pathname != "/") {
      // this.props.requestSiteContent();
      this.setState({
        citySearchKeyword: "",
        stateSearchKeyword: "",
        countrySearchKeyword: "",
        defaultSearchValue: "",
      });
    }
    this.setState({ pathName: window.location.pathname });

    var origin = (window as any).location.pathname;
    var currencydropdownhtml = "hidden";
    if (origin == "/" || origin.indexOf("/place/") !== -1) {
      currencydropdownhtml = "visible";
    }
    this.setState({ currencyDropdown: currencydropdownhtml });

    //read cookie value
    var nameEQ = "user_currency=";
    var readCookie = "";
    var ca = document.cookie.split(";");
    for (var i = 0; i < ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == " ") c = c.substring(1, c.length);
      if (c.indexOf(nameEQ) == 0) {
        readCookie = c.substring(nameEQ.length, c.length);
      }
    }

    if (readCookie != "") {
      this.setState({ selectCurrency: readCookie });
    }

    ReactTooltip.show(document.getElementById("feelrTooltip"));
    PubSub.subscribe("UPDATE_CURRENCY_TAB", this.toggleCurrencyTab.bind(this));
    if (typeof window != "undefined" && window.screen.width < 768) {
      this.setState({ isMobileScreen: true });
    }
  }

  public toggleCurrencyTab() {
    var origin = (window as any).location.pathname;
    var currencydropdownhtml = "none";
    if (
      origin.indexOf("/ticket-purchase-selection/") !== -1 ||
      origin.indexOf("/checkout") !== -1 ||
      origin.indexOf("/payment") !== -1
    ) {
      currencydropdownhtml = "none";
    } else {
      currencydropdownhtml = "block";
    }
    this.setState({ currencyDropdown: currencydropdownhtml });
  }

  public subscriber = (msg, data) => {
    if (
      typeof window !== "undefined" &&
      localStorage.getItem("currentCartItems") != null &&
      localStorage.getItem("currentCartItems") != "0"
    ) {
      var data = JSON.parse(localStorage.getItem("currentCartItems"));
      this.setState({ cartCount: data.length });
    }
    this.setState({ pathName: window.location.pathname });
  };
  public clearSearchAction(e) {
    this.props.clearSearchAction();
    this.setState({
      citySearchKeyword: "",
      stateSearchKeyword: "",
      countrySearchKeyword: "",
      defaultSearchValue: "",
    });
  }

  public setCitySearchKeyword(search) {
    this.setState({
      citySearchKeyword: search,
      stateSearchKeyword: "",
      countrySearchKeyword: "",
      defaultSearchValue: search,
    });
  }

  public setStateSearchKeyword(search) {
    this.setState({
      citySearchKeyword: "",
      stateSearchKeyword: search,
      countrySearchKeyword: "",
      defaultSearchValue: search,
    });
  }

  public setCountrySearchKeyword(search) {
    this.setState({
      citySearchKeyword: "",
      stateSearchKeyword: "",
      countrySearchKeyword: search,
      defaultSearchValue: search,
    });
  }

  getNearMeData = () => {
    let slug = getCurrentSlug();
    this.setState({ isNearMeClicked: !this.state.isNearMeClicked }, () => {
      if (this.state.isNearMeClicked) {
        navigator.geolocation.getCurrentPosition(async (pos) => {
          let lat = parseFloat(pos.coords.latitude.toFixed(6));
          let lon = parseFloat(pos.coords.longitude.toFixed(6));
          const loc = await geoCodingApi.getLocationFromLatLong(lat, lon);
          axios
            .get("api/itinerary/cities")
            .then((response: any) => {
              let cityData = response.data.itinerarySerchData.filter(
                (item) => item.cityName == loc.city
              );
              this.props.history.push(
                `/country/${cityData[0].countryName}/${
                  cityData[0].cityName
                }?country=${cityData[0].countryId}&city=${cityData[0].cityId}`
              );
            })
            .catch((error) => {
              throw error;
            });
        });
      }
    });
  };

  updateNearMeData = (lat, lon, distance) => {
    let slug = getCurrentSlug();
    this.props
      .getNearByLocations(lat, lon, distance)
      .then((data) => {
        if (data.nearbyPlaces.length > 0) {
          this.props.getCategoryEventsByPaginationidex(
            1,
            slug,
            data.nearbyPlaces[0].city
          );
        } else {
          this.updateNearMeData(lat, lon, distance + 100);
        }
      })
      .catch((error) => {
        throw error;
      });
  };

  public render() {
    var shortCurrency = getCurrencyList().shortCurrency;
    var fullCurrency = getCurrencyList().fullCurrency;

    var currentCurrencyIndex = shortCurrency.indexOf(this.state.selectCurrency);

    var currentCurrency = this.state.selectCurrency;
    shortCurrency.sort(function(x, y) {
      return x == currentCurrency ? -1 : y == currentCurrency ? 1 : 0;
    });

    fullCurrency.splice(0, 0, fullCurrency.splice(currentCurrencyIndex, 1)[0]);

    var showCurrency = shortCurrency.map((item, i) => {
      return (
        <a
          key={i}
          className={
            item == this.state.selectCurrency
              ? "dropdown-item font-weight-bold"
              : "dropdown-item"
          }
          href="JavaScript:Void(0);"
          onClick={(e) => setCookieAndReload(item)}
          dangerouslySetInnerHTML={{ __html: fullCurrency[i] }}
        />
      );
    });
    return (
      <div>
        {typeof window !== "undefined" &&
          window.navigator.userAgent.indexOf("gonative") === -1 && (
            <div
              className="alert alert-primary rounded-0 border-0 alert-dismissible py-2 px-2 mob-app-noti d-block d-sm-none m-0 fade show"
              role="alert"
            >
              <button
                type="button"
                className="close"
                data-dismiss="alert"
                aria-label="Close"
              >
                <span aria-hidden="true">×</span>
              </button>
              <div className="media position-relative">
                <div className="bg-white p-1 text-center mr-2 rounded logo-app-noti">
                  <img
                    src={`${this.state.s3BaseUrl}/icons/feelList-hear-new.png`}
                    width="24"
                  />
                </div>

                <div className="media-body">
                  <div>feelitLIVE</div>
                  <div className="text-warning small">
                    <span className="fa fa-star checked" />
                    <span className="fa fa-star checked" />
                    <span className="fa fa-star checked" />
                    <span className="fa fa-star checked" />
                    <span className="fa fa-star-half-o" />
                  </div>
                  {osName.toLowerCase() == "ios" && isMobile ? (
                    <a
                      href="https://itunes.apple.com/in/app/feelaplace/id1458680003?mt=8"
                      className="btn btn-sm btn-warning"
                    >
                      INSTALL NOW
                    </a>
                  ) : (
                    <a
                      href="https://play.google.com/store/apps/details?id=com.feelaplace.app"
                      className="btn btn-sm btn-warning"
                    >
                      INSTALL NOW
                    </a>
                  )}
                </div>
              </div>
            </div>
          )}

        <div className="fil-site fil-home-page site-header shadow-none">
          <nav className="navbar bg-white fixed-top shadow-sm">
            <a className="navbar-brand" href="/">
              <img
                src={`${this.state.s3BaseUrl}/logos/fap-live-stream.png`}
                alt="FeelitLIVE Logo"
                width="130"
              />
            </a>
            {typeof window !== "undefined" && window.location.pathname != "/" && (
              <div className="fap-inner-search">
                <InnerPageSearch />
              </div>
            )}
            <div className="d-block d-sm-none">
              {this.state.isMobileScreen && (
                <div
                  id="google_translate_element"
                  className="d-inline-block sel-lang"
                />
              )}

              <div
                style={{ visibility: this.state.currencyDropdown }}
                className="d-inline-block ml-2 align-top mt-1"
              >
                <div className="btn-group currencyDropdown">
                  <button
                    type="button"
                    className="btn btn-sm btn-outline-secondary dropdown-toggle"
                    data-toggle="dropdown"
                    aria-haspopup="true"
                    aria-expanded="false"
                  >
                    <b>{this.state.selectCurrency}</b>
                  </button>
                  <div className="dropdown-menu">{showCurrency}</div>
                </div>
              </div>
            </div>
            <div className="nav-right d-none d-sm-block">
              <a
                href="/create-online-experience"
                className="btn btn-sm btn-outline-primary"
              >
                Host Online
              </a>
              <a
                href="/host-a-feel"
                className="btn btn-sm btn-outline-primary ml-4"
              >
                Host In-Real-Life
              </a>
              <a
                href="/feel-itineraryplanner"
                className="btn btn-link p-0 ml-4"
                id="feelitnav"
              >
                <img
                  src={`${
                    this.state.s3BaseUrl
                  }/fil-images/icon/feelit-icon.svg`}
                  alt="Feel It"
                  width="20"
                />
              </a>
              <div className="dropdown d-inline site-header shadow-none">
                <NavCart
                  isAuth={false}
                  showSignInSignUp={this.props.showSignInSignUp}
                />
              </div>
              <div className="dropdown d-inline site-header shadow-none">
                <a
                  href="#"
                  className="btn btn-link p-0 ml-4"
                  id="feelList"
                  data-toggle="dropdown"
                  aria-haspopup="true"
                  aria-expanded="false"
                >
                  <img
                    src={`${this.state.s3BaseUrl}/icons/feellist.svg`}
                    alt="feelList"
                    width="21"
                  />
                </a>
                <ul
                  className="dropdown-menu dropdown-cart p-4 dropdown-menu-right"
                  aria-labelledby="feelList"
                >
                  <li>
                    Please{" "}
                    <a
                      onClick={(e) => {
                        this.props.showSignInSignUp(true, null);
                      }}
                      href="javascript:void(0)"
                    >
                      sign in
                    </a>{" "}
                    to see your feelList.
                  </li>
                </ul>
              </div>
              <a
                onClick={(e) => {
                  this.props.showSignInSignUp(true, null);
                }}
                href="javascript:void(0)"
                className="btn btn-link p-0 ml-4 site-text-link"
              >
                Log in
              </a>
              <a
                onClick={(e) => {
                  this.props.showSignInSignUp(false, null);
                }}
                href="javascript:void(0)"
                className="btn btn-link p-0 ml-4 site-text-link"
              >
                Sign up
              </a>
            </div>
          </nav>
        </div>

        <footer className="appFooter container-fluid d-none">
          <div className="row text-center">
            <div className="col p-0">
              <a
                className="text-white stretched-link d-block w-100 p-0 bg-transparent border-0"
                href="/feel-itineraryplanner"
              >
                <img
                  src={`${this.state.s3BaseUrl}/logos/filit-logo-mob.png`}
                  alt="feel It"
                  width="15"
                />
                <span className="d-block">Plan</span>
              </a>
            </div>
            <div className="col p-0">
              <button
                className="text-white stretched-link d-block w-100 p-0 bg-transparent border-0"
                data-toggle="dropdown"
                aria-haspopup="true"
                aria-expanded="false"
              >
                <img
                  src={`${
                    this.state.s3BaseUrl
                  }/footer/app-footer/hostafeel.png`}
                  alt="host a feel"
                />
                <span className="d-block">Host</span>
              </button>
              <div className="dropdown-menu">
                <a href="/create-online-experience" className="dropdown-item">
                  Host online
                </a>
                <div className="dropdown-divider" />
                <a href="/host-a-feel" className="dropdown-item">
                  Host in-real-life
                </a>
                <div className="dropdown-divider" />
                <a href="/login" className="dropdown-item">
                  Become a feelGuide
                </a>
              </div>
            </div>
            <div className="col p-0 build-itin">
              <a
                href="/create-online-experience"
                className="text-white stretched-link d-block itin-AppIcon-link"
              >
                <span className="itin-AppIcon">
                  <img
                    src={`${this.state.s3BaseUrl}/icons/live-tag.svg`}
                    alt="Live Stream"
                  />
                </span>
                <span className="d-block">Now Live</span>
              </a>
            </div>
            <div className="col p-0">
              <a
                href="/itinerary"
                className="text-white stretched-link d-block"
              >
                <img
                  src={`${this.state.s3BaseUrl}/footer/app-footer/bag.png`}
                  alt="feel bag"
                />
                <span className="d-block">Bag</span>
              </a>
            </div>
            <div className="col p-0">
              <a
                onClick={(e) => {
                  this.props.showSignInSignUp(true, null);
                }}
                href="javascript:void(0)"
                className="text-white stretched-link d-block"
              >
                <img
                  src={`${this.state.s3BaseUrl}/footer/app-footer/account.png`}
                  alt="feel account"
                />
                <span className="d-block">Account</span>
              </a>
            </div>
          </div>
        </footer>
      </div>
    );
  }
}

export default NavMenuV1;
