import axios from "axios";
import { OrderConfirmationNewResponseViewModel as OrderConfirmationResponseViewModel } from "../models/OrderConfirmationNewResponseViewModel";

function getOrderConfirmation(order) {
    return axios.post("api/orderconfirmations", order, {
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then((response) => response.data as Promise<OrderConfirmationResponseViewModel>)
        .catch((error) => {
        });
}

export const orderConfirmationService = {
    getOrderConfirmation
};
