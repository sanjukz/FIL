import { Action, Reducer } from "redux";
import { fetch, addTask } from 'domain-task';
import { IAppThunkAction } from "./";
import { GetUserDetailResponseModel } from "../models/LiveOnline/GetUserDetailResponseModel";
import { DeactivateUserResponseModel } from "../models/LiveOnline/DeactivateUserResponseModel";


export interface ILiveOnlineProps {
    LiveOnline: ILiveOnlineState;
}


export interface ILiveOnlineState {
    isLoading?: boolean;
    errors?: any;
    userDetails?: GetUserDetailResponseModel;
    deactivate?: DeactivateUserResponseModel;
}


interface IReqestUserDataAction {
    type: "REQUEST_USER_DATA_ACTION";
}

interface IRecieveUserDataAction {
    type: "RECIEVE_USER_DATA_ACTION";
    userDetails: GetUserDetailResponseModel;
}
interface IReqestDeactivateUserAction {
    type: "REQUEST_DEACTIVATE_USER_ACTION";
}

interface IRecieveDeactivateUserAction {
    type: "RECIEVE_DEACTIVATE_USER_DATA_ACTION";
    deactivate: DeactivateUserResponseModel;
}

type KnownAction = IReqestUserDataAction | IRecieveUserDataAction | IRecieveDeactivateUserAction | IReqestDeactivateUserAction;

export const actionCreators = {

    requestUserData: (altId: string, uniqueId: string, isZoomLandingPage: boolean, callback: (ReviewsRatingResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/validate-user/${altId}/${uniqueId}/${isZoomLandingPage}`)
            .then((response) => response.json() as Promise<GetUserDetailResponseModel>)
            .then((userDetails) => {
                dispatch({ type: 'RECIEVE_USER_DATA_ACTION', userDetails });
                callback(userDetails);
            });
        addTask(fetchTask);
        dispatch({ type: 'REQUEST_USER_DATA_ACTION' });
    },
    deactiveUser: (altId: string, uniqueId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/deactivate-user/${altId}/${uniqueId}`)
            .then((response) => response.json() as Promise<DeactivateUserResponseModel>)
            .then((deactivate) => {
                dispatch({ type: 'RECIEVE_DEACTIVATE_USER_DATA_ACTION', deactivate });
            });
        addTask(fetchTask);
        dispatch({ type: 'REQUEST_DEACTIVATE_USER_ACTION' });
    },


};
const unloadedState: ILiveOnlineState = {
    isLoading: false,
    errors: null,
    userDetails: null,
    deactivate: null
};
export const reducer: Reducer<ILiveOnlineState> = (state: ILiveOnlineState, action: KnownAction) => {
    switch (action.type) {
        case "REQUEST_USER_DATA_ACTION":
            return {
                isLoading: true,
                userDetails: state.userDetails
            };
        case "RECIEVE_USER_DATA_ACTION":
            return {
                isLoading: false,
                userDetails: action.userDetails
            };
        case "REQUEST_DEACTIVATE_USER_ACTION":
            return {
                isLoading: true,
                userDetails: state.userDetails,
                deactivate: state.deactivate
            };
        case "RECIEVE_DEACTIVATE_USER_DATA_ACTION":
            return {
                isLoading: false,
                userDetails: state.userDetails,
                deactivate: action.deactivate
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};