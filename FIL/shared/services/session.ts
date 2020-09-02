import axios from "axios";
import { ForgotPasswordResponseViewModel } from "../models/ForgotPasswordResponseViewModel";
import { LoginResponseViewModel } from "../models/LoginResponseViewModel";
import { SessionViewModel } from "../models/SessionViewModel";

function login(user) {
    return axios.post("api/session/login", user, {
            headers: {
                "Content-Type": "application/json"
            }
        }).then((response) => response.data as Promise<LoginResponseViewModel>)
        .catch((error) => {
            //todo: Error logging, sentry
        });

}

function forgotPassword(user) {
    return axios.post("api/account/forgotpassword", user, {
            headers: {
                "Content-Type": "application/json"
            }
        }).then((response) => response.data as Promise<ForgotPasswordResponseViewModel>)
        .catch((error) => {
            //todo: Error logging, sentry
        });
}


function forgotPasswordRasv(user) {
    return axios.post("api/account/forgotpasswordRasv", user, {
            headers: {
                "Content-Type": "application/json"
            }
        }).then((response) => response.data as Promise<ForgotPasswordResponseViewModel>)
        .catch((error) => {
            //todo: Error logging, sentry
        });
}

function forgotPasswordICC(user) {
    return axios.post("api/account/forgotpasswordIcc", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ForgotPasswordResponseViewModel>)
        .catch((error) => {
            //todo: Error logging, sentry
        });
}

function get() {
    return axios.get("api/session", {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<SessionViewModel>);
}

function logout() {
    return axios.get("api/session/logout", {
        headers: {
            "Content-Type": "application/json"
        },
        maxRedirects: 1
    });
}

export const sessionService = {
    get,
    login,
    forgotPassword,
    forgotPasswordRasv,
    forgotPasswordICC,
    logout,
};
