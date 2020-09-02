import { autobind } from "core-decorators";
import * as React from "react";
import * as ReactDOM from "react-dom";
import {  Button } from "react-bootstrap";

export default class PaymentForm extends React.Component<any, any> {
	_form;

	public constructor(props) {
		super(props);
		this.state = { paymentData: "" };
	}

	public attachNode(node) {
		this._form = ReactDOM.findDOMNode(node);
	}

	@autobind
	public handleClick() {
		this._form.submit();
	}


	public componentDidMount() {
		if (window) {
			window.scrollTo(0, 0)
		}
		this._form.submit();
	}

	public submitPaymentForm(e) {

	}

	public render() {
		var paymentDetail = this.props.location.state.paymentInfo;
		var keys = Object.keys(paymentDetail.formFields);
		var formKeys = keys.map(function (val) {
			if (val == "termUrl") {
				return <input name="TermUrl" type="hidden" value={paymentDetail.formFields[val]} />
			}
			else if (val == "paReq") {
				return <input name="PaReq" type="hidden" value={paymentDetail.formFields[val]} />
			}
			else if (val == "md") {
				return <input name="MD" type="hidden" value={paymentDetail.formFields[val]} />
			}
			else if (val == "transId") {
				return <input name="TransId" type="hidden" value={paymentDetail.formFields[val]} />
			}
			else if (val == "transAmt") {
				return <input name="TransAmt" type="hidden" value={paymentDetail.formFields[val]} />
			}
			else {
				return < input name={val} type="hidden" value={paymentDetail.formFields[val]} />
			}
		});
		return (<html>
			<head>
			</head>
			<body>
				<form onSubmit={this.submitPaymentForm.bind(this)} ref={this.attachNode.bind(this)} id="frmPayment" method={paymentDetail.method} action={paymentDetail.action} >
					{formKeys}
					<Button type="submit" onClick={this.handleClick} bsStyle="warning">Submit</Button>
				</form>
			</body >
		</html >);
	}
}
