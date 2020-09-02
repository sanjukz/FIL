import { Action, Reducer } from "redux";
import { userService } from "../services/user";
import { IAppThunkAction } from "./";
import { AlertDataViewModel } from "../models/Alert/AlertDataViewModel";
import { ResetPasswordFormDataViewModel } from "shared/models/ResetPasswordFormDataViewModel";
import { ResetPasswordResponseViewModel } from "shared/models/ResetPasswordResponseViewModel";

export const RESET_PASSWORD_REQUEST = "USERS_RESET_PASSWORD_REQUEST";
export const RESET_PASSWORD_SUCCESS = "USERS_RESET_PASSWORD_SUCCESS";
export const RESET_PASSWORD_FAILURE = "USERS_RESET_PASSWORD_FAILURE";
export const PASSWORD_MISMATCH = "USERS_PASSWORD_MISMATCH";
export const HIDE_ALERT = "HIDE_RESET_PASSWORD_ALERT";
export const FROM_VALIDATION = "FROM_VALIDATION";

export interface IResetPasswordState {
    requesting: boolean;
    resetSuccessful: boolean;
    error: boolean;
    passwordMismatch: boolean;
    alertMessage: AlertDataViewModel;
    errorMessage: string;
    validationError?: string;
}

const initialAlert: AlertDataViewModel = {
    success: false,
    subject: "",
    body: "",
};

const DefaultResetPasswordState: IResetPasswordState = {
    requesting: false,
    resetSuccessful: false,
    error: false,
    passwordMismatch: false,
    alertMessage: initialAlert,
    errorMessage: "",
    validationError: '',
};

interface IResetPasswordRequestAction {
    type: "USERS_RESET_PASSWORD_REQUEST";
}

interface IResetPasswordSuccesstAction {
    type: "USERS_RESET_PASSWORD_SUCCESS";
    alertMessage: AlertDataViewModel;
}

interface IResetPasswordFailureAction {
    type: "USERS_RESET_PASSWORD_FAILURE";
    alertMessage: AlertDataViewModel;
    error: any;
}

interface IPasswordMismatchAction {
    type: "USERS_PASSWORD_MISMATCH";
}

interface IHideAlertAction {
    type: "HIDE_RESET_PASSWORD_ALERT";
}

interface IValidationAction {
    type: "FROM_VALIDATION";
    msg: string;
}

type KnownAction = IResetPasswordRequestAction | IResetPasswordSuccesstAction | IResetPasswordFailureAction |
    IPasswordMismatchAction | IHideAlertAction | IValidationAction;

export const actionCreators = {
    resetPassword: (resetPasswordModel: ResetPasswordFormDataViewModel, callback: (ResetPasswordResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: RESET_PASSWORD_REQUEST });
        userService.resetPassword(resetPasswordModel)
            .then(
                (user: ResetPasswordResponseViewModel) => {
                    if (user.success) {
                        var alertModel: AlertDataViewModel = {
                            success: true,
                            subject: "Password Changed",
                            body: "Password changed successfully",
                        };
                        dispatch({ type: RESET_PASSWORD_SUCCESS, alertMessage: alertModel });
                        callback(user);
                    }
                },
                (error) => {
                    var alertModel: AlertDataViewModel = {
                        success: false,
                        subject: "Registration failed",
                        body: error,
                    };
                    dispatch({ type: RESET_PASSWORD_FAILURE, alertMessage: alertModel, error: error });
                });
    },
    passwordMismatchAction: (): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: PASSWORD_MISMATCH });
    },
    validationAction: (msg): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: FROM_VALIDATION, msg: msg });
    },
    hideAlertAction: (): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: HIDE_ALERT });
    },
};

export const reducer: Reducer<IResetPasswordState> = (state: IResetPasswordState, action: KnownAction) => {
    switch (action.type) {
        case RESET_PASSWORD_REQUEST:
            return { requesting: true, resetSuccessful: false, alertMessage: initialAlert, error: false, errorMessage: "", passwordMismatch: false, validationError: '' };
        case RESET_PASSWORD_SUCCESS:
            return { requesting: false, resetSuccessful: true, alertMessage: action.alertMessage, error: false, errorMessage: "", passwordMismatch: false, validationError: '' };
        case RESET_PASSWORD_FAILURE:
            return { requesting: false, resetSuccessful: false, alertMessage: action.alertMessage, error: true, errorMessage: action.error, passwordMismatch: false, validationError: '' };
        case PASSWORD_MISMATCH:
            return {
                requesting: false, resetSuccessful: false, alertMessage: initialAlert, error: false, errorMessage: "", passwordMismatch: true, validationError: ''
            };
        case HIDE_ALERT:
            return {
                requesting: false, resetSuccessful: false, alertMessage: initialAlert, error: false, errorMessage: "", passwordMismatch: false, validationError: ''
            };
        case FROM_VALIDATION:
            return {
                ...state,
                validationError: action.msg
            }
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultResetPasswordState;
};
