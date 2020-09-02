import * as React from "react";
import { connect } from "react-redux";
import * as EventLearnPageStore from "../stores/EventLearnPage";
import { IApplicationState } from "../stores";
import { bindActionCreators } from "redux";
import { actionCreators as sessionActionCreators, ISessionProps } from "shared/stores/Session";
import FeelLoader from "../components/Loader/FeelLoader";
import {  RouteComponentProps } from "react-router-dom";

type EventLearnPageComponentProps =
    EventLearnPageStore.IEventLearnPageComponentProps
    & typeof EventLearnPageStore.actionCreators
    & ISessionProps
    & typeof sessionActionCreators
    & RouteComponentProps<{ ratingAltId: string; }>;

class ActiveReview extends React.Component<EventLearnPageComponentProps, any> {
    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }

        this.props.activeUserRatingAndReviews(this.props.match.params.ratingAltId);
    }

    public render() {
        if (this.props.eventLearnPage.registered) {
            return <div className="text-center text-success py-5 h-50">
                <h4> Review and Rate has been activated. </h4>
            </div>;
        } else {
            return <div>
                <FeelLoader />
            </div>;
        }
    }
}

export default connect(
    (state: IApplicationState) => ({ session: state.session, eventLearnPage: state.eventLearnPage }),
    (dispatch) => bindActionCreators({ ...sessionActionCreators, ...EventLearnPageStore.actionCreators }, dispatch)
)(ActiveReview);
