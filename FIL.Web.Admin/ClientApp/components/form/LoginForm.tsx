import { Field } from "formik";
import * as React from "react";
import FILForm from "./SharedForm/FILForm";
import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import Yup from "yup";

interface ILoginFormProps {
    onSubmit: (values: LoginFormDataViewModel) => void;
}

export class LoginForm extends React.Component<ILoginFormProps, {}> {
    public render() {
        const schema = this.getSchema();
        return (
            <FILForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="input-field">
                    <Field type="text" name="email" placeholder="Username" className="form-control" required />
                </div>
                <div className="input-field">
                    <Field type="password" name="password" placeholder="Password" className="form-control" required />
                </div>
            </FILForm>
        );
    }

    private getSchema() {
        return Yup.object().shape({
            email: Yup.string().required(),
            password: Yup.string().required().min(5)
        });
    }
}
