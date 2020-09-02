import { autobind } from "core-decorators";
import Countdown from "react-countdown-now";
import * as React from "react";
import { connect } from "react-redux";
import * as Yup from "yup";
import { RouteComponentProps } from "react-router-dom";
import { PaymentFormDataViewModel } from "../models/Payment/PaymentFormDataViewModel";
import { PaymentFormResponseViewModel } from "../models/Payment/PaymentFormResponseViewModel";
import { PromocodeResponseModel } from "../models/Payment/PromocodeResponseModel";
import { PromoCodeFormViewModel } from "../models/Payment/PromoCodeFormViewModel";
import KzLoader from "../components/Loader/KzLoader";
import { IApplicationState } from "../stores";
import * as numeral from "numeral";
import * as PaymentStore from "../stores/Payment";
import "../scss/_custom.scss";
import "../scss/_payment.scss";
import * as TransactionStore from "../stores/Transaction";
import PlaceAutocomplete from "./PlaceAutocomplete";
import { bindActionCreators } from "redux";
import Metatag from "../components/Metatags/Metatag";
import paymentHelpers from "shared/utils/payments/helpers";
import StripeCardDetailForm from "../components/Payment/StripeCardDetailForm";
import stripeHelpers from "shared/utils/payments/stripe";
import { StripeAccount } from "shared/models/enum/StripeAccount";
import { Channel } from "shared/models/enum/Channel";
import {
    StripeProvider,
    Elements,
} from "react-stripe-elements";
import * as getSymbolFromCurrency from 'currency-symbol-map'

type PaymentComponentProps =
    PaymentStore.IPaymentProps
    & TransactionStore.ITransactionComponentProps
    & typeof TransactionStore.actionCreators
    & typeof PaymentStore.actionCreators
    & RouteComponentProps<{ transactionAltId: string; }>;

var isPromoApplied = false;

export class Payment extends React.Component<PaymentComponentProps, any> {

    public constructor(props) {
        super(props);
        this.state = {
            transactionDetail: null,
            timer: new Date().getTime() + 600000,
            paymentOption: "1",
            cardTypeId: 0,
            cardNumber: "",
            isPromoApplied: false,
            isPromoAppliedSuccess: false,
            promocodeText: "",
            promocodeValidationMessage: "",
            show: true,
            transactionData: null
        };
    }

    public renderer = ({ minutes, seconds, completed }) => {
        if (completed) {
            return this.props.history.push({ pathname: "/pgerror", search: "SessionExpired" });
        } else {
            return <span>{minutes}:{seconds}</span>;
        }
    }

    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0);
        }
        if (this.props.location.state && this.props.location.state.isPaymentByPass == true) {
            this.props.history.push(`/order-confirmation/${this.props.match.params.transactionAltId}`)
        } else {
            this.props.requestPaymentOptionData();
            this.props.requestCountryData();
            this.props.requestTransactionData(this.props.match.params.transactionAltId, (response: any) => { });
            this.setState({ timer: new Date().getTime() + 600000 });
        }
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        if (nextProps.transaction.transactionData !== prevState.transactionData) {
            return { transactionData: nextProps.transaction.transactionData };
        }
        else return null;
    }

    componentDidUpdate(prevProps, prevState) {
        if (prevState.transactionData !== this.state.transactionData) {
            this.setState({ transactionData: this.state.transactionData });
            if (this.state.transactionData.discountAmount > 0) {
                this.setState({ promocodeValidationMessage: "Promocode applied successfully", isPromoAppliedSuccess: true });
            }
        }
    }

    public setPaymentOption(option, e) {
        this.setState({ paymentOption: option });
    }

    public setCardTypeId(type, cardNumber) {
        this.setState({ cardTypeId: type, cardNumber });
    }

    public checkCardType(e) {
        const cardNumber = e.target.value;
        this.setCardTypeId(paymentHelpers.resolveCardType(cardNumber), cardNumber);
    }

    @autobind
    public autoCompleteAddress(address, city, state, country, zipcode) {
        this.setState({
            billingAddress: address, billingCity: city,
            billingState: state, billingCountry: country,
            billingZipcode: zipcode
        });
    }

    @autobind
    public onChangeZipCode(zipcode) {
        this.setState({
            billingZipcode: zipcode
        });
    }

    @autobind
    public onChangeAddress(address) {
        this.setState({
            billingAddress: address
        });
    }

    @autobind
    public onChangeCity(city) {
        this.setState({
            billingCity: city
        });
    }

    @autobind
    public onChangeState(state) {
        this.setState({
            billingState: state
        });
    }

    @autobind
    public onChangeCountry(country) {
        this.setState({
            billingCountry: country
        });
    }

    public addPromocode(e) {
        this.setState({ promocodeText: e.target.value, isPromoApplied: false, isPromoAppliedSuccess: false });
    }
    public validatePromocode() {
        if (this.state.transactionData.discountAmount == 0) {
            if (this.state.promocodeText == "") {
                this.setState({ promocodeValidationMessage: "Enter promocode", isPromoApplied: true, isPromoAppliedSuccess: false })
            }
            else {
                if (this.props.transaction.transactionData.id != null) {
                    var promocode: PromoCodeFormViewModel = {
                        transactionId: this.props.transaction.transactionData.id,
                        promocode: this.state.promocodeText
                    }
                    this.props.savePromocode(promocode, (response: PromocodeResponseModel) => {
                        if (response.success) {
                            if (response.isPaymentBypass) {
                                this.props.history.push(`/order-confirmation/${this.props.match.params.transactionAltId}`)
                            }
                            isPromoApplied = true;
                            this.props.requestTransactionData(this.props.match.params.transactionAltId, (item) => { });
                            this.setState({ promocodeValidationMessage: "Promocode applied successfully", isPromoApplied: true, isPromoAppliedSuccess: true });
                        }
                        else if (!response.success && response.isLimitExceeds) {
                            this.setState({ promocodeValidationMessage: "Maximum tickets limit per promocode exceeds", isPromoApplied: true, isPromoAppliedSuccess: false });
                        } else {
                            this.setState({ promocodeValidationMessage: "You are not eligible for this promocode", isPromoApplied: true, isPromoAppliedSuccess: false });
                        }
                    });
                }
            }
        }
    }

    public render() {
        const schema = this.getSchema();
        if (this.props.Payment.fetchPaymentOptionsSuccess && this.props.transaction.transactionData.altId
            != null) {
            let countries = this.props.Payment.countryList.countries.map((item) => {
                return <option value={item.name}>{item.name}</option>;
            });
            let data = this.props.transaction.transactionData;
            let symbol = getSymbolFromCurrency(data.currency);
            return (<div>
                <Metatag url="payment" title="Payment Options" />
                <div className="container checkout-pages pt-3 pb-5">
                    <div className="card checkout-card">
                        <div>
                            <h5>Payment <small className="float-sm-right text-muted">Time left to complete transaction <span className="text-danger"><Countdown
                                date={Date.now() + (this.state.timer - new Date().getTime())}
                                renderer={this.renderer}
                            /></span></small></h5>
                            <section>
                                <div className="payment-opp p-3">
                                    <div className="container">
                                        <ul className="nav nav-tabs gradient-bg border rounded">
                                            <li className="nav-item border-right" onClick={this.setPaymentOption.bind(this, "1")}><a className="nav-link active" data-toggle="tab" href="#cc-dc">Credit / Debit Card</a></li>
                                        </ul>
                                        <StripeProvider apiKey={stripeHelpers.getStripeKeyOrDefault(data.currency, Channel.feel, ((this.props.location.state && this.props.location.state.StripeAccount) ? this.props.location.state.StripeAccount : StripeAccount.None))}>
                                            <Elements>
                                                <StripeCardDetailForm
                                                    onSubmit={this.onSubmitStripePaymentDetails}
                                                    siteId={1}
                                                    validationSchema={schema}
                                                    countries={this.props.Payment.countryList.countries}
                                                    initialValues={{}}
                                                    requesting={this.props.Payment.requesting}>
                                                    <PlaceAutocomplete
                                                        countries={countries}
                                                        autoCompletePlace={this.autoCompleteAddress}
                                                        onChangeZipCode={this.onChangeZipCode}
                                                        onChangeAddress={this.onChangeAddress}
                                                        onChangeCity={this.onChangeCity}
                                                        onChangeState={this.onChangeState}
                                                        onChangeCountry={this.onChangeCountry}
                                                    />
                                                </StripeCardDetailForm>
                                            </Elements>
                                        </StripeProvider>
                                    </div>
                                </div>
                            </section>
                        </div>
                        <div className="card-body">
                            <div className="row">
                                <div className="col-sm-4">
                                    <div className="input-group">
                                        <input type="text" className="form-control" placeholder="Promo Code (Optional)" value={this.state.promocodeText} onChange={this.addPromocode.bind(this)} />
                                        <span className="input-group-btn">
                                            <button className="btn btn-default" type="button" onClick={this.validatePromocode.bind(this)}>Apply</button></span></div>
                                    {this.state.isPromoApplied && !this.state.isPromoAppliedSuccess && <div className="text-danger mt-2"><small>{this.state.promocodeValidationMessage}</small></div>}
                                    {this.state.isPromoApplied && this.state.isPromoAppliedSuccess && <div className="text-success mt-2"><small>{this.state.promocodeValidationMessage}</small></div>}
                                </div>
                                <div className="col-sm-8 text-right">
                                    <p>
                                        <small>Bag Subtotal {symbol + numeral(data.grossTicketAmount - ((data.donationAmount && data.donationAmount != null && data.donationAmount > 0) ? data.donationAmount : 0)).format('0.00') + ' ' + data.currency}</small><br />
                                        {data.discountAmount > 0 && <> <small>Discount <span className="ml-20">{symbol + " " + numeral(data.discountAmount).format('0.00') + ' ' + data.currency}</span></small> <br /> </>}
                                        {data.donationAmount > 0 && <><small>Donation {symbol + numeral(data.donationAmount).format('0.00') + ' ' + data.currency}</small><br /></>}
                                        <small><i className="fa fa-info-circle text-primary mr-2" data-toggle="tooltip" data-placement="top" title="" data-original-title="Bank, service and handling charges">
                                        </i> Bank, service and handling charges {symbol + numeral(data.convenienceCharges + data.serviceCharge).format('0.00') + ' ' + data.currency} </small></p>
                                    <div className="total-price">
                                        <span className="border-top pl-3 pt-2 mt-2">Total	{symbol + numeral(data.netTicketAmount).format('0.00') + ' ' + data.currency}</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="card-footer gradient-bg">
                            <small>By clicking continue, you accept and agree to all the terms of feelitLIVE's policy. We will send you a confirmation email that contains these terms and an itemized list of your purchase (including taxes and shipping charges, if applicable). For more information about our terms and conditions, <a href={`https://${window.location.href.split('/')[2]}/privacy-policy`} target="_blank">click here</a> . To view our privacy policy.</small>
                        </div>
                    </div>
                </div >
            </div >
            );
        } else {
            return <div><KzLoader /></div>;
        }
    }

    @autobind
    public handleResponseAction(response, values) {
        if (response.success && response.action != "") {
            this.props.history.replace({ pathname: "/paymentform", state: { paymentInfo: response } });
        }
        else if (response.success && response.action == "" && response.errorMessage == "") {
            this.props.history.push({ pathname: `/order-confirmation/${response.transactionAltId}` });
        }
        else if (!response.success && response.action == "RequireSourceAction") {
            var data = this.state.transactionDetail;
            (window as any).Stripe(stripeHelpers.getStripeKeyOrDefault(this.props.transaction.transactionData.currency, Channel.feel, ((this.props.location.state && this.props.location.state.StripeAccount) ? this.props.location.state.StripeAccount : StripeAccount.None)))
                .handleCardAction(response.method).then((handleCardActionResponse) => {
                    if (handleCardActionResponse.error) {
                        alert(handleCardActionResponse.error.message);
                        this.props.requestPaymentOptionData();
                    }
                    else {
                        var paymentIntent = handleCardActionResponse.paymentIntent;
                        values.token = paymentIntent.id + "~" + paymentIntent.payment_method;
                        values.nameOnCard = "intent";
                        paymentHelpers.processPayment(values,
                            this.props.proceedToPayment,
                            (response: PaymentFormResponseViewModel) => {
                                this.handleResponseAction(response, values);
                            },
                            (error) => {
                                // TODO: Modal
                                alert(error.message);
                            }
                        );
                    }
                });
        }
        else {
            this.props.history.push({ pathname: "/pgerror", search: response.errorMessage });
        }
    }

    @autobind
    private onSubmitStripePaymentDetails(values: PaymentFormDataViewModel, response: any) {
        if (response.error) {
            alert(response.error.message);
            this.props.requestPaymentOptionData();
            return;
        }
        values.transactionId = this.props.transaction.transactionData.id;
        values.cardTypeId = this.state.cardTypeId;
        values.cardNumber = this.state.cardNumber;
        values.paymentOption = this.state.paymentOption;

        //values.paymentGateway = paymentHelpers.resolveGateway(values.paymentOption, this.props.transaction.transactionData.currency);
        values.paymentGateway = 1;
        if (response.paymentMethod.card.brand == "Visa") {
            values.cardTypeId = 2;
        }
        else if (response.paymentMethod.card.brand == "AmericanExpress") {
            values.cardTypeId = 3;
        }
        else if (response.paymentMethod.card.brand == "Maestro") {
            values.cardTypeId = 5;
        }
        values.token = response.paymentMethod.id;
        values.expiryMonth = response.paymentMethod.card.exp_month;
        values.expiryYear = response.paymentMethod.card.exp_year;
        values.cardNumber = "4444XXXXXXXX" + response.paymentMethod.card.last4;
        values.address = this.state.billingAddress;
        values.city = this.state.billingCity;
        values.state = this.state.billingState;
        values.country = this.state.billingCountry;
        values.zipcode = this.state.billingZipcode;
        paymentHelpers.processPayment(values,
            this.props.proceedToPayment,
            (response: PaymentFormResponseViewModel) => {
                this.handleResponseAction(response, values)
            },
            (error) => {
                // TODO: Modal
                alert(error.message);
            }
        );
    }

    /* @autobind
     private onSubmitPaymentDetails(values: PaymentFormDataViewModel) {
         const data = this.state.transactionDetail;
                var that = this;
         if (data != null) {
 
             if (this.state.billingZipcode != undefined) {
                    values.address = this.state.billingAddress;
                values.city = this.state.billingCity;
                values.country = this.state.billingCountry;
                values.state = this.state.billingState;
                values.zipcode = this.state.billingZipcode;
             } else {
                    alert("Please complete your payment information in order to proceed.");
            }
            const transactionId = data.transactionId;
            values.transactionId = +transactionId;
            values.cardTypeId = this.state.cardTypeId;
            values.cardNumber = this.state.cardNumber;
            values.paymentOption = this.state.paymentOption;
             switch (+values.paymentOption) {
                 case 1:
                    values.paymentGateway = 1;
                    break;
                case 4:
                    values.paymentGateway = 4;
                    break;
                case 16:
                    values.paymentGateway = 4;
                    break;
            }
             if (values.paymentGateway == 1) {
                 var token = (window as any).stripePublicToken;
                 if (token == undefined || token == null) {
                    token = "pk_live_TeewitnA3sFmUmckXGkc7eaS";
            }
            console.log("" + token);
            (window as any).Stripe.setPublishableKey(token);
                 var card: StripePaymentModel = {
                    number: that.state.cardNumber.toString(),
                cvc: values.cvv.toString(),
                exp_month: values.expiryMonth.toString(),
                exp_year: values.expiryYear.toString(),
            };
                 (window as any).Stripe.createToken(card, function (status, response) {
                     if (response.error) {
                    alert(response.error.message);
                that.props.requestPaymentOptionData();
            }
                     else {
                    values.token = response.id;
                paymentHelpers.processPayment(values,
                    that.props.proceedToPayment,
                             (response: PaymentFormResponseViewModel) => {
                                 if (response.success && response.action != "") {
                    that.props.history.replace({ pathname: "/paymentform", state: { paymentInfo: response } });
            }
                                 else if (response.success && response.action == "" && response.errorMessage == "") {
                    localStorage.setItem("confirmationFromMyOrders", "false");
                                     that.props.history.push({pathname: `/order-confirmation/${response.transactionAltId}` });
            }
                                 else {
                    that.props.history.push({ pathname: "/pgerror", search: response.errorMessage });
            }
        },
                             (error) => {
                    // TODO: Modal
                    alert(error.message);
            }
        );
    }
});
             } else {
                    that.props.proceedToPayment(values, (response: PaymentFormResponseViewModel) => {
                        if (response.success && response.action != "") {
                            that.props.history.replace({ pathname: "/paymentform", state: { paymentInfo: response } });
                        }
                        else if (response.success && response.action == "" && response.errorMessage == "") {
                            localStorage.setItem("confirmationFromMyOrders", "false");
                            that.props.history.push({ pathname: `/order-confirmation/${response.transactionAltId}` });
                        }
                        else {
                            that.props.history.push({ pathname: "/pgerror", search: response.errorMessage });
                        }
                    });
            }
         } else {
                    that.props.history.replace("/");
            }
        } */

    private getSchema() {
        return Yup.object().shape({
        });
    }
}

export default connect(
    (state: IApplicationState) => ({
        Payment: state.Payment,
        transaction: state.Transaction
    }),
    (dispatch) => bindActionCreators({
        ...PaymentStore.actionCreators,
        ...TransactionStore.actionCreators
    }, dispatch)
)(Payment);