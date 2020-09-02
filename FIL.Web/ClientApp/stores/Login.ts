import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import { LoginResponseViewModel } from "shared/models/LoginResponseViewModel";
import { ForgotPasswordFormDataViewModel } from "shared/models/ForgotPasswordFormDataViewModel";
import { ForgotPasswordResponseViewModel } from "shared/models/ForgotPasswordResponseViewModel";
import { sessionService } from "shared/services/session";
import {
    actionCreators as sessionActionCreators,
    IGetSessionReceivedAction,
    IGetSessionRequestAction
} from "shared/stores/Session";
import { IAppThunkAction } from "./";

const LOGIN_REQUEST = "LoginRequestAction";
const LOGIN_SUCCESS = "LoginSuccessAction";
const LOGIN_INVALID = "LoginInvalidAction";
const LOGIN_ERROR = "LoginErrorAction";

const FORGOT_PASSWORD_REQUEST = "ForgotPasswordRequestAction";
const FORGOT_PASSWORD_SUCCESS = "ForgotPasswordSuccessAction";
const FORGOT_PASSWORD_INVALID = "ForgotPasswordInvalidAction";
const FORGOT_PASSWORD_ERROR = "ForgotPasswordErrorAction";

const LOGOUT_REQUEST = "LogoutRequestAction";
const LOGOUT_SUCCESS = "LogoutSuccessAction";
const LOGOUT_ERROR = "LogoutErrorAction";

const REST_FORGOT_PASSWORD = "RestForgotPasswordAction";

export interface ILoginProps {
    loginState: ILoginState;
}

export interface ILoginState {
    authenticating: boolean;
    authenticated: boolean;
    invalidCredentials: boolean;
    invalidEmail: boolean;
    changePasswordMailSent: boolean;
    isLockedOut: boolean;
    isActivated?: boolean;
    isloading?: boolean;
}

const DefaultLoginState: ILoginState = {
    authenticating: false,
    authenticated: false,
    invalidCredentials: false,
    invalidEmail: false,
    changePasswordMailSent: false,
    isLockedOut: false,
    isActivated: true,
    isloading: false
};

interface ILoginRequestAction {
    type: "LoginRequestAction";
}

interface ILoginErrorAction {
    type: "LoginErrorAction";
}

export interface ILoginSuccessAction {
    type: "LoginSuccessAction";
}

interface ILoginInvalidAction {
    type: "LoginInvalidAction";
    isLockedOut?: boolean;
    isActivated?: boolean;
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

interface ILogoutErrorAction {
    type: "LogoutErrorAction";
}

interface IAuthenticatedAction {
    type: "AuthenticatedAction";
    idToken: any;
    authToken: string;
}

interface ILogoutRequestAction {
    type: "LogoutRequestAction";
}

interface ILogoutSuccessAction {
    type: "LogoutSuccessAction";
}

interface IRestForgotPasswordAction {
    type: "RestForgotPasswordAction";
}

type KnownAction =
    | ILoginRequestAction
    | ILoginErrorAction
    | ILoginSuccessAction
    | IForgotPasswordRequestAction
    | IForgotPasswordErrorAction
    | IForgotPasswordSuccessAction
    | IForgotPasswordInvalidAction
    | ILoginInvalidAction
    | ILogoutRequestAction
    | ILogoutErrorAction
    | ILogoutSuccessAction
    | IRestForgotPasswordAction;

export const actionCreators = {
    login: (
        loginInput: LoginFormDataViewModel,
        callback: (LoginResponseViewModel) => void
    ): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: LOGIN_REQUEST });

        sessionService.login(loginInput).then(
            (response: LoginResponseViewModel) => {
                if (response == undefined)
                    dispatch({ type: LOGIN_INVALID, isLockedOut: false, isActivated: true });
                if (
                    response.success == true &&
                    response.session.isAuthenticated == true
                ) {
                    dispatch({ type: LOGIN_SUCCESS });
                    dispatch(sessionActionCreators.getSession(response.session) as any);
                } else if (response.success == false &&
                    response.session.isAuthenticated == false) {
                    dispatch({ type: LOGIN_INVALID, isLockedOut: response.isLockedOut, isActivated: response.isActivated });
                }
                callback(response);
            },
            error => {
                dispatch({ type: LOGIN_ERROR });
            }
        );
    },

    restForgotPassword: () => {
        return { type: REST_FORGOT_PASSWORD };
    },

    forgotPassword: (
        forgotPasswordInput: ForgotPasswordFormDataViewModel,
        callback: (ForgotPasswordResponseViewModel) => void
    ): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: FORGOT_PASSWORD_REQUEST });
        sessionService.forgotPassword(forgotPasswordInput).then(
            (response: ForgotPasswordResponseViewModel) => {
                if (response.success == true && response.isExisting == true) {
                    dispatch({ type: FORGOT_PASSWORD_SUCCESS });
                } else if (response.success == true && response.isExisting == false) {
                    dispatch({ type: FORGOT_PASSWORD_INVALID });
                }
                callback(response);
            },
            error => {
                dispatch({ type: FORGOT_PASSWORD_ERROR });
            }
        );
    }
};

export const reducer: Reducer<ILoginState> = (
    state: ILoginState,
    action: KnownAction
) => {
    switch (action.type) {
        case LOGIN_REQUEST:
            return {
                ...state,
                authenticating: true,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: true
            };
        case LOGIN_ERROR:
            return {
                ...state,
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: false
            };
        case LOGIN_SUCCESS:
            return {
                ...state,
                authenticating: false,
                authenticated: true,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: false
            };
        case LOGIN_INVALID:
            return {
                authenticating: false,
                authenticated: false,
                invalidCredentials: true,
                changePasswordMailSent: false,
                invalidEmail: false,
                isLockedOut: action.isLockedOut,
                isActivated: action.isActivated,
                isloading: false
            };
        case FORGOT_PASSWORD_REQUEST:
            return {
                ...state,
                authenticating: true,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: true
            };
        case FORGOT_PASSWORD_ERROR:
            return {
                ...state,
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: false
            };
        case FORGOT_PASSWORD_SUCCESS:
            return {
                ...state,
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: true,
                invalidEmail: false
            };
        case FORGOT_PASSWORD_INVALID:
            return {
                ...state,
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: true,
                isloading: false
            };
        case LOGOUT_REQUEST:
            return {
                ...state,
                authenticating: true,
                authenticated: true,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: true
            };
        case LOGOUT_ERROR:
            return {
                ...state,
                authenticating: false,
                authenticated: true,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: false
            };
        case LOGOUT_SUCCESS:
            return {
                ...state,
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false,
                isloading: false
            };
        case REST_FORGOT_PASSWORD: {
            return {
                ...state,
                authenticating: false,
                authenticated: false,
                invalidCredentials: false,
                changePasswordMailSent: false,
                invalidEmail: false
            };
        }
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultLoginState;
};