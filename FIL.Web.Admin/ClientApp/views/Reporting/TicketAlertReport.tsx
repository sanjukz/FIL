import * as React from 'react';
import { IApplicationState } from '../../stores';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router-dom';
import { MyFeels } from '../../components/Dashboard';
import { View } from '../../components/Dashboard/utils';
import * as ReportStore from "../../stores/Reporting/Report";

type ReportStore = ReportStore.ITransactionReportComponentState
  & typeof ReportStore.actionCreators
  & RouteComponentProps<{}>;

function FeelUserReport(props: ReportStore) {
  const [tableRowData, setTableRowData] = React.useState([]);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [event, setEvent] = React.useState(null);

  React.useEffect(() => {
    props.requestTicketAlertEventData();
  }, []);

  React.useEffect(() => {
    if (props.ticketAlertReport.ticketAlertData.length > 0) {
      setTableRowData(props.ticketAlertReport.ticketAlertData);
    }
  }, [props.fetchReportSuccess]);

  React.useEffect(() => {
    let seachedData = props.ticketAlertReport.ticketAlertData.filter(
      (item) => {
        let phoneNumber = item.phoneNumber ? item.phoneNumber.toLowerCase() : "";
        return (item.firstName.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
          || item.lastName.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
          || item.email.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1
          || phoneNumber.indexOf(searchTerm.toLowerCase()) > -1
        )
      }
    );
    setTableRowData(seachedData);
  }, [searchTerm]);

  const getComponentBody = () => {
    return <MyFeels
      isFetchSuccess={props.fetchReportSuccess}
      isLoading={props.isLoading}
      setSearchTerm={setSearchTerm}
      tableRowData={tableRowData}
      searchValue={searchTerm}
      data={props.ticketAlertReport.ticketAlertData}
      view={View.TicketAlert}
      selectedEvent={event}
      eventOptions={eventSelect}
      handleTicketAlertEventSelection={(e) => {
        setEvent(e);
        setSearchTerm('')
        props.requestTransactionAlertReportData(e.split('X')[1]);
      }}
    />
  }

  const eventSelect = props.allEvents.events.filter((val) => { return val.isFeel }).map(function (event) {
    if (event.altId != null) {
      return { "altId": event.altId, "label": event.name };
    }
    else {
      return { "label": event };
    }
  });

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