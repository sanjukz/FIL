import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "../";
import { ReportEventsResponseViewModel } from "../../models/ReportingWIzard/ReportEventsResponseViewModel";
import { ReportSubEventsResponseViewModel } from "../../models/ReportingWIzard/ReportSubEventsResponseViewModel";
import { PaymentGatewayResponseViewModel } from "../../models/ReportingWIzard/PaymentGatewayResponseViewModel";
import { FailedTransactionReportFormDataViewModel } from "../../models/ReportingWizard/FailedTransactionReportFormDataViewModel";
import { ReportResponseDataViewModel } from "../../models/ReportingWizard/ReportResponseDataViewModel";
import { reportService } from "../../services/reporting/report";

export interface IFailedReportEventsProps {
    reportEvents: IFailedTransactionReportComponentState;
}

export interface IFailedTransactionReportComponentState {
    userAltId?: any;
    isLoading: boolean;
    fetchEventSuccess: boolean;
    fetchSubEventSuccess: boolean;
    fetchPGSuccess: boolean;
    fetchReportSuccess: boolean;
    valEvent?: any;
    valSubEvent?: any;
    valPaymentGateway?: any;
    valFromDate?: any;
    valToDate?: any;
    errors?: any;
    reportData?: any;
    allEvents: IEvents;
    allSubEvents: ISubEvents;
    allPaymentGateways: IPaymentGateways;
    transactionreport: ReportResponseDataViewModel;
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

export interface IPaymentGateways {
    paymentGateways: PaymentGatewayResponseViewModel[];
}

interface IRequestPaymnetGatewaysDataAction {
    type: "REQUEST_PAYMENTGATEWAYS_DATA";
}

interface IReceivePaymnetGatewaysDataAction {
    type: "RECEIVE_PAYMENTGATEWAYS_DATA";
    allPaymentGateways: IPaymentGateways;
}

interface IRequestEventReportDataAction {
    type: "REQUEST_REPORT_DATA";
}

interface IReceiveEventReportDataAction {
    type: "RECEIVE_REPORT_DATA";
    reportData: ReportResponseDataViewModel;
}

interface IFailureEventReportDataAction {
    type: "RECEIVE_REPORT_DATA_FAILURE";
    errors: any;
}

interface IRefreshDataAction {
    type: "REQUEST_REFRESH_DATA";
}

type KnownAction = IRequestEventDataAction | IReceiveEventDataAction | IRequestSubEventDataAction | IReceiveSubEventDataAction | IRequestEventReportDataAction | IReceiveEventReportDataAction | IFailureEventReportDataAction | IRefreshDataAction |
    IRequestPaymnetGatewaysDataAction | IReceivePaymnetGatewaysDataAction;

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
    requestPaymentGateways: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/paymentgateway/all`)
            .then((response) => response.json() as Promise<IPaymentGateways>)
            .then((data) => {
                dispatch({ type: "RECEIVE_PAYMENTGATEWAYS_DATA", allPaymentGateways: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_PAYMENTGATEWAYS_DATA" });
    },
    getFailedTransactionReport: (values: FailedTransactionReportFormDataViewModel, callback: (ReportResponseDataViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_REPORT_DATA" });
        reportService.getFailedTransactionReport(values)
            .then(
                (response: ReportResponseDataViewModel) => {
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

const emptyPaymentGateways: IPaymentGateways = {
    paymentGateways: []
};

const emptyEventReport: ReportResponseDataViewModel = {
    transaction: [],
    transactionDetail: [],
    transactionDeliveryDetail: [],
    transactionPaymentDetail: [],
    currencyType: [],
    eventTicketAttribute: [],
    eventTicketDetail: [],
    ticketCategory: [],
    event: [],
    eventDetail: [],
    eventAttribute: [],
    venue: [],
    city: [],
    state: [],
    country: [],
    user: [],
    userCardDetail: [],
    reportColumns: [],
    ipDetail: [],
    ticketFeeDetail: [],
};


const unloadedState: IFailedTransactionReportComponentState = {
    allEvents: emptyEvent, allSubEvents: emptySubEvent, allPaymentGateways: emptyPaymentGateways, transactionreport: emptyEventReport, isLoading: false, fetchEventSuccess: false, fetchSubEventSuccess: false, fetchPGSuccess: false, fetchReportSuccess: false, errors: null
}

export const reducer: Reducer<IFailedTransactionReportComponentState> = (state: IFailedTransactionReportComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_EVENT_DATA":
            return {
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                allPaymentGateways: state.allPaymentGateways,
                transactionreport: state.transactionreport,
                fetchEventSuccess: false,
                fetchSubEventSuccess: false,
                fetchPGSuccess: false,
                fetchReportSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_EVENT_DATA":
            return {
                allEvents: action.allEvents,
                allSubEvents: state.allSubEvents,
                allPaymentGateways: state.allPaymentGateways,
                transactionreport: state.transactionreport,
                fetchEventSuccess: true,
                fetchSubEventSuccess: false,
                fetchPGSuccess: false,
                fetchReportSuccess: false,
                isLoading: false,
                errors: {}
            };
        case "REQUEST_SUBEVENT_DATA":
            return {
                allSubEvents: state.allSubEvents,
                allEvents: state.allEvents,
                allPaymentGateways: state.allPaymentGateways,
                transactionreport: state.transactionreport,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: false,
                fetchPGSuccess: false,
                fetchReportSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_SUBEVENT_DATA":
            return {
                allSubEvents: action.allSubEvents,
                allEvents: state.allEvents,
                allPaymentGateways: state.allPaymentGateways,
                transactionreport: state.transactionreport,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: true,
                fetchPGSuccess: false,
                fetchReportSuccess: false,
                isLoading: false,
                errors: {}
            };
        case "REQUEST_PAYMENTGATEWAYS_DATA":
            return {
                allSubEvents: state.allSubEvents,
                allEvents: state.allEvents,
                allPaymentGateways: state.allPaymentGateways,
                transactionreport: state.transactionreport,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchPGSuccess: false,
                fetchSubEventSuccess: false,
                fetchReportSuccess: false,
                isLoading: state.isLoading,
                errors: {}
            };
        case "RECEIVE_PAYMENTGATEWAYS_DATA":
            return {
                allPaymentGateways: action.allPaymentGateways,
                allSubEvents: state.allSubEvents,
                allEvents: state.allEvents,
                transactionreport: state.transactionreport,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchPGSuccess: true,
                fetchSubEventSuccess: false,
                fetchReportSuccess: false,
                isLoading: state.isLoading,
                errors: {}
            };
        case "REQUEST_REPORT_DATA":
            return {
                transactionreport: state.transactionreport,
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                allPaymentGateways: state.allPaymentGateways,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: state.fetchSubEventSuccess,
                fetchPGSuccess: state.fetchPGSuccess,
                fetchReportSuccess: false,
                isLoading: true,
                errors: {}
            };
        case "RECEIVE_REPORT_DATA":
            return {
                transactionreport: action.reportData,
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                allPaymentGateways: state.allPaymentGateways,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: state.fetchSubEventSuccess,
                fetchPGSuccess: state.fetchPGSuccess,
                fetchReportSuccess: true,
                isLoading: false,
                errors: {}
            };
        case "RECEIVE_REPORT_DATA_FAILURE":
            return {
                transactionreport: state.transactionreport,
                allEvents: state.allEvents,
                allSubEvents: state.allSubEvents,
                allPaymentGateways: state.allPaymentGateways,
                fetchEventSuccess: state.fetchEventSuccess,
                fetchSubEventSuccess: state.fetchSubEventSuccess,
                fetchPGSuccess: state.fetchPGSuccess,
                fetchReportSuccess: false,
                isLoading: false,
                errors: action.errors
            };
        case "REQUEST_REFRESH_DATA":
            return {
                allSubEvents: emptySubEvent,
                allEvents: state.allEvents,
                allPaymentGateways: state.allPaymentGateways,
                transactionreport: emptyEventReport,
                valEvent: "",
                valSubEvent: "",
                fetchEventSuccess: true,
                fetchSubEventSuccess: false,
                fetchPGSuccess: true,
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