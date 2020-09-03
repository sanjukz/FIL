import * as React from "React";
import {Button } from "react-bootstrap";
import { NewsLetterSignupFooterDataViewModel } from "../../models/Footer/NewsLetterSignupFooterDataViewModel";
import FilForm from "shared/components/form/FilForm";
import * as Yup from "yup";
import { Field } from "formik";
import { gets3BaseUrl } from "../../utils/imageCdn";

interface ISubscribeProps {
    onSubmit: (values: NewsLetterSignupFooterDataViewModel) => void;
}

export default class Notifyme extends React.Component<ISubscribeProps, any> {
    public render() {
        const schema = this.getSchema();
        return (
            <FilForm {...this.props} hideSubmit={true} validationSchema={schema} initialValues={{}}>
                <div className="bg-light">
                    <div className="container sign-form pt-3 pb-5 text-center">
                        <img src={`${gets3BaseUrl()}/logos/coming-soon-logo.jpg`} alt="coming soon logo" className="rounded-circle" />
                        <h2 className="mt-2 text-danger">Coming Soon</h2>
                        <p className="m-0">We're laying the foundation that'll change the way you <img src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`} alt="feelitLIVE logo" width="15" /> a place.</p>
                        <p>Sign up to get notified when we launch this <img src={`${gets3BaseUrl()}/footer/feel-heart-logo.png`} alt="feelitLIVE logo" width="15" /></p>
                        <div className="form-inline">
                            <div className="form-group m-auto">
                                <Field type="email" name="email" placeholder="Email" className="form-control rounded-0" required />
                                <Button type="submit" className="btn btn-primary rounded-0">Subscribe</Button>
                            </div>
                        </div>
                        <div className="social-link h3 mt-3">
                            <a href="https://www.instagram.com/feelit_live/" target="_blank"><i className="fa fa-instagram site-txt-primery-color"></i></a>
                            <a href="https://www.facebook.com/feelitlivecom/" target="_blank"><i className="fa fa-facebook-square site-txt-primery-color mr-1 ml-1"></i></a>
                            <a href="https://twitter.com/feelit_LIVE" target="_blank"><i className="fa fa-twitter-square site-txt-primery-color"></i></a>
                        </div>
                    </div>
                </div>
            </FilForm>
        );
    }
    private getSchema() {
        return Yup.object().shape({
            email: Yup.string().email().required()
        });
    }

}
