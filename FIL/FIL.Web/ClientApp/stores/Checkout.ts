import { fetch, addTask } from "domain-task";
import { Reducer } from "redux";
import CountryDataViewModel from "../models/Country/CountryDataViewModel";
import { LoginTransactionFormDataViewModel } from "../models/Checkout/TransactionFormDataViewModel"
import { UserCheckoutResponseViewModel } from "../models/UserCheckoutResponseViewModel";
import { DeliveryOptionsFormDataViewModel } from "../models/DeliveryOptions/DeliveryDetailFormDataViewModel";
import { UpdateTransactionResponseViewModel } from "../models/UpdateTransactionResponseViewModel";
import { deliveryservice } from "../services/deliveryOptions";
import { sessionService } from "shared/services/session";
import { userService } from "../services/user";
import {
    actionCreators as sessionActionCreators
} from "shared/stores/Session";
import { IAppThunkAction } from "./";
import { checkoutservice } from "../services/checkout";
import { ForgotPasswordFormDataViewModel } from "shared/models/ForgotPasswordFormDataViewModel";
import { ForgotPasswordResponseViewModel } from "shared/models/ForgotPasswordResponseViewModel";
import { DeliveryOptionsDataViewModel } from "../models/DeliveryOptions/DeliveryOptionsDataViewModel";

export const CHECKOUT_LOGIN_REQUEST = "CheckoutLoginRequestAction";
export const CHECKOUT_LOGIN_SUCCESS = "CheckoutLoginSuccessAction";
export const CHECKOUT_LOGIN_INVALID = "CheckoutLoginInvalidAction";
export const CHECKOUT_LOGIN_ERROR = "CheckoutLoginErrorAction";

export const GUEST_REGISTER_REQUEST = "GuestRegisterRequestAction";
export const GUEST_REGISTER_SUCCESS = "GuestRegisterSuccessAction";
export const GUEST_REGISTER_FAILURE = "GuestRegisterFailure";

export const FORGOT_PASSWORD_REQUEST = "ForgotPasswordRequestAction";
export const FORGOT_PASSWORD_SUCCESS = "ForgotPasswordSuccessAction";
export const FORGOT_PASSWORD_INVALID = "ForgotPasswordInvalidAction";
export const FORGOT_PASSWORD_ERROR = "ForgotPasswordErrorAction";

export const GET_COUNTRY_LIST_REQUEST = "CHECKOUT_GET_COUNTRY_LIST_REQUEST";
export const GET_COUNTRY_LIST_SUCCESS = "CHECKOUT_GET_COUNTRY_LIST_SUCCESS";
export const GET_COUNTRY_LIST_FAILURE = "CHECKOUT_GET_COUNTRY_LIST_FAILURE";

export const SAVE_DELIVERY_OPTIONS_REQUEST = "USERS_SAVE_DELIVERY_OPTIONS_REQUEST";
export const SAVE_DELIVERY_OPTIONS_SUCCESS = "USERS_SAVE_DELIVERY_OPTIONS_SUCCESS";
export const SAVE_DELIVERY_OPTIONS_FAILURE = "USERS_SAVE_DELIVERY_OPTIONS_FAILURE";

export const RESET_FORGOT_PASSWORD = "ResetForgotPasswordAction";

export interface ICheckoutLoginProps {
    checkout: ICheckoutLoginState;
}

export interface ICheckoutLoginState {
    currentPage?: string;
    authenticating?: boolean;
    authenticated?: boolean;
    requesting?: boolean;
    registered?: boolean;
    invalidCredentials: boolean;
    fetchCountriesSuccess?: boolean;
    transactionId?: number;
    transactionDetail?: UpdateTransactionResponseViewModel;
    countryList?: CountryDataViewModel;
    countries?: any;
    errors?: any;
    changePasswordMailSent: boolean;
    invalidEmail: boolean;
    deliveryOptions?: DeliveryOptionsDataViewModel;
}

const emptyDeliveryOptionsDataViewModel: DeliveryOptionsDataViewModel = {
    eventDeliveryTypeDetails: [],
    userDetails: [],
};

const emptyCountries: CountryDataViewModel = {
    countries: [],
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

const DefaultLoginState: ICheckoutLoginState = {
    authenticating: false,
    currentPage: "deliveryOptions",
    authenticated: false,
    invalidCredentials: false,
    requesting: false,
    registered: false,
    fetchCountriesSuccess: false,
    transactionDetail: emptyTransactionDetail,
    countryList: emptyCountries,
    errors: {},
    changePasswordMailSent: false,
    invalidEmail: false,
    deliveryOptions: emptyDeliveryOptionsDataViewModel
};

interface ICheckoutLoginRequestAction {
    type: "CheckoutLoginRequestAction";
}

interface ICheckoutLoginErrorAction {
    type: "CheckoutLoginErrorAction";
}

export interface ICheckoutLoginSuccessAction {
    type: "CheckoutLoginSuccessAction";
    transactionId: number;
}

interface ILoginInvalidAction {
    type: "CheckoutLoginInvalidAction";
}

interface IAuthenticatedAction {
    type: "AuthenticatedAction";
    idToken: any;
    authToken: string;
}

interface IGuestRegistrationRequestAction {
    type: "GuestRegisterRequestAction";
}

interface IGuestRegistrationSuccesstAction {
    type: "GuestRegisterSuccessAction";
    transactionId: number;
}

interface IGuestRegistrationFailureAction {
    type: "GuestRegisterFailure";
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
    transactionDetails: UpdateTransactionResponseViewModel;
}

interface ISaveDeliveryOptionsFailureAction {
    type: "USERS_SAVE_DELIVERY_OPTIONS_FAILURE";
    errors: {};
}

interface IForgotPasswordRequestAction {
    type: "ForgotPasswordRequestAction";
}

interface IForgotPasswordErrorAction {
    type: "ForgotPasswordErrorAction";
}

export interface IForgotPasswordSuccessAction {
    type: "ForgotPasswordSuccessAction";
}

interface IForgotPasswordInvalidAction {
    type: "ForgotPasswordInvalidAction";
}

interface IDeliveryOptionsRequestAction {
    type: "DeliveryOptionsRequestAction";
}

export interface IDeliveryOptionsSuccessAction {
    type: "DeliveryOptionsSuccessAction";
    deliveryOptions: DeliveryOptionsDataViewModel;
}

interface IResetForgotPasswordAction {
    type: "ResetForgotPasswordAction";
}

type KnownAction = ICheckoutLoginRequestAction
    | ICheckoutLoginErrorAction
    | ICheckoutLoginSuccessAction
    | ILoginInvalidAction
    | IGuestRegistrationRequestAction
    | IGuestRegistrationSuccesstAction
    | IGuestRegistrationFailureAction
    | IDeliveryOptionsRequestAction
    | IDeliveryOptionsSuccessAction
    | IRequestGetCountryListAction | IReceiveGetCountryListAction | IGetCountryListFailureAction |
    ISaveDeliveryOptionsRequestAction | ISaveDeliveryOptionsSuccesstAction | ISaveDeliveryOptionsFailureAction | IForgotPasswordRequestAction | IForgotPasswordErrorAction | IForgotPasswordSuccessAction | IForgotPasswordInvalidAction | IResetForgotPasswordAction;

export const actionCreators = {
    checkoutLogin: (checkoutLoginInput: LoginTransactionFormDataViewModel, callback: (UserCheckoutResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: CHECKOUT_LOGIN_REQUEST });
            checkoutservice.login(checkoutLoginInput)
                .then((response: UserCheckoutResponseViewModel) => {
                    if (response.success == true && response.session.isAuthenticated == true) {
                        dispatch({ type: CHECKOUT_LOGIN_SUCCESS, transactionId: response.transactionId });
                        dispatch(sessionActionCreators.getSession(response.session) as any);
                    } else {
                        dispatch({ type: CHECKOUT_LOGIN_INVALID });
                    }
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: CHECKOUT_LOGIN_ERROR });
                    });
        },

    resetForgotPassword: () => {
        return ({ type: RESET_FORGOT_PASSWORD })
    },

    checkoutLoginToDeliveryOption: (checkoutLoginInput: LoginTransactionFormDataViewModel, callback: (UserCheckoutResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: CHECKOUT_LOGIN_REQUEST });
            checkoutservice.checkoutLoginToDeliveyOption(checkoutLoginInput)
                .then((response: UserCheckoutResponseViewModel) => {
                    if (response.success == true && response.session.isAuthenticated == true) {
                        dispatch({ type: CHECKOUT_LOGIN_SUCCESS, transactionId: response.transactionId });
                        dispatch(sessionActionCreators.getSession(response.session) as any);
                    } else {
                        dispatch({ type: CHECKOUT_LOGIN_INVALID });
                    }
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: CHECKOUT_LOGIN_ERROR });
                    });
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

    requestDeliveryOptions: (eventDetailId: number): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/get/deliveryoptions/${eventDetailId}`)
            .then((response) => response.json() as Promise<DeliveryOptionsDataViewModel>)
            .then((data) => {
                dispatch({ type: "DeliveryOptionsSuccessAction", deliveryOptions: data, });
            },
                (error) => {
                },
            );
        addTask(fetchTask);
        dispatch({ type: "DeliveryOptionsRequestAction" });
    },

    saveDeliveryOptions: (detailModel: DeliveryOptionsFormDataViewModel, callback: (UpdateTransactionResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "USERS_SAVE_DELIVERY_OPTIONS_REQUEST" });
            deliveryservice.saveDeliveryDetails(detailModel)
                .then((response: UpdateTransactionResponseViewModel) => {
                    dispatch({ type: "USERS_SAVE_DELIVERY_OPTIONS_SUCCESS", transactionDetails: response });
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "USERS_SAVE_DELIVERY_OPTIONS_FAILURE", errors: error });
                    });
        },
    forgotPassword: (forgotPasswordInput: ForgotPasswordFormDataViewModel, callback: (ForgotPasswordResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: FORGOT_PASSWORD_REQUEST });
            userService.forgotPassword(forgotPasswordInput)
                .then((response: ForgotPasswordResponseViewModel) => {
                    if (response.success == true && response.isExisting == true) {
                        dispatch({ type: FORGOT_PASSWORD_SUCCESS });
                    } else if (response.success == true && response.isExisting == false) {
                        dispatch({ type: FORGOT_PASSWORD_INVALID });
                    }
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: FORGOT_PASSWORD_ERROR });
                    });
        },
};

export const reducer: Reducer<ICheckoutLoginState> = (state: ICheckoutLoginState, action: KnownAction) => {
    switch (action.type) {
        case CHECKOUT_LOGIN_REQUEST:
            return { authenticating: true, authenticated: false, invalidCredentials: false, changePasswordMailSent: false, invalidEmail: false };
        case CHECKOUT_LOGIN_ERROR:
            return { authenticating: false, authenticated: false, invalidCredentials: false, changePasswordMailSent: false, invalidEmail: false };
        case CHECKOUT_LOGIN_SUCCESS:
            return { authenticating: false, transactionId: action.transactionId, authenticated: true, invalidCredentials: false, changePasswordMailSent: false, invalidEmail: false };
        case CHECKOUT_LOGIN_INVALID:
            return { authenticating: false, authenticated: false, invalidCredentials: true, changePasswordMailSent: false, invalidEmail: true };
        case GUEST_REGISTER_REQUEST:
            return { requesting: true, registered: false, invalidCredentials: false, errors: {}, changePasswordMailSent: false, invalidEmail: false };
        case GUEST_REGISTER_SUCCESS:
            return { requesting: false, registered: false, transactionId: action.transactionId, invalidCredentials: false, errors: {}, changePasswordMailSent: false, invalidEmail: false };
        case GUEST_REGISTER_FAILURE:
            return { requesting: false, registered: false, invalidCredentials: false, errors: {}, changePasswordMailSent: false, invalidEmail: false };

        case FORGOT_PASSWORD_REQUEST:
            return {
                authenticating: true,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: false
            };
        case FORGOT_PASSWORD_ERROR:
            return {
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: false
            };
        case FORGOT_PASSWORD_SUCCESS:
            return {
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: true,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: false
            };
        case FORGOT_PASSWORD_INVALID:
            return {
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: true
            };

        case "CHECKOUT_GET_COUNTRY_LIST_REQUEST":
            return {
                countryList: state.countryList,
                requesting: true,
                invalidCredentials: false,
                fetchCountriesSuccess: false,
                errors: {},
                changePasswordMailSent: state.changePasswordMailSent,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: state.invalidEmail
            };
        case "CHECKOUT_GET_COUNTRY_LIST_SUCCESS":
            return {
                countryList: action.countryList,
                requesting: false,
                fetchCountriesSuccess: true,
                invalidCredentials: state.invalidCredentials,
                errors: {},
                changePasswordMailSent: state.changePasswordMailSent,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: state.invalidEmail
            };
        case "CHECKOUT_GET_COUNTRY_LIST_FAILURE":
            return {
                requesting: false,
                fetchCountriesSuccess: false,
                invalidCredentials: state.invalidCredentials,
                errors: action.errors,
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: state.invalidEmail
            };
        case "USERS_SAVE_DELIVERY_OPTIONS_REQUEST":
            return {
                requesting: true,
                transactionDetails: state.transactionDetail,
                countryList: state.countryList,
                fetchCountriesSuccess: false,
                invalidCredentials: false,
                errors: {},
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: false
            };
        case "USERS_SAVE_DELIVERY_OPTIONS_SUCCESS":
            return {
                requesting: false,
                transactionDetails: action.transactionDetails,
                countryList: state.countryList,
                currentPage: "payment",
                invalidCredentials: false,
                fetchCountriesSuccess: true,
                errors: {},
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: false
            };
        case "USERS_SAVE_DELIVERY_OPTIONS_FAILURE":
            return {
                requesting: false,
                invalidCredentials: false,
                errors: action.errors,
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: false
            };
        case "DeliveryOptionsSuccessAction":
            return {
                requesting: false,
                transactionDetails: state.transactionDetail,
                countryList: state.countryList,
                currentPage: "payment",
                invalidCredentials: false,
                fetchCountriesSuccess: true,
                errors: {},
                changePasswordMailSent: false,
                deliveryOptions: action.deliveryOptions,
                invalidEmail: false
            };
        case "DeliveryOptionsRequestAction":
            return {
                requesting: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                deliveryOptions: emptyDeliveryOptionsDataViewModel
            };
        case RESET_FORGOT_PASSWORD:
            return {
                authenticating: true,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                deliveryOptions: state.deliveryOptions,
                invalidEmail: false,
            }


        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultLoginState;
};
