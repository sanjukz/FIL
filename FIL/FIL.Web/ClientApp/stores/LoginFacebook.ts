﻿import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import { LoginResponseViewModel } from "shared/models/LoginResponseViewModel";
import FacebookSignInFormDataViewModel from "../models/SocialSignIn/FacebookSignInFormDataViewModel";
import { FacebookSignInResponseViewModel } from "../models/SocialSignIn/FacebookSignInResponseViewModel";
import { sessionService } from "shared/services/session";
import {
    actionCreators as sessionActionCreators,
    IGetSessionReceivedAction,
    IGetSessionRequestAction
} from "shared/stores/Session";
import { IAppThunkAction } from "./";
import { FacebookSignInService } from "../services/FacebookSignIn";

export const LOGIN_REQUEST = "LoginRequestAction";
export const LOGIN_SUCCESS = "LoginSuccessAction";
export const LOGIN_INVALID = "LoginInvalidAction";
export const LOGIN_ERROR = "LoginErrorAction";

export interface ILoginState {
    currentPage?: string;
    authenticating?: boolean;
    authenticated?: boolean;
    requesting?: boolean;
    registered?: boolean;
    invalidCredentials: boolean;
	transactionId?: number;
    errors?: any;
}

const DefaultLoginState: ILoginState = {
    authenticating: false,
    currentPage: "loginFacebook",
    authenticated: false,
    invalidCredentials: false,
    requesting: false,
    registered: false,
    errors: {},
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
}

interface IAuthenticatedAction {
    type: "AuthenticatedAction";
    idToken: any;
    authToken: string;
}

type KnownAction = ILoginRequestAction
    | ILoginErrorAction
    | ILoginSuccessAction
    | ILoginInvalidAction;

export const actionCreators = {
    facebookLogin: (LoginInput: FacebookSignInFormDataViewModel, callback: (FacebookSignInResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: LOGIN_REQUEST });
            FacebookSignInService.facebooklogin(LoginInput)
                .then((response: FacebookSignInResponseViewModel) => {
                    if (response.success == true) {
                        dispatch({ type: LOGIN_SUCCESS });
                        dispatch(sessionActionCreators.getSession(response.session) as any);
                    } else {
                        dispatch({ type: LOGIN_INVALID });
                    }
                    callback(response);
                },
                (error) => {
                    dispatch({ type: LOGIN_ERROR });
               });
        },
};
export const reducer: Reducer<ILoginState> = (state: ILoginState, action: KnownAction) => {
    switch (action.type) {
        case LOGIN_REQUEST:
            return { authenticating: true, authenticated: false, invalidCredentials: false };
        case LOGIN_ERROR:
            return { authenticating: false, authenticated: false, invalidCredentials: false };
        case LOGIN_SUCCESS:
            return { authenticating: false, authenticated: true, invalidCredentials: false };
        case LOGIN_INVALID:
            return { authenticating: false, authenticated: false, invalidCredentials: true };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultLoginState;
};