import axios from "axios";
import { UpdateTransactionResponseViewModel } from "../models/UpdateTransactionResponseViewModel";

function saveDeliveryDetails(user) {
	return axios.post("api/deliveryoptions", user, {
        headers: {
            "Content-Type": "application/json"
        }
	}).then((response) => response.data as Promise<UpdateTransactionResponseViewModel>)
        .catch((error) => {            
            alert(error);
        });
}

export const deliveryservice = {
	saveDeliveryDetails,	
};
