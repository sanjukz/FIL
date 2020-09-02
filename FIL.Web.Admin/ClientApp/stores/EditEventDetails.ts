import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "./";
import { EventCreationViewModel } from "../models/EventCreation/EventCreationViewModel";
import { EventDetailsViewModel } from "../models/EventCreation/EventDetailsViewModel";

export interface IEditEventProps {
    events:IEditEventComponentState;
}

export interface IEditEventComponentState {
    isLoading: boolean;
    events: EventDetailsViewModel;

}



interface IRequestEditEvent {
    type: "REQUEST_EDIT_EVENT_DATA";
}

interface IReceiveEditEvent {
    type: "RECEIVE_EDIT_EVENT_DATA";
    payload: EventDetailsViewModel;
}



type KnownAction = IRequestEditEvent | IReceiveEditEvent ;

export const actionCreators = {

    requestEditEvent: (financeModel: string, callback: (EventCreationViewModel) => void)


        : IAppThunkAction<KnownAction> => (dispatch, getState) => {

            const fetchTask = fetch(`api/finance/` + financeModel)
                .then((response) => response.json() as Promise<EventDetailsViewModel>)
                .then((data) => {
                    
                    dispatch({ type: "RECEIVE_EDIT_EVENT_DATA", payload: data });
                    callback(data);
                });

            addTask(fetchTask);
            dispatch({ type: "REQUEST_EDIT_EVENT_DATA" });
        },

        


};

const unloadedEditEvents: EventDetailsViewModel = {
    events: [],
};





const unloadedEditEventsState: IEditEventComponentState = {
    isLoading: false,
    events: unloadedEditEvents

};


export const reducer: Reducer<IEditEventComponentState> = (state: IEditEventComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_EDIT_EVENT_DATA":
            return {
                isLoading: true,
                events: unloadedEditEvents,


            };
        case "RECEIVE_EDIT_EVENT_DATA":

            return {

                isLoading: false,
                events: action.payload,

            };

        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedEditEventsState;
};
