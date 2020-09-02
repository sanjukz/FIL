import axios from "axios";
import { PaymentFormResponseViewModel } from "../models/Payment/PaymentFormResponseViewModel";
import { PaymentResponseViewModel } from "../models/Payment/PaymentResponseViewModel";
import { PromocodeResponseModel } from "../models/Payment/PromocodeResponseModel";
function payWithCard(card) {
	return axios.post("api/payment", card, {
		headers: {
			"Content-Type": "application/json"
		}
	}).then((response) => response.data as Promise<PaymentFormResponseViewModel>)
		.catch((error) => {
			alert(error);
		});
}

function processResponse(card) {
	return axios.post("api/payment/response", card, {
		headers: {
			"Content-Type": "application/json"
		}
	}).then((response) => response.data as Promise<PaymentFormResponseViewModel>)
		.catch((error) => {
			alert(error);
		});
}
function savePromocode(promoCode) {
	return axios.post("api/applypromocode", promoCode, {
		headers: {
			"Content-Type": "application/json"
		}
	}).then((response) => response.data as Promise<PromocodeResponseModel>)
		.catch((error) => {
			alert(error);
		});
}

export const paymentService = {
	payWithCard,
	processResponse,
	savePromocode
};
