import * as React from 'react';
import { connect } from "react-redux";
import { IApplicationState } from "../../stores/index";
import * as FeelPlaces from "../../stores/FeelPlaces";
import { actionCreators as sessionActionCreators } from "shared/stores/Session";
import { bindActionCreators } from "redux";
import MonumentTicketsV1 from "../Home/MonumentTicketsV1";
import MonumentTicketSkeleton from '../Home/MonumentTicketSkeleton';

class LiveOnlineSection extends React.PureComponent<any, any> {
    constructor(props) {
        super(props);
        this.state = {}
    }

    componentDidMount() {
        this.props.getCategoryPlaces("all-top-120", (response) => {
            this.setState({ categoryPlaces: response })
        });
    }

    public goToLogin() {
        localStorage.setItem("isRedirectHomePage", "true");
        this.props.history.push("/login");
    }

    render() {
        let categoryPlaces = [];
        if (this.state.categoryPlaces && this.state.categoryPlaces.categoryEvents) {
            categoryPlaces = this.state.categoryPlaces.categoryEvents.filter((val) => {
                return val.event.masterEventTypeId == 4
            })
        }
        return (categoryPlaces.length > 0) ?
            <section className="fil-tiles-sec">
                <div className="container">
                    <h2 data-aos="zoom-in" title="Live-streaming experiences and events from around the world" className="text-center mb-5">Latest live-stream events</h2>
                    <MonumentTicketsV1
                        categoryData={categoryPlaces}
                        count={4}
                        session={this.props.session}
                        isLiveStreamSection={true}
                    />
                    <a href="c/coaching?category=123" className="show-all-link">Show all online live-stream experiences ❯</a>
                </div>
            </section> :
            <div className="all-feels sec-spacing">
                <div className="nav-tab-content container">
                    <div className="row">
                        <div className="col-sm-12 p-0">
                            <h2 data-aos="zoom-in" className="m-0 text-center" title="Live-streaming experiences and events from around the world">Latest live-stream events</h2>
                        </div>
                        <MonumentTicketSkeleton number={4} />
                    </div>
                </div>
            </div>
    }
}

const mapState = (state: IApplicationState, ownProps) => {
    return {
        categoryPlaces: state.categoryPlaces,
        session: state.session,
        ...ownProps
    };
};

const mapDispatch = (dispatch) => bindActionCreators({ ...FeelPlaces.actionCreators, ...sessionActionCreators }, dispatch);

export default connect(mapState, mapDispatch)(LiveOnlineSection)
