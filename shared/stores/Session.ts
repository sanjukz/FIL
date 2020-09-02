import { addTask, fetch } from "domain-task";
import * as extend from "lodash/extend";
import { Action, ActionCreator, Reducer } from "redux";
import { SessionViewModel } from "../models/SessionViewModel";
import { UserViewModel } from "../models/UserViewModel";
import { ReportingUserViewModel } from "../models/ReportingUserViewModel";
import { sessionService } from "../services/session";

export interface ISessionApplicationState {
    session: ISessionState;
}

export const GETSESSION_REQUEST = "GetSessionRequestAction";
export const GETSESSION_RECEIVED = "GetSessionReceivedAction";
export const RESETSESSION_REQUEST = "ResetSessionRequestAction";
export const GETSESSION_ERROR = "GetSessionFailureAction"

export interface ISessionProps {
    session: ISessionState;

}

export interface ISessionState {
    isLoading: boolean;
    id: string;
    success: boolean;
    isAuthenticated: boolean;
    isRequestRecieved?: boolean;
    user: UserViewModel;
    reportingUser: ReportingUserViewModel;
    masterEventCreationContext?: any;
    intercomHash?: string;
}

const DefaultSessionState: ISessionState = {
    isLoading: false,
    id: undefined,
    isAuthenticated: false,
    success: false,
    user: undefined,
    reportingUser: undefined,
    intercomHash: undefined,
    isRequestRecieved: false
};

export interface IResetSessionRequestAction {
    type: "ResetSessionRequestAction";
}

export interface IGetSessionRequestAction {
    type: "GetSessionRequestAction";
}

export interface IGetSessionFailureAction {
    type: "GetSessionFailureAction";
}

export interface IGetSessionReceivedAction {
    type: "GetSessionReceivedAction";
    payload: SessionViewModel;
}

export type ISessionAppThunkAction<TAction> = (dispatch: (action: TAction) => void, getState: () => ISessionApplicationState) => void;

// todo: import Success and Invalid actions from server and handle them
type KnownAction = IResetSessionRequestAction | IGetSessionRequestAction | IGetSessionReceivedAction | IGetSessionFailureAction;

export const actionCreators = {

    getSession: (sessionViewModel?: SessionViewModel): ISessionAppThunkAction<KnownAction> => async (dispatch, getState) => {
        if (sessionViewModel) {
            dispatch({ type: GETSESSION_RECEIVED, payload: sessionViewModel });
            return;
        }

        const options: RequestInit = {
            credentials: "include",
            headers: new Headers({
                "Content-Type": "application/json"
            })
        };

        const fetchTask = fetch("/api/session", options)
            .then((response) => response.json() as Promise<SessionViewModel>)
            .then((responseSessionViewModel) => dispatch({
                type: GETSESSION_RECEIVED,
                payload: responseSessionViewModel
            })).catch((error) => dispatch({
                type: GETSESSION_ERROR,
            }));

        addTask(fetchTask);
        dispatch({ type: GETSESSION_REQUEST });
    },

    logout: (): ISessionAppThunkAction<KnownAction> => async (dispatch, getState) => {
        const options: RequestInit = {
            credentials: "include",
            redirect: "follow",
            headers: new Headers({
                "Content-Type": "application/json"
            })
        };

        const fetchTask = fetch("/api/session/logout", options)
            .then((response) => dispatch({ type: RESETSESSION_REQUEST }));
        addTask(fetchTask);
    },
};

export const reducer: Reducer<ISessionState> = (state: ISessionState, action: KnownAction) => {
    switch (action.type) {
        case RESETSESSION_REQUEST: {
            return DefaultSessionState;
        }
        case GETSESSION_REQUEST: {
            return extend({}, state, { isLoading: true, isRequestRecieved: false });
        }
        case GETSESSION_RECEIVED: {
            const session = action.payload;
            return extend({}, state, {
                isLoading: false,
                success: true,
                isRequestRecieved: true,
                ...session
            });
        }
        case GETSESSION_ERROR: {
            return extend({}, state, { isLoading: false });
        }
        default:
            const exhaustiveCheck: never = action;
    }

    return state || DefaultSessionState;
};
