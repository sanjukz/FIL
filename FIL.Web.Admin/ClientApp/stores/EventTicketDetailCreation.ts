import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { eventCreationService } from "../services/EventCreation";
import TicektResponseViewModel from "../models/EventCreation/TicektResponseViewModel";
import SaveEventTicketDetailDataViewModel from "../models/EventCreation/SaveEventTicketDetailDataViewModel";
import EventTicketdetailResponseDataViewModel from "../models/EventCreation/EventTicketdetailResponseDataViewModel";
import EventTicketTypeDataViewModel from "../models/EventCreation/EventTicketTypeDataViewModel";
import ChannelTypeDataViewModel from "../models/EventCreation/ChannelTypeDataViewModel";
import EventTypedDataViewMdel from "../models/EventCreation/EventValueTypeViewModel";
import CurrencyRespnseViewModel from "../models/EventCreation/CurrencyRespnseViewModel";
import { AlertDataViewModel } from "../models/Alert/AlertDataViewModel";
import GetTicketDetailViewModel from "../models/eventCreation/GetTicketDetailViewModel";
import {GetTicketDetailResponseViewModel} from "../models/EventCreation/GetTicketDetailResponseViewModel";
import { IAppThunkAction } from "./";

export const SAVE_EVENT_TICKET_DETAIL_REQUEST = "saveEventTicketDetailRequestAction";
export const SAVE_EVENT_TICKET_DETAIL_SUCCESS = "saveEventTicketDetailSuccessAction";
export const SAVE_EVENT_TICKET_DETAIL_FAILURE = "saveEventTicketDetailFailure";

export interface IEventTicketDetailprops {
    eventTicketDetailCreation: IEventTicketDetailComponentState;
}


export interface IEventTicketDetailComponentState {
    tickets?: ITicket;
    ticketfetchsuccess?: boolean;
    eventTicketDetailFetchsuccess?: boolean;
    eventTicketType?: EventTicketTypeDataViewModel;
    eventChannelType?: ChannelTypeDataViewModel;
    eventTicketTypefetchsuccess?: boolean;
    eventChannelTypefetchsuccess?: boolean;
    eventValutype?: EventTypedDataViewMdel;
    eventvalueTypeFetchsuccess?: boolean;
    currencyType?: ICurrency;
    currencyFetchSuccess?: boolean;
    alertMessage?: AlertDataViewModel;
    getTicketDetailData?: GetTicketDetailResponseViewModel;
    getTicketDetailFetchSucess?: boolean
}

const eventTicketType: EventTicketTypeDataViewModel = {
    ticketTypes: [],
};

const eventChannelType: ChannelTypeDataViewModel = {
    channels: [],
};

const eventValutype: EventTypedDataViewMdel = {
    valueTypes: [],
}

const emptyTickets: ITicket = {
    ticektCategories: []
}

const emptyCurrencies: ICurrency = {
    currencies: []
}

export interface ITicket {
    ticektCategories: TicektResponseViewModel[]
}

export interface ICurrency {
    currencies: CurrencyRespnseViewModel[]
}

const initialAlert: AlertDataViewModel = {
    success: false,
    subject: "",
    body: "",
};

const eventTicketCategoryData: GetTicketDetailResponseViewModel = {
    eventTicketAttribute: [],
    eventTicketDetail: [],
    ticketCategory: [],
    ticketFeeDetail: []
};


const DefaultEventCategoriesCategories: IEventTicketDetailComponentState = {
    tickets: emptyTickets,
    ticketfetchsuccess: false,
    eventTicketDetailFetchsuccess: false,
    eventTicketType: eventTicketType,
    eventChannelType: eventChannelType,
    eventValutype: eventValutype,
    eventvalueTypeFetchsuccess: false,
    currencyType: emptyCurrencies,
    currencyFetchSuccess: false,
    eventTicketTypefetchsuccess: false,
    eventChannelTypefetchsuccess: false,
    alertMessage: initialAlert
}

interface IRequestTicketsDataAction {
    type: "REQUEST_TICKETS_DATA";
}

interface IReceiveTicketsDataAction {
    type: "RECEIVE_TICKETS_DATA";
    ticektCategories: ITicket;
}

interface ISaveEventTicketDetailRequestAction {
    type: "saveEventTicketDetailRequestAction";
}

interface ISaveEventTicketDetailSuccesstAction {
    type: "saveEventTicketDetailSuccessAction";
    success: boolean;
    alertMessage: AlertDataViewModel;
}

interface ISaveEventTicketDetailFailureAction {
    type: "saveEventTicketDetailFailure";
    error: any;
    alertMessage: AlertDataViewModel;
}


interface IRequestGetTicketTypeListAction {
    type: "EVENT_TICKETTYPE_REQUEST";
}

interface IReceiveGetTicketTypeListListAction {
    type: "EVENT_TICKETTYPE_SUCCESS";
    ticketTypes: EventTicketTypeDataViewModel;
}

interface IEventGetTicketTypeListListFailureAction {
    type: "EVENT_TICKETTYPE_FAILURE";
    errors: any;
}

interface IRequestGetChannelTypeListAction {
    type: "EVENT_CHANNELTYPE_REQUEST";
}

interface IReceiveGetChannelTypeListListAction {
    type: "EVENT_CHANNELTYPE_SUCCESS";
    channels: ChannelTypeDataViewModel;
}

interface IEventGetChannelypeListListFailureAction {
    type: "EVENT_EVENT_CHANNELTYPE_FAILURE";
    errors: any;
}

interface IRequestEventValueTypeListAction {
    type: "EVENT_ValueType_REQUEST";
}

interface IReceiveValueTypeListListAction {
    type: "EVENT_ValueType_SUCCESS";
    valueTypes: EventTypedDataViewMdel;
}

interface IEventGetValueTypeListFailureAction {
    type: "EVENT_ValueType_FAILURE";
    errors: any;
}

interface IRequestCurrencyListAction {
    type: "EVENT_Currency_REQUEST";
}

interface IReceiveCurrencyListAction {
    type: "EVENT_Currency_SUCCESS";
    currencies: ICurrency;
}

interface IEventGetCurrencyFailureAction {
    type: "EVENT_Currency_FAILURE";
    errors: any;
}

interface IRequestTicketDetailData {
    type: "USERS_EventTicketDetail_REQUEST";
}

interface IReceiveTicketDetailData {
    type: "USERS_EventTicketDetail_SUCCESS";
    ticketData: GetTicketDetailResponseViewModel;
}

type KnownAction = IRequestTicketsDataAction | IReceiveTicketsDataAction | ISaveEventTicketDetailRequestAction | ISaveEventTicketDetailSuccesstAction
    | ISaveEventTicketDetailFailureAction | IRequestGetTicketTypeListAction | IReceiveGetTicketTypeListListAction | IEventGetTicketTypeListListFailureAction |
    IRequestGetChannelTypeListAction | IReceiveGetChannelTypeListListAction | IEventGetChannelypeListListFailureAction |
    IRequestEventValueTypeListAction | IReceiveValueTypeListListAction | IEventGetValueTypeListFailureAction | IRequestCurrencyListAction
    | IReceiveCurrencyListAction | IEventGetCurrencyFailureAction | IRequestTicketDetailData | IReceiveTicketDetailData

export const actionCreators = {
    requestTicketType: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/eventtickettype/all`)
            .then((response) => response.json() as Promise<EventTicketTypeDataViewModel>)
            .then((data) => {
                dispatch({ type: "EVENT_TICKETTYPE_SUCCESS", ticketTypes: data, });
            },
            (error) => {
                dispatch({ type: "EVENT_TICKETTYPE_FAILURE", errors: error });
            },
        );
        addTask(fetchTask);
        dispatch({ type: "EVENT_TICKETTYPE_REQUEST" });
    },

    requestChannelType: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/channeltype/all`)
            .then((response) => response.json() as Promise<ChannelTypeDataViewModel>)
            .then((data) => {
                dispatch({ type: "EVENT_CHANNELTYPE_SUCCESS", channels: data, });
            },
            (error) => {
                dispatch({ type: "EVENT_EVENT_CHANNELTYPE_FAILURE", errors: error });
            },
        );
        addTask(fetchTask);
        dispatch({ type: "EVENT_CHANNELTYPE_REQUEST" });
    },

    requestValueType: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/eventvalueType/all`)
            .then((response) => response.json() as Promise<EventTypedDataViewMdel>)
            .then((data) => {
                dispatch({ type: "EVENT_ValueType_SUCCESS", valueTypes: data, });
            },
            (error) => {
                dispatch({ type: "EVENT_ValueType_FAILURE", errors: error });
            },
        );
        addTask(fetchTask);
        dispatch({ type: "EVENT_ValueType_REQUEST" });
    },

    requestCurrencyType: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/CurrencyId/all`)
            .then((response) => response.json() as Promise<ICurrency>)
            .then((data) => {
                dispatch({ type: "EVENT_Currency_SUCCESS", currencies: data, });
            },
            (error) => {
                dispatch({ type: "EVENT_Currency_FAILURE", errors: error });
            },
        );
        addTask(fetchTask);
        dispatch({ type: "EVENT_Currency_REQUEST" });
    },

    requesttickets: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/TicketCategory/all`)
            .then((response) => response.json() as Promise<ITicket>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TICKETS_DATA", ticektCategories: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_TICKETS_DATA" });
    },

    saveEventTicketDetail: (eventTicketDetailData: SaveEventTicketDetailDataViewModel, callback: (EventTicketdetailResponseDataViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: SAVE_EVENT_TICKET_DETAIL_REQUEST });
            eventCreationService.saveEventTicketDetailData(eventTicketDetailData)
                .then((response: EventTicketdetailResponseDataViewModel) => {
                    if (response.success) {
                        var alertModel: AlertDataViewModel = {
                            success: true,
                            subject: "EventDetail saved successfully",
                            body: "EventDetail saved successfully",
                        };
                    }
                    dispatch({ type: SAVE_EVENT_TICKET_DETAIL_SUCCESS, success: response.success, alertMessage: alertModel });
                    callback(response);
                },
                (error) => {
                    var alertModel: AlertDataViewModel = {
                        success: false,
                        subject: "EventDetail Save failed",
                        body: error,
                    };
                    dispatch({ type: SAVE_EVENT_TICKET_DETAIL_FAILURE, alertMessage: alertModel, error: error });
                });
        },

    geEventTicketDetailData: (eventDetailId: GetTicketDetailViewModel, callback: (GetTicketDetailResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "USERS_EventTicketDetail_REQUEST" });
        eventCreationService.getEventTicketDetailData(eventDetailId)
            .then(
            (ticketData: GetTicketDetailResponseViewModel) => {
                dispatch({ type: "USERS_EventTicketDetail_SUCCESS", ticketData: ticketData });
                callback(ticketData);
            },
        );
    },
}

export const reducer: Reducer<IEventTicketDetailComponentState> = (state: IEventTicketDetailComponentState, action: KnownAction) => {
    switch (action.type) {
        case SAVE_EVENT_TICKET_DETAIL_REQUEST:
            return {
                errors: {},
                success: false,
                alertMessage: initialAlert,
                errorMessage: "",
                EventDetailSaveSuccessful: false,
                tickets: state.tickets,
                ticketfetchsuccess: false,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case SAVE_EVENT_TICKET_DETAIL_SUCCESS:
            return {
                success: action.success,
                errors: {},
                alertMessage: action.alertMessage,
                errorMessage: "",
                EventDetailSaveSuccessful: true,
                tickets: state.tickets,
                ticketfetchsuccess: false,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case SAVE_EVENT_TICKET_DETAIL_FAILURE:
            return {
                errors: {},
                success: false,
                alertMessage: action.alertMessage,
                errorMessage: action.error,
                EventDetailSaveSuccessful: false,
                tickets: state.tickets,
                ticketfetchsuccess: false,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "REQUEST_TICKETS_DATA":
            return {
                tickets: state.tickets,
                fetchvenueSuccess: false,
                isLoading: true,
                alertMessage: initialAlert,
                errorMessage: "",
                ticketfetchsuccess: false,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "RECEIVE_TICKETS_DATA":
            return {
                tickets: action.ticektCategories,
                fetchvenueSuccess: false,
                isLoading: false,
                alertMessage: initialAlert,
                errorMessage: "",
                ticketfetchsuccess: true,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };


        case "EVENT_TICKETTYPE_REQUEST":
            return {
                eventTicketType: state.eventTicketType,
                fetchEventCategoriesSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                tickets: state.tickets,
                eventTicketTypefetchsuccess: state.eventChannelTypefetchsuccess,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                eventChannelType: state.eventChannelType,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess

            };
        case "EVENT_TICKETTYPE_SUCCESS":
            return {
                eventTicketType: action.ticketTypes,
                fetchEventCategoriesSuccess: true,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                subEventFetchSuccess: false,
                tickets: state.tickets,
                ticketfetchsuccess: true,
                eventTicketTypefetchsuccess: true,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                eventChannelType: state.eventChannelType,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "EVENT_TICKETTYPE_FAILURE":
            return {
                eventTicketType: state.eventTicketType,
                fetchEventCategoriesSuccess: false,
                errors: action.errors,
                alertMessage: initialAlert,
                errorMessage: "",
                tickets: state.tickets,
                eventTicketTypefetchsuccess: false,
                ticketfetchsuccess: false,
                eventChannelTypefetchsuccess: false,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                eventChannelType: state.eventChannelType,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };

        case "EVENT_CHANNELTYPE_REQUEST":
            return {
                eventChannelType: state.eventChannelType,
                fetchEventCategoriesSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                eventChannelTypefetchsuccess: false,
                tickets: state.tickets,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "EVENT_CHANNELTYPE_SUCCESS":
            return {
                eventChannelType: action.channels,
                fetchEventCategoriesSuccess: true,
                fetchEventTypeSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                eventChannelTypefetchsuccess: true,
                tickets: state.tickets,
                ticketfetchsuccess: true,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "EVENT_EVENT_CHANNELTYPE_FAILURE":
            return {
                eventChannelType: state.eventChannelType,
                eventTicketType: state.eventTicketType,
                fetchEventCategoriesSuccess: false,
                errors: action.errors,
                alertMessage: initialAlert,
                errorMessage: "",
                tickets: state.tickets,
                eventTicketTypefetchsuccess: false,
                eventChannelTypefetchsuccess: false,
                ticketfetchsuccess: false,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };

        case "EVENT_ValueType_REQUEST":
            return {
                eventValutype: state.eventValutype,
                fetchEventCategoriesSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                tickets: state.tickets,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventvalueTypeFetchsuccess: false,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess

            };
        case "EVENT_ValueType_SUCCESS":
            return {
                eventValutype: action.valueTypes,
                fetchEventCategoriesSuccess: true,
                fetchEventTypeSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                tickets: state.tickets,
                eventvalueTypeFetchsuccess: true,
                ticketfetchsuccess: true,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "EVENT_ValueType_FAILURE":
            return {
                eventValutype: state.eventValutype,
                eventChannelType: state.eventChannelType,
                eventTicketType: state.eventTicketType,
                fetchEventCategoriesSuccess: false,
                errors: action.errors,
                alertMessage: initialAlert,
                errorMessage: "",
                tickets: state.tickets,
                eventTicketTypefetchsuccess: false,
                eventChannelTypefetchsuccess: false,
                ticketfetchsuccess: false,
                eventvalueTypeFetchsuccess: false,
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "EVENT_Currency_REQUEST":
            return {
                currencyType: state.currencyType,
                fetchEventCategoriesSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                currencyFetchSuccess: false,
                tickets: state.tickets,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };

        case "EVENT_Currency_SUCCESS":
            return {
                currencyType: action.currencies,
                fetchEventCategoriesSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                currencyFetchSuccess: true,
                tickets: state.tickets,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };

        case "EVENT_Currency_FAILURE":
            return {
                currencyType: state.currencyType,
                fetchEventCategoriesSuccess: false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                currencyFetchSuccess: false,
                tickets: state.tickets,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess: state.getTicketDetailFetchSucess
            };
        case "USERS_EventTicketDetail_REQUEST":
            return {
                getTicketDetailData: state.getTicketDetailData,
                getTicketDetailFetchSucess:false,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                currencyType: state.currencyType,  
                currencyFetchSuccess: state.currencyFetchSuccess,
                tickets: state.tickets,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
            }
        case "USERS_EventTicketDetail_SUCCESS":
            return {
                getTicketDetailData: action.ticketData,
                getTicketDetailFetchSucess: true,
                errors: {},
                alertMessage: initialAlert,
                errorMessage: "",
                currencyType: state.currencyType,
                currencyFetchSuccess: state.currencyFetchSuccess,
                tickets: state.tickets,
                ticketfetchsuccess: state.ticketfetchsuccess,
                eventTicketType: state.eventTicketType,
                eventTicketTypefetchsuccess: state.eventTicketTypefetchsuccess,
                eventChannelType: state.eventChannelType,
                eventChannelTypefetchsuccess: state.eventChannelTypefetchsuccess,
                eventValutype: state.eventValutype,
                eventvalueTypeFetchsuccess: state.eventvalueTypeFetchsuccess,
                }
    }
    return state || DefaultEventCategoriesCategories;
}
