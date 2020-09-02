import * as React from 'react';
import { Modal, message } from 'antd';
import 'antd/dist/antd.css';
import OtpInput from 'react-otp-input';
import { SendAndValidateOTPFormModel } from "shared/models/SendAndValidateOTPFormModel";
import Countdown from "react-countdown-now";
import { connect } from "react-redux";
import { IApplicationState } from "../../../stores";
import * as AccountStore from "../../../stores/Account";
import * as SignUpStore from "../../../stores/SignUp";
import { bindActionCreators } from "redux";
import { SendAndValidateOTPResponseModel } from 'shared/models/SendAndValidateOTPResponseModel';
import KzLoader from "../../../components/Loader/KzLoader";
import UserProfileResponseViewModel, { UserProfile } from '../../../models/Account/UserProfileResponseViewModel';

interface IProps {
    user: UserProfile
    hideOtp: () => void;
}
type OtpFormProps = IProps & AccountStore.IUserAccountProps &
    SignUpStore.ISignUpProps &
    typeof SignUpStore.actionCreators &
    typeof AccountStore.actionCreators;

class OTPForm extends React.PureComponent<OtpFormProps, any>{
    constructor(props) {
        super(props);
        this.state = {
            userModel: {
                phoneCode: '',
                phoneNumber: '',
                sendOTP: true,
                token: '',
                otp: null
            },
            isLoading: true,
            hasErrored: false,
            otpCountDown: 0,
            CountdownCompleted: false
        }
    }
    componentDidMount = () => {
        this.requestOtp();
    }

    requestOtp = () => {
        const { user } = this.props;
        let userModel: SendAndValidateOTPFormModel = {
            phoneCode: user.phoneCode,
            phoneNumber: user.phoneNumber,
            sendOTP: true
        }
        this.props.requestAndValidateOTP(userModel, (response: SendAndValidateOTPResponseModel) => {
            console.log(response);
            if (response.success && response.isOTPSent) {
                let userModel = { ...this.state };
                userModel.token = response.token
                this.setState({
                    isLoading: false,
                    userModel: userModel,
                    otpCountDown: Date.now() + (new Date().getTime() + 30000 - new Date().getTime()),
                    CountdownCompleted: false
                })
            } else {
                window.location.reload();
            }
        });
    }
    handleOTPChange = (otp) => {
        if (otp.toString().length == 6) {
            const { user } = this.props;
            const { userModel } = this.state;
            let userData: SendAndValidateOTPFormModel = {
                phoneCode: user.phoneCode,
                phoneNumber: user.phoneNumber,
                sendOTP: false,
                otp: otp,
                token: userModel.token
            }
            this.props.requestAndValidateOTP(userData, (response: SendAndValidateOTPResponseModel) => {
                console.log(response);
                if (response.success && response.isOtpValid) {
                    this.setState({ hasErrored: false })
                    this.updateDetails();
                } else {
                    this.setState({ hasErrored: true })
                }
            });
        }
        if (otp.toString().length <= 6) {
            let user = Object.assign({}, this.state.userModel);
            user["otp"] = otp;
            this.setState({ userModel: user, hasErrored: false });
        }

    }

    updateDetails = () => {
        let user: UserProfileResponseViewModel = {
            userProfile: this.props.user
        }
        this.props.updateUserDetails(user, (response: UserProfileResponseViewModel) => {
            if (response) {
                this.showSuccessAlert();
                this.props.hideOtp()
            }
        });
    }

    showSuccessAlert = () => {
        message.success({
            content: 'Phone number updated successfully',
            duration: 5,
            className: 'mt-5'
        })

    }

    showCountDown = ({ minutes, seconds, completed }) => {
        if (completed) {
            this.setState({ CountdownCompleted: true })
            return null;
        } else {
            return <span>{minutes}:{seconds}</span>;
        }
    }
    render() {
        const { user } = this.props;
        const { userModel } = this.state;
        if (this.state.isLoading) {
            return <KzLoader />
        }
        return <>
            < Modal
                title={"Please confirm your mobile number"}
                centered
                footer={null}
                wrapClassName="otp-form"
                visible={true}
                maskClosable={false}
                onCancel={() => this.props.hideOtp()}
            >
                <p>We've sent you a code to confirm your number</p>

                <p>Enter the code just sent to +{user.phoneCode.split("~")[0]} {user.phoneNumber}: </p>
                <OtpInput
                    onChange={otp => { this.handleOTPChange(otp) }}
                    numInputs={6}
                    separator={<span>-</span>}
                    value={userModel.otp}
                    inputStyle="inputStyle"
                    errorStyle="inputStyle error"
                    hasErrored={this.state.hasErrored}
                />
                {this.state.hasErrored && <p className="text-danger mt-2">Incorrect code</p>}

                {this.state.otpCountDown != 0 &&
                    <>
                        {!this.state.CountdownCompleted ? <p className="mt-2">Expect OTP in {<Countdown
                            date={this.state.otpCountDown}
                            renderer={(e) => this.showCountDown(e)}
                        />}</p>
                            : <>
                                <p className="mt-3">Didn’t get a text? <a href="javascript:void(0)"
                                    onClick={(e) => { this.requestOtp() }} className="btn-link ml-2">Send again
                        </a >
                                </p>

                            </>
                        }
                    </>
                }
            </Modal >
        </>
    }
}


export default connect(
    (state: IApplicationState, ownProps) => ({
        account: state.account,
        signup: state.signUp,
        ...ownProps
    }),
    dispatch =>
        bindActionCreators(
            { ...AccountStore.actionCreators, ...SignUpStore.actionCreators },
            dispatch
        )
)(OTPForm);