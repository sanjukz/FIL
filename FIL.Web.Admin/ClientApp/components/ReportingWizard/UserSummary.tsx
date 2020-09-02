import * as React from 'react';
import { filteredTestUsers } from "../../utils/Reporting";
import ReactTable from "react-table";
import "react-table/react-table.css";
import _ from 'lodash';
import Select from 'react-select';

var dynamicCurrencyHeader;
var masterPivotColumn = [];
var pivotBy = [];
const columns_options = [{ value: 'signUpMethod', label: 'Sign Up Method' }, { value: 'isOptIn', label: 'Opt In' }, { value: 'isCreator', label: 'Creator' }, { value: 'ipCity', label: 'IP Based City' },
{ value: 'ipState', label: 'IP Based State' }, { value: 'ipCountry', label: 'IP Based Country' }];

export default class UserSummary extends React.PureComponent<any, any>{
  constructor(props) {
    super(props); {
      this.state = {
        userId: "",
        roleId: "",
        selectedRows: null,
        selectedColumns: null
      }
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
          accessor: item.value
        }
      } else {
        pivotColumns = {
          Header: item.label,
          width: 150,
          id: item.value,
          accessor: item.value
        }
      }

      columnData.push(pivotColumns);
      pivotByColumn.push(item.value);
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
  render() {
    var summaryColumns = [], that = this;

    var data = this.props.tableRowData;
    if (!this.props.isShowTest) {
      data = filteredTestUsers(data);
    }
    data.map((item) => {
      item.count = 1;
      if (item.signUpMethod == "Regular") {
        item.signUpMethod = "Email"
      }
    })
    let columnInfo = {
      Header: 'Count',
      accessor: 'count',
      minWidth: 170,
      className: "text-center",
      aggregate: function (val) {
        var sum = that.sum(val, false);
        return sum;
      },
    }
    summaryColumns.push(columnInfo);


    return <>
      <div className="row">
        <div className="col-sm-12">
          <div className="form-group">
            < label > Select columns for which you want summary:</label>
            < Select
              isMulti
              options={columns_options}
              onChange={this.onChangeColumns}
              value={this.state.selectedColumns}
            />
          </div>
        </div>
      </div>



      {(this.state.selectedColumns != null) && <>
        <section className="summary-1">
          <div className="table-responsive seat-layout-tbl">
            <ReactTable
              data={data}
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
      </>}

    </>
  }
}