import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { EventSiteIdMappingViewModel } from "../models/EventSiteIdMappingViewModel";

export interface IEventSiteIdComponentState {
    isLoading: boolean;
    eventsiteidmappings: IEventSiteIdMappings;
    sitedata: ISiteData
    eventid: number;
    sortorder: number;
    isenabled: boolean;
    siteid: number;
    alertMessage: string;
    isSuccess: boolean;
    isFailure: boolean;
    siteidmapid: number;
}



export interface IEventSiteIdMap {

    id: number;
    eventId: number;
    sortOrder: number;
    isenabled: boolean;
    siteId: number;
    eventName: string;
    siteName: string;
}
export interface IEventSiteIdMappings {
    eventsiteidmapping: IEventSiteIdMap[];
}

export interface ISite {

    id: number;
    siteName: string;
}

export interface ISiteData {
    siteData: ISite[];
}

interface IRequestSiteDataAction {
    type: "REQUEST_SITE_DATA";
}

interface IReceiveSiteDataAction {
    type: "RECEIVE_SITE_DATA";
    sitedata: ISiteData;
}

interface IRequestEventSiteIdMapDataAction {
    type: "REQUEST_EVENT_SITEID_MAP_DATA";
}

interface IReceiveEventSiteIdMapDataAction {
    type: "RECEIVE_EVENT_SITEID_MAP_DATA";
    eventsiteidmappings: IEventSiteIdMappings;
}

interface IRequestUpdateEventSiteIdMapAction {
    type: "REQUEST_UPDATE_EVENT_SITEID_MAP";
}
interface IUpdatedEventSiteIdMapAction {
    type: "UPDATED_EVENT_SITEID_MAP";
}
interface IFailedEventSiteIdMapAction {
    type: "FAILED_EVENT_SITEID_MAP";
}


interface IUpdateEventSiteIdMapState {
    type: "UPDATE_EVENT_SITEID_MAP_STATE";
    payload:
    {
        eventid: number;
        sortorder: number;
        isenabled: boolean;
        siteid: number;
        siteidmapid: number;
    };
}

type KnownAction = IRequestUpdateEventSiteIdMapAction | IUpdatedEventSiteIdMapAction | IFailedEventSiteIdMapAction
    | IReceiveEventSiteIdMapDataAction | IRequestEventSiteIdMapDataAction | IUpdateEventSiteIdMapState
    | IRequestSiteDataAction | IReceiveSiteDataAction;

export const actionCreators = {

    requestEventSiteIdMappingdata: (eventId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        
        const fetchTask = fetch(`/api/event/siteidmapping/${eventId}`)
            .then((response) => response.json() as Promise<IEventSiteIdMappings>)
            .then((data) => {            
                dispatch({ type: "RECEIVE_EVENT_SITEID_MAP_DATA", eventsiteidmappings: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_SITEID_MAP_DATA" });
    },
    requestSiteData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        
        const fetchTask = fetch(`/api/event/sitedata/`)
            .then((response) => response.json() as Promise<ISiteData>)
            .then((data) => {
                dispatch({ type: "RECEIVE_SITE_DATA", sitedata: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_SITE_DATA" });
    },
    updateEventSiteId: (eventcatModel: EventSiteIdMappingViewModel, callback: (EventSiteIdMappingViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {


        const fetchTask = fetch(`/api/event/sitemap`, {
            method: 'POST',
            body: JSON.stringify(eventcatModel),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then((response: any) => {
                if (response.ok) {
                    dispatch({ type: "UPDATED_EVENT_SITEID_MAP" });
                }
                else if (!response.ok) {
                    dispatch({ type: "FAILED_EVENT_SITEID_MAP" });
                }
                callback(response);
            });

        addTask(fetchTask);
        //dispatch({ type: "REQUEST_EVENT_SITEID_DATA" });
    },
    updateEventSiteIdMappingState: (eventcatModel, callback: (EventSiteIdMappingViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        
        dispatch({
            type: "UPDATE_EVENT_SITEID_MAP_STATE", payload:
            {
                eventid: eventcatModel.original.eventId,
                sortorder: eventcatModel.original.sortOrder,
                isenabled: eventcatModel.original.isEnabled,
                siteidmapid: eventcatModel.original.id,
                siteid: eventcatModel.original.siteId
            }
        });
    }
};

const emptyeventsiteidmappings: IEventSiteIdMappings = {
    eventsiteidmapping: null
};

const emptysitedata: ISiteData = {
    siteData: null
};

const unloadedState: IEventSiteIdComponentState = {
    isLoading: false, eventid: 0, siteid: 0,
    siteidmapid: 0, isenabled: false, alertMessage: "", isSuccess: false, isFailure: false,
    eventsiteidmappings: emptyeventsiteidmappings, sortorder: 0, sitedata: emptysitedata
}

export const reducer: Reducer<IEventSiteIdComponentState> = (state: IEventSiteIdComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        
        case "REQUEST_SITE_DATA":
            return {
                isLoading: true,
                eventid: 0, siteid: 0, sortorder: 0, isenabled: false,
                alertMessage: "", isSuccess: false, isFailure: false
                , eventsiteidmappings: state.eventsiteidmappings, siteidmapid: 0, sitedata: state.sitedata
            };
        case "RECEIVE_SITE_DATA":            
            return {
                isLoading: true,
                eventid: 0, siteid: 0, sortorder: 0, isenabled: false,
                alertMessage: "", isSuccess: false, isFailure: false
                , eventsiteidmappings: state.eventsiteidmappings, siteidmapid: 0, sitedata: action.sitedata
            };

        case "REQUEST_UPDATE_EVENT_SITEID_MAP":

            return {
                isLoading: true,
                eventid: 0, siteid: 0, sortorder: 0, isenabled: false,
                alertMessage: "", isSuccess: false, isFailure: false
                , eventsiteidmappings: state.eventsiteidmappings, siteidmapid: 0, sitedata: state.sitedata
            };
        case "UPDATED_EVENT_SITEID_MAP":
            return {
                isLoading: false,
                eventid: 0, siteid: 0, sortorder: 0, isenabled: false,
                alertMessage: "Successfully updated", isSuccess: true, isFailure: false
                , eventsiteidmappings: state.eventsiteidmappings, siteidmapid: 0, sitedata: state.sitedata
            };
        case "FAILED_EVENT_SITEID_MAP":
            return {
                isLoading: false,
                eventid: 0, siteid: 0, sortorder: 0, isenabled: false,
                alertMessage: "Update failure", isSuccess: false, isFailure: true
                , eventsiteidmappings: state.eventsiteidmappings, siteidmapid: 0, sitedata: state.sitedata
            };
        case "RECEIVE_EVENT_SITEID_MAP_DATA":            
            return {
                isLoading: false,
                eventid: 0, siteid: 0, sortorder: 0, isenabled: false,
                alertMessage: "", isSuccess: false, isFailure: false
                , eventsiteidmappings: action.eventsiteidmappings, siteidmapid: 0, sitedata: state.sitedata
            };
        case "REQUEST_EVENT_SITEID_MAP_DATA":
            return {
                isLoading: false,
                eventid: 0, siteid: 0, sortorder: 0, isenabled: false,
                alertMessage: "", isSuccess: false, isFailure: false
                , eventsiteidmappings: state.eventsiteidmappings, siteidmapid: 0, sitedata: state.sitedata
            };
        case "UPDATE_EVENT_SITEID_MAP_STATE":            
            return {
                isLoading: true,
                eventid: state.eventid, siteid: state.siteid, sortorder: state.sortorder,
                isenabled: state.isenabled, alertMessage: "", isSuccess: false, isFailure: false
                , eventsiteidmappings: state.eventsiteidmappings, siteidmapid: state.siteidmapid, sitedata: state.sitedata
            }
        default:
            const exhaustiveCheck: never = action;
    }
    
    return state || unloadedState;
};
