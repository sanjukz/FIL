import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "../";
import { ReportFormDataViewModel } from "../../models/ReportingWizard/ReportFormDataViewModel";
import { ReportEventsResponseViewModel } from "../../models/ReportingWIzard/ReportEventsResponseViewModel";
import { ReportSubEventsResponseViewModel } from "../../models/ReportingWIzard/ReportSubEventsResponseViewModel";
import { InventoryReportResponseDataViewModel } from "../../models/ReportingWizard/InventoryReportResponseDataViewModel";
import { reportService } from "../../services/reporting/report";

export interface IInventoryReportEventsProps {
    reportEvents: IInventoryReportState;
}


export interface IInventoryReportState {
    isLoading: boolean;
    fetchEventSuccess: boolean;
    fetchSubEventSuccess: boolean;
    fetchReportSuccess: boolean;
    allEvents: IEvents;
    allSubEvents: ISubEvents;
    invetoryreport: InventoryReportResponseDataViewModel;
    errors?: any;
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

interface IRequestEventReportDataAction {
    type: "REQUEST_REPORT_DATA";
}

interface IReceiveEventReportDataAction {
    type: "RECEIVE_REPORT_DATA";
    reportData: InventoryReportResponseDataViewModel;
}

interface IFailureEventReportDataAction {
    type: "RECEIVE_REPORT_DATA_FAILURE";
    errors: any;
}

interface IRefreshDataAction {
    type: "REQUEST_REFRESH_DATA";
}

type KnownAction = IRequestEventDataAction | IReceiveEventDataAction | IRequestSubEventDataAction | IReceiveSubEventDataAction | IRequestEventReportDataAction | IReceiveEventReportDataAction | IFailureEventReportDataAction | IRefreshDataAction;

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
    getInventoryReport: (values: ReportFormDataViewModel, callback: (InventoryReportResponseDataViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_REPORT_DATA" });
        reportService.getInventoryReport(values)
            .then(
                (response: InventoryReportResponseDataViewModel) => {
                    dispatch({ type: "RECEIVE_REPORT_DATA", reportData: response });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: "RECEIVE_REPORT_DATA_FAILURE", errors: error });
                },
        );
    },
};

const emptyEvent: IEvents = {
    events: []
};

const emptySubEvent: ISubEvents = {
    subEvents: []
};

const emptyEventReport: InventoryReportResponseDataViewModel = {
    ticketCategory: [],
    eventTicketAttribute: [],
    eventTicketDetail: [],
    corporateTicketAllocationDetail: [],
    corporateTransactionDetail: [],
    sponsor: [],
    transaction: [],
    transactionDetail: [],
    reportColumns: []
};


const unloadedState: IInventoryReportState = {
    allEvents: emptyEvent, allSubEvents: emptySubEvent, invetoryreport: emptyEventReport, isLoading: false, fetchEventSuccess: false, fetchSubEventSuccess: false, fetchReportSuccess: false, errors: null
}

export const reducer: Reducer<IInventoryReportState> = (state: IInventoryReportState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_EVENT_DATA":
            return {
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                invetoryreport: state.invetoryreport,
                fetchEventSuccess: false,
                fetchSubEventSuccess: false,
                fetchReportSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_EVENT_DATA":
            return {
                allEvents: action.allEvents,
                allSubEvents: state.allSubEvents,
                invetoryreport: state.invetoryreport,
                fetchEventSuccess: true,
                fetchSubEventSuccess: false,
                fetchReportSuccess: false,
                isLoading: false,
                errors: {}
            };
        case "REQUEST_SUBEVENT_DATA":
            return {
                allSubEvents: state.allSubEvents,
                allEvents: state.allEvents,
                invetoryreport: state.invetoryreport,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: false,
                fetchReportSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_SUBEVENT_DATA":
            return {
                allSubEvents: action.allSubEvents,
                allEvents: state.allEvents,
                invetoryreport: state.invetoryreport,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: true,
                fetchReportSuccess: false,
                isLoading: false,
                errors: {}
            };
        case "REQUEST_REPORT_DATA":
            return {
                invetoryreport: state.invetoryreport,
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: state.fetchSubEventSuccess,
                fetchReportSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_REPORT_DATA":
            return {
                invetoryreport: action.reportData,
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: state.fetchSubEventSuccess,
                fetchReportSuccess: true,
                isLoading: false,
                errors: {}
            };
        case "RECEIVE_REPORT_DATA_FAILURE":
            return {
                invetoryreport: state.invetoryreport,
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: state.fetchSubEventSuccess,
                fetchReportSuccess: false,
                isLoading: false,
                errors: action.errors
            };
        case "REQUEST_REFRESH_DATA":
            return {
                allSubEvents: emptySubEvent,
                allEvents: state.allEvents,
                invetoryreport: emptyEventReport,
                valEvent: "",
                valSubEvent: "",
                fetchEventSuccess: true,
                fetchSubEventSuccess: false,
                valFromDate: "",
                valToDate: "",
                fetchReportSuccess: false,
                isLoading: false,
                errors: {}
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};