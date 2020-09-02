/* Third party imports */
import * as React from "react";
import * as _ from "lodash";
import { Select as AntdSelect } from 'antd';
import * as moment from 'moment';

/* Local imports */
import { Select } from '../Common/Select';
import { DatePicker } from '../Common/DatePicker';
import { EventScheduleViewModel } from "../../../../models/CreateEventV1/EventScheduleViewModel";
import { setRecurranceScheduleObject } from "../../utils/DefaultObjectSetter";
import { Footer } from "../../Footer/FormFooter";
import { scrapeNumbers } from '../Utils/Common';
import { getOrdinalNum } from '../Utils/Common';
import { OccuranceType } from "../../../../Enums/OccuranceType";
import { OCCURANCE_TYPE, DAYS, Time_ARRAY } from '../Utils/Constant';

/* css / scss imports */
import 'antd/dist/antd.css';

export class AddSchedule extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      eventScheduleViewModel: setRecurranceScheduleObject(this.props.props.props.slug, this.props.startDate.format()),
      occurance_Text: 'Select frequency'
    }
  }

  form: any;

  public componentDidMount() {
  }

  selectTimezone = (e) => {
    this.setState({
      timezone: e
    });
  }

  isButtonDisable = () => {
    let eventSchedule = this.state.eventScheduleViewModel;
    if (eventSchedule.occuranceType == OccuranceType.Weekly) {
      return eventSchedule.startDateTime && eventSchedule.localStartTime && eventSchedule.localEndTime && eventSchedule.dayIds && eventSchedule.endDateTime
    } else if (eventSchedule.occuranceType == OccuranceType.Monthly || eventSchedule.occuranceType == OccuranceType.Daily) {
      return eventSchedule.startDateTime && eventSchedule.localStartTime && eventSchedule.localEndTime && eventSchedule.endDateTime
    } else {
      return eventSchedule.startDateTime && eventSchedule.localStartTime && eventSchedule.localEndTime
    }
  }

  onTimeZoneChange = (val) => {
    let timeZone = val.split("X");
    let eventScheduleViewModel = this.state.eventScheduleViewModel;
    eventScheduleViewModel.timeZoneAbbrivation = timeZone[1]
    eventScheduleViewModel.timeZoneOffset = timeZone[2]
    eventScheduleViewModel.timeZoneText = timeZone[0]
    return eventScheduleViewModel;
  }

  public render() {
    const { Option } = AntdSelect;
    let ocuurance_Type = OCCURANCE_TYPE;
    let ocuurance_Type_options = [];
    let time_Array = Time_ARRAY;
    let start_time_Array_Options = [];
    let end_time_Array_Options = [];
    let start_Time_Item = time_Array.filter((val) => { return val.value == this.state.eventScheduleViewModel.localStartTime });
    ocuurance_Type.map((item) => {
      ocuurance_Type_options.push(<Option value={`${item.id}X${item.displayText}`} key={`${item.id}X${item.displayText}`}>{item.displayText}</Option>)
    });
    time_Array.map((item) => {
      start_time_Array_Options.push(<Option value={`${item.value}X${item.displayText}`} key={`${item.value}X${item.displayText}`}>{item.displayText}</Option>)
    });
    time_Array.map((item) => {
      end_time_Array_Options.push(<Option value={`${item.value}X${item.displayText}`} key={`${item.value}X${item.displayText}`}>{item.displayText}</Option>)
    });
    return <>
      <form onSubmit={(e: any) => {
        e.preventDefault();
        this.setState({ isSaveRequest: true });
        let inputModel = this.state.eventScheduleViewModel;
        if (inputModel.occuranceType == OccuranceType.Once) {
          inputModel.endDateTime = inputModel.startDateTime;
        }
        inputModel.startDateTime = inputModel.startDateTime.split("+")[0];
        inputModel.endDateTime = inputModel.endDateTime.split("+")[0];
        inputModel = this.onTimeZoneChange(this.props.timezoneKey);
        this.props.props.props.props.saveBulkInsert(inputModel, (response: EventScheduleViewModel) => {
          if (response.success) {
            this.props.onSubmitSuccess();
          } else {
          }
          this.setState({ isSaveRequest: false });
        })
      }}
        ref={(ref) => { this.form = ref; }}
      >
        <div data-aos="fade-up" data-aos-duration="1000">
          <div className="row">
            <div className="form-group mb-2 position-relative col-sm-12">
              <label className="d-block"><b>Start Date</b></label>
              <DatePicker
                dateTime={this.state.eventScheduleViewModel.startDateTime}
                onChange={(e) => {
                  let eventScheduleViewModel = this.state.eventScheduleViewModel;
                  eventScheduleViewModel.startDateTime = e.format();
                  eventScheduleViewModel.endDateTime = '';
                  this.setState({ eventScheduleViewModel: eventScheduleViewModel });
                }}
              />
            </div>
            <div className="col-sm-6">
              <label className="d-block"><b>Start time</b></label>
              <Select
                value={this.state.eventScheduleViewModel.localStartTime ? time_Array.filter((val) => { return val.value == this.state.eventScheduleViewModel.localStartTime })[0].displayText : ''}
                placeHolder={'Select start time'}
                options={start_time_Array_Options}
                onChange={(e: any) => {
                  let eventScheduleViewModel = this.state.eventScheduleViewModel;
                  eventScheduleViewModel.localStartTime = e.split('X')[0];
                  eventScheduleViewModel.localEndTime = '';
                  this.setState({ eventScheduleViewModel: eventScheduleViewModel });
                }}
              />
            </div>
            <div className="col-sm-6">
              <label className="d-block"><b>End time</b></label>
              <Select
                value={this.state.eventScheduleViewModel.localEndTime ? time_Array.filter((val) => { return val.value == this.state.eventScheduleViewModel.localEndTime })[0].displayText : ''}
                placeHolder={'Select end time'}
                options={end_time_Array_Options}
                onChange={(e: any) => {
                  let eventScheduleViewModel = this.state.eventScheduleViewModel;
                  eventScheduleViewModel.localEndTime = e.split('X')[0];
                  this.setState({ eventScheduleViewModel: eventScheduleViewModel });
                }}
              />
            </div>
            <div className="form-group mb-2 position-relative col-sm-12 mt-2">
              <label className="d-block"><b>Select frequency</b></label>
              <Select
                value={OccuranceType[this.state.eventScheduleViewModel.occuranceType]}
                placeHolder={this.state.occurance_Text}
                options={ocuurance_Type_options}
                onChange={(e: any) => {
                  let eventScheduleViewModel = this.state.eventScheduleViewModel;
                  eventScheduleViewModel.occuranceType = e.split('X')[0];
                  this.setState({ eventScheduleViewModel: eventScheduleViewModel });
                }}
              />
            </div>
            {(this.state.eventScheduleViewModel.occuranceType != OccuranceType.Once) && <>
              {(this.state.eventScheduleViewModel.occuranceType == OccuranceType.Weekly) && <div className="form-group mb-2 position-relative col-sm-12">
                <label className="d-block"><b>Days</b></label>
                {DAYS.map((val) => {
                  return <button
                    onClick={(e: any) => {
                      let value = e.target.value;
                      let eventScheduleViewModel = this.state.eventScheduleViewModel;
                      eventScheduleViewModel.dayIds = scrapeNumbers(eventScheduleViewModel.dayIds.indexOf(value) > -1 ? eventScheduleViewModel.dayIds.split(value).join('') : `${eventScheduleViewModel.dayIds},${value}`);
                      this.setState({ eventScheduleViewModel: eventScheduleViewModel })
                    }}
                    className={this.state.eventScheduleViewModel.dayIds.indexOf(val.id) > -1 ? 'circle-btn active mr-2' : 'circle-btn mr-2'}
                    type='button'
                    value={val.id}>
                    {val.displayText}
                  </button>
                })}
              </div>}
              {(this.state.eventScheduleViewModel.occuranceType == OccuranceType.Monthly) && <div className="form-group mb-2 position-relative col-sm-12">
                <AntdSelect style={{ width: "100%" }} value={getOrdinalNum(new Date(this.state.eventScheduleViewModel.startDateTime).getDate())} disabled>
                  <Option value={getOrdinalNum(new Date(this.state.eventScheduleViewModel.startDateTime).getDate())}>{`On ${getOrdinalNum(new Date(this.state.eventScheduleViewModel.startDateTime).getDate())} of each month`}</Option>
                </AntdSelect>
              </div>}
              <hr />
              <div className="form-group position-relative col-sm-12">
                <label className="d-block"><b>End Date</b></label>
                <DatePicker
                  dateTime={this.state.eventScheduleViewModel.endDateTime}
                  startDate={this.state.eventScheduleViewModel.startDateTime}
                  onChange={(e) => {
                    let eventScheduleViewModel = this.state.eventScheduleViewModel;
                    eventScheduleViewModel.endDateTime = e.format();
                    this.setState({ eventScheduleViewModel: eventScheduleViewModel });
                  }}
                />
              </div>
            </>}
          </div>
        </div>
        <Footer
          onClickCancel={() => { this.props.onClickCancel(); }}
          isDisabled={!this.isButtonDisable()}
          isSaveRequest={this.state.isSaveRequest}
          saveText={'Save'}
          onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
      </form>
    </>
  }
}
