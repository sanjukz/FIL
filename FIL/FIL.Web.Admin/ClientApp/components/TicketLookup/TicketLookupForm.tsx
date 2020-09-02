import * as React from "react";
import Yup from "yup";
import SubmitForm from "../../components/TicketLookup/Form";
import { Field, FormikValues } from "formik";
import { TicketLookupDataViewModel } from "../../models/TicketLookup/TicketLookupDataViewModel";

interface ITicketLookupProps {
    onSubmit: (values: TicketLookupDataViewModel) => void;
}
export default class TicketLookupForm extends React.Component<ITicketLookupProps, any> {
    public render() {
        const schema = this.getSchema();
        return (

            <SubmitForm {...this.props} validationSchema={schema} initialValues={{}}>
                <div className="col-sm-6">
                    <div className="form-group">
                        <Field type="text" name="transactionId" placeholder="TransactionId" className="form-control" />
                    </div>
                </div>
                <div className="col-sm-6">
                    <div className="form-group">
                        <Field type="text" name="emailId" placeholder="EmailId" className="form-control" />
                    </div>
                </div>
                <div className="col-sm-6">
                    <div className="form-group">
                        <Field type="text" name="name" placeholder="Name" className="form-control" />
                    </div>
                </div>
                <div className="col-sm-6">
                    <div className="form-group">
                        <Field type="text" name="phone" placeholder="Phone Number" className="form-control" />
                    </div>
                </div>
            </SubmitForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({
            //  transactionId: Yup.number().transactionId().required()
        });
    }
}