import * as React from "react";
import { autobind } from "core-decorators";
import { ReactHTMLConverter } from "react-html-converter/browser";
import { CartItem } from "../models/Cart/CartDataViewModel";
import * as getSymbolFromCurrency from "currency-symbol-map";
import * as ReactTooltip from "react-tooltip";
import { Board } from "react-trello/dist";
import { GetItineraryData } from "../utils/GetItineraryData";
import ItineraryBoardInputViewModel, {
  Card,
} from "../models/Itinerary/ItineraryBoardInputViewModel";
import Modal from "react-awesome-modal";
import CategorySearchResult from "../components/Search/CategorySearchResult";
import { gets3BaseUrl } from "../utils/imageCdn";
import * as isEmpty from "lodash/isEmpty";
import ImageComponent from "./Comman/ImageComponent";
import KzLoader from "../components/Loader/KzLoader";

var month_names_short = [
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
var daysInWeek = ["SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT"];

export default class ItineraryResult extends React.Component<any, any> {
  public constructor(props) {
    super(props);
    this.state = {
      isDateFilter: false,
      filterDate: "",
      showAltId: "",
      isItinerayUpdated: false,
      isItineraryPriceUpdated: false,
      adultQuantity: 0,
      childQuantity: 0,
      classNameHeader: "",
      isShowEditPage: false,
      visible: false,
      modalIsOpen: false,
      isModelSearch: false,
      defaultSearchValue: "",
      isTyping: false,
      searchedValue: "",
      searchedKeywords: [],
      s3BaseUrl: gets3BaseUrl(),
      show: false,
    };
    this.hideBar = this.hideBar.bind(this);
  }

  componentDidMount() {
    if (sessionStorage.getItem("itineraryForm") != null) {
      var data = JSON.parse(sessionStorage.getItem("itineraryForm"));
      this.setState({
        adultQuantity: data.noOfAdults,
        childQuantity: data.noOfChilds,
        inputModel: data,
      });
    }
    var allItinearyResultData = this.props.data;
    if (
      this.props.itinerary.itineraryBoardResponse.itineraryBoardData[0]
        .length != 0
    ) {
      allItinearyResultData = this.props.itinerary.itineraryBoardResponse
        .itineraryBoardData;
      if (!this.props.itinerary.itineraryBoardResponse.success) {
        alert("Current operation is invalid");
      }
    }
    let itineraryBoard = GetItineraryData(allItinearyResultData);
    this.setState({
      classNameHeader: "",
      allItinearyResult: allItinearyResultData,
      itineraryBoard: itineraryBoard,
      sourceData: allItinearyResultData,
    });
    window.addEventListener("scroll", this.hideBar);
  }

  hideBar() {
    if (window.pageYOffset >= 450 && window.pageXOffset === 0) {
      this.setState({ classNameHeader: "learn-head-stiky" });
    } else {
      this.setState({ classNameHeader: "" });
    }
  }

  @autobind
  onDateClick(val) {
    this.setState({ isDateFilter: true, filterDate: val });
  }

  @autobind
  onAllDaysClick() {
    this.setState({ isDateFilter: false, filterDate: "" });
  }

  @autobind
  isShowMoreClick(val) {
    this.setState({ showAltId: val.eventAltId });
  }

  @autobind
  isShowLessClick(val) {
    this.setState({ showAltId: "" });
  }

  @autobind
  onClickRemovePlace(currentItem, currentDay, currentDayData, e) {
    var allItineraryData = [];
    for (var i = 0; i < this.props.data.length; i++) {
      if (this.props.data[i].length > 0) {
        allItineraryData.push(this.props.data[i]);
      }
    }
    var data = allItineraryData;
    data.map(function(item, index) {
      var days = item;
      days.map(function(val, dayIndex) {
        if (
          val.eventAltId.toLowerCase() == currentItem.eventAltId.toLowerCase()
        ) {
          days.splice(dayIndex, 1);
        }
      });
      item = days;
    });
    this.setState({ allItinearyResult: data, isItinerayUpdated: true });
  }

  @autobind
  setValuetoLocalStorage(e) {
    var allItinearyResultData = this.state.allItinearyResult;
    var itineraryData = [];
    allItinearyResultData.map((item, index) => {
      item.map((val, currentIndex) => {
        itineraryData.push(val);
      });
    });
    var cartDataArray = [];
    var quantity = 1;
    if (this.state.childQuantity > 0) {
      quantity = 2;
    }
    itineraryData.map((item) => {
      for (var i = 0; i < quantity; i++) {
        let cartJson: CartItem = {
          altId: item.eventAltId,
          eventDetailId: item.eventId,
          name: item.eventName != undefined ? item.eventName : "",
          eventStartDate: new Date(item.placeVisitDate).toString(),
          selectedDate: new Date(item.placeVisitDate).toString(),
          venue: item.eventName != undefined ? item.eventName : "",
          city: item.cityName,
          ticketCategoryId: item.eventId,
          ticketCategoryName: i == 0 ? "Adult" : "Child",
          isAdult: i == 0 ? true : false,
          eventTicketAttributeId: i == 0 ? item.adultETAId : item.childETAId,
          currencyId: 1,
          currencyName: item.currency,
          quantity:
            i == 0 ? this.state.adultQuantity : this.state.childQuantity,
          pricePerTicket: i == 0 ? item.adultPrice : item.childPrice,
          ticketCategoryDescription: "",
          eventTermsAndConditions: "",
          isTimeSelection: false,
          placeVisitTime: "",
          guestList: [],
          isTiqetsPlace: false,
          isAddOn: false,
          ticketSubCategoryId: 1,
          ticketCategoryTypeId: 1,
          deliveryOptions: [],
          visitStartTime: item.startTime,
          visitEndTime: item.endTime,
          isItinerary: true,
        };
        cartDataArray.push(cartJson);
      }
    });
    localStorage.setItem("cartItems", JSON.stringify(cartDataArray));
    this.props.history.push("/itinerary");
  }

  @autobind
  filterUniquePlaces(places) {
    var data = places;
    places.map(function(item, index) {
      var itineraryArray = [];
      item.map((val) => {
        var isExists = itineraryArray.filter((currentArray) => {
          if (
            new Date().setHours(0, 0, 0, 0) ==
              new Date(currentArray.placeVisitDate).setHours(0, 0, 0, 0) &&
            currentArray.startTime <
              new Date().getHours() + ":" + new Date().getMinutes()
          ) {
          } else {
            return currentArray.eventId == val.eventId;
          }
        }).length;
        if (isExists == 0) {
          itineraryArray.push(val);
        }
      });
      data[index] = itineraryArray;
      return item;
    });
    return data;
  }

  @autobind
  itineraryBoardReqest(
    cardDetails,
    cardId,
    position,
    rootObject,
    sourceLaneId,
    targetLaneId,
    ItineraryBoardData,
    isDelete
  ) {
    let itineraryBoardInput: ItineraryBoardInputViewModel = {
      cardDetails: cardDetails,
      cardId: cardId,
      position: position,
      rootObject: rootObject,
      sourceLaneId: sourceLaneId,
      targetLaneId: targetLaneId,
      ItineraryBoardData: ItineraryBoardData,
      ItineraryBoardCopyData: ItineraryBoardData,
      isDelete: isDelete,
      isAddNew: false,
      placeId: 0,
    };
    this.props.itineraryBoardRequest(itineraryBoardInput);
  }

  @autobind
  openModal() {
    this.setState({
      visible: true,
      isModelSearch: false,
    });
  }

  @autobind
  closeModal() {
    this.setState({
      visible: false,
      isModelSearch: false,
    });
  }

  public onRemoveAllSearch = () => {
    this.setState({
      searchedKeywords: undefined,
      isTyping: false,
      searchedValue: "",
    });
    window.sessionStorage.removeItem("searchKeyword");
  };

  @autobind
  public getSearchResults(e) {
    let target = e.target;
    if (target) {
      let val = target.value;
      if (val != "") {
        this.setState({ isTyping: true, searchedValue: val });
      } else {
        this.setState({ isTyping: false, searchedValue: "" });
      }
      if (val && val.trim().length > 0) {
        this.props.getAlgoliaResults(val);
      }
    }
    this.props.clearSearchAction();
  }

  @autobind
  getPlaceHTML(resultModel, placeDurations, converter, symbol, isModelShow) {
    var that = this;
    var placeIndex = 0;
    var places = [];
    resultModel.map(function(item, index) {
      var visitDateData = [];
      if (
        that.state.isDateFilter &&
        item[0].placeVisitDate == that.state.filterDate
      ) {
        visitDateData = item;
      } else if (!that.state.isDateFilter || isModelShow) {
        visitDateData = item;
      }
      var currentDayPlaces = visitDateData.map(function(val, dayIndex) {
        var description = converter.convert(val.eventDescription);
        var currentDescritpion = "";
        var isShowLess = false;
        if (description != null && description != "") {
          if (
            val.eventDescription.length > 285 &&
            that.state.showAltId != val.eventAltId
          ) {
            isShowLess = true;
            currentDescritpion = description[0];
          }
        }
        var category = val.categoryName.split(",");
        var categoryData = category.map(function(item) {
          return <span className="badge badge-primary mr-3">{item}</span>;
        });
        var startTime = val.startTime.split(":");
        var endTime = val.endTime.split(":");
        placeIndex = placeIndex + 1;
        return (
          <div
            className={
              !that.state.visible ? "card shadow mt-4 mb-5" : "card shadow"
            }
          >
            <div className="row no-gutters">
              <div className="col-md-4">
                <ImageComponent
                  parentCategorySlug={val.categoryName}
                  subCategorySlug={val.categoryName}
                  s3BaseUrl={that.state.s3BaseUrl}
                  imgUrl={
                    `${that.state.s3BaseUrl}/places/tiles/` +
                    val.eventAltId.toUpperCase() +
                    "-ht-c1.jpg"
                  }
                />
              </div>
              <div className="col-md-8">
                <div className="card-body">
                  {!that.state.visible && that.state.isShowEditPage && (
                    <button
                      type="button"
                      onClick={that.onClickRemovePlace.bind(
                        that,
                        val,
                        item,
                        item
                      )}
                      className="close"
                      aria-label="Close"
                    >
                      <span aria-hidden="true">&times;</span>
                    </button>
                  )}
                  {!that.state.visible && that.state.isShowEditPage && (
                    <button
                      type="button"
                      onClick={() => {
                        that.setState({ visible: false });
                      }}
                      className="close"
                      aria-label="Close"
                    >
                      <span aria-hidden="true">&times;</span>
                    </button>
                  )}
                  <h5 className="card-title m-0">
                    {val.eventName}
                    <span className="text-muted">
                      {" "}
                      (
                      {startTime.length >= 2
                        ? startTime[0] + ":" + startTime[1]
                        : ""}{" "}
                      -{" "}
                      {endTime.length >= 2 ? endTime[0] + ":" + endTime[1] : ""}
                      )
                    </span>
                    {that.state.childQuantity > 0 && (
                      <small className="d-block">
                        Child: ({that.state.childQuantity} x {symbol}
                        {val.childPrice.toFixed(2)})
                      </small>
                    )}
                    <small className="d-block">
                      Adult: ({that.state.adultQuantity} x {symbol}
                      {val.adultPrice ? val.adultPrice.toFixed(2) : "0.00"})
                    </small>
                  </h5>
                  <p className="m-0">
                    {categoryData}
                    <a href="JavaScript:Void(0)">
                      <img
                        src={`${gets3BaseUrl()}/header/cart-icon-fill-v1.png`}
                        alt="Feel Cart Icon"
                        width="18"
                      />
                    </a>
                  </p>
                  {isShowLess && description != null && !that.state.visible && (
                    <p className="card-text card-paira">
                      <small>
                        {currentDescritpion}...{" "}
                        <a
                          href="JavaScript:Void(0)"
                          onClick={() => that.isShowMoreClick(val)}
                          className="stretched-link"
                        >
                          Read More
                        </a>
                      </small>{" "}
                    </p>
                  )}
                  {!isShowLess && description != null && !that.state.visible && (
                    <p className="card-text card-paira">
                      <small>
                        {description}...{" "}
                        <a
                          href="JavaScript:Void(0)"
                          onClick={() => that.isShowLessClick(val)}
                          className="stretched-link"
                        >
                          Show Less
                        </a>{" "}
                      </small>
                    </p>
                  )}
                  {that.state.visible && (
                    <p className="card-text card-paira place-desc-modal">
                      <small>{description} </small>
                    </p>
                  )}
                </div>
              </div>
            </div>
            {placeIndex < placeDurations.length &&
              placeDurations.length >= 2 &&
              !that.state.visible && (
                <a
                  href="JavaScript:Void(0)"
                  className="time-route text-muted text-decoration-none"
                >
                  {" "}
                  {placeDurations[placeIndex]} route »{" "}
                </a>
              )}
          </div>
        );
      });
      currentDayPlaces.map(function(val) {
        places.push(val);
      });
      return currentDayPlaces;
    });
    return places;
  }

  @autobind
  getFilteredSearchResult() {
    var algoliaResult = this.props.algoliaResults;
    if (
      this.state.inputModel &&
      this.props.algoliaResults &&
      this.props.algoliaResults.length > 0
    ) {
      var cities = this.state.inputModel.queryString.split(",");
      var categories = this.state.inputModel.categories.split(",");
      algoliaResult = this.props.algoliaResults
        .filter((val) => {
          if (cities.indexOf(val.city) > -1) {
            return val;
          }
        })
        .filter((val) => {
          return val != undefined;
        });
    }
    return algoliaResult;
  }

  @autobind
  getNewlyAddedPlace(item) {
    var isExists = this.state.sourceData
      .map((val) => {
        var data = val.filter((card) => {
          return card.id == +item.objectID;
        });
        return data;
      })
      .filter((val) => val.length > 0).length;
    if (isExists > 0) {
      alert("Place already exists in your itinerary, Thanks!");
    } else {
      let itineraryBoardInput: ItineraryBoardInputViewModel = {
        isAddNew: true,
        placeId: +item.objectID,
        ItineraryBoardData: this.state.sourceData,
        ItineraryBoardCopyData: this.state.sourceData,
        sourceLaneId: "0",
        targetLaneId: "0",
        cardId: "0",
        position: 0,
        rootObject: null,
        cardDetails: null,
        isDelete: false,
      };
      this.props.itineraryBoardRequest(itineraryBoardInput);
    }
  }

  public render() {
    if (this.state.allItinearyResult) {
      var startDate = "";
      var endDate = "";
      var year = "";
      var price = 0;
      var currency = "";
      var days;
      var places = [];
      var visitDates = [];
      var placeDurations = [];
      var that = this;
      var placeIndex = 0;
      var placeAltId = "";
      var modelData;
      var className = "itinerary-places";
      const converter = ReactHTMLConverter();
      var selectedCountry = "";
      var selectedCountries = this.props.selectedCountries;
      if (selectedCountries.lenght == 1) {
        selectedCountry = selectedCountries[0];
      } else {
        selectedCountries.map(function(item, index) {
          if (selectedCountries.length - 1 > index) {
            selectedCountry = selectedCountry + item + "-";
          } else {
            selectedCountry = selectedCountry + item;
          }
        });
      }
      converter.registerComponent("ItineraryResult", ItineraryResult);
      this.state.allItinearyResult.map(function(item, index) {
        days = item.map(function(val, dayIndex) {
          if (index == 0 && dayIndex == 0) {
            placeAltId = val.eventAltId;
          }
          price =
            price +
            (val.adultPrice * that.state.adultQuantity +
              val.childPrice * that.state.childQuantity);
          currency = val.currency;
          var currentClass = "col border-right ";
          var date = new Date(val.placeVisitDate).getDate();
          var month = new Date(val.placeVisitDate).getMonth();
          var day = new Date(val.placeVisitDate).getDay();
          year = new Date(val.placeVisitDate).getFullYear().toString();
          if (that.state.filterDate == "") {
            placeDurations.push(val.travelDuration);
          } else if (val.placeVisitDate == that.state.filterDate) {
            placeDurations.push(val.travelDuration);
          }
          if (val.placeVisitDate == that.state.filterDate) {
            currentClass = "col border-right active";
          }
          if (index == 0) {
            startDate = date + " " + month_names_short[month];
          }
          if (that.state.allItinearyResult.length == index + 1) {
            endDate = date + " " + month_names_short[month];
          }
          if (dayIndex == 0) {
            return (
              <a
                className={currentClass}
                onClick={() => that.onDateClick(val.placeVisitDate)}
              >
                {date + " " + month_names_short[month]}{" "}
                <span className="d-block"> {daysInWeek[day]} </span>{" "}
              </a>
            );
          }
        });
        days.map(function(val) {
          if (val != undefined) {
            visitDates.push(val);
          }
        });
      });
      if (placeDurations.length == 1) {
        className = "";
      }
      var symbol = getSymbolFromCurrency(currency);
      var itineraryModel = this.state.sourceData
        .map(function(item, index) {
          var data = [];
          item.filter((val, index) => {
            if (val.id == that.state.cardId) {
              data.push(val);
            }
          });
          return data;
        })
        .filter((item) => {
          return item.length > 0;
        });

      var places = this.getPlaceHTML(
        this.state.allItinearyResult,
        placeDurations,
        converter,
        symbol,
        false
      );
      if (itineraryModel.length > 0) {
        modelData = this.getPlaceHTML(
          itineraryModel,
          placeDurations,
          converter,
          symbol,
          true
        );
      }
      let anyResults =
        !isEmpty(this.props.searchResult.categoryEvents) ||
        !isEmpty(this.props.searchResult.cities) ||
        !isEmpty(this.props.searchResult.countries) ||
        !isEmpty(this.props.searchResult.states);

      let showResults =
        (this.props.searchSuccess || this.props.clearSearchSuccess) &&
        anyResults;
      let classNames = "search-dropdown bg-white p-3 border dropdown-menu";
      var filteredAlgoliaData = this.getFilteredSearchResult();
      var totalPrice = price.toFixed(2);
      return (
        <div>
          <div className="itineraryResult">
            <div className="inner-banner">
              <img
                src={`${gets3BaseUrl()}/places/about/${placeAltId.toUpperCase()}-about.jpg`}
                alt="card image"
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
                <div className="banner-text">
                  <h1 className="text-white">
                    {this.state.allItinearyResult.length}{" "}
                    {this.state.allItinearyResult.length == 1 ? `day` : `days`}{" "}
                    in {selectedCountry}
                    <small className="d-block">
                      {startDate} - {endDate}, {year}{" "}
                      <a
                        href="JavaScript:Void(0)"
                        onClick={this.props.isShowForm}
                        className="badge badge-secondary"
                      >
                        <i className="fa fa-pencil mr-1" aria-hidden="true" />{" "}
                        Edit
                      </a>
                    </small>
                  </h1>
                </div>
              </div>
            </div>
            <div className="pt-2 pb-2 container page-content learn-more-page">
              <header className={this.state.classNameHeader}>
                <div className="row learn-head">
                  <div className="col-xl-4 col-lg-4 col-sm-12">
                    <h3 className="mb-0 mt-2 h3">
                      {this.state.allItinearyResult.length}{" "}
                      {this.state.allItinearyResult.length == 1
                        ? `day`
                        : `days`}{" "}
                      in {selectedCountry}
                    </h3>
                  </div>

                  <div className="col-xl-8 col-lg-8 col-sm-12 right-head text-lg-right">
                    <div className="d-inline-block align-middle pr-3">
                      <a
                        href="JavaScript:Void(0)"
                        onClick={() => {
                          this.setState({
                            visible: !this.state.visible,
                            isModelSearch: !this.state.isModelSearch,
                          });
                        }}
                        className="btn btn-outline-primary btn-sm"
                      >
                        {" "}
                        <i
                          className="fa fa-plus mr-1 board-view-i"
                          aria-hidden="true"
                        />{" "}
                        Add
                      </a>
                    </div>
                    <div className="d-inline-block align-middle pr-3">
                      <a
                        href="JavaScript:Void(0)"
                        onClick={() => {
                          this.setState({
                            isShowEditPage: !this.state.isShowEditPage,
                          });
                        }}
                        className="btn btn-outline-primary btn-sm"
                      >
                        {!this.state.isShowEditPage ? (
                          <i
                            className="fa fa-pencil mr-1 board-view-i"
                            aria-hidden="true"
                          />
                        ) : null}{" "}
                        {this.state.isShowEditPage
                          ? "Place View"
                          : "Board View"}
                      </a>
                    </div>
                    <div className="d-inline-block align-middle pr-3">
                      <a
                        href="JavaScript:Void(0)"
                        className="text-left text-muted text-decoration-none"
                        data-tip=""
                        data-for="dwnPdf"
                        id="dwnPdf"
                      >
                        <small>
                          <i className="fa fa-download" aria-hidden="true" />{" "}
                        </small>
                      </a>
                      <ReactTooltip id="dwnPdf" type="info">
                        <span id="build_itinerary">Download PDF</span>
                      </ReactTooltip>
                    </div>
                    <div className="d-inline-block align-middle pr-3">
                      <a
                        href="JavaScript:Void(0)"
                        className="text-left text-muted text-decoration-none"
                      >
                        <small>
                          <i className="fa fa-print" aria-hidden="true" />{" "}
                        </small>
                      </a>
                    </div>
                    <div className="d-inline-block align-middle pr-3">
                      <a
                        href="JavaScript:Void(0)"
                        className="text-left text-muted text-decoration-none"
                      >
                        {" "}
                        <img
                          src={`${gets3BaseUrl()}/itinerary/feel-pay.png`}
                          alt=""
                        />{" "}
                      </a>
                    </div>
                    <div className="d-inline-block align-middle pr-3">
                      <a
                        href="JavaScript:Void(0)"
                        className="text-left text-muted text-decoration-none"
                      >
                        <small>
                          <i className="fa fa-cog" aria-hidden="true" />
                        </small>
                      </a>
                    </div>
                    <div className="d-inline-block align-middle">
                      <h5 className="text-left pr-3 m-0">
                        Total: {symbol}
                        {totalPrice}
                      </h5>
                    </div>
                    <div className="d-inline-block align-middle book-btn-iti">
                      <a
                        href="JavaScript:Void(0)"
                        onClick={this.setValuetoLocalStorage.bind(this)}
                        className="btn site-primery-btn text-uppercase"
                        style={{ fontSize: "16px" }}
                      >
                        Book Entire Itinerary
                      </a>
                    </div>
                  </div>
                </div>
                <hr />
                {!this.state.isShowEditPage && (
                  <div className="row m-0 text-center date-tab border-top border-bottom border-left">
                    <a
                      className={
                        !this.state.isDateFilter
                          ? "col border-right active"
                          : "col border-right"
                      }
                      onClick={this.onAllDaysClick}
                    >
                      {startDate}- {endDate}
                    </a>
                    {visitDates}
                  </div>
                )}
              </header>
              {!this.state.isShowEditPage && (
                <div className={className}>{places}</div>
              )}
              {this.state.isShowEditPage && (
                <div>
                  <Board
                    editable
                    data={this.state.itineraryBoard}
                    style={{ backgroundColor: "#fff", height: "auto" }}
                    draggable
                    laneDraggable={false}
                    handleDragEnd={(
                      cardId,
                      sourceLaneId,
                      targetLaneId,
                      position,
                      cardDetails
                    ) => {
                      that.itineraryBoardReqest(
                        cardDetails,
                        cardId,
                        position,
                        that.state.itineraryBoard,
                        sourceLaneId,
                        targetLaneId,
                        that.state.sourceData,
                        false
                      );
                    }}
                    onCardDelete={(cardId, laneId) => {
                      let cardDetails: Card = {
                        description: "",
                        id: "0",
                        label: "",
                        title: "",
                      };
                      that.itineraryBoardReqest(
                        cardDetails,
                        cardId,
                        0,
                        that.state.itineraryBoard,
                        laneId,
                        laneId,
                        that.state.sourceData,
                        true
                      );
                    }}
                    onCardClick={(cardId, metadata, laneId) => {
                      that.setState({ cardId: cardId, visible: true });
                    }}
                  />
                </div>
              )}
            </div>
          </div>
          <Modal
            visible={this.state.visible}
            width="800"
            effect="fadeInUp"
            onClickAway={() => this.closeModal()}
          >
            {!this.state.isModelSearch && (
              <div className={className}>{modelData}</div>
            )}
            {this.state.isModelSearch && (
              <div className="home-slider">
                <div
                  className="header-search"
                  style={{ maxWidth: "100%", margin: "0 15px" }}
                >
                  <div className="form-group gray-placehoder dropdown">
                    {(this.state.searchedKeywords == undefined ||
                      this.state.searchedKeywords.length == 0) && (
                      <img
                        src={`${gets3BaseUrl()}/header/search-icon.png`}
                        alt="search"
                        title="search icon"
                        className="search-icon"
                        style={{ filter: "none" }}
                      />
                    )}
                    <input
                      type="text"
                      className="form-control border bg-white"
                      aria-describedby="“FeelJaipur”"
                      value={this.state.searchedValue}
                      id="dropdownMenuButton"
                      data-toggle="dropdown"
                      aria-haspopup="true"
                      aria-expanded="false"
                      placeholder={`Add more places in ${this.state.inputModel.queryString
                        .split(/[\d,]+/)
                        .join(", ")}`}
                      onChange={this.getSearchResults}
                    />

                    {this.state.isTyping && (
                      <button
                        onClick={this.onRemoveAllSearch}
                        type="button"
                        className="close search-clear"
                        aria-label="Close"
                      >
                        <span aria-hidden="true">×</span>
                      </button>
                    )}

                    <div
                      className={
                        anyResults ? classNames : `${classNames} search-hidden`
                      }
                      aria-labelledby="dropdownMenuButton"
                      x-placement="bottom-start"
                    >
                      {(showResults ||
                        this.props.algoliaResults.length > 0) && (
                        <CategorySearchResult
                          searchSuccess={this.props.searchSuccess}
                          emptySearch={this.props.clearSearchSuccess}
                          searchResult={this.props.searchResult.categoryEvents}
                          searchText={this.state.searchedValue}
                          getAlgoliaResults={this.props.getAlgoliaResults}
                          algoliaResults={filteredAlgoliaData}
                          isItinerarySearch={true}
                          getSelectedPlace={this.getNewlyAddedPlace}
                        />
                      )}
                    </div>
                  </div>
                </div>
              </div>
            )}
          </Modal>
        </div>
      );
    } else {
      return <KzLoader />;
    }
  }
}
