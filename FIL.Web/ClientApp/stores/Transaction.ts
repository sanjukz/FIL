import { addTask, fetch } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import Transaction from "../models/Comman/TransactionViewModel";
import { IAppThunkAction } from "./";

export interface ITransactionComponentProps {
    transaction: ITransactionComponentState;
}

export interface ITransactionComponentState {
    isLoading?: boolean;
    errors?: any;
    fetchSuccess: boolean;
    transactionData: Transaction;
    requesting: boolean;
    registered: boolean;
}

interface IRequestTransactionData {
    type: "REQUEST_TRANSACTION_DATA";
}

interface IReceiveTransactionData {
    type: "RECEIVE_TRANSACTION_DATA";
    transactionData: Transaction;
}

type KnownAction = IRequestTransactionData | IReceiveTransactionData;

export const actionCreators = {
    requestTransactionData: (transactionAltId: string, callback: (Transaction) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/transaction/${transactionAltId}`)
            .then((response) => response.json() as Promise<Transaction>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TRANSACTION_DATA", transactionData: data, });
                callback(data);
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_TRANSACTION_DATA" });

    },
};

const transactionUnloadedData: Transaction = {
    altId: null,
    discountAmount: null,
    grossTicketAmount: null,
    netTicketAmount: null,
    convenienceCharges: null,
    countryName: null,
    createdUtc: null,
    deliveryCharges: null,
    emailId: null,
    firstName: null,
    id: null,
    lastName: null,
    phoneCode: null,
    phoneNumber: null,
    serviceCharge: null,
    totalTickets: null,
    transactionStatusId: null,
    events: [],
    transactions: [],
    donationAmount: 0
};

const unloadedState: ITransactionComponentState = {
    errors: null,
    fetchSuccess: false,
    transactionData: transactionUnloadedData,
    isLoading: false,
    requesting: false,
    registered: false,
};

export const reducer: Reducer<ITransactionComponentState> = (state: ITransactionComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_TRANSACTION_DATA":
            return {
                transactionData: transactionUnloadedData,
                fetchSuccess: false,
                isLoading: true,
                requesting: false,
                registered: false
            };
        case "RECEIVE_TRANSACTION_DATA":
            return {
                transactionData: action.transactionData,
                fetchSuccess: true,
                isLoading: false,
                requesting: false,
                registered: true
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
