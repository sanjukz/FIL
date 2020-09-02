import * as React from "react";
import { Link } from "react-router-dom";
import { gets3BaseUrl } from "../../utils/imageCdn";
import "../../scss/_hostafeel.scss";
import "../../scss/landing-page.scss";
import {
  setCookieAndReload,
  getCurrencyList,
} from "../../utils/currencyFormatter";
import * as PubSub from "pubsub-js";

const HostAFeelNavigation: React.FunctionComponent<any> = (props) => {
  const [s3BaseUrl, setS3BaseUrl] = React.useState("");
  const [selectCurrency, setSelectCurrency] = React.useState("Change Currency");
  const [currencyDropdown, setCurrencyDropdown] = React.useState("hidden");

  React.useEffect(() => {
    const getUrl = async () => {
      try {
        const baseUrl: string = await gets3BaseUrl();
        setS3BaseUrl(baseUrl);
      } catch (error) {
        console.log(error);
      }
    };
    getUrl();
    var origin = (window as any).location.pathname;
    var currencydropdownhtml = "hidden";
    if (origin == "/" || origin.indexOf("/place/") !== -1) {
      currencydropdownhtml = "visible";
    }
    setCurrencyDropdown(currencydropdownhtml);
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
      setSelectCurrency(readCookie);
    }
  }, []);
  const redirectToAdmin = (view = "") => {
    let feelAdminUrl = `https://host.feelitlive.com/verifyauth/`;
    if (window.origin.indexOf("dev") > -1) {
      feelAdminUrl = `https://devadmin.feelitlive.com/verifyauth/`;
    }

    if (props.session.isAuthenticated && view) {
      window.open(
        `${feelAdminUrl}${props.session.user.altId}?view=${view}`,
        "_blank" // <- This makes it open in a new window.
      );
    } else if (props.session.isAuthenticated) {
      window.open(
        feelAdminUrl + props.session.user.altId,
        "_blank" // <- This makes it open in a new window.
      );
    } else {
      let url = "";
      if (view) {
        url = `${feelAdminUrl}userAltId?view=${view}`;
      } else {
        url = `${feelAdminUrl}userAltId?view=${view}`;
      }
      props.showSignInSignUp(true, url);
    }
  };

  var shortCurrency = getCurrencyList().shortCurrency;
  var fullCurrency = getCurrencyList().fullCurrency;

  var currentCurrencyIndex = shortCurrency.indexOf(selectCurrency);

  var currentCurrency = selectCurrency;
  shortCurrency.sort(function(x, y) {
    return x == currentCurrency ? -1 : y == currentCurrency ? 1 : 0;
  });

  fullCurrency.splice(0, 0, fullCurrency.splice(currentCurrencyIndex, 1)[0]);

  var showCurrency = shortCurrency.map((item, i) => {
    return (
      <a
        key={i}
        className={
          item == selectCurrency
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
    <>
      <header className="site-header">
        <nav className="navbar navbar-expand-lg navbar-light fixed-top shadow-sm fil-landing-tab">
          <div className="container-fluid">
            <a className="navbar-brand p-0 inner-bug-logo" href="#">
              {props.isLiveStream ? (
                <img
                  width="130"
                  src={`${s3BaseUrl}/logos/fap-live-stream.png`}
                  alt="feelitLIVE Logo"
                  title="feelitLIVE Logo"
                />
              ) : (
                <img
                  height="40"
                  src={`${s3BaseUrl}/icons/feelList-hear-new.png`}
                  alt={"FeelitLIVE Logo"}
                  title={"FeelitLIVE Logo"}
                  style={{ maxHeight: "50px", width: "auto" }}
                />
              )}
            </a>

            <ul className="navbar-nav d-inline-flex">
              {props.session.isAuthenticated ? (
                <React.Fragment>
                  <li className="nav-item d-none d-sm-block">
                    <Link
                      to="/account"
                      className="nav-link"
                      role="button"
                      aria-expanded="false"
                    >
                      My Account
                    </Link>
                  </li>
                  <li className="nav-item d-none d-sm-block">
                    <Link
                      to="#"
                      className="nav-link"
                      role="button"
                      aria-expanded="false"
                      onClick={(event) => {
                        event.preventDefault();
                        localStorage.removeItem("userToken");
                        localStorage.removeItem("cartItems");
                        props.logoutAction();
                      }}
                    >
                      Sign Out
                    </Link>
                  </li>
                </React.Fragment>
              ) : (
                <React.Fragment>
                  <li className="nav-item d-none d-sm-block">
                    <a
                      onClick={(e) => {
                        props.showSignInSignUp(true);
                      }}
                      href="javascript:void(0)"
                      className="nav-link"
                      role="button"
                      aria-expanded="false"
                    >
                      Log in
                    </a>
                  </li>
                  <li className="nav-item d-none d-sm-block">
                    <a
                      className="nav-link"
                      role="button"
                      aria-expanded="false"
                      onClick={(e) => {
                        props.showSignInSignUp(false);
                      }}
                      href="javascript:void(0)"
                    >
                      Sign up
                    </a>
                  </li>
                </React.Fragment>
              )}
              <div className="d-block d-sm-none">
                <div
                  id="google_translate_element"
                  className="d-inline-block mt-1"
                />
                <div
                  style={{ visibility: "visible" }}
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
                      <b>{selectCurrency}</b>
                    </button>
                    <div className="dropdown-menu">{showCurrency}</div>
                  </div>
                </div>
              </div>
              <li className="nav-item d-none d-sm-inline-block">
                <a
                  className="btn btn-primary"
                  role="button"
                  aria-expanded="false"
                  onClick={() => redirectToAdmin("3")}
                  href="javascript:void(0)"
                >
                  Get Started
                </a>
              </li>
            </ul>
          </div>
        </nav>
      </header>
      <footer className="appFooter container-fluid d-none">
        <div className="row text-center">
          <div className="col p-0">
            <a
              className="text-white stretched-link d-block w-100 p-0 bg-transparent border-0"
              href="/feel-itineraryplanner"
            >
              <img
                src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/logos/filit-logo-mob.png"
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
                src={`${s3BaseUrl}/footer/app-footer/hostafeel.png`}
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
              <a
                href={
                  props.session &&
                  props.session.user &&
                  props.session.user.altId
                    ? `https://host.feelitlive.com/verifyauth/${
                        props.session &&
                        props.session.user &&
                        props.session.user.altId
                          ? props.session.user.altId
                          : ""
                      }?view=2`
                    : "/login"
                }
                className="dropdown-item"
                target="_blank"
              >
                Become a feelGuide
              </a>
            </div>
          </div>
          <div className="col p-0 build-itin">
            {/* <a
                            href="/feel-itineraryplanner"
                            className="text-white stretched-link d-block itin-AppIcon-link"
                        >
                            <span className="itin-AppIcon">
                                <img
                                    src={`${s3BaseUrl}/icons/feelList-hear-new.png`}
                                    alt="Build Itinerary"
                                />
                            </span>
                            <span className="d-block">Build Itinerary</span>
                        </a> */}
            <a
              href="/create-online-experience"
              className="text-white stretched-link d-block itin-AppIcon-link"
            >
              <span className="itin-AppIcon">
                <img
                  src={`${s3BaseUrl}/icons/live-tag.svg`}
                  alt="Live Stream"
                />
              </span>
              <span className="d-block">Now Live</span>
            </a>
          </div>
          <div className="col p-0">
            <a href="/itinerary" className="text-white stretched-link d-block">
              <img
                src={`${s3BaseUrl}/footer/app-footer/bag.png`}
                alt="feel bag"
              />
              <span className="d-block">Bag</span>
            </a>
          </div>
          <div className="col p-0">
            <Link
              to={{
                pathname: "/login",
                state: {
                  refer: "/account",
                },
              }}
              className="text-white stretched-link d-block"
            >
              <img
                src={`${s3BaseUrl}/footer/app-footer/account.png`}
                alt="feel account"
              />
              <span className="d-block">Account</span>
            </Link>
          </div>
        </div>
      </footer>
    </>
  );
};

export default HostAFeelNavigation;
