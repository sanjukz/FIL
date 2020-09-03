import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../stores";
import TransactionReportTable from "../../components/TransactionReportTable";
import ReportingSummary from "../../components/ReportingWizard/ReportingSummaryTable";
import * as transReportStore from "../../stores/Reporting/Report";
import TransactionReportResponseViewModel, { TransactionReportViewModel } from "../../models/Report/TransactionReportResponseViewModel";
import TransactionReportRequestViewModel from "../../models/Report/TransactionReportRequestViewModel";
import "../../scss/FILSuite/_transaction-report.scss";
import { actionCreators as sessionActionCreators, ISessionProps } from "shared/stores/Session";
import "../../scss/FILSuite/custom-table.scss";
import { bindActionCreators } from "redux";
import Select from "react-select";
import "./Report.scss";
import Newloader from "../../components/NewLoader/NewLoader";
import { DatePicker, Spin, Alert } from 'antd';
import 'antd/dist/antd.css';
import { Footer } from '../EventManagement/Footer/FormFooter';
import { CopyOutlined } from '@ant-design/icons';
import { Popover } from 'antd';
import { CopyToClipboard } from 'react-copy-to-clipboard';

type TransactionReportProps = transReportStore.IReportEventsProps
  & ISessionProps
  & typeof sessionActionCreators
  & typeof transReportStore.actionCreators;

class TransactionReportSingleDataModel extends React.Component<TransactionReportProps, any> {
  constructor(props) {
    super(props);
    this.state = {
      valEvent: "",
      valSubEvent: "",
      valFromDate: null,
      valToDate: null,
      showSummary: false,
      roleId: "",
      errorMessage: "",
      selectedSubEvents: null,
      selectedEvents: null,
      selectedCurrency: null,
      lastSelectedeventLength: 0,
      modifySearch: true,
      showTestTxs: false,
      href: 'feelitlive'
    }
    this.changeEventVal = this.changeEventVal.bind(this);
    this.changeSubEventVal = this.changeSubEventVal.bind(this);
    this.showHide = this.showHide.bind(this);
    this.getTransactionReport = this.getTransactionReport.bind(this);
    this.handleonChangeStartDate = this.handleonChangeStartDate.bind(this);
    this.handleonChangeEndDate = this.handleonChangeEndDate.bind(this);
    this.modifySearch = this.modifySearch.bind(this);
  }

  public componentDidMount() {
    this.props.requestAllPlaces(this.props.session.user.altId);
    this.setState({ roleId: localStorage.getItem('roleId'), href: window.location.href });
  }

  public changeEventVal(e) {
    this.setState({ valEvent: e });
    this.setState({ valSubEvent: "" });
    if (e != null) {
      this.props.requestMultipleSubEventData(e.altId);
    }
  }

  public changeSubEventVal(e) {
    this.setState({ valSubEvent: e });
  }

  handleonChangeStartDate(e) {
    this.setState({ valFromDate: e });
  }
  handleonChangeEndDate(e) {
    this.setState({ valToDate: e });
  }

  public modifySearch() {
    this.setState({ modifySearch: true });
  }

  public getTransactionReport() {

    this.setState({ errorMessage: "" });
    this.setState({ showSummary: false });
    this.setState({ showSummary: false });
    this.setState({ modifySearch: false });

    var eventAltIds = "";
    var eventDetailIds = "";
    var currencyTypes = "";

    if (this.state.selectedEvents != null) {
      if (this.state.selectedEvents.filter((val) => { return val.id == 0 }).length > 0) {
        this.props.reportEvents.places.places.map((val) => {
          if (val.id == 0) return;
          eventAltIds = eventAltIds + val.altId + ","
        })
      }
      this.state.selectedEvents.map(function (item) {
        if (item.id == 0) return;
        eventAltIds = eventAltIds + item.value + ","
      })
    } else {
      if (this.state.valFromDate && this.state.valToDate) {
        this.props.reportEvents.places.places.map((val) => {
          if (val.id == 0) return;
          eventAltIds = eventAltIds + val.altId + ","
        })
      } else {
        return;
      }
    }

    if (this.state.selectedSubEvents != null) {
      this.state.selectedSubEvents.map(function (item) {
        eventDetailIds = eventDetailIds + item.value + ","
      })
    }

    if (this.state.selectedCurrency != null) {
      this.state.selectedCurrency.map(function (item) {
        currencyTypes = currencyTypes + item.value + ","
      })
    }

    eventAltIds = eventAltIds.replace(/(^,)|(,$)/g, "");
    eventDetailIds = eventDetailIds.replace(/(^,)|(,$)/g, "");
    currencyTypes = currencyTypes.replace(/(^,)|(,$)/g, "");

    var ReportFormDataViewModel: TransactionReportRequestViewModel = {
      eventAltId: eventAltIds,
      userAltId: (localStorage.getItem("altId") != null ? (localStorage.getItem("altId")) : ""),
      eventDetailId: eventDetailIds,
      currencyTypes: currencyTypes,
      fromDate: this.state.valFromDate == "" || this.state.valFromDate == null ? "2018-01-01" : this.state.valFromDate,
      toDate: this.state.valToDate == "" || this.state.valToDate == null ? "2050-12-31" : this.state.valToDate
    };
    this.props.getTransactionReportAsSingleDataModel(ReportFormDataViewModel, (response: TransactionReportResponseViewModel) => {
      if (response) {
        setTimeout(() => {
        }, 3000);
      }
    });
  }

  showHide() {
    if (this.props.reportEvents.fetchReportSuccess) {
      this.setState({
        showSummary: !this.state.showSummary
      });
    }
  }

  onChangeEvent = (selectedEvents) => {
    this.setState({ selectedEvents });
    if (selectedEvents.length == 0) {
      this.setState({ selectedEvents: null });
      this.setState({ lastSelectedeventLength: 0 });
    } else {
      this.setState({ selectedEvents });
      this.setState({ lastSelectedeventLength: selectedEvents.length });
    }
  }

  getStreamLink = (streamLink) => {
    if (this.state.href.indexOf('dev') > -1 || this.state.href.indexOf('local') > -1) {
      return `https://dev.feelitlive.com/stream-online?token=${streamLink}`
    } else {
      return `https://www.feelitlive.com/stream-online?token=${streamLink}`
    }
  }

  public render() {
    var columns = [];
    var tableData = [];
    var that = this;
    let sellerRestrictedColumns = [1, 2, 3, 9, 11, 13, 36, 14, 16, 15, 19, 20, 93, 25, 21, 24, 88, 94];
    if (this.props.reportEvents.fetchAllPlaces == true) {
      var eventData = this.props.reportEvents.places ? this.props.reportEvents.places.places : [];
      var subEventData = this.props.reportEvents.allSubEvents.subEvents === undefined ? [] : this.props.reportEvents.allSubEvents.subEvents;
      var userAltId = localStorage.getItem('altId');
      var columns = [];
      let newName = {};
      if (this.props.reportEvents.transactionReportSingleDataModel != undefined) {
        let columnData = this.props.reportEvents.transactionReportSingleDataModel.transactionReports.reportColumns || [];
        if (this.props.session && this.props.session.isAuthenticated && this.props.session.user.rolesId == 11) { // is it's seller
          columnData = columnData.filter((item) => {
            return sellerRestrictedColumns.indexOf(item.id) > -1
          });
        }
        columnData.map(function (item) {
          if (item.dbFieldName == "pricePerTicket" || item.dbFieldName == "grossTicketAmount" || item.dbFieldName == "netTicketAmountUSD" || item.dbFieldName == "discountAmount" || item.dbFieldName == "serviceCharge" || item.dbFieldName == "convenienceCharges" || item.dbFieldName == "deliveryCharges" || item.dbFieldName == "entryCount" || item.dbFieldName == "totalTransactedAmount" || item.dbFieldName == "totalTransactedAmountUSD" || item.dbFieldName == "netTicketAmount" || item.dbFieldName == "donationAmount") {
            newName = {
              Header: item.displayName,
              accessor: item.dbFieldName,
              minWidth: 170,
              className: "right",
            };
            columns.push(newName);
          }
          else if (item.dbFieldName == "transactionId" || item.dbFieldName == "numberOfTickets") {
            newName = {
              Header: item.displayName,
              accessor: item.dbFieldName,
              minWidth: 170,
              className: "text-center",
            };
            columns.push(newName);
          }
          else if (item.dbFieldName == "streamLink") {
            newName = {
              Header: item.displayName,
              accessor: item.dbFieldName,
              minWidth: 170,
              className: "text-center",
              Cell: e => <><Popover content="Copy to clipboard">
                <CopyToClipboard text={that.getStreamLink(e.value)}
                  onCopy={() => { }}>
                  <CopyOutlined onClick={(e: any) => {
                  }} /></CopyToClipboard></Popover><a target="_blank" href={that.getStreamLink(e.value)}> {that.getStreamLink(e.value)} </a></>
            };
            columns.push(newName);
          }
          else {
            if (item.dbFieldName == "eventName" || item.dbFieldName == "eventStartDateTime") {  // replace event to place
              let header = item.dbFieldName == "eventName" ? "Place/Event Name" : "Place/Event Visit Date";
              newName = {
                Header: header,
                accessor: item.dbFieldName,
                minWidth: 170,
              };
              columns.push(newName);
            } else {
              newName = {
                Header: item.displayName,
                accessor: item.dbFieldName,
                minWidth: 170,
              };
              columns.push(newName);
            }
          }
        });
        var data = this.props.reportEvents.transactionReportSingleDataModel.transactionReports.transactionData.slice(0, 100);
        var t0 = performance.now();
        data.forEach(function (key, value) {
          var listPropertyNames = Object.keys(key);
          var object = new Object();
          for (var i = 0; i < columns.length; i++) {
            if (listPropertyNames.indexOf(columns[i].accessor) > -1) {
              var dbFieldName = columns[i].accessor
              object[dbFieldName] = key[dbFieldName];
            }
          }
          tableData.push(object);
        });
        var t1 = performance.now();
      }
      var eventOptions = [];
      var subEventOptions = [];
      var currencyOptions = [];
      if (this.props.session.user.rolesId == 10) {
        var allModel = {
          value: 'All',
          label: 'All',
          id: 0
        };
        eventOptions.push(allModel);
      }
      eventData.map((item) => {
        if (this.state.selectedEvents != null && this.state.selectedEvents.filter((val) => { return val.id == 0 }).length > 0) {
          return;
        }
        var data = {
          value: item.altId,
          label: item.name,
          id: item.id
        };
        eventOptions.push(data);
      });
      if (this.props.reportEvents.allSubEvents.currencyTypes != undefined) {
        this.props.reportEvents.allSubEvents.currencyTypes.map(function (item) {
          var data = {
            value: item.id,
            label: item.code
          };
          currencyOptions.push(data);
        });
      }

      subEventData.map(function (item) {
        var isEventExist = false;
        if (that.state.selectedEvents != null) {
          var isEventExistItem = that.state.selectedEvents.filter(function (val) {
            return val.id == item.eventId
          });
          if (isEventExistItem.length > 0) {
            isEventExist = true;
          }
        }
        if (isEventExist) {
          var data = {
            value: item.id,
            label: item.name,
            eventId: item.eventId
          };
          subEventOptions.push(data);
        }
      });

      return <div className="card border-0 right-cntent-area pb-5 bg-light" >
        <div className="card-body bg-light p-0">
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box" id="nav-tabContent">
            <div className="row">
              <div className="col-md-6">
                <div className="form-group" style={{ zIndex: 3, position: "relative" }}>
                  <label>Select Place/Experience/Event {userAltId == "f253336e-6461-4fc4-a63b-540f6dbb88b7" ? " " : <span className="text-danger">*</span>}</label>
                  < Select
                    isMulti
                    options={eventOptions}
                    onChange={this.onChangeEvent}
                    value={this.state.selectedEvents}
                    placeholder={"Select Place/Experience/Event"}
                  />
                </div>
              </div>
              <div className="col-md-3">
                <label>Start Date</label>
                <div className="form-group">
                  <DatePicker
                    onChange={this.handleonChangeStartDate}
                    format="MM/DD/YYYY"
                    placeholder="MM/DD/YYYY"
                    allowClear={true}
                    style={{ width: "100%" }}
                  />
                </div>
              </div>
              <div className="col-md-3">
                <label>End Date</label>
                <div className="form-group">
                  <DatePicker
                    onChange={this.handleonChangeEndDate}
                    format="MM/DD/YYYY"
                    placeholder="MM/DD/YYYY"
                    allowClear={true}
                    style={{ width: "100%" }}
                  />
                </div>
              </div>
            </div>
            {this.props.reportEvents.isLoading && <div className="text-center"> <Spin tip="Loading...">
            </Spin></div>}
            {this.props.reportEvents.fetchReportSuccess &&
              <div className="form-group">
                {this.props.reportEvents.fetchReportSuccess == true && !this.state.modifySearch && <> <button type="button" className="btn btn-outline-primary" onClick={() => this.showHide()}>{this.state.showSummary == false ? "Generate Summary" : "Hide Summary"}</button>
                  {/* <span className="mr-2">
                    <span className="mr-2"><small className="ml-10"><b>Show Test Transactions</b></small></span>
                    <Switch checked={this.state.showTestTxs} onChange={(check) => {
                      this.setState({ showTestTxs: !this.state.showTestTxs })
                    }} />
                  </span> */}
                </>}
              </div>}
            {(this.props.reportEvents.fetchReportSuccess == true) && <ReportingSummary showTestTxs={this.state.showTestTxs} transactionReportData={this.props.reportEvents.transactionReportSingleDataModel} showSummary={this.state.showSummary} />}
            {(this.props.reportEvents.fetchReportSuccess) && <TransactionReportTable
              showTestTxs={this.state.showTestTxs}
              myTableData={tableData}
              transactionReportData={this.props.reportEvents.transactionReportSingleDataModel}
              myTableColumns={columns} />}
          </div>
          <Footer
            isHideCancel={true}
            isDisabled={!(this.state.selectedEvents && this.state.valFromDate && this.state.valToDate)}
            saveText={this.props.reportEvents.fetchReportSuccess ? 'Update' : 'Submit'}
            isSaveRequest={this.props.reportEvents.isLoading}
            onSubmit={() => { this.getTransactionReport() }} />
        </div>
      </div>;
    }
    else {
      return <Newloader />
    }
  }
}

export default connect(
  (state: IApplicationState) => ({
    reportEvents: state.transactionReport,
    session: state.session,
  }),
  (dispatch) => bindActionCreators({
    ...transReportStore.actionCreators,
    ...sessionActionCreators,
  }, dispatch)
)(TransactionReportSingleDataModel);
