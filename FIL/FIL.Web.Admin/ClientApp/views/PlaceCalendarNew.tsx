import * as React from "react";
import { connect } from "react-redux";
import { autobind } from "core-decorators";
import { IApplicationState } from "../stores";
import { bindActionCreators } from "redux";
import { PlaceCalendarRequestViewModel, customTimeModelData, regularViewModel, seasonViewModel, specailDatimeViewModel, specialDayViewModel, speecialDateSeasonTimeViewModel } from "../models/PlaceCalendar/PlaceCalendarRequestViewModel";
import PlaceCalendarResponseViewModel from "../models/PlaceCalendar/PlaceCalendarResponseViewModel";
import DocumentTypesSaveResponseViewModel from "../models/Inventory/DocumentTypesSaveResponseViewModel";
import InventoryResponseViewModel from "../models/Inventory/InventoryResponseViewModel";
import * as placeCalendarStore from "../stores/PlaceCalendar";
import * as currencyTypeStore from "../stores/CurrencyType";
import * as inventoryStore from "../stores/Inventory";
import Inventory from "./Inventory";
import * as Datetime from "react-datetime";
import { timeViewModel } from "../models/PlaceCalendar/PlaceCalendarRequestViewModel";
import DayPicker, { DayPickerProps } from 'react-day-picker';
import DateUtils from 'react-day-picker';
import * as moment from "moment";
import Newloader from "../components/NewLoader/NewLoader";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import 'react-day-picker/lib/style.css';
import "./DatePicker.css";

type PlaceCalendarProps = placeCalendarStore.IPlaceCalendarProps
    & inventoryStore.InventoryProps
    & currencyTypeStore.ICurrencyTypeProps
    & typeof currencyTypeStore.actionCreators
    & typeof inventoryStore.actionCreators
    & typeof placeCalendarStore.actionCreators;

const daysInWeek = ['All Days', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
var timingId = 0;

var customTimeModel = [{
    "day": "",
    "time": [{
        "id": 1,
        "from": "",
        "to": "",
    }]
}];

var time = [{
    "id": 1,
    "from": "",
    "to": ""
}];

var seasonModel = [{
    "id": 1,
    "name": "",
    "startDate": null,
    "endDate": null,
    "isSameTime": true,
    "sameTime": time,
    "daysOpen": [false, false, false, false, false, false, false, false],
    "time": customTimeModel
}];

var specialDayMasterModel = [{
    "id": 1,
    "name": "",
    "specialDate": null,
    "from": "",
    "to": ""
}];
var summary;

class PlaceCalendarNew extends React.Component<PlaceCalendarProps, any> {
    fromTime: any;
    toTime: any;

    constructor(props) {
        super(props);
        this.handleDayClick = this.handleDayClick.bind(this);
        this.state = {
            placeHolidayDate: [],
            weekOffDays: [],
            startdatetime: new Date(),
            enddatetime: new Date(),
            isPerennialCalendar: true,
            isHolidayCalendarShow: false,
            timingModel: [],
            totalTimings: 1,
            selectedDays: [],
            calenderAccordianEditComponent: <div></div>,
            isCalenderEdit: true,
            inventoryDetailAccordianEditComponent: <div></div>,
            isInventoryDetailEdit: false,
            isEdit: false,
            isDefaultStateUpdated: false,
            isAlertCall: false,
            isDaysClick: false,
            isRegularClick: false,
            isSeasonClick: false,
            isSpecialClick: false,
            isRegularCustomeTime: false,
            customTimeModel: [],
            seasonTimeModel: seasonModel,
            specialDayModel: specialDayMasterModel,
            isSeasonTimeAllow: true,
            isSpecialDayTimeAllow: true,
            isSeasonTouched: false,
            isSpecialDayTouched: false
        }
    }
    editId: any;

    handleEditButtonPressed(editedComponent) {
        if (editedComponent === "Calender") {
            this.setState({
                calenderAccordianEditComponent: <span className=" pull-right"><i className="fa fa-check" onClick={() => this.handleEditSaveButtonPressed('Calender')} style={{ color: "green", marginRight: 8 }}  ></i><i className="fa fa-times " onClick={() => this.handleEditRevertButtonPressed('Calender')} style={{ color: "red" }} data-icon="&#x25a8;"></i></span>
                , isCalenderEdit: false
            });
        }
        if (editedComponent === "Inventory") {
            this.setState({
                inventoryDetailAccordianEditComponent: <span className=" pull-right"><i className="fa fa-check" onClick={() => this.handleEditSaveButtonPressed('Inventory')} style={{ color: "green", marginRight: 8 }}  ></i><i className="fa fa-times " onClick={() => this.handleEditRevertButtonPressed('Inventory')} style={{ color: "red" }} data-icon="&#x25a8;"></i></span>
                , isImagesEdit: false
            });
        }

    }

    @autobind
    notify() {
        toast.success('Calendar saved successfully', {
            position: "top-center",
            autoClose: 6000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true
        });
        this.setState({ isCreated: true });
    }

    handleEditSaveButtonPressed(editedComponent) {
        if (editedComponent === "Calender") {
            document.getElementById('calenderSubmitBtn').click();
            this.setState({
                calenderAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Calender')} aria-hidden="true"></i>,
                isCalenderEdit: true
            });
        }

        if (editedComponent === "Inventory") {
            document.getElementById('saveBtn').click();
            this.setState({
                inventoryDetailAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Inventory')} aria-hidden="true"></i>,
                isDetailEdit: true
            });
        }

    }
    handleEditRevertButtonPressed(editedComponent) {
        if (editedComponent === "Calender") {
            this.setState({
                calenderAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Calender')} aria-hidden="true"></i>,
                isCalenderEdit: true
            });
        }
        if (editedComponent === "Inventory") {
            this.setState({
                inventoryDetailAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Inventory')} aria-hidden="true"></i>,
                isDetailEdit: true
            });
        }

    }

    @autobind
    public handleEditButtonPressedInventory() {
        this.setState({
            inventoryDetailAccordianEditComponent: <span className=" pull-right"><i className="fa fa-check" onClick={() => this.handleEditSaveButtonPressed('Inventory')} style={{ color: "green", marginRight: 8 }}  ></i><i className="fa fa-times " onClick={() => this.handleEditRevertButtonPressed('Inventory')} style={{ color: "red" }} data-icon="&#x25a8;"></i></span>
            , isInventoryDetailEdit: false
        });
    }

    @autobind
    public handleEditSaveButtonPressedInventory() {
        this.setState({
            inventoryDetailAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Inventory')} aria-hidden="true"></i>,
            isInventoryDetailEdit: true
        });

    }
    @autobind
    public handleEditRevertButtonPressedInventory() {
        this.setState({
            inventoryDetailAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Inventory')} aria-hidden="true"></i>,
            isInventoryDetailEdit: true
        });
    }

    public componentDidMount() {
        var placeTimings = [];
        timingId = timingId + 1;
        var timingViewModel: timeViewModel = {
            id: timingId,
            from: "",
            to: ""
        }
        placeTimings.push(timingViewModel);
        if (this)
            this.props.requestTicketCategoryTypes();
        if (this.props.currencyType.currencyTypes.currencyTypes.length <= 0) {
            this.props.requestCurrencyTypeData();
        }
        if (this.props.currencyType.documentTypes.documentTypes.length <= 0) {
            this.props.requestDocumentTypeData();
        }

        if (this.props.currencyType.deliveryTypes.deliveryTypes.length <= 0) {
            this.props.requestDeliveryTypeData();
        }

        if (this.props.currencyType.refundPolicies.refundPolicies.length <= 0) {
            this.props.requestRefundPolicies();
        }
        this.props.requestCustomerInformationControlData();
        localStorage.removeItem("timings");
        this.setState({ timingModel: placeTimings });

        let urlparts = window.location.href.split('/');
        this.editId = parseInt(urlparts[urlparts.length - 1], 0);
        var edit = urlparts[urlparts.length - 2];
        var isEdit = false;
        if (edit == "edit") {
            isEdit = true;
        }
        if (isEdit) {
            this.setState({
                isCalenderEdit: true,
                isEdit: true,
                placeAltId: urlparts[4],
                isInventoryDetailEdit: true,
                calenderAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Calender')} aria-hidden="true"></i>,
                inventoryDetailAccordianEditComponent: <i className="fa fa-edit pull-right" onClick={() => this.handleEditButtonPressed('Inventory')} aria-hidden="true"></i>,
            });
            this.props.requestInentoryData(urlparts[4], (item) => { });
        }
    }

    @autobind
    private onSubmitLeaveDates(holidayDates) {
        this.setState({ placeHolidayDate: holidayDates });
    }

    @autobind
    private onDaysSelect(val, e) {
        var index = daysInWeek.indexOf(val);
        var weekOffDays = [];
        var that = this;
        var daysTimeModel = [];
        var seasonTime = this.state.seasonTimeModel;
        var daySize = 8;
        for (var i = 0; i < daySize; i++) {
            if (val == "All Days" && e.target.checked) {
                weekOffDays.push(true);
            } else if (val == "All Days" && !e.target.checked) {
                weekOffDays.push(false);
            } else if (!e.target.checked && i == 0) {
                weekOffDays.push(false);
            } else {
                var isHolidayDay = false;
                if (this.state.weekOffDays.length > 0) {
                    isHolidayDay = this.state.weekOffDays[i];
                }
                if (i == index && !isHolidayDay) {
                    isHolidayDay = true;
                } else if (i == index && isHolidayDay) {
                    isHolidayDay = false;
                }
                weekOffDays.push(isHolidayDay);
            }
        }
        var isDayExists = weekOffDays.filter(function (item) {
            return item == true
        });
        weekOffDays.map(function (item, index) {
            var day = daysInWeek[index];
            if (item && index > 0) {
                var TimeModel;
                that.state.customTimeModel.filter(function (val) {
                    if (val.day == day) {
                        TimeModel = val;
                    }
                })
                if (TimeModel == undefined) {
                    TimeModel = {
                        "day": day,
                        "id": index,
                        "time": [{
                            "id": 0,
                            "from": "",
                            "to": "",
                        }]
                    };
                }

                daysTimeModel.push(TimeModel);
            }
        });
        this.setState({ weekOffDays: weekOffDays, isDaysClick: (isDayExists.length > 0 ? true : false), customTimeModel: daysTimeModel }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onClickTimeType(val, e) {
        if (val == "regular") {
            this.setState({ isRegularClick: true, isSeasonClick: false, isSpecialClick: false }, function () { this.updateCalendarSummary(); });
        } else if (val == "season") {
            this.setState({ isSeasonClick: true, isRegularClick: false, isSpecialClick: false }, function () { this.updateCalendarSummary(); });
        } else {
            this.setState({ isSpecialClick: true, isRegularClick: false, isSeasonClick: false }, function () { this.updateCalendarSummary(); });
        }

    }

    @autobind
    onClickSubRegularTimeType(val, e) {
        if (val == "same") {
            this.setState({ isRegularSameTime: true, isRegularCustomeTime: false }, function () { this.updateCalendarSummary(); });
        } else if (val == "custom") {
            this.setState({ isRegularCustomeTime: true, isRegularSameTime: false }, function () { this.updateCalendarSummary(); });
        }
    }

    @autobind
    public OnChangeStartDatetime(e) {
        this.setState({ startdatetime: e })
    }

    @autobind
    public OnChangeEndtime(e) {
        this.setState({ enddatetime: e })
    }

    @autobind
    public onClickPerennialCalendarType() {
        if (!this.state.isPerennialCalendar) {
            this.setState({ isPerennialCalendar: true }, function () { this.updateCalendarSummary(); })
        }
    }

    @autobind
    public onClickRegularCalendarType() {
        if (this.state.isPerennialCalendar) {
            this.setState({ isPerennialCalendar: false }, function () { this.updateCalendarSummary(); })
        }
    }

    @autobind
    public onClickShowHolidayCalendar() {
        if (this.state.isCalenderEdit)
            this.setState({ isHolidayCalendarShow: !this.state.isHolidayCalendarShow });
    }

    @autobind
    public onClickAddTiming() {
        var placeTimings = this.state.timingModel;
        timingId = timingId + 1;
        var timingViewModel: timeViewModel = {
            id: timingId,
            from: "",
            to: ""
        }
        placeTimings.push(timingViewModel);
        this.setState({ timingModel: placeTimings }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    public onChange(timeType, index, time) {
        if (timeType == 1) {
            this.setState({ ["from" + index]: time }, function () { this.updateCalendarSummary(); });
        } else {
            this.setState({ ["to" + index]: time }, function () { this.updateCalendarSummary(); });
        }
    }

    @autobind
    onTimeChange(time) {
        this.setState({ time });
    }

    @autobind
    public onValueChange(id, name, e) {
        var timing = this.state.timingModel;
        var that = this;
        timing.map(function (item) {
            if (item.id == id) {
                if (name == "fromTime") {
                    item.from = e.target.value
                } else if (name == "toTime") {
                    item.to = e.target.value;
                }
            }
        });
        this.setState({ timingModel: timing }, function () { this.updateCalendarSummary(); });
    }

    handleDayClick(day) {
        const { selectedDays } = this.state;
        const selectedIndex = selectedDays.findIndex(selectedDay =>
            DateUtils.DateUtils.isSameDay(selectedDay, day)
        );
        if (selectedIndex >= 0) {
            selectedDays.splice(selectedIndex, 1);
        } else {
            selectedDays.push(day);
        }
        this.setState({ selectedDays }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onRemoveTiming(index, e) {
        this.state.timingModel.splice(index, 1);
        this.setState({ timingModel: this.state.timingModel }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    getSummaryModel() {
        var summaryModel = {
            "id": 0,
            "timeType": "",
            "daysOpen": "",
            "time": "",
            "isSeasonSameTime": false,
            "seasonTimeId": 0
        }
        return summaryModel;
    }

    @autobind
    onRemoveTime(index, e) {
        var open = [false, false, false, false, false, false, false, false];
        if (index.timeType.indexOf("Regular") > -1) {
            if (this.state.isRegularCustomeTime) {
                var data = this.state.customTimeModel;
                this.state.customTimeModel.map(function (val, currentIndex) {
                    if (val.id == index.id) {
                        data.splice(currentIndex, 1);
                    }
                });
                data.map(function (item) {
                    var isDayExists = daysInWeek.filter(function (val) {
                        return item.day == val;
                    });
                    if (isDayExists.length > 0) {
                        open[daysInWeek.indexOf(isDayExists[0])] = true;
                    }
                })
                this.setState({ customTimeModel: data, weekOffDays: open }, function () { this.updateCalendarSummary(); });
            } else {
                var data = this.state.timingModel;
                this.state.timingModel.map(function (val, currentIndex) {
                    if (val.id == index.id) {
                        data.splice(currentIndex, 1);

                    }
                });
                this.setState({ timingModel: data, weekOffDays: open }, function () { this.updateCalendarSummary(); });
            }

        } else if (index.timeType.indexOf("Season") > -1) {
            if (index.isSeasonSameTime) {
                var data = this.state.seasonTimeModel;
                this.state.seasonTimeModel.map(function (val, currentIndex) {
                    if (val.id == index.id) {
                        data.splice(currentIndex, 1);
                    }
                });
                this.setState({ seasonTimeModel: data }, function () { this.updateCalendarSummary(); });
            } else {
                var data = this.state.seasonTimeModel;
                data.map(function (val, currentIndex) {
                    if (val.id == index.id && val.time.length > 1) {
                        var currentTime = val.time;
                        val.time.map(function (currentTimeValue, currentTimeIndex) {
                            if (currentTimeValue.id == index.seasonTimeId) {
                                currentTime.splice(currentTimeIndex, 1);
                            }
                        });
                        val.time = currentTime;
                        val.time.map(function (item) {
                            var isDayExists = daysInWeek.filter(function (val) {
                                return item.day == val;
                            });
                            if (isDayExists.length > 0) {
                                open[daysInWeek.indexOf(isDayExists[0])] = true;
                            }
                        });
                        val.daysOpen = open;
                    } else if (val.id == index.id && val.time.length == 1) {
                        data.splice(currentIndex, 1);
                    }
                });
                this.setState({ seasonTimeModel: data }, function () { this.updateCalendarSummary(); });
            }
        } else if (index.timeType.indexOf("Special") > -1) {
            var specialDay = this.state.specialDayModel;
            specialDay.map(function (item, currentIndex) {
                if (item.id == index.id) {
                    specialDay.splice(currentIndex, 1);
                }
            });
            this.setState({ specialDayModel: specialDay }, function () { this.updateCalendarSummary(); });
        }
    }

    @autobind
    updateCalendarSummary() {
        var regularDays = this.state.weekOffDays;
        var regularOpendays = "";
        var openDaysCount = 0;
        var that = this;
        var summaryData = [];
        var time = "";
        if (this.state.isRegularSameTime) {
            regularDays.map(function (item, index) {
                if (item && index > 0) {
                    var day = daysInWeek[index];
                    regularOpendays = regularOpendays + day + ", ";
                    openDaysCount = openDaysCount + 1;
                }
            });
            var dataModel = this.getSummaryModel();
            this.state.timingModel.map(function (item) {
                var currentTime = item.from + " - " + item.to;
                if (item.from != "" && item.to != "") {
                    time = time + currentTime + ", ";
                }
                dataModel.id = item.id;
            })
            if (openDaysCount == 7) {
                regularOpendays = "All days";
            }
            regularOpendays = regularOpendays.trim().replace(/(^,)|(,$)/g, "");
            time = time.trim().replace(/(^,)|(,$)/g, "");
            dataModel.timeType = "Regular";
            dataModel.daysOpen = regularOpendays;
            dataModel.time = time;
            summaryData.push(dataModel);
        } else if (this.state.isRegularCustomeTime) {
            this.state.customTimeModel.map(function (item, index) {
                if (item.day != "All Days") {
                    var day = item.day;
                    time = "";
                    item.time.map(function (val) {
                        var currentTime = val.from + " - " + val.to;
                        if (val.from != "" && val.to != "") {
                            time = time + currentTime + ", ";
                        }
                    });
                    var dataModel = that.getSummaryModel();
                    time = time.trim().replace(/(^,)|(,$)/g, "");
                    dataModel.time = time;
                    dataModel.id = item.id;
                    dataModel.daysOpen = day;
                    dataModel.timeType = "Regular";
                    summaryData.push(dataModel);
                }
            });
        }
        if (this.state.isSeasonTimeAllow) {
            this.state.seasonTimeModel.map(function (item) {
                time = "";
                regularOpendays = "";
                openDaysCount = 0;
                if (item.name != "") {
                    if (item.isSameTime) {
                        item.daysOpen.map(function (val, index) {
                            if (val && index > 0) {
                                var day = daysInWeek[index];
                                regularOpendays = regularOpendays + day + ", ";
                                openDaysCount = openDaysCount + 1;
                            }
                        });
                        item.sameTime.map(function (item) {
                            var currentTime = item.from + " - " + item.to;
                            if (item.from != "" && item.to != "") {
                                time = time + currentTime + ", ";
                            }
                        })
                        var dataModel = that.getSummaryModel();
                        if (openDaysCount == 7) {
                            regularOpendays = "All days";
                        }
                        regularOpendays = regularOpendays.trim().replace(/(^,)|(,$)/g, "");
                        time = time.trim().replace(/(^,)|(,$)/g, "");
                        dataModel.timeType = "Season - " + item.name;
                        dataModel.daysOpen = regularOpendays;
                        dataModel.id = item.id;
                        dataModel.time = time;
                        dataModel.isSeasonSameTime = true;
                        summaryData.push(dataModel);
                    } else {
                        item.time.map(function (val, index) {
                            if (val.day != "All Days") {
                                var day = val.day;
                                time = "";
                                val.time.map(function (currentTimeValue) {
                                    var currentTime = currentTimeValue.from + " - " + currentTimeValue.to;
                                    if (currentTimeValue.from != "" && currentTimeValue.to != "") {
                                        time = time + currentTime + ", ";
                                    }
                                });
                                var dataModel = that.getSummaryModel();
                                time = time.trim().replace(/(^,)|(,$)/g, "");
                                dataModel.id = item.id;
                                dataModel.time = time;
                                dataModel.seasonTimeId = val.id
                                dataModel.daysOpen = day;
                                dataModel.timeType = "Season - " + item.name;
                                dataModel.isSeasonSameTime = false;
                                summaryData.push(dataModel);
                            }
                        });
                    }
                }
            })
        }
        if (this.state.isSpecialDayTimeAllow) {
            this.state.specialDayModel.map(function (item) {
                if (item.name != "" && item.from != "" && item.to != "") {
                    var dataModel = that.getSummaryModel();
                    dataModel.time = item.from + " - " + item.to;
                    dataModel.daysOpen = " - ";
                    dataModel.id = item.id;
                    dataModel.timeType = "Special Day/Holiday - " + item.name;
                    summaryData.push(dataModel);
                }
            })
        }
        summaryData = summaryData.filter(function (item) {
            return item.daysOpen != ""
        });
        if (summaryData.length > 0) {
            summary = summaryData.map(function (item) {
                return <tr>
                    <td className="text-center">{item.timeType}</td>
                    <td className="text-center">{item.daysOpen}</td>
                    <td className="text-center">{item.time}</td>
                    <td className="text-center"><a href="JavaScript:Void(0)" onClick={that.onRemoveTime.bind(that, item)} className="text-decoration-none btn-link mr-4">Delete</a></td>
                </tr>
            });
        } else {
            summary = undefined
        }

        this.setState({ updateSummary: true });
    }

    @autobind
    updatedCalendarState() {
        var weekOffDays = [];
        var daySize = 7;
        var placeTimings = [];
        var holidayDates = [];
        timingId = 0;
        var that = this;
        var isOffAllDay = false;
        if (this.props.inventory.getPlaceInventoryDataResponseViewModel.placeWeekOffs != undefined) {
            this.props.inventory.getPlaceInventoryDataResponseViewModel.placeWeekOffs.map(function (item, index) {
                if (item.weekOffDay == 0) {
                    isOffAllDay = true;
                }
            });
            if (isOffAllDay) {
                for (var i = 1; i <= daySize; i++) {
                    weekOffDays.push(true);
                }
            }
            if (!isOffAllDay) {
                for (var i = 1; i <= daySize; i++) {
                    var isOpen = that.props.inventory.getPlaceInventoryDataResponseViewModel.placeWeekOffs.filter(function (item, index) {
                        return item.weekOffDay == i
                    });
                    if (isOpen.length > 0) {
                        weekOffDays.push(true);
                    } else {
                        weekOffDays.push(false);
                    }
                }
            }
            this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails.map(function (item, index) {
                timingId = timingId + 1;
                var startDate = new Date(item.startDateTime);
                var endDate = new Date(item.endDateTime);

                var timingViewModel: timeViewModel = {
                    id: timingId,
                    from: (startDate.getHours().toString() + ":" + ((startDate.getMinutes().toString() == "0") ? "00" : startDate.getMinutes().toString())),
                    to: (endDate.getHours().toString() + ":" + ((endDate.getMinutes().toString() == "0") ? "00" : endDate.getMinutes().toString())),
                };
                if (timingViewModel.from.length == 4) {
                    timingViewModel.from = "0" + timingViewModel.from;
                } if (timingViewModel.to.length == 4) {
                    timingViewModel.to = "0" + timingViewModel.to;
                }
                placeTimings.push(timingViewModel);
            });
            this.props.inventory.getPlaceInventoryDataResponseViewModel.placeHolidayDates.map(function (item, index) {
                var date = new Date(item.leaveDateTime);
                holidayDates.push(date);
            });
            var yesterday = new Date();
            yesterday.setDate(yesterday.getDate() - 1);
            var seasonTimeModel = this.props.inventory.getPlaceInventoryDataResponseViewModel.seasonTimeModel;
            var specialDayTimeModel = this.props.inventory.getPlaceInventoryDataResponseViewModel.specialDayModel;
            if (seasonTimeModel.length > 0) {
                seasonTimeModel.map(function (item) {
                    item.startDate = new Date(item.startDate).toDateString();
                    item.endDate = new Date(item.endDate).toDateString();
                });
                seasonTimeModel = seasonTimeModel
            }
            if (specialDayTimeModel.length > 0) {
                specialDayTimeModel.map(function (item) {
                    item.specialDate = new Date(item.specialDate).toDateString();
                });
                specialDayTimeModel = specialDayTimeModel
            }

            this.setState({
                selectedDays: holidayDates,
                isPerennialCalendar: (this.props.inventory.getPlaceInventoryDataResponseViewModel.event.eventTypeId == 1 ? false : true),
                startdatetime: (this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails.length > 0 ? new Date(this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails[0].startDateTime) : yesterday),
                enddatetime: (this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails.length > 0 ? new Date(this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails[0].endDateTime) : yesterday),
                isDefaultStateUpdated: true,
                seasonTimeModel: (seasonTimeModel.length > 0 ? seasonTimeModel : this.state.seasonTimeModel),
                specialDayModel: (specialDayTimeModel.length > 0 ? specialDayTimeModel : this.state.specialDayModel),
                isSeasonTimeAllow: (this.props.inventory.getPlaceInventoryDataResponseViewModel.seasonTimeModel.length > 0 ? true : false),
                isSpecialDayTimeAllow: (this.props.inventory.getPlaceInventoryDataResponseViewModel.specialDayModel.length > 0 ? true : false),
                weekOffDays: (this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.daysOpen.length > 0 ? this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.daysOpen : this.state.weekOffDays),
                customTimeModel: (this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.customTimeModel.length > 0 ? this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.customTimeModel : this.state.customTimeModel),
                isRegularCustomeTime: !this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.isSameTime,
                isRegularSameTime: this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.isSameTime,
                timingModel: (this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.timeModel.length > 0 ? this.props.inventory.getPlaceInventoryDataResponseViewModel.regularTimeModel.timeModel : this.state.timingModel),
                isDaysClick: true,
            }, function () { this.updateCalendarSummary(); });
        } else {
            this.setState({
                placeHolidayDate: [],
                weekOffDays: [],
                startdatetime: new Date(),
                enddatetime: new Date(),
                isPerennialCalendar: true,
                isHolidayCalendarShow: false,
                timingModel: [],
                totalTimings: 1,
                selectedDays: [],
                calenderAccordianEditComponent: <div></div>,
                isCalenderEdit: true,
                inventoryDetailAccordianEditComponent: <div></div>,
                isInventoryDetailEdit: false,
                isEdit: false,
                isAlertCall: false,
                isDaysClick: false,
                isRegularClick: false,
                isSeasonClick: false,
                isSpecialClick: false,
                isRegularCustomeTime: false,
                customTimeModel: [],
                seasonTimeModel: seasonModel,
                specialDayModel: specialDayMasterModel,
                isSeasonTimeAllow: true,
                isSpecialDayTimeAllow: true,
                isSeasonTouched: false,
                isSpecialDayTouched: false,
                isDefaultStateUpdated: true,
            })
        }
    }

    public enableField(e, index, isEnable) {
        this.setState({ [index]: isEnable });
    }

    @autobind
    public onCalendarCalcel() {
        if (!this.state.isEdit) {
            var placeTimings = [];
            timingId = 0;
            timingId = timingId + 1;
            var timingViewModel: timeViewModel = {
                id: timingId,
                from: "",
                to: ""
            }
            placeTimings.push(timingViewModel);
            this.setState({
                placeHolidayDate: [],
                weekOffDays: [],
                startdatetime: new Date(),
                enddatetime: new Date(),
                isPerennialCalendar: true,
                isHolidayCalendarShow: false,
                totalTimings: 1,
                selectedDays: [],
                calenderAccordianEditComponent: <div></div>,
                isCalenderEdit: true,
                inventoryDetailAccordianEditComponent: <div></div>,
                isInventoryDetailEdit: false,
                isEdit: false,
                isDefaultStateUpdated: false,
                timingModel: placeTimings,
            });

        }
    }

    @autobind
    onClickAddTimeForSpecificDay(item, e) {
        var data = this.state.customTimeModel;
        data.map(function (val) {
            if (val.id == item.id) {
                var TimeModel = {
                    "id": val.time.length + 1,
                    "from": "",
                    "to": "",
                };
                val.time.push(TimeModel);
            }
        });
        data.map(function (val) {
            val.time.map(function (currentTime, index) {
                currentTime.id = index + 1;
            })
        })
        this.setState({ customTimeModel: data }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeCurrentTimeForSpecificDay(item, currentTimeModel, timeType, e) {
        var data = this.state.customTimeModel;
        data.map(function (val) {
            if (val.id == item.id) {
                val.time.map(function (currentTime) {
                    if (currentTime.id == currentTimeModel.id) {
                        if (timeType == "fromTime") {
                            currentTime.from = e.target.value;
                        } else if (timeType = "toTime") {
                            currentTime.to = e.target.value;
                        }
                    }
                })
            }
        });
        this.setState({ customTimeModel: data }, function () { this.updateCalendarSummary(); });
    }


    @autobind
    onRemoveCurrentTimeForSpecificDay(item, currentTimeModel, e) {
        var data = this.state.customTimeModel;
        data.map(function (val) {
            if (val.id == item.id) {
                val.time.map(function (currentTime, index) {
                    if (currentTime.id == currentTimeModel.id) {
                        val.time.splice(index, 1);
                    }
                })
            }
        });
        data.map(function (val) {
            val.time.map(function (currentTime, index) {
                currentTime.id = index + 1;
            })
        })
        this.setState({ customTimeModel: data }, function () { this.updateCalendarSummary(); });
    }

    getNewSeasonModel() {
        var customTimeModels = [{
            "day": "",
            "time": [{
                "id": 0,
                "from": "",
                "to": "",
            }]
        }];
        var times = [{
            "id": 1,
            "from": "",
            "to": ""
        }];

        var newSeasonModel = {
            "id": 1,
            "name": "",
            "startDate": null,
            "endDate": null,
            "isSameTime": true,
            "sameTime": times,
            "daysOpen": [false, false, false, false, false, false, false, false],
            "time": customTimeModels
        };
        return newSeasonModel;
    }

    getNewSpecialDayModel() {
        var specialDayModel = {
            "id": 1,
            "name": "",
            "specialDate": null,
            "from": "",
            "to": ""
        };
        return specialDayModel;
    }

    @autobind
    onClickAddSeason() {
        var seasonState = this.state.seasonTimeModel;
        var copyOfMyArray = this.getNewSeasonModel();
        copyOfMyArray.id = seasonState.length + 1;
        seasonState.push(copyOfMyArray);
        seasonState.map(function (item, index) {
            item.id = index + 1;
        })
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onClickAddSpecailDay() {
        var specialDayState = this.state.specialDayModel;
        var copyOfMyArray = this.getNewSpecialDayModel();
        copyOfMyArray.id = specialDayState.length + 1;
        specialDayState.push(copyOfMyArray);
        specialDayState.map(function (item, index) {
            item.id = index + 1;
        })
        this.setState({ specialDayModel: specialDayState, isSpecialDayTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onClickSubSeasonTimeType(currentSeason, timeType, e) {
        var seasonState = this.state.seasonTimeModel;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                if (timeType == "same") {
                    item.isSameTime = true;
                } else if (timeType == "custom") {
                    item.isSameTime = false;
                }
            }
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onSeasonNameChange(currentSeason, e) {
        var seasonState = this.state.seasonTimeModel;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                item.name = e.target.value;
            }
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeSeasonDate(currentSeason, dateType, e) {
        var seasonState = this.state.seasonTimeModel;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                if (dateType == "start") {
                    item.startDate = e;
                } else if (dateType == "end") {
                    item.endDate = e;
                }
            }
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onClickAddSeasonSameTime(currentSeason, e) {
        var seasonState = this.state.seasonTimeModel;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                var data = {
                    "id": item.sameTime.length + 1,
                    "from": "",
                    "to": ""
                }
                item.sameTime.push(data);
            }
        });
        seasonState.map(function (item, index) {
            item.id = index + 1;
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeSeasonDopenDays(currentSeason, dayIndex, e) {
        var seasonState = this.state.seasonTimeModel;

        seasonState.map(function (item) {
            var seasonTime = item.time;
            if (item.id == currentSeason.id) {
                if (dayIndex == 0) {
                    var openday = [];
                    item.daysOpen.map(function (val) {
                        if (e.target.checked) {
                            val = true;
                            openday.push(true);
                        } else {
                            val = false;
                            openday.push(false);
                        }
                    });
                    item.daysOpen = openday;
                } else {
                    var openday = [];
                    item.daysOpen.map(function (val, index) {
                        if (index == 0) {
                            val = false;
                            openday.push(false);
                        }
                        else if (index == dayIndex && e.target.checked) {
                            val = true;
                            openday.push(true);
                        } else if (index == dayIndex && !e.target.checked) {
                            val = false;
                            openday.push(false);
                        } else {
                            openday.push(val);
                        }
                    });
                    var data = openday.filter(function (item) { return item == true });
                    if (data.length >= 7) {
                        openday[0] = true;
                    }
                    item.daysOpen = openday;
                }
                var daysTimeModel = [];

                daysInWeek.map(function (val, index) {
                    if (val != "All Days") {
                        var isDayExistsInSeason = openday[index];
                        var isDayExists = item.time.filter(function (currentDay) {
                            return currentDay.day == val
                        });
                        if (isDayExists.length > 0 && isDayExistsInSeason) {
                            daysTimeModel.push(isDayExists[0])
                        } else if (isDayExists.length == 0 && isDayExistsInSeason) {
                            var TimeModel = {
                                "day": val,
                                "id": index,
                                "time": [{
                                    "id": 0,
                                    "from": "",
                                    "to": "",
                                }]
                            };
                            daysTimeModel.push(TimeModel);
                        }
                    }
                });
                item.time = daysTimeModel;
            }
        });
        this.setState({ seasonTimeModel: seasonState }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeSeasonSameTime(currentTime, currentSeason, timeType, e) {
        var seasonState = this.state.seasonTimeModel;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                var allTimes = item.sameTime;
                allTimes.map(function (val) {
                    if (val.id == currentTime.id) {
                        if (timeType == "from") {
                            val.from = e.target.value;
                        } else if (timeType == "to") {
                            val.to = e.target.value;
                        }
                    }
                });
                item.sameTime = allTimes;
            }
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true, }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onAddSeasonSpecificDayTime(currentTime, currentSeason, e) {
        var seasonState = this.state.seasonTimeModel;
        var currentDay = currentTime;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                var currentDayTime = item.time;
                currentDayTime.map(function (val) {
                    if (val.id == currentDay.id) {
                        var time = {
                            "id": currentDay.time.length + 1,
                            "from": "",
                            "to": ""
                        }
                        val.time.push(time);
                    }
                    val.time.map(function (currentTime, currentTimeIndex) {
                        currentTime.id = currentTimeIndex + 1;
                    })
                });
                item.time = currentDayTime;
            }
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onRemoveSeasonTimeForSameDays(currentSeason, currentTime, e) {
        var seasonState = this.state.seasonTimeModel;
        var currentDay = currentTime;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                var currentDayTime = item.sameTime;
                currentDayTime.map(function (val, index) {
                    if (val.id == currentDay.id) {
                        currentDayTime.splice(index, 1);
                    }
                });
                currentDayTime.map(function (val, currentIndex) {
                    val.id = currentIndex + 1;
                })
                item.sameTime = currentDayTime;
            }
        });
        this.setState({ seasonTimeModel: seasonState }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onRemoveSeasonTimeForSpecificDays(currentDay, currentSeason, timeToRemove, e) {
        var seasonState = this.state.seasonTimeModel;
        var currentDay = currentDay;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                var currentDayModel = item.time;
                currentDayModel.map(function (val, currentDayIndex) {
                    if (val.id == currentDay.id) {
                        var currentDayTime = val.time;
                        currentDayTime.map(function (currentTime, currentTimeIndex) {
                            if (currentTime.id == timeToRemove.id) {
                                currentDayTime.splice(currentTimeIndex, 1);
                            }
                        });
                        currentDayTime.map(function (currentTime, currentTimeIndex) {
                            currentDayTime.id = currentTimeIndex + 1;
                        })
                        val.time = currentDayTime;
                    }
                });
                item.time = currentDayModel;
            }
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeSeasonTimeForSpecificDays(currentDay, currentSeason, timeType, timeToRemove, e) {
        var seasonState = this.state.seasonTimeModel;
        var currentDay = currentDay;
        seasonState.map(function (item) {
            if (item.id == currentSeason.id) {
                var currentDayModel = item.time;
                currentDayModel.map(function (val, currentDayIndex) {
                    if (val.id == currentDay.id) {
                        var currentDayTime = val.time;
                        currentDayTime.map(function (currentTime, currentTimeIndex) {
                            if (currentTime.id == timeToRemove.id) {
                                if (timeType == "from") {
                                    currentTime.from = e.target.value;
                                } else if (timeType == "to") {
                                    currentTime.to = e.target.value;
                                }
                            }
                        });
                        val.time = currentDayTime;
                    }
                });
                item.time = currentDayModel;
            }
        });
        this.setState({ seasonTimeModel: seasonState, isSeasonTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeSpecialDayName(currentDay, currentDayData, e) {
        var specialDayState = this.state.specialDayModel;
        specialDayState.map(function (item) {
            if (item.id == currentDay.id) {
                item.name = e.target.value;
            }
        });
        this.setState({ specialDayModel: specialDayState, isSpecialDayTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeSpecialDayDate(currentDay, currentDayData, e) {
        var specialDayState = this.state.specialDayModel;
        specialDayState.map(function (item) {
            if (item.id == currentDay.id) {
                item.specialDate = e;
            }
        });
        this.setState({ specialDayModel: specialDayState, isSpecialDayTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onChangeSpecialDayTime(currentDay, timeType, currentDayData, e) {
        var specialDayState = this.state.specialDayModel;
        specialDayState.map(function (item) {
            if (item.id == currentDay.id) {
                if (timeType == "from") {
                    item.from = e.target.value;
                } else if (timeType == "to") {
                    item.to = e.target.value;
                }

            }
        });
        this.setState({ specialDayModel: specialDayState, isSpecialDayTouched: true }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onClickSeasonTimexists() {
        this.setState({ isSeasonTimeAllow: true, isSeasonTouched: true }, function () { this.updateCalendarSummary(); })
    }

    @autobind
    onClickRemoveSeasonTimeExists() {
        this.setState({ isSeasonTimeAllow: false }, function () { this.updateCalendarSummary(); })
    }

    @autobind
    onClickSpecialDayTimexists() {
        this.setState({ isSpecialDayTimeAllow: true }, function () { this.updateCalendarSummary(); })
    }

    @autobind
    onClickRemoveSpecialDayTimexists() {
        this.setState({ isSpecialDayTimeAllow: false }, function () { this.updateCalendarSummary(); })
    }

    @autobind
    onClickRemoveSeason(currentSeason, that) {
        var seasonState = this.state.seasonTimeModel;
        var currentDay = currentDay;
        seasonState.map(function (item, index) {
            if (item.id == currentSeason.id) {
                seasonState.splice(index, 1);
            }
        });
        seasonState.map(function (item, index) {
            item.id = index + 1;
        });
        this.setState({ seasonTimeModel: seasonState }, function () { this.updateCalendarSummary(); });
    }

    @autobind
    onClickRemoveSpecialDay(currentSpecialDay, that) {
        var specialDayState = this.state.specialDayModel;
        var currentDay = currentSpecialDay;
        specialDayState.map(function (item, index) {
            if (item.id == currentDay.id) {
                specialDayState.splice(index, 1);
            }
        });
        specialDayState.map(function (item, index) {
            item.id = index + 1;
        });
        this.setState({ specialDayModel: specialDayState, isSpecialDayTouched: true }, function () { this.updateCalendarSummary(); });
    }

    renderDay = (date, modifiers) => {
        return date;
    };
    renderWeek = (weekNumber, week, month) => {
        return weekNumber;
    };

    public render() {
        var that = this;
        var timing = [];
        var customDaysTime;
        var seasonTime;
        var specialDyTime;
        var i = 0;
        for (i = 0; i < this.state.totalTimings; i++) {
            timing.push(i);
        }

        if (this.props.inventory.isInventoryDataSuccess && !this.state.isDefaultStateUpdated && this.state.isEdit) {
            this.updatedCalendarState();
        }

        if (this.props.placeCalendar.isShowSuccessAlert == true && !this.state.isAlertCall) {
            //alert("Calendar saved successfully");
            // this.notify();
            // this.setState({ isAlertCall: true });
        }

        var days;
        /* ------------------------------- Regular Time --------------------------------------------- */
        customDaysTime = that.state.customTimeModel.map(function (val, index) {

            var currentDayTime = val.time.map(function (currentTime, currentIndex) {
                return <div className="form-group row mb-0">
                    {(val.time.length > 1) &&
                        < div className="col-12">
                            <label>Time {currentIndex + 1}</label>
                            <a href="JavaScript:Void(0)" onClick={that.onRemoveCurrentTimeForSpecificDay.bind(that, val, currentTime)} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>
                        </div>
                    }
                    <div className="col-sm-6 mb-2 pr-1">
                        <label>From</label>
                        <div className="pb-1"><input type="time" value={currentTime.from} className="form-control" onChange={that.onChangeCurrentTimeForSpecificDay.bind(that, val, currentTime, "fromTime")} /></div>
                    </div>
                    <div className="col-sm-6 mb-2 pl-1">
                        <label>To</label>
                        <div className="pb-1"><input type="time" value={currentTime.to} className="form-control" onChange={that.onChangeCurrentTimeForSpecificDay.bind(that, val, currentTime, "toTime")} /></div>
                    </div>
                </div>
            });
            return <div>
                <div className="card">
                    <div className="card-header p-0" id={"headingDays" + index}>
                        <p className="mb-0 pr-3"><button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target={"#collapseCalendar" + index} aria-controls="collapseCalendar">{val.day}</button></p>
                    </div>
                    <div id={"collapseCalendar" + index} className="collapse" aria-labelledby={"headingDays" + index} data-parent="#accordionExampleTiming"  >
                        <div className="card-body">
                            {currentDayTime}
                            <div className="form-group" >
                                <a href="JavaScript:Void(0)" className="text-decoration-none btn-link mr-3" onClick={that.onClickAddTimeForSpecificDay.bind(that, val)}>
                                    <small>
                                        <i className="fa fa-plus mr-2" aria-hidden="true" ></i>Add additional timing
                                        </small>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="line"></div>
            </div>
        });

        /*------------------------------------- Season Time ---------------------------------------------- */

        seasonTime = that.state.seasonTimeModel.map(function (val, index) {
            var currentVal = val;

            var seasonDaysTime = val.time.map(function (item, currentDayIndex) {
                var currentDayTime = item.time.map(function (currentTime, currentIndex) {
                    return <div className="form-group row mb-0">
                        {(item.time.length > 1) &&
                            < div className="col-12">
                                <label>Time {currentIndex + 1}</label>
                                <a href="JavaScript:Void(0)" onClick={that.onRemoveSeasonTimeForSpecificDays.bind(that, item, val, currentTime)} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>
                            </div>
                        }
                        <div className="col-sm-6 mb-2 pr-1">
                            <label>From</label>
                            <div className="pb-1"><input type="time" value={currentTime.from} onChange={that.onChangeSeasonTimeForSpecificDays.bind(that, item, val, "from", currentTime)} className="form-control" /></div>
                        </div>
                        <div className="col-sm-6 mb-2 pl-1">
                            <label>To</label>
                            <div className="pb-1"><input type="time" value={currentTime.to} onChange={that.onChangeSeasonTimeForSpecificDays.bind(that, item, val, "to", currentTime)} className="form-control" /></div>
                        </div>
                    </div>
                });

                return <div className="card">
                    <div className="card-header p-0" id={"headingSeasonDays" + currentDayIndex}>
                        <p className="mb-0 pr-3"><button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target={"#collapseSeasonDayTime" + currentDayIndex} aria-controls="collapseSeasonDayTime">{item.day}</button></p>

                    </div>
                    <div id={"collapseSeasonDayTime" + currentDayIndex} className="collapse" aria-labelledby={"headingSeasonDays" + currentDayIndex} data-parent="#accordionExampleSeasonDateTiming"  >
                        <div className="card-body">
                            {currentDayTime}
                            <div className="form-group" >
                                <a href="JavaScript:Void(0)" className="text-decoration-none btn-link mr-3" onClick={that.onAddSeasonSpecificDayTime.bind(that, item, val)} >
                                    <small>
                                        <i className="fa fa-plus mr-2" aria-hidden="true" ></i>Add additional timing
                                                             </small>
                                </a>
                            </div>
                        </div>
                    </div>
                    <div className="line"></div>
                </div>
            });

            var seasonOpenDays = daysInWeek.map(function (currentDay, currentIndex) {
                var isChecked = val.daysOpen[currentIndex];
                var uniqId = (index + 1) + (currentIndex + 2);
                var random = Math.floor(Math.random() * (+1 - +1000)) + +1;
                return <span className="btn btn-outline-primary">
                    <div className="custom-control custom-checkbox">
                        <input type="checkbox" checked={isChecked} id={"check" + random} onChange={that.onChangeSeasonDopenDays.bind(that, val, currentIndex)} name={currentDay} value={currentDay} className="custom-control-input" />
                        <label className="custom-control-label" htmlFor={"check" + random}>{currentDay}</label>
                    </div>
                </span>
            });

            var sameTime = val.sameTime.map(function (currentTime, currentIndex) {
                var isChecked = val.daysOpen[currentIndex];
                var uniqId = (index + 1) + (currentIndex + 2);
                var random = Math.floor(Math.random() * (+1 - +1000)) + +1;
                return <div className="row">
                    <div className="col-sm-12"> <label>Time</label></div>

                    {(val.sameTime.length > 1) &&
                        < div className="col-12">
                            <label>Time {currentIndex + 1}</label>
                            <a href="JavaScript:Void(0)" onClick={that.onRemoveSeasonTimeForSameDays.bind(that, val, currentTime)} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>
                        </div>
                    }

                    <div className="col">
                        <label>From</label>
                        <div className="pb-1">
                            <input type="time" onChange={that.onChangeSeasonSameTime.bind(that, currentTime, val, "from")} value={currentTime.from} className="form-control" />
                        </div>
                    </div>
                    <div className="col">
                        <label>To</label>
                        <div className="pb-1">
                            <input type="time" onChange={that.onChangeSeasonSameTime.bind(that, currentTime, val, "to")} value={currentTime.to} className="form-control" />
                        </div>
                    </div>
                </div>
            });


            return <div>
                <div>
                    <div className="card">
                        <div className="card-header p-0" id={"headingSeason" + index}>
                            <p className="mb-0 pr-3"><button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target={"#collapseSeasonCalendar" + index} aria-controls="collapseSeasonCalendar">{val.name == "" ? "Season Name" : val.name}</button></p>
                            {(that.state.seasonTimeModel.length > 1) && <a href="JavaScript:Void(0)" onClick={that.onClickRemoveSeason.bind(that, val)} className="text-decoration-none btn-link text-danger float-right mt-2"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}
                        </div>
                        <div id={"collapseSeasonCalendar" + index} className="collapse" aria-labelledby={"headingSeason" + index} data-parent="#accordionExampleSeasonTiming"  >
                            <div className="card-body">
                                < div className="row">
                                    <div className="col-sm-3 mb-1 mb-sm-0">
                                            <input type="text" className="form-control" placeholder="Name of season" value={val.name} onChange={that.onSeasonNameChange.bind(that, val)} />
                                    </div>
                                    <div className="col-sm-4 mb-1 mb-sm-0">
                                            <Datetime inputProps={{ placeholder: 'Select season start date', }} onChange={that.onChangeSeasonDate.bind(that, currentVal, "start")} value={val.startDate} dateFormat="YYYY-MM-DD" timeFormat={true} />
                                    </div>
                                    <div className="col-sm-4 mb-1 mb-sm-0">
                                            <Datetime inputProps={{ placeholder: 'Select season end date', }} onChange={that.onChangeSeasonDate.bind(that, currentVal, "end")} value={val.endDate} dateFormat="YYYY-MM-DD" timeFormat={true} />
                                    </div>
                                </div>
                                <div className="form-group mt-4 ">
                                    <label className="d-block">Days open in a week <span className="text-danger">*</span> </label>
                                    <div className="btn-group days-open" role="group" aria-label="Basic example">
                                        {seasonOpenDays}
                                    </div>
                                </div>
                                <div>
                                    <label className="d-block">Timing <span className="text-danger">*</span> </label>
                                    < div className="row">
                                        <div className="col-sm-3">
                                            <div className="custom-control custom-checkbox" >
                                                <input type="checkbox" checked={val.isSameTime} onChange={that.onClickSubSeasonTimeType.bind(that, currentVal, "same")} id={"same" + currentVal.id.toString()} className="custom-control-input" value="Same time for all day" />
                                                <label className="custom-control-label" htmlFor={"same" + currentVal.id.toString()} >Same for all day</label>
                                            </div>
                                        </div>
                                        <div className="col-sm-3">
                                            <div className="custom-control custom-checkbox" >
                                                <input type="checkbox" checked={!val.isSameTime} onChange={that.onClickSubSeasonTimeType.bind(that, currentVal, "custom")} id={"custom" + currentVal.id.toString()} className="custom-control-input" value="Specific Day Time" />
                                                <label className="custom-control-label" htmlFor={"custom" + currentVal.id.toString()} >Specific day time</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                {(val.isSameTime) && <div>
                                    {sameTime}
                                    <div className="form-group">
                                        <a href="JavaScript:Void(0)" onClick={that.onClickAddSeasonSameTime.bind(that, val)} className="text-decoration-none btn-link mr-3"> <small><i className="fa fa-plus mr-2" aria-hidden="true"></i>Add additional timing</small> </a>
                                    </div>
                                </div>}
                                {(!val.isSameTime) && <div className="accordion border-bottom" id="accordionExampleSeasonDateTiming">
                                    {seasonDaysTime}
                                </div>}
                            </div>
                        </div>
                    </div>
                </div>
                <div className="line"></div>
            </div >
        });

        /* ------------------------------- Special Day Time --------------------------------------------- */
        specialDyTime = that.state.specialDayModel.map(function (val, index) {
            return <div>
                <div className="card">
                    <div className="card-header p-0" id={"headingSpecialDays" + index}>
                        <p className="mb-0 pr-3"><button className="btn btn-link collapsed" type="button" data-toggle="collapse" data-target={"#collapseSpecialDay" + index} aria-controls="collapseSpecialDay">{val.name == "" ? "Special day name" : val.name}</button></p>
                        {(that.state.specialDayModel.length > 1) && <a href="JavaScript:Void(0)" onClick={that.onClickRemoveSpecialDay.bind(that, val)} className="text-decoration-none btn-link text-danger float-right mt-2"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}
                    </div>
                    <div id={"collapseSpecialDay" + index} className="collapse" aria-labelledby={"headingSpecialDays" + index} data-parent="#accordionExampleSpecialTiming"  >
                        <div className="card-body">
                            < div className="row">
                                <div className="col-sm-3">
                                    <div className="custom-control" >
                                        <input type="text" className="form-control" placeholder="Day Name" onChange={that.onChangeSpecialDayName.bind(val, val, that)} value={val.name} />
                                    </div>
                                </div>
                                <div className="col-sm-3">
                                    <div className="custom-control"  >
                                        <Datetime inputProps={{ placeholder: 'Select special day', }} value={val.specialDate} onChange={that.onChangeSpecialDayDate.bind(val, val, that)} dateFormat="YYYY-MM-DD" timeFormat={true} />
                                    </div>
                                </div>
                                <div className="col-sm-3">
                                    <div className="pb-1">
                                        <input type="time" value={val.from} onChange={that.onChangeSpecialDayTime.bind(val, val, "from", that)} className="form-control" />
                                    </div>
                                </div>
                                <div className="col-sm-3">
                                    <div className="pb-1">
                                        <input type="time" value={val.to} onChange={that.onChangeSpecialDayTime.bind(val, val, "to", that)} className="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="line"></div>
            </div >
        });
        if (!this.state.isEdit) {
            days = daysInWeek.map(function (val, index) {
                i = i + 1;
                var isChecked = that.state.weekOffDays[index];

                return <span className="btn btn-outline-primary">
                    <div className="custom-control custom-checkbox">
                        <input type="checkbox" checked={isChecked} id={"customCheck" + i} onChange={that.onDaysSelect.bind(that, val)} name={val} value={val} className="custom-control-input" />
                        <label className="custom-control-label" htmlFor={"customCheck" + i}>{val}</label>
                    </div>
                </span>
            });

        } else if (this.state.isEdit && (that.state.weekOffDays.length > 0 || this.props.inventory.getPlaceInventoryDataResponseViewModel.placeWeekOffs == undefined)) {
            days = daysInWeek.map(function (val, index) {
                i = i + 1;;
                var isChecked = that.state.weekOffDays[index];
                return <span className="btn btn-outline-primary">
                    <div className="custom-control custom-checkbox">
                        <input checked={isChecked} type="checkbox" id={"customCheck" + i} onChange={that.onDaysSelect.bind(that, val)} name={val} value={val} className="custom-control-input" />
                        <label className="custom-control-label" htmlFor={"customCheck" + i}>{val}</label>
                    </div>
                </span>
            });
        }

        var i = 0;
        var timings = this.state.timingModel.map(function (item) {
            i = i + 1;
            return <div>

                {(!that.state.isEdit) && <div className="row">
                    {(that.state.timingModel.length > 1) && < div className="col-12"><label>Time {i}</label>{(that.state.timingModel.length > 1) && <a href="JavaScript:Void(0)" onClick={that.onRemoveTiming.bind(that, i - 1)} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}</div>}
                    <div className="col">
                        <label>From</label>
                        <div className="pb-1">
                            <input type="time" value={item.from} ref={element => that.fromTime = element} onChange={that.onValueChange.bind(that, item.id, "fromTime")} className="form-control" />
                        </div>
                    </div>
                    <div className="col">
                        <label>To</label>
                        <div className="pb-1">
                            <input type="time" value={item.to} ref={element => that.toTime = element} onChange={that.onValueChange.bind(that, item.id, "toTime")} className="form-control" />
                        </div>
                    </div>
                </div>}

                {(that.state.isEdit) && < div className="row" >
                    {(that.state.timingModel.length > 1) && < div className="col-12"><label>Time {i}</label>{(that.state.timingModel.length > 1) && <a href="JavaScript:Void(0)" onClick={that.onRemoveTiming.bind(that, i - 1)} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}</div>}
                    <div className="col">
                        <div className="form-group">
                            <label>From</label>
                            <div className="input-group">
                                <input disabled={(that.state["From" + item.id] == undefined || that.state["From" + item.id] == false) ? true : false} type="time" value={item.from} ref={element => that.fromTime = element} onChange={that.onValueChange.bind(that, item.id, "fromTime")} className="form-control" />
                                {((that.state["From" + item.id] == undefined || that.state["From" + item.id] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "From" + item.id, true)}>
                                    <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                                </div>}
                                {((that.state["From" + item.id] == true) ? true : false) && <div className="input-group-append border-success" onClick={() => that.enableField(that, "From" + item.id, false)}>
                                    <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                                </div>}
                            </div>
                        </div>
                    </div>

                    <div className="col">
                        <div className="form-group">
                            <label>To</label>
                            <div className="input-group">
                                <input disabled={(that.state["To" + item.id] == undefined || that.state["To" + item.id] == false) ? true : false} type="time" value={item.to} ref={element => that.toTime = element} onChange={that.onValueChange.bind(that, item.id, "toTime")} className="form-control" />
                                {((that.state["To" + item.id] == undefined || that.state["To" + item.id] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "To" + item.id, true)}>
                                    <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                                </div>}
                                {((that.state["To" + item.id] == true) ? true : false) && <div className="input-group-append border-success" onClick={() => that.enableField(that, "To" + item.id, false)}>
                                    <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                                </div>}
                            </div>
                        </div>
                    </div>
                </div>
                }</div>
        });

        return <div>
            {(this.props.placeCalendar.isShowSuccessAlert) && <ToastContainer
                position="top-center"
                autoClose={6000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
            />}
            <div>
                {(!this.props.placeCalendar.isCalendarSaveRequest) && <div className="col-sm-12">
                    <a className="place-listing active" data-toggle="collapse" href="#PlaceCalendar" role="button" aria-expanded="false" aria-controls="PlaceCalendar">
                        <span className="rounded-circle border border-primary place-listing">1</span>Inventory Calendar</a>
                    <div className="collapse multi-collapse show pt-3" id="PlaceCalendar">
                        <div className="mb-4">
                            <label className="d-block">Calendar Type<span className="text-danger">*</span> </label>
                            <div className="custom-control custom-radio custom-control-inline">
                                <input type="radio" id="customRadioInline1" name="customRadioInline1" onClick={this.onClickPerennialCalendarType} checked={this.state.isPerennialCalendar} className="custom-control-input" />
                                <label className="custom-control-label" htmlFor="customRadioInline1">Perennial</label>
                            </div>
                            <div className="custom-control custom-radio custom-control-inline">
                                <input type="radio" id="customRadioInline2" name="customRadioInline1" onClick={this.onClickRegularCalendarType} checked={!this.state.isPerennialCalendar} className="custom-control-input" />
                                <label className="custom-control-label" htmlFor="customRadioInline2">Regular</label>
                            </div>
                        </div>
                        {(summary != undefined) && < table className="table table-bordered table-condensed m-0">
                            <thead>
                                <tr>
                                    <th className="text-center">Time Type</th>
                                    <th className="text-center">Days Open</th>
                                    <th className="text-center">Time</th>
                                    <th className="text-center">Remove</th>
                                </tr>
                            </thead>
                            <tbody>
                                {summary}
                            </tbody>
                        </table>}
                        {(!this.state.isPerennialCalendar && this.state.isEdit) && <div>

                            <div className="row">
                                <div className="form-group mb-2 position-relative col-12 col-sm-6">
                                    <label>From:</label>
                                    <div className="input-group">
                        <Datetime inputProps={{ placeholder: 'Select place start date & time', disabled: (that.state["fromdate"] == undefined || that.state["fromdate"] == false) ? true : false }} onChange={this.OnChangeStartDatetime} value={this.state.startdatetime} dateFormat="YYYY-MM-DD" timeFormat={true} />
                                        {((that.state["fromdate"] == undefined || that.state["fromdate"] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "fromdate", true)}>
                                            <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                                        </div>}
                                        {((that.state["fromdate"] == true) ? true : false) && <div className="input-group-append border-success" onClick={() => that.enableField(that, "fromdate", false)}>
                                            <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                                        </div>}
                                    </div>
                                </div>

                                <div className="form-group mb-2 position-relative col-12 col-sm-6">
                                    <label>To:</label>
                                    <div className="input-group">
                                        <Datetime inputProps={{ placeholder: 'Select place end date & time', disabled: (that.state["todate"] == undefined || that.state["todate"] == false) ? true : false }} onChange={this.OnChangeEndtime} value={this.state.enddatetime} dateFormat="YYYY-MM-DD" timeFormat={true} />
                                        {((that.state["todate"] == undefined || that.state["todate"] == false) ? true : false) && <div className="input-group-append" onClick={() => that.enableField(that, "todate", true)}>
                                            <span className="input-group-text"><i className="fa fa-pencil" aria-hidden="true"></i></span>
                                        </div>}
                                        {((that.state["todate"] == true) ? true : false) && <div className="input-group-append border-success" onClick={() => that.enableField(that, "todate", false)}>
                                            <span className="input-group-text bg-success c-pointer"><i className="fa fa-check text-white" aria-hidden="true"></i></span>
                                        </div>}
                                    </div>
                                </div>
                            </div>
                        </div>}

                        {(!this.state.isPerennialCalendar && !this.state.isEdit) && <div>
                            <div className="row">
                                <div className="form-group mb-2 position-relative col-12 col-sm-6">
                                    <label>From:</label>
                                    <Datetime inputProps={{ placeholder: 'Select place start date & time', }} onChange={this.OnChangeStartDatetime} value={this.state.startdatetime} dateFormat="YYYY-MM-DD" timeFormat={true} isValidDate={function (current){ return current.isAfter(moment().subtract(1, 'days')) }}/>
                                </div>

                                <div className="form-group mb-2 position-relative col-12 col-sm-6">
                                    <label>To:</label>
                                    <Datetime inputProps={{ placeholder: 'Select place end date & time' }} onChange={this.OnChangeEndtime} value={this.state.enddatetime} dateFormat="YYYY-MM-DD" timeFormat={true} isValidDate={function (current){ return current.isAfter(moment().subtract(1, 'days')) }}/>
                                </div>
                            </div>

                        </div>}
                        <div className="mb-4">
                            <label className="d-block">Timing Types<span className="text-danger">*</span> </label>
                            < div className="row">
                                <div className="col-sm-2">
                                    <div className="custom-control custom-checkbox" >
                                        <input type="checkbox" id="regular" checked={this.state.isRegularClick} className="custom-control-input" onChange={that.onClickTimeType.bind(that, "regular")} value="Regular Time" />
                                        <label className="custom-control-label" htmlFor="regular" >Regular</label></div>
                                </div>
                                <div className="col-sm-2">
                                    <div className="custom-control custom-checkbox" >
                                        <input type="checkbox" id="season" checked={this.state.isSeasonClick} className="custom-control-input" onChange={that.onClickTimeType.bind(that, "season")} value="Season Time" />
                                        <label className="custom-control-label" htmlFor="season">Season</label></div>
                                </div>
                                <div className="col-sm-3">
                                    <div className="custom-control custom-checkbox" >
                                        <input type="checkbox" id="special" checked={this.state.isSpecialClick} className="custom-control-input" onChange={that.onClickTimeType.bind(that, "special")} value="Special day/ Holiday time" />
                                        <label className="custom-control-label" htmlFor="special" >Holiday/Special day</label></div></div>
                            </div>
                        </div>

                        {(this.state.isRegularClick) &&
                            <div>
                                <div className="form-group mt-4 table-responsive">
                                    <label className="d-block">Days open in a week <span className="text-danger">*</span> </label>
                                    <div className="btn-group text-nowrap" role="group" aria-label="Basic example">
                                        {days}
                                    </div>
                                </div>
                                <label className="d-block">Timing <span className="text-danger">*</span> </label>
                                < div className="row">
                                    <div className="col-sm-2">
                                        <div className="custom-control custom-checkbox" >
                                            <input type="checkbox" checked={this.state.isRegularSameTime} onChange={that.onClickSubRegularTimeType.bind(that, "same")} id="sameRegular" className="custom-control-input" value="Same time for all day" />
                                            <label className="custom-control-label" htmlFor="sameRegular" >Same for all days</label>
                                        </div>
                                    </div>
                                    <div className="col-sm-2">
                                        <div className="custom-control custom-checkbox" >
                                            <input type="checkbox" checked={this.state.isRegularCustomeTime} onChange={that.onClickSubRegularTimeType.bind(that, "custom")} id="customeRegular" className="custom-control-input" value="Specific Days Time" />
                                            <label className="custom-control-label" htmlFor="customeRegular" >Specific day time</label>
                                        </div>
                                    </div>
                                </div>

                            </div>}

                        {(this.state.isRegularSameTime && this.state.isRegularClick && this.state.isDaysClick) && < div className="form-group mt-4">
                            <label className="d-block">Timings <span className="text-danger">*</span> </label>
                            {timings}

                            <div className="form-group">
                                <a href="JavaScript:Void(0)" onClick={this.onClickAddTiming} className="text-decoration-none btn-link mr-3"> <small><i className="fa fa-plus mr-2" aria-hidden="true"></i>Add additional timing</small> </a>
                            </div>

                        </div>}

                        {(this.state.isRegularCustomeTime && this.state.isRegularClick && this.state.isDaysClick) && <div className="accordion border-bottom" id="accordionExampleTiming">
                            {customDaysTime}
                        </div>}

                        {(this.state.isSeasonClick) &&
                            <div>
                                <div className="form-group">
                                    <label className="d-block">Add Season Time <span className="text-danger">*</span> </label>
                                    <div className="custom-control custom-radio custom-control-inline">
                                        <input type="radio" id="seasonYes" name="seasonYes" onClick={this.onClickSeasonTimexists} checked={this.state.isSeasonTimeAllow} className="custom-control-input" />
                                        <label className="custom-control-label" htmlFor="seasonYes">Yes</label>
                                    </div>
                                    <div className="custom-control custom-radio custom-control-inline">
                                        <input type="radio" id="seasonNO" name="seasonNO" onClick={this.onClickRemoveSeasonTimeExists} checked={!this.state.isSeasonTimeAllow} className="custom-control-input" />
                                        <label className="custom-control-label" htmlFor="seasonNO">No</label>
                                    </div>
                                </div>
                                {(that.state.isSeasonTimeAllow) && <div>
                                    <label className="d-block">Season Type<span className="text-danger">*</span> </label>
                                    <div className="accordion border-bottom" id="accordionExampleSeasonTiming">
                                        {seasonTime}
                                    </div>
                                    <div className="form-group" >
                                        <a href="JavaScript:Void(0)" onClick={this.onClickAddSeason} className="text-decoration-none btn-link mr-3">
                                            <small>
                                                <i className="fa fa-plus mr-2" aria-hidden="true" ></i>Add Season
                                        </small>
                                        </a>
                                    </div>
                                </div>}
                            </div>}

                        {(this.state.isSpecialClick) &&
                            <div>
                                <div className="form-group">
                                    <label className="d-block mb-4">Special Day/Holiday Time <span className="text-danger">*</span> </label>
                                    <div className="custom-control custom-radio custom-control-inline">
                                        <input type="radio" id="SpecialDayYes" name="SpecialDayYes" onClick={this.onClickSpecialDayTimexists} checked={this.state.isSpecialDayTimeAllow} className="custom-control-input" />
                                        <label className="custom-control-label" htmlFor="SpecialDayYes">Yes</label>
                                    </div>
                                    <div className="custom-control custom-radio custom-control-inline">
                                        <input type="radio" id="SpecialDayNo" name="SpecialDayNo" onClick={this.onClickRemoveSpecialDayTimexists} checked={!this.state.isSpecialDayTimeAllow} className="custom-control-input" />
                                        <label className="custom-control-label" htmlFor="SpecialDayNo">No</label>
                                    </div>
                                </div>
                                {(this.state.isSpecialDayTimeAllow) && < div >
                                    <div className="accordion border-bottom" id="accordionExampleSpecialTiming">
                                        {specialDyTime}
                                    </div>
                                    <div className="form-group" >
                                        <a href="JavaScript:Void(0)" onClick={this.onClickAddSpecailDay} className="text-decoration-none btn-link mr-3">
                                            <small>
                                                <i className="fa fa-plus mr-2" aria-hidden="true" ></i>Add special day
                                        </small>
                                        </a>
                                    </div>
                                </div>}
                            </div>}

                        <div className="form-group">
                            <a href="JavaScript:Void(0)" className="text-decoration-none btn-link" onClick={this.onClickShowHolidayCalendar}><i className="fa fa-calendar-check-o mr-2" aria-hidden="true"></i>Holidays/Days Closed Calendar</a>
                        </div>
                        {(this.state.isHolidayCalendarShow) && <div>
                            <DayPicker
                                selectedDays={this.state.selectedDays}
                                onDayClick={this.handleDayClick}
                            />
                        </div>}
                        <div className="text-center pb-4">
                            <form onSubmit={this.onSubmitPlaceCalendar.bind(this)}>
                                <a href="JavaScript:Void(0)" onClick={this.onCalendarCalcel} className="text-decoration-none mr-4">Cancel</a>
                                <button id="calenderSubmitBtn" type="submit" className="btn btn-outline-primary">{(this.state.isEdit) ? "Update" : "Submit"}</button>
                            </form>
                        </div>
                    </div>
                </div>}
                {(this.props.placeCalendar.isCalendarSaveRequest) && <Newloader />}

                <div className="line"></div>

                <Inventory
                    deliveryType={this.props.currencyType.deliveryTypes}
                    documentType={this.props.currencyType.documentTypes}
                    currencyType={this.props.currencyType.currencyTypes}
                    refundPolicies={this.props.currencyType.refundPolicies}
                    allInventoryStore={this.props.inventory}
                    handleEditButtonPressedInventory={this.handleEditButtonPressedInventory}
                    handleEditSaveButtonPressedInventory={this.handleEditSaveButtonPressedInventory}
                    handleEditRevertButtonPressedInventory={this.handleEditRevertButtonPressedInventory}
                    onSubmitInventoryData={this.onSubmitInventory}
                    onSubmitSaveNewCustomerIdType={this.onSubmitNewCustomerIdType}
                    placeCalendarResponse={this.props.placeCalendar.placeCalendarResponse}
                    disabled={this.state.isInventoryDetailEdit}
                    inventoryData={this.props.inventory.getPlaceInventoryDataResponseViewModel}
                    isEdit={this.state.isEdit}
                    placeAltId={this.state.placeAltId}
                    ticketCategoryType={this.props.inventory.ticketCategoryTypeResponse}
                    customerInformationControls={this.props.currencyType.customerInformationControl}
                />
                <div className="line"></div>
            </div>

        </div>

    }

    @autobind
    public onSubmitPlaceCalendar(e) {
        e.preventDefault();
        var state = this.state;
        var isRegularOpendaysExists = true;
        var isRegularSameTimeExists = true;
        var isRegularCustomeTime = true;
        var isRegularCustomTimeExists = true;
        var isSeasonStartDateExists = true;
        var isSeasonEndDateExists = true;
        var isSeasonNameExists = true;
        var isSeasonSameTimeExists = true;
        var isSeasonSpecialTimeExists = true;
        var isSeasonDaysOpen = true;
        var isSpecialDayNameExists = true;
        var isSpecailDayDateExists = true;
        var isSpecialDayTimeExists = true;
        var specialDayName = "";
        var seasonName = "";
        var seasonDay = "";
        var regularDay = "";

      var checkValidTime = this.state.timingModel.filter(tm => tm.from === tm.to);
        if (checkValidTime.length > 0) {
          alert("Both times should not be the same. Please enter valid time.");
          return;
        }
        if (this.state.isSeasonTimeAllow && this.state.isSeasonTouched) {
            for (var i = 0; i < this.state.seasonTimeModel.length; i++) {
                seasonName = this.state.seasonTimeModel[i].name;
                var isDaysExists = this.state.seasonTimeModel[i].daysOpen.filter(function (item) {
                    return item == true;
                })
                if (this.state.seasonTimeModel[i].name == "") {
                    isSeasonNameExists = false;
                    break;
                }
                else if (this.state.seasonTimeModel[i].startDate == null) {
                    isSeasonStartDateExists = false;
                    break;
                }
                else if (this.state.seasonTimeModel[i].endDate == null) {
                    isSeasonEndDateExists = false;
                    break;
                } else if (isDaysExists.length == 0) {
                    isSeasonDaysOpen = false;
                    break;
                }
                else if (this.state.seasonTimeModel[i].isSameTime) {
                    for (var j = 0; j < this.state.seasonTimeModel[i].sameTime.length; j++) {
                        if (this.state.seasonTimeModel[i].sameTime[j].from == "" || this.state.seasonTimeModel[i].sameTime[j].to == "") {
                            isSeasonSameTimeExists = false;
                            break;
                        }
                    }
                }
                else if (!this.state.seasonTimeModel[i].isSameTime) {
                    for (var j = 0; j < this.state.seasonTimeModel[i].time.length; j++) {
                        seasonDay = this.state.seasonTimeModel[i].time[j].day;
                        for (var k = 0; k < this.state.seasonTimeModel[i].time[j].time.length; k++) {
                            if ((this.state.seasonTimeModel[i].time[j].time[k].from == "" || this.state.seasonTimeModel[i].time[j].time[k].to == "") && this.state.seasonTimeModel[i].time[j].time[k].day != "All Days") {
                                isSeasonSpecialTimeExists = false;
                                break;
                            }
                        }
                        if (!isSeasonSpecialTimeExists) {
                            break;
                        }
                    }
                }

            }
        }
        if (this.state.isSpecialDayTimeAllow && this.state.isSpecialDayTouched) {
            for (var i = 0; i < this.state.specialDayModel.length; i++) {
                specialDayName = this.state.specialDayModel[i].name;
                if (this.state.specialDayModel[i].name == "") {
                    isSpecialDayNameExists = false;
                    break;
                } else if (this.state.specialDayModel[i].specialDate == null) {
                    isSpecailDayDateExists = false;
                    break;
                } else if (this.state.specialDayModel[i].from == "" || this.state.specialDayModel[i].to == "") {
                    isSpecialDayTimeExists = false;
                    break;
                }
            }
        }
        var weekOffDays = this.state.weekOffDays.filter(function (item) {
            return item == true;
        });
        if (weekOffDays.length == 0) {
            isRegularOpendaysExists = false;
        }
        if (isRegularOpendaysExists) {
            if (!this.state.isRegularCustomeTime) {
                for (var i = 0; i < this.state.timingModel.length; i++) {
                    if (this.state.timingModel[i].from == "" || this.state.timingModel[i].to == "") {
                        isRegularSameTimeExists = false;
                        break;
                    }
                }
            }
            if (this.state.isRegularCustomeTime) {
                for (var i = 0; i < this.state.customTimeModel.length; i++) {
                    regularDay = this.state.customTimeModel[i].day;
                    for (var j = 0; j < this.state.customTimeModel[i].time.length; j++) {
                        if (this.state.customTimeModel[i].time[j].from == "" || this.state.customTimeModel[i].time[j].to == "") {
                            isRegularCustomTimeExists = false;
                            break;
                        }
                    }
                    if (!isRegularCustomTimeExists) {
                        break;
                    }
                }
            }
        }
        if (!this.state.isSeasonTouched && !isRegularOpendaysExists && !this.state.isSpecialDayTouched) {
            alert("Oops, looks like you missed something! Please go back and make sure you've selected time type.");
        } else if (isRegularOpendaysExists && !isRegularSameTimeExists && !this.state.isRegularCustomeTime) {
            alert("Oops, looks like you missed something! Please go back and make sure you've entered all timings under regular time type");
        } else if (isRegularOpendaysExists && !isRegularCustomTimeExists && this.state.isRegularCustomeTime) {
            alert("Oops, looks like you missed something! Please go back and make sure you've entered timings of " + regularDay + " under regular time type");
        } else if (!isSeasonNameExists) {
            alert("Please add season name");
        } else if (!isSeasonStartDateExists) {
            alert("Please select start date of the season " + seasonName);
        } else if (!isSeasonEndDateExists) {
            alert("Please select season end date of the season " + seasonName);
        } else if (!isSeasonDaysOpen) {
            alert("Please select open days in the season " + seasonName);
        } else if (!isSeasonSameTimeExists) {
            alert("Oops, looks like you missed something! Please go back and make sure you've entered season time of the " + seasonName);
        } else if (!isSeasonSpecialTimeExists) {
            alert("Oops, looks like you missed something! Please go back and make sure you've entered season time of " + seasonDay + " for the season " + seasonName);
        } else if (!isSpecialDayNameExists) {
            alert("Please add special day name");
        } else if (!isSpecailDayDateExists) {
            alert("Please select date of the  " + specialDayName + " under special/Holiday day");
        } else if (!isSpecialDayTimeExists) {
            alert("Oops, looks like you missed something! Please go back and make sure you've entered time of the " + specialDayName + " under special/Holiday day");
        } else {
            var placeType = 2; // Perennial
            if (!this.state.isPerennialCalendar) {
                placeType = 1; // Regular
            }

            var placeAltId = "4A4A848E-B9F0-4B68-8426-48138EEC51CC";
            if (localStorage.getItem("placeAltId") != null) {
                placeAltId = localStorage.getItem("placeAltId");
            } if (this.state.isEdit) {
                placeAltId = this.state.placeAltId;
            }

            var seasonModel = [];
            var specialDayModel = [];
            var regularModel: regularViewModel = {
                isSameTime: this.state.isRegularSameTime,
                timeModel: this.state.timingModel,
                customTimeModel: this.state.customTimeModel,
                daysOpen: this.state.weekOffDays
            };

            if (this.state.isSeasonTimeAllow && (this.state.isSeasonTouched || this.state.isEdit)) {
                for (var i = 0; i < this.state.seasonTimeModel.length; i++) {
                    var seasonModelData: seasonViewModel = {
                        daysOpen: this.state.seasonTimeModel[i].daysOpen,
                        endDate: this.state.seasonTimeModel[i].endDate,
                        isSameTime: this.state.seasonTimeModel[i].isSameTime,
                        name: this.state.seasonTimeModel[i].name,
                        sameTime: this.state.seasonTimeModel[i].sameTime,
                        startDate: this.state.seasonTimeModel[i].startDate,
                        time: this.state.seasonTimeModel[i].time,
                        id: this.state.seasonTimeModel[i].id
                    };
                    seasonModel.push(seasonModelData);
                }
            }
            if (this.state.isSpecialDayTimeAllow && (this.state.isSpecialDayTouched || this.state.isEdit)) {
                for (var i = 0; i < this.state.specialDayModel.length; i++) {
                    var specialDayViewModel: specialDayViewModel = {
                        from: this.state.specialDayModel[i].from,
                        to: this.state.specialDayModel[i].to,
                        name: this.state.specialDayModel[i].name,
                        specialDate: this.state.specialDayModel[i].specialDate,
                        id: this.state.specialDayModel[i].id,
                    };
                    specialDayModel.push(specialDayViewModel);
                }
            }

            var placeCalendar: PlaceCalendarRequestViewModel = {
                placeAltId: placeAltId,
                venueAltId: (localStorage.getItem("venueAltId") != null ? localStorage.getItem("venueAltId") : "BBF9647A-E370-42C7-A0E3-56738AD56E11"),
                weekOffDays: this.state.weekOffDays,
                placeStartDate: this.state.startdatetime,
                placeEndDate: this.state.enddatetime,
                placeType: placeType,
                holidayDates: this.state.selectedDays,
                placeTimings: this.state.timingModel,
                isEdit: (this.state.isEdit ? true : false),
                regularTimeModel: regularModel,
                seasonTimeModel: seasonModel,
                specialDayModel: specialDayModel,
                isNewCalendar: true
            }
            this.props.placeCalendarSubmit(placeCalendar, (response: PlaceCalendarResponseViewModel) => {
                if (this.state.isEdit) {
                } else {
                    if (response.success) {
                    }
                }
                alert("Calendar saved successfully");
            });
        }
    }

    @autobind
    public onSubmitNewCustomerIdType() {
        if (localStorage.getItem("_customer_Type") != null) {
            var customerTypeData = JSON.parse(localStorage.getItem("_customer_Type"));
            this.props.saveCustomerIdType(customerTypeData, (response: DocumentTypesSaveResponseViewModel) => {
                if (response.success) {
                    alert("customer type saved successfully");
                    this.props.requestDocumentTypeData();
                }
            });
        }
    }

    @autobind
    public onSubmitInventory() {
        if (localStorage.getItem("_Inventory_data")) {
            var inventoryData = JSON.parse(localStorage.getItem("_Inventory_data"));
            this.props.inventoryDataSubmit(inventoryData, (response: InventoryResponseViewModel) => {
                if (this.state.isEdit) {
                } else {
                }
                alert("Your inventory information has been saved successfully!");
            });
        }
    }
}
export default connect(
    (state: IApplicationState) => ({ placeCalendar: state.placeCalendar, currencyType: state.currencyType, inventory: state.inventory }),
    (dispatch) => bindActionCreators({ ...placeCalendarStore.actionCreators, ...currencyTypeStore.actionCreators, ...inventoryStore.actionCreators }, dispatch)
)(PlaceCalendarNew);
