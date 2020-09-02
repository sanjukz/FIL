import * as React from "react";
import { ReactHTMLConverter } from "react-html-converter/browser";
import ReactStars from "react-stars";
import { formatDate } from "../../utils/ShuffleArray";
import { autobind } from "core-decorators";
import { getWeather } from '../../services/weather';
import { gets3BaseUrl } from "../../utils/imageCdn";
import * as parse from "url-parse";

export default class CountryDetailBlock extends React.Component<any, any> {
    public weatherapikey = '';
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
    componentDidMount() {
        const data: any = parse(location.search, location, true);
        const placeName = data.query.state ? this.props.dynamicSections.allPlaceTiles.placeDetails[0].cityName : this.props.sectionDetails.sectionTitle;
        getWeather(placeName, (data) => {
            this.setState({
                weather: {
                    maxtemp: data.DailyForecasts[0].Temperature.Maximum.Value,
                    mintemp: data.DailyForecasts[0].Temperature.Minimum.Value,
                    link: data.DailyForecasts[0].Link
                }
            })
        });
    }

  @autobind
  showLessClick(val) {
    this.setState({ isShowLess: !this.state.isShowLess });
  }
  render() {
    const converter = ReactHTMLConverter();
    converter.registerComponent("CountryDetailBlock", CountryDetailBlock);
    return (
      <div className="p-0 container page-content learn-more-page">
        <div className="row learn-head pb-3">
          <div className="col-xl-7 col-sm-6">
            <h1 className="mb-2 h3">
              {this.props.sectionDetails.sectionTitle}
            </h1>
            <div className="rating">
              <span>
                Ratings:
                <ReactStars
                  className="rating d-inline-block"
                  color2={"#572483"}
                  edit={false}
                  value={0}
                  count={5}
                />
              </span>
              <span className="review-text">0 Reviews</span>
            </div>
          </div>
          <div className="col-xl-5 col-sm-6 right-head text-sm-right">
            <div className="d-inline-block align-middle">
              <h5 className="text-left pr-3 m-0">
                <img
                  src={`${gets3BaseUrl()}/header/cart-icon-blank-v1.png`}
                  alt="Feel Cart Icon"
                  className="mr-2 mb-1"
                  width="23"
                />
                <small>What to pack</small>
              </h5>
            </div>
            <div
              className="d-inline-block align-middle"
              onClick={this.props.ScrollToFeels}
            >
              <label className="btn site-primery-btn text-uppercase mb-0">
                {`${this.props.sectionDetails.count}+ feels`}
              </label>
            </div>
          </div>
        </div>
        <div className="row sec-spacing">
          <div
            className={
              !this.state.isShowLess
                ? "col-xl-8 col-lg-7 col-sm-12"
                : "col-xl-8 col-lg-7 col-sm-12 page-content-text"
            }
          >
            {this.props.sectionDetails.description &&
              this.props.sectionDetails.description != "" && (
                <div>
                  <p>
                    {converter.convert(this.props.sectionDetails.description)}
                  </p>{" "}
                  <div className="ShowLink">
                    <a
                      href="JavaScript:Void(0)"
                      onClick={this.showLessClick}
                      className="stretched-link"
                    >
                      {this.state.isShowLess ? "Show More" : "Show Less"}
                    </a>
                  </div>
                </div>
              )}
          </div>
          <div className="col-xl-4 col-lg-5 col-sm-12">
            <div className="border mb-3 text-center p-3">
              <div className="d-flex justify-content-center">
                <div style={{ width: "40%" }}>
                  <img
                    style={{ maxWidth: "50%" }}
                    src={`${gets3BaseUrl()}/icons/weather-icon.png`}
                  />
                </div>
                <div style={{ width: "60%" }}>
                  <p className="mb-2 d-flex justify-content-around">
                    <span style={{ fontSize: "1.8rem", fontWeight: "bold" }}>
                      {this.state.weather.mintemp.toFixed(0)}&#8457;
                    </span>
                    <span style={{ fontSize: "1.8rem", fontWeight: "bold" }}>
                      {this.state.weather.maxtemp.toFixed(0)}&#8457;
                    </span>
                  </p>
                  <p>{formatDate(new Date())}</p>
                </div>
              </div>
            </div>
            <div className="content-map">
              <iframe
                src={`https://www.google.com/maps/embed/v1/place?key=AIzaSyCc3zoz5TZaG3w2oF7IeR-fhxNXi8uywNk&q=${this.props.sectionDetails.sectionTitle}`}
                width="100%"
                height="310"
                frameBorder="0"
                className="iframes"
                allowFullScreen
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}
