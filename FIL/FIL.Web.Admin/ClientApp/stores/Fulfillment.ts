import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { TransactionLocatorService } from "../services/TransactionLocator";
import { TransactionLocatorFormData } from "../models/TransactionLocator/TransactionLocatorFormData";
import { TransactionLocatorResponseModel } from "../models/TransactionLocator/TransactionLocatorResponseModel";
import { GenerateOTPModel } from "../models/TransactionLocator/GenerateOTPModel";
import { SubmitFulfilmentResponseModel } from "../models/TransactionLocator/SubmitFulfilmentResponseModel";
import { SubmitFulFilmentFormDetails } from "../models/TransactionLocator/SubmitFulFilmentFormDetails";
export interface IFulfillmentProps {
    transactionLocator: IFulfillmentComponentState;
}
export interface IFulfillmentComponentState {
    isLoading?: boolean;
    fetchFloatTransactionDataSuccess?: boolean;
    transactionData?: TransactionLocatorResponseModel;
    errors?: any;
    generateOTP?: GenerateOTPModel;
    fetchOTPStatus?: boolean;
    submitData?: SubmitFulfilmentResponseModel;
    fetchSubmitData?: boolean;
}
interface IRequestTransactionData {
    type: "REQUEST_TRANSACTION_DATA";
}

interface IRecieveTransactionData {
    type: "RECIEVE_TRANSACTION_DATA";
    transactionData: TransactionLocatorResponseModel;
}

interface IFailureTransactionData {
    type: "RECEIVE_TRANSACTION_DATA_FAILURE";
    errors: any;
}
interface IRequestOTP {
    type: "REQUEST_OTP",

}
interface IReceiveIRecieveOTP {
    type: "RECEIVE_OTP",
    generateOTP: GenerateOTPModel
}
interface ISaveFulFilmentDetailRequest {
    type: "SAVE_FULFILMENT_DATA_REQUEST";
}

interface ISaveFulFilmentDetailRecieve {
    type: "SAVE_FULFILMENT_DATA_RECIEVE";
    submitData: SubmitFulfilmentResponseModel;
}
type KnownAction = IRequestTransactionData | IRecieveTransactionData | IFailureTransactionData | IReceiveIRecieveOTP | IRequestOTP | ISaveFulFilmentDetailRequest | ISaveFulFilmentDetailRecieve

export const actionCreators = {

    getFulFilmentDetails: (values: TransactionLocatorFormData): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        if (values.barcodeNumber == "0" && values.confirmationNumber == 0 && values.emailid == "0" && values.firstName == "0" && values.lastName == "0" && values.userMobileNo == "0") {
            alert("Please fill the details of atleast any one");
            return false;
        }
        dispatch({ type: "REQUEST_TRANSACTION_DATA" });
        TransactionLocatorService.getFulFilmentData(values)
            .then(
                (response: TransactionLocatorResponseModel) => {
                    dispatch({ type: "RECIEVE_TRANSACTION_DATA", transactionData: response });
                },
                (error) => {
                    dispatch({ type: "RECEIVE_TRANSACTION_DATA_FAILURE", errors: error });
                },
            );
    },
    generatetheOTP: (phoneCode: string, phoneNumber: string, transDetailAltId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_OTP" });
        const fetchTask = fetch(`api/fulfillment/generate-otp/${phoneCode}/${phoneNumber}/${transDetailAltId}`)
            .then((response) => response.json() as Promise<GenerateOTPModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_OTP", generateOTP: data });
            });
        addTask(fetchTask);
    },
    SubmitFulFilmentDetails: (detailModel: SubmitFulFilmentFormDetails, callback: (SubmitFulfilmentResponseModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "SAVE_FULFILMENT_DATA_REQUEST" });
        TransactionLocatorService.submitFulFilmentDetails(detailModel)
            .then((response: SubmitFulfilmentResponseModel) => {
                dispatch({ type: "SAVE_FULFILMENT_DATA_RECIEVE", submitData: response });
                callback(response);
            },
                (error) => {
                    dispatch({ type: "RECEIVE_TRANSACTION_DATA_FAILURE", errors: error });
                });

    },
};
const emptyTransactionData: TransactionLocatorResponseModel = {
    transactionData: []
}
const emptyGenerateOTP: GenerateOTPModel = {
    success: false
}
const emptySubmitData: SubmitFulfilmentResponseModel = {
    isValid: false,
    message: ''
}
const unloadedState: IFulfillmentComponentState = {
    isLoading: false, fetchFloatTransactionDataSuccess: false, transactionData: emptyTransactionData, fetchOTPStatus: false, generateOTP: emptyGenerateOTP,
    fetchSubmitData: false, submitData: emptySubmitData
}

export const reducer: Reducer<IFulfillmentComponentState> = (state: IFulfillmentComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {

        case "REQUEST_TRANSACTION_DATA":
            return {
                transactionData: state.transactionData,
                fetchFloatTransactionDataSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECIEVE_TRANSACTION_DATA":
            return {
                transactionData: action.transactionData,
                fetchFloatTransactionDataSuccess: true,
                isLoading: false,
                errors: {}
            };
        case "RECEIVE_TRANSACTION_DATA_FAILURE":
            return {
                transactionData: state.transactionData,
                fetchFloatTransactionDataSuccess: true,
                isLoading: false,
                errors: action.errors
            };
        case "REQUEST_OTP":

            return {
                fetchOTPStatus: false,
                generateOTP: state.generateOTP,
                fetchFloatTransactionDataSuccess: state.fetchFloatTransactionDataSuccess,
                transactionData: state.transactionData
            };
        case "RECEIVE_OTP":

            return {
                fetchFloatTransactionDataSuccess: state.fetchFloatTransactionDataSuccess,
                transactionData: state.transactionData,
                fetchOTPStatus: true,
                generateOTP: action.generateOTP
            };
        case "SAVE_FULFILMENT_DATA_REQUEST":

            return {
                fetchFloatTransactionDataSuccess: state.fetchFloatTransactionDataSuccess,
                transactionData: state.transactionData,
                isLoading: true,
                fetchSubmitData: false,
                submitData: state.submitData
            };
        case "SAVE_FULFILMENT_DATA_RECIEVE":

            return {
                fetchFloatTransactionDataSuccess: state.fetchFloatTransactionDataSuccess,
                transactionData: state.transactionData,
                isLoading: false,
                fetchSubmitData: true,
                submitData: action.submitData
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};