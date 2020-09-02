import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { AlertDataViewModel } from "../models/Alert/AlertDataViewModel";
import { ChangePasswordFormDataViewModel } from "../models/Account/ChangePasswordFormDataViewModel";
import { ChangePasswordResponseViewModel } from "../models/Account/ChangePasswordResponseViewModel";
import { ChangeProfilePicDataViewModel } from "../models/Account/ChangeProfilePicDataViewModel";
import { ChangeProfilePicResponseViewModel } from "../models/Account/ChangeProfilePicResponseViewModel";
import { accountService } from "../services/account";

export const CHANGE_PASSWORD_REQUEST = "USERS_CHANGE_PASSWORD_REQUEST";
export const CHANGE_PASSWORD_SUCCESS = "USERS_CHANGE_PASSWORD_SUCCESS";
export const CHANGE_PASSWORD_INVALID = "USERS_CHANGE_PASSWORD_INVALID";
export const CHANGE_PASSWORD_FAILURE = "USERS_CHANGE_PASSWORD_FAILURE";

export const CHANGE_PROFILEPIC_REQUEST = "USERS_CHANGE_PROFILEPIC_REQUEST";
export const CHANGE_PROFILEPIC_SUCCESS = "USERS_CHANGE_PROFILEPIC_SUCCESS";
export const CHANGE_PROFILEPIC_INVALID = "USERS_CHANGE_PROFILEPIC_INVALID";
export const CHANGE_PROFILEPIC_FAILURE = "USERS_CHANGE_PROFILEPIC_FAILURE";

export interface IUserAccountProps {
    UserAccount: IUserAccountState;
}

export interface IUserAccountState {
    alertMessage: AlertDataViewModel,
    requesting: boolean
    updateSuccess: boolean;
    updateProfilePicSuccess: boolean;
    errors?: any;
    invalidPassword?: boolean;
}

const initialAlert: AlertDataViewModel = {
    success: false,
    subject: "",
    body: "",
};

interface IRequestChangePasswordAction {
    type: "USERS_CHANGE_PASSWORD_REQUEST";
}

interface IReceiveChangePasswordAction {
    type: "USERS_CHANGE_PASSWORD_SUCCESS";
    alertMessage: AlertDataViewModel;
}

interface IChangePasswordInvalidAction {
    type: "USERS_CHANGE_PASSWORD_INVALID";
}

interface IChangePasswordFailureAction {
    type: "USERS_CHANGE_PASSWORD_FAILURE";
    errors: any;
}

interface IRequestChangeProfilePicAction {
    type: "USERS_CHANGE_PROFILEPIC_REQUEST";
}

interface IReceiveChangeProfilePicAction {
    type: "USERS_CHANGE_PROFILEPIC_SUCCESS";
    alertMessage: AlertDataViewModel;
}

interface IChangeProfilePicInvalidAction {
    type: "USERS_CHANGE_PROFILEPIC_INVALID";
}

interface IChangeProfilePicFailureAction {
    type: "USERS_CHANGE_PROFILEPIC_FAILURE";
    errors: any;
}

const DefaultRegisterState: IUserAccountState = {
    alertMessage: initialAlert,
    requesting: false,
    updateSuccess: false,
    updateProfilePicSuccess: false,
    errors: {},
    invalidPassword: false
};

type KnownAction = IRequestChangePasswordAction | IReceiveChangePasswordAction | IChangePasswordInvalidAction |
    IChangePasswordFailureAction | IRequestChangeProfilePicAction | IReceiveChangeProfilePicAction |
    IChangeProfilePicInvalidAction | IChangeProfilePicFailureAction;

export const actionCreators = {
    changePasswordAction: (changePasswordModel: ChangePasswordFormDataViewModel, callback: (ChangePasswordResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: CHANGE_PASSWORD_REQUEST });

            accountService.changePassword(changePasswordModel)
                .then((user: ChangePasswordResponseViewModel) => {
                    if (user.success == true) {
                        var alertModel: AlertDataViewModel = {
                            success: true,
                            subject: "Change Password",
                            body: "Password updated successfully",
                        };
                        dispatch({ type: CHANGE_PASSWORD_SUCCESS, alertMessage: alertModel });
                        callback(user);
                    }
                    else if (user.success == false) {
                        dispatch({ type: CHANGE_PASSWORD_INVALID });
                    }

                },
                    (error) => {
                        dispatch({ type: CHANGE_PASSWORD_FAILURE, errors: error });
                    });
        },
    changeProfilePicAction: (changeProfilePicModel: ChangeProfilePicDataViewModel)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: CHANGE_PROFILEPIC_REQUEST });

            accountService.changeProfilePic(changeProfilePicModel)
                .then((response: ChangeProfilePicResponseViewModel) => {
                    if (response.success == true) {
                        var alertModel: AlertDataViewModel = {
                            success: true,
                            subject: "Change Password",
                            body: "Password updated successfully",
                        };
                        dispatch({ type: CHANGE_PROFILEPIC_SUCCESS, alertMessage: alertModel });
                        //callback(response);
                    }
                    else if (response.success == false) {
                        dispatch({ type: CHANGE_PROFILEPIC_INVALID });
                    }

                },
                    (error) => {
                        dispatch({ type: CHANGE_PROFILEPIC_FAILURE, errors: error });
                    });
        }
};

export const reducer: Reducer<IUserAccountState> = (state: IUserAccountState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "USERS_CHANGE_PASSWORD_REQUEST":
            return { requesting: true, updateSuccess: false, updateProfilePicSuccess: false, alertMessage: initialAlert, invalidPassword: false, errors: {} };
        case "USERS_CHANGE_PASSWORD_SUCCESS":
            return { requesting: false, updateSuccess: true, updateProfilePicSuccess: false, alertMessage: action.alertMessage, invalidPassword: false, errors: {} };
        case "USERS_CHANGE_PASSWORD_INVALID":
            return { requesting: false, updateSuccess: false, updateProfilePicSuccess: false, alertMessage: initialAlert, invalidPassword: true, errors: {} };
        case "USERS_CHANGE_PASSWORD_FAILURE":
            return { requesting: false, updateSuccess: false, updateProfilePicSuccess: false, alertMessage: initialAlert, invalidPassword: false, errors: action.errors };

        case "USERS_CHANGE_PROFILEPIC_REQUEST":
            return { requesting: true, updateSuccess: false, updateProfilePicSuccess: false, alertMessage: initialAlert, invalidPassword: false, errors: {} };
        case "USERS_CHANGE_PROFILEPIC_SUCCESS":
            return { requesting: false, updateSuccess: false, updateProfilePicSuccess: true, alertMessage: action.alertMessage, invalidPassword: false, errors: {} };
        case "USERS_CHANGE_PROFILEPIC_INVALID":
            return { requesting: false, updateSuccess: false, updateProfilePicSuccess: false, alertMessage: initialAlert, invalidPassword: false, errors: {} };
        case "USERS_CHANGE_PROFILEPIC_FAILURE":
            return { requesting: false, updateSuccess: false, updateProfilePicSuccess: false, alertMessage: initialAlert, invalidPassword: false, errors: action.errors };

        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultRegisterState;
};