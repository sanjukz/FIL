import axios from "axios";
import { TransactionLocatorResponseModel } from "../models/TransactionLocator/TransactionLocatorResponseModel";
import { SubmitFulfilmentResponseModel } from "../models/TransactionLocator/SubmitFulfilmentResponseModel";


function getTransactionLocatorData(values) {
    return axios.post("api/boxoffice/gettransactioninfos", values, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<TransactionLocatorResponseModel>)
        .catch((error) => {
        });
}
function getFulFilmentData(values) {
    return axios.post("api/fulfillment/getfulfilmemtData", values, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<TransactionLocatorResponseModel>)
        .catch((error) => {
        });
}
function submitFulFilmentDetails(values) {
    return axios.post("api/fulfillment/submitFulFilmentDetails", values, {
        headers: {
            "Content-Type": "application/json"
        }
    }).then((response) => response.data as Promise<SubmitFulfilmentResponseModel>)
        .catch((error) => {
        });
}

export const TransactionLocatorService = {
    getTransactionLocatorData,
    getFulFilmentData,
    submitFulFilmentDetails
};
