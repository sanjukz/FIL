import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { autobind } from "core-decorators";
import "../scss/FILSuite/_transaction-report.scss";
import { filteredTestTransactions } from "../utils/Reporting";

export default class TableComponent extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      tableData: [],
      alltableData: [],
      isCustomPagination: false,
      isLoaded: false,
    }
  }

  componentDidMount() {
    var tableData = [];
    if (this.props.transactionReportData != undefined) {
      var data = this.props.transactionReportData.transactionReports.transactionData;
      var columns = this.props.myTableColumns;
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
    }
    this.setState({ isLoaded: true, alltableData: tableData });
    //alert("Data hs been loaded...");
  }

  public getExcelData() {
    var tableData = [];
    if (this.props.transactionReportData != undefined) {
      var data = this.props.transactionReportData.transactionReports.transactionData;
      var columns = this.props.myTableColumns;
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
    }
    return tableData;
  }

  @autobind
  public exportToExcel() {
    const data = this.getExcelData();
    const headers = this.props.myTableColumns;
    var row = [];
    var colNames = [];
    var html = "<table border='1'><tr>";
    var n = headers.length;

    for (var i = 0; i < n; i++) {
      html += "<th>";
      html += headers[i].Header + "</th>";
    }
    html += "</tr>";

    var listPropertyNames;
    data.forEach(function (key, value) {
      html += "<tr>";
      Object.keys(key).forEach(function (subKey, index) {
        if (key[subKey] == '') {
          html += "<td align='right'>" + '-' + "</td>";
        }
        else if (subKey == 'barcodeNo') {
          var barcode = key[subKey];
          html += "<td>&nbsp;" + barcode + "</td>";
        }
        else {
          html += "<td align='right'>" + key[subKey] + "</td>";
        }
      });
      html += "</tr>";
    });

    html += "</table>";

    var a = document.createElement('a');
    a.id = 'ExcelDL';

    var fileName = "Report.xls";
    var blob = new Blob([html], {
      type: "application/csv;charset=utf-8;"
    });

    if (window.navigator.msSaveBlob) {
      // FOR IE BROWSER
      navigator.msSaveBlob(blob, fileName);
    } else {
      // FOR OTHER BROWSERS
      var csvUrl = URL.createObjectURL(blob);
      a.href = csvUrl;
      a.download = fileName;
    }

    document.body.appendChild(a);
    a.click();
    document.getElementById('ExcelDL').remove();
  }

  @autobind
  fetchData(state, instance) {
    var page = state.page + 1;
    var pageSize = state.pageSize;
    var tableData = [];
    if (this.props.transactionReportData != undefined) {
      var data = this.props.transactionReportData.transactionReports.transactionData.slice(0, (page * pageSize * 2));
      var columns = this.props.myTableColumns;
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
    }
    this.setState({ tableData: tableData, isCustomPagination: true })
  }

  public render() {
    var data = this.props.myTableData;
    if (this.state.isCustomPagination) {
      data = this.state.tableData
    }
    const headers = this.props.myTableColumns;
    var pages = 0;

    const filterCaseInsensitive = (filter, row) => {
      const id = filter.pivotId || filter.id;
      return (
        row[id] !== undefined ?
          String(row[id].toString().toLowerCase()).includes(filter.value.toString().toLowerCase())
          :
          true
      );
    };
    var isFilter = false;
    if (this.state.isLoaded) {
      data = this.state.alltableData;
      isFilter = true;
    }

    var defaultPageSize;
    if (data.length <= 1) {
      defaultPageSize = 1
    }
    else {
      defaultPageSize = 50
    }
    if (this.props.transactionReportData != undefined) {
      pages = Math.ceil(this.props.transactionReportData.transactionReports.transactionData.length / 50);
    }


    var PageSizeOptions = [20, 25, 50, 100, 1000, 5000, 10000, 50000, 100000];

    ////filtering test transactions
    //if (!this.props.showTestTxs) {
    //  data = filteredTestTransactions(data);
    //}

    return <div>
      <section className="summary-1 mt-4">
        <span className="d-block mb-2 ">Detailed Report</span>

        <div className="table-responsive seat-layout-tbl">
          <ReactTable
            data={data}
            columns={headers}
            defaultPageSize={defaultPageSize}
            minRows={0}
            pageSizeOptions={PageSizeOptions}
            filterable={isFilter}
            pages={pages}
            previousText='Previous'
            nextText='Next'
            loadingText='Loading...'
            noDataText='No records found'
            pageText='Page'
            ofText='of'
            rowsText='rows'
            className='table table-striped table-condensed mb-0'
            defaultFilterMethod={filterCaseInsensitive}
            onFetchData={this.fetchData}
          />
        </div>
      </section>
      <footer className="footer-export bg-white bdr-top row p-3">
        <div className="dropup pull-right">
          <button className="btn btn-primary dropdown-toggle" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
            Export
                <span className="caret" style={{ display: "inline-block", marginLeft: "4px" }}> </span>
          </button>
          <div className="dropdown-menu" aria-labelledby="dropdownMenu2">
            <a className="dropdown-item" href="javascript:void(0);" onClick={this.exportToExcel}>
              <i className="fa fa-download text-primary" aria-hidden="true"></i>
                            &nbsp; Save as excel</a>
          </div>
        </div>
      </footer>
    </div>;

  }
}
