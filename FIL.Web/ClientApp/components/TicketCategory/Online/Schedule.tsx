import * as React from "React";
import { DatePicker, Drawer, Select } from 'antd';
import * as getSymbolFromCurrency from "currency-symbol-map";
import { Convert24hrTo12hrTimeFormat, getBasePrice } from '../../../utils/currencyFormatter';
import { EventRecurranceScheduleModel } from "../../../models/TicketCategoryResponseViewModel";
import * as moment from "antd/node_modules/moment";
import * as numeral from "numeral";

export class Schedule extends React.Component<any, any> {
    public constructor(props) {
        super(props);
        this.state = {
            scheduleStartIndex: 0,
            scheduleEndIndex: 9
        };
    }

    getDisabledDates = (date: any, scheduleDetails: any) => {
        let isDisable = true;
        for (var i = 0; i < scheduleDetails.length; i++) {
            let currentDate = this.getStandardDate(scheduleDetails[i].localStartDateTime);
            var d = currentDate.getDate();
            var m = currentDate.getMonth() + 1; //Month from 0 to 11
            var y = currentDate.getFullYear();
            if (date.format('YYYY-MM-DD') == `${y}-${m < 10 ? `0${m}` : m}-${d < 10 ? `0${d}` : d}`) {
                isDisable = false;
                break;
            }
        }
        return isDisable;
    }

    getStandardDate = (dateString) => {
        let splitStreamTime = dateString.split(/[^0-9]/);
        let currentUtcDate = new Date(+splitStreamTime[0], +(splitStreamTime[1]) - 1, +(splitStreamTime[2]), +(splitStreamTime[3]), +(splitStreamTime[4]), +(splitStreamTime[5]));
        return currentUtcDate;
    }

    getScheduleCard = (item: EventRecurranceScheduleModel) => {
        return <div className="col-md-4 mt-4">
            <div className="card">
                <h5 className="card-header">{item.localStartDateString}</h5>
                <div className="card-body">
                    <p className="card-text text-muted">
                        {Convert24hrTo12hrTimeFormat(item.localStartTime)} - {Convert24hrTo12hrTimeFormat(item.localEndTime)} {this.props.ticketCategoryData.eventAttributes[0].timeZoneAbbreviation}
                        <a href="javascript:Void(0)" className="btn btn-primary pull-right" onClick={(e) => {
                            this.props.onSelectSchedule(item);
                        }}>Select <img src="https://static1.feelitlive.com/images/fil-images/icon/arrow-right.svg" className="ml-2" alt="" width={5} /></a>
                    </p>
                    <p>From <span className="font-weight-bold">{getSymbolFromCurrency(this.props.ticketCategoryData.currencyType.code)}{numeral(getBasePrice(this.props.ticketCategoryData.eventTicketAttribute)).format('0.00')}</span> {this.props.ticketCategoryData.currencyType.code} / person</p>
                    <a href="javascript:Void(0)" className="text-body mr-3"><img src="https://static6.feelitlive.com/images/fil-images/fil-share-icon.svg" alt="" className="mr-2" /> <u>Share</u> </a>
                </div>
            </div>
        </div>
    }

    getEventRecurranceRange = (scheduleDetails: any) => {
        let eventRecurranceScheduleModels = scheduleDetails;
        if (this.state.dateRange) {
            eventRecurranceScheduleModels = scheduleDetails.filter((item: EventRecurranceScheduleModel) => {
                let standardStartDate = this.getStandardDate(item.endDateTime);
                let standardEndDate = this.getStandardDate(item.endDateTime);
                let dateRangeStartDate = this.state.dateRange[0]._d as Date;
                let dateRangeEndDate = this.state.dateRange[1]._d as Date;
                standardStartDate = this.getStandardDate(`${standardStartDate.getFullYear()}-${standardStartDate.getMonth()}-${standardStartDate.getDate()}T00:00:00.000Z`)
                standardEndDate = this.getStandardDate(`${standardEndDate.getFullYear()}-${standardEndDate.getMonth()}-${standardEndDate.getDate()}T00:00:00.000Z`)
                dateRangeStartDate = this.getStandardDate(`${dateRangeStartDate.getFullYear()}-${dateRangeStartDate.getMonth()}-${dateRangeStartDate.getDate()}T00:00:00.000Z`)
                dateRangeEndDate = this.getStandardDate(`${dateRangeEndDate.getFullYear()}-${dateRangeEndDate.getMonth()}-${dateRangeEndDate.getDate()}T00:00:00.000Z`)
                if (standardStartDate.getTime() >= dateRangeStartDate.getTime() && standardEndDate.getTime() <= dateRangeEndDate.getTime()) {
                    return true;
                } else {
                    return false;
                }
            })
        } else {
            eventRecurranceScheduleModels = eventRecurranceScheduleModels.slice(this.state.scheduleStartIndex, this.state.scheduleEndIndex)
        }
        return eventRecurranceScheduleModels;
    }

    public render() {
        const { RangePicker } = DatePicker;
        const { Option } = Select;
        let scheduleDetails = this.props.ticketCategoryData.eventRecurranceScheduleModels;
        if (!scheduleDetails) {
            return <div>Timeslot doesn't exists</div>
        } else if (scheduleDetails.length == 0) {
            return <div>Timeslot doesn't exists</div>
        }
        let endDate = this.getStandardDate(scheduleDetails[scheduleDetails.length - 1].localStartDateTime);
        endDate.setDate(endDate.getDate() + 1);
        return <>
            <Drawer
                title="Basic Drawer"
                placement={"bottom"}
                closable={this.props.isClosable}
                onClose={() => { this.props.onCloseDrawer() }}
                visible={true}
                height={"100%"}
                key={"bottom"}
            >
                <div className="fil-site fil-exp-landing-page">
                    <section className="exp-sec-pad fil-selc-date-guest bg-white">
                        <div className="container">
                            <p>From <br />
                                <span className="font-weight-bold">{getSymbolFromCurrency(this.props.ticketCategoryData.currencyType.code)} {numeral(getBasePrice(this.props.ticketCategoryData.eventTicketAttribute)).format('0.00')}</span>
                                <span> {this.props.ticketCategoryData.currencyType.code} per person</span>
                                {this.props.isClosable && <a onClick={() => {
                                    this.props.onCloseDrawer()
                                }} className="pull-right"><img src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/fil-images/close-icon.png" width="24" /></a>}
                            </p>
                            <div className="form-group mr-sm-4">
                                <label className="d-block">DATES</label>
                                <RangePicker
                                    bordered={false}
                                    defaultValue={[moment(this.getStandardDate(scheduleDetails[0].localEndDateTime).getTime() < new Date().getTime() ? new Date() : this.getStandardDate(scheduleDetails[0].localEndDateTime)), moment(this.getStandardDate(scheduleDetails[scheduleDetails.length > 8 ? 8 : scheduleDetails.length - 1].localEndDateTime))]}
                                    disabledDate={(d => !d
                                        || d.isAfter(endDate)
                                        || d.isBefore(this.getStandardDate(scheduleDetails[0].localStartDateTime))
                                        || this.getDisabledDates(d, scheduleDetails)
                                    )}
                                    onChange={(e: any) => {
                                        if (e == null) {
                                            this.setState({ scheduleEndIndex: 9 });
                                        }
                                        this.setState({ dateRange: e })
                                    }}
                                />
                            </div>
                            <div className="form-group">
                                <label className="d-block">GUESTS</label>
                                <Select defaultValue="1 guest" style={{ width: "100%" }} bordered={false}>
                                    <Option value="1">1 guest</Option>
                                </Select>
                            </div>
                        </div>
                    </section>
                    <section className="exp-sec-pad fil-date-event bg-white pt-0">
                        <div className="container">
                            <div className="row">
                                {this.getEventRecurranceRange(scheduleDetails).map(item => {
                                    return <>{this.getScheduleCard(item)}</>;
                                })}
                            </div>
                        </div>
                    </section>
                    <section className="exp-sec-pad fil-date-submit pt-0 text-center bg-white">
                        {this.state.scheduleEndIndex <= scheduleDetails.length && !this.state.dateRange && <a className="btn btn-outline-primary" onClick={item => this.setState({ scheduleEndIndex: this.state.scheduleEndIndex + 9 })} >See More Dates</a>}
                    </section>
                </div>
            </Drawer>
        </>
    }
}

