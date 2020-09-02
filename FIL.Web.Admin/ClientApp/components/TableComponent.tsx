import * as React from "react";
import ReactTable from "react-table";
import "react-table/react-table.css";
import { autobind } from "core-decorators";

export default class TableComponent extends React.Component<any, any> {
    @autobind
    public exportToExcel() {
        const data = this.props.myTableData;
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
    public render() {

        const data = this.props.myTableData;
        const headers = this.props.myTableColumns;
        const filterCaseInsensitive = (filter, row) => {
            const id = filter.pivotId || filter.id;
            return (
                row[id] !== undefined ?
                    String(row[id].toString().toLowerCase()).includes(filter.value.toString().toLowerCase())
                    :
                    true
            );
        };

        var defaultPageSize;
        if (data) {
            if (data.length <= 1) {
                defaultPageSize = 1
            }
            else {
                defaultPageSize = 100
            }
        }
        var PageSizeOptions = [20, 25, 50, 100, 1000, 5000, 10000, 50000, 100000];

        return (<div>
            <section className="summary-1  mt-20">
                <div className="container-fluid">
                    <div className="table-responsive bg-light mb-20 approve-tbl">
                        {data ? <ReactTable
                            data={data}
                            columns={headers}
                            defaultPageSize={defaultPageSize}
                            minRows={0}
                            pageSizeOptions={PageSizeOptions}
                            filterable={true}
                            previousText='Previous'
                            nextText='Next'
                            loadingText='Loading...'
                            noDataText='No records found'
                            pageText='Page'
                            ofText='of'
                            rowsText='rows'
                            className='table table-striped table-condensed mb-0'
                            defaultFilterMethod={filterCaseInsensitive}
                        /> : null}
                    </div>
                </div>
                {/* <Pagination totalRecords={this.props.noOfRecords} pageNo={this.props.currentPageNumber} ShowRecords={this.props.ShowRecords} OnChangeNoRecordsPerPage={this.props.OnChangeNoRecordsPerPage} noRecordsPerPage={this.props.noRecordsPerPage} OnChangeNoPaginationInput={this.props.OnChangeNoPaginationInput}/> */}
            </section>
            <footer className="d-none footer-export bg-white bdr-top row p-3">
                <div className="dropup pull-right">
                    {this.props.isMyFeel == false || this.props.isMyFeel == undefined ? <button className="btn btn-primary dropdown-toggle" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Export
                        <span className="caret" style={{ display: "inline-block", marginLeft: "4px" }}> </span>
                    </button> : null}
                    <div className="dropdown-menu" aria-labelledby="dropdownMenu2">

                        < a className="dropdown-item" href="javascript:void(0);" onClick={this.exportToExcel}>
                            <i className="fa fa-download text-primary" aria-hidden="true"></i>
                            &nbsp; Save as excel</a>
                    </div>
                </div>
            </footer>
        </div>);
    }
}
