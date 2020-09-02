import * as React from 'react';
import { IApplicationState } from '../stores';
import { connect } from 'react-redux';
import { actionCreators as sessionActionCreators, ISessionProps } from 'shared/stores/Session';
import { bindActionCreators } from 'redux';
import { RouteComponentProps } from 'react-router-dom';
import * as SellerStore from '../stores/Seller';
import { MyFeels } from '../components/Dashboard';
import { Switch, Modal, Spin } from 'antd';
import * as ApproveModerateStore from "../stores/ApproveModerate";
import { View, Experience } from '../components/Dashboard/utils';
import { parseDateLocal, EVENT_STATUS } from '../components/Dashboard/Table/Provider/BodyProvider';
import * as numeral from "numeral";

type SellerProps = SellerStore.ISellerProps
  & ApproveModerateStore.IApproveModerateProps
  & typeof ApproveModerateStore.actionCreators &
  typeof SellerStore.actionCreators &
  ISessionProps &
  typeof sessionActionCreators &
  RouteComponentProps<any>;

function DashBoard(props: SellerProps) {
  const [tableRowData, setTableRowData] = React.useState([]);
  const [searchTerm, setSearchTerm] = React.useState('');
  const [status, setStatus] = React.useState(0);
  const [isActive, setIsActive] = React.useState(false);
  const [eventType, setEventType] = React.useState(0);
  const [eventStatus, setEventStatus] = React.useState(0);
  const [isShowTest, setIsShowTest] = React.useState(false);
  const [column, setColumnFilter] = React.useState({});

  React.useEffect(() => {
    if (props.session.user && props.session.user.altId) {
      props.requestSellerEventData(props.session.user.altId, true, false);
    }
  }, [props.session.isAuthenticated]);

  React.useEffect(() => {
    if (props.seller.events.myFeels.length > 0) {
      let data = [];
      props.seller.events.myFeels.map((item) => {
        if (getFilterStatus(item)) {
          data.push(item);
        }
      })
      setTableRowData(data);
    }
  }, [props.seller.isFetchSuccess]);

  React.useEffect(() => {
    let seachedData = props.seller.events.myFeels.filter(
      (item) => getFilterStatus(item)
    );
    setTableRowData(seachedData);
  }, [searchTerm]);

  React.useEffect(() => {
    let seachedData = props.seller.events.myFeels.filter(
      (item) => {
        return getFilterStatus(item)
      }
    );
    setTableRowData(seachedData);
  }, [isShowTest]);

  React.useEffect(() => {
    let seachedData = props.seller.events.myFeels.filter(
      (item) => {
        return getFilterStatus(item)
      }
    );
    setTableRowData(seachedData);
  }, [eventStatus]);

  const getSearchFilter = (item) => {
    let searchColumn = column as any;
    let eventStatus = item.isPastEvent ? "Past" : EVENT_STATUS[item.isEnabled ? 5 : item.eventStatusId ? item.eventStatusId - 1 : 2];
    let role = item.roleId == 10 ? 'Admin' : 'Seller'
    return (searchTerm ? (
      (item.name.toLowerCase().indexOf(searchColumn.name ? searchColumn.name.toLowerCase() : '') > -1 || item.eventStartDateTimeString.toLowerCase().indexOf(searchColumn.name ? searchColumn.name.toLowerCase() : '') > -1)
      && (item.userEmail && item.userEmail.toLowerCase().indexOf(searchColumn.userEmail ? searchColumn.userEmail.toLowerCase() : '') > -1)
      && (parseDateLocal(item.eventCreatedDateTime).toDateString().split(' ').join(', ').toUpperCase().substring(0, 8) + parseDateLocal(item.eventCreatedDateTime).toDateString().split(' ').join(', ').toUpperCase().substring(9) + ", " + numeral(parseDateLocal(item.eventCreatedDateTime).getHours()).format('00') + ":" + numeral(parseDateLocal(item.eventCreatedDateTime).getMinutes()).format('00')).toLowerCase().indexOf(searchColumn.eventCreatedDateTime ? searchColumn.eventCreatedDateTime.toLowerCase() : '') > -1
      && eventStatus.toLowerCase().indexOf(searchColumn.eventStatusId ? searchColumn.eventStatusId.toLowerCase() : '') > -1
      && role.toLowerCase().indexOf(searchColumn.roleId ? searchColumn.roleId.toLowerCase() : '') > -1
    ) : true)
  }

  const getEventTypeFilter = (item) => {
    return (eventType == 4 ? item.masterEventType == 4 : eventType == 1 ? item.masterEventType != 4 : true)
  }

  const getEventStatusFilter = (item) => {
    return (eventStatus == 2 ? item.isPastEvent : eventStatus == 1 ? !item.isPastEvent : true)
  }

  const getFilteredTestEvents = (item) => {
    if (isShowTest) {
      return item;
    } else {
      return item.name.toLowerCase().indexOf('test') == -1
        && (item.userEmail && item.userEmail.toLowerCase().indexOf('rupalihegade'.toLowerCase()) == -1)
        && (item.userEmail && item.userEmail.toLowerCase().indexOf('chavansanjiv'.toLowerCase()) == -1)
        && (item.userEmail && item.userEmail.toLowerCase().indexOf('pratibha'.toLowerCase()) == -1)
        && (item.userEmail && item.userEmail.toLowerCase().indexOf('dhawanavi'.toLowerCase()) == -1)
        && (item.userEmail && item.userEmail.toLowerCase().indexOf('shashikant.pan'.toLowerCase()) == -1)
        && (item.userEmail && item.userEmail.toLowerCase().indexOf('vivekonline'.toLowerCase()) == -1)
    }
  }

  const getFilterStatus = (item) => {
    if (status == 0) {
      return item
        && getEventTypeFilter(item)
        && getSearchFilter(item)
        && getFilteredTestEvents(item)
        && getEventStatusFilter(item)
    } if (status == 5) {
      return item.isEnabled
        && getEventTypeFilter(item)
        && getSearchFilter(item)
        && getFilteredTestEvents(item)
        && getEventStatusFilter(item)
    } else {
      return item.eventStatusId == status
        && getEventTypeFilter(item)
        && getSearchFilter(item)
        && getFilteredTestEvents(item)
        && getEventStatusFilter(item)
    }
  }

  const getComponentBody = () => {
    return <MyFeels
      isFetchSuccess={props.seller.isFetchSuccess}
      isLoading={props.seller.isLoading}
      setSearchTerm={setSearchTerm}
      setStatus={setStatus}
      setIsShowTest={setIsShowTest}
      tableRowData={tableRowData}
      data={props.seller.events.myFeels}
      isActive={isActive}
      view={View.ApproveModerate}
      searchValue={searchTerm}
      isShowTest={isShowTest}
      handleSearchColumn={(enumValue, input) => {
        let filter = column as any;
        let userColumn = Experience[enumValue] as any;
        filter[`${userColumn}`] = input;
        setColumnFilter(filter)
        setSearchTerm(JSON.stringify(filter));
      }}
      onChangeEventStatus={(item, isActive) => {
        props.requestEnableApproveModeratePlace(item.altId, !isActive, 0, 0, (response) => {
          if (response.success) {
            Modal.success({
              title: isActive ? `Congratulations! Your experience ${item.name} has been reviewed and approved by our team. Go check it out on FeelitLIVE.com!` : `${item.name} has been Deactivated from FeelitLIVE.com`,
              centered: true,
              onOk: () => {
                props.requestSellerEventData(props.session.user.altId, true, !isActive);
              }
            });
          }
        });
      }}
      handleEventTypeSection={setEventType}
      handleEventStatusSection={setEventStatus}
    />
  }

  React.useEffect(() => {
    let filteredData = props.seller.events.myFeels.filter(
      (item: any) => {
        return getFilterStatus(item);
      }
    );
    setTableRowData(filteredData);
  }, [status]);

  React.useEffect(() => {
    let filteredData = props.seller.events.myFeels.filter(
      (item: any) => {
        return getFilterStatus(item);
      }
    );
    setTableRowData(filteredData);
  }, [eventType]);

  return (
    <>
      <div className="card border-0 right-cntent-area pb-5 bg-light">
        <div className="card-body bg-light p-0">
          <div className="col-sm-12 mb-3">
            <Switch checked={isActive} onChange={(check) => {
              if (check) {
                setIsActive(true)
                props.requestSellerEventData(props.session.user.altId, true, true);
              } else {
                setIsActive(false)
                props.requestSellerEventData(props.session.user.altId, true, false);
              }
            }} />
            <span><small className="ml-10"><b>Show Active Feels</b></small></span>
          </div>
          {props.ApproveModerate.isLoading && <div className="text-center"> <Spin tip="Loading...">
          </Spin></div>}
          {!props.ApproveModerate.isLoading && <>{getComponentBody()}</>}
        </div>
      </div>
    </>
  );
}

export default connect(
  (state: IApplicationState) => ({
    ApproveModerate: state.ApproveModerate,
    session: state.session,
    seller: state.sellar
  }),
  (dispatch) => bindActionCreators({
    ...SellerStore.actionCreators,
    ...ApproveModerateStore.actionCreators
  }, dispatch)
)(DashBoard);
