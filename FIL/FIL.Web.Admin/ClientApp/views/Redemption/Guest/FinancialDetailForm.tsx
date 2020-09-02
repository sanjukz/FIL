import * as React from "react";
import { Formik, Form, Field, FormikProps } from "formik";
import Select from "react-select";
import * as _ from "lodash";
import CreateGuideFormFooter from "./CreateGuideFormFooter";
import AsyncSelect from "react-select/lib/Async";

const FinancialDetailForm: React.FunctionComponent<any> = (parentProps) => {
    return (
        <Formik
            enableReinitialize
            initialValues={{ ...parentProps.formState }}
            onSubmit={(values) => {
                parentProps.saveFormSegmentState("financialDetailForm", values,
                    () => parentProps.onTabChange("tab3"))
            }}
        >
            {(props: FormikProps<any>) => (
                <Form>
                    <div className="col-sm-12 pb-3">
                        <div className="form-group pt-3">
                            <div className="form-group">
                                <label className="d-block">Seller/Inventory Owner Information</label>
                                <div className="custom-control custom-radio custom-control-inline p-0">
                                    <label style={{ marginLeft: 5, marginBottom: 10, marginRight: 10 }} >
                                        <Field
                                            checked={props.values.accountType == "1" ? true : false}
                                            style={{ transform: "scale(1.5)", marginRight: 10 }}
                                            type="radio"
                                            value="1"
                                            onChange={() => { props.setFieldValue("accountType", 1) }}
                                            name="accountType" />
                                        Individual
                                    </label>
                                </div>
                                <div className="custom-control custom-radio custom-control-inline">
                                    <label style={{ marginLeft: 5, marginBottom: 10 }} >
                                        <Field
                                            checked={props.values.accountType == "2" ? true : false}
                                            style={{ transform: "scale(1.5)", marginRight: 10 }}
                                            onChange={() => { props.setFieldValue("accountType", 2) }}
                                            type="radio"
                                            value="2"
                                            name="accountType" />
                                        Company
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div className="form-group">
                            <div className="row">
                                <div className="col-12 col-sm-6">
                                    <label >Currency</label>
                                    <Select
                                        name="currency"
                                        value={props.values.currency}
                                        options={parentProps.currencyOptions}
                                        onChange={e => props.setFieldValue("currency", e)}
                                        required
                                    />
                                </div>
                                <div className="col-12 col-sm-6">
                                    <label >In which country will you be paid?</label>
                                    <Select
                                        name="financeCountry"
                                        defaultValue={parentProps.formState.financeCountry}
                                        value={props.values.financeCountry}
                                        options={parentProps.countryOptions}
                                        onChange={e => {
                                            props.setFieldValue("financeCountry", e)
                                            parentProps.getDefaultStateOption(e)
                                        }}
                                        required
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="form-group">
                            <div className="row">
                                <div className="col-12 col-sm-6">
                                    <label >Bank Account Type</label>
                                    <select
                                        value={parentProps.formState.bankAccountType}
                                        name="bankAccountType"
                                        className="form-control"
                                        onChange={(e) => { props.setFieldValue("bankAccountType", e.target.value) }}
                                        required>
                                        <option value="1" >Checking/Current</option>
                                        <option value="2" >Saving</option>
                                    </select>
                                </div>
                                <div className="col-12 col-sm-6">
                                    <label >Bank Name</label>
                                    <Field
                                        type="text"
                                        className="form-control"
                                        name="bankName"
                                        placeholder="Enter Bank Name"
                                        required />
                                </div>
                            </div>
                        </div>
                        <div className="form-group">
                            <div className="row">
                                <div className="col-12 col-sm-6">
                                    <label >Account Number</label>
                                    <Field
                                        type="text"
                                        className="form-control"
                                        name="accountNumber"
                                        placeholder="Enter Account Number"
                                        required />
                                </div>
                                <div className="col-12 col-sm-6">
                                    <label >Branch Code</label>
                                    <Field
                                        type="text"
                                        className="form-control"
                                        name="branchCode"
                                        placeholder="Enter Branch Code"
                                        required />
                                </div>
                            </div>
                        </div>
                        <div className="form-group">
                            <div className="row">
                                <div className="col-12 col-sm-6">
                                    <label >Select State</label>
                                    <Select
                                        name="financeState"
                                        value={props.values.financeState}
                                        onChange={e => props.setFieldValue("financeState", e)}
                                        options={parentProps.stateOptions}
                                        required
                                    />
                                </div>
                                <div className="col-12 col-sm-6">
                                    <label >Enter Routing Number (Optional)</label>
                                    <Field
                                        type="text"
                                        className="form-control"
                                        name="routingNumber"
                                        placeholder="Enter Routing Name"
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="form-group">
                            <div className="row">
                                <div className="col-12 col-sm-6">
                                    <label>Tax information</label>
                                    <div className="custom-control custom-checkbox p-0">
                                        <label  >
                                            <Field
                                                type="checkbox"
                                                checked={parentProps.formState.isTax}
                                                style={{ transform: "scale(1.5)", marginRight: 10 }}
                                                name="isTax"
                                                value={props.values.isTax}
                                            />
                                            I have Tax/ GST Number   </label>
                                    </div>
                                </div>
                                {(props.values.isTax) && <div className="col-12 col-sm-12">
                                    <label >Tax Id</label>
                                    <Field
                                        type="text"
                                        className="form-control"
                                        name="taxId"
                                        placeholder="Enter Tax Code"
                                    />
                                </div>}
                            </div>
                        </div>
                        <CreateGuideFormFooter
                            previousTabHandler={() => parentProps.onTabChange("tab1")}
                        />
                    </div>
                </Form>
            )}
        </Formik >
    );
}

export default FinancialDetailForm;