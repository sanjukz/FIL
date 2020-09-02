import { autobind } from "core-decorators";
import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import { IApplicationState } from "../stores";
import Transaction from "../models/Comman/TransactionViewModel";
import DeliveryPickupOptions from "../components/DeliveryOptions/DeliveryPickupOptions";
import KzLoader from "../components/Loader/KzLoader";
import { UserDeliveryDetailFormDataViewModel } from "../models/DeliveryOptions/UserDeliveryDetailFormDataViewModel";
import { DeliveryOptionResponse } from "../utils/CheckoutProvider";
import { UpdateTransactionResponseViewModel } from "../models/UpdateTransactionResponseViewModel";
import * as Checkoutstore from "../stores/Checkout";
import * as DeliveryOptionsStore from "../stores/DeliveryOptions";
import * as TransactionStore from "../stores/Transaction";
import "../scss/_custom.scss";
import "../scss/_payment.scss";
import { checkIsLiveOnlineEvents } from "../../ClientApp/utils/TicketCategory/ItineraryProvider";
import { bindActionCreators } from "redux";
import { StripeAccount } from "shared/models/enum/StripeAccount";
import Metatag from "../components/Metatags/Metatag";

type DeliveryOptionsComponentProps = Checkoutstore.ICheckoutLoginProps
    & typeof Checkoutstore.actionCreators
    & TransactionStore.ITransactionComponentProps
    & typeof TransactionStore.actionCreators
    & DeliveryOptionsStore.IDeliveryOptionsProps
    & typeof DeliveryOptionsStore.actionCreators
    & RouteComponentProps<{ transactionAltId: string; }>;

export class DeliveryOptions extends React.Component<DeliveryOptionsComponentProps, any> {

    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }
        let isLiveOnlineEvents = checkIsLiveOnlineEvents();
        if (isLiveOnlineEvents) {
            let userData: UserDeliveryDetailFormDataViewModel = {
                email: '',
                firstName: '',
                lastName: '',
                phoneCode: '',
                phoneNumber: ''
            }
            let userDataArray = [];
            userDataArray.push(userData);
            this.props.requestTransactionData(this.props.match.params.transactionAltId, (response: Transaction) => {
                this.onSubmitPickupDetails(userDataArray);
            });
        } else {
            if (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') {
                var data = JSON.parse(localStorage.getItem("cartItems"));
                if (data.length > 0) {
                    var eventDetailId = data[0].eventDetailId;
                    this.props.requestDeliveryOptions(eventDetailId);
                }
            }
            this.props.requestCountryData();
            this.props.requestTransactionData(this.props.match.params.transactionAltId, (response: Transaction) => {
            });
        }
        this.setState({ currentPage: "deliveryOptions" });
    }

    public switchPage(page, e) {
        this.setState({
            currentPage: page,
        });
    }

    public render() {
        if (this.props.checkout.fetchCountriesSuccess && this.props.transaction.fetchSuccess) {
            return (<div> <Metatag url="delivery-options" title="Delivery Options" />
                <div className="container checkout-pages pt-3 pb-5">
                    <div className="card checkout-card">
                        <h5> Delivery &amp; Pickup Options
                                <small className="float-right text-muted"><i className="fa fa-lock text-success"></i> Secure</small>
                        </h5>
                        {(this.state.currentPage == "deliveryOptions" || this.state.currentPage == "pickupDetails" || this.state.currentPage == "payment") &&
                            <DeliveryPickupOptions
                                switchPage={this.switchPage.bind(this)}
                                showButtons={this.state.currentPage == "deliveryOptions"}
                                deliveryOptions={this.props.checkout.deliveryOptions}
                                countryData={this.props.checkout.countryList}
                                submitDetails={this.submitDetails}
                            />}
                        <div className="card-footer gradient-bg">
                            <small>By clicking continue, you accept and agree to all the terms of feelitLIVE's policy. We will send you a confirmation email that contains these terms and an itemized list of your purchase (including taxes and shipping charges, if applicable). For more information about our terms and conditions, <a href={`https://${window.location.href.split('/')[2]}/privacy-policy`} target="_blank">click here</a> . To view our privacy policy.</small>
                        </div>
                    </div>
                </div>
            </div>
            );
        } else {
            return <div><KzLoader /></div>;
        }
    }

    private onSubmitPickupDetails(userDetails) {
        let pickupDetail = DeliveryOptionResponse(userDetails, this.props.transaction.transactionData.id);
        if (pickupDetail) {
            this.props.saveDeliveryOptions(pickupDetail, (response: UpdateTransactionResponseViewModel) => {
                if (response.success) {
                    this.props.history.push({
                        pathname: `/payment/${this.props.match.params.transactionAltId}`, state: {
                            isPaymentByPass: ((this.props.location.state && this.props.location.state.isPaymentByPass == true) ? true : false),
                            StripeAccount: ((this.props.location.state && this.props.location.state.StripeAccount) ? this.props.location.state.StripeAccount : StripeAccount.None)
                        }
                    });
                }
            });
        } else {
            this.props.history.replace("/");
        }
    }

    @autobind
    private submitDetails(userDetails) {
        this.onSubmitPickupDetails(userDetails);
    }
}

export default connect(
    (state: IApplicationState) => ({
        DeliveryOptions: state.DeliveryOptions,
        checkout: state.checkout,
        transaction: state.Transaction
    }),
    (dispatch) => bindActionCreators({
        ...DeliveryOptionsStore.actionCreators,
        ...Checkoutstore.actionCreators,
        ...TransactionStore.actionCreators
    }, dispatch)
)(DeliveryOptions);