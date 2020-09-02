import * as React from "react";
import { Link, RouteComponentProps } from "react-router-dom";
import { IApplicationState } from "../stores";
import * as PaymentErrorStore from "../stores/PaymentError";
import { connect } from "react-redux";
import * as PubSub from "pubsub-js";

type PaymentErrorDescriptionProps = PaymentErrorStore.IPaymentErrorResponseState & typeof PaymentErrorStore.actionCreators & RouteComponentProps<{}>;

export class PaymentError extends React.Component<PaymentErrorDescriptionProps, {}> {
    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }

        var errorName = this.props.location.search.split('?')[1];
        this.props.getPaymentErrorDescription(errorName);
        localStorage.removeItem("cartItems");
        localStorage.removeItem("currentCartItems");
        PubSub.publish("UPDATE_NAV_CART_DATA_EVENT", 1);
        PubSub.publish("UPDATE_CART_COUNT_EVENT", 1);
    }

    public render() {
        return <div>
            <div className="container forgot-password pt-3 pb-5 inner-banner">
                <div className="mt-20">
                    <h4>There was an error while processing your payment - {this.props.paymentErrorDescription.errorDescription}</h4>
                    Please input your information and try again. If you are still facing problems, please contact us at support@feelitLIVE.com. 
				<div className="mb-20 mt-20">
                        <Link to="/" className="btn btn-success">Continue</Link>
                    </div>
                    For further assistance please write to us at support@feelitLIVE.com
			</div>
            </div>
        </div>;
    }
}

export default connect(
    (state: IApplicationState) => state.PaymentError,
    PaymentErrorStore.actionCreators
)(PaymentError);
