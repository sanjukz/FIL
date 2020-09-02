import axios from "axios";
import {FacebookSignInResponseViewModel}  from "../models/SocialSignIn/FacebookSignInResponseViewModel";

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

export const FacebookSignInService = {
	facebooklogin
};
