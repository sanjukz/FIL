import * as React from 'react';
import { Select, Switch } from 'antd';
import { View } from './utils';
import { Footer } from '../../views/EventManagement/Footer/FormFooter';
import { TransactionLocatorForm } from './Form/TransactionLocatorForm';

const getCommanSelection = (props: any) => {
  const { Option } = Select;
  return <><label className="mr-2">Status</label><Select showSearch style={{ width: 150 }} size={'large'} defaultValue="All" onChange={props.handleStatusSelection}>
    <Option value={0} key={0}>
      All
    </Option>
    <Option value={3} key={3}>
      Draft
    </Option>
    <Option value={4} key={4}>
      Waiting for Approval
    </Option>
    <Option value={6} key={6}>
      Published
    </Option>
  </Select></>
}

const getSelection = (props: any) => {
  const { view } = props.props;
  const { Option } = Select;
  if (view == View.ApproveModerate) {
    return <>
      <span className="mr-2">
        <span className="mr-2"><small className="ml-10"><b>Show Test Experiences</b></small></span>
        <Switch checked={props.props.isShowTest} onChange={(check) => {
          props.handleTestShowSwitch(check)

        }} />
      </span>
      <label className="mr-2">Type</label>
      <Select showSearch style={{ width: 150 }} size={'large'} defaultValue="All" onChange={props.handleEventType}>
        <Option value={0} key={0}>
          All
      </Option>
        <Option value={4} key={4}>
          Online Experiences
      </Option>
        <Option value={1} key={1}>
          In-Real Life
      </Option>
      </Select>
      <label className="mr-2">Timing</label>
      <Select showSearch style={{ width: 150 }} size={'large'} defaultValue="All" onChange={props.handleEventStatus}>
        <Option value={0} key={0}>
          All
      </Option>
        <Option value={1} key={1}>
          Current
      </Option>
        <Option value={2} key={2}>
          Past
      </Option>
      </Select>
      {getCommanSelection(props)}
    </>
  } else if (view == View.MyFeels) {
    return <>{getCommanSelection(props)}</>
  } else if (view == View.UserReport) {
    return <>
      <span className="mr-2">
        <span className="mr-2"><small className="ml-10"><b>Show Test Users</b></small></span>
        <Switch checked={props.props.isShowTest} onChange={(check) => {
          props.handleTestShowSwitch(check)

        }} />
      </span>
      <button type="button" className="btn btn-outline-primary mr-2"
        onClick={() => props.showSummary(!props.props.showSummary)}>{!props.props.showSummary ? "Generate Summary" : "Hide Summary"}</button>
    </>
  } else {
    return <></>
  }
}

const getLeftSide = (props: any) => {
  const { view } = props.props;
  const { Option } = Select;
  let form;

  if (view == View.ApproveModerate) {
    return <div><h3 className="m-0 text-purple">All Experiences</h3></div>
  } else if (view == View.UserReport) {
    return <div><h3 className="m-0 text-purple">User Report</h3></div>
  } else if (view == View.MyFeels) {
    return <div><h3 className="m-0 text-purple">My Experiences</h3></div>
  } else if (view == View.TransactionLocator) {
    return <div>
      <h3 className="m-0 text-purple">Transaction Locator</h3>
      <TransactionLocatorForm props={props} onSubmit={(query) => {
        props.handleSubmitTransactionLocatorForm(query)
      }} />
    </div>
  } else if (view == View.TicketAlert) {
    return <div>
      <div>Please select event</div>
      <Select
        showSearch
        style={{ width: 500 }}
        size={'large'}
        placeholder={'Please select an event'}
        onChange={props.handleTicketAlertEventChange}>
        {props.props.eventOptions.map((item, index) => {
          return <Option value={`${item.label}X${item.altId}`} key={`${item.label}X${item.altId}`}>
            {item.label}
          </Option>
        })}
      </Select></div>
  }
}

function NavBar(props: any) {
  const { setSearchTerm, view } = props.props.props;
  return (
    <nav className={view != View.TransactionLocator ? "navbar px-0 pt-0 bg-white mb-2 shadow-none" : ''}>
      {getLeftSide(props.props)}
      <div>
        {getSelection(props.props)}
      </div>
    </nav>
  );
}

export default NavBar;
