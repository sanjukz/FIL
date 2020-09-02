import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";

interface IAccountFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: figure out where Yup SCHEMA went
}

export default class AccountForm extends React.Component<IAccountFormProps, {}> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}               
            >
                {(props) => (<Form>
                    {this.props.children}
                    <div>
                        <button type="submit" className="btn btn-primary mr-10">Change Password</button> 
                    </div>                    
                </Form>)}
            </Formik>
        );
    }

}

