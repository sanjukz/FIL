import * as React from "react";
import { Link, RouteComponentProps, Route } from "react-router-dom";
import { ProgressBar } from "react-bootstrap";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import Select from "react-select";
import * as CategoryStore from "../stores/Category";
import * as ItineraryStore from "../stores/Itinerary";
import { autobind } from "core-decorators";
import * as Datetime from "react-datetime";
import FilLoader from "../components/Loader/FilLoader";
import ItineraryResult from "./ItineraryResult";
import ItineraryRequestViewModel from "../models/Itinerary/ItineraryRequestViewModel";
import ItineraryBoardInputViewModel from "../models/Itinerary/ItineraryBoardInputViewModel";
import ItineraryBoardResponseViewModel from "../models/Itinerary/ItineraryBoardResponseViewModel";
import { gets3BaseUrl } from "../utils/imageCdn";
import "./Itinerary.scss";
import "./DatePicker.css";
import * as algoliasearch from "algoliasearch";

const searchClient = algoliasearch(
  // intializing alogolia client
  "8AMLTQYRXE", //ToDo Set this keys in environmental variable
  "122a9a1f411959c3cd46db326c56334a"
);

const index = searchClient.initIndex("Places");
type ItineraryComponentProps = CategoryStore.ICategoryProps &
  ItineraryStore.ICategoryProps &
  typeof CategoryStore.actionCreators &
  typeof ItineraryStore.actionCreators &
  RouteComponentProps<{}>;

var flag = true,
  eventCategory = [];
var errflag = false,
  responseFlg = false,
  formFlg = false,
  selectedCountries = [];

enum FeelType {
  Popular = 1,
  Fast,
  Slow
}
const formatOptionLabel = ({ value, label, countryName }) => (
  <div style={{ display: "flex" }}>
    <div>
      <img
        src={`${gets3BaseUrl()}/flags/flag-${
          countryName.indexOf(".") != -1
            ? countryName
                .split(".")
                .join("")
                .toUpperCase()
            : countryName
                .split(" ")
                .join("-")
                .toLowerCase()
        }.jpg`}
        alt="feel country flag"
        width="30"
        onError={e => {
          e.currentTarget.src = `${gets3BaseUrl()}/places/tiles/tiles-placeholder.jpg`;
        }}
        className="mr-2"
      />
      {label}
    </div>
  </div>
);
class ItineraryInputForm extends React.Component<ItineraryComponentProps, any> {
  public constructor(props) {
    super(props);
    this.state = {
      destination: "",
      destinationPlaces: null,
      valFromDate: "",
      valToDate: "",
      fromDate: null,
      toDate: null,
      adultvalue: 1,
      childQuantity: 0,
      kidsValue: 0,
      inputValue: "",
      eventCategory: "",
      selectedCategory: [],
      selectedCategoryName: [],
      budgetRange: "2",
      isBudgetRangeClick: false,
      traveller: 1,
      feelType: FeelType.Popular,
      dropDownOpen: false,
      isShowForm: true,
      destinationValue: "",
      isShowInputForm: true,
      anivationValue: 50,
      cityOptions: []
    };
  }

  componentDidMount() {
    this.props.requestMasterBudgetData();
    if (window.location.pathname.indexOf("result") > -1) {
      this.props.history.push("/feel-itineraryplanner");
    }
    if (
      sessionStorage.getItem("itineraryForm") != null &&
      sessionStorage.getItem("itineraryForm") != undefined
    ) {
      sessionStorage.removeItem("itineraryForm");
    }
  }

  handleonChangeStartDate = e => {
    if (
      this.state.toDate != null &&
      new Date(e) > new Date(this.state.toDate)
    ) {
      this.setState({ toDate: null });
    }
    this.setState({ fromDate: e });
  };

  handleonChangeEndDate = e => {
    this.setState({ toDate: e });
  };

  increaseAdultQuantity(e) {
    this.setState({
      adultvalue: this.state.adultvalue + 1,
      traveller: this.state.traveller + 1
    });
  }
  decreaseAdultQuantity(e) {
    if (this.state.adultvalue > 1) {
      this.setState({
        adultvalue: this.state.adultvalue - 1,
        traveller: this.state.traveller - 1
      });
    }
  }
  increaseChildQuantity(e) {
    this.setState({
      childQuantity: this.state.childQuantity + 1,
      traveller: this.state.traveller + 1
    });
  }
  decreaseChildQuantity(e) {
    if (this.state.childQuantity > 0) {
      this.setState({
        childQuantity: this.state.childQuantity - 1,
        traveller: this.state.traveller - 1
      });
    }
  }

  @autobind
  handleSelectChange(val) {
    this.setState({
      destination: val.label
    });
  }
  @autobind
  handleClick(val, e) {
    var eventCategory = this.state.selectedCategory;
    var eventCategoryName = this.state.selectedCategoryName;
    var isExists = this.state.selectedCategory.includes(val.eventCategory);
    if (isExists) {
      var index = eventCategory.indexOf(val.eventCategory);
      var indexName = eventCategoryName.indexOf(val.displayName);
      eventCategory.splice(index, 1);
      eventCategoryName.splice(indexName, 1);
    } else {
      eventCategory.push(val.eventCategory);
      eventCategoryName.push(val.displayName);
    }
    this.setState({
      selectedCategory: eventCategory,
      selectedCategoryName: eventCategoryName
    });
  }

  @autobind
  onChangeDates(selectedDates) {
    if (selectedDates != null) {
      this.setState({
        destination: selectedDates.label,
        destinationValue: selectedDates,
        destinationPlaces: selectedDates
      });
    } else {
      this.setState({ destination: "" });
    }
  }

  @autobind
  onBudgetRangeClick(budget) {
    this.setState({ budgetRange: budget, isBudgetRangeClick: true });
  }

  @autobind
  onClickFeelType(feelType) {
    this.setState({ feelType: feelType });
  }
  dropDownButton(e) {
    this.setState({
      dropDownOpen: true
    });
  }

  @autobind
  isShowForm() {
    this.setState({
      isShowForm: true,
      isShowInputForm: true,
      anivationValue: 50
    });
  }

  @autobind
  requestItineraryBoard(itineraryBoardInout: ItineraryBoardInputViewModel) {
    this.props.requestItinerarayBoardData(
      itineraryBoardInout,
      (response: ItineraryBoardResponseViewModel) => {}
    );
  }

  @autobind
  getInputForm(e) {
    this.setState({ isShowInputForm: true, anivationValue: 50 });
  }

  @autobind
  requestCategoryData(e) {
    if (
      this.state.toDate == null ||
      this.state.fromDate == null ||
      this.state.destinationPlaces == null ||
      this.state.destinationPlaces.length == 0
    ) {
      alert(
        "Oops! Looks like you forgot something. Please double check that you've filled in all the required boxes so that you can go forward."
      );
      return false;
    } else {
      let data = JSON.parse(sessionStorage.getItem("itineraryForm"));
      if (data !== undefined && data !== null) {
        this.setState({
          isShowInputForm: false,
          anivationValue: 100,
          selectedCategoryName: data.selectedCategoryName,
          selectedCategory: data.selectedCategory
        });
      } else {
        this.setState({
          isShowInputForm: false,
          selectedCategoryName: [],
          selectedCategory: [],
          anivationValue: 100
        });
      }
      let cityIds = "";
      for (var i = 0; i < this.state.destinationPlaces.length; i++) {
        cityIds = cityIds.concat(this.state.destinationPlaces[i].value + ",");
      }
      cityIds = cityIds.replace(/(^,)|(,$)/g, "");
      this.props.requestCategoryByLocation(cityIds);
    }
  }

  getYourItinerary(e) {
    if (
      this.state.selectedCategoryName.length == 0 ||
      this.state.toDate == null ||
      this.state.fromDate == null ||
      this.state.destinationPlaces == null ||
      this.state.destinationPlaces.length == 0
    ) {
      alert(
        "Oops! Looks like you forgot something. Please double check that you've filled in all the required boxes so that you can go forward."
      );
      return false;
    } else if (
      new Date(this.state.fromDate) > new Date(this.state.toDate) ||
      new Date(this.state.fromDate) < new Date(new Date().setHours(0, 0, 0, 0))
    ) {
      alert(
        "Oops! Looks like you forgot something. Please check if you've entered correct start date."
      );
      return false;
    } else {
      let flg = true;
      var fromDate = this.state.fromDate._d
        ? this.state.fromDate._d
        : new Date(this.state.fromDate);
      var toDate = this.state.toDate._d
        ? this.state.toDate._d
        : this.state.toDate;
      let diff = new Date(toDate).getTime() - new Date(fromDate).getTime();
      if (flg) {
        var d = this.state.destinationPlaces;
        let cityIds = [];
        var destinationPlaces = "";
        for (var i = 0; i < this.state.destinationPlaces.length; i++) {
          cityIds.push(this.state.destinationPlaces[i].value);
          let place = this.state.destinationPlaces[i].label.split(", ");
          destinationPlaces = destinationPlaces + place[0];
          if (i < this.state.destinationPlaces.length - 1) {
            destinationPlaces = destinationPlaces + ",";
          }
        }
        var categoryNames = "";
        selectedCountries = destinationPlaces.split(",");
        for (var i = 0; i < this.state.selectedCategoryName.length; i++) {
          categoryNames =
            categoryNames + this.state.selectedCategoryName[i] + ",";
        }
        (errflag = true), (responseFlg = true), (formFlg = false);
        categoryNames = categoryNames.trim().replace(/(^,)|(,$)/g, "");
        this.setState({ isShowForm: false });
        let itineraryRequest: ItineraryRequestViewModel = {
          queryString: destinationPlaces.trim(),
          startDate: new Date(this.state.fromDate).toDateString(),
          endDate: new Date(this.state.toDate).toDateString(),
          speed: (this.state.feelType - 1).toString(),
          categories: categoryNames,
          budgetRange: this.state.budgetRange,
          noOfAdults: this.state.adultvalue,
          noOfChilds: this.state.childQuantity,
          cityIds: cityIds
        };
        sessionStorage.setItem(
          "itineraryForm",
          JSON.stringify({
            ...itineraryRequest,
            selectedCategoryName: this.state.selectedCategoryName,
            selectedCategory: this.state.selectedCategory
          })
        );
        this.props.requestItinerarayResponseData(itineraryRequest);
      }
    }
    this.setState({ isShowInputForm: false });
  }
  @autobind
  handleInputChange(val) {
    if (val != "" && val != null) {
      var that = this;
      index.search({ query: val, hitsPerPage: 10 }).then(({ hits }) => {
        var a = 10;
        that.setState({ cityOptions: hits });
      });
    } else {
      this.setState({ cityOptions: [] });
    }
  }
  @autobind
  GetFilteredCities(cityOptions) {
    let filteredCities = [];
    if (cityOptions && cityOptions.length > 0) {
      let distintCities = [];
      cityOptions.map(item => {
        if (distintCities.indexOf(item.city) == -1) {
          distintCities.push(item.city);
        }
      });
      if (distintCities.length > 0) {
        distintCities.map(item => {
          let cityId = cityOptions.filter(val => {
            return val.city == item;
          });
          if (cityId && cityId.length > 0) {
            let tempData = {
              cityName: cityId[0].city,
              country: cityId[0].country,
              cityId: cityId[0].cityId,
              countryId: cityId[0].countryId
            };
            filteredCities.push(tempData);
          }
        });
      }
    }
    filteredCities.filter(item => {
      return item.cityName != "" && item.country != "";
    });
    return filteredCities;
  }

  public render() {
    var options = [],
      cityOptions = [];
    if (this.props.itinerary.isError && errflag) {
      alert(
        "Something went wrong for selected city please try again with another city"
      );
      errflag = false;
    }
    if (this.props.itinerary.itineraryResult != undefined) {
      if (
        this.props.itinerary.itineraryResult.length == 0 &&
        responseFlg &&
        this.props.itinerary.fetchItinerarySuccess
      ) {
        alert(
          "Sorry! we are unable to find the results for selected inputs, can you please try again"
        );
        responseFlg = false;
        formFlg = true;
      }
    }
    var that = this;
    var from = new Date(new Date().setDate(new Date().getDate() - 1));
    var to = new Date(new Date().setDate(new Date().getDate() - 1));
    if (this.state.fromDate != null) {
      to = new Date(
        new Date().setDate(new Date(this.state.fromDate).getDate() - 1)
      );
    }
    var fromValid = (currentDate, selectedDate) => {
      return currentDate.isAfter(from);
    };

    var toValid = current => {
      return current.isAfter(to);
    };

    const cats = this.props.category.categories.map((item, index) => {
      var img;
      var classname, clsname;
      var isParestExists = false;
      img = `${gets3BaseUrl()}/category-tab-icon/` + item.slug + `.svg`;
      var subCats = item.subCategories
        .map(function(val) {
          if (val.displayName != "All") {
            if (that.state.selectedCategory.includes(val.eventCategory)) {
              isParestExists = true;
              clsname = "fa fa-check-circle text-success position-absolute";
            } else {
              clsname = "";
            }
            return (
              <button
                type="button"
                onClick={that.handleClick.bind(that, val)}
                className="btn btn-sm btn-outline-secondary position-relative mr-1"
              >
                <i
                  className={clsname}
                  style={{ right: "-2px", top: "-4px" }}
                  aria-hidden="true"
                ></i>
                {val.displayName}
              </button>
            );
          }
        })
        .filter(item => {
          return item != undefined;
        });
      return (
        <div className="form-group text-left">
          <div
            className={
              "d-inline-block text-center align-middle iti-main-cat mr-3"
            }
          >
            <span className="border rounded-circle d-inline-block position-relative text-center">
              <i
                className={
                  isParestExists
                    ? "fa fa-check-circle text-success position-absolute"
                    : ""
                }
                style={{ right: "0" }}
              ></i>
              <img src={img} alt="" width="25" />
            </span>
            <small className="d-block">{item.displayName}</small>
          </div>
          <div className="d-inline-block align-middle check-icon-sub-cat">
            {subCats}
          </div>
        </div>
      );
    });
    if (
      this.props.itinerary.fetchSearchSuccess &&
      this.props.itinerary.result != undefined &&
      this.props.itinerary.result != null &&
      this.props.itinerary.result.itinerarySerchData != null &&
      options.length <= 0
    ) {
      this.props.itinerary.result.itinerarySerchData.map(function(item, index) {
        var data = {
          value: item.cityId,
          label: item.cityName + " - " + item.countryName
        };
        options.push(data);
      });
    }
    let budgetRanges = [1, 2, 3];
    var budget = budgetRanges.map((item, index) => {
      return (
        <div className="col-sm mb-2">
          <button
            type="button"
            className={
              that.state.budgetRange == index.toString() &&
              that.state.isBudgetRangeClick
                ? "w-100 d-block pt-1 pb-2 btn btn-outline-dark" + " active"
                : "w-100 d-block pt-1 pb-2 btn btn-outline-dark"
            }
            onClick={() => that.onBudgetRangeClick(index.toString())}
          >
            {index == 0 && <span>Budget </span>}
            {index == 1 && <span>Value </span>}
            {index == 2 && <span> Luxury </span>}
          </button>
        </div>
      );
    });
    if (this.state.cityOptions.length > 0) {
      let filteredCities = this.GetFilteredCities(this.state.cityOptions);
      filteredCities.map(item => {
        let tempData = {
          label: item.cityName + ", " + item.country,
          value: item.cityId,
          countryName: item.country
        };
        cityOptions.push(tempData);
      });
    }
    // if (this.props.itinerary.fetchSearchSuccess ) {
    return (
      <div>
        {(this.state.isShowForm || this.props.itinerary.isError || formFlg) && (
          <div>
            <div className="inner-banner">
              <img
                src={`${gets3BaseUrl()}/itinerary/itinerary-header-bg.jpg`}
                alt="itinerary"
                className="card-img"
              />
              <div className="card-img-overlay">
                <nav aria-label="breadcrumb">
                  <ol className="breadcrumb bg-white m-0 p-1 pl-2 d-inline-block">
                    <li className="breadcrumb-item">
                      <a href="\">Home</a>
                    </li>
                    <li className="breadcrumb-item active" aria-current="page">
                      Itinerary Planner
                    </li>
                    <li className="breadcrumb-item active" aria-current="page">
                      feelit
                    </li>
                  </ol>
                </nav>
              </div>
            </div>
            <div className="container itinerary-form pt-3">
              <div className="bd-example-modal-lg">
                <div className="modal-dialog modal-lg mt-0">
                  <div className="text-center pb-3">
                    <img
                      src={`${gets3BaseUrl()}/logos/feelr-logo-new.png`}
                      alt="feelitLIVE Logo"
                      height="40"
                      title="feelitLIVE Logo"
                    />
                  </div>
                  <div className="modal-content bg-light shadow-sm border">
                    <div className="modal-body">
                      <div className="mb-20">
                        <ProgressBar
                          striped
                          variant="warning"
                          now={this.state.anivationValue}
                        />
                      </div>

                      {this.state.isShowInputForm && (
                        <div>
                          <div className="form-group iti-search position-relative">
                            <Select
                              name="destinationname"
                              isMulti
                              required
                              placeholder="Enter destination (Country, Region or City)"
                              onChange={this.onChangeDates}
                              isClearable={true}
                              options={cityOptions}
                              value={this.state.destinationValue}
                              onInputChange={this.handleInputChange}
                              noOptionsMessage={() => {
                                return "Search Country, City, Region...";
                              }}
                              formatOptionLabel={formatOptionLabel}
                            />
                          </div>
                          <div className="form-group">
                            <div className="input-group feelr-dates">
                              <Datetime
                                inputProps={{ placeholder: "Start Date" }}
                                isValidDate={fromValid}
                                onChange={this.handleonChangeStartDate}
                                value={this.state.fromDate}
                                dateFormat="YYYY-MM-DD"
                                timeFormat={false}
                              />
                              <Datetime
                                inputProps={{ placeholder: "End Date" }}
                                className="ml-3"
                                isValidDate={toValid}
                                onChange={this.handleonChangeEndDate}
                                value={this.state.toDate}
                                dateFormat="YYYY-MM-DD"
                                timeFormat={false}
                              />

                              <div className="ml-3">
                                <button
                                  className="btn btn-outline-secondary dropdown-toggle"
                                  onClick={this.dropDownButton.bind(this)}
                                  type="button"
                                  data-toggle="dropdown"
                                  aria-haspopup="true"
                                  aria-expanded="false"
                                >
                                  {this.state.traveller}{" "}
                                  {this.state.kidsValue == 0 &&
                                  this.state.childQuantity == 0
                                    ? "adult"
                                    : "persons"}{" "}
                                </button>
                                <div className="dropdown-menu dropdown-menu-right p-3">
                                  <div className="row">
                                    <div className="col-4 pt-1">
                                      <small>Adult</small>
                                    </div>
                                    <div className="col-8 text-right">
                                      <div className="add-to-cart-btns">
                                        <a
                                          href="javascript:void(0)"
                                          className="rounded-circle border decrease-iten"
                                          onClick={this.decreaseAdultQuantity.bind(
                                            this
                                          )}
                                        >
                                          -
                                        </a>
                                        <input
                                          type="text"
                                          disabled={true}
                                          value={this.state.adultvalue}
                                          className="form-control velue-item mr-1 ml-1 bg-white"
                                        />
                                        <a
                                          href="javascript:void(0)"
                                          className="rounded-circle border increase-item"
                                          onClick={this.increaseAdultQuantity.bind(
                                            this
                                          )}
                                        >
                                          +
                                        </a>
                                      </div>
                                    </div>
                                  </div>
                                  <hr />
                                  <div className="row">
                                    <div className="col-4 pt-1">
                                      <small>Children</small>
                                    </div>
                                    <div className="col-8 text-right">
                                      <div className="add-to-cart-btns">
                                        <a
                                          href="javascript:void(0)"
                                          className="rounded-circle border decrease-iten"
                                          onClick={this.decreaseChildQuantity.bind(
                                            this
                                          )}
                                        >
                                          -
                                        </a>
                                        <input
                                          type="text"
                                          disabled={true}
                                          placeholder="0"
                                          value={this.state.childQuantity}
                                          className="form-control velue-item mr-1 ml-1 bg-white"
                                        />
                                        <a
                                          href="javascript:void(0)"
                                          className="rounded-circle border increase-item"
                                          onClick={this.increaseChildQuantity.bind(
                                            this
                                          )}
                                        >
                                          +
                                        </a>
                                      </div>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div className="form-group">
                            <h6>How would you like to feel ?</h6>
                            <div className="custom-control custom-radio custom-control-inline">
                              <input
                                type="radio"
                                id="customRadioInline1"
                                name="customRadioInline1"
                                onClick={() =>
                                  this.onClickFeelType(FeelType.Popular)
                                }
                                checked={
                                  this.state.feelType == FeelType.Popular
                                    ? true
                                    : false
                                }
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label"
                                htmlFor="customRadioInline1"
                              >
                                Popular
                              </label>
                            </div>
                            <div className="custom-control custom-radio custom-control-inline">
                              <input
                                type="radio"
                                id="customRadioInline2"
                                name="customRadioInline1"
                                onClick={() =>
                                  this.onClickFeelType(FeelType.Fast)
                                }
                                checked={
                                  this.state.feelType == FeelType.Fast
                                    ? true
                                    : false
                                }
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label"
                                htmlFor="customRadioInline2"
                              >
                                Fast-paced
                              </label>
                            </div>
                            <div className="custom-control custom-radio custom-control-inline">
                              <input
                                type="radio"
                                id="customRadioInline3"
                                name="customRadioInline1"
                                onClick={() =>
                                  this.onClickFeelType(FeelType.Slow)
                                }
                                checked={
                                  this.state.feelType == FeelType.Slow
                                    ? true
                                    : false
                                }
                                className="custom-control-input"
                              />
                              <label
                                className="custom-control-label"
                                htmlFor="customRadioInline3"
                              >
                                Slow & Easy
                              </label>
                            </div>
                          </div>
                          <hr />
                          <div className="form-group mb-0 budget-range">
                            <h6>Pick your budget range</h6>
                            <div className="row">{budget}</div>
                          </div>
                        </div>
                      )}
                      {!this.state.isShowInputForm && (
                        <div>
                          <h6>Pick your feels</h6>
                          {cats}
                        </div>
                      )}
                      {this.props.category.isLoadingCategory && (
                        <div>
                          <FilLoader />
                        </div>
                      )}
                    </div>
                    {this.state.isShowInputForm && (
                      <div className="modal-footer justify-content-center">
                        <button
                          type="button"
                          onClick={this.requestCategoryData}
                          className="btn btn-lg btn-primary text-uppercase"
                        >
                          Next
                        </button>
                      </div>
                    )}
                    {!this.state.isShowInputForm && (
                      <div className="modal-footer justify-content-center">
                        <button
                          type="button"
                          onClick={this.getInputForm}
                          className="btn btn-sm btn-link mr-2"
                        >
                          {" "}
                          <i
                            className="fa fa-long-arrow-left mr-2"
                            aria-hidden="true"
                          ></i>{" "}
                          Back
                        </button>
                        <Link
                          type="button"
                          onClick={this.getYourItinerary.bind(this)}
                          to={`${this.props.match.url}/result`}
                          className="btn btn-lg btn-primary text-uppercase"
                        >
                          get your feelr itinerary
                        </Link>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}

        {this.props.itinerary.fetchItinerarySuccess &&
          !this.state.isShowForm &&
          !this.props.itinerary.isError &&
          !formFlg && (
            <Route
              path={`${this.props.match.path}/result`}
              render={renderProps => (
                <ItineraryResult
                  data={this.props.itinerary.itineraryResult}
                  isShowForm={this.isShowForm}
                  selectedCountries={selectedCountries}
                  history={this.props.history}
                  ticketData={this.props.itinerary.itineraryTicketResult}
                  itineraryBoardRequest={this.requestItineraryBoard}
                  itinerary={this.props.itinerary}
                  searchAction={this.props.searchAction}
                  searchResult={this.props.category.searchResult}
                  getAlgoliaResults={this.props.getAlgoliaResults}
                  algoliaResults={this.props.category.algoliaResults}
                  searchSuccess={this.props.category.searchSuccess}
                  clearSearchAction={this.props.clearSearchAction}
                  clearSearchSuccess={this.props.category.clearSearchSuccess}
                />
              )}
            />
          )}
        {!this.props.itinerary.fetchItinerarySuccess &&
          !this.state.isShowForm &&
          !this.props.itinerary.isError && <FilLoader />}
      </div>
    );
  }
  componentWillUnmount() {
    sessionStorage.removeItem("itineraryForm");
  }
}
export default connect(
  (state: IApplicationState) => ({
    category: state.category,
    itinerary: state.Itinerary
  }),
  dispatch =>
    bindActionCreators(
      {
        ...CategoryStore.actionCreators,
        ...ItineraryStore.actionCreators
      },
      dispatch
    )
)(ItineraryInputForm);
