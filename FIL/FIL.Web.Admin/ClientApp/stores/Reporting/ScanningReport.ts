import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "../";
import { ReportEventsResponseViewModel } from "../../models/ReportingWIzard/ReportEventsResponseViewModel";
import { ReportSubEventsResponseViewModel } from "../../models/ReportingWIzard/ReportSubEventsResponseViewModel";
import { ReportFormDataViewModel } from "../../models/ReportingWizard/ReportFormDataViewModel";
import { ScanningReportResponseViewModel } from "../../models/ReportingWizard/ScanningReportResponseViewModel";
import { reportService } from "../../services/reporting/report";

export interface IScanningReportProps {
    reportEvents: IScanningReportComponentState;
}

export interface IScanningReportComponentState {
    userAltId?: any;
    isLoading: boolean;
    fetchEventSuccess: boolean;
    fetchSubEventSuccess: boolean;
    fetchReportSuccess: boolean;
    valEvent?: any;
    valSubEvent?: any;
    valFromDate?: any;
    valToDate?: any;
    errors?: any;
    reportData?: any;
    allEvents: IEvents;
    allSubEvents: ISubEvents;
    scanningReport: ScanningReportResponseViewModel;
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
    reportData: ScanningReportResponseViewModel;
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
    getScanningReport: (values: ReportFormDataViewModel, callback: (ScanningReportResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_REPORT_DATA" });
        reportService.getScanningReport(values)
            .then(
                (response: ScanningReportResponseViewModel) => {
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

const emptyEventReport: ScanningReportResponseViewModel = {
    transaction: [],
    transactionDetail: [],
    matchSeatTicketDetail: [],
    eventTicketDetail: [],
    eventDetail: [],
    eventAttribute: [],
    event: [],
    ticketCategory: [],
    reportColumns: []
};


const unloadedState: IScanningReportComponentState = {
    allEvents: emptyEvent, allSubEvents: emptySubEvent, scanningReport: emptyEventReport, isLoading: false, fetchEventSuccess: false, fetchSubEventSuccess: false, fetchReportSuccess: false, errors: null
}

export const reducer: Reducer<IScanningReportComponentState> = (state: IScanningReportComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_EVENT_DATA":
            return {
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                scanningReport: state.scanningReport,
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
                scanningReport: state.scanningReport,
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
                scanningReport: state.scanningReport,
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
                scanningReport: state.scanningReport,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: true,
                fetchReportSuccess: false,
                isLoading: false,
                errors: {}
            };
        case "REQUEST_REPORT_DATA":
            return {
                scanningReport: state.scanningReport,
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
                scanningReport: action.reportData,
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
                scanningReport: state.scanningReport,
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
                scanningReport: emptyEventReport,
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