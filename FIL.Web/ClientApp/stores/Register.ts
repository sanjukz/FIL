import { fetch, addTask } from "domain-task";
import { Action, Reducer } from "redux";
import { userService } from "../services/user";
import { IAppThunkAction } from "./";
import { AlertDataViewModel } from "../models/Alert/AlertDataViewModel";
import { RegistrationFormDataViewModel } from "shared/models/RegistrationFormDataViewModel";
import { RegistrationResponseViewModel } from "shared/models/RegistrationResponseViewModel";
import CountryDataViewModel from "../models/Country/CountryDataViewModel";

export const REGISTER_REQUEST = "USERS_REGISTER_REQUEST";
export const REGISTER_SUCCESS = "USERS_REGISTER_SUCCESS";
export const REGISTER_FAILURE = "USERS_REGISTER_FAILURE";
export const USER_EXISTS = "USERS_EXISTING_REGISTER";
export const HIDE_ALERT = "HIDE_REGISTER_ALERT";

export const GET_COUNTRY_LIST_REQUEST = "REGISTER_GET_COUNTRY_LIST_REQUEST";
export const GET_COUNTRY_LIST_SUCCESS = "REGISTER_GET_COUNTRY_LIST_SUCCESS";
export const GET_COUNTRY_LIST_FAILURE = "REGISTER_GET_COUNTRY_LIST_FAILURE";

export interface IRegisterFormModel {
    email: string;
}

export interface IRegisterState {
    requesting: boolean;
    registered: boolean;
    userExists: boolean;
    error: boolean;
    requestCountry?: boolean;
    alertMessage: AlertDataViewModel;
    errorMessage: any;
    fetchCountriesSuccess?: boolean;
    countryList?: CountryDataViewModel;
    countries?: any;
}

const emptyCountries: CountryDataViewModel = {
    countries: [],
};

const initialAlert: AlertDataViewModel = {
    success: false,
    subject: "",
    body: "",
};

const DefaultRegisterState: IRegisterState = {
    requesting: false,
    registered: false,
    userExists: false,
    error: false,
    requestCountry: false,
    fetchCountriesSuccess: false,
    countryList: emptyCountries,
    alertMessage: initialAlert,
    errorMessage: {}
};

interface IRegistrationRequestAction {
    type: "USERS_REGISTER_REQUEST";
}

interface IRegistrationSuccesstAction {
    type: "USERS_REGISTER_SUCCESS";
    alertMessage: AlertDataViewModel;
}

interface IRegistrationFailureAction {
    type: "USERS_REGISTER_FAILURE";
    alertMessage: AlertDataViewModel;
    error: any;
}

interface IRegistrationUserExistsAction {
    type: "USERS_EXISTING_REGISTER";
    alertMessage: AlertDataViewModel;
}

interface IHideAlertAction {
    type: "HIDE_REGISTER_ALERT";
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

type KnownAction = IRegistrationRequestAction | IRegistrationSuccesstAction | IRegistrationFailureAction |
    IRegistrationUserExistsAction | IHideAlertAction | IRequestGetCountryListAction | IReceiveGetCountryListAction | IGetCountryListFailureAction;

export const actionCreators = {
    register: (registerModel: RegistrationFormDataViewModel, callback: (RegistrationResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: REGISTER_REQUEST });

        userService.register(registerModel)
            .then(
                (user: RegistrationResponseViewModel) => {

                    if (user.isExisting) {
                        var alertModel: AlertDataViewModel = {
                            success: true,
                            subject: "Alredy Registered",
                            body: "This email is already registered with an account. Please use a different email address and try again.",
                        };
                        dispatch({ type: USER_EXISTS, alertMessage: alertModel });
                        callback(user);
                    } else if (user.success) {
                        var alertModel: AlertDataViewModel = {
                            success: true,
                            subject: "Registered Successfully",
                            body: "Woohoo! Your account has now been created. Have a great experience as you feelaplace and tell us how you feel. You can also refer your friends to feelaplace by sending out invites from your “My account” section.",
                        };
                        dispatch({ type: REGISTER_SUCCESS, alertMessage: alertModel });
                        callback(user);
                    }
                    else {
                        var alertModel: AlertDataViewModel = {
                            success: false,
                            subject: "Not successful",
                            body: "Oops! Something went wrong. Please try again.",
                        };
                        dispatch({ type: REGISTER_FAILURE, alertMessage: alertModel, error: '' });
                        callback(user);
                    }
                },
                (error) => {
                    var alertModel: AlertDataViewModel = {
                        success: false,
                        subject: "Registration failed",
                        body: error,
                    };
                    dispatch({ type: REGISTER_FAILURE, alertMessage: alertModel, error: error });
                }
            );
    },
    hideAlertAction: (): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: HIDE_ALERT });
    },
    requestCountryData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/country/all`)
            .then((response) => response.json() as Promise<CountryDataViewModel>)
            .then((data) => {
                dispatch({ type: "REGISTER_GET_COUNTRY_LIST_SUCCESS", countryList: data, });
            },
                (error) => {
                    dispatch({ type: "REGISTER_GET_COUNTRY_LIST_FAILURE", errors: error });
                },
            );
        addTask(fetchTask);
        dispatch({ type: "REGISTER_GET_COUNTRY_LIST_REQUEST" });
    },
};

export const reducer: Reducer<IRegisterState> = (state: IRegisterState, action: KnownAction) => {
    switch (action.type) {
        case REGISTER_REQUEST:
            return { requesting: true, userExists: false, registered: false, alertMessage: initialAlert, error: false, errorMessage: {}, countryList: state.countryList, fetchCountriesSuccess: state.fetchCountriesSuccess, };
        case REGISTER_SUCCESS:
            return { requesting: false, userExists: false, registered: true, alertMessage: action.alertMessage, error: false, errorMessage: {}, countryList: state.countryList, fetchCountriesSuccess: state.fetchCountriesSuccess, };
        case REGISTER_FAILURE:
            return { requesting: false, userExists: false, registered: false, alertMessage: action.alertMessage, error: true, errorMessage: action.error, countryList: state.countryList, fetchCountriesSuccess: state.fetchCountriesSuccess, };
        case USER_EXISTS:
            return {
                requesting: false, userExists: true, registered: false, alertMessage: action.alertMessage, error: false, errorMessage: {}, countryList: state.countryList, fetchCountriesSuccess: state.fetchCountriesSuccess,
            };
        case HIDE_ALERT:
            return {
                requesting: false, userExists: false, registered: false, alertMessage: initialAlert, error: false, errorMessage: {}, countryList: state.countryList, fetchCountriesSuccess: state.fetchCountriesSuccess,
            };
        case "REGISTER_GET_COUNTRY_LIST_REQUEST":
            return {
                countryList: state.countryList,
                requestCountry: true,
                requesting: state.requesting,
                userExists: state.userExists,
                registered: state.registered,
                alertMessage: state.alertMessage,
                error: false,
                errorMessage: state.errorMessage,
                fetchCountriesSuccess: false,
            };
        case "REGISTER_GET_COUNTRY_LIST_SUCCESS":
            return {
                countryList: action.countryList,
                requestCountry: false,
                requesting: state.requesting,
                userExists: state.userExists,
                registered: state.registered,
                alertMessage: state.alertMessage,
                error: false,
                errorMessage: state.errorMessage,
                fetchCountriesSuccess: true,
            };
        case "REGISTER_GET_COUNTRY_LIST_FAILURE":
            return {
                requestCountry: false,
                requesting: state.requesting,
                userExists: state.userExists,
                registered: state.registered,
                alertMessage: state.alertMessage,
                error: false,
                errorMessage: state.errorMessage,
                fetchCountriesSuccess: false,
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultRegisterState;
};