import { Field } from "formik";
import * as React from "react";
import FILForm from "./SharedForm/FILForm";
import Yup from "yup";
import { CityFormDataViewModel } from "../../models/CityFormDataViewModel";

interface ICityFormProps {
    onSubmit: (values: CityFormDataViewModel) => void;
}

export default class CountryCreationForm extends React.Component<ICityFormProps, any> {
    public render() {
        const schema = this.getSchema();
        return (
            <FILForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="input-field">
                    <Field type="text" name="name" placeholder="CityName" className="form-control" required />
                </div>
            </FILForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({

        });
    }
}
