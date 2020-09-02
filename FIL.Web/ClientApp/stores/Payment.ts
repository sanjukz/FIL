import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import CountryDataViewModel from "../models/Country/CountryDataViewModel";
import { UpdateTransactionResponseViewModel } from "../models/UpdateTransactionResponseViewModel";
import { PaymentFormDataViewModel } from "../models/Payment/PaymentFormDataViewModel";
import { PaymentFormResponseViewModel } from "../models/Payment/PaymentFormResponseViewModel";
import PaymentOptionsResponseViewModel from "../models/Payment/PaymentOptionsResponseViewModel";
import { PromocodeResponseModel } from "../models/Payment/PromocodeResponseModel";
import { PromoCodeFormViewModel } from "../models/Payment/PromoCodeFormViewModel";
import { sessionService } from "shared/services/session";
import { paymentService } from "../services/payment";
import {
    actionCreators as sessionActionCreators,
    IGetSessionReceivedAction,
    IGetSessionRequestAction
} from "shared/stores/Session";
import { IAppThunkAction } from "./";

export const GET_COUNTRY_LIST_REQUEST = "CHECKOUT_GET_COUNTRY_LIST_REQUEST";
export const GET_COUNTRY_LIST_SUCCESS = "CHECKOUT_GET_COUNTRY_LIST_SUCCESS";
export const GET_COUNTRY_LIST_FAILURE = "CHECKOUT_GET_COUNTRY_LIST_FAILURE";

export const GET_PAYMENTOPTIONS_LIST_REQUEST = "CHECKOUT_GET_PAYMENTOPTIONS_LIST_REQUEST";
export const GET_PAYMENTOPTIONS_LIST_SUCCESS = "CHECKOUT_GET_PAYMENTOPTIONS_LIST_SUCCESS";
export const GET_PAYMENTOPTIONS_LIST_FAILURE = "CHECKOUT_GET_PAYMENTOPTIONS_LIST_FAILURE";


export const PAYMENT_REQUEST = "USERS_PAYMENT_REQUEST";
export const PAYMENT_SUCCESS = "USERS_PAYMENT_SUCCESS";
export const PAYMENT_FAILURE = "USERS_PAYMENT_FAILURE";

export interface IPaymentProps {
    Payment: IPaymentState;
}

export interface IPaymentState {
    cardTypeId?: number;
    cardNumber?: string;
    paymentOption?: string;
    requesting?: boolean;
    fetchCountriesSuccess?: boolean;
    fetchPaymentOptionsSuccess?: boolean;
    timer?: number;
    transactionDetail?: any;
    countryList?: CountryDataViewModel;
    paymentOptionList?: PaymentOptionsResponseViewModel;
    errors?: any;
    promocoderequest?: boolean;
    promocodesuccess?: boolean;
}

const emptyCountries: CountryDataViewModel = {
    countries: [],
};

const emptyPaymentOptions: PaymentOptionsResponseViewModel = {
    bankDetails: [],
    cashCardDetails: [],
};

const emptyTransactionDetail: UpdateTransactionResponseViewModel = {
    success: false,
    transactionId: null,
    currencyId: null,
    grossTicketAmount: null,
    deliveryCharges: null,
    convenienceCharges: null,
    serviceCharge: null,
    discountAmount: null,
    netTicketAmount: null,
}

const DefaultPaymentState: IPaymentState = {
    requesting: false,
    fetchCountriesSuccess: false,
    fetchPaymentOptionsSuccess: false,
    transactionDetail: emptyTransactionDetail,
    countryList: emptyCountries,
    paymentOptionList: emptyPaymentOptions,
    errors: {},
    promocodesuccess: false,
    promocoderequest: false
};

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

interface IRequestGetPaymentOptionsListAction {
    type: "CHECKOUT_GET_PAYMENTOPTIONS_LIST_REQUEST";
}

interface IReceiveGetPaymentOptionsListAction {
    type: "CHECKOUT_GET_PAYMENTOPTIONS_LIST_SUCCESS";
    paymentOptionList: PaymentOptionsResponseViewModel;
}

interface IGetPaymentOptionsListFailureAction {
    type: "CHECKOUT_GET_PAYMENTOPTIONS_LIST_FAILURE";
    errors: any;
}

interface IPaymentRequestAction {
    type: "USERS_PAYMENT_REQUEST";
}

interface IPaymentSuccesstAction {
    type: "USERS_PAYMENT_SUCCESS";
    paymentDetails: PaymentFormResponseViewModel;
}

interface IPaymentFailureAction {
    type: "USERS_PAYMENT_FAILURE";
    errors: {};
}

interface IPromocodeRequestAction {
    type: "PROMOCODE_REQUEST";
}

interface IPromocodeSuccesstAction {
    type: "PROMOCODE_SUCCESS";
    promocodesuccess: boolean;
}

interface IPromocodeFailureAction {
    type: "PROMOCODE_FAILURE";
    errors: {};
}

type KnownAction = IRequestGetCountryListAction | IReceiveGetCountryListAction | IGetCountryListFailureAction |
    IRequestGetPaymentOptionsListAction | IReceiveGetPaymentOptionsListAction | IGetPaymentOptionsListFailureAction |
    IPaymentRequestAction | IPaymentSuccesstAction | IPaymentFailureAction | IPromocodeRequestAction | IPromocodeSuccesstAction | IPromocodeFailureAction;

export const actionCreators = {
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
    requestPaymentOptionData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/paymentOptions`)
            .then((response) => response.json() as Promise<PaymentOptionsResponseViewModel>)
            .then((data) => {
                dispatch({ type: "CHECKOUT_GET_PAYMENTOPTIONS_LIST_SUCCESS", paymentOptionList: data, });
            },
                (error) => {
                    dispatch({ type: "CHECKOUT_GET_PAYMENTOPTIONS_LIST_FAILURE", errors: error });
                },
            );
        addTask(fetchTask);
        dispatch({ type: "CHECKOUT_GET_PAYMENTOPTIONS_LIST_REQUEST" });
    },

    proceedToPayment: (paymentModel: PaymentFormDataViewModel, callback: (PaymentFormResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "USERS_PAYMENT_REQUEST" });
            paymentService.payWithCard(paymentModel)
                .then((response: PaymentFormResponseViewModel) => {

                    dispatch({ type: "USERS_PAYMENT_SUCCESS", paymentDetails: response });
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "USERS_PAYMENT_FAILURE", errors: error });
                    });
        },
    savePromocode: (promocodeModel: PromoCodeFormViewModel, callback: (RasvPromocodeResponceViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "PROMOCODE_REQUEST" });
            paymentService.savePromocode(promocodeModel)
                .then((response: PromocodeResponseModel) => {
                    localStorage.setItem("transactionState", JSON.stringify(response));
                    // store("transactionState", response);
                    dispatch({ type: "PROMOCODE_SUCCESS", promocodesuccess: response.success });
                    callback(response);
                });
        },
};

export const reducer: Reducer<IPaymentState> = (state: IPaymentState, action: KnownAction) => {
    switch (action.type) {
        case "CHECKOUT_GET_COUNTRY_LIST_REQUEST":
            return {
                countryList: state.countryList,
                paymentOptionList: state.paymentOptionList,
                requesting: true,
                fetchCountriesSuccess: false,
                fetchPaymentOptionsSuccess: state.fetchPaymentOptionsSuccess,
                errors: {},
            };
        case "CHECKOUT_GET_COUNTRY_LIST_SUCCESS":
            return {
                countryList: action.countryList,
                paymentOptionList: state.paymentOptionList,
                requesting: false,
                fetchCountriesSuccess: true,
                fetchPaymentOptionsSuccess: state.fetchPaymentOptionsSuccess,
                errors: {},
            };
        case "CHECKOUT_GET_COUNTRY_LIST_FAILURE":
            return {
                requesting: false,
                fetchCountriesSuccess: false,
                fetchPaymentOptionsSuccess: state.fetchPaymentOptionsSuccess,
                errors: action.errors,
            };


        case "CHECKOUT_GET_PAYMENTOPTIONS_LIST_REQUEST":
            return {
                countryList: state.countryList,
                paymentOptionList: state.paymentOptionList,
                requesting: true,
                fetchPaymentOptionsSuccess: false,
                errors: {},
            };
        case "CHECKOUT_GET_PAYMENTOPTIONS_LIST_SUCCESS":
            return {
                paymentOptionList: action.paymentOptionList,
                countryList: state.countryList,
                fetchCountriesSuccess: state.fetchCountriesSuccess,
                requesting: false,
                fetchPaymentOptionsSuccess: true,
                errors: {},
            };
        case "CHECKOUT_GET_PAYMENTOPTIONS_LIST_FAILURE":
            return {
                requesting: false,
                countryList: state.countryList,
                fetchPaymentOptionsSuccess: false,
                fetchCountriesSuccess: state.fetchCountriesSuccess,
                paymentOptionList: state.paymentOptionList,
                errors: action.errors,
            };

        case "USERS_PAYMENT_REQUEST":
            return {
                requesting: true,
                countryList: state.countryList,
                fetchCountriesSuccess: state.fetchCountriesSuccess,
                paymentOptionList: state.paymentOptionList,
                errors: {},
            };
        case "USERS_PAYMENT_SUCCESS":
            return {
                requesting: false,
                countryList: state.countryList,
                fetchCountriesSuccess: state.fetchCountriesSuccess,
                paymentDetails: action.paymentDetails,
                paymentOptionList: state.paymentOptionList,
                errors: {},
            };
        case "USERS_PAYMENT_FAILURE":
            return {
                requesting: false,
                countryList: state.countryList,
                fetchCountriesSuccess: state.fetchCountriesSuccess,
                paymentOptionList: state.paymentOptionList,
                errors: action.errors,
            };
        case "PROMOCODE_REQUEST":
            return {
                ...state,
                requesting: true,
                promocodesuccess: false,
                fetchPaymentOptionsSuccess: false,
                errors: {},
            };
        case "PROMOCODE_SUCCESS":
            return {
                ...state,
                requesting: false,
                fetchPaymentOptionsSuccess: true,
                promocodesuccess: action.promocodesuccess,
                errors: {},
            };
        case "PROMOCODE_FAILURE":
            return {
                ...state,
                fetchPaymentOptionsSuccess: true,
                requesting: false,
                promocodesuccess: false,
                errors: action.errors,
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultPaymentState;
};