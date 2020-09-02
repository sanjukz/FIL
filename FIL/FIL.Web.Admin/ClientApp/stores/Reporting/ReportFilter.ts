import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "../";
import { ReportEventsResponseViewModel } from "../../models/ReportingWIzard/ReportEventsResponseViewModel";
import { ReportSubEventsResponseViewModel } from "../../models/ReportingWIzard/ReportSubEventsResponseViewModel";
import { ReportFormDataViewModel } from "../../models/ReportingWizard/ReportFormDataViewModel";
import { ReportResponseDataViewModel } from "../../models/ReportingWizard/ReportResponseDataViewModel";
import { reportService } from "../../services/reporting/report";

export interface IReportFilterState {
    userAltId?: any;
    isLoading: boolean;
    fetchEventSuccess: boolean;
    fetchSubEventSuccess: boolean;
    valEvent?: any;
    valSubEvent?: any;
    valFromDate?: any;
    valToDate?: any;
    errors?: any;
    allEvents: IEvents;
    allSubEvents: ISubEvents;
}

export interface IEvents {
    events: ReportEventsResponseViewModel[];
}

interface IRequestEventDataAction {
    type: "REQUEST_EVENT_DATA";
}

interface IReceiveEventDataAction {
    type: "RECEIVE_EVENT_DATA";
    allEvents: IEvents;
}

export interface ISubEvents {
    subEvents: ReportSubEventsResponseViewModel[];
}

interface IRequestSubEventDataAction {
    type: "REQUEST_SUBEVENT_DATA";
}

interface IReceiveSubEventDataAction {
    type: "RECEIVE_SUBEVENT_DATA";
    allSubEvents: ISubEvents;
}

type KnownAction = IRequestEventDataAction | IReceiveEventDataAction | IRequestSubEventDataAction | IReceiveSubEventDataAction;

export const actionCreators = {
    requestEventData: (altId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/report/event/all/` + altId)
            .then((response) => response.json() as Promise<IEvents>)
            .then((data) => {
                dispatch({ type: "RECEIVE_EVENT_DATA", allEvents: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_DATA" });
    },
    requestSubEventData: (altId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/report/subevent/all/` + altId)
            .then((response) => response.json() as Promise<ISubEvents>)
            .then((data) => {
                dispatch({ type: "RECEIVE_SUBEVENT_DATA", allSubEvents: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_SUBEVENT_DATA" });
    },
};

const emptyEvent: IEvents = {
    events: []
};

const emptySubEvent: ISubEvents = {
    subEvents: []
};

const unloadedState: IReportFilterState = {
    allEvents: emptyEvent, allSubEvents: emptySubEvent, isLoading: false, fetchEventSuccess: false, fetchSubEventSuccess: false, errors: null
}

export const reducer: Reducer<IReportFilterState> = (state: IReportFilterState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_EVENT_DATA":
            return {
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                fetchEventSuccess: false,
                fetchSubEventSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_EVENT_DATA":
            return {
                allEvents: action.allEvents,
                allSubEvents: state.allSubEvents,
                fetchEventSuccess: true,
                fetchSubEventSuccess: false,
                isLoading: false,
                errors: {}
            };
        case "REQUEST_SUBEVENT_DATA":
            return {
                allSubEvents: state.allSubEvents,
                allEvents: state.allEvents,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_SUBEVENT_DATA":
            return {
                allSubEvents: action.allSubEvents,
                allEvents: state.allEvents,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: true,
                isLoading: false,
                errors: {}
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};