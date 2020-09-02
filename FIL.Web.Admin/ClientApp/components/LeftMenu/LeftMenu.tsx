import * as React from 'react';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { IApplicationState } from '../../stores';
import * as SessionState from 'shared/stores/Session';
import LeftMenuItem from './LeftMenuItem';

type SideMenuProps = SessionState.ISessionProps & typeof SessionState.actionCreators;

function LeftMenu(props: SideMenuProps) {
  const menueItems = [
    { id: 1, name: 'Dashboard', icon: 'dashboard', path: '/' },
    { id: 2, name: 'Create New Experience', icon: 'create-event', path: '/host-online/basics' },
    { id: 3, name: 'Reports', icon: 'report', path: '/transactionreport' },
    { id: 4, name: 'Toolkit', icon: 'toolkit', path: 'javascript:Void(0)' }
  ];

  return (
    <div className="card border-0 bg-light left-main-nav d-none d-md-block" id="fil-collapse-left-menu">
      <div className="card-body p-0">
        <div className="list-group">
          {menueItems.map((item) => (
            <LeftMenuItem key={item.id} menuItem={item} />
          ))}
        </div>
      </div>
    </div>
  );
}

export default connect(
  (state: IApplicationState) => ({
    session: state.session
  }),
  (dispatch) =>
    bindActionCreators(
      {
        ...SessionState.actionCreators
      },
      dispatch
    )
)(LeftMenu);
