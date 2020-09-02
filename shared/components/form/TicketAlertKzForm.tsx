import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";

interface IKzFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class TicketAlertKzForm extends React.Component<IKzFormProps, any> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}
            >
                {(props) => (<Form method="POST">
                    {this.props.children}
                    {!this.props.hideSubmit && <div> <hr />
                        <div className="mb-5 text-center">
                            <Button type="submit" className="btn btn-success">Submit</Button>
                        </div>
                        </div>
                    }
                </Form>)}
            </Formik>
        );
    }
}

