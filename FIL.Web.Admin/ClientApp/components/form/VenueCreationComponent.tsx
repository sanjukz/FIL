import { Field } from "formik";
import * as React from "react";
import FILForm from "./SharedForm/FILForm";
import Yup from "yup";
import { VenueCreationFormDataviewModel } from "../../models/VenueCreationFormDataviewModel"

interface IVenueCreationFormProps {
    onSubmit: (values: VenueCreationFormDataviewModel) => void;
}

export default class VenueCreationComponent extends React.Component<IVenueCreationFormProps, {}> {
    public render() {
        const schema = this.getSchema();
        return (
            <FILForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="input-field">
                    <Field type="text" name="name" placeholder="VenueName" className="form-control" required />
                </div>

                <div className="input-field">
                    <Field type="text" name="addressLineOne" placeholder="AddressLineOne" className="form-control" required />
                </div>

                <div className="input-field">
                    <Field type="text" name="addressLineTwo" placeholder="AddressLineTwo" className="form-control" required />
                </div>

                <div className="input-field">
                    <Field type="text" name="latitude" placeholder="Latitude" className="form-control" required />
                </div>

                <div className="input-field">
                    <Field type="text" name="longitude" placeholder="Longitude" className="form-control" required />
                </div>

                <div className="input-field">
                    <Field type="text" name="prefix" placeholder="Prefix" className="form-control" required />
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