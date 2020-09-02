import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from ".";
import { EventCreationViewModel } from '../models/EventCreation/EventCreationViewModel';

export interface ILocationTypeProps {
    statesType: ILocationTypeComponentState;
}

export interface ILocationTypeComponentState {
    isLoading: boolean;
    locationTypeSuccess: boolean;

}

interface IRequestLocationTypes {
    type: "REQUEST_LOCATION_TYPE_DATA";
}

interface IReceiveLoactionTypes {
    type: "RECEIVE_LOCATION_TYPE_DATA";
}

type KnownAction = IRequestLocationTypes | IReceiveLoactionTypes;

export const actionCreators = {


    requestStatesTypeData: (locationModel: EventCreationViewModel, callback: (EventCreationViewModel) => void)

    
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {

        const fetchTask = fetch(`api/state/`)
            .then((response) => response.json() as Promise<EventCreationViewModel>)
            .then((data) => {
                
                dispatch({ type: "RECEIVE_LOCATION_TYPE_DATA"});
                callback(data);
            });
            
        addTask(fetchTask);
        dispatch({ type: "REQUEST_LOCATION_TYPE_DATA" });
    },

};

const unloadedLocationType: ILocationTypeComponentState = {
    isLoading: false,
    locationTypeSuccess: false,
};

export const reducer: Reducer<ILocationTypeComponentState> = (state: ILocationTypeComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    
    switch (action.type) {
        case "REQUEST_LOCATION_TYPE_DATA":
            return {
                isLoading: true,
                locationTypeSuccess: false,
            };
        case "RECEIVE_LOCATION_TYPE_DATA":
            return {
                isLoading: true,
                locationTypeSuccess: true,
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedLocationType;
};
