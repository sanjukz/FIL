import * as React from "react";
import { connect } from "react-redux";
import { autobind } from "core-decorators";
import { IApplicationState } from "../stores";
import { bindActionCreators } from "redux";
import DocumentTypesSaveResponseViewModel from "../models/Inventory/DocumentTypesSaveResponseViewModel";
import InventoryResponseViewModel from "../models/Inventory/InventoryResponseViewModel";
import * as placeCalendarStore from "../stores/PlaceCalendar";
import * as currencyTypeStore from "../stores/CurrencyType";
import * as inventoryStore from "../stores/Inventory";
import Inventory from "./Inventory";
import * as Datetime from "react-datetime";
import { timeViewModel } from "../models/PlaceCalendar/PlaceCalendarRequestViewModel";
import DayPicker from 'react-day-picker';
import DateUtils from 'react-day-picker';
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

const daysInWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
var timingId = 0;
class PlaceCalendar extends React.Component<PlaceCalendarProps, any> {
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
            isAlertCall: false
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
        var daySize = 7;
        for (var i = 0; i < daySize; i++) {
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
        this.setState({ weekOffDays: weekOffDays });
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
            this.setState({ isPerennialCalendar: true })
        }
    }

    @autobind
    public onClickRegularCalendarType() {
        if (this.state.isPerennialCalendar) {
            this.setState({ isPerennialCalendar: false })
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
        this.setState({ timingModel: placeTimings });
    }

    @autobind
    public onChange(timeType, index, time) {
        if (timeType == 1) {
            this.setState({ ["from" + index]: time });
        } else {
            this.setState({ ["to" + index]: time });
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
        this.setState({ timingModel: timing });
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
        this.setState({ selectedDays });
    }

    @autobind
    onRemoveTiming(index, e) {
        this.state.timingModel.splice(index, 1);
        this.setState({ timingModel: this.state.timingModel });
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
        this.setState({
            weekOffDays: weekOffDays,
            selectedDays: holidayDates,
            timingModel: placeTimings,
            isPerennialCalendar: (this.props.inventory.getPlaceInventoryDataResponseViewModel.event.eventTypeId == 1 ? false : true),
            startdatetime: (this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails.length > 0 ? new Date(this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails[0].startDateTime) : yesterday),
            enddatetime: (this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails.length > 0 ? new Date(this.props.inventory.getPlaceInventoryDataResponseViewModel.eventDetails[0].endDateTime) : yesterday),
            isDefaultStateUpdated: true
        });
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

    public render() {
        var that = this;
        var timing = [];
        var i = 0;
        for (i = 0; i < this.state.totalTimings; i++) {
            timing.push(i);
        }

        if (this.props.inventory.isInventoryDataSuccess && !this.state.isDefaultStateUpdated && this.state.isEdit) {
            this.updatedCalendarState();
        }

        if (this.props.placeCalendar.isShowSuccessAlert == true && !this.state.isAlertCall) {
            this.notify();
            this.setState({ isAlertCall: true });
        }

        var days;
        if (!this.state.isEdit) {
            days = daysInWeek.map(function (val) {
                i = i + 1;
                return <span className="btn btn-outline-primary">
                    <div className="custom-control custom-checkbox">
                        <input type="checkbox" id={"customCheck" + i} onChange={that.onDaysSelect.bind(that, val)} name={val} value={val} className="custom-control-input" />
                        <label className="custom-control-label" htmlFor={"customCheck" + i}>{val}</label>
                    </div>
                </span>
            });
        } else if (this.state.isEdit && that.state.weekOffDays.length > 0) {
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
                        <div className="form-group">
                            <label className="d-block mb-4">Calendar Type <span className="text-danger">*</span> </label>
                            <div className="custom-control custom-radio custom-control-inline">
                                <input type="radio" id="customRadioInline1" name="customRadioInline1" onClick={this.onClickPerennialCalendarType} checked={this.state.isPerennialCalendar} className="custom-control-input" />
                                <label className="custom-control-label" htmlFor="customRadioInline1">Perennial</label>
                            </div>
                            <div className="custom-control custom-radio custom-control-inline">
                                <input type="radio" id="customRadioInline2" name="customRadioInline1" onClick={this.onClickRegularCalendarType} checked={!this.state.isPerennialCalendar} className="custom-control-input" />
                                <label className="custom-control-label" htmlFor="customRadioInline2">Regular</label>
                            </div>
                        </div>

                        {(!this.state.isPerennialCalendar && this.state.isEdit) && <div>

                            <div className="row">
                                <div className="form-group mb-2 position-relative col">
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

                                <div className="form-group mb-2 position-relative col">
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
                                <div className="form-group mb-2 position-relative col">
                                    <label>From:</label>
                                    <Datetime inputProps={{ placeholder: 'Select place start date & time', }} onChange={this.OnChangeStartDatetime} value={this.state.startdatetime} dateFormat="YYYY-MM-DD" timeFormat={true} />
                                </div>

                                <div className="form-group mb-2 position-relative col">
                                    <label>To:</label>
                                    <Datetime inputProps={{ placeholder: 'Select place end date & time' }} onChange={this.OnChangeEndtime} value={this.state.enddatetime} dateFormat="YYYY-MM-DD" timeFormat={true} />
                                </div>
                            </div>

                        </div>}

                        <div className="form-group mt-4">
                            <label className="d-block">Days open in a week <span className="text-danger">*</span> </label>
                            <div className="btn-group" role="group" aria-label="Basic example">
                                {days}
                            </div>
                        </div>

                        <div className="form-group mt-4">
                            <label className="d-block">Timings <span className="text-danger">*</span> </label>
                            {timings}

                            <div className="form-group">
                                <a href="JavaScript:Void(0)" onClick={this.onClickAddTiming} className="text-decoration-none btn-link mr-3"> <small><i className="fa fa-plus mr-2" aria-hidden="true"></i>Add additional timing</small> </a>
                            </div>

                        </div>
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
        /*if (this.state.timingModel.length == 0) {
            alert("Please add timing");
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
                regularTimeModel: [],
                seasonTimeModel: [],
                specialDayModel: [],
                isNewCalendar: false
            }
            this.props.placeCalendarSubmit(placeCalendar, (response: PlaceCalendarResponseViewModel) => {
                if (this.state.isEdit) {
                } else {
                    if (response.success) {
                    }
                }
            });
        }*/
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
            });
        }
    }
}
export default connect(
    (state: IApplicationState) => ({ placeCalendar: state.placeCalendar, currencyType: state.currencyType, inventory: state.inventory }),
    (dispatch) => bindActionCreators({ ...placeCalendarStore.actionCreators, ...currencyTypeStore.actionCreators, ...inventoryStore.actionCreators }, dispatch)
)(PlaceCalendar);