import * as React from "react";
import { Link } from "react-router-dom";
import "../../scss/site.scss";
import "../../scss/_footer.scss";
import * as PubSub from "pubsub-js";
import {
  setCookieAndReload,
  getCurrencyList,
} from "../../utils/currencyFormatter";

export default class FooterBarV1 extends React.Component<any, any> {
  public constructor(props) {
    super(props);
    this.state = {
      currencyDropdown: "none",
      selectCurrency: "Change Currency",
    };
  }

  componentDidMount() {
    var origin = (window as any).location.pathname;
    var currencydropdownhtml = "none";
    if (
      origin == "/" ||
      origin.indexOf("/place/") !== -1 ||
      origin.indexOf("/ticket-purchase-selection/") !== -1
    ) {
      currencydropdownhtml = "block";
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

    PubSub.subscribe("UPDATE_CURRENCY_TAB", this.toggleCurrencyTab.bind(this));
  }

  public toggleCurrencyTab() {
    var origin = (window as any).location.pathname;
    var currencydropdownhtml = "none";
    if (
      origin.indexOf("/checkout") !== -1 ||
      origin.indexOf("/payment") !== -1 ||
      origin.indexOf("/itinerary") !== -1
    ) {
      currencydropdownhtml = "none";
    } else {
      currencydropdownhtml = "block";
    }
    this.setState({ currencyDropdown: currencydropdownhtml });
  }
  redirectToAdmin = () => {
    let redirectLink =
      this.props.session &&
        this.props.session.user &&
        this.props.session.user.altId
        ? `https://host.feelitlive.com/verifyauth/${
        this.props.session &&
          this.props.session.user &&
          this.props.session.user.altId
          ? this.props.session.user.altId
          : ""
        }?view=2`
        : null;
    if (redirectLink) {
      window.location.assign(redirectLink);
    } else {
      let feelAdminUrl = `https://host.feelitlive.com/verifyauth/`;
      if (typeof window != "undefined" && window.origin.indexOf("dev") > -1) {
        feelAdminUrl = `https://devadmin.feelitlive.com/verifyauth/`;
      }

      this.props.showSignInSignUp(true, feelAdminUrl + "userAltId?view =2");
    }
  };
  public render() {
    var shortCurrency = getCurrencyList().shortCurrency;
    var fullCurrency = getCurrencyList().fullCurrency;

    var currentCurrencyIndex = shortCurrency.indexOf(this.state.selectCurrency);

    var currentCurrency = this.state.selectCurrency;
    shortCurrency.sort(function (x, y) {
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
      <div className="fil-site fil-home-page d-none d-sm-block">
        <footer className="fil-footer bg-light border-top">
          <div className="container">
            <div className="row footer-top">
              <div className="col">
                <dl>
                  <dt>FeelitLIVE</dt>
                  <dd>
                    <Link to="/about-us">About Us</Link>
                  </dd>
                  <dd>
                    <a href="https://feelitlive.blog" target="_blank">
                      Blog
                    </a>
                  </dd>
                  <dd>
                    <a href="/coming-soon">Newsroom</a>
                  </dd>
                  <dd>
                    <a href="/careers">Careers</a>
                  </dd>
                </dl>
              </div>
              <div className="col">
                <dl>
                  <dt>DISCOVER</dt>
                  <dd>
                    {" "}
                    <Link to="/feel-itineraryplanner">Feelit</Link>
                  </dd>
                  <dd>
                    <Link to="/coming-soon">Referral Credit</Link>
                  </dd>
                  <dd>
                    <Link to="/feelforbusiness">Feel for Business</Link>
                  </dd>
                  <dd>
                    <Link to="/advertiseafeel">Advertise an experience</Link>
                  </dd>
                </dl>
              </div>
              <div className="col">
                <dl>
                  <dt>HOST</dt>
                  <dd>
                    <a href="/create-online-experience">
                      Host an online experience
                    </a>
                  </dd>
                  <dd>
                    <a href="/host-a-feel">Host an In-Real-Life experience</a>
                  </dd>
                  <dd>
                    <a
                      href="javascript:void(0"
                      onClick={() => this.redirectToAdmin()}
                    >
                      FeelGuide
                    </a>
                  </dd>
                </dl>
              </div>
              <div className="col">
                <dl>
                  <dt>SUPPORT</dt>
                  <dd>
                    <a href="https://help.feelitlive.com" target="_blank">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/help.svg"
                        width="12"
                        className="mr-2"
                        alt=""
                      />{" "}
                      Help Center
                    </a>
                  </dd>
                  <dd>
                    <a href="mailto:support@feelitLIVE.com">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/mail.svg"
                        width="12"
                        className="mr-2"
                        alt=""
                      />{" "}
                      support@feelitlive.com
                    </a>
                  </dd>
                  <dd>
                    <a href="tel:1-650-399-1712">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/phone-icon.svg"
                        width="12"
                        className="mr-2"
                        alt=""
                      />
                      +1-650-334-9500
                    </a>
                  </dd>
                </dl>
              </div>
              <div className="col">
                <dl>
                  <dt>DOWNLOAD APP</dt>
                  <dd>
                    <a
                      target="_blank"
                      href="https://itunes.apple.com/in/app/feelaplace/id1458680003?mt=8"
                    >
                      <img
                        src="https://static7.feelitlive.com/images/footer/app-store-icon.png"
                        alt=""
                        width="81"
                      />
                    </a>
                  </dd>
                  <dd>
                    <a
                      target="_blank"
                      href="https://play.google.com/store/apps/details?id=com.feelaplace.app"
                    >
                      <img
                        src="https://static7.feelitlive.com/images/footer/google-play-icon.png"
                        alt=""
                        width="81"
                      />
                    </a>
                  </dd>
                </dl>
              </div>
            </div>
          </div>
          <div className="border-top">
            <div className="container">
              <div className="row footer-bottom">
                <div className="col-sm-6">
                  © 2020 FeelitLIVE, Inc. All rights reserved
                  <span className="d-inline-block px-2">|</span>
                  <Link to="/privacy-policy">Privacy</Link>
                  <span className="d-inline-block px-2">|</span>
                  <Link to="/terms">Terms</Link>
                </div>
                <div className="col-sm-6 text-sm-right">
                  <div id="google_translate_element" />
                  <div className="btn-group currencyDropdown">
                    <a
                      href="#"
                      data-toggle="dropdown"
                      aria-haspopup="true"
                      aria-expanded="false"
                      className="mr-3  dropdown-toggle mt-1"
                    >
                      {" "}
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/currency-icon.svg"
                        alt="FIL Currency"
                        width="12"
                        className="mr-2"
                      />
                      {this.state.selectCurrency}{" "}
                    </a>
                    <div className="dropdown-menu">{showCurrency}</div>
                  </div>
                  <a
                    href="https://www.facebook.com/feelitlivecom/"
                    target="_blank"
                    rel="noopener"
                    title="Facebook"
                    aria-label="Facebook"
                    className="mr-2"
                  >
                    {" "}
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/social/facebook.svg"
                      alt="FIL Facebook"
                      width="20"
                    />
                  </a>
                  <a
                    href="https://twitter.com/feelit_LIVE"
                    target="_blank"
                    rel="noopener"
                    title="Twitter"
                    aria-label="Twitter"
                    className="mr-2"
                  >
                    {" "}
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/social/twitter.svg"
                      alt="FIL Twitter"
                      width="20"
                    />
                  </a>
                  <a
                    href="https://www.instagram.com/feelit_live/"
                    target="_blank"
                    rel="noopener"
                    title="Instagram"
                    aria-label="Instagram"
                    className="mr-2"
                  >
                    {" "}
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/social/instagram.svg"
                      alt="FIL Instagram"
                      width="20"
                    />
                  </a>
                  <a
                    href="https://www.linkedin.com/company/feelitlive"
                    target="_blank"
                    rel="noopener"
                    title="LinkedIn"
                    aria-label="LinkedIn"
                    className="mr-2"
                  >
                    {" "}
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/social/linkedin.svg"
                      alt="FIL Linkedin"
                      width="20"
                    />
                  </a>
                  {/* <a href="#" target="_blank"
                                        rel="noopener"
                                        title="Youtube"
                                        aria-label="Youtube"> <img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/social/youtube.svg" alt="FIL Youtube" width="20" /></a> */}
                </div>
              </div>
            </div>
          </div>
        </footer>
      </div>
    );
  }
}
