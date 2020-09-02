import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../../stores/index";
import * as FeelPlaces from "../../stores/FeelPlaces";
import { Link } from "react-router-dom";
import { actionCreators as sessionActionCreators } from "shared/stores/Session";
import { bindActionCreators } from "redux";
import MonumentTicketsV1 from "./MonumentTicketsV1";
import MonumentTicketSkeleton from "./MonumentTicketSkeleton";

class LiveOnlineSection extends React.PureComponent<any, any> {
  constructor(props) {
    super(props);
    this.state = {};
  }

  componentDidMount() {
    this.props.getCategoryPlaces("all-top-120", (response) => {
      this.setState({ categoryPlaces: response });
    });
  }

  public goToLogin() {
    localStorage.setItem("isRedirectHomePage", "true");
    this.props.history.push("/login");
  }

  render() {
    let categoryPlaces = [];
    if (this.state.categoryPlaces && this.state.categoryPlaces.categoryEvents) {
      categoryPlaces = this.state.categoryPlaces.categoryEvents.filter(
        (val) => {
          return val.event.masterEventTypeId == 4;
        }
      );
    }
    return categoryPlaces.length > 0 ? (
      <section
        className={`fil-tiles-sec ${
          this.props.isExperiencePage ? "exp-sec-pad " : ""
        }`}
      >
        <div className="container">
          <h3
            title="Live-streaming experiences and events from around the world"
            className="text-purple"
          >
            {this.props.isExperiencePage
              ? "Other live-stream experiences"
              : "Online live-stream experiences from around the world"}
          </h3>
          <MonumentTicketsV1
            categoryData={categoryPlaces}
            count={8}
            session={this.props.session}
            isLiveStreamSection={true}
          />
          <a href="c/online-experiences?eventType=4" className="show-all-link">
            Show all online live-stream experiences ❯
          </a>
        </div>
      </section>
    ) : (
      <div className="all-feels sec-spacing">
        <div className="nav-tab-content container">
          <div className="row">
            <div className="col-sm-12 p-0">
              <h3
                className="m-0"
                title="Live-streaming experiences and events from around the world"
              >
                {this.props.isExperiencePage
                  ? "Other live-stream experiences"
                  : "Online live-stream experiences from around the world"}
              </h3>
            </div>
            <MonumentTicketSkeleton number={4} />
          </div>
        </div>
      </div>
    );
  }
}

const mapState = (state: IApplicationState, ownProps) => {
  return {
    categoryPlaces: state.categoryPlaces,
    session: state.session,
    ...ownProps,
  };
};

const mapDispatch = (dispatch) =>
  bindActionCreators(
    { ...FeelPlaces.actionCreators, ...sessionActionCreators },
    dispatch
  );

export default connect(
  mapState,
  mapDispatch
)(LiveOnlineSection);
