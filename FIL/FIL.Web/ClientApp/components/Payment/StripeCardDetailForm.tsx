import { autobind } from "core-decorators";
import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";
import {
    CardElement,
    injectStripe,
} from "react-stripe-elements";
import { PaymentFormDataViewModel } from "../../models/Payment/PaymentFormDataViewModel";
import * as debounce from "lodash/debounce";

interface IPickupDetailFormProps {
    onSubmit: (values: any, stripe: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: figure out where Yup SCHEMA went
    siteId: number;
    requesting: boolean;
    stripe: any;
    countries: any;
}

class StripeCardDetailForm extends React.Component<IPickupDetailFormProps, any> {

    @autobind
    onSubmit(values: PaymentFormDataViewModel) {
        debounce(() => {
            let address: any = {};
            if (values.zipcode) {
                address.postal_code = values.zipcode.toString();
            }
            if (values.country) {
                let country = this.props.countries.find(item => item.name == values.country);
                if (country && country.isoAlphaTwoCode) {
                    address.country = country.isoAlphaTwoCode.toString();
                }
            }
            if (values.state) {
                address.state = values.state.toString();
            }

            this.props.stripe.createPaymentMethod('card', {
                billing_details: {
                    address
                }
            }).then((response) => {
                this.props.onSubmit(values, response);
            });
        }, 500)();
    }

    public render() {
        let buttonDisabled = this.props.requesting;
        return <Formik
            onSubmit={this.onSubmit}
            initialValues={this.props.initialValues || {}}
            validationSchema={this.props.validationSchema}
        >
            {(props) => (<Form className="row">
                <div className="col-sm-6">
                    <h5 className="mt-0 mb-10 text-black">Card Details</h5>
                    <div className="login-ctrl clearfix">
                        <CardElement />
                    </div>
                </div>
                {this.props.children}
                <div className="col-sm-12 pt-20 text-right">
                    <Button type="submit" disabled={buttonDisabled} className="btn btn-success visible-xs">Continue</Button>
                </div>
            </Form>)}
        </Formik>
    }
}

export default injectStripe(StripeCardDetailForm)