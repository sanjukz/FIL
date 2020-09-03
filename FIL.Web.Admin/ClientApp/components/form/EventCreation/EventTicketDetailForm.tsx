import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";

interface IFILFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class EventTicketDetailForm extends React.Component<IFILFormProps, {}> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}

            >
                {(props) => (<Form className="d-block w-100 mt-3">
                    {this.props.children}
                    {!this.props.hideSubmit &&
                        <div className="card-footer text-center mt-3" style={{ margin: "-1.25rem" }} >
                        <Button className="btn btn-link">Reset</Button>
                        <Button type="submit" className="btn btn-primary custom-btn-lg" disabled={props.isSubmitting}>Submit</Button>
                       </div>
                    }
                </Form>)}
            </Formik>
        );
    }
}
