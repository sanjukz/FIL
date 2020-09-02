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

export default class ResetPasswordKzForm extends React.Component<IKzFormProps, any> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}
            >
                {(props) => (<Form method="POST">
                    {this.props.children}
                    {!this.props.hideSubmit && <div className="row">
                        <div className="col-sm-6 mb-5"><Button type="submit" className="btn site-primery-btn">Reset Password</Button></div>                        
                    </div>
                    }
                </Form>)}
            </Formik>
        );
    }
}

