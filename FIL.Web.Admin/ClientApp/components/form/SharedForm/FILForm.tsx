import { Form, Formik, FormikProps } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";
import "./FILForm.scss";

interface IFILFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class FILForm extends React.Component<IFILFormProps, any> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}
                
            >
                {(props: FormikProps<any>) => (<Form method="POST">
                    {this.props.children}
                    {!this.props.hideSubmit && 
                        <Button type="submit" className="btn btn-block btn-primary" disabled={props.isSubmitting}>Submit</Button>
                    }
                </Form>)}
            </Formik>
        );
    }
}

