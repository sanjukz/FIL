import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "./";

export interface IAllocationComponentState {
    isLoading: boolean;
    allocations: IAllocationsData;
}

export interface IAllocation {
    stand: string;
    block: string;    
    level: string;
    ticketsAvailable: string;
    tbd: string;
}

export interface IAllocationsData {
    allocationsData: IAllocation[]; 
}

interface IRequestAllocationDataAction {
    type: "REQUEST_ALLOCATION_DATA";

}

interface IReceiveAllocationDataAction {
    type: "RECEIVE_ALLOCATION_DATA";

    allocations: IAllocationsData;
}

type KnownAction = IRequestAllocationDataAction | IReceiveAllocationDataAction;

export const actionCreators = {
    requestAllocationdata: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/allocation`)
            .then((response) => response.json() as Promise<IAllocationsData>)
            .then((data) => {
                dispatch({ type: "RECEIVE_ALLOCATION_DATA", allocations: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_ALLOCATION_DATA" });
    }
};

const emptyAllocation: IAllocationsData = {
    allocationsData: []
};

const unloadedState: IAllocationComponentState = {
    allocations: emptyAllocation, isLoading: false
}

export const reducer: Reducer<IAllocationComponentState> = (state: IAllocationComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_ALLOCATION_DATA":
            return {
                allocations: state.allocations,
                isLoading: true
            };
        case "RECEIVE_ALLOCATION_DATA":
            return {
                allocations: action.allocations,
                isLoading: false
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
