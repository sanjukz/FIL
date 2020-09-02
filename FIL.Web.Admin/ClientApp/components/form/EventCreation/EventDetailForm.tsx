import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";
import { Link, RouteComponentProps } from "react-router-dom";

interface IKzFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
    onCancle: () => void;
}

export default class EventDetailForm extends React.Component<IKzFormProps, {}> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}
            >
                {(props) => (<Form className="d-block w-100 mt-3 pb-3 mb-3 border-bottom">
                    {this.props.children}
                    {!this.props.hideSubmit &&
                        <div className="mt-3">
                        
                        <Button type="submit" className="btn btn-primary btn-sm" disabled={props.isSubmitting}>Submit</Button>
                      </div>                  
                    }
                </Form>)}
            </Formik>
        );
    }
}
