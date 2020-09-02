import * as React from "react";
import * as _ from "lodash";
import DateUtils from 'react-day-picker';
import { PlaceCalendarRequestViewModel, regularViewModel } from "../../../../models/PlaceCalendar/PlaceCalendarRequestViewModel";
import { timeViewModel } from "../../../../models/PlaceCalendar/PlaceCalendarRequestViewModel";
import { TimePicker, Select } from 'antd';
import { DatePicker } from 'antd';
import 'antd/dist/antd.css';
import * as moment from 'moment';
import { timezone } from "../../../../utils/timezones";

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

export default class EventCalendar extends React.Component<any, any> {
    fromTime: any;
    toTime: any;

    constructor(props) {
        super(props);
        this.state = {
            placeHolidayDate: [],
            weekOffDays: [],
            startdatetime: null,
            enddatetime: null,
            isPerennialCalendar: false,
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
            isSpecialDayTouched: false,
            disableddate: [],
            timezone: null,
            isValidTimeModal: true,
            timezoneValue: 'Select Your Timezone'
        }
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
        this.setState({ timingModel: placeTimings });
    }

    handleDayClick = (day) => {
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

    updateCalendarSummary = () => {

    }

    checkIsSameDate = () => {
        var open = [false, false, false, false, false, false, false, false];
        let disableddate = [];
        for (var d = new Date(this.state.startdatetime); d <= new Date(this.state.enddatetime); d.setDate(d.getDate() + 1)) {
            if (d.getDay() == 0) {
                disableddate.push(7)
                open[7] = true;
            } else {
                disableddate.push(d.getDay())
                open[d.getDay()] = true;
            }
        }
        if (disableddate.length == 7) {
            disableddate = [];
        }
        this.setState({ isSameDate: true, weekOffDays: open, isRegularSameTime: true })
    }

    OnChangeStartDatetime = (e) => {
        this.setState({
            startdatetime: e,
            enddatetime: e
        }, function () {
            this.checkIsSameDate();
        })
    }

    onRemoveTiming = (e, index) => {
        this.state.timingModel.splice(index, 1);
        this.setState({ timingModel: this.state.timingModel }, function () { this.updateCalendarSummary(); });
    }

    selectTimezone = (e) => {
        this.setState({
            timezone: e
        });
    };

    convertTime12to24 = (time12h) => {
        var time = time12h;
        var hrs = Number(time.match(/^(\d+)/)[1]);
        var mnts = Number(time.match(/:(\d+)/)[1]);
        var format = time.match(/\s(.*)$/)[1];
        if (format == "pm" && hrs < 12) hrs = hrs + 12;
        if (format == "am" && hrs == 12) hrs = hrs - 12;
        var hours = hrs.toString();
        var minutes = mnts.toString();
        if (hrs < 10) hours = "0" + hours;
        if (mnts < 10) minutes = "0" + minutes;
        return (hours + ":" + minutes);
    }

    getDisabledHours = () => {
        let hours: number[] = [];
        let currentDate = moment().format("DD/MM/YYYY");
        let selectedDate = this.state.startdatetime.format("DD/MM/YYYY");
        if (currentDate == selectedDate) {
            for (let i = 0; i < moment().hour(); i++) {
                hours.push(i);
            }
            return hours;
        }
        return hours;
    };

    setDefaultForm = () => {
        var startDateTime = moment(moment.utc(this.props.inventoryData.eventDetails[0].startDateTime).toDate());
        var endDateTime = moment(moment.utc(this.props.inventoryData.eventDetails[0].endDateTime).toDate());
        let timeModel = [];
        var timingViewModel: timeViewModel = {
            id: 1,
            from: this.props.inventoryData.regularTimeModel.timeModel.length && this.props.inventoryData.regularTimeModel.timeModel[0].from,
            to: this.props.inventoryData.regularTimeModel.timeModel.length && this.props.inventoryData.regularTimeModel.timeModel[0].to
        }
        timeModel.push(timingViewModel);
        let tZone = timezone;
        let currenctTZone = tZone.filter((val) => {
            return val.abbr == this.props.inventoryData.eventAttribute.timeZoneAbbreviation
        });
        if (currenctTZone.length == 0) {
            currenctTZone = tZone.filter((val) => {
                return val.abbr == "IST"
            });
        }
        this.setState({
            startdatetime: startDateTime,
            enddatetime: endDateTime,
            timingModel: timeModel,
            isSetDefaultForm: true,
            timezoneValue: `${currenctTZone[0].value} ${currenctTZone[0].text}`,
            timezone: `${currenctTZone[0].text.substring(4, 5)}${currenctTZone[0].offset * 60}`
        }, () => {
            this.onSubmitPlaceCalendar(this);
        });
    }

    public render() {
        if (this.props.isEdit && !this.state.isSetDefaultForm && this.props.inventoryData && this.props.inventoryData.ticketCategoryContainer && this.props.inventoryData.ticketCategoryContainer.length > 0) {
            this.setDefaultForm();
        }
        const { Option } = Select;
        let tZone = timezone;
        let tZoneOption = [];
        tZone.map((item) => {
            tZoneOption.push(<Option value={item.value} key={`${item.value} ${item.text}X${item.abbr}X${item.offset * 60}`}>{`${item.value} ${item.text}`}</Option>)
        })
        var format = 'HH:mm';
        var that = this;
        var timing = [];
        var customDaysTime;
        var i = 0;
        for (i = 0; i < this.state.totalTimings; i++) {
            timing.push(i);
        }
        var i = 0;
        var timings = this.state.timingModel.map((item) => {
            i = i + 1;
            return <div key={item.id}>
                {(!that.state.isEdit) && <div>
                    {(that.state.timingModel.length > 1) && < div className="col-12"><label>Time {i}</label>{(that.state.timingModel.length > 1) && <a href="JavaScript:Void(0)" onClick={() => { that.onRemoveTiming(that, i - 1) }} className="text-decoration-none btn-link text-danger float-right"> <small><i className="fa fa-minus-circle" aria-hidden="true"></i> Remove </small> </a>}</div>}
                    <div className="d-inline-block">
                        <label>From</label>
                        <div className="pb-1">
                            <TimePicker
                                {...(this.props.isEdit ? { defaultValue: moment(this.state.timingModel[0].from, 'h:mm a') } : {})}
                                disabled={this.props.isEdit ? true : false}
                                disabledHours={that.getDisabledHours}
                                minuteStep={30}
                                use12Hours
                                format="h:mm a"
                                allowClear={false}
                                onChange={(value, val) => {
                                    var timing = that.state.timingModel;
                                    timing.map(function (time) {
                                        if (time.id == item.id) {
                                            item.from = that.convertTime12to24(val)
                                        }
                                    });
                                    let isValidTimeModal = timing[0].to ? ((timing[0].to < timing[0].from) || (timing[0].to == timing[0].from)) ? false : true : true;
                                    that.setState({ isValidTimeModal: isValidTimeModal, timingModel: timing, isSetFromTime: true, startHours: timing[0].from.split(":")[0] }, () => { });
                                }} />
                        </div>
                    </div>
                    <div className="d-inline-block ml-2">
                        <label>To</label>
                        <div className="pb-1">
                            <TimePicker
                                disabledHours={() => {
                                    let hours: number[] = [];
                                    for (let i = 0; i < +that.state.startHours; i++) {
                                        hours.push(i);
                                    }
                                    return hours;
                                }}
                                minuteStep={30}
                                {...(this.props.isEdit ? { defaultValue: moment(this.state.timingModel[0].to, 'h:mm a') } : {})}
                                use12Hours
                                format="h:mm a"
                                allowClear={false}
                                disabled={(that.state.isSetFromTime && !this.props.isEdit) ? false : true}
                                onChange={(value, val) => {
                                    var timing = that.state.timingModel;
                                    timing.map(function (time) {
                                        if (time.id == item.id) {
                                            item.to = that.convertTime12to24(val)
                                        }
                                    });
                                    let isValidTimeModal = timing[0].to ? ((timing[0].to < timing[0].from) || (timing[0].to == timing[0].from)) ? false : true : true;
                                    that.setState({ timingModel: timing, isValidTimeModal: isValidTimeModal }, () => { that.onSubmitPlaceCalendar(that); });
                                }} />
                        </div>
                    </div>
                </div>}
            </div>
        });
        var date = new Date();
        date.setDate(date.getDate() - 1);
        var valid = function (current) {
            return current.isAfter(date);
        };
        return <div>
            <div className="row">
                <div className="col-sm-12">
                    <div className="form-group mb-2 position-relative col-sm-6">
                        <label className="d-block" >Select Date</label>
                        {(this.props.isEdit) && <DatePicker
                            disabled={true}
                            onChange={this.OnChangeStartDatetime}
                            disabledDate={d => !d || d.isBefore(new Date().setDate(new Date().getDate() - 1))}
                            format="MM/DD/YYYY"
                            placeholder="MM/DD/YYYY"
                            value={moment(this.state.startdatetime, 'MM/DD/YYYY')}
                            allowClear={false}
                        />}
                        {(!this.props.isEdit) && <DatePicker
                            onChange={this.OnChangeStartDatetime}
                            disabledDate={d => !d || d.isBefore(new Date().setDate(new Date().getDate() - 1))}
                            format="MM/DD/YYYY"
                            placeholder="MM/DD/YYYY"
                            allowClear={false}
                        />}
                    </div>
                    {(this.state.startdatetime != null &&
                        this.state.enddatetime != null) &&
                        (<>
                            <div className="form-group mb-2 position-relative col-sm-6">
                                <label className="d-block">Timezone</label>
                                <Select
                                    disabled={this.props.isEdit ? true : false}
                                    showSearch
                                    placeholder="Select Your Timezone"
                                    value={this.state.timezoneValue}
                                    onChange={(e: any) => {
                                        let key = e;
                                        let timeZone = key.split("X");
                                        this.setState({ timezoneValue: timeZone[0], timezone: timeZone[2], timezoneAbbr: timeZone[1] })
                                    }}>
                                    {tZoneOption}
                                </Select>
                            </div>
                            {(this.state.timezone) && <div className="form-group mb-2 position-relative col-12">
                                <label className="d-block">Enter Timing</label>
                                {timings}
                                {(!this.state.isValidTimeModal) && <div className="text-danger"> Start time should be less than end time </div>}
                            </div>}

                        </>)}
                </div>
            </div>
        </div>
    }

    public onSubmitPlaceCalendar = (e) => {
        var isRegularOpendaysExists = true;
        var isRegularCustomTimeExists = true;
        var isRegularSameTimeExists = true;
        var isRegularCustomeTime = true;
        var regularDay = "";
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
        var placeAltId = "4A4A848E-B9F0-4B68-8426-48138EEC51CC";
        var seasonModel = [];
        var specialDayModel = [];
        var regularModel: regularViewModel = {
            isSameTime: this.state.isRegularSameTime,
            timeModel: this.state.timingModel,
            customTimeModel: this.state.customTimeModel,
            daysOpen: this.state.weekOffDays
        };

        var placeCalendar: PlaceCalendarRequestViewModel = {
            placeAltId: placeAltId,
            venueAltId: (localStorage.getItem("venueAltId") != null ? localStorage.getItem("venueAltId") : "BBF9647A-E370-42C7-A0E3-56738AD56E11"),
            weekOffDays: this.state.weekOffDays,
            placeStartDate: this.state.startdatetime,
            placeEndDate: this.state.enddatetime,
            placeType: 1,
            holidayDates: this.state.selectedDays,
            placeTimings: this.state.timingModel,
            isEdit: (this.state.isEdit ? true : false),
            regularTimeModel: regularModel,
            seasonTimeModel: seasonModel,
            specialDayModel: specialDayModel,
            isNewCalendar: true,
            timeZone: this.state.timezone,
            timeZoneAbbreviation: this.state.timezoneAbbr
        }
        this.props.onSubmit(placeCalendar);
    }
}
