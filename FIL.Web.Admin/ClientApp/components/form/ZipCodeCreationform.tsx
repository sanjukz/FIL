import { Field } from "formik";
import * as React from "react";
import FILForm from "./SharedForm/FILForm";
import Yup from "yup";
import { ZipcodeFormDataViewModel } from "../../models/ZipcodeFormDataViewModel";

interface IZipcodeFormProps {
    onSubmit: (values: ZipcodeFormDataViewModel) => void;
}

export default class CountryCreationForm extends React.Component<IZipcodeFormProps, any> {
    public render() {
        const schema = this.getSchema();
        return (
            <FILForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="input-field">
                    <Field type="text" name="zipcode" placeholder="Zipcode" className="form-control" required />
                </div>
                <div className="input-field">
                    <Field type="text" name="region" placeholder="Region" className="form-control" required />
                </div>
            </FILForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({

        });
    }
}
