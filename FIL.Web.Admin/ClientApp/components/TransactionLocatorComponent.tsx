import { Field } from "formik";
import * as React from "react";
import CommonForm from "./form/BOCommonForm";
import { TransactionLocatorFormData } from "../models/TransactionLocator/TransactionLocatorFormData";
import Yup from "yup";
import "./Locator.scss";

interface ITransactionLocatorComponentProps {
    onSubmit: (values: TransactionLocatorFormData) => void;
}

export default class TransactionLocatorComponent extends React.Component<ITransactionLocatorComponentProps, any> {

    public render() {
        const schema = this.getSchema();

        return (
            <CommonForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="row">
                    <div className="col-md-5 mb-2">
                        <div className="input-group mb-2">
                            <div className="input-group-prepend">
                                <div className="input-group-text" id="">
                                    <span className="fa fa-user" aria-hidden="true"></span>
                                </div>
                            </div>
                            <Field type="firstName" name="firstName" className="form-control" placeholder="Enter First Name" />
                        </div>
                    </div>
                    <div className="col-md-5 mb-2">
                        <div className="input-group mb-2">
                            <div className="input-group-prepend">
                                <div className="input-group-text" id="">
                                    <span className="fa fa-user" aria-hidden="true"></span>
                                </div>
                            </div>
                            <Field type="lastName" name="lastName" className="form-control" placeholder="Enter Last Name" />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-5 mb-2">
                        <div className="input-group mb-2">
                            <div className="input-group-prepend">
                                <div className="input-group-text" id="">
                                    <span className="fa fa-at" aria-hidden="true"></span>
                                </div>
                            </div>
                            <Field type="email" name="emailid" className="form-control" placeholder="Enter Email Id" />
                        </div>
                    </div>
                    <div className="col-md-5 mb-2">
                        <div className="input-group mb-2">
                            <div className="input-group-prepend">
                                <div className="input-group-text" id="">
                                    <span className="fa fa-mobile" aria-hidden="true"></span>
                                </div>
                            </div>
                            <Field type="number" name="userMobileNo" className="form-control" placeholder="Enter Mobile Number" />
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-5 mb-2">
                        <div className="input-group mb-2">
                            <div className="input-group-prepend">
                                <div className="input-group-text" id="">
                                    <span className="fa fa-ticket" aria-hidden="true"></span>
                                </div>
                            </div>
                            <Field type="number" name="confirmationNumber" className="form-control" placeholder="Enter Confirmation No." />
                        </div>
                    </div>
                    <div className="col-md-5 mb-2">
                        <div className="input-group mb-2">
                            <div className="input-group-prepend">
                                <div className="input-group-text" id="">
                                    <span className="fa fa-barcode" aria-hidden="true"></span>
                                </div>
                            </div>
                            <Field type="barcodeNumber" name="barcodeNumber" className="form-control" placeholder="Enter Barcode Number" />
                        </div>
                    </div>
                </div>
            </CommonForm >

        );

    }
    private getSchema() {
        return Yup.object().shape({
        });
    }
}
