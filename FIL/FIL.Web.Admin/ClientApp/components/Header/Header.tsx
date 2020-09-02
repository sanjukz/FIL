import * as React from 'react';
import { connect } from 'react-redux';
import * as SessionState from 'shared/stores/Session';
import { IApplicationState } from '../../stores';
import AuthItems from './AuthItems';

type IHeaderProps = SessionState.ISessionProps &
  typeof SessionState.actionCreators;

function Header(props: IHeaderProps) {
  return (
    <nav className="navbar navbar-light bg-white px-0 shadow-sm py-2">
      <div className="container-fluid">
        <a className="navbar-brand">
          <img
            src="https://static5.feelitlive.com/images/logos/fap-live-stream.png"
            alt="FeelitLIVE Logo"
            width="130"
          />
        </a>
        {props.session.isAuthenticated ? (
          <AuthItems logout={props.logout} session={props.session} />
        ) : (
            null
          )}
      </div>
    </nav>
  );
}

export default connect(
  (state: IApplicationState) => ({
    session: state.session,
  }),
  { ...SessionState.actionCreators }
)(Header);
