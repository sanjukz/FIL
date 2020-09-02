import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";


interface ISubmitFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class SubmitForm extends React.Component<ISubmitFormProps, {}> {
    public render() {
        return (
            <Formik
                onSubmit={this.props.onSubmit}
                initialValues={this.props.initialValues || {}}
                validationSchema={this.props.validationSchema}

            >
                {(props) => (<Form>
                    {this.props.children}
                    {!this.props.hideSubmit &&
                        <div className="col-sm-12 pb-20 text-center">
                        <Button type="submit" className="btn btn-success">Submit</Button>
                        </div>
                    }
                </Form>)}
            </Formik>
        );
    }
}

