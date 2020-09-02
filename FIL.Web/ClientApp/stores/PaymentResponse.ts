import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { PaymentFormDataViewModel } from "../models/Payment/PaymentFormDataViewModel";
import { PaymentFormResponseViewModel } from "../models/Payment/PaymentFormResponseViewModel";
import { PaymentResponseFormDataViewModel } from "../models/Payment/PaymentResponseFormDataViewModel";
import { PaymentResponseViewModel } from "../models/Payment/PaymentResponseViewModel";
import { sessionService } from "shared/services/session";
import { paymentService } from "../services/payment";
import {
	actionCreators as sessionActionCreators,
	IGetSessionReceivedAction,
	IGetSessionRequestAction
} from "shared/stores/Session";
import { IAppThunkAction } from "./";

export const PAYMENT_RESPONSE_REQUEST = "USERS_PAYMENT_RESPONSE_REQUEST";
export const PAYMENT_RESPONSE_SUCCESS = "USERS_PAYMENT_RESPONSE_SUCCESS";
export const PAYMENT_RESPONSE_FAILURE = "USERS_PAYMENT_RESPONSE_FAILURE";

export interface IPaymentResponseState {	
	requesting?: boolean;			
	paymentDetails?: any;
	errors?: any;
}

const emptyPaymentResponseState: PaymentResponseViewModel = {
	success: false,
	errorMessage: "",
}

const DefaultPaymentResponseState: IPaymentResponseState = {	
	requesting: false,			
	paymentDetails: emptyPaymentResponseState,
	errors: {},
};

interface IPaymentResponseRequestAction {
	type: "USERS_PAYMENT_RESPONSE_REQUEST";
}

interface IPaymentResponseSuccesstAction {
	type: "USERS_PAYMENT_RESPONSE_SUCCESS";
	paymentDetails: PaymentResponseViewModel;
}

interface IPaymentResponseFailureAction {
	type: "USERS_PAYMENT_RESPONSE_FAILURE";
	errors: {};
}

type KnownAction = IPaymentResponseRequestAction | IPaymentResponseSuccesstAction | IPaymentResponseFailureAction;

export const actionCreators = {			
	sendPaymentResponse: (paymentModel: PaymentResponseFormDataViewModel, callback: (PaymentResponseViewModel) => void)
		: IAppThunkAction<KnownAction> => async (dispatch, getState) => {
			dispatch({ type: "USERS_PAYMENT_RESPONSE_REQUEST" });
			paymentService.processResponse(paymentModel)
				.then((response: PaymentFormResponseViewModel) => {					
					dispatch({ type: "USERS_PAYMENT_RESPONSE_SUCCESS", paymentDetails: response });
					callback(response);
				},
				(error) => {
					dispatch({ type: "USERS_PAYMENT_RESPONSE_FAILURE", errors:error });
				});
		},
};

export const reducer: Reducer<IPaymentResponseState> = (state: IPaymentResponseState, action: KnownAction) => {
	switch (action.type) {				
		case "USERS_PAYMENT_RESPONSE_REQUEST":
			return {				
				requesting: true,				
				errors: {},
			};
		case "USERS_PAYMENT_RESPONSE_SUCCESS":
			return {				
				requesting: false,
				paymentDetails: action.paymentDetails,
				errors: {},
			};
		case "USERS_PAYMENT_RESPONSE_FAILURE":
			return {
				requesting: false,				
				errors: action.errors,
			};
		default:
			const exhaustiveCheck: never = action;
	}
	return state || DefaultPaymentResponseState;
};
