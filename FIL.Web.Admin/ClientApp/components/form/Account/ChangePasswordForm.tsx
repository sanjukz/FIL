import { Field } from "formik";
import * as React from "react";
import AccountForm from "./AccountForm";
import { ChangePasswordFormDataViewModel } from "../../../models/Account/ChangePasswordFormDataViewModel";

interface IChangePasswordFormProps {
    onSubmit: (values: ChangePasswordFormDataViewModel) => void;
}

export class ChangePasswordForm extends React.Component<IChangePasswordFormProps, {}> {
    public render() {
        const schema = this.getSchema();
        return (
            <AccountForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="form-group">
                    <Field type="password" name="oldPassword" placeholder="Old Password" className="form-control" required />
                    <Field type="password" ref="password" name="newPassword" placeholder="New Password" className="form-control" required />
                    <div className="myaccount-error">
                        <Field type="password" name="confirmPassword" placeholder="Confirm New Password" className="form-control" required />
                    </div>
                </div>
            </AccountForm>
        );
    }

    private getSchema() {
        
    }
}
