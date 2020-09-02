import * as React from 'react';
import { IApplicationState } from '../stores';
import { connect } from 'react-redux';
import * as numeral from "numeral";
import { actionCreators as sessionActionCreators, ISessionProps } from 'shared/stores/Session';
import { bindActionCreators } from 'redux';
import { RouteComponentProps } from 'react-router-dom';
import * as SellerStore from '../stores/Seller';
import { SalesGraph, MyFeels, Overview, IBodyRowData, getTableBodyData } from '../components/Dashboard';
import { View, Experience } from '../components/Dashboard/utils';
import { parseDateLocal, EVENT_STATUS } from '../components/Dashboard/Table/Provider/BodyProvider';

type SellerProps = SellerStore.ISellerProps &
  typeof SellerStore.actionCreators &
  ISessionProps &
  typeof sessionActionCreators &
  RouteComponentProps<any>;

function DashBoard(props: SellerProps) {
  const [tableRowData, setTableRowData] = React.useState([]);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [status, setStatus] = React.useState();
  const [column, setColumnFilter] = React.useState({});

  React.useEffect(() => {
    if (props.session.user && props.session.user.altId) {
      props.requestSellerEventData(props.session.user.altId);
    }
  }, [props.session.isAuthenticated]);

  React.useEffect(() => {
    if (props.seller.events.myFeels.length > 0) {
      setTableRowData(props.seller.events.myFeels);
    }
  }, [props.seller.isFetchSuccess]);

  React.useEffect(() => {
    let filteredData = props.seller.events.myFeels;
    let searchColumn = column as any;
    let seachedData = filteredData.filter(
      (item) => {
        let eventStatus = item.isPastEvent ? "Past" : EVENT_STATUS[item.isEnabled ? 5 : item.eventStatusId ? item.eventStatusId - 1 : 2];
        return ((item.name.toLowerCase().indexOf(searchColumn.name ? searchColumn.name.toLowerCase() : '') > -1 || item.eventStartDateTimeString.toLowerCase().indexOf(searchColumn.name ? searchColumn.name.toLowerCase() : '') > -1)
          && eventStatus.toLowerCase().indexOf(searchColumn.eventStatusId ? searchColumn.eventStatusId.toLowerCase() : '') > -1
          && getStatusFilter(item)
        )
      }
    );
    setTableRowData(seachedData);
  }, [searchTerm]);

  const getStatusFilter = (item) => {
    if (status == 0 || !status) {
      return true
    } if (status == 6) {
      return item.isEnabled
    } if (status == 6) {
      return item.isPastEvent
    } else {
      return item.eventStatusId == status
    }
  }

  return (
    <>
      <div className="card border-0 right-cntent-area pb-5 bg-light">
        <div className="card-body bg-light p-0">
          <MyFeels
            isFetchSuccess={props.seller.isFetchSuccess}
            isLoading={props.seller.isLoading}
            setSearchTerm={setSearchTerm}
            setStatus={(enumValue) => {
              let filter = column as any;
              filter[`filterStatus`] = enumValue;
              setColumnFilter(filter)
              setStatus(enumValue)
              setSearchTerm(JSON.stringify(filter));
            }}
            view={View.MyFeels}
            tableRowData={tableRowData}
            data={props.seller.events.myFeels}
            searchValue={searchTerm}
            handleSearchColumn={(enumValue, input) => {
              let filter = column as any;
              let userColumn = Experience[enumValue] as any;
              filter[`${userColumn}`] = input;
              setColumnFilter(filter)
              setSearchTerm(JSON.stringify(filter));
            }}
          />
        </div>
      </div>
    </>
  );
}

export default connect(
  (state: IApplicationState) => ({ session: state.session, seller: state.sellar }),
  (dispatch) => bindActionCreators({ ...SellerStore.actionCreators }, dispatch)
)(DashBoard);
