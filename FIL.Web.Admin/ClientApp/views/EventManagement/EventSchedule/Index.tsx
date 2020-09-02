/* Third party imports */
import * as React from "react";
import * as _ from "lodash";
import { Select as AntdSelect } from 'antd';

/* Local imports */
import { timezone } from "../../../utils/timezones";
import { EventScheduleViewModel } from "../../../models/CreateEventV1/EventScheduleViewModel";
import { setScheduleObject } from "../utils/DefaultObjectSetter";
import { convertTime12to24 } from './Utils/Common';
import { Single } from './Single';
import { EventFrequencyType } from '../../../Enums/EventFrequencyType';
import { Recurring } from './Recurring';
import Spinner from "../../../components/Spinner/index";
import { Select } from './Common/Select';
import { Footer } from "../Footer/FormFooter";
import { showNotification } from "../Notification";
/* css / scss imports */
import 'antd/dist/antd.css';

export class Index extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      eventScheduleViewModel: setScheduleObject(this.props.slug),
      eventFrequencyType: EventFrequencyType.None,
      timezoneValue: 'Select timezone'
    }
  }

  form: any;

  public componentDidMount() {
    window.scrollTo(0, document.body.scrollHeight || document.documentElement.scrollHeight);
    if (this.props.slug) {
      this.props.props.requestEventSchedule(this.props.slug, (response: EventScheduleViewModel) => {
        if (response.success && response.isValidLink && !response.isDraft) {
          response.currentStep = this.state.eventScheduleViewModel.currentStep;
          console.log(response);
          let offSetValue = +response.eventScheduleModel.timeZoneOffset / 60;
          let timeZone = timezone.filter(s => (s.abbr == response.eventScheduleModel.timeZoneAbbrivation && offSetValue == s.offset));
          this.props.props.onChangeTimezone(`${timeZone[0].value} ${timeZone[0].text}X${timeZone[0].abbr}X${timeZone[0].offset * 60}`);
          response.eventScheduleModel.timeZoneText = `${timeZone[0].value} ${timeZone[0].text}`;
          this.setState({ eventScheduleViewModel: response, eventFrequencyType: response.eventScheduleModel.eventFrequencyType });
        } else {
          this.setState({ eventFrequencyType: response.eventScheduleModel.eventFrequencyType });
        }
      });
    }
  }

  OnChangeStartDate = (e) => {
    let eventScheduleViewModel = this.state.eventScheduleViewModel;
    eventScheduleViewModel.eventScheduleModel.startDateTime = e;
    eventScheduleViewModel.eventScheduleModel.localStartDateTime = e;
    this.setState({ eventScheduleViewModel: eventScheduleViewModel });
  }

  OnChangeEndDate = (e) => {
    let eventScheduleViewModel = this.state.eventScheduleViewModel;
    eventScheduleViewModel.eventScheduleModel.endDateTime = e;
    eventScheduleViewModel.eventScheduleModel.localEndDateTime = e;
    this.setState({ eventScheduleViewModel: eventScheduleViewModel });
  }

  isButtonDisable = () => {
    let eventSchedule = this.state.eventScheduleViewModel.eventScheduleModel;
    return (eventSchedule.startDateTime
      && eventSchedule.timeZoneAbbrivation
      && eventSchedule.localEndTime
      && eventSchedule.localStartTime);
  }

  onTimeZoneChange = (val) => {
    let timeZone = val.split("X");
    let eventScheduleViewModel = this.state.eventScheduleViewModel;
    eventScheduleViewModel.eventScheduleModel.timeZoneAbbrivation = timeZone[1]
    eventScheduleViewModel.eventScheduleModel.timeZoneOffset = timeZone[2]
    eventScheduleViewModel.eventScheduleModel.timeZoneText = timeZone[0]
    this.setState({ eventScheduleViewModel: eventScheduleViewModel });
    return eventScheduleViewModel;
  }

  public render() {
    const { Option } = AntdSelect;
    let tZone = timezone;
    let tZoneOption = [];
    tZone.map((item) => {
      tZoneOption.push(<Option value={`${item.value} ${item.text}X${item.abbr}X${item.offset * 60}`} key={`${item.value} ${item.text}X${item.abbr}X${item.offset * 60}`}>{`${item.value} ${item.text}`}</Option>)
    });

    return <>
      <div data-aos="fade-up" data-aos-duration="1000">
        <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent" key="2">
          <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
            <h3 className="m-0 text-purple">Getting it on the calendar</h3></nav>
          <div className="row">
            <div className="col-sm-6">
              <label className="d-block" >Event Frequency</label>
              <div className="btn-group custom-toggle-btn mb-4" role="group" aria-label="Basic example">
                <button
                  onClick={(e: any) => {
                    this.setState({ eventFrequencyType: EventFrequencyType.Single })
                  }}
                  type="button"
                  className={
                    this.state.eventFrequencyType == EventFrequencyType.Single ?
                      'btn btn-outline-primary active' : 'btn btn-outline-primary'
                  }
                >
                  Single Event
                </button>
                <button
                  onClick={(e: any) => {
                    this.setState({ eventFrequencyType: EventFrequencyType.Recurring })
                  }} type="button"
                  className={
                    this.state.eventFrequencyType == EventFrequencyType.Recurring ?
                      'btn btn-outline-primary active' : 'btn btn-outline-primary'
                  }
                >
                  Recurring Events
               </button>
                <button
                  onClick={(e: any) => {
                    this.setState({ eventFrequencyType: EventFrequencyType.OnDemand })
                  }} type="button"
                  className={
                    this.state.eventFrequencyType == EventFrequencyType.OnDemand ?
                      'btn btn-outline-primary active' : 'btn btn-outline-primary'
                  }
                >
                  On-demand
               </button>
              </div>
            </div>
            <div className="col-sm-6">
              <label className="d-block">Timezone</label>
              <Select
                value={this.state.eventScheduleViewModel.eventScheduleModel.timeZoneText}
                options={tZoneOption}
                placeHolder={'Select timezone'}
                onChange={(e: any) => {
                  this.onTimeZoneChange(e);
                  this.props.props.onChangeTimezone(e);
                }}
              />
              {(!this.props.props.EventSchedule.timeZoneKey && this.state.errorMsg) && <label className="d-block text-danger">{this.state.errorMsg}</label>}
            </div>
          </div>

          {(this.state.eventFrequencyType == EventFrequencyType.Single || this.state.eventFrequencyType == EventFrequencyType.OnDemand) && <Single
            eventScheduleViewModel={this.state.eventScheduleViewModel}
            props={this.props}
            onChangeStartDate={this.OnChangeStartDate}
            onChangeEndDate={this.OnChangeEndDate}
            eventFrequency={this.state.eventFrequencyType}
            onChangeStartTime={(val) => {
              let eventScheduleViewModel = this.state.eventScheduleViewModel;
              eventScheduleViewModel.eventScheduleModel.localStartTime = convertTime12to24(val)
              this.setState({ eventScheduleViewModel: eventScheduleViewModel });
            }}
            onTimeZoneChange={(e: any) => {
              this.onTimeZoneChange(e);
              this.props.props.onChangeTimezone(e);
            }}
            onChangeEndTime={(val) => {
              let eventScheduleViewModel = this.state.eventScheduleViewModel;
              eventScheduleViewModel.eventScheduleModel.localEndTime = convertTime12to24(val)
              this.setState({ eventScheduleViewModel: eventScheduleViewModel });
            }}
            isButtonDisable={this.isButtonDisable}
          />}

          {this.state.eventFrequencyType == EventFrequencyType.Recurring && <Recurring
            eventScheduleViewModel={this.state.eventScheduleViewModel.eventScheduleModel}
            timezoneKey={this.props.props.EventSchedule.timeZoneKey}
            onError={() => {
              window.scrollTo(0, 0);
              showNotification('error', 'Please select timezone');
              this.setState({ errorMsg: 'Please select timezone' })
            }}
            props={this.props}
          />}

          {this.state.eventFrequencyType == EventFrequencyType.None && <Spinner />}
        </div>

        {this.state.eventFrequencyType == EventFrequencyType.Recurring && <Footer
          onClickCancel={() => { this.props.changeRoute(2); }}
          isDisabled={!this.props.props.EventSchedule.timeZoneKey}
          isSaveRequest={this.state.isSaveRequest}
          onSubmit={() => {
            let eventSchedule = this.state.eventScheduleViewModel;
            eventSchedule = this.onTimeZoneChange(this.props.props.EventSchedule.timeZoneKey);
            eventSchedule.startDateTime = new Date().toISOString();
            eventSchedule.endDateTime = new Date().toISOString();
            eventSchedule.eventScheduleModel.startDateTime = new Date().toISOString();
            eventSchedule.eventScheduleModel.endDateTime = new Date().toISOString();
            eventSchedule.eventScheduleModel.localStartDateTime = new Date().toISOString();
            eventSchedule.eventScheduleModel.endDateTime = new Date().toISOString();
            eventSchedule.eventScheduleModel.eventFrequencyType = EventFrequencyType.Recurring;
            eventSchedule.eventScheduleModel.localStartTime = "00:00";
            eventSchedule.eventScheduleModel.localEndTime = "00:00";
            this.setState({ isSaveRequest: true });
            this.props.props.saveEventSchedule(eventSchedule, (response: EventScheduleViewModel) => {
              if (response.success) {
                this.props.changeRoute(4, response.completedStep);
              }
              this.setState({ isSaveRequest: false });
            })
          }} />}
      </div>
    </>
  }
}
