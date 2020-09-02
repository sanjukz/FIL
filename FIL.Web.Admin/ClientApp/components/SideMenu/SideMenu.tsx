import * as React from 'react'
import { connect } from 'react-redux'
import { IApplicationState } from '../../stores'
import * as AuthedRoleFeatureState from '../../stores/AuthedRoleFeature'
import { bindActionCreators } from 'redux'
import * as SessionState from 'shared/stores/Session'
import { formatMenuItems } from './utils'
import SideMenuItems from './SideMenuItems'
import { Spin } from 'antd'

const MenuSpinner = (
  <div className="spinner-border text-primary text-center" role="status">
    <span className="sr-only">Loading...</span>
  </div>
)

type SideMenuProps = SessionState.ISessionProps &
  AuthedRoleFeatureState.IAuthedNavMenuFeatureState &
  typeof SessionState.actionCreators &
  typeof AuthedRoleFeatureState.actionCreators

class SideMenu extends React.Component<SideMenuProps, any> {
  state = {
    isEventCreation: false,
    path: '',
    href: ''
  }

  componentDidMount() {
    if (this.props.session.user != undefined) {
      localStorage.setItem('altId', this.props.session.user.altId)
      localStorage.setItem('roleId', this.props.session.user.rolesId.toString())
      this.props.getRoleFeatures(this.props.session.user.altId)
    }
    if (sessionStorage.getItem('isEventCreation') != null) {
      this.setState({
        isEventCreation: true
      })
    }
    if (typeof window !== 'undefined') {
      this.setState({
        path: window.location.pathname,
        href: window.location.href
      })
    }
  }

  public render() {
    let featureMenuData = []
    if (this.props.featureFetchSuccess) {
      featureMenuData = formatMenuItems(this.props.feature.feature, this.state.path)
    }

    return (
      <nav id="sidebar" className="collapse in d-block">
        <div className="sidebar-header m-3 text-purple">
          {!this.state.isEventCreation == true ? (
            <h3 className="m-0 text-purple">Host a feel</h3>
          ) : (
            <h3 className="m-0">
              <a
                href={
                  this.state.href.indexOf('dev') > -1
                    ? 'https://dev.feelitlive.com/create-online-experience'
                    : 'https://www.feelitlive.com/create-online-experience'
                }
                target="_blank"
              >
                {' '}
                <img
                  width="120"
                  src="https://feelaplace-cdn.s3-us-west-2.amazonaws.com/images/logos/fap-live-stream.png"
                  alt="FeelitLIVE"
                  title="FeelitLIVE"
                />
              </a>
            </h3>
          )}
          <strong>
            {!this.state.isEventCreation == true ? (
              'Host a feel'
            ) : (
              <a
                href={
                  this.state.href.indexOf('dev') > -1
                    ? 'https://dev.feelitlive.com/create-online-experience'
                    : 'https://www.feelitlive.com/create-online-experience'
                }
                target="_blank"
              >
                <img
                  width="70"
                  src="https://static5.feelitlive.com/images/icons/live-tag.svg"
                  alt="FeelitLIVE"
                  title="FeelitLIVE"
                />
              </a>
            )}
          </strong>
        </div>
        {featureMenuData.length > 0 ? (
          <SideMenuItems
            path={this.state.path}
            isEventCreation={this.state.isEventCreation}
            featureList={featureMenuData}
          />
        ) : (
          <div className="m-5">
            <Spin indicator={MenuSpinner} />
          </div>
        )}
      </nav>
    )
  }
}

export default connect(
  (state: IApplicationState) => ({
    session: state.session,
    ...state.authedRoleFeature
  }),
  (dispatch) =>
    bindActionCreators(
      {
        ...SessionState.actionCreators,
        ...AuthedRoleFeatureState.actionCreators
      },
      dispatch
    )
)(SideMenu)
