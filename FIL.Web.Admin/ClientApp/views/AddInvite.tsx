import * as React from 'react';
import { connect } from 'react-redux';
import { IApplicationState } from '../stores';
import * as InviteStore from '../stores/EditInvite';
import { Formik, Field, Form } from 'formik';
import { UserInviteRequestViewModel } from '../models/UserInviteRequestViewModel';
import Yup from 'yup';
import './Login.scss';
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
class AddInvite extends React.Component<InviteComponentStateProps, any> {
  obj = {
    userEmail: '',
    userInviteCode: '',
    isUsed: false,
    id: 0
  };
  public initialValues: any;
  public schema: any;
  public componentWillMount() {
    // if (typeof window !== 'undefined') {
    // this.obj = JSON.parse(localStorage.getItem("invite"));
    // this.setState({value: this.obj.isUsed});
    // }
  }

  public componentDidMount() {
    if (window) {
      window.scrollTo(0, 0);
    }
  }
  constructor(props) {
    super(props);
    this.state = { value: false };
    this.handleChange = this.handleChange.bind(this);
  }

  handleChange(event) {
    let val = this.obj.isUsed == false ? 'on' : 'off';
    this.setState({ value: !this.obj.isUsed });
    this.obj.isUsed = !this.obj.isUsed;
  }
  public render() {
    this.schema = this.getSchema();
    this.initialValues = { email: '', invitecode: '', isused: false, id: 0 };
    return (
      <div className="card border-0 right-cntent-area pb-5 bg-light">
        <div className="card-body bg-light p-0">
          <div className="bg-white fil-admin-white-box shadow-sm mb-2 rounded-box dashboard-list">
            <nav className="navbar px-0 pt-0 bg-white mb-2 shadow-none">
              <h3 className="m-0 text-purple">Add Invite</h3>
            </nav>
            <div className="row invite">
              <div className="col-sm-6">
                <Formik
                  initialValues={this.initialValues || {}}
                  validationSchema={this.schema}
                  onSubmit={(values: Values) => {
                    setTimeout(() => {
                      let ui: UserInviteRequestViewModel = new UserInviteRequestViewModel();
                      ui.email = values.email;
                      ui.inviteCode = values.invitecode;
                      ui.isUsed = this.state.value;
                      this.props.updateInvite(ui, (response) => {
                        setTimeout(() => {
                          this.props.resetProps();
                        }, 2000);
                      });
                    }, 400);
                  }}
                  render={({ errors, touched, isSubmitting }) => (
                    <Form>
                      <div className="form-group">
                        <Field
                          placeholder="Email..."
                          className="form-control"
                          style={{ marginBottom: '10' }}
                          type="email"
                          name="email"
                        />
                      </div>
                      <div className="form-group">
                        <Field
                          placeholder="Invite Code"
                          className="form-control"
                          style={{ marginTop: '10px' }}
                          type="text"
                          name="invitecode"
                        />
                      </div>
                      <div className="checkbox">
                        <label>
                          <Field
                            style={{ transform: 'scale(1.5)', display: 'none', marginBottom: '15px' }}
                            checked={this.state.value}
                            onChange={this.handleChange}
                            type="checkbox"
                            name="isused"
                          />
                        </label>
                      </div>
                      <button className="btn btn-success" type="submit">
                        Add Invite
                      </button>
                    </Form>
                  )}
                />

                {this.props.inviteSuccess ? (
                  <div className="alert alert-success p-10 mt-10 text-left">
                    <small>{this.props.alertMessage}</small>
                  </div>
                ) : null}
                {this.props.inviteFailure ? (
                  <div className="alert alert-danger p-10 mt-10 text-left">
                    <small>{this.props.alertMessage}</small>
                  </div>
                ) : null}
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
  private getSchema() {
    return Yup.object().shape({
      email: Yup.string().email('Email is not valid').required('Email is required'),
      invitecode: Yup.string().required('Invite code is required'),
      isused: Yup.string().required('This value is required')
    });
  }
}

export default connect((state: IApplicationState) => state.editInvites, InviteStore.actionCreators)(AddInvite);
