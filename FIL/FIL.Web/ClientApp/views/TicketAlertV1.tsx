/* Third Party Imports */
import * as React from "react";
import { RouteComponentProps } from "react-router-dom";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import { message } from 'antd';

/* Local Imports */
import { IApplicationState } from "../stores";
import { ISessionProps, actionCreators as sessionActionCreators } from "shared/stores/Session";
import TicketAlertComponent from "../components/TicketAlert/TicketAlertComponent";
import * as TicketAlertStore from "../stores/TicketAlert";
import * as EventLearnPageStore from "../stores/EventLearnPage";
import KzLoader from "../components/Loader/KzLoader";
import EventLearnPageDataViewModel from "../models/EventLearnPageDataViewModel";
import * as CheckoutStore from "../stores/Checkout";
import { TicketAlertRequestViewModel } from "../models/TicketAlert/TicketAlertRequestViewModel";
import { TicketAlertUserMappingResponseViewModel } from "../models/TicketAlert/TicketAlertUserMappingResponseViewModel";

type TicketAlertComponentProps = TicketAlertStore.ITicketAlertProps
    & CheckoutStore.ICheckoutLoginProps
    & typeof TicketAlertStore.actionCreators &
    EventLearnPageStore.IEventLearnPageComponentProps
    & typeof EventLearnPageStore.actionCreators
    & typeof CheckoutStore.actionCreators
    & ISessionProps
    & typeof sessionActionCreators
    & RouteComponentProps<{}>;

class TicketAlertV1 extends React.Component<TicketAlertComponentProps, any> {
    getDefaultObject = () => {
        let ticketAlertResponse: TicketAlertUserMappingResponseViewModel = {
            success: false,
            isAlreadySignUp: false
        }
        return ticketAlertResponse;
    }
    state = {
        isSiginSignUpShow: false,
        isLoginPage: false,
        showSignInModal: true,
        ticketAlertResponse: this.getDefaultObject(),
        url: ""
    }

    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }
        this.props.requestCountryData();
        if (this.props.location.pathname.split('/').length > 2) {
            if (this.props.location.pathname.split('/')[2] != undefined) {
                var user: EventLearnPageDataViewModel = {
                    slug: this.props.location.pathname.split('/')[2],
                };
                this.props.requestEventLearnPageData(user);
            }
        }
    }

    render() {
        if (this.props.eventLearnPage.fetchSuccess && this.props.checkout.fetchCountriesSuccess && !this.props.ticketAlert.requesting) {
            let eventData = this.props.eventLearnPage.eventDetails;
            return (
                <>
                    <TicketAlertComponent
                        eventData={eventData}
                        countryList={this.props.checkout.countryList.countries}
                        ticketAlertResponse={this.state.ticketAlertResponse}
                        onSubmit={(form: TicketAlertRequestViewModel) => {
                            this.props.ticketAlertSignUp(form, (response: TicketAlertUserMappingResponseViewModel) => {
                                this.setState({ ticketAlertResponse: response });
                            });
                        }}
                    />

                </>
            );
        } else {
            return <KzLoader />
        }
    }
}

export default connect(
    (state: IApplicationState) => ({
        session: state.session,
        eventLearnPage: state.eventLearnPage,
        checkout: state.checkout,
        ticketAlert: state.ticketAlert
    }),
    (dispatch) => bindActionCreators({
        ...sessionActionCreators,
        ...EventLearnPageStore.actionCreators,
        ...CheckoutStore.actionCreators,
        ...TicketAlertStore.actionCreators
    }, dispatch)
)(TicketAlertV1);