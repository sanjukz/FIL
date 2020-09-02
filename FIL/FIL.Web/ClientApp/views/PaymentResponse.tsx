import * as React from "react";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import { PaymentResponseFormDataViewModel } from "../models/Payment/PaymentResponseFormDataViewModel";
import { PaymentResponseViewModel } from "../models/Payment/PaymentResponseViewModel";
import { IApplicationState } from "../stores";
import * as PaymentResponseStore from "../stores/PaymentResponse";
import "../scss/_custom.scss";
import "../scss/_payment.scss";

type PaymentResponseComponentProps =
	PaymentResponseStore.IPaymentResponseState
	& RouteComponentProps<{}>
	& typeof PaymentResponseStore.actionCreators;

export class PaymentResponse extends React.Component<PaymentResponseComponentProps, {}> {

	public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }

		if (localStorage.getItem('transactionId') != null && localStorage.getItem('transactionId') != '0') {
			var transactionId = localStorage.getItem("transactionId");
			var query = this.props.location.search.split('?')[1];
			var responseObject: PaymentResponseFormDataViewModel = {
				queryString: query,
				transactionId: +transactionId,
				paymentOption: "1",
			}
			this.props.sendPaymentResponse(responseObject, (response: PaymentResponseViewModel) => {
				if (response.success) {
					alert("success");					
				} else {
					alert(response.errorMessage);
				}
			});
		}
	}

	public render() {
		return <div></div>;
	}
}
export default connect(
	(state: IApplicationState) => state.PaymentResponse,
	PaymentResponseStore.actionCreators
)(PaymentResponse);
