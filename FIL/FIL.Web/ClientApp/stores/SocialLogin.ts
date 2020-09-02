import { Reducer } from "redux";
import { GoogleSignInFormDataViewModel } from "../models/SocialSignIn/GoogleSignInFormDataViewModel";
import { GoogleSignInResponseViewModel } from "../models/SocialSignIn/GoogleSignInResponseViewModel";
import FacebookSignInFormDataViewModel from "../models/SocialSignIn/FacebookSignInFormDataViewModel";
import { FacebookSignInResponseViewModel } from "../models/SocialSignIn/FacebookSignInResponseViewModel";
import { actionCreators as sessionActionCreators } from "shared/stores/Session";
import { IAppThunkAction } from "./";
import { SocialLoginService } from "../services/SocialLogin";


export interface ISocialLoginState {
    authenticating?: boolean;
    authenticated?: boolean;
    invalidCredentials: boolean;
    errors?: any;
}

const DefaultSocialLoginState: ISocialLoginState = {
    authenticating: false,
    authenticated: false,
    invalidCredentials: false,
    errors: {},
};

interface IGoogleLoginRequestAction {
    type: "GoogleLoginRequestAction";
}

interface IGoogleLoginErrorAction {
    type: "GoogleLoginErrorAction";
}

export interface IGoogleLoginSuccessAction {
    type: "GoogleLoginSuccessAction";
}

interface IGoogleLoginInvalidAction {
    type: "GoogleLoginInvalidAction";
}

interface IFacebookLoginRequestAction {
    type: "FacebookLoginRequestAction";
}

interface IFacebookLoginErrorAction {
    type: "FacebookLoginErrorAction";
}

export interface IFacebookLoginSuccessAction {
    type: "FacebookLoginSuccessAction";
}

interface IFacebookLoginInvalidAction {
    type: "FacebookLoginInvalidAction";
}

type KnownAction = IGoogleLoginRequestAction
    | IGoogleLoginErrorAction | IFacebookLoginInvalidAction
    | IGoogleLoginSuccessAction | IFacebookLoginErrorAction
    | IGoogleLoginInvalidAction | IFacebookLoginSuccessAction | IFacebookLoginRequestAction

export const actionCreators = {
    googleLogin: (LoginInput: GoogleSignInFormDataViewModel, callback: (GoogleSignInResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "GoogleLoginRequestAction" });
            SocialLoginService.googlelogin(LoginInput)
                .then((response: GoogleSignInResponseViewModel) => {
                    if (response.success == true) {
                        dispatch({ type: "GoogleLoginSuccessAction" });
                        dispatch(sessionActionCreators.getSession(response.session) as any);
                    } else {
                        dispatch({ type: "GoogleLoginInvalidAction" });
                    }
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "GoogleLoginErrorAction" });
                    });
        },

    facebookLogin: (LoginInput: FacebookSignInFormDataViewModel, callback: (FacebookSignInResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "FacebookLoginRequestAction" });
            SocialLoginService.facebooklogin(LoginInput)
                .then((response: FacebookSignInResponseViewModel) => {
                    if (response.success == true) {
                        dispatch({ type: "FacebookLoginSuccessAction" });
                        dispatch(sessionActionCreators.getSession(response.session) as any);
                    } else {
                        dispatch({ type: "FacebookLoginInvalidAction" });
                    }
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "FacebookLoginErrorAction" });
                    });
        }
};

export const reducer: Reducer<ISocialLoginState> = (state: ISocialLoginState, action: KnownAction) => {
    switch (action.type) {
        case "GoogleLoginRequestAction":
            return { authenticating: true, authenticated: false, invalidCredentials: false };
        case "GoogleLoginErrorAction":
            return { authenticating: false, authenticated: false, invalidCredentials: false };
        case "GoogleLoginSuccessAction":
            return { authenticating: false, authenticated: true, invalidCredentials: false };
        case "GoogleLoginInvalidAction":
            return { authenticating: false, authenticated: false, invalidCredentials: true };
        case "FacebookLoginRequestAction":
            return { authenticating: true, authenticated: false, invalidCredentials: false };
        case "FacebookLoginErrorAction":
            return { authenticating: false, authenticated: false, invalidCredentials: false };
        case "FacebookLoginSuccessAction":
            return { authenticating: false, authenticated: true, invalidCredentials: false };
        case "FacebookLoginInvalidAction":
            return { authenticating: false, authenticated: false, invalidCredentials: true };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultSocialLoginState;
};
