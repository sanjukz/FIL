import * as React from 'react';
import { IApplicationState } from '../../stores';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router-dom';
import { MyFeels } from '../../components/Dashboard';
import Newloader from "../../components/NewLoader/NewLoader";
import { View, UserReportColumn, TransactionLocatorEnum } from '../../components/Dashboard/utils';
import * as ReportStore from "../../stores/Reporting/Report";
import { parseDateLocal } from '../../components/Dashboard/Table/Provider/BodyProvider';
import * as numeral from "numeral";
import { filteredTestUsers } from "../../utils/Reporting";

type ReportStore = ReportStore.ITransactionReportComponentState
  & typeof ReportStore.actionCreators
  & RouteComponentProps<{}>;

function TransactionLocatorReport(props: ReportStore) {
  const [tableRowData, setTableRowData] = React.useState([]);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [isShowTest, setIsShowTest] = React.useState(false);
  const [showSummary, handleSummary] = React.useState(false);
  const [column, setColumnFilter] = React.useState({});

  React.useEffect(() => {
    if (props.transactionLocatorResponseViewModel.filTransactionLocator.transactionData.length > 0) {
      setTableRowData(props.transactionLocatorResponseViewModel.filTransactionLocator.transactionData);
    }
  }, [props.isLoading]);

  React.useEffect(() => {
    let searchColumn = column as any;

    let seachedData = props.transactionLocatorResponseViewModel.filTransactionLocator.transactionData.filter(
      (item) => {
        let phoneNumber = item.phoneNumber ? item.phoneNumber.toLowerCase() : "";
        let ipAddress = item.ipAddress ? item.ipAddress.toLowerCase() : "";
        let ipCity = item.ipCity ? item.ipCity.toLowerCase() : "";
        let ipCountry = item.ipCountry ? item.ipCountry.toLowerCase() : "";
        let ipState = item.ipState ? item.ipState.toLowerCase() : "";
        let promoCode = item.promoCode ? item.promoCode.toString().toLowerCase() : "";
        let streamAltId = item.streamAltId ? item.streamAltId.toString().toLowerCase() : "";
        let payConfNumber = item.payConfNumber ? item.payConfNumber.toString().toLowerCase() : "";

        return (item.transactionId.toString().toLowerCase().indexOf(searchColumn.transactionId ? searchColumn.transactionId.toLowerCase() : '') > -1
          && item.eventName.toLowerCase().indexOf(searchColumn.eventName ? searchColumn.eventName.toLowerCase() : '') > -1
          && item.firstName.toLowerCase().indexOf(searchColumn.firstName ? searchColumn.firstName.toLowerCase() : '') > -1
          && item.lastName.indexOf(searchColumn.lastName ? searchColumn.lastName.toLowerCase() : '') > -1
          && phoneNumber.indexOf(searchColumn.phoneNumber ? searchColumn.phoneNumber.toLowerCase() : '') > -1
          && item.email.indexOf(searchColumn.email ? searchColumn.email.toLowerCase() : '') > -1
          && item.transactionStatus.toLowerCase().indexOf(searchColumn.transactionStatus ? searchColumn.transactionStatus.toLowerCase() : '') > -1
          && item.totalTicket.toString().indexOf(searchColumn.totalTicket ? searchColumn.totalTicket.toLowerCase() : '') > -1
          && item.ticketCategoryName.toLowerCase().indexOf(searchColumn.ticketCategoryName ? searchColumn.ticketCategoryName.toLowerCase() : '') > -1
          && item.currencyCode.toLowerCase().indexOf(searchColumn.currencyCode ? searchColumn.currencyCode.toLowerCase() : '') > -1
          && promoCode.toString().indexOf(searchColumn.promoCode ? searchColumn.promoCode.toLowerCase() : '') > -1
          && item.discountAmount.toString().indexOf(searchColumn.discountAmount ? searchColumn.discountAmount.toLowerCase() : '') > -1
          && item.grossTicketAmount.toString().indexOf(searchColumn.grossTicketAmount ? searchColumn.grossTicketAmount.toLowerCase() : '') > -1
          && item.netTicketAmount.toString().indexOf(searchColumn.netTicketAmount ? searchColumn.netTicketAmount.toLowerCase() : '') > -1
          && payConfNumber.indexOf(searchColumn.payConfNumber ? searchColumn.payConfNumber.toLowerCase() : '') > -1
          && streamAltId.indexOf(searchColumn.streamAltId ? searchColumn.streamAltId.toLowerCase() : '') > -1
          && ipAddress.indexOf(searchColumn.ipAddress ? searchColumn.ipAddress.toLowerCase() : '') > -1
          && ipCity.indexOf(searchColumn.ipCity ? searchColumn.ipCity.toLowerCase() : '') > -1
          && ipCountry.indexOf(searchColumn.ipCountry ? searchColumn.ipCountry.toLowerCase() : '') > -1
          && ipState.indexOf(searchColumn.ipState ? searchColumn.ipState.toLowerCase() : '') > -1
          && (parseDateLocal(item.transactionCreatedUtc).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + parseDateLocal(item.transactionCreatedUtc).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(parseDateLocal(item.transactionCreatedUtc).getHours()).format('00') + ":" + numeral(parseDateLocal(item.transactionCreatedUtc).getMinutes()).format('00')).toLowerCase().indexOf(searchColumn.transactionCreatedUtc ? searchColumn.transactionCreatedUtc.toLowerCase() : '') > -1
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
      isFetchSuccess={props.transactionLocatorResponseViewModel.filTransactionLocator.transactionData.length > 0}
      isLoading={props.isLoading}
      setSearchTerm={setSearchTerm}
      tableRowData={tableRowData}
      searchValue={searchTerm}
      data={props.transactionLocatorResponseViewModel.filTransactionLocator.transactionData}
      view={View.TransactionLocator}
      isShowTest={isShowTest}
      setIsShowTest={setIsShowTest}
      handleSummary={handleSummary}
      showSummary={showSummary}
      apiResponseData={props.transactionLocatorResponseViewModel.filTransactionLocator.transactionData}
      fetchReportSuccess={props.fetchReportSuccess}
      onSubmitTransactionLocatorForm={(query) => {
        props.requestTransactionLocator(query);
      }}
      handleSearchColumn={(enumValue, input) => {
        let filter = column as any;
        let userColumn = TransactionLocatorEnum[enumValue] as any;
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
)(TransactionLocatorReport);
