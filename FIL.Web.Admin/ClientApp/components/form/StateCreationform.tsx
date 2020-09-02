import { Field } from "formik";
import * as React from "react";
import KzForm from "./SharedForm/KzForm";
import Yup from "yup";
import { StateFormDataViewModel } from "../../models/StateFormDataViewModel";

interface IStateFormProps {
    onSubmit: (values: StateFormDataViewModel) => void;
}

export default class CountryCreationForm extends React.Component<IStateFormProps, any> {
    public render() {
        const schema = this.getSchema();
        return (
            <KzForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="input-field">
                    <Field type="text" name="name" placeholder="StateName" className="form-control" required />
                </div>
                <div className="input-field">
                    <Field type="text" name="abbreviation" placeholder="Abbrevation" className="form-control" required />
                </div>
            </KzForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({
            
        });
    }
}
