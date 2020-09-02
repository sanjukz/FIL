/* Third party imports */
import * as React from "react";
import * as _ from "lodash";
import * as moment from 'moment';
import { Calendar, Drawer, Tag, Popover, Modal, Radio, Spin } from 'antd';
import { LeftOutlined, RightOutlined } from '@ant-design/icons';

/* Local imports */
import { setRecurranceScheduleObject } from "../utils/DefaultObjectSetter";
import { AddSchedule } from './Recurring/AddSchedule';
import { Reschedule } from './Recurring/Reschedule';
import { Time_ARRAY } from './Utils/Constant';
import { DatePicker } from './Common/DatePicker';

/* css / scss imports */
import 'antd/dist/antd.css';
import { format } from "path";

export enum View {
  none = 1,
  create,
  reschedule,
  delete
}

export class Recurring extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      eventScheduleViewModel: setRecurranceScheduleObject(this.props.props.slug, this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).toISOString()),
      isShowDrawer: false,
      isShowConfirmModel: false,
      eventRecurranceScheduleModel: [],
      deleteType: 1,
      view: View.none,
      fromDate: `${this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getFullYear()}-${this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getMonth() + 1}-01`,
      toDate: `${this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getFullYear()}-${this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getMonth() + 1}-${this.getDaysInMonth(this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getFullYear(), this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getMonth() + 1)}`,
      timezoneValue: 'Select timezone'
    }
  }

  componentDidMount() {
    this.requestSchedule();
  }

  getDaysInMonth = (year, month) => {
    return new Date(year, month, 0).getDate();
  }

  confirm = () => {
    return <Modal
      title="Delete occurence"
      visible={this.state.isShowConfirmModel}
      centered
      okType='danger'
      okText="Delete"
      cancelText="Cancel"
      onOk={() => {
        this.setState({ isShowConfirmModel: false });
        if (this.state.deleteType == 1) {
          this.props.props.props.deleteSingleSchedule(this.state.eventScheduleViewModel, (response: any) => {
            if (response.success) {
              this.requestSchedule()
            }
          })
        } else {
          this.props.props.props.deleteBulkSchedule(this.state.eventScheduleViewModel, (response: any) => {
            if (response.success) {
              this.requestSchedule()
            }
          })
        }
      }}
      onCancel={() => { this.setState({ isShowConfirmModel: false }) }}
      afterClose={() => {
        this.setState({ deleteType: 1 });
      }}
    >
      <Radio.Group onChange={() => { }} value={this.state.deleteType ? this.state.deleteType : 1}>
        <div className="antd-radio-grp mb-2"><Radio onClick={(e) => { this.setState({ deleteType: 1 }) }} value={1}><div><b>This occurence only</b></div><div>Delete {this.state.selectScheduleDetail.localStartDateString} ({this.state.selectScheduleDetail.localStartTime} - {this.state.selectScheduleDetail.localEndTime})</div></Radio></div>
        <div className="antd-radio-grp mb-2"><Radio onClick={(e) => { this.setState({ deleteType: 2 }) }} value={2}><div><b>Custom range</b></div><div>Delete all ({this.state.selectScheduleDetail.localStartTime} - {this.state.selectScheduleDetail.localEndTime}) within a selected date range</div></Radio></div>
        <div className="antd-radio-grp mb-2"><Radio onClick={(e) => { this.setState({ deleteType: 3 }) }} value={3}><div><b>Delete all schedule</b></div><div> Delete ({this.state.selectScheduleDetail.localStartTime} - {this.state.selectScheduleDetail.localEndTime}) occurance within {this.state.selectScheduleDetail.localEventScheduleStartDateTimeString} - {this.state.selectScheduleDetail.localEventScheduleEndDateTimeString}</div></Radio></div>
      </Radio.Group>
      {this.state.deleteType == 2 && <div className="row">
        <div className="form-group mb-2 position-relative col-sm-6">
          <label className="d-block">Start Date</label>
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
        <div className="form-group mb-2 position-relative col-sm-6">
          <label className="d-block">End Date</label>
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
      </div>}
    </Modal>
  }

  requestSchedule = () => {
    this.props.props.props.requestReschedule(this.props.props.slug, this.state.fromDate, this.state.toDate, (response: any) => {
      console.log(response);
      if (response.success) {
        this.setState({ eventRecurranceScheduleModel: response.eventRecurranceScheduleModel })
      }
    })
  }

  getCurrentUTC = (utcString) => {
    let splitStreamTime = (typeof utcString) == 'string' ? utcString.split(/[^0-9]/) : utcString.toISOString().split(/[^0-9]/);
    let currentUtcDate = new Date(+splitStreamTime[0], +(splitStreamTime[1]) - 1, +(splitStreamTime[2]), +(splitStreamTime[3]), +(splitStreamTime[4]), +(splitStreamTime[5]));
    return currentUtcDate;
  }

  getSchedule = (value) => {
    let scheduleData = [];
    this.state.eventRecurranceScheduleModel.forEach((val) => {
      let date = new Date(value);
      let dateToCheck = this.getCurrentUTC(`${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}T00:00:00`);
      var dateFrom = this.getCurrentUTC(val.localStartDateTime.split('T')[0] + 'T00:00:00');
      var dateTo = this.getCurrentUTC(val.localEndDateTime.split('T')[0] + 'T00:00:00');
      if (dateToCheck.getTime() >= dateFrom.getTime() && dateToCheck.getTime() <= dateTo.getTime()) {
        scheduleData.push(val);
      }
    });
    // scheduleData = _.uniq(scheduleData, true /* array already sorted */, (item) => {
    //   return item.localStartTime - item.localEndTime;
    // });
    scheduleData = scheduleData.filter((arr, index, self) =>
      index === self.findIndex((t) => (t.localStartTime === arr.localStartTime && t.localEndTime === arr.localEndTime)))
    return scheduleData;
  }

  getEventScheduleViewModel = (scheduleDetail: any, view: View) => {
    let eventScheduleViewModel = this.state.eventScheduleViewModel;
    eventScheduleViewModel.startDateTime = view == View.delete ? scheduleDetail.eventScheduleStartDateTime : scheduleDetail.localStartDateTime;
    eventScheduleViewModel.endDateTime = view == View.delete ? scheduleDetail.eventScheduleEndDateTime : scheduleDetail.localEndDateTime;
    eventScheduleViewModel.localStartDateTime = view == View.delete ? scheduleDetail.eventScheduleStartDateTime : scheduleDetail.localStartDateTime;
    eventScheduleViewModel.localEndDateTime = view == View.delete ? scheduleDetail.eventScheduleEndDateTime : scheduleDetail.localEndDateTime;
    eventScheduleViewModel.localStartTime = scheduleDetail.localStartTime.split(' ')[0];
    eventScheduleViewModel.localEndTime = scheduleDetail.localEndTime.split(' ')[0];
    eventScheduleViewModel.eventScheduleId = scheduleDetail.eventScheduleId;
    eventScheduleViewModel.scheduleDetailId = scheduleDetail.scheduleDetailId;
    return eventScheduleViewModel;
  }

  getPopoverBody = (item) => {
    let scheduleDetail = item;
    return <div>
      <button className="btn btn-link text-danger" onClick={(e) => {
        this.setState({ isShowConfirmModel: true, selectScheduleDetail: scheduleDetail, eventScheduleViewModel: this.getEventScheduleViewModel(item, View.delete) })
      }} >Delete</button>
      <button className="btn btn-link" onClick={(e) => {
        this.setState({ isShowDrawer: true, view: View.reschedule, selectScheduleDetail: scheduleDetail, eventScheduleViewModel: this.getEventScheduleViewModel(item, View.reschedule) })
      }} > Reschedule</button>
    </div>
  }

  getPopoverTitle = (item) => {
    return <div><b>{item.localStartDateString}</b><div>{Time_ARRAY.filter((val) => { return val.value == item.localStartTime })[0].displayText} - {Time_ARRAY.filter((val) => { return val.value == item.localEndTime })[0].displayText} </div></div>
  }

  getTags = (item) => {
    { Time_ARRAY.filter((val) => { return val.value == item.localStartTime })[0].displayText }
    return <Popover content={this.getPopoverBody(item)} title={this.getPopoverTitle(item)}><div className="mb-1"><Tag color="#108ee9">{Time_ARRAY.filter((val) => { return val.value == item.localStartTime })[0].displayText} - {Time_ARRAY.filter((val) => { return val.value == item.localEndTime })[0].displayText} </Tag></div></Popover>
  }

  monthCellRender = (value) => {
    let scheduleData = this.getSchedule(value);
    let isBefore = value.isBefore(new Date().setDate(new Date().getDate() - 1));
    return <div style={{ textAlign: "center" }} >
      {scheduleData.map((val) => {
        return <>{this.getTags(val)}</>
      })}
      {!isBefore && <section className="text-blueberry schedule-cal mt-3" onClick={(e) => {
        if (!this.props.timezoneKey) {
          this.props.onError();
          return;
        }
        this.setState({ isShowDrawer: true, selectedDate: value, view: View.create })
      }} >+Schedule</section>}
    </div>;
  }

  public render() {
    return <>
      <div className="fil-admin-cal">
        <div className="clearfix mb-3">
          <h5 className="float-sm-left">Schedule</h5>
          <button type="button" className="btn btn-outline-primary float-sm-right" onClick={(e) => {
            if (!this.props.timezoneKey) {
              this.props.onError();
              return;
            }
            this.setState({ isShowDrawer: true, view: View.create, selectedDate: moment(new Date()) })
          }} >+ Add Dates</button>
        </div>
        <div data-aos="fade-up" data-aos-duration="1000" >
          <Spin spinning={this.props.props.props.EventSchedule.isLoading} tip="Loading...">
            <Calendar
              dateCellRender={this.monthCellRender}
              headerRender={({ value, type, onChange, onTypeChange }) => {
                const current = value.clone();
                const localeData = value.localeData();
                const months = [];
                for (let i = 0; i < 12; i++) {
                  current.month(i);
                  months.push(localeData.monthsShort(current));
                }

                const month = value.month();
                return <div className="p-3 bg-light text-blueberry">
                  <LeftOutlined
                    className="alig-middle"
                    onClick={(e) => {
                      const newValue = value.clone();
                      newValue.month(parseInt(month - 1 as any, 10));
                      onChange(newValue);
                    }} />
                  <RightOutlined
                    className="ml-3 alig-middle"
                    onClick={(e) => {
                      const newValue = value.clone();
                      console.log(newValue)
                      newValue.month(parseInt(month + 1 as any, 10));
                      onChange(newValue);
                    }} />
                  <span style={{ fontSize: '16px' }} className="ml-3 align-middle mt-1"><b>{months[month]} {current.year()}</b></span>
                </div>
              }}
              defaultValue={moment(`${this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getFullYear()}-${this.getCurrentUTC(this.props.eventScheduleViewModel.startDateTime ? this.props.eventScheduleViewModel.startDateTime : new Date().toISOString()).getMonth() + 1}-${new Date().getDate()}`, ["MM-DD-YYYY", "YYYY-MM-DD"])}
              disabledDate={d => !d || d.isBefore(new Date().setDate(new Date().getDate() - 1))}
              onPanelChange={(e: any) => {
                this.setState({
                  fromDate: `${e._d.getFullYear()}-${e._d.getMonth() + 1}-01`,
                  toDate: `${e._d.getFullYear()}-${e._d.getMonth() + 1}-${this.getDaysInMonth(e._d.getFullYear(), e._d.getMonth() + 1)}`
                }, () => {
                  this.requestSchedule()
                })
              }} />
          </Spin>
          <Drawer
            title={this.state.view == View.create ? "Create new schedule" : "Reschedule"}
            width={520}
            onClose={() => {
              this.setState({ isShowDrawer: false })
            }}
            visible={this.state.isShowDrawer}
            bodyStyle={{ paddingBottom: 80 }}
          >
            {this.state.isShowDrawer ? (
              this.state.view == View.create ? <AddSchedule
                props={this.props}
                startDate={this.state.selectedDate}
                timezoneKey={this.props.timezoneKey}
                onSubmitSuccess={(e) => {
                  this.setState({ isShowDrawer: false });
                  this.requestSchedule()
                }}
                onClickCancel={(e) => {
                  this.setState({ isShowDrawer: false });
                }}
              /> : <Reschedule
                  eventScheduleViewModel={this.state.eventScheduleViewModel}
                  props={this.props}
                  timezoneKey={this.props.timezoneKey}
                  onSubmitSuccess={(e) => {
                    this.setState({ isShowDrawer: false });
                    this.requestSchedule()
                  }}
                  onClickCancel={(e) => {
                    this.setState({ isShowDrawer: false });
                  }}
                />
            ) : (
                <div></div>
              )}
          </Drawer>
          {this.state.selectScheduleDetail && <>{this.confirm()}</>}
        </div>
      </div>
    </>
  }
}
