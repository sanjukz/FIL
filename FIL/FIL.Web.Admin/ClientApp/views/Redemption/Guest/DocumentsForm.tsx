import * as React from "react";
import { Formik, Form, Field, FormikProps } from "formik";
import Select from "react-select";
import * as _ from "lodash";
import DocumentImageUpload from "../../../components/Redemption/Guide/DocumentImageUpload";
import CreateGuideFormFooter from "./CreateGuideFormFooter";

const DocumentsForm: React.FunctionComponent<any> = (parentProps) => {
    return (
        <Formik
            enableReinitialize
            initialValues={{...parentProps.formState}}
            onSubmit={(values) => {
                parentProps.saveFormSegmentState("documentsForm", values,
                    parentProps.submitFormHandler)
            }}
        >
            {(props: FormikProps<any>) => (
                <Form>
                    <div className="col-sm-12 pb-3">
                        <div className="form-group">
                            <div className="row">
                                <div className="col-sm-6">
                                    <label >Select Document Type</label>
                                    <Select 
                                        name="document"
                                        value={props.values.document}
                                        defaultValue={parentProps.formState.document}
                                        options={parentProps.documentOptions}
                                        onChange={e => props.setFieldValue("document", e)}
                                        required
                                    />
                                </div>
                            </div>
                            <div className="row">
                                <div className="col-sm-6">
                                    <DocumentImageUpload uploadImage={(item) => {
                                    }} />
                                </div>
                            </div>
                        </div>
                        <CreateGuideFormFooter
                            previousTabHandler={() => parentProps.onTabChange("tab3")}
                            isDisabledNextButton={true}
                        />
                    </div>
                    <div className="text-center pt-4 pb-4">
                        <button type="submit" className="btn btn-outline-primary">
                            Submit
                        </button>
                    </div>
                </Form>
            )}
        </Formik>
    );
}

export default DocumentsForm;