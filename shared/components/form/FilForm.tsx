import { Form, Formik } from "formik";
import * as React from "react";
// import { Button } from "react-bootstrap";
import "./FilForm.scss";

interface IFilFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class FilForm extends React.Component<IFilFormProps, {}> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}
                
            >
                {(props) => (<Form method="POST">
                    {this.props.children}
                    {!this.props.hideSubmit && 
                        <button type="submit" className="btn btn-block btn-primary" disabled={props.isSubmitting}>Submit</button>
                    }
                </Form>)}
            </Formik>
        );
    }
}

