import axios from "axios";
import { GoogleSignInResponseViewModel } from "../models/SocialSignIn/GoogleSignInResponseViewModel";
import { FacebookSignInResponseViewModel } from "../models/SocialSignIn/FacebookSignInResponseViewModel";

function googlelogin(user) {
    return axios.post("api/login/google", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<GoogleSignInResponseViewModel>)
        .catch((error) => {
            alert(error);
        });

}
function facebooklogin(user) {
    return axios.post("api/login/facebook", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<FacebookSignInResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

export const SocialLoginService = {
    googlelogin,
    facebooklogin
};
