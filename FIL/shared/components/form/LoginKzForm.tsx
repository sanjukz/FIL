import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";

interface IKzFormProps {
    onSubmit: (values: any) => void;
    onClick: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class LoginKzForm extends React.Component<IKzFormProps, {}> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                onClick={this.props.onClick}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}
            >
                {(props) => (<Form method="POST">
                    {this.props.children}
                    {!this.props.hideSubmit &&  
                        <div className="row">
                      <div className="col-sm-6  mb-5">
                        <a onClick={this.props.onClick} href="#" className="text-info"><small>Forgot your ID and password?</small></a>
                      </div>
                      <div className="col-sm-6 text-right">
                        <Button type="submit" className="btn btn-info" disabled={props.isSubmitting}>Sign In</Button>
                      </div>
                    </div>
                    }
                </Form>)}
            </Formik>
        );
    }
}

