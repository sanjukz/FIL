import * as React from "React";
import * as numeral from "numeral";
import axios from "axios";
import { formatAddress } from "../../../utils/addressFormatter";
import { gets3BaseUrl } from "../../../utils/imageCdn";

export default class Checklist extends React.Component<any, any> {
  state = {
    weather: {
      mintemp: 0,
      maxtemp: 0,
      link: ""
    }
  };

  public weatherapikey = "";
  UNSAFE_componentWillMount() {
    this.getWeatherConfig();
  }
  public parseDateLocal(s) {
    var b = s.split(/\D/);
    return new Date(
      b[0],
      b[1] - 1,
      b[2],
      b[3] || 0,
      b[4] || 0,
      b[5] || 0,
      b[6] || 0
    );
  }

  async GetLocationKey() {
    const url =
      "https://dataservice.accuweather.com/locations/v1/cities/search?q=" +
      this.props.cityName +
      "&apikey=" +
      this.weatherapikey;
    const api_call = await fetch(url, { mode: "cors" });
    const response = api_call.json();
    response.then(data => {
      var flag = 0;
      for (let i = 0; i < data.length; i++) {
        if (data[i].Country.EnglishName === this.props.countryName) {
          this.GetWeather(data[i].Key);
          flag = 1;
          break;
        }
      }
      if (flag === 0 && data.length > 0) {
        this.GetWeather(data[0].Key);
      }
    });
  }

  async GetWeather(key) {
    let url =
      "https://dataservice.accuweather.com/forecasts/v1/daily/1day/" +
      key +
      "?apikey=" +
      this.weatherapikey;
    const api_call = await fetch(url);

    const response = api_call.json();
    const data = response.then(data => {
      this.state.weather.maxtemp =
        data.DailyForecasts[0].Temperature.Maximum.Value;
      this.state.weather.mintemp =
        data.DailyForecasts[0].Temperature.Minimum.Value;
      this.state.weather.link = data.DailyForecasts[0].Link;
    });
  }
  private getWeatherConfig() {
    return axios
      .get("api/weather/getconfig", {
        headers: {
          "Content-Type": "application/json"
        }
      })
      .then(response => {
        this.weatherapikey = response.data;
        this.GetLocationKey();
      })
      .catch(error => { });
  }
  checkStartEndTime = (startTime, endTime) => {
    return (
      new Date(startTime).getHours() + new Date(startTime).getMinutes() ==
      new Date(endTime).getHours() + new Date(endTime).getMinutes()
    );
  };
  public render() {
    var isDefaultTime = false;
    var that = this;
    var timeModel;
    const daysInWeek = [
      "All Days",
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday",
      "Sunday"
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
      "Dec"
    ];
    if (
      this.props.allData.regularTimeModel.customTimeModel.length == 0 &&
      this.props.allData.regularTimeModel.timeModel.length == 0 &&
      this.props.allData.specialDayModel.length == 0 &&
      this.props.allData.seasonTimeModel.length == 0
    ) {
      isDefaultTime = true;
    } else {
      if (this.props.allData.regularTimeModel.isSameTime) {
        timeModel = this.props.allData.regularTimeModel.daysOpen
          .map(function (item, index) {
            if (item && index > 0) {
              return (
                <p className="mb-2">
                  <span>
                    <i className="fa fa-clock-o" aria-hidden="true" />
                  </span>
                  {daysInWeek[index]}:
                  {that.props.allData.regularTimeModel.timeModel[0].from}-
                  {that.props.allData.regularTimeModel.timeModel[0].to}{" "}
                </p>
              );
            }
          })
          .filter(function (val) {
            return val != undefined;
          });
      } else {
        timeModel =
          this.props.allData.regularTimeModel.customTimeModel[0].time.length &&
          this.props.allData.regularTimeModel.customTimeModel.map(function (
            item,
            index
          ) {
            return (
              <p className="mb-2">
                <span>
                  <i className="fa fa-clock-o" aria-hidden="true" />
                </span>
                {item.day}:{item.time[0].from}-{item.time[0].to}{" "}
              </p>
            );
          });
      }
      var sesonTimings = this.props.allData.seasonTimeModel.map(function (
        item,
        index
      ) {
        if (item.isSameTime) {
          var seasonSameTimeModel = item.daysOpen
            .map(function (val, currentIndex) {
              if (val && currentIndex > 0) {
                return (
                  <p className="mb-2">
                    <span>
                      <i className="fa fa-clock-o" aria-hidden="true" />
                    </span>
                    {daysInWeek[currentIndex]}:{item.sameTime[0].from}-
                    {item.sameTime[0].to}{" "}
                  </p>
                );
              }
            })
            .filter(function (val) {
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
          var seasonCustomTime = item.time.map(function (val, currentIndex) {
            return (
              <p className="mb-2">
                <span>
                  <i className="fa fa-clock-o" aria-hidden="true" />
                </span>
                {val.day}:{val.time[0].from}-{val.time[0].to}{" "}
              </p>
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
      });

      var specialDayTiming = this.props.allData.specialDayModel.map(function (
        item,
        index
      ) {
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
            <p className="mb-2">
              <span>
                <i className="fa fa-clock-o" aria-hidden="true" />
              </span>
              {day}:{item.from}-{item.to}{" "}
            </p>
          </div>
        );
      });
    }
    return (
      <div className="col-sm-4 more-checklist">
        <h5 className="mb-4">
          <img
            src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`}
            className="mr-1"
            width="20"
            alt="feelitLIVE logo"
          />
          Checklist
        </h5>
        {this.props.allData.category.id != 98 &&
          <p className="mb-2">
            <span>
              <i
                className="fa fa-map-marker"
                aria-hidden="true"
                style={{ fontSize: "19px" }}
              />
            </span>
            <span className="w-75 text-left align-text-top">
              {formatAddress(
                `${
                this.props.addressLineOne !== ""
                  ? this.props.addressLineOne
                  : ""
                }, ${
                this.props.cityName !== "" ? this.props.cityName + ", " : ""
                }${
                this.props.stateName !== "" ? this.props.stateName + ", " : ""
                }${this.props.countryName !== "" ? this.props.countryName : ""}`
              )}
            </span>
          </p>
        }
        {isDefaultTime && this.props.allData.event.eventCategoryId !== 17 && (
          <p className="mb-2">
            <span>
              <i className="fa fa-clock-o" aria-hidden="true" />
            </span>
            {this.checkStartEndTime(
              this.props.eventDetails.startDateTime,
              this.props.eventDetails.endDateTime
            )
              ? numeral(
                this.parseDateLocal(
                  this.props.eventDetails.startDateTime
                ).getHours()
              ).format("00") +
              ":" +
              numeral(
                this.parseDateLocal(
                  this.props.eventDetails.startDateTime
                ).getMinutes()
              ).format("00")
              : `${numeral(
                this.parseDateLocal(
                  this.props.eventDetails.startDateTime
                ).getHours()
              ).format("00") +
              ":" +
              numeral(
                this.parseDateLocal(
                  this.props.eventDetails.startDateTime
                ).getMinutes()
              ).format("00")}
                    -${numeral(
                this.parseDateLocal(
                  this.props.eventDetails.endDateTime
                ).getHours()
              ).format("00") +
              ":" +
              numeral(
                this.parseDateLocal(
                  this.props.eventDetails.endDateTime
                ).getMinutes()
              ).format("00")}`}
          </p>
        )}

        {!isDefaultTime && timeModel.length > 0 && (
          <div>
            {" "}
            <b className="mb-2">Regular Timing</b> {timeModel}
          </div>
        )}
        {sesonTimings != undefined && sesonTimings.length > 0 && (
          <div>
            {" "}
            <b className="mb-2">Season Timing</b> {sesonTimings}
          </div>
        )}
        {specialDayTiming != undefined && specialDayTiming.length > 0 && (
          <div>
            {" "}
            <b className="mb-2">Holiday/ Special Day Timing</b>{" "}
            {specialDayTiming}
          </div>
        )}
        <p className="mb-2">
          <span>
            <i className="fa fa-map-o"></i>
          </span>{" "}
          <a href="javascript:void(0)">How to get there</a>
        </p>
        <p className="mb-2">
          <span>
            <i className="fa fa-cloud" style={{ fontSize: "12px" }}></i>
          </span>{" "}
          Min:{this.state.weather.mintemp.toFixed(0)}&#8457; Max:
          {this.state.weather.maxtemp.toFixed(0)}&#8457;{" "}
        </p>
        <p>
          <span>
            <i className="fa fa-briefcase"></i>
          </span>{" "}
          <a href="javascript:void(0)">What to pack</a>{" "}
        </p>
      </div>
    );
  }
}
