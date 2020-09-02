import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import _ from 'lodash';
import Select from 'react-select';
import { filteredTestTransactions } from "../../utils/Reporting";

var dynamicCurrencyHeader;
var masterPivotColumn = [];
var pivotBy = [];

export default class ReportingSummaryTable extends React.Component<any, any>{
  constructor(props) {
    super(props);
    this.state = {
      userId: "",
      roleId: "",
      selectedRows: null,
      selectedColumns: null
    }
  }

  public isExistInDataModel(dbFieldName, jsonData) {
    var isExistInCurrency = false;
    for (var i in jsonData) {
      var key = i;
      var val = jsonData[i];
      for (var j in val) {
        var sub_key = j;
        var sub_val = val[j];
        if (sub_key == dbFieldName) {
          isExistInCurrency = true;
        }
      }
    }
    return isExistInCurrency;
  }

  onChangeRows = (selectedRows) => {
    this.setState({ selectedRows });
    if (selectedRows.length == 0) {
      this.setState({ selectedRows: null });
    }

  }

  onChangeColumns = (selectedColumns) => {
    this.setState({ selectedColumns });
    if (selectedColumns.length == 0) {
      masterPivotColumn = [];
      pivotBy = [];
      this.setState({ selectedColumns: null });
    }
    dynamicCurrencyHeader = selectedColumns.map(item => {
      return <th>{item.label}</th>
    });
    var pivotColumns;
    var columnData = [];
    var pivotByColumn = [];
    selectedColumns.map(function (item, index) {
      if (index == 0) {
        pivotColumns = {
          Header: item.label,
          width: 150,
          accessor: item.dbFieldName
        }
      } else {
        pivotColumns = {
          Header: item.label,
          width: 150,
          id: item.dbFieldName,
          accessor: item.dbFieldName
        }
      }

      columnData.push(pivotColumns);
      pivotByColumn.push(item.dbFieldName);
    });
    masterPivotColumn = columnData;
    pivotBy = pivotByColumn;
  }

  public sum(val, isFixed) {
    var sum = 0;
    for (var i = 0; i < val.length; i++) {
      sum = sum + val[i];
    }
    if (!isFixed) {
      return +sum;
    } else {
      return +((+sum).toFixed(2));
    }

  }


  public render() {
    var currencySummary;
    var channelWiseSummary;
    var ticketTypeWiseSummary;
    var venueWiseSummary;
    var eventWiseSummary;

    var currencyHeader = [];
    var currencyRowData = [];

    var channelHeader = [];
    var channelRowData = [];

    var ticketTypeHeader = [];
    var ticketTypeRowData = [];

    var venueHeader = [];
    var venueRowData = [];

    var eventHeader = [];
    var eventRowData = [];

    var that = this;

    if (this.props.transactionReportData != undefined && this.props.showSummary) {
      this.props.transactionReportData.transactionReports.summaryColumns.map(function (item) {

        var currencyData = that.props.transactionReportData.transactionReports.currencyWiseSummary;
        var currencyColumnData = [];
        var isExist = that.isExistInDataModel(item.dbFieldName, currencyData);

        if (isExist) {
          currencyHeader.push(item.displayName);
          currencyData.map(function (val) {
            currencyColumnData.push(val[item.dbFieldName]);
          })
        }

        if (currencyColumnData.length > 0) {
          currencyRowData.push(currencyColumnData);
        }


        var channelData = that.props.transactionReportData.transactionReports.channelWiseSummary;
        var channelColumnData = [];
        isExist = that.isExistInDataModel(item.dbFieldName, channelData);

        if (isExist) {
          channelHeader.push(item.displayName);
          channelData.map(function (val) {
            channelColumnData.push(val[item.dbFieldName]);
          })
        }

        if (channelColumnData.length > 0) {
          channelRowData.push(channelColumnData);
        }


        var ticketTypeData = that.props.transactionReportData.transactionReports.ticketTypeWiseSummary;
        var ticketTypeColumnData = [];
        isExist = that.isExistInDataModel(item.dbFieldName, ticketTypeData);

        if (isExist) {
          ticketTypeHeader.push(item.displayName);
          ticketTypeData.map(function (val) {
            ticketTypeColumnData.push(val[item.dbFieldName]);
          })
        }

        if (ticketTypeColumnData.length > 0) {
          ticketTypeRowData.push(ticketTypeColumnData);
        }

        var venueData = that.props.transactionReportData.transactionReports.venueWiseSummary;
        var venueColumnData = [];
        isExist = that.isExistInDataModel(item.dbFieldName, venueData);

        if (isExist) {
          venueHeader.push(item.displayName);
          venueData.map(function (val) {
            venueColumnData.push(val[item.dbFieldName]);
          })
        }

        if (venueColumnData.length > 0) {
          venueRowData.push(venueColumnData);
        }

        var eventData = that.props.transactionReportData.transactionReports.eventWiseSummary;
        var eventColumnData = [];
        isExist = that.isExistInDataModel(item.dbFieldName, eventData);

        if (isExist) {
          eventHeader.push(item.displayName);
          eventData.map(function (val) {
            eventColumnData.push(val[item.dbFieldName]);
          })
        }

        if (eventColumnData.length > 0) {
          eventRowData.push(eventColumnData);
        }
      });
    }

    var currencySummaryMasterData = currencyRowData;
    var currencySummaryData;
    if (currencyRowData.length > 0) {
      currencySummaryData = currencyRowData[0].map((item, index) => {
        return (
          <tr>
            {currencySummaryMasterData.map((i, subIndex) => {
              return <td>{currencySummaryMasterData[subIndex][index]}</td>
            })}
          </tr>
        )
      });
    }

    var currencySummaryHeaders = currencyHeader.map(item => {
      return <th>{item}</th>
    });

    var channelSummaryMasterData = channelRowData;
    var channelSummaryData;
    if (channelRowData.length > 0) {
      channelSummaryData = channelRowData[0].map((item, index) => {
        return (
          <tr>
            {channelSummaryMasterData.map((i, subIndex) => {
              return <td>{channelSummaryMasterData[subIndex][index]}</td>
            })}
          </tr>
        )
      });
    }

    var channelSummaryHeaders = channelHeader.map(item => {
      return <th>{item}</th>
    });

    var ticketTypeSummaryMasterData = ticketTypeRowData;
    var ticketTypeSummaryData;
    if (ticketTypeRowData.length > 0) {
      ticketTypeSummaryData = ticketTypeRowData[0].map((item, index) => {
        return (
          <tr>
            {ticketTypeSummaryMasterData.map((i, subIndex) => {
              return <td>{ticketTypeSummaryMasterData[subIndex][index]}</td>
            })}
          </tr>
        )
      });
    }

    var ticketTypeSummaryHeaders = ticketTypeHeader.map(item => {
      return <th>{item}</th>
    });

    var venueSummaryMasterData = venueRowData;
    var venueSummaryData;
    if (venueRowData.length > 0) {
      venueSummaryData = venueRowData[0].map((item, index) => {
        return (
          <tr>
            {venueSummaryMasterData.map((i, subIndex) => {
              return <td>{venueSummaryMasterData[subIndex][index]}</td>
            })}
          </tr>
        )
      });
    }

    var venueSummaryHeaders = venueHeader.map(item => {
      return <th>{item}</th>
    });


    var eventSummaryMasterData = eventRowData;
    var eventSummaryData;
    if (eventRowData.length > 0) {
      eventSummaryData = eventRowData[0].map((item, index) => {
        return (
          <tr>
            {eventSummaryMasterData.map((i, subIndex) => {
              return <td>{eventSummaryMasterData[subIndex][index]}</td>
            })}
          </tr>
        )
      });
    }

    var eventSummaryHeaders = eventHeader.map(item => {
      return <th>{item}</th>
    });

    var column = [];
    var summaryColumns = [];
    if (this.props.transactionReportData != undefined) {
      this.props.transactionReportData.transactionReports.dynamicSummaryInfoColumns.map(function (item, index) {
        if (item.dbFieldName == "pricePerTicket" || item.dbFieldName == "grossTicketAmount" || item.dbFieldName == "netTicketAmountUSD" || item.dbFieldName == "discountAmount" || item.dbFieldName == "serviceCharge" || item.dbFieldName == "convenienceCharges" || item.dbFieldName == "deliveryCharges" || item.dbFieldName == "entryCount" || item.dbFieldName == "totalTransactedAmount" || item.dbFieldName == "totalTransactedAmountUSD" || item.dbFieldName == "netTicketAmount") {
          var summaryColumnInfo1 = {
            Header: item.displayName,
            accessor: item.dbFieldName,
            minWidth: 170,
            className: "right",
            aggregate: function (val) {
              var sum = that.sum(val, true);
              return sum;
            },
          }
          summaryColumns.push(summaryColumnInfo1);
        }
        else if (item.dbFieldName == "transactionId" || item.dbFieldName == "numberOfTickets") {
          var summaryColumnInfo2 = {
            Header: item.displayName,
            accessor: item.dbFieldName,
            minWidth: 170,
            className: "text-center",
            aggregate: function (val) {
              var sum = that.sum(val, false);
              return sum;
            },
          }
          summaryColumns.push(summaryColumnInfo2);
        }
        else {
          if (item.dbFieldName == "eventName" || item.dbFieldName == "eventStartDateTime") {  // replace event to place
            let header = item.dbFieldName == "eventName" ? "Place Name" : "Place Visit Date";
            let summaryColumnInfo = {
              Header: item.displayName,
              accessor: item.dbFieldName,
              minWidth: 170,
              aggregate: function (val) {
                var sum = that.sum(val, true);
                return sum;
              },
            }
            summaryColumns.push(summaryColumnInfo);
          } else {
            var summaryColumnInfo = {
              Header: item.displayName,
              accessor: item.dbFieldName,
              minWidth: 170,
              aggregate: function (val) {
                var sum = that.sum(val, true);
                return sum;
              },
            }
            summaryColumns.push(summaryColumnInfo);
          }
        }
      });

      this.props.transactionReportData.transactionReports.dynamicSummaryColumns.map(function (item, index) {
        if (item.dbFieldName != "channel") {
          let displayname = item.dbFieldName == "eventName" ? "Place Name" : item.displayName;
          var data = {
            value: index,
            label: displayname,
            dbFieldName: item.dbFieldName
          };
          column.push(data);
        }
      });
    }
    let tdata = this.props.transactionReportData.transactionReports.transactionData;
    // //filtering test transactions
    // if (!this.props.showTestTxs) {
    //   tdata = filteredTestTransactions(tdata);
    // }


    return <div>
      {
        this.props.showSummary == true ?
          <div>
            <div className="row">
              <div className="col-sm-12">
                <div className="form-group">
                  < label > Select columns for which you want summary:</label>
                  < Select
                    isMulti
                    options={column}
                    onChange={this.onChangeColumns}
                    value={this.state.selectedColumns}
                  />
                </div>
              </div>
            </div>

            {(this.props.transactionReportData != undefined && this.state.selectedColumns != null) && <div>
              <section className="summary-1">
                <div className="table-responsive seat-layout-tbl">
                  <ReactTable
                    data={tdata}
                    columns={[
                      {
                        Header: "Filter",
                        columns: masterPivotColumn
                      },
                      {
                        Header: "Summary",
                        columns: summaryColumns
                      }
                    ]}
                    pivotBy={pivotBy}
                    className="-striped -highlight"
                    minRows={0}
                  />
                  <br />
                </div>
              </section>
            </div>}

            {/*<section className="summary">
                            <div className="container-fluid container-xl">

                                <h5 className="mt-0">Currency Wise Summary</h5>
                                <div className="table-responsive bg-white mb-20 bdr-1">
                                    <table className="table table-striped table-condensed mb-0 table-prices">
                                        <thead>
                                            <tr>
                                                {currencySummaryHeaders}
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {currencySummaryData}
                                        </tbody>
                                    </table>
                                </div>

                                <h5 className="mt-20">Channel Wise Summary</h5>
                                <div className="table-responsive bg-white mb-20 bdr-1">
                                    <table className="table table-striped table-condensed mb-0 table-prices">
                                        <thead>
                                            <tr>
                                                {channelSummaryHeaders}
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {channelSummaryData}
                                        </tbody>
                                    </table>
                                </div>
                                <div>
                                    <h5 className="mt-20">Ticket Type Summary</h5>
                                    <div className="table-responsive bg-white mb-20 bdr-1">
                                        <table className="table table-striped table-condensed mb-0 table-prices">
                                            <thead>
                                                <tr>
                                                    {ticketTypeSummaryHeaders}
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {ticketTypeSummaryData}
                                            </tbody>
                                        </table>
                                    </div>
                                    <h5 className="mt-20">Venue Wise Summary</h5>
                                    <div className="table-responsive bg-white mb-20 bdr-1">
                                        <table className="table table-striped table-condensed mb-0 table-prices">
                                            <thead>
                                                <tr>
                                                    {venueSummaryHeaders}
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {venueSummaryData}
                                            </tbody>
                                        </table>
                                    </div>
                                    <h5 className="mt-20">Event Wise Summary</h5>
                                    <div className="table-responsive bg-white mb-20 bdr-1">
                                        <table className="table table-striped table-condensed mb-0 table-prices">
                                            <thead>
                                                <tr>
                                                    {eventSummaryHeaders}
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {eventSummaryData}
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </section>*/}
          </div> : ""
      }
    </div>;
  }
}
