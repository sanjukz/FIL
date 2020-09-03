import { Field } from "formik";
import * as React from "react";
import FILForm from "./SharedForm/FILForm";
import Yup from "yup";
import { CountryFormDataViewModel } from "../../models/CountryFormDataViewModel";

interface ICountryFormProps {
    onSubmit: (values: CountryFormDataViewModel) => void;
}

export default class CountryCreationForm extends React.Component<ICountryFormProps, any> {
    public render() {
        const schema = this.getSchema();
        return  (
            <FILForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="input-field">
                    <Field type="text" name="name" placeholder="CountryName" className="form-control" required />
                </div>
                <div className="input-field">
                    <Field type="text" name="isoAlphaTwoCode" placeholder="AlphaTwoCode" className="form-control" required />
                </div>  
                <div className="input-field">
                    <Field type="text" name="isoAlphaThreeCode" placeholder="AlphaThreeCode" className="form-control" required />
                </div>  
            </FILForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({
            //name: Yup.string().countryname().required(),
            //isoAlphaTwoCode: Yup.string().isoAlphaTwoCode().required(),
            //isoAlphaThreeCode: Yup.string().isoAlphaThreeCode().required(),
        });
    }
}
