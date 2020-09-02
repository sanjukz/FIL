import axios from "axios";
import { GoogleSignInResponseViewModel } from "../models/SocialSignIn/GoogleSignInResponseViewModel";

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

export const GoogleSignInService = {
	googlelogin
};
