import * as React from 'react';
import BodyRow from './Table/BodyRow';
import HeaderRow from './Table/HeaderRow';
import Spinner from '../Spinner';
import { Select, Pagination, Input } from 'antd';
import { View } from './utils';
import NavBar from './NavBar'
import UserSummary from "../ReportingWizard/UserSummary";

function MyFeels(props: any) {
  const {
    tableRowData,
    setIsShowTest,
    setStatus,
    isFetchSuccess,
    setSearchTerm,
    data,
    isLoading,
    view,
    isActive,
    onChangeEventStatus,
    handleEventStatusSection,
    handleSummary,
    handleEventTypeSection,
    handleTicketAlertEventSelection,
    handleSearchColumn,
    onSubmitTransactionLocatorForm,
    apiResponseData,
    fetchReportSuccess
  } = props;

  //Setting state for pagination
  const [current, setcurrent] = React.useState(1);
  const [minValue, setMinValue] = React.useState(0);
  const [maxValue, setMaxValue] = React.useState(15);

  const handlePageSelection = (value) => {
    if (value <= 1) {
      setMinValue(0);
      setMaxValue(15);
      setcurrent(value);
    } else {
      setMinValue((value - 1) * 15);
      setMaxValue(value * 15);
      setcurrent(value);
    }
  };

  const handleStatusSelection = (value) => {
    setcurrent(1);
    setStatus(value);
  };

  const handleSearch = (value) => {
    setcurrent(1);
    setSearchTerm(value)
  };

  const handleTestShowSwitch = (value) => {
    setcurrent(1);
    setIsShowTest(value)
  };

  const handleEventType = (value) => {
    setcurrent(1);
    handleEventTypeSection(value)
  }
  const showSummary = (value) => {
    handleSummary(value);
  }

  const handleEventStatus = (value) => {
    setcurrent(1);
    handleEventStatusSection(value)
  }

  const handleTicketAlertEventChange = (value) => {
    setcurrent(1);
    handleTicketAlertEventSelection(value)
  }

  const onChangeSearchColumn = (value, input) => {
    setcurrent(1);
    handleSearchColumn(value, input)
  }

  const handleSubmitTransactionLocatorForm = (query) => {
    setcurrent(1);
    onSubmitTransactionLocatorForm(query)
  }

  return (
    <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box dashboard-list">
      <NavBar props={{
        props,
        handleStatusSelection,
        handleSearch,
        handleEventType,
        handleTestShowSwitch,
        handleEventStatus,
        showSummary,
        handleTicketAlertEventChange,
        handleSubmitTransactionLocatorForm
      }} />

      <div className="my-feels-list">
        {props.showSummary && <UserSummary tableRowData={tableRowData} isShowTest={props.isShowTest} />}

        {isFetchSuccess && data.length > 0 ? (<>
          <div className="table-responsive">
            <table className="w-100" style={{ minWidth: "1000px" }}>
              <thead style={{ whiteSpace: "nowrap" }}>
                <HeaderRow view={view} onChangeColumnSearch={(enumValue, input) => {
                  onChangeSearchColumn(enumValue, input);
                }} />
              </thead>
              <tbody>
                {tableRowData.length > 0 && tableRowData.slice(minValue, maxValue).map((item, index) => (
                  <BodyRow
                    view={view}
                    key={index}
                    isActive={isActive}
                    onChangeEventStatus={(item, isActive) => {
                      onChangeEventStatus(item, isActive)
                    }}
                    rowData={item}
                    props={props}
                  />
                ))}
                {tableRowData.length == 0 && <td className="py-5" >
                  Search not found
                    </td>}
              </tbody>
            </table>
          </div>
          <div className="bg-light p-3 rounded"><Pagination
            defaultCurrent={1}
            defaultPageSize={15}
            current={current}
            onChange={(value) => handlePageSelection(value)}
            showSizeChanger={false}
            total={tableRowData.length}
          /></div>
        </>
        ) : (
            <>
              {isLoading ? (
                <div>
                  <Spinner />
                </div>
              ) : (
                  <></>
                )}
            </>
          )}
        {data.length == 0 && view != View.UserReport && isFetchSuccess && (
          <a href="/host-online/basics" className="btn btn-primary">
            Create New
          </a>
        )}
      </div>
    </div>
  );
}

export default MyFeels;
