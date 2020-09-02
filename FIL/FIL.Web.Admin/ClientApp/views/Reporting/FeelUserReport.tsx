import * as React from 'react';
import { IApplicationState } from '../../stores';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router-dom';
import { MyFeels } from '../../components/Dashboard';
import Newloader from "../../components/NewLoader/NewLoader";
import { View, UserReportColumn } from '../../components/Dashboard/utils';
import * as ReportStore from "../../stores/Reporting/Report";
import { parseDateLocal } from '../../components/Dashboard/Table/Provider/BodyProvider';
import * as numeral from "numeral";
import { filteredTestUsers } from "../../utils/Reporting";

type ReportStore = ReportStore.ITransactionReportComponentState
  & typeof ReportStore.actionCreators
  & RouteComponentProps<{}>;

function FeelUserReport(props: ReportStore) {
  const [tableRowData, setTableRowData] = React.useState([]);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [isShowTest, setIsShowTest] = React.useState(false);
  const [showSummary, handleSummary] = React.useState(false);
  const [column, setColumnFilter] = React.useState({});

  React.useEffect(() => {
    props.requestFeelUsers();
  }, []);

  React.useEffect(() => {
    if (props.feelUsers.feelUsers.length > 0) {
      let filteredData = filteredTestUsers(props.feelUsers.feelUsers);
      setTableRowData(filteredData);
    }
  }, [props.feelUsers.feelUsers.length > 0]);

  React.useEffect(() => {
    let filteredData = [];
    if (!isShowTest) {
      filteredData = filteredTestUsers(props.feelUsers.feelUsers);
    } else {
      filteredData = props.feelUsers.feelUsers;
    }
    let searchColumn = column as any;

    let seachedData = filteredData.filter(
      (item) => {
        let signupMethod = item.signUpMethod.toLowerCase() == "regular" ? "email" : item.signUpMethod.toLowerCase();
        let phoneNumber = item.phoneNumber ? item.phoneNumber.toLowerCase() : "";
        let ipAddress = item.ipAddress ? item.ipAddress.toLowerCase() : "";
        let ipCity = item.ipCity ? item.ipCity.toLowerCase() : "";
        let ipCountry = item.ipCountry ? item.ipCountry.toLowerCase() : "";
        let ipState = item.ipState ? item.ipState.toLowerCase() : "";
        let optIn = item.isOptIn;
        return (item.firstName.toLowerCase().indexOf(searchColumn.firstName ? searchColumn.firstName.toLowerCase() : '') > -1
          && item.lastName.toLowerCase().indexOf(searchColumn.lastName ? searchColumn.lastName.toLowerCase() : '') > -1
          && item.email.toLowerCase().indexOf(searchColumn.email ? searchColumn.email.toLowerCase() : '') > -1
          && phoneNumber.indexOf(searchColumn.phoneNumber ? searchColumn.phoneNumber.toLowerCase() : '') > -1
          && ipAddress.indexOf(searchColumn.ipAddress ? searchColumn.ipAddress.toLowerCase() : '') > -1
          && ipCity.indexOf(searchColumn.ipCity ? searchColumn.ipCity.toLowerCase() : '') > -1
          && ipCountry.indexOf(searchColumn.ipCountry ? searchColumn.ipCountry.toLowerCase() : '') > -1
          && ipState.indexOf(searchColumn.ipState ? searchColumn.ipState.toLowerCase() : '') > -1
          && signupMethod.indexOf(searchColumn.signUpMethod ? searchColumn.signUpMethod.toLowerCase() : '') > -1
          && optIn.toLowerCase().indexOf(searchColumn.isOptIn ? searchColumn.isOptIn.toLowerCase() : '') > -1
          && (parseDateLocal(item.createdDate).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + parseDateLocal(item.createdDate).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(parseDateLocal(item.createdDate).getHours()).format('00') + ":" + numeral(parseDateLocal(item.createdDate).getMinutes()).format('00')).toLowerCase().indexOf(searchColumn.createdDate ? searchColumn.createdDate.toLowerCase() : '') > -1
        )
      }
    );
    setTableRowData(seachedData);
  }, [searchTerm]);

  React.useEffect(() => {
    if (isShowTest) {
      setTableRowData(props.feelUsers.feelUsers);
    } else {
      let filteredData = filteredTestUsers(props.feelUsers.feelUsers);
      setTableRowData(filteredData);
    }
  }, [isShowTest]);

  const getComponentBody = () => {
    return <MyFeels
      isFetchSuccess={props.feelUsers.feelUsers.length > 0}
      isLoading={props.isLoading}
      setSearchTerm={setSearchTerm}
      tableRowData={tableRowData}
      searchValue={searchTerm}
      data={props.feelUsers.feelUsers}
      view={View.UserReport}
      isShowTest={isShowTest}
      setIsShowTest={setIsShowTest}
      handleSummary={handleSummary}
      showSummary={showSummary}
      handleSearchColumn={(enumValue, input) => {
        let filter = column as any;
        let userColumn = UserReportColumn[enumValue] as any;
        filter[`${userColumn}`] = input;
        setColumnFilter(filter)
        setSearchTerm(JSON.stringify(filter));
      }}
    />
  }
  if (props.isLoading) {
    return <Newloader />
  }
  return (
    <>
      <div className="card border-0 right-cntent-area pb-5 bg-light">
        <div className="card-body bg-light p-0">
          <>{getComponentBody()}</>
        </div>
      </div>
    </>
  );
}

export default connect(
  (state: IApplicationState) => state.transactionReport,
  ReportStore.actionCreators
)(FeelUserReport);
