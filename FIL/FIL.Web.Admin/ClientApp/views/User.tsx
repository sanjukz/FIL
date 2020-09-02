import * as React from "react";
import { connect } from "react-redux";
import * as SessionState from "shared/stores/Session";
import { IApplicationState } from "../stores";

type UserProps = SessionState.ISessionProps & typeof SessionState.actionCreators;

class User extends React.Component<UserProps, any> {
    public render() {
        return <div>
            <h1>User</h1>
            <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
            <p>User is authenticated: {this.props.session.isAuthenticated ? "Yes! :-)" : "No :-("}</p>
            <p>User first name: {this.props.session.user && this.props.session.user.firstName}</p>
        </div>;
    }
}

export default connect(
    (state: IApplicationState) => ({ session: state.session }),
    SessionState.actionCreators
)(User);


