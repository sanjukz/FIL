import { Field } from "formik";
import * as React from "react";
import KzForm from "./SharedForm/KzForm";
import Yup from "yup";
import { CityFormDataViewModel } from "../../models/CityFormDataViewModel";

interface ICityFormProps {
    onSubmit: (values: CityFormDataViewModel) => void;
}

export default class CountryCreationForm extends React.Component<ICityFormProps, any> {
    public render() {
        const schema = this.getSchema();
        return (
            <KzForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="input-field">
                    <Field type="text" name="name" placeholder="CityName" className="form-control" required />
                </div>
            </KzForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({

        });
    }
}
