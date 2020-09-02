import * as React from "react";
import { ReactHTMLConverter } from "react-html-converter/browser";
import { EventLearnPageViewModel } from "../../../models/EventLearnPageViewModel";
import NearByEvent from "./NearByEvent";
import "../../../scss/_irl-learn-more.scss";
import { gets3BaseUrl } from "../../../utils/imageCdn";

interface Iprops {
  eventData: EventLearnPageViewModel;
}
const ExperienceAndHostDetailV1 = (props: any) => {
  const [isShowLess, setIsShowLess] = React.useState(false);
  let isDefaultTime = false;
  let timeModel;
  const baseUrl =
    "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/";
  const converter = ReactHTMLConverter();
  converter.registerComponent(
    "ExperienceAndHostDetailV1",
    ExperienceAndHostDetailV1
  );
  let hostDetails = [];
  hostDetails = props.eventData.eventHostMappings.map((item, index) => {
    return (
      <div>
        <img
          src={`https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/eventHost/${item.altId.toUpperCase()}.jpg`}
          alt="fap avrat"
          className="rounded-circle mb-4"
          width="120px"
          height="120px"
          onError={(e) => {
            e.currentTarget.src =
              "https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/icons/fapAvtar.png";
          }}
        />
        <h4>
          {capitalizeFirstLetter(item.firstName)}{" "}
          {capitalizeFirstLetter(item.lastName)}
        </h4>
        <p>{converter.convert(item.description)}</p>
        {index < props.eventData.eventHostMappings.length - 1 && <hr />}
      </div>
    );
  });

  const daysInWeek = [
    "All Days",
    "Monday",
    "Tuesday",
    "Wednesday",
    "Thursday",
    "Friday",
    "Saturday",
    "Sunday",
  ];
  const month_names_short = [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ];
  if (
    props.eventLearnPage.eventDetails.regularTimeModel.customTimeModel.length ==
      0 &&
    props.eventLearnPage.eventDetails.regularTimeModel.timeModel.length == 0 &&
    props.eventLearnPage.eventDetails.specialDayModel.length == 0 &&
    props.eventLearnPage.eventDetails.seasonTimeModel.length == 0
  ) {
    isDefaultTime = true;
  } else {
    if (props.eventLearnPage.eventDetails.regularTimeModel.isSameTime) {
      timeModel = props.eventLearnPage.eventDetails.regularTimeModel.daysOpen
        .map(function(item, index) {
          if (item && index > 0) {
            return (
              <div className="irl-times-sec">
                <div className="irl-days">{daysInWeek[index]}:</div>
                {
                  props.eventLearnPage.eventDetails.regularTimeModel
                    .timeModel[0].from
                }
                -
                {
                  props.eventLearnPage.eventDetails.regularTimeModel
                    .timeModel[0].to
                }{" "}
              </div>
            );
          }
        })
        .filter(function(val) {
          return val != undefined;
        });
    } else {
      timeModel =
        props.eventLearnPage.eventDetails.regularTimeModel.customTimeModel[0]
          .time.length &&
        props.eventLearnPage.eventDetails.regularTimeModel.customTimeModel.map(
          function(item, index) {
            return (
              <div className="irl-times-sec">
                <div className="irl-days">{item.day}:</div>
                {item.time[0].from}-{item.time[0].to}{" "}
              </div>
            );
          }
        );
    }
    var sesonTimings = props.eventLearnPage.eventDetails.seasonTimeModel.map(
      function(item, index) {
        if (item.isSameTime) {
          var seasonSameTimeModel = item.daysOpen
            .map(function(val, currentIndex) {
              if (val && currentIndex > 0) {
                return (
                  <div className="irl-times-sec">
                    <div className="irl-days">{daysInWeek[currentIndex]}:</div>
                    {item.sameTime[0].from}-{item.sameTime[0].to}{" "}
                  </div>
                );
              }
            })
            .filter(function(val) {
              return val != undefined;
            });
          return (
            <div className="mb-2">
              <div className="mb-2">
                {" "}
                {item.name} {new Date(item.startDate).getDate()}{" "}
                {month_names_short[new Date(item.startDate).getMonth()]} -{" "}
                {new Date(item.endDate).getDate()}{" "}
                {month_names_short[new Date(item.endDate).getMonth()]}
              </div>
              {seasonSameTimeModel}
            </div>
          );
        } else {
          var seasonCustomTime = item.time.map(function(val, currentIndex) {
            return (
              <div className="irl-times-sec">
                <div className="irl-days">{val.day}:</div>
                {val.time[0].from}-{val.time[0].to}{" "}
              </div>
            );
          });
          return (
            <div className="mb-2">
              <div className="mb-2">
                {item.name} {new Date(item.startDate).getDate()}{" "}
                {month_names_short[new Date(item.startDate).getMonth()]} -{" "}
                {new Date(item.endDate).getDate()}{" "}
                {month_names_short[new Date(item.endDate).getMonth()]}
              </div>
              {seasonCustomTime}
            </div>
          );
        }
      }
    );

    var specialDayTiming = props.eventLearnPage.eventDetails.specialDayModel.map(
      function(item, index) {
        var currentDay = new Date(item.specialDate).getDay();
        var day = daysInWeek[currentDay];
        if (currentDay == 0) {
          // if it's sunday
          day = daysInWeek[7];
        }
        return (
          <div className="mb-2">
            <div className="mb-2">
              {" "}
              {item.name} {new Date(item.specialDate).getDate()}{" "}
              {month_names_short[new Date(item.specialDate).getMonth()]}
            </div>
            <div className="irl-times-sec">
              <div className="irl-days">{day}:</div>
              {item.from}-{item.to}
            </div>
          </div>
        );
      }
    );
  }
  var timeline = props.timelineData.map(function(val) {
    return <img src={val.image} className="img-fluid" alt="" />;
  });
  var immersiveExperience = props.immersiveData.map(function(val) {
    return (
      <img
        src={val.image}
        className="img-fluid"
        alt=""
        onError={(e) => {
          e.currentTarget.src =
            "https://static6.feelitlive.com/images/places/about/in-depth/photo-coming-soon.jpg";
        }}
      />
    );
  });
  var architectureSlider = props.architectureData.map(function(val, index) {
    return (
      <li
        data-target="#Architecturaldetail"
        data-slide-to="0"
        className={index == 0 ? "active" : ""}
      />
    );
  });
  var architectureDataDetail = props.architectureData.map(function(val, index) {
    return (
      <div className={index == 0 ? "carousel-item active" : "carousel-item"}>
        <div className="row">
          <div className="popup-text col-md-6">
            {converter.convert(val.description)}
          </div>
          <div className="col-md-6">
            <img
              className="card-img-top"
              src={val.image}
              alt="Card image cap"
              onError={(e) => {
                e.currentTarget.src = `${gets3BaseUrl()}/places/about/in-depth/photo-coming-soon.jpg`;
              }}
            />
          </div>
        </div>
      </div>
    );
  });
  var highlightsSlider = props.highlightsData.map(function(val, index) {
    return (
      <li
        data-target="#carouselHighlightNuggets"
        data-slide-to="0"
        className={index == 0 ? "active" : ""}
      />
    );
  });
  var highlightsDataDetail = props.highlightsData.map(function(val, index) {
    return (
      <div className={index == 0 ? "carousel-item active" : "carousel-item"}>
        <div className="row">
          <div className="popup-text col-md-6">
            {converter.convert(val.description)}
          </div>
          <div className="col-md-6">
            <img
              className="card-img-top"
              src={val.image}
              alt="Card image cap"
              onError={(e) => {
                e.currentTarget.src = `${gets3BaseUrl()}/places/about/in-depth/photo-coming-soon.jpg`;
              }}
            />
          </div>
        </div>
      </div>
    );
  });
  var placeMapPlan = props.mapPlanData.map(function(val) {
    return <img className="card-img-top img-fluid" src={val.image} alt="" />;
  });
  return (
    <>
      <section className="exp-sec-pad">
        <div className="container">
          <div className="card-deck">
            <div className="card">
              <h3 className="text-purple">About this experience</h3>
            </div>
            <div className="card">
              <div
                className={!isShowLess ? "page-content-text lean-more-txt" : ""}
              >
                <p>{converter.convert(props.eventData.event.description)}</p>
                <div className="ShowLink">
                  <a
                    href="JavaScript:Void(0)"
                    onClick={() => setIsShowLess(!isShowLess)}
                    className="stretched-link"
                  >
                    {!isShowLess ? "Show More" : "Show Less"}
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
      <section className="exp-sec-pad">
        <div className="container">
          <div className="card-deck">
            <div className="card">
              <h3 className="text-purple">Location and opening hours</h3>
            </div>
            <div className="card overflow-inherit">
              <div className="content-map">
                <iframe
                  src={`https://www.google.com/maps/embed/v1/place?key=AIzaSyCc3zoz5TZaG3w2oF7IeR-fhxNXi8uywNk&q=${
                    props.eventData.event.name
                  }`}
                  width="100%"
                  height="310"
                  frameBorder="0"
                  className="iframes shadow"
                  allowFullScreen
                />
              </div>
              <div className="media pt-3 mt-5">
                <img
                  src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/map-pin.svg"
                  className="mr-3"
                  alt="fil address"
                />
                <div className="media-body">
                  <h5 className="mt-0">Address</h5>
                  {props.eventData.venue.addressLineTwo
                    ? props.eventData.venue.addressLineTwo
                    : props.eventData.venue.addressLineOne}
                </div>
              </div>
              {!isDefaultTime && timeModel.length > 0 && (
                <div className="media mt-5">
                  <img
                    src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/clock.svg"
                    className="mr-3"
                    alt="fil opening hours"
                  />
                  <div className="media-body">
                    <h5 className="mt-0">Opening hours</h5>
                    {timeModel}
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </section>
      {props.eventData.eventAmenitiesList.length > 0 ? (
        <section className="exp-sec-pad">
          <div className="container">
            <div className="card-deck">
              <div className="card">
                <h3 className="text-purple">Amenities / Inclusions</h3>
              </div>
              <div className="card">
                {props.eventData.eventAmenitiesList.length > 0 &&
                  props.eventData.eventAmenitiesList.map((item) => (
                    <div className="media">
                      <img
                        src={`${baseUrl}${item.toLowerCase()}.svg`}
                        alt={`${item} logo`}
                        className="mr-3"
                      />
                      <div className="media-body">{item}</div>
                    </div>
                  ))}
              </div>
            </div>
          </div>
        </section>
      ) : null}
      {props.eventData.eventLearnMoreAttributes.length > 0 ? (
        <section className="exp-sec-pad">
          <div className="container">
            <div className="card-deck">
              <div className="card">
                <h3 className="text-purple">Other information</h3>
              </div>
              <div className="card">
                <div className="row">
                  {props.historyData.length > 0 ? (
                    <div className="col-sm-6 media mb-5">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/history.svg"
                        className="mr-3"
                        alt="..."
                      />
                      <div className="media-body">
                        <a
                          href="#"
                          data-toggle="modal"
                          data-target="#popup-history"
                          className="text-body"
                        >
                          History
                        </a>
                      </div>
                    </div>
                  ) : null}
                  {props.timelineData.length > 0 ? (
                    <div className="col-sm-6 media mb-5">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/timeline.svg"
                        className="mr-3"
                        alt="..."
                      />
                      <div className="media-body">
                        <a
                          href="#"
                          data-toggle="modal"
                          data-target="#popup-timeline"
                          className="text-body"
                        >
                          Timeline
                        </a>
                      </div>
                    </div>
                  ) : null}
                  {props.highlightsData.length > 0 ? (
                    <div className="col-sm-6 media mb-5">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/highlights-nuggets.svg"
                        className="mr-3"
                        alt="..."
                      />
                      <div className="media-body">
                        <a
                          href="#"
                          data-toggle="modal"
                          data-target="#HighlightsNuggets"
                          className="text-body"
                        >
                          Highlights/Nuggets
                        </a>
                      </div>
                    </div>
                  ) : null}
                  {props.immersiveData.length > 0 ? (
                    <div className="col-sm-6 media mb-5">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/immersive-experience.svg"
                        className="mr-3"
                        alt="..."
                      />
                      <div className="media-body">
                        <a
                          href="#"
                          data-toggle="modal"
                          data-target="#popup-immersiveExperience"
                          className="text-body"
                        >
                          Immersive experience
                        </a>
                      </div>
                    </div>
                  ) : null}
                  {props.mapPlanData.length > 0 ? (
                    <div className="col-sm-6 media mb-3">
                      <img
                        src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/place-map-plan.svg"
                        className="mr-3"
                        alt="..."
                      />
                      <div className="media-body">
                        <a
                          href="#"
                          data-toggle="modal"
                          data-target="#popup-mapplan"
                          className="text-body"
                        >
                          Place map/plan
                        </a>
                      </div>
                    </div>
                  ) : null}
                  <div className="col-sm-6 media mb-3">
                    <img
                      src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/icon/irl/architectural-detail.svg"
                      className="mr-3"
                      alt="..."
                    />
                    <div className="media-body">
                      <a
                        href="#"
                        data-toggle="modal"
                        data-target="#popup-Architecturaldetail"
                        className="text-body"
                      >
                        Architectural detail
                      </a>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </section>
      ) : null}
      <section className="exp-sec-pad learn-more-page">
        <div className="container near-by">
          <h3 className="text-purple">Nearby</h3>
          <hr />
          <NearByEvent
            venue={props.eventData.venue}
            eventInfo={props.eventData.event}
            city={props.eventData.city.name}
            country={props.eventData.country.name}
            getDataByCategory={props.getDataByCategory}
            feelUserJourney={props.feelUserJourney}
            {...props}
          />
        </div>
      </section>
      <div
        className="modal fade bd-example-modal-lg popup-history"
        id="popup-history"
        role="dialog"
        aria-labelledby="popup-history"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-lg modal-dialog-centered">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="exampleModalCenterTitle">
                <img
                  src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`}
                  width="20"
                  className="mr-2"
                  alt="feelaplace logo"
                />{" "}
                History
              </h5>
              <button
                type="button"
                className="close"
                data-dismiss="modal"
                aria-label="Close"
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              {props.historyData.length > 0
                ? converter.convert(props.historyData[0].description)
                : ""}
            </div>
          </div>
        </div>
      </div>
      <div
        className="modal fade bd-example-modal-lg popup-timeline p-0"
        id="popup-timeline"
        role="dialog"
        aria-labelledby="popup-timeline"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-dialog-full modal-dialog-centered">
          <button
            type="button"
            className="close text-white position-absolute"
            data-dismiss="modal"
            aria-label="Close"
          >
            <span aria-hidden="true" className="h1">
              &times;
            </span>
          </button>
          {timeline}
        </div>
      </div>
      <div
        className="modal fade bd-example-modal-lg popup-with-slide"
        id="popup-Architecturaldetail"
        role="dialog"
        aria-labelledby="popup-Architecturaldetail"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-lg modal-dialog-centered">
          <div className="modal-content ">
            <div className="modal-header">
              <h5 className="modal-title" id="exampleModalCenterTitle">
                <img
                  src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`}
                  width="20"
                  className="mr-2"
                  alt="feelaplace logo"
                />{" "}
                Architectural detail
              </h5>
              <button
                type="button"
                className="close"
                data-dismiss="modal"
                aria-label="Close"
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              <div className="custome-slider">
                <div
                  id="Architecturaldetail"
                  className="carousel slide"
                  data-ride="carousel"
                >
                  <ol className="carousel-indicators">{architectureSlider}</ol>
                  <div className="carousel-inner">{architectureDataDetail}</div>
                  <a
                    className="slick-arrow slick-prev"
                    href="#Architecturaldetail"
                    role="button"
                    data-slide="prev"
                  />
                  <a
                    className="slick-arrow slick-next"
                    href="#Architecturaldetail"
                    role="button"
                    data-slide="next"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        className="modal fade bd-example-modal-lg popup-with-slide"
        id="HighlightsNuggets"
        role="dialog"
        aria-labelledby="HighlightsNuggets"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-lg modal-dialog-centered">
          <div className="modal-content ">
            <div className="modal-header">
              <h5 className="modal-title" id="exampleModalCenterTitle">
                <img
                  src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`}
                  width="20"
                  className="mr-2"
                  alt="feelaplace logo"
                />{" "}
                Highlights/Nuggets
              </h5>
              <button
                type="button"
                className="close"
                data-dismiss="modal"
                aria-label="Close"
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              <div className="custome-slider">
                <div
                  id="carouselHighlightNuggets"
                  className="carousel slide"
                  data-ride="carousel"
                >
                  <ol className="carousel-indicators">{highlightsSlider}</ol>
                  <div className="carousel-inner">{highlightsDataDetail}</div>
                  <a
                    className="slick-arrow slick-prev"
                    href="#carouselHighlightNuggets"
                    role="button"
                    data-slide="prev"
                  />
                  <a
                    className="slick-arrow slick-next"
                    href="#carouselHighlightNuggets"
                    role="button"
                    data-slide="next"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        className="modal fade bd-example-modal-lg popup-mapplan p-0"
        id="popup-mapplan"
        role="dialog"
        aria-labelledby="popup-mapplan"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-dialog-full modal-dialog-centered">
          <button
            type="button"
            className="close text-white position-absolute"
            data-dismiss="modal"
            aria-label="Close"
          >
            <span aria-hidden="true" className="h1">
              &times;
            </span>
          </button>
          {placeMapPlan}
        </div>
      </div>
      <div
        className="modal fade bd-example-modal-lg popup-with-slide"
        id="popup-Architecturaldetail"
        role="dialog"
        aria-labelledby="popup-Architecturaldetail"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-lg modal-dialog-centered">
          <div className="modal-content ">
            <div className="modal-header">
              <h5 className="modal-title" id="exampleModalCenterTitle">
                <img
                  src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`}
                  width="20"
                  className="mr-2"
                  alt="feelaplace logo"
                />{" "}
                Architectural detail
              </h5>
              <button
                type="button"
                className="close"
                data-dismiss="modal"
                aria-label="Close"
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              <div className="custome-slider">
                <div
                  id="Architecturaldetail"
                  className="carousel slide"
                  data-ride="carousel"
                >
                  <ol className="carousel-indicators">{architectureSlider}</ol>
                  <div className="carousel-inner">{architectureDataDetail}</div>
                  <a
                    className="slick-arrow slick-prev"
                    href="#Architecturaldetail"
                    role="button"
                    data-slide="prev"
                  />
                  <a
                    className="slick-arrow slick-next"
                    href="#Architecturaldetail"
                    role="button"
                    data-slide="next"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div
        className="modal fade bd-example-modal-lg popup-with-slide"
        id="popup-immersiveExperience"
        role="dialog"
        aria-labelledby="popup-immersiveExperience"
        aria-hidden="true"
      >
        <div className="modal-dialog modal-lg modal-dialog-centered">
          <div className="modal-content ">
            <div className="modal-header">
              <h5 className="modal-title" id="exampleModalCenterTitle">
                <img
                  src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`}
                  width="20"
                  className="mr-2"
                  alt="feelaplace logo"
                />{" "}
                Immersive experience
              </h5>
              <button
                type="button"
                className="close"
                data-dismiss="modal"
                aria-label="Close"
              >
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div className="modal-body">
              <div className="custome-slider">
                <div
                  id="immersiveExperience"
                  className="carousel slide"
                  data-ride="carousel"
                >
                  {immersiveExperience}
                  <a
                    className="slick-arrow slick-prev"
                    href="#immersiveExperience"
                    role="button"
                    data-slide="prev"
                  />
                  <a
                    className="slick-arrow slick-next"
                    href="#immersiveExperience"
                    role="button"
                    data-slide="next"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
function capitalizeFirstLetter(string) {
  return string.charAt(0).toUpperCase() + string.slice(1);
}
export default ExperienceAndHostDetailV1;
