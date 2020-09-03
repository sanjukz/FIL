import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import * as ActivateUserStore from "../stores/ActivateUser";
import FilLoader from "../components/Loader/FilLoader";

export class Activate extends React.Component<any, any> {

    public componentDidMount() {
        this.props.requestActivateUserData(this.props.match.params.altId);
    }
    redirect = (e) => {
        if (sessionStorage.getItem("checkoutRedirect") && localStorage.getItem("cartItems") != null &&
            localStorage.getItem("cartItems") != "0") {
            this.props.history.push("/itinerary");
        } else {
            this.props.history.push("/");
        }
    }

    public render() {
        if (this.props.fetchSuccess) {
            var Data = this.props.result;
            if (Data.success) {
                return <div className="text-center text-success container py-5 my-5" >
                    <h3> Woohoo! Your account has now been created. Have a great experience as you feelitLIVE and tell us how you feel. You can also refer your friends to feelitLIVE by sending out invites from your “My account” section. </h3>
                    <a href="javascript:void(0)" onClick={(e) => this.redirect(e)} className="btn bnt-lg site-primery-btn mt-4">Continue</a>
                </div>;
            } else {
                return <div className="text-center text-danger" >
                    <small> Please contact customer support.</small> </div>;
            }
        } else {
            return <div>
                <FilLoader />
            </div>;
        }
    }
}
export default connect(
    (state: IApplicationState) => state.ActivateUser,
    ActivateUserStore.actionCreators
)(Activate);

