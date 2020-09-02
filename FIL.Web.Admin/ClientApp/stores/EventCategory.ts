import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { EventCategoryMappingViewModel } from "../models/EventCategoryMappingViewModel";
export interface IEventCategoryComponentState {
    isLoading: boolean;
    eventcategories: IEventCategories;
    eventcategorymappings: IEventCategoryMappings;
    eventid: number;
    parentcategoryid: number;
    subcategoryid: number;
    isenabled: boolean;
    alertMessage: string;
    isSuccess: boolean;
    isFailure: boolean;
    eventCategoryMapId: number;
}

export interface ICategory {
    categoryId: number;
    displayName: string;
    isHomePage: boolean;
    order: number;
    slug: string;
    value: number;
}



export interface IEventCategoryMap {
    id: number;
    eventCategoryId: number;
    eventId: number;
    isEnabled: number;
    eventName: string;
    categoryName: string;
}
export interface IEventCategoryMappings {    
    eventcatmappings: IEventCategoryMap[];
}

export interface IEventCategories {
    categories: ICategory[];
}

interface IRequestEventDataAction {
    type: "REQUEST_EVENT_CATEGORY_DATA";
}

interface IReceiveEventDataAction {
    type: "RECEIVE_EVENT_CATEGORY_DATA";
    eventcategories: IEventCategories;
}

interface IRequestEventCategoryMapDataAction {
    type: "REQUEST_EVENT_CATEGORY_MAP_DATA";
}

interface IReceiveEventCategoryMapDataAction {
    type: "RECEIVE_EVENT_CATEGORY_MAP_DATA";
    eventcatmappings: IEventCategoryMappings;
}



interface IRequestUpdateEventCategoryMapAction {
    type: "REQUEST_UPDATE_EVENT_CATEGORY_MAP";
}
interface IUpdatedEventCategoryMapAction {
    type: "UPDATED_EVENT_CATEGORY_MAP";
}
interface IFailedEventCategoryMapAction {
    type: "FAILED_EVENT_CATEGORY_MAP";
}


interface IUpdateEventCategoryMapState {
    type: "UPDATE_EVENT_CATEGORY_MAP_STATE";
    payload : 
    { 
        eventid: number;
        parentcategoryid: number;
        subcategoryid: number;
        isenabled: boolean; 
        eventCategoryMapId: number;
    };
}

type KnownAction = IRequestEventDataAction | IReceiveEventDataAction | IRequestUpdateEventCategoryMapAction | IUpdatedEventCategoryMapAction | IFailedEventCategoryMapAction
| IReceiveEventCategoryMapDataAction | IRequestEventCategoryMapDataAction | IUpdateEventCategoryMapState;

export const actionCreators = {
    requestEventdata: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`/api/event/categories`)
            .then((response) => response.json() as Promise<IEventCategories>)
            .then((data) => {
                dispatch({ type: "RECEIVE_EVENT_CATEGORY_DATA", eventcategories: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_CATEGORY_DATA" });
    },
    requestEventCategoryMappingdata: (eventId): IAppThunkAction<KnownAction> => (dispatch, getState) => { 
               
        const fetchTask = fetch(`/api/event/categorymapping/${eventId}`)
            .then((response) => response.json() as Promise<IEventCategoryMappings>)
            .then((data) => {
                localStorage.setItem("datamap", JSON.stringify(data))
                dispatch({ type: "RECEIVE_EVENT_CATEGORY_MAP_DATA", eventcatmappings: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_CATEGORY_MAP_DATA" });
    },
    updateEventCategory: (eventcatModel: EventCategoryMappingViewModel, callback: (EventCategoryMappingViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {

        
        const fetchTask = fetch(`/api/event/map`, {
            method: 'POST',
            body: JSON.stringify(eventcatModel),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then((response: any) => {
                if (response.ok) {                    
                    dispatch({ type: "UPDATED_EVENT_CATEGORY_MAP" });
                }
                else if (!response.ok) {
                    dispatch({ type: "FAILED_EVENT_CATEGORY_MAP" });
                }
                callback(response);
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_CATEGORY_DATA" });
    },
    updateEventCategoryMappingState: (eventcatModel, callback: (EventCategoryMappingViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
                
        dispatch({ type: "UPDATE_EVENT_CATEGORY_MAP_STATE",payload : 
            { 
                eventid: eventcatModel.original.eventId,
                parentcategoryid: 0,
                subcategoryid: eventcatModel.original.eventCategoryId,
                isenabled: eventcatModel.original.isEnabled,
                eventCategoryMapId: eventcatModel.original.id
            } 
        });
    }
};

const emptyeventcategories: IEventCategories = {
    categories: []
};
const emptyeventcategorymappings: IEventCategoryMappings = {
    eventcatmappings: []
};

const unloadedState: IEventCategoryComponentState = {
    eventcategories: emptyeventcategories, isLoading: false, eventid: 0, parentcategoryid: 0,
     subcategoryid: 0, isenabled: false, alertMessage: "", isSuccess : false, isFailure:false,
     eventcategorymappings: emptyeventcategorymappings, eventCategoryMapId: 0
}

export const reducer: Reducer<IEventCategoryComponentState> = (state: IEventCategoryComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_EVENT_CATEGORY_DATA":
            return {
                eventcategories: state.eventcategories,
                isLoading: true,
                eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false,
                alertMessage: "", isSuccess : false, isFailure:false
                , eventcategorymappings: state.eventcategorymappings, eventCategoryMapId: 0
            };
        case "RECEIVE_EVENT_CATEGORY_DATA":
            return {
                eventcategories: action.eventcategories,
                isLoading: false,
                eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false,
                alertMessage: "", isSuccess : false, isFailure:false
                , eventcategorymappings: state.eventcategorymappings, eventCategoryMapId: 0
            };
        case "REQUEST_UPDATE_EVENT_CATEGORY_MAP":
            return {
                isLoading: true,
                eventcategories: state.eventcategories,
                eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false,
                alertMessage: "", isSuccess : false, isFailure:false
                , eventcategorymappings: state.eventcategorymappings, eventCategoryMapId: 0
            };
        case "UPDATED_EVENT_CATEGORY_MAP":
            return {
                isLoading: false,
                eventcategories: state.eventcategories,
                eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false,
                alertMessage: "Successfully updated", isSuccess : true, isFailure:false
                , eventcategorymappings: state.eventcategorymappings, eventCategoryMapId: 0
            };
        case "FAILED_EVENT_CATEGORY_MAP":
            return {
                isLoading: false,
                eventcategories: state.eventcategories,
                eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false,
                alertMessage: "Update failure", isSuccess : false, isFailure:true
                , eventcategorymappings: state.eventcategorymappings, eventCategoryMapId: 0
            };
        case "RECEIVE_EVENT_CATEGORY_MAP_DATA":
            return {
                isLoading: false,
                eventcategories: state.eventcategories,
                eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false,
                alertMessage: "", isSuccess : false, isFailure:false
                , eventcategorymappings: JSON.parse(localStorage.getItem("datamap")), eventCategoryMapId: 0
            };
        case "REQUEST_EVENT_CATEGORY_MAP_DATA":
            return {
                isLoading: false,
                eventcategories: state.eventcategories,
                eventid: 0, parentcategoryid: 0, subcategoryid: 0, isenabled: false,
                alertMessage: "", isSuccess : false, isFailure:false
                , eventcategorymappings: state.eventcategorymappings, eventCategoryMapId: 0
            };
        case "UPDATE_EVENT_CATEGORY_MAP_STATE":
            return {
                isLoading: true,
                eventcategories: state.eventcategories,
                eventid: state.eventid, parentcategoryid: state.parentcategoryid, subcategoryid: state.subcategoryid,
                isenabled: state.isenabled, alertMessage: "", isSuccess : false, isFailure:false
                , eventcategorymappings: state.eventcategorymappings, eventCategoryMapId: state.eventCategoryMapId
            }
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
