import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";

interface IKzFormProps {
    onSubmit: (values: any) => void;
    onClick: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class ForgotPasswordKzForm extends React.Component<IKzFormProps, {}> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                onClick={this.props.onClick}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}
            >
                {(props) => (<Form>
                    {this.props.children}
                    {!this.props.hideSubmit &&
                        <div className="row">
                            <div className="col-sm-6  mb-5">
                            <a onClick={this.props.onClick} href="#" className="text-info"><small>Already a member? Sign In</small></a>
                            </div>
                            <div className="col-sm-6 text-right">
                                <Button type="submit" className="btn btn-info">Submit</Button>
                            </div>
                        </div>
                    }
                </Form>)}
            </Formik>
        );
    }
}

