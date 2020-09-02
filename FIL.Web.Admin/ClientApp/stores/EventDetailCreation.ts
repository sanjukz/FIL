import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { eventCreationService } from "../services/EventCreation";
import { AddEventDetailViewModel } from "../models/EventCreation/AddEventDetailViewModel";
import { EventDetailResposeViewModel } from "../models/EventCreation/EventDetailResposeViewModel";
import VenueCreationViewModel from "../models/Venues/VenueCreation";
import { AlertDataViewModel } from "../models/Alert/AlertDataViewModel";
import GetsubeventViewModel from "../models/EventCreation/GetsubeventViewModel";
import SubEventDetailDataResponseViewModel from "../models/EventCreation/SubEventDetailDataResponseViewModel";
import { SubEventDeleteViewModel } from "../models/EventCreation/SubEventDeleteViewModel";
import { DeleteSubeventResponseViewModel } from "../models/EventCreation/DeleteSubeventResponseViewModel";
import { IAppThunkAction } from "./";

export const SAVE_EVENT_DETAIL_REQUEST = "saveEventDetailRequestAction";
export const SAVE_EVENT_DETAIL_SUCCESS = "saveEventDetailSuccessAction";
export const SAVE_EVENT_DETAIL_FAILURE = "saveEventDetailFailure";

export interface IEventDetailprops {
    eventDetailCreation: IEventDetailComponentState;
}

export interface IEventDetailComponentState {
    fetchvenueSuccess?: boolean;
    venues?: IVenues;
    eventDetail?: SubEventDetailDataResponseViewModel;
    errors?: any;
    success?: boolean;
    alertMessage?: AlertDataViewModel;
    EventDetailSaveSuccessful?: boolean;
    error?: boolean;
    subEventFetchSuccess?: boolean;
}

export interface IVenues {
    venues: VenueCreationViewModel[];
}

const emptyVenues: IVenues = {
    venues: []
}

const emptySubevents: SubEventDetailDataResponseViewModel = {
    eventDetail: [],
    id:null,
    name:""
}

const initialAlert: AlertDataViewModel = {
    success: false,
    subject: "",
    body: "",
};

const DefaultEventCategoriesCategories: IEventDetailComponentState = {
    success: false,
    venues: emptyVenues,
    fetchvenueSuccess: false,
    alertMessage: initialAlert,
    error: false,
    EventDetailSaveSuccessful: false,
    eventDetail: emptySubevents,
    subEventFetchSuccess: false,
};

interface ISaveEventDetailRequestAction {
    type: "saveEventDetailRequestAction";
}

interface ISaveEventDetailSuccesstAction {
    type: "saveEventDetailSuccessAction";
    success: boolean;
    alertMessage: AlertDataViewModel;
}

interface ISaveEventDetailFailureAction {
    type: "saveEventDetailFailure";
    alertMessage: AlertDataViewModel;
    error: any;
}

interface IRequestVenueDataAction {
    type: "REQUEST_VENUE_DATA";
}

interface IReceiveVenueDataAction {
    type: "RECEIVE_VENUE_DATA";
    venues: IVenues;
}

interface IRequestSubEventDataAction {
    type: "REQUEST_SubEvent_DATA"
}

interface IReceiveSubEventDataAction {
    type: "RECEIVE_SubEvent_DATA"
    eventDetail: SubEventDetailDataResponseViewModel
}


interface IRequestDeleteSubEventAction {
    type: "USERS_DELETE_SubEvent_REQUEST";
}

interface IReceiveDeleteSubEventdAction {
    type: "USERS_DELETE_Subevent_SUCCESS";
    alertMessage: AlertDataViewModel;
}

interface ISubEventFailureAction {
    type: "USERS_DELETE_SubEvent_FAILURE";
    errors: any;
}

type KnownAction = ISaveEventDetailRequestAction | ISaveEventDetailSuccesstAction |
    ISaveEventDetailFailureAction | IRequestVenueDataAction | IReceiveVenueDataAction | IRequestSubEventDataAction |
    IReceiveSubEventDataAction | IRequestDeleteSubEventAction | IReceiveDeleteSubEventdAction | ISubEventFailureAction

export const actionCreators = {
    saveEventDetail: (eventDetailData: AddEventDetailViewModel, callback: (EventDetailResposeViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: SAVE_EVENT_DETAIL_REQUEST });
            eventCreationService.saveEventDetailData(eventDetailData)
                .then((response: EventDetailResposeViewModel) => {
                    if (response.success) {
                        var alertModel: AlertDataViewModel = {
                            success: true,
                            subject: "EventDetail saved successfully",
                            body: "EventDetail saved successfully",
                        };
                    }
                    dispatch({ type: SAVE_EVENT_DETAIL_SUCCESS, success: response.success, alertMessage: alertModel });
                    callback(response);
                },
                (error) => {
                    var alertModel: AlertDataViewModel = {
                        success: false,
                        subject: "EventDetail Save failed",
                        body: error,
                    };
                    dispatch({ type: SAVE_EVENT_DETAIL_FAILURE, alertMessage: alertModel, error: error });
                });
        },

    requestVenues: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/venue/all`)
            .then((response) => response.json() as Promise<IVenues>)
            .then((data) => {
                dispatch({ type: "RECEIVE_VENUE_DATA", venues: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_VENUE_DATA" });
    },

    getSubeventData: (EventAltId: GetsubeventViewModel, callback: (SubEventDetailDataResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "REQUEST_SubEvent_DATA" });
        eventCreationService.getSubEventList(EventAltId)
            .then(
            (data: SubEventDetailDataResponseViewModel) => {
                dispatch({ type: "RECEIVE_SubEvent_DATA", eventDetail: data });
                callback(data);
            },
            (error) => {
            },
        );
    },

    deleteSubeventAction: (deleteEventModel: SubEventDeleteViewModel, callback: (DeleteSubeventResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "USERS_DELETE_SubEvent_REQUEST" });
        eventCreationService.deleteSubevent(deleteEventModel)
            .then(
            (card: DeleteSubeventResponseViewModel) => {
                var alertModel: AlertDataViewModel = {
                    success: true,
                    subject: "Delete Card",
                    body: "Card deleted successfully",
                };
                dispatch({ type: "USERS_DELETE_Subevent_SUCCESS", alertMessage: alertModel });
                callback(card);
            },
            (error) => {
                dispatch({ type: "USERS_DELETE_SubEvent_FAILURE", errors: error });
            },
        );
    },

}

export const reducer: Reducer<IEventDetailComponentState> = (state: IEventDetailComponentState, action: KnownAction) => {
    switch (action.type) {
        case SAVE_EVENT_DETAIL_REQUEST:
            return {
                errors: {},
                success: false,
                venues: state.venues,
                eventDetail: state.eventDetail,
                alertMessage: initialAlert,
                errorMessage: "",
                EventDetailSaveSuccessful: false,
                subEventFetchSuccess: state.subEventFetchSuccess,
                fetchvenueSuccess:state.fetchvenueSuccess
            };
        case SAVE_EVENT_DETAIL_SUCCESS:
            return {
                success: action.success,
                errors: {},
                venues: state.venues,
                eventDetail: state.eventDetail,
                alertMessage: action.alertMessage,
                errorMessage: "",
                EventDetailSaveSuccessful: true,
                subEventFetchSuccess: state.subEventFetchSuccess,
                fetchvenueSuccess:state.fetchvenueSuccess
            };
        case SAVE_EVENT_DETAIL_FAILURE:
            return {
                errors: {},
                success: false,
                venues: state.venues,
                eventDetail: state.eventDetail,
                alertMessage: action.alertMessage,
                errorMessage: action.error,
                EventDetailSaveSuccessful: false,
                subEventFetchSuccess: false,
                fetchvenueSuccess:state.fetchvenueSuccess
            };
            case "REQUEST_VENUE_DATA":
            return {
                fetchvenueSuccess: false,
                venues: state.venues,
                eventDetail: state.eventDetail,
                isLoading: true,
                alertMessage: initialAlert,
                errorMessage: "",
                subEventFetchSuccess: state.subEventFetchSuccess,
            };
        case "RECEIVE_VENUE_DATA":
            return {
                fetchvenueSuccess: true,
                venues: action.venues,
                eventDetail: state.eventDetail,
                isLoading: false,
                alertMessage: initialAlert,
                errorMessage: "",
                subEventFetchSuccess: state.subEventFetchSuccess,
                };
        case "REQUEST_SubEvent_DATA":
            return {
                venues: state.venues,
                eventDetail: state.eventDetail,
                alertMessage: initialAlert,
                errorMessage: "",
                subEventFetchSuccess: false,
                fetchvenueSuccess:state.fetchvenueSuccess,
            };
        case "RECEIVE_SubEvent_DATA":
            return {
                venues: state.venues,
                eventDetail: action.eventDetail,
                alertMessage: initialAlert,
                errorMessage: "",
                subEventFetchSuccess: true,
                fetchvenueSuccess:state.fetchvenueSuccess,
            };
        case "USERS_DELETE_SubEvent_REQUEST":
            return {
                errors: {},
                success: false,
                altId: undefined,
                venues: state.venues,
                eventDetail: state.eventDetail,
                alertMessage: initialAlert,
                errorMessage: "",
                EventSaveSuccessful: false,
                subEventFetchSuccess: false,
            };
        case "USERS_DELETE_Subevent_SUCCESS":
            return {
                errors: {},
                success: false,
                altId: undefined,
                venues: state.venues,
                eventDetail: state.eventDetail,
                alertMessage: initialAlert,
                errorMessage: "",
                EventSaveSuccessful: false,
                subEventFetchSuccess: false,
            };
        case "USERS_DELETE_SubEvent_FAILURE":
            return {
                errors: {},
                success: false,
                altId: undefined,
                venues: state.venues,
                eventDetail: state.eventDetail,
                alertMessage: initialAlert,
                errorMessage: "",
                subEventFetchSuccess: false,
            };
    }
    return state || DefaultEventCategoriesCategories;
}