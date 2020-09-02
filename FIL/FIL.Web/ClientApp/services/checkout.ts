import axios from "axios";
import { GuestCheckoutResponseViewModel } from "../models/GuestCheckoutResponseViewModel";
import { UserCheckoutResponseViewModel } from "../models/UserCheckoutResponseViewModel";

function guestRegister(user) {
    return axios.post("api/guestcheckout/transaction", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<GuestCheckoutResponseViewModel>)
        .catch((error) => {            
            alert(error);
        });
}

function checkoutLoginToDeliveyOption(user) {
    return axios.post("api/loginUserToDeliveryOption/transaction", user, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<UserCheckoutResponseViewModel>)
        .catch((error) => {
            alert(error);
        });
}

function login(user) {
    return axios.post("api/loginuser/transaction", user, {
		headers: {
			"Content-Type": "application/json"
		}
	}).then((response) => response.data as Promise<UserCheckoutResponseViewModel>)
		.catch((error) => {			
			alert(error);
		});
}

export const checkoutservice = {
	guestRegister,
    login,
    checkoutLoginToDeliveyOption
};
