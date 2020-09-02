import * as React from "react";
import { Formik, Form, Field, FormikProps } from "formik";
import Select from "react-select";
import * as _ from "lodash";
import CreateGuideFormFooter from "./CreateGuideFormFooter";

const CreateGuideForm: React.FunctionComponent<any> = (parentProps) => {
    return (
        <Formik
            enableReinitialize
            initialValues={{ ...parentProps.formState }}
            onSubmit={(values) => {
                parentProps.saveFormSegmentState("createGuideForm", values, () => parentProps.onTabChange("tab2"))
            }}
        >
            {(props: FormikProps<any>) => (
                <Form>
                    <div className="col-sm-12 pb-3">
                        <div className="form-group pt-3">
                            <div className="row">
                                <div className="col-12">
                                    <label>Search Place</label>
                                </div>
                                <div className="col-12 col-sm-12">
                                    <Select
                                        name="destinationname"
                                        isMulti
                                        required
                                        placeholder="Enter destination (Place, Country, Region or City)"
                                        onChange={e => props.setFieldValue('destinationname', e)}
                                        isClearable={true}
                                        options={parentProps.placeOptions}
                                        value={props.values.destinationname}
                                        onInputChange={parentProps.getAlgoliaResult}
                                        noOptionsMessage={() => { return "Search Place, Country, City, Region..." }}
                                        formatOptionLabel={parentProps.formatOptionLabel}
                                    />
                                </div>
                            </div>
                            <div className="form-group">
                                <div className="row">
                                    <div className="col-12 col-sm-6">
                                        <label >First Name</label>
                                        <Field
                                            name="firstName"
                                            placeHolder="Enter First Name"
                                            className="form-control"
                                            type="text"
                                            required />
                                    </div>
                                    <div className="col-12 col-sm-6">
                                        <label >Last Name</label>
                                        <Field
                                            name="lastName"
                                            placeHolder="Enter Last Name"
                                            className="form-control"
                                            type="text"
                                            required />
                                    </div>
                                </div>
                            </div>
                            <div className="form-group">
                                <div className="row">
                                    <div className="col-12 col-sm-6">
                                        <label >Email</label>
                                        <Field
                                            name="email"
                                            placeHolder="Enter Email"
                                            className="form-control"
                                            type="email"
                                            required />
                                    </div>
                                    <div className="col-12 col-sm-6">
                                        <label >Select Languages</label>
                                        <Select
                                            name="language"
                                            isMulti
                                            defaultValue={parentProps.formState.language}
                                            value={props.values.language}
                                            options={parentProps.languageOptions}
                                            onChange={e => props.setFieldValue("language", e)}
                                            required
                                        />
                                    </div>
                                </div>
                            </div>
                            <div className="form-group">
                                <label>Phone Number</label>
                                <div className="row">
                                    <div className="col-sm-3">
                                        <Select
                                            name="phonecode"
                                            value={props.values.phonecode}
                                            options={parentProps.countryOptions.map(item => ({
                                                label: `(+${item.phonecode}) ${item.label}`,
                                                value: item.value,
                                                phonecode: item.phonecode
                                            }))}
                                            onChange={e => {
                                                props.setFieldValue("phonecode", e)
                                            }}
                                            required
                                        />
                                    </div>
                                    <div className="col-sm-9">
                                        <Field
                                            type="phone"
                                            pattern="[0-9]*"
                                            className="form-control"
                                            name="phoneNumber"
                                            placeholder="Enter Mobile Number"
                                            required />
                                    </div>

                                </div>
                            </div>
                            <div className="form-group">
                                <label>Your Address</label>
                                <div className="row mb-1">
                                    <div className="col-sm-6 col-12 mt-2">
                                        <Field
                                            name="address1"
                                            type="text"
                                            className="form-control"
                                            placeholder="Address Line 1"
                                            required
                                        />
                                    </div>
                                    <div className="col-sm-6 col-12 mt-2">
                                        <Field
                                            name="address2"
                                            type="text"
                                            className="form-control"
                                            placeholder="Address Line 2"
                                        />
                                    </div>
                                </div>
                                <div className="row mb-1">
                                    <div className="col-sm-6 col-12">
                                        <Select
                                            name="residentCountry"
                                            placeholder="Select Country"
                                            defaultValue={parentProps.formState.residentCountry}
                                            value={props.values.residentCountry}
                                            options={parentProps.countryOptions}
                                            onChange={e => {
                                                props.setFieldValue("residentCountry", e)
                                                parentProps.getDefaultStateOption(e)
                                            }}
                                            required
                                        />
                                    </div>
                                    <div className="col-sm-6 col-12">
                                        <Select
                                            name="residentState"
                                            placeholder="Select State"
                                            defaultValue={parentProps.formState.residentState}
                                            value={props.values.residentState}
                                            options={parentProps.stateOptions}
                                            onChange={e => {
                                                props.setFieldValue("residentState", e)
                                                parentProps.getDefaultCityOption(e)
                                            }}
                                            required
                                        />
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col-sm-6 col-12">
                                        <Select
                                            name="residentCity"
                                            placeholder="Select City"
                                            defaultValue={parentProps.formState.residentCity}
                                            value={props.values.residentCity}
                                            options={parentProps.cityOptions}
                                            onChange={e => props.setFieldValue("residentCity", e)}
                                            required
                                        />
                                    </div>
                                    <div className="col-sm-6 col-12">
                                        <Field
                                            name="zip"
                                            type="text"
                                            className="form-control"
                                            placeholder="Zip/Postal"
                                            required
                                        />
                                    </div>
                                </div>
                            </div>
                            <CreateGuideFormFooter
                                isDisabledPreviousButton={true}
                            />
                        </div>
                    </div>
                </Form>
            )}
        </Formik>
    );
}

export default CreateGuideForm;