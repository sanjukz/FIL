import { addTask, fetch } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { ActivateUserResponseViewModel } from "../models/ActivateUserResponseViewModel";
import { IAppThunkAction } from "./";

export interface IActivateUserPageComponentState {
    isLoading?: boolean;
    errors?: any;
    fetchSuccess: boolean;
    result: ActivateUserResponseViewModel;
}

interface IRequestActivateUserPageData {
    type: "REQUEST_ACTIVATE_USER_PAGE_DATA";
}

interface IReceiveActivateUserPageData {
    type: "RECEIVE_ACTIVATE_USER_PAGE_DATA";
    result: ActivateUserResponseViewModel;
}

type KnownAction = IRequestActivateUserPageData | IReceiveActivateUserPageData;

export const actionCreators = {
    requestActivateUserData: (altId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/account/activate/${altId}`)
            .then((response) => response.json() as Promise<ActivateUserResponseViewModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_ACTIVATE_USER_PAGE_DATA", result: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_ACTIVATE_USER_PAGE_DATA" });

    }
};


const activateUserPageData: ActivateUserResponseViewModel = {
    success: false,
 };

const unloadedState: IActivateUserPageComponentState = {
    errors: null,
    fetchSuccess: false,
    result: activateUserPageData,
    isLoading: false
};

export const reducer: Reducer<IActivateUserPageComponentState> = (state: IActivateUserPageComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_ACTIVATE_USER_PAGE_DATA":
            return {
                result: state.result,
                fetchSuccess: false,
                isLoading: true
            };
        case "RECEIVE_ACTIVATE_USER_PAGE_DATA":
            return {
                result: action.result,
                fetchSuccess: true,
                isLoading: false
            };

        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
