import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import * as TicketLookupStore from "../stores/TicketLookup";
import "../scss/FILSuite/_transaction-report.scss";
import FILLoader from "../components/Loader/FILLoader";
import TicketLookupForm from "../components/TicketLookup/TicketLookupForm";
import TicketLookupEmailDetailComponent from "../components/TicketLookup/TicketLookupEmailDetailComponent";
import TicketLookupComponent from "../components/TicketLookup/TicketLookupComponent";
import { TicketLookupDataViewModel } from "../models/TicketLookup/TicketLookupDataViewModel";

type TicketLookupComponentProps = TicketLookupStore.ITicketLookupState & typeof TicketLookupStore.actionCreators

var ticketLookupData;
class TicketLookup extends React.Component<TicketLookupComponentProps, any> {
    public render() {
        if (this.props.isLoading == false) {
            return <div>
                <div className="trans-report">
                    <section className="filters">
                        <div className="container bg-light p-20 bdr-radius">
                            <h3 className="text-center mt-0 mb-20">Ticket Lookup</h3>
                            <div className="row">
                                <TicketLookupForm onSubmit={this.onSubmitTicketLookup} />
                            </div>
                        </div>
                    </section>
                </div>
                <div className="form-group">
                    {(this.props.fetchTicketLookupSuccess == true) && (this.props.ticketLookup.transaction != null) &&
                        <TicketLookupComponent ticketLookup={this.props.ticketLookup} />}
                    {(this.props.fetchTicketLookupEmailDetailSuccess == true) && (this.props.ticketLookupEmailDetails.ticketLookupEmailDetailContainer != undefined) &&
                        <TicketLookupEmailDetailComponent ticketLookupEmailDetails={this.props.ticketLookupEmailDetails} />}
                    {(this.props.fetchTicketLookupEmailDetailSuccess == true) && (this.props.ticketLookupEmailDetails.ticketLookupEmailDetailContainer === undefined)
                        && (<div className="alert alert-danger" style={{ padding: "6px", maxWidth: "350px", margin: "0px auto 12px" }}> Records not found</div>)}
                    {(this.props.fetchTicketLookupSuccess == true) && (this.props.ticketLookup.transaction == null) &&
                        (<div className="alert alert-danger" style={{ padding: "6px", maxWidth: "350px", margin: "0px auto 12px" }}>Invalid ConfirmationId</div>)}
                </div>
            </div>
        }
        else {
            return <div>
                <FILLoader />
                </div>
        }
    }

    @autobind
    private onSubmitTicketLookup(values: TicketLookupDataViewModel) {
        if (values.transactionId != null || values.transactionId != undefined) {
            this.props.requestTicketLookupTransactionData(values.transactionId);
            values.transactionId = null;
            values.emailId = null;
            values.name = null;
            values.phone = null;
        }
        else if (values.emailId != null || values.emailId != undefined) {
            this.props.requestTicketLookupData(values.emailId);
            values.transactionId = null;
            values.emailId = null;
            values.name = null;
            values.phone = null;

        } else if (values.name != null || values.name != undefined) {
            this.props.requestTicketLookupDataByName(values.name);
            values.transactionId = null;
            values.emailId = null;
            values.name = null;
            values.phone = null;

        } else {
            this.props.requestTicketLookupDataByPhone(values.phone);
            values.transactionId = null;
            values.emailId = null;
            values.name = null;
            values.phone = null;
        }
    }
}

export default connect(
    (state: IApplicationState) => state.ticketLookup,
    TicketLookupStore.actionCreators
)(TicketLookup);
