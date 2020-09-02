import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import TicketLookupResponseViewModel from "../models/TicketLookup/TicketLookupResponseViewModel"
import { TicketLookupDataViewModel } from "../models/TicketLookup/TicketLookupDataViewModel";
import { TicketLookupEmailDetailResponseViewModel } from "../models/TicketLookup/TicketLookupEmailDetailResponseViewModel";
export interface ITicketLookupState {
    isLoading?: boolean;
    fetchTicketLookupSuccess: boolean;
    errors?: any;
    ticketLookup: TicketLookupResponseViewModel;
    fetchTicketLookupEmailDetailSuccess: boolean;
    ticketLookupEmailDetails: ITicketLookupEmailDetailData;
}
export interface IRequestCategoryData {
    type: "REQUEST_TICKET_LOOKUP_DATA";
}
export interface IReceiveCategoryData {
    type: "RECEIVE_TICKET_LOOKUP_DATA"
    ticketLookup: TicketLookupResponseViewModel;
}

export interface ITicketLookupEmailDetailData {
    ticketLookupEmailDetailContainer: TicketLookupEmailDetailResponseViewModel[];
}

export interface IRequestEmailCategoryData {
    type: "REQUEST_TICKET_LOOKUP_EMAIL_DETAIL_DATA";
}

export interface IReceiveEmailCategoryData {
    type: "RECEIVE_TICKET_LOOKUP_EMAIL_DETAIL_DATA";
    ticketLookupEmailDetails: ITicketLookupEmailDetailData;
}
type KnownAction = IRequestCategoryData | IReceiveCategoryData | IRequestEmailCategoryData | IReceiveEmailCategoryData;

export const actionCreators = {
    requestTicketLookupTransactionData: (transactionId: number): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/ticketlookup/${transactionId}`)
            .then((response) => response.json() as Promise<TicketLookupResponseViewModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TICKET_LOOKUP_DATA", ticketLookup: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_TICKET_LOOKUP_DATA" });

    },
    requestTicketLookupData: (emailId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/ticketlookupemaildetail/${emailId}`)
            .then((response) => response.json() as Promise<ITicketLookupEmailDetailData>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TICKET_LOOKUP_EMAIL_DETAIL_DATA", ticketLookupEmailDetails: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_TICKET_LOOKUP_EMAIL_DETAIL_DATA" });

    },
    requestTicketLookupDataByName: (name: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/ticketlookupnamedetail/${name}`)
            .then((response) => response.json() as Promise<ITicketLookupEmailDetailData>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TICKET_LOOKUP_EMAIL_DETAIL_DATA", ticketLookupEmailDetails: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_TICKET_LOOKUP_EMAIL_DETAIL_DATA" });
    },
    requestTicketLookupDataByPhone: (phone: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/ticketlookupphonedetail/${phone}`)
            .then((response) => response.json() as Promise<ITicketLookupEmailDetailData>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TICKET_LOOKUP_EMAIL_DETAIL_DATA", ticketLookupEmailDetails: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_TICKET_LOOKUP_EMAIL_DETAIL_DATA" });
    }
};

  
const ticketLookupData: TicketLookupResponseViewModel = {
    currencyType: undefined,
    transaction: undefined,
    paymentOption: undefined,
    ticketLookupSubContainer: undefined,
    payconfigNumber: undefined
};
const ticketLookupEmailDetailData: ITicketLookupEmailDetailData = {
    ticketLookupEmailDetailContainer: undefined
};
const unloadedState: ITicketLookupState = {
    ticketLookup: ticketLookupData, isLoading: false, fetchTicketLookupSuccess: false, ticketLookupEmailDetails: ticketLookupEmailDetailData, fetchTicketLookupEmailDetailSuccess: false, errors:null
}
export const reducer: Reducer<ITicketLookupState> = (state: ITicketLookupState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_TICKET_LOOKUP_DATA":
            return {
                ticketLookup: state.ticketLookup,
                ticketLookupEmailDetails: state.ticketLookupEmailDetails,
                isLoading: true,
                fetchTicketLookupSuccess: false,
                fetchTicketLookupEmailDetailSuccess: false
            };
        case "RECEIVE_TICKET_LOOKUP_DATA":
            return {
                ticketLookup: action.ticketLookup,
                ticketLookupEmailDetails: state.ticketLookupEmailDetails,
                isLoading: false,
                fetchTicketLookupSuccess: true,
                fetchTicketLookupEmailDetailSuccess: false
            };
        case "REQUEST_TICKET_LOOKUP_EMAIL_DETAIL_DATA":
            return {
                ticketLookupEmailDetails: state.ticketLookupEmailDetails,
                ticketLookup: state.ticketLookup,
                fetchTicketLookupSuccess: false,
                isLoading: true,
                fetchTicketLookupEmailDetailSuccess: false
            };
        case "RECEIVE_TICKET_LOOKUP_EMAIL_DETAIL_DATA":
            return {
                ticketLookupEmailDetails: action.ticketLookupEmailDetails,
                ticketLookup: state.ticketLookup,
                fetchTicketLookupSuccess: false,
                isLoading: false,
                fetchTicketLookupEmailDetailSuccess: true
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};