import { Form, Formik } from "formik";
import * as React from "react";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";

interface ICommonFormProps {
    onSubmit: (values: any) => void;
    initialValues: any;
    validationSchema: any; // TODO: Figure out where yup schema went
    hideSubmit?: boolean;
}

export default class BoCommonForm extends React.Component<ICommonFormProps, {}> {
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
                        <div className="text-center mt-4 mb-2">
                            <Button type="submit" style={{ marginRight: "180px" }} className="btn btn-success">Submit</Button>
                        </div>
                    }
                </Form>)}
            </Formik>
        );
    }
}
