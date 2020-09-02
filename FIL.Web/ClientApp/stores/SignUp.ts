import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from ".";
import CountryDataViewModel from "../models/Country/CountryDataViewModel";
import { RegistrationFormDataViewModel } from "shared/models/RegistrationFormDataViewModel";
import { RegistrationResponseViewModel } from "shared/models/RegistrationResponseViewModel";
import { SendAndValidateOTPFormModel } from "shared/models/SendAndValidateOTPFormModel";
import { SendAndValidateOTPResponseModel } from "shared/models/SendAndValidateOTPResponseModel";
import { LoginWithOTPFormModel } from "shared/models/LoginWithOTPFormModel";
import { LoginWithOTPResponseModel } from "shared/models/LoginWithOTPResponseModel";
import { userService } from "../services/user";
import { actionCreators as sessionActionCreators } from "shared/stores/Session";

export interface ISignUpProps {
    signUp: ISignUpState;
}

export interface ISignUpState {
    isLoading?: boolean;
    errors?: any;
    fetchCountriesSuccess?: boolean;
    countryList?: CountryDataViewModel;
    signUpRequestSuccess?: boolean;
    requestError?: boolean;
    requestOTP?: SendAndValidateOTPResponseModel;
}

interface IRequestGetCountryListAction {
    type: "REGISTER_GET_COUNTRY_LIST_REQUEST";
}

interface IReceiveGetCountryListAction {
    type: "REGISTER_GET_COUNTRY_LIST_SUCCESS";
    countryList: CountryDataViewModel;
}

interface IGetCountryListFailureAction {
    type: "REGISTER_GET_COUNTRY_LIST_FAILURE";
    errors: any;
}
interface IRegistrationRequestAction {
    type: "USERS_REGISTER_REQUEST";
}

interface IRegistrationSuccesstAction {
    type: "USERS_REGISTER_SUCCESS";
}

interface IRegistrationFailureAction {
    type: "USERS_REGISTER_FAILURE";
    error: any;
}
interface IOTPRequestAction {
    type: "USERS_OTP_REQUEST";
}

interface IOTPSuccesstAction {
    type: "USERS_OTP_SUCCESS";
}

interface IOTPFailureAction {
    type: "USERS_OTP_FAILURE";
    error: any;
}

interface IOTPLoginRequestAction {
    type: "USERS_OTP_LOGIN_REQUEST";
}

interface IOTPLoginSuccesstAction {
    type: "USERS_OTP_LOGIN_SUCCESS";
}

interface IOTPLoginFailureAction {
    type: "USERS_OTP_LOGIN_FAILURE";
    error: any;
}
type KnownAction = IRequestGetCountryListAction | IReceiveGetCountryListAction | IGetCountryListFailureAction |
    IRegistrationRequestAction | IRegistrationSuccesstAction | IRegistrationFailureAction | IOTPRequestAction
    | IOTPSuccesstAction | IOTPFailureAction | IOTPLoginRequestAction | IOTPLoginSuccesstAction | IOTPLoginFailureAction;

export const actionCreators = {
    requestCountryData: (callback?: (CountryDataViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        const fetchTask = fetch(`api/country/all`)
            .then((response) => response.json() as Promise<CountryDataViewModel>)
            .then((data) => {
                dispatch({ type: "REGISTER_GET_COUNTRY_LIST_SUCCESS", countryList: data, });
                if (callback)
                    callback(data);
            },
                (error) => {
                    dispatch({ type: "REGISTER_GET_COUNTRY_LIST_FAILURE", errors: error });
                },
            );
        addTask(fetchTask);
        dispatch({ type: "REGISTER_GET_COUNTRY_LIST_REQUEST" });
    },

    register: (registerModel: RegistrationFormDataViewModel, callback: (RegistrationResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "USERS_REGISTER_REQUEST" });
        userService.register(registerModel)
            .then(
                (user: RegistrationResponseViewModel) => {
                    dispatch({ type: "USERS_REGISTER_SUCCESS" });
                    callback(user);
                },
                (error) => {
                    dispatch({ type: "USERS_REGISTER_FAILURE", error });
                }
            );
    },

    //For Otp Signup/sign in 
    requestAndValidateOTP: (registerModel: SendAndValidateOTPFormModel, callback: (SendAndValidateOTPResponseModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "USERS_OTP_REQUEST" });
        userService.requestAndValidateOTP(registerModel)
            .then(
                (user: SendAndValidateOTPResponseModel) => {
                    dispatch({ type: "USERS_OTP_SUCCESS" });
                    callback(user);
                },
                (error) => {
                    dispatch({ type: "USERS_OTP_FAILURE", error });
                }
            );
    },
    loginWithOtp: (requestModel: LoginWithOTPFormModel, callback: (LoginWithOTPResponseModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "USERS_OTP_LOGIN_REQUEST" });
        userService.loginWithOtp(requestModel)
            .then(
                (user: LoginWithOTPResponseModel) => {
                    if (user.success)
                        dispatch(sessionActionCreators.getSession(user.session) as any);
                    dispatch({ type: "USERS_OTP_LOGIN_SUCCESS" });
                    callback(user);
                },
                (error) => {
                    dispatch({ type: "USERS_OTP_LOGIN_FAILURE", error });
                }
            );
    },
};


const emptyCountries: CountryDataViewModel = {
    countries: [],
};

const unloadedState: ISignUpState = {
    isLoading: false,
    errors: null,
    fetchCountriesSuccess: false,
    countryList: emptyCountries,
    requestError: false,
    signUpRequestSuccess: false,
    requestOTP: null
};

export const reducer: Reducer<ISignUpState> = (
    state: ISignUpState,
    incomingAction: Action
) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REGISTER_GET_COUNTRY_LIST_REQUEST":
            return {
                countryList: state.countryList,
                fetchCountriesSuccess: false,
            };
        case "REGISTER_GET_COUNTRY_LIST_SUCCESS":
            return {
                countryList: action.countryList,
                fetchCountriesSuccess: true,
            };
        case "REGISTER_GET_COUNTRY_LIST_FAILURE":
            return {
                requestCountry: false,
                errors: action.errors,
                fetchCountriesSuccess: false,
            };
        case "USERS_REGISTER_REQUEST":
            return {
                ...state,
                isLoading: true,
                signUpRequestSuccess: false,
                requestError: false,
            };
        case "USERS_REGISTER_SUCCESS":
            return {
                ...state,
                isLoading: false,
                signUpRequestSuccess: true,
                requestError: false,
            };
        case "USERS_REGISTER_FAILURE":
            return {
                requestError: true,
                isLoading: false,
                errors: action.error
            };
        case "USERS_OTP_REQUEST":
            return {
                ...state,
                isLoading: true,
                signUpRequestSuccess: false,
                requestError: false,
            };
        case "USERS_OTP_SUCCESS":
            return {
                ...state,
                isLoading: false,
                signUpRequestSuccess: true,
                requestError: false,
            };
        case "USERS_OTP_FAILURE":
            return {
                requestError: true,
                isLoading: false,
                errors: action.error
            };
        case "USERS_OTP_LOGIN_REQUEST":
            return {
                ...state,
                isLoading: true,
                signUpRequestSuccess: false,
                requestError: false,
            };
        case "USERS_OTP_LOGIN_SUCCESS":
            return {
                ...state,
                isLoading: false,
                signUpRequestSuccess: true,
                requestError: false,
            };
        case "USERS_OTP_LOGIN_FAILURE":
            return {
                requestError: true,
                isLoading: false,
                errors: action.error
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
