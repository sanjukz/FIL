import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { PaymentErrorResponseViewModel } from "../models/Payment/PaymentErrorResponseViewModel";
import { IAppThunkAction } from "./";

export const PAYMENT_ERROR_REQUEST = "USERS_PAYMENT_ERROR_DESCRIPTION_REQUEST";
export const PAYMENT_ERROR_SUCCESS = "USERS_PAYMENT_ERROR_DESCRIPTION_SUCCESS";
export const PAYMENT_ERROR_FAILURE = "USERS_PAYMENT_ERROR_DESCRIPTION_FAILURE";

export interface IPaymentErrorResponseState {    
    requesting: boolean    
    fetchPaymentErrorSuccess: boolean;    
    paymentErrorDescription: PaymentErrorResponseViewModel;
    errors?: any;
}

const defaultError: PaymentErrorResponseViewModel = {
    errorDescription: "",
};

const DefaultPaymentErrorResponseState: IPaymentErrorResponseState = {        
    requesting: false,
    fetchPaymentErrorSuccess: false,
    paymentErrorDescription: defaultError,
    errors: {},
};

interface IRequestPaymentErrorAction {
    type: "USERS_PAYMENT_ERROR_DESCRIPTION_REQUEST";
}

interface IReceivePaymentErrorAction {
    type: "USERS_PAYMENT_ERROR_DESCRIPTION_SUCCESS";
    paymentErrorDescription: PaymentErrorResponseViewModel;
}

interface IPaymentErrorFailureAction {
    type: "USERS_PAYMENT_ERROR_DESCRIPTION_FAILURE";
    errors: any;
}

type KnownAction = IRequestPaymentErrorAction | IReceivePaymentErrorAction | IPaymentErrorFailureAction;

export const actionCreators = {   
    getPaymentErrorDescription: (err: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/payment/error/${err}`)
            .then((response) => response.json() as Promise<PaymentErrorResponseViewModel>)
            .then((data) => {
                dispatch({ type: "USERS_PAYMENT_ERROR_DESCRIPTION_SUCCESS", paymentErrorDescription: data });
            },
            (error) => {
                dispatch({ type: "USERS_PAYMENT_ERROR_DESCRIPTION_FAILURE", errors: error });
            },
        );
        addTask(fetchTask);
        dispatch({ type: "USERS_PAYMENT_ERROR_DESCRIPTION_REQUEST" });
    },
};

export const reducer: Reducer<IPaymentErrorResponseState> = (state: IPaymentErrorResponseState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "USERS_PAYMENT_ERROR_DESCRIPTION_REQUEST":
            return {
                requesting: true,
                paymentErrorDescription: state.paymentErrorDescription,
                fetchPaymentErrorSuccess: false,
                errors: {},
            };
        case "USERS_PAYMENT_ERROR_DESCRIPTION_SUCCESS":
            return {
                requesting: false,
                paymentErrorDescription: action.paymentErrorDescription,
                fetchPaymentErrorSuccess: false,
                errors: {},
            };
        case "USERS_PAYMENT_ERROR_DESCRIPTION_FAILURE":
            return {
                requesting: false,
                paymentErrorDescription: state.paymentErrorDescription,
                fetchPaymentErrorSuccess: false,
                errors: action.errors,
            };        
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultPaymentErrorResponseState;
};
