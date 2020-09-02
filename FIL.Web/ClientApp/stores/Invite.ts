import { Action, Reducer } from "redux";
import { inviteService } from "../services/Invite";
import { IAppThunkAction } from "./";
import { UserInviteResponseViewModel } from "../models/Invite/UserInviteResponseViewModel";
import { UserInviteRequestViewModel } from  "../models/Invite/UserInviteRequestViewModel";
export const INVITE_REQUEST = "WEBSITE_INVITE_REQUEST";
export const INVITE_SUCCESS = "WEBSITE_INVITE_SUCCESS";
export const INVITE_FAILURE = "WEBSITE_INVITE_FAILURE";
export const ALREADY_INVITED = "USER_ALREADY_INVITED";
export const HIDE_ALERT = "HIDE_LETTER_INVITE_ALERT";

export interface IInviteState {
    requesting: boolean;
    Invited: boolean;
    alertVisible: boolean;
    inviteSuccess: boolean;
    inviteFailure: boolean;
    subscriptionExists: boolean;
    errors: any;
    alertMessage: string;
}

const DefaultInviteState: IInviteState = {
    requesting: false,
    Invited: false,
    alertVisible: false,
    subscriptionExists: false,
    errors: {},
    inviteSuccess: false,
    inviteFailure: false,
    alertMessage: ""
};

interface IRegistrationRequestAction {
    type: "WEBSITE_INVITE_REQUEST";
}

interface IRegistrationSuccesstAction {
    type: "WEBSITE_INVITE_SUCCESS";
}

interface IRegistrationFailureAction {
    type: "WEBSITE_INVITE_FAILURE";
}

interface ISubscribedAlreadyAction {
    type: "USER_ALREADY_INVITED";
}

interface IHideAlertAction {
    type: "HIDE_LETTER_INVITE_ALERT";
}

type KnownAction = IRegistrationRequestAction | IRegistrationSuccesstAction |
    IRegistrationFailureAction | ISubscribedAlreadyAction | IHideAlertAction;

export const actionCreators = {
    InviteRequest: (InviteModel: UserInviteRequestViewModel, callback: (InviteResponseDataViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: INVITE_REQUEST });
            inviteService.sendUserInvite(InviteModel)
                .then((response:any) => {
                    
                    if (response.isUsed ) {
                        dispatch({ type: ALREADY_INVITED });
                    } else if (response.success) {
                        dispatch({ type: INVITE_SUCCESS });
                    }
                    else if(!response.success)
                    {
                        dispatch({ type: INVITE_FAILURE });
                    }
                    callback(response);
                },
                (error) => {
                    dispatch({ type: INVITE_FAILURE });
                });
        },
    hideAlertAction: (): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: HIDE_ALERT });
    },
    getWebsiteInviteConfig : (): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        inviteService.getSiteInviteConfig("default")
                .then((response) => {
                    console.log(response);
                },
                (error) => {
                    dispatch({ type: INVITE_FAILURE });
                });
    },
};

export const reducer: Reducer<IInviteState> = (state: IInviteState, action: KnownAction) => {
    switch (action.type) {
        case INVITE_REQUEST:
            return {
                requesting: true, Invited: false, subscriptionExists: false, alertVisible: false, errors: {},
                inviteSuccess : false, inviteFailure:false, alertMessage:"Invite request"
            };
        case INVITE_SUCCESS:
            return {
                requesting: false, Invited: true, subscriptionExists: false, alertVisible: true, errors: {},
                inviteSuccess : true, inviteFailure:false, alertMessage:"Thanks for helping us out! The invite is on its way.", 
            };
        case INVITE_FAILURE:
            return {
                requesting: false, Invited: false, subscriptionExists: false, alertVisible: false, errors: {},
                inviteSuccess : false, inviteFailure:false, alertMessage:"Invite failed"
            };
        case ALREADY_INVITED:
            return {
                requesting: false, Invited: false, subscriptionExists: true, alertVisible: true, errors: {},
                inviteSuccess : false, inviteFailure:false, alertMessage:"Already Invited",
            };
        case HIDE_ALERT:
            return {
                requesting: false, Invited: true, subscriptionExists: false, alertVisible: false, errors: {},
                inviteSuccess : false, inviteFailure:false, alertMessage:"Alert hidden",
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultInviteState;
};