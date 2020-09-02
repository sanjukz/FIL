/* Third party imports */
import * as React from "react";
import * as _ from "lodash";
import { Select as AntdSelect, Checkbox } from 'antd';
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

export class Reschedule extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      eventScheduleViewModel: { ...this.props.eventScheduleViewModel }
    }
  }

  form: any;

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
        inputModel.endDateTime = this.state.isBulkUpdate ? this.state.eventScheduleViewModel.endDateTime : inputModel.startDateTime;
        inputModel = this.onTimeZoneChange(this.props.timezoneKey);
        if (this.state.isBulkUpdate) {
          this.props.props.props.props.saveBulkReschedule(inputModel, (response: EventScheduleViewModel) => {
            if (response.success) {
              this.props.onSubmitSuccess();
            } else {
            }
            this.setState({ isSaveRequest: false });
          })
        } else {
          this.props.props.props.props.saveSingleReschedule(inputModel, (response: EventScheduleViewModel) => {
            if (response.success) {
              this.props.onSubmitSuccess();
            } else {
            }
            this.setState({ isSaveRequest: false });
          })
        }
      }}
        ref={(ref) => { this.form = ref; }}
      >
        <div data-aos="fade-up" data-aos-duration="1000">
          <div className="row">
            <div className="form-group mb-2 position-relative col-sm-12">
              <label className="d-block"><b>Start Date</b></label>
              <DatePicker
                dateTime={this.state.eventScheduleViewModel.localStartDateTime}
                onChange={(e) => {
                  let eventScheduleViewModel = this.state.eventScheduleViewModel;
                  eventScheduleViewModel.startDateTime = e.format();
                  eventScheduleViewModel.localStartDateTime = e.format();
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
            <div className="col-sm-6 my-3">
              <Checkbox onChange={(e) => {
                this.setState({ isBulkUpdate: e.target.checked });
              }}>Apply to multiple occurance</Checkbox>
            </div>
            {this.state.isBulkUpdate && <div className="form-group mb-2 position-relative col-sm-12">
              <label className="d-block"><b>End Date</b></label>
              <DatePicker
                dateTime={this.state.eventScheduleViewModel.localEndDateTime}
                startDate={this.state.eventScheduleViewModel.localStartDateTime}
                onChange={(e) => {
                  let eventScheduleViewModel = this.state.eventScheduleViewModel;
                  eventScheduleViewModel.endDateTime = e.format();
                  eventScheduleViewModel.localEndDateTime = e.format();
                  this.setState({ eventScheduleViewModel: eventScheduleViewModel });
                }}
              />
            </div>}

          </div>
        </div>
        {(this.state.isBulkUpdate && this.state.eventScheduleViewModel.localStartTime && this.state.eventScheduleViewModel.localEndTime) && <div> Reschedule all ({time_Array.filter((val) => { return val.value == this.props.eventScheduleViewModel.localStartTime })[0].displayText} - {time_Array.filter((val) => { return val.value == this.props.eventScheduleViewModel.localEndTime })[0].displayText}) between start & end date. </div>}
        <Footer
          onClickCancel={() => { this.props.onClickCancel(); }}
          isDisabled={false}
          saveText={'Save'}
          isSaveRequest={this.state.isSaveRequest}
          onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
      </form>
    </>
  }
}
