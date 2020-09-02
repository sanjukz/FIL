import * as React from 'react';
import { Footer } from '../../../views/EventManagement/Footer/FormFooter';

export const TransactionLocatorForm = (props: any) => {
  const [firstName, setFirstName] = React.useState("");
  const [lastName, setLastName] = React.useState("");
  const [email, setEmail] = React.useState("");
  const [phoneNumber, setPhoneNumber] = React.useState("");
  const [confirmationId, setConfirmationId] = React.useState("");
  let form;
  return <form
    ref={(ref) => {
      this.form = ref
    }}
    onSubmit={(e: any) => {
      props.onSubmit(`?TransactionId=${confirmationId ? confirmationId : 0}&FirstName=${firstName ? firstName : 0}&LastName=${lastName ? lastName : 0}&EmailId=${email ? email : 0}&UserMobileNo=${phoneNumber ? phoneNumber : 0}`);
    }}>
    <div className="row mt-3">
      <div className="col-md-6 mb-2">
        <div className="input-group mb-2">
          <div className="input-group-prepend">
            <div className="input-group-text input-group-zs" id="">
              <span className="fa fa-user" aria-hidden="true"></span>
            </div>
          </div>
          <input onChange={e => setFirstName(e.target.value)} type="firstName" name="firstName" className="form-control" placeholder="Enter First Name" />
        </div>
      </div>
      <div className="col-md-6 mb-2">
        <div className="input-group mb-2">
          <div className="input-group-prepend">
            <div className="input-group-text input-group-zs" id="">
              <span className="fa fa-user" aria-hidden="true"></span>
            </div>
          </div>
          <input onChange={e => setLastName(e.target.value)} type="lastName" name="lastName" className="form-control" placeholder="Enter Last Name" />
        </div>
      </div>
    </div>
    <div className="row">
      <div className="col-md-6 mb-2">
        <div className="input-group mb-2">
          <div className="input-group-prepend">
            <div className="input-group-text input-group-zs" id="">
              <span className="fa fa-at" aria-hidden="true"></span>
            </div>
          </div>
          <input onChange={e => setEmail(e.target.value)} type="email" name="emailid" className="form-control" placeholder="Enter Email Id" />
        </div>
      </div>
      <div className="col-md-6 mb-2">
        <div className="input-group mb-2">
          <div className="input-group-prepend">
            <div className="input-group-text input-group-zs" id="">
              <span className="fa fa-mobile" aria-hidden="true"></span>
            </div>
          </div>
          <input onChange={e => setPhoneNumber(e.target.value)} type="number" name="userMobileNo" className="form-control" placeholder="Enter Mobile Number" />
        </div>
      </div>
    </div>
    <div className="row">
      <div className="col-md-6 mb-2">
        <div className="input-group mb-2">
          <div className="input-group-prepend">
            <div className="input-group-text input-group-zs" id="">
              <span className="fa fa-ticket" aria-hidden="true"></span>
            </div>
          </div>
          <input onChange={e => setConfirmationId(e.target.value)} type="number" name="confirmationNumber" className="form-control" placeholder="Enter Confirmation No." />
        </div>
      </div>
    </div>
    {props.props.props.fetchReportSuccess && props.props.props.apiResponseData && props.props.props.apiResponseData.length == 0 && <div className="text-center text-danger">Details not found</div>}
    <Footer
      saveText={'Submit'}
      isDisabled={!firstName
        && !lastName
        && !email
        && !phoneNumber
        && !confirmationId}
      isSaveRequest={false}
      isHideCancel={true}
      onClickCancel={() => { this.props.onClickCancel() }}
      onSubmit={() => { this.form.dispatchEvent(new Event('submit')) }} />
  </form>
}