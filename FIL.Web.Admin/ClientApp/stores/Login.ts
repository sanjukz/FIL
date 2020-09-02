import { fetch } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import { LoginResponseViewModel } from "shared/models/LoginResponseViewModel";
import { sessionService } from "shared/services/session";
import {
    actionCreators as sessionActionCreators,
    IGetSessionReceivedAction,
    IGetSessionRequestAction
} from "shared/stores/Session";
import { IAppThunkAction } from "./";

export const LOGIN_REQUEST = "LoginRequestAction";
export const LOGIN_SUCCESS = "LoginSuccessAction";
export const LOGIN_INVALID = "LoginInvalidAction";
export const LOGIN_ERROR = "LoginErrorAction";
export const LOGOUT_REQUEST = "LogoutRequestAction";
export const LOGOUT_SUCCESS = "LogoutSuccessAction";
export const LOGOUT_ERROR = "LogoutErrorAction";

export interface ILoginState {
    authenticating: boolean;
    authenticated: boolean;
    invalidCredentials: boolean;
}

const DefaultLoginState: ILoginState = {
    authenticating: false,
    authenticated: false,
    invalidCredentials: false
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

type KnownAction = ILoginRequestAction | ILoginErrorAction | ILoginSuccessAction |
    ILoginInvalidAction | ILogoutRequestAction | ILogoutErrorAction | ILogoutSuccessAction;

export const actionCreators = {
    login: (loginInput: LoginFormDataViewModel, callback: (LoginResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {

            dispatch({ type: LOGIN_REQUEST });

            sessionService.login(loginInput)
                .then((response: LoginResponseViewModel) => {
                    
                    if (response.success == true && response.session.isAuthenticated == true && response.session.user.rolesId==1) {
                        dispatch({ type: LOGIN_SUCCESS });
                        dispatch(sessionActionCreators.getSession(response.session) as any);
                    } else if (response.success == false && response.session.isAuthenticated == false) {
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
        case LOGOUT_REQUEST:
            return { authenticating: true, authenticated: true, invalidCredentials: false };
        case LOGOUT_ERROR:
            return { authenticating: false, authenticated: true, invalidCredentials: false };
        case LOGOUT_SUCCESS:
            return { authenticating: false, authenticated: false, invalidCredentials: false };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultLoginState;
};
