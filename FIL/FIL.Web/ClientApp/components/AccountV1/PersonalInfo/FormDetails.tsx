import * as React from 'react';
import { DatePicker } from 'antd';
import * as moment from 'moment';
import { UserProfile } from '../../../models/Account/UserProfileResponseViewModel';
import { RouteComponentProps } from "react-router-dom";
import * as AccountStore from "../../../stores/Account";
import {
    actionCreators as sessionActionCreators,
    ISessionProps
} from "shared/stores/Session";
import UploadProfile from './UploadProfile';
import { Radio } from 'antd';
import OTPForm from './OTPForm';
import { IPersonalInfo } from '../../../models/Account/ValidationResponse';

const dateFormat = 'DD/MM/YYYY';

interface Iprops {
    user: UserProfile;
    gets3BaseUrl: string;
    phoneCodeOptions: [];
    onChangeValue: (e: any) => void;
    onSaveValue: (name: string) => void;
    getFormatedDate: (user: UserProfile) => void;
    hideOtp: () => void;
    isMobileValid: boolean;
    showOtp: boolean;
    validateResponse: IPersonalInfo;
}
type AccountProps = Iprops & AccountStore.IUserAccountProps &
    ISessionProps &
    typeof sessionActionCreators &
    typeof AccountStore.actionCreators &
    RouteComponentProps<{}>;

function FormDetails(props: AccountProps) {
    const { user, account, phoneCodeOptions, isMobileValid, showOtp, validateResponse } = props;
    return <>

        {showOtp && <OTPForm  {...props} />}

        <div className="row">
            <div className="col-sm-8">
                <section className="pb-5">
                    <UploadProfile {...props} />
                </section>
                <section>
                    <div>
                        <div className="font-weight-bold pb-3">Legal name</div>
                        {`${account.userProfile.userProfile.firstName} ${account.userProfile.userProfile.lastName}`}
                        <a
                            className="btn btn-outline-primary btn-sm float-right"
                            data-toggle="collapse"
                            href="#fullName"
                            role="button"
                            aria-expanded="false"
                            aria-controls="fullName"
                        >
                            Edit</a>
                        <div className="collapse pt-3" id="fullName">
                            <p>
                                This is the name on your travel document, which could be a
                                licence or a passport.
                  </p>
                            <div className="form-row py-4">
                                <div className="col-sm-6">
                                    <label className="font-weight-bold">First name</label>
                                    <input
                                        type="text"
                                        className={"form-control" + ((validateResponse && validateResponse.firstName.isInValid) ? " is-invalid" : "")}
                                        placeholder="First name"
                                        name="firstName"
                                        value={user.firstName}
                                        onChange={(e) => props.onChangeValue(e)}
                                    />
                                    {validateResponse && validateResponse.firstName.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.firstName.message}</div>}
                                </div>
                                <div className="col-sm-6">
                                    <label className="font-weight-bold">Last name</label>
                                    <input
                                        type="text"
                                        className={"form-control" + ((validateResponse && validateResponse.lastName.isInValid) ? " is-invalid" : "")}
                                        placeholder="Last name"
                                        name="lastName"
                                        value={user.lastName}
                                        onChange={(e) => props.onChangeValue(e)}
                                    />
                                    {validateResponse && validateResponse.lastName.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.lastName.message}</div>}
                                </div>
                            </div>
                            <a
                                className="btn btn-primary"
                                data-toggle="collapse"
                                href="#fullName"
                                role="button"
                                aria-expanded="false"
                                aria-controls="fullName"
                                onClick={() => props.onSaveValue('fullName')}
                            >
                                Save</a>
                        </div>
                    </div>
                </section>
                <hr className="my-4" />
                <section>
                    <div>
                        <div className="font-weight-bold pb-3">Gender (optional)</div>
                        {account.userProfile.userProfile.gender ? account.userProfile.userProfile.gender : "Not Provided"}
                        <a
                            data-toggle="collapse"
                            href="#gender"
                            role="button"
                            aria-expanded="false"
                            aria-controls="gender"
                            className="btn btn-outline-primary btn-sm float-right">
                            {user.gender ? "Edit" : "Add"}</a>
                        <div className="collapse pt-3" id="gender">

                            <Radio.Group onChange={(e) => props.onChangeValue(e)} value={user.gender} name="gender">
                                <Radio value={"Male"}>Male</Radio>
                                <Radio value={"Female"}>Female</Radio>
                            </Radio.Group>
                            <div className="mt-2">
                                <a
                                    className="btn btn-primary"
                                    data-toggle="collapse"
                                    href="#gender"
                                    role="button"
                                    aria-expanded="false"
                                    aria-controls="gender"
                                    onClick={() => props.onSaveValue('gender')}
                                >
                                    Save</a>
                            </div>
                        </div>
                    </div>
                </section>
                <hr className="my-4" />
                <section>
                    <div>
                        <div className="font-weight-bold pb-3">Date of birth</div>
                        {account.userProfile.userProfile.dob != "" ? props.getFormatedDate(account.userProfile.userProfile) : "Not Provided"}
                        <a className="btn btn-outline-primary btn-sm float-right"
                            data-toggle="collapse"
                            href="#dob"
                            role="button"
                            aria-expanded="false"
                            aria-controls="dob">
                            {user.dob ? "Edit" : "Add"}</a>
                        <div className="collapse pt-3" id="dob">

                            <DatePicker
                                {...(user.dob != "" ? {
                                    value: moment(moment(user.dob).toDate(), dateFormat)
                                } : {})}
                                format={dateFormat}
                                disabledDate={(d => !d || d.isAfter(new Date().setDate(new Date().getDate() - 1)))}
                                name="dob"
                                onChange={(e) => props.onChangeValue(e)}
                                allowClear={false}
                            />

                            {validateResponse && validateResponse.dob.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.dob.message}</div>}

                            <div className="mt-4">
                                <a
                                    className="btn btn-primary"
                                    data-toggle="collapse"
                                    href="#dob"
                                    role="button"
                                    aria-expanded="false"
                                    aria-controls="dob"
                                    onClick={() => props.onSaveValue('dob')}
                                >
                                    Save</a>
                            </div>
                        </div>
                    </div>
                </section>
                <hr className="my-4" />
                <section>
                    <div>
                        <div className="font-weight-bold pb-3">Email address</div>
                        {user.email}
                        <a href="#" className="btn btn-outline-primary btn-sm float-right disabled">
                            Edit</a>
                    </div>
                </section>
                <hr className="my-4" />
                <section>
                    <div>
                        <div className="font-weight-bold pb-3">Mobile number</div>
                        {account.userProfile.userProfile.phoneNumber ? `+${account.userProfile.userProfile.phoneCode.split('~')[0]} - ${account.userProfile.userProfile.phoneNumber}` : "Not Provided"}
                        <a
                            data-toggle="collapse"
                            href="#mobile"
                            role="button"
                            aria-expanded="false"
                            aria-controls="mobile"
                            className="btn btn-outline-primary btn-sm float-right">
                            {user.phoneNumber ? "Edit" : "Add"}</a>
                        <div className={`collapse pt-3 ${!isMobileValid ? ' show' : ''}`} id="mobile">
                            <div className="form-row py-4">
                                <div className="col-sm-6">
                                    <label className="font-weight-bold">Country/Region</label>
                                    <select
                                        className={"form-control" + ((validateResponse && validateResponse.phoneCode.isInValid) ? " is-invalid" : "")}
                                        value={user.phoneCode} onChange={(e) => props.onChangeValue(e)}
                                        name="phoneCode">
                                        <option value="" disabled selected>
                                            Phone Code
                                        </option>
                                        {phoneCodeOptions}
                                    </select>
                                    {validateResponse && validateResponse.phoneCode.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.phoneCode.message}</div>}
                                </div>
                                <div className="col-sm-6">
                                    <label className="font-weight-bold">Phone Number</label>
                                    <input type="number"
                                        value={user.phoneNumber}
                                        className={"form-control" + ((validateResponse && validateResponse.phoneNumber.isInValid) ? " is-invalid" : "")}
                                        min="0"
                                        pattern="[0-9]*"
                                        onWheelCapture={e => { e.target.addEventListener("mousewheel", (evt) => { evt.preventDefault() }) }}
                                        name="phoneNumber" id="signup-phoneNumber"
                                        required
                                        onKeyPress={(e: any) => {
                                            e = e || window.event;
                                            var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
                                            var charStr = String.fromCharCode(charCode);
                                            if (!charStr.match(/^[0-9]+$/))
                                                e.preventDefault();
                                        }}
                                        onChange={(e) => props.onChangeValue(e)} />
                                    {validateResponse && validateResponse.phoneNumber.isInValid && <div className="d-block invalid-feedback">{validateResponse && validateResponse.phoneNumber.message}</div>}

                                </div>
                                {!isMobileValid && <div className="d-block invalid-feedback col-sm-12 mt-2">The above number is already registered with another account, please try again</div>}
                            </div>


                            <a
                                className="btn btn-primary"
                                data-toggle="collapse"
                                href="#mobile"
                                role="button"
                                aria-expanded="false"
                                aria-controls="mobile"
                                onClick={() => props.onSaveValue('mobile')}
                            >
                                Save</a>
                        </div>
                    </div>
                </section>
                <hr className="my-4" />
                {/* <section>
                    <div>
                        <div className="font-weight-bold pb-3">Government ID</div>
                Not provided
                <a href="#" className="btn btn-outline-primary btn-sm float-right disabled">
                            Add</a>
                    </div>
                </section>
                <hr className="my-4" />
                <section>
                    <div>
                        <div className="font-weight-bold pb-3">Address</div>
                Not provided
                <a href="#" className="btn btn-outline-primary btn-sm float-right disabled">
                            Add</a>
                    </div>
                </section>
                <hr className="my-4" />
                <section>
                    <div>
                        <div className="font-weight-bold pb-3">Emergency contact</div>
                Not provided
                <a href="#" className="btn btn-outline-primary btn-sm float-right disabled">
                            Add</a>
                    </div>
                </section> */}
            </div>
            <div className="col-sm-4 mt-4 mt-sm-0 pl-md-5">
                <div className="border p-4">
                    <img
                        src={`${props.gets3BaseUrl}/my-account/right-bar-images/personal-info.svg`}
                        className="img-fluid mb-4"
                        alt=""
                    />
                    <div>
                        <h5 className="mb-3">What info is shared with others?</h5>
                        <p className="m-0">
                            FeelitLIVE only releases contact information for hosts and
                            guests after a reservation is confirmed.
                </p>
                    </div>
                </div>
            </div>
        </div>
    </>
}

export default FormDetails;