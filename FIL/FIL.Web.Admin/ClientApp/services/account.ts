import axios from "axios";
import { ChangePasswordResponseViewModel } from "../models/Account/ChangePasswordResponseViewModel";
import { ChangeProfilePicResponseViewModel } from "../models/Account/ChangeProfilePicResponseViewModel";

function changePassword(user) {
    return axios.post("api/account/changepassword", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ChangePasswordResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

function changeProfilePic(user) {
    return axios.post("api/account/changeprofilepic", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<ChangeProfilePicResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

export const accountService = {
    changePassword,
    changeProfilePic
};