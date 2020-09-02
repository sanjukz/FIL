import { Action, Reducer } from "redux";
import { IAppThunkAction } from "./";
import { UserInviteResponseViewModel } from "../models/UserInviteResponseViewModel";
import { UserInviteRequestViewModel } from  "../models/UserInviteRequestViewModel";
export const UPDATE_INVITE_REQUEST = "UPDATE_INVITE_REQUEST";
export const UPDATE_INVITE_SUCCESS = "UPDATE_INVITE_SUCCESS";
export const UPDATE_INVITE_FAILURE = "UPDATE_INVITE_FAILURE";

export interface IInviteState {
    requesting: boolean;
    alertVisible: boolean;
    inviteSuccess: boolean;
    inviteFailure: boolean;
    errors: any;
    alertMessage: string;    
}

const DefaultInviteState: IInviteState = {
    requesting: false,
    alertVisible: false,
    errors: {},
    inviteSuccess: false,
    inviteFailure: false,
    alertMessage: ""
};

interface IUpdateInviteRequestAction {
    type: "UPDATE_INVITE_REQUEST";
}

interface IUpdateInviteSuccesstAction {
    type: "UPDATE_INVITE_SUCCESS";
}

interface IUpdateInviteFailureAction {
    type: "UPDATE_INVITE_FAILURE";
}


type KnownAction = IUpdateInviteRequestAction | IUpdateInviteSuccesstAction |
    IUpdateInviteFailureAction ;

export const actionCreators = {
    updateInvite: (InviteModel: UserInviteRequestViewModel, callback: (InviteResponseDataViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: UPDATE_INVITE_REQUEST });
            
            fetch('/api/invite/edit', {
                method: 'POST',
                body: JSON.stringify(InviteModel),
                headers:{
                  'Content-Type': 'application/json'
                }
              }).then((response:any) => {    
                  
                    
                    if (response.ok) {
                        dispatch({ type: UPDATE_INVITE_SUCCESS });
                    }
                    else if(!response.ok)
                    {
                        dispatch({ type: UPDATE_INVITE_FAILURE });
                    }
                    callback(response);
                },
                (error) => {
                    dispatch({ type: UPDATE_INVITE_FAILURE });
                });
        },
        resetProps: (): IAppThunkAction<KnownAction> => async (dispatch, getState) =>{
            dispatch({type: UPDATE_INVITE_REQUEST});
        }

};

export const reducer: Reducer<IInviteState> = (state: IInviteState, action: KnownAction) => {
    switch (action.type) {
        case UPDATE_INVITE_REQUEST:
            return {
                requesting: true,  alertVisible: false, errors: {},
                inviteSuccess : false, inviteFailure:false, alertMessage:"Update request"
            };
        case UPDATE_INVITE_SUCCESS:
            return {
                requesting: false,  alertVisible: true, errors: {},
                inviteSuccess : true, inviteFailure:false, alertMessage:"Successfully updated", 
            };
        case UPDATE_INVITE_FAILURE:
            return {
                requesting: false,  alertVisible: false, errors: {},
                inviteSuccess : false, inviteFailure:false, alertMessage:"Update failed"
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultInviteState;
};