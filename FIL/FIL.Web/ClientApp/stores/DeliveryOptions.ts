import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import CountryDataViewModel from "../models/Country/CountryDataViewModel";
import { DeliveryOptionsFormDataViewModel, DeliveryDetail } from "../models/DeliveryOptions/DeliveryDetailFormDataViewModel";
import { UserDeliveryDetailFormDataViewModel } from "../models/DeliveryOptions/UserDeliveryDetailFormDataViewModel";
import { UpdateTransactionResponseViewModel } from "../models/UpdateTransactionResponseViewModel";
import { sessionService } from "shared/services/session";
import {
	actionCreators as sessionActionCreators,
	IGetSessionReceivedAction,
	IGetSessionRequestAction
} from "shared/stores/Session";
import { IAppThunkAction } from "./";
import { deliveryservice } from "../services/deliveryOptions";

export const GET_COUNTRY_LIST_REQUEST = "CHECKOUT_GET_COUNTRY_LIST_REQUEST";
export const GET_COUNTRY_LIST_SUCCESS = "CHECKOUT_GET_COUNTRY_LIST_SUCCESS";
export const GET_COUNTRY_LIST_FAILURE = "CHECKOUT_GET_COUNTRY_LIST_FAILURE";

export const SAVE_DELIVERY_OPTIONS_REQUEST = "USERS_SAVE_DELIVERY_OPTIONS_REQUEST";
export const SAVE_DELIVERY_OPTIONS_SUCCESS = "USERS_SAVE_DELIVERY_OPTIONS_SUCCESS";
export const SAVE_DELIVERY_OPTIONS_FAILURE = "USERS_SAVE_DELIVERY_OPTIONS_FAILURE";

export interface IDeliveryOptionsProps {
    DeliveryOptions: IDeliveryOptionsState;
}


export interface IDeliveryOptionsState {
	currentPage?: string;
	isLoading?: boolean;
	requesting?: boolean;
	fetchTransactionSuccess?: boolean;	
	fetchCountriesSuccess?: boolean;
	countryList?: CountryDataViewModel;
	errors?: any;
}

const emptyCountries: CountryDataViewModel = {
	countries: [],
};

interface IRequestTransactionData {
	type: "REQUEST_TICKET_TRANSACTION_DATA";
}

interface IReceiveTransactionData {
	type: "RECEIVE_TICKET_TRANSACTION_DATA";
}

interface IRequestGetCountryListAction {
	type: "CHECKOUT_GET_COUNTRY_LIST_REQUEST";
}

interface IReceiveGetCountryListAction {
	type: "CHECKOUT_GET_COUNTRY_LIST_SUCCESS";
	countryList: CountryDataViewModel;
}

interface IGetCountryListFailureAction {
	type: "CHECKOUT_GET_COUNTRY_LIST_FAILURE";
	errors: any;
}

interface ISaveDeliveryOptionsRequestAction {
	type: "USERS_SAVE_DELIVERY_OPTIONS_REQUEST";
}

interface ISaveDeliveryOptionsSuccesstAction {
	type: "USERS_SAVE_DELIVERY_OPTIONS_SUCCESS";
	transactionId: number;
}

interface ISaveDeliveryOptionsFailureAction {
	type: "USERS_SAVE_DELIVERY_OPTIONS_FAILURE";
}

type KnownAction = IRequestTransactionData | IReceiveTransactionData |
	IRequestGetCountryListAction | IReceiveGetCountryListAction | IGetCountryListFailureAction | 
	ISaveDeliveryOptionsRequestAction | ISaveDeliveryOptionsSuccesstAction | ISaveDeliveryOptionsFailureAction;

export const actionCreators = {
	getTransaction: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		dispatch({ type: "RECEIVE_TICKET_TRANSACTION_DATA" });
		dispatch({ type: "REQUEST_TICKET_TRANSACTION_DATA" });
	}, 
	requestCountryData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/country/all`)
			.then((response) => response.json() as Promise<CountryDataViewModel>)
			.then((data) => {
				dispatch({ type: "CHECKOUT_GET_COUNTRY_LIST_SUCCESS", countryList: data, });
			},
			(error) => {
				dispatch({ type: "CHECKOUT_GET_COUNTRY_LIST_FAILURE", errors: error });
			},
		);
		addTask(fetchTask);
		dispatch({ type: "CHECKOUT_GET_COUNTRY_LIST_REQUEST" });
	},
	saveDeliveryOptions: (detailModel: DeliveryOptionsFormDataViewModel, callback: (GuestCheckoutResponseViewModel) => void)
		: IAppThunkAction<KnownAction> => async (dispatch, getState) => {
			dispatch({ type: "USERS_SAVE_DELIVERY_OPTIONS_REQUEST" });
			deliveryservice.saveDeliveryDetails(detailModel)
				.then((response: UpdateTransactionResponseViewModel) => {
					dispatch({ type: "USERS_SAVE_DELIVERY_OPTIONS_SUCCESS", transactionId: response.transactionId });
					callback(response);
				},
				(error) => {
					dispatch({ type: "USERS_SAVE_DELIVERY_OPTIONS_FAILURE" });
				});
		},
};

const unloadedState: IDeliveryOptionsState = {
	fetchTransactionSuccess: false,
	fetchCountriesSuccess: false,
	countryList: emptyCountries,
};

export const reducer: Reducer<IDeliveryOptionsState> = (state: IDeliveryOptionsState, incomingAction: Action) => {
	const action = incomingAction as KnownAction;
	switch (action.type) {
		case "REQUEST_TICKET_TRANSACTION_DATA":
			return {
				isLoading: true,
				fetchTransactionSuccess: false
			};
		case "RECEIVE_TICKET_TRANSACTION_DATA":
			return {
				isLoading: false,
				fetchTransactionSuccess: true
			};
		case "CHECKOUT_GET_COUNTRY_LIST_REQUEST":
			return {
				countryList: state.countryList,
				requesting: true,
				fetchCountriesSuccess: false,
				errors: {},
			};
		case "CHECKOUT_GET_COUNTRY_LIST_SUCCESS":
			return {
				countryList: action.countryList,
				requesting: false,
				fetchCountriesSuccess: true,
				errors: {},
			};
		case "CHECKOUT_GET_COUNTRY_LIST_FAILURE":
			return {
				requesting: false,
				fetchCountriesSuccess: false,
				errors: action.errors,
			};
		case "USERS_SAVE_DELIVERY_OPTIONS_REQUEST":
			return {
				countryList: state.countryList,
				requesting: true,
				fetchCountriesSuccess: false,
				errors: {},
			};
		case "USERS_SAVE_DELIVERY_OPTIONS_SUCCESS":
			return {
				countryList: state.countryList,
				requesting: false,
				fetchCountriesSuccess: true,
				errors: {},
			};
		case "USERS_SAVE_DELIVERY_OPTIONS_FAILURE":
			return {
				requesting: false,
				fetchCountriesSuccess: false,
				errors: {},
			};
		default:
			const exhaustiveCheck: never = action;
	}
	return state || unloadedState;
};
