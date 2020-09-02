/* Third party imports */
import * as React from "react";
import * as _ from "lodash";
import { Select as AntdSelect } from 'antd';

/* Local imports */
import { Footer } from "../Footer/FormFooter";
import { timezone } from "../../../utils/timezones";
import { EventScheduleViewModel } from "../../../models/CreateEventV1/EventScheduleViewModel";
import { Time } from './Common/Time';
import { Select } from './Common/Select';
import { DatePicker } from './Common/DatePicker';
import { EventFrequencyType } from '../../../Enums/EventFrequencyType';

/* css / scss imports */
import 'antd/dist/antd.css';

export class Single extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {}
  }

  form: any;

  public render() {
    const { Option } = AntdSelect;
    let tZone = timezone;
    let tZoneOption = [];
    tZone.map((item) => {
      tZoneOption.push(<Option value={`${item.value} ${item.text}X${item.abbr}X${item.offset * 60}`} key={`${item.value} ${item.text}X${item.abbr}X${item.offset * 60}`}>{`${item.value} ${item.text}`}</Option>)
    });
    return <form onSubmit={(e: any) => {
      e.preventDefault();
      let eventScheduleViewModel = this.props.eventScheduleViewModel;
      this.setState({ isSaveRequest: true });
      eventScheduleViewModel.eventScheduleModel.eventFrequencyType = this.props.eventFrequency;
      this.props.props.props.saveEventSchedule(eventScheduleViewModel, (response: EventScheduleViewModel) => {
        if (response.success) {
          this.props.props.changeRoute(4, response.completedStep);
          this.setState({ eventScheduleViewModel: response });
        } else {
        }
        this.setState({ isSaveRequest: false });
      })
    }}
      ref={(ref) => { this.form = ref; }}
    >
      <div data-aos="fade-up" data-aos-duration="1000">
        <div className="row">
          <div className="form-group mb-2 position-relative col-sm-6">
            <label className="d-block">Select Date</label>
            <DatePicker
              dateTime={this.props.eventScheduleViewModel.eventScheduleModel.localStartDateTime}
              onChange={this.props.onChangeStartDate}
            />
          </div>
          <div className="form-group mb-2 position-relative col-sm-6">
            <label className="d-block">Start time</label>
            <Time
              time={this.props.eventScheduleViewModel.eventScheduleModel.localStartTime}
              onChange={(val) => {
                this.props.onChangeStartTime(val);
              }}
            />
          </div>
          <div className="col-sm-6">
            <label className="d-block">End Date</label>
            <DatePicker
              dateTime={this.props.eventScheduleViewModel.eventScheduleModel.localEndDateTime}
              startDate={this.props.eventScheduleViewModel.eventScheduleModel.localStartDateTime}
              onChange={this.props.onChangeEndDate}
            />
          </div>
          <div className="col-sm-6">
            <label className="d-block">End time</label>
            <Time
              time={this.props.eventScheduleViewModel.eventScheduleModel.localEndTime}
              onChange={(val) => {
                this.props.onChangeEndTime(val);
              }}
            />
          </div>
        </div>
      </div>
      <Footer
        onClickCancel={() => { this.props.props.changeRoute(2); }}
        isDisabled={!this.props.isButtonDisable() || (this.props.props.props.StepDetails.stepDetails.eventStatus == 6 && this.props.props.props.StepDetails.stepDetails.isTransacted)}
        isSaveRequest={this.state.isSaveRequest}
        onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
    </form>
  }
}
