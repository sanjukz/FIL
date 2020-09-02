import * as React from "react";
import { Formik, Form, Field, FormikProps } from "formik";
import * as _ from "lodash";
import ServiceModalComponent from "../../../components/Redemption/Guide/ServiceModalComponent";
import { Service } from "../../../models/Redemption/Service";
import CKEditor from "react-ckeditor-component";
import CreateGuideFormFooter from "./CreateGuideFormFooter";

const ServicesForm: React.FunctionComponent<any> = (parentProps) => {
    return (
        <Formik
            enableReinitialize
            initialValues={{ ...parentProps.formState }}
            onSubmit={(values) => {
                parentProps.saveFormSegmentState("servicesForm", values,
                    () => parentProps.onTabChange("tab4"))
            }}
        >
            {(props: FormikProps<any>) => (
                <Form>
                    <div className="col-sm-12 pb-3">
                        <div className="form-group">
                            <div className="row">
                                <div className="col-12">
                                    <ServiceModalComponent
                                        services={parentProps.formState.selectedServices || []}
                                        onClickSubmit={(item: Service[]) => {
                                            props.setFieldValue("selectedServices", item);
                                        }}
                                    />
                                </div>
                                <div className="col-12">
                                    <label >Service Notes</label>
                                    <CKEditor
                                        activeClass="p10"
                                        content={props.values.serviceNotes}
                                        required
                                        events={{
                                            "change": (e) => {
                                                props.setFieldValue("serviceNotes", e.editor.getData())
                                            }
                                        }}
                                    />
                                </div>
                            </div>
                        </div>
                        <CreateGuideFormFooter
                            previousTabHandler={() => parentProps.onTabChange("tab2")}
                        />
                    </div>
                </Form>
            )}
        </Formik>
    );
}

export default ServicesForm;