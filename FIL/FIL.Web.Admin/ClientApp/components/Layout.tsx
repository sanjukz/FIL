import * as React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from '../../node_modules/redux';
import { actionCreators as sessionActionCreators, ISessionProps } from 'shared/stores/Session';
import { IApplicationState } from '../stores';
import '../scss/site.scss';
import SideMenu from '../components/SideMenu/SideMenu';
import Header from './Header';
import LeftMenu from './LeftMenu';
import { GoogleTagManager, Intercom } from './analytics';

interface IProps {
  gtmId: string;
  url: string;
}
type LayoutProps = ISessionProps & typeof sessionActionCreators & IProps;

class Layout extends React.Component<any, any> {
  constructor(props) {
    super(props);
    this.state = {
      isHideSideMenue: false
    };
  }
  componentDidMount() {
    if (window.location.pathname.indexOf('host-online') > -1) {
      this.setState({ isHideSideMenue: true });
    }
  }

  public render() {
    return (
      <>
        <div className="fil-admin bg-light">
          <GoogleTagManager gtmId={this.props.gtmId} />
          {this.props.session.isAuthenticated ? <Intercom session={this.props.session} /> : null}
          <Header />
          {!this.state.isHideSideMenue && (
            <section className="container-fluid">
              <h2 className="m-0 clr-link text-blueberry fil-admin-title">Online Host Dashboard</h2>
            </section>
          )}
          <div className="containt-colum container-fluid">
            <div className="card-deck">
              {!this.state.isHideSideMenue &&
                this.props.session.isAuthenticated &&
                (this.props.session.user && this.props.session.user.rolesId == 11 ? <LeftMenu /> : <SideMenu />)}
              {this.props.children}
            </div>
          </div>
        </div>
      </>
    );
  }
}

export default connect(
  (state: IApplicationState, ownProps) => ({
    session: state.session,
    ...ownProps
  }),
  (dispatch) => bindActionCreators({ ...sessionActionCreators }, dispatch)
)(Layout);
