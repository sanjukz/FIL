import axios from "axios";
import { RegistrationResponseViewModel } from "shared/models/RegistrationResponseViewModel";
import { ResetPasswordResponseViewModel } from "shared/models/ResetPasswordResponseViewModel";
import { ForgotPasswordResponseViewModel } from "shared/models/ForgotPasswordResponseViewModel";
import { SendAndValidateOTPResponseModel } from "shared/models/SendAndValidateOTPResponseModel";
import { LoginWithOTPResponseModel } from "shared/models/LoginWithOTPResponseModel";

function register(user) {
    return axios.post("api/account/register", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<RegistrationResponseViewModel>)
        .catch((error) => { return; });
}

function resetPassword(user) {
    return axios.post("api/account/resetpassword", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ResetPasswordResponseViewModel>)
        .catch((error) => { return; });
}

function forgotPassword(user) {
    return axios.post("api/account/forgotpassword", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ForgotPasswordResponseViewModel>)
        .catch((error) => {
            //todo: Error logging, sentry
            alert(error);
        });
}
function requestAndValidateOTP(user) {
    return axios.post("api/otp/send-and-validate", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<SendAndValidateOTPResponseModel>)
        .catch((error) => {
            //todo: Error logging, sentry
        });
}
function loginWithOtp(user) {
    return axios.post("api/otp/login", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<LoginWithOTPResponseModel>)
        .catch((error) => {
            //todo: Error logging, sentry
        });
}


export const userService = {
    register,
    resetPassword,
    forgotPassword,
    requestAndValidateOTP,
    loginWithOtp
};
