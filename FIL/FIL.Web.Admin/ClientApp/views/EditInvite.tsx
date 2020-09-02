import * as React from "react";
import { connect } from "react-redux";
import { IApplicationState } from "../stores";
import * as InviteStore from "../stores/EditInvite";
import { Formik, Field, Form } from "formik";
import { UserInviteRequestViewModel } from "../models/UserInviteRequestViewModel";
import Yup from "yup";
import "./Login.scss";
type InviteComponentStateProps = InviteStore.IInviteState & Values & typeof InviteStore.actionCreators;


interface IFormProps {
    initialValues: any;
    validationSchema: any;

}
interface Values {
    invitecode: string;
    isused: boolean;
    email: string;
    id: number;
}
class EditInvite extends React.Component<InviteComponentStateProps, any> {
    obj = {
        userEmail: '',
        userInviteCode: '',
        isUsed: false,
        id: 0
    };
    public initialValues: any;
    public schema: any;
    public componentWillMount() {

        if (typeof window !== 'undefined') {
            this.obj = JSON.parse(localStorage.getItem("invite"));
            this.setState({ value: this.obj.isUsed });
        }
    }

    public componentDidMount() {
        if (window) {
            window.scrollTo(0, 0)
        }
    }
    constructor(props) {
        super(props);
        this.state = { value: false };
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {
        let val = (this.obj.isUsed == false) ? 'on' : 'off';
        this.setState({ value: !this.obj.isUsed });
        this.obj.isUsed = !this.obj.isUsed;
    }
    public render() {

        const email = this.obj.userEmail;
        const invitecode = this.obj.userInviteCode;
        const isused = this.obj.isUsed;
        const id = this.obj.id;
        this.schema = this.getSchema();
        this.initialValues = { email: email, invitecode: invitecode, isused: isused, id: id };
        return <div>
            <div className="wrapper">

                <div className="container my-acnt-content pt-2 pb-3">
                    <div className="my-acnt-header">
                        <h3>Update Invite</h3>
                    </div>
                    <hr />
                    <div className="row invite">
                        <div className="col-sm-6 offset-2 left-act-pnl text-left">

                            <Formik
                                initialValues={this.initialValues || {}}
                                validationSchema={this.schema}
                                onSubmit={(values: Values) => {

                                    setTimeout(() => {
                                        let ui: UserInviteRequestViewModel = new UserInviteRequestViewModel();
                                        ui.email = values.email;
                                        ui.inviteCode = values.invitecode;
                                        ui.isUsed = this.state.value;
                                        ui.id = values.id;
                                        this.props.updateInvite(ui, (response) => {
                                            setTimeout(() => {
                                                this.props.resetProps();
                                            }, 2000);
                                        });
                                    }, 400);
                                }}
                                render={({ errors, touched, isSubmitting }) => (
                                    <Form>

                                        <Field type="hidden" name="id" />
                                        <div>
                                            <Field placeholder="Email..." className="form-control" style={{ marginBottom: "10" }} type="email" name="email" />
                                        </div>
                                        <div>
                                            <Field placeholder="Invite Code" className="form-control" type="text" name="invitecode" />
                                        </div>

                                        {/*
                                            <div className="checkbox">
                                            <div className="custom-control custom-checkbox">
                                                <input type="checkbox" className="custom-control-input" checked={this.state.value} onChange={this.handleChange} name="isused" />
                                                <label className="custom-control-label" htmlFor="isused">Is Used</label>
                                            </div>
                                            </div>
                                        */}

                                        <div className="mt-3">
                                            <button className="btn btn-success" type="submit">Update Invite</button>
                                        </div>
                                    </Form>
                                )}
                            />

                            {this.props.inviteSuccess ? <div className="alert alert-success p-10 mt-10 text-left"><small>{this.props.alertMessage}</small></div> : null}
                            {this.props.inviteFailure ? <div className="alert alert-danger p-10 mt-10 text-left"><small>{this.props.alertMessage}</small></div> : null}
                        </div>
                    </div>

                </div>
            </div>
        </div>
    }
    private getSchema() {
        return Yup.object().shape({
            email: Yup.string().email("Email is not valid").required("Email is required"),
            invitecode: Yup.string().required("Invite code is required"),
            isused: Yup.string().required("This value is required"),
        });
    }
}

export default connect(
    (state: IApplicationState) => state.editInvites,
    InviteStore.actionCreators
)(EditInvite);

