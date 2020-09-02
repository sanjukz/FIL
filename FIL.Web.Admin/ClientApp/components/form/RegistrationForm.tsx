import { Field } from "formik";
import * as React from "react";
import KzForm from "./SharedForm/KzForm";
import { RegistrationFormDataViewModel } from "shared/models/RegistrationFormDataViewModel";
import Yup from "yup";

interface IRegistrationFormProps {
    onSubmit: (values: RegistrationFormDataViewModel) => void;
}

export class RegistrationForm extends React.Component<IRegistrationFormProps, {}> {
    public render() {
        const schema = this.getSchema();
        return (
            <KzForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="row pb-2">
                    <div className="col-sm-6 pr-1 input-field">
                        <Field type="firstname"
                            name="firstname"
                            className="form-control"
                            placeholder="First Name"
                            required />
                    </div>
                    <div className="col-sm-6 pl-1 input-field">
                        <Field type="lastname"
                            name="lastname"
                            className="form-control"
                            placeholder="Last Name"
                            required />
                    </div>
                </div>
                <div className="input-field pb-2">
                    <Field type="username"
                        name="username"
                        placeholder="Username"
                        className="form-control"
                        required />
                </div>
                <div className="input-field pb-2">
                    <Field type="email"
                        name="email"
                        placeholder="Email"
                        className="form-control"
                        required />
                </div>
                <div className="input-field pb-2">
                    <Field type="password"
                        name="password"
                        className="form-control"
                        placeholder="Password"
                        required />
                </div>
                <div className="input-field pb-2">
                    <Field type="Password"
                        name="confPassword"
                        className="form-control"
                        placeholder="Confirm Password"
                        required />
                </div>
           </KzForm>
        );
    }

    private getSchema() {
        Yup.addMethod(Yup.mixed, 'sameAs', function (ref, message) {
            return this.test('sameAs', message, function (value) {
                let other = this.resolve(ref);
                return !other || !value || value === other;
            })
        });

        return Yup.object().shape({
            firstname: Yup.string().required(),
            lastname: Yup.string().required(),
            username: Yup.string().required(),
            password: Yup.string().required(),
            confPassword: Yup.string().sameAs(Yup.ref("password")).required()
        });        
    }
}
