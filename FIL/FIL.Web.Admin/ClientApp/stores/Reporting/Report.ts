import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "../";
import { ReportEventsResponseViewModel } from "../../models/ReportingWIzard/ReportEventsResponseViewModel";
import { ReportSubEventsResponseViewModel } from "../../models/ReportingWIzard/ReportSubEventsResponseViewModel";
import { ReportFormDataViewModel } from "../../models/ReportingWizard/ReportFormDataViewModel";
import { ReportResponseDataViewModel } from "../../models/ReportingWizard/ReportResponseDataViewModel";
import TransactionReportRequestViewModel from "../../models/Report/TransactionReportRequestViewModel";
import TicketAlertReportViewModel from "../../models/Report/TicketAlertReportViewModel";
import TransactionReportResponseViewModel, { TransactionReportViewModel } from "../../models/Report/TransactionReportResponseViewModel";
import FeelUserReportResponseViewModel, { FeelUserReport } from "../../models/Report/FeeluserReportResponseViewModel";
import { reportService } from "../../services/reporting/report";
import PlacesReponseModel from "../../models/Report/PlacesResponseModel";
import CurrencyType from "../../models/Common/CurrencyTypeViewModel";
import { TransactionLocatorResponseViewModel, FilTransactionLocator } from "../../models/Report/TransactionLocatorResponseViewModel";

export interface IReportEventsProps {
  reportEvents: ITransactionReportComponentState;
}

export interface ITransactionReportComponentState {
  userAltId?: any;
  isLoading: boolean;
  fetchEventSuccess?: boolean;
  fetchSubEventSuccess?: boolean;
  fetchReportSuccess?: boolean;
  valEvent?: any;
  valSubEvent?: any;
  valFromDate?: any;
  valToDate?: any;
  errors?: any;
  reportData?: any;
  allEvents?: IEvents;
  allSubEvents?: ISubEvents;
  transactionreport?: ReportResponseDataViewModel;
  transactionReportSingleDataModel?: TransactionReportResponseViewModel;
  ticketAlertReport?: TicketAlertResponseViewModels;
  feelUsers?: FeelUserReportResponseViewModel;
  fetchAllPlaces?: boolean;
  places?: PlacesReponseModel;
  transactionLocatorResponseViewModel?: TransactionLocatorResponseViewModel;
}

export interface IEvents {
  events: ReportEventsResponseViewModel[];
}

export interface TicketAlertResponseViewModels {
  ticketAlertData: TicketAlertReportViewModel[];
}

interface IRequestTicketAlertDataAction {
  type: "REQUEST_TICKET_ALERT_DATA";
}

interface IReceiveTicketAlertDataAction {
  type: "RECEIVE_TICKET_ALERT_DATA";
  ticketAlertReport: TicketAlertResponseViewModels;
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
  currencyTypes: CurrencyType[];
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
  reportData: ReportResponseDataViewModel;
}

interface IFailureEventReportDataAction {
  type: "RECEIVE_REPORT_DATA_FAILURE";
  errors: any;
}

interface IRequestTransactionLocatorAction {
  type: "REQUEST_TRANSACTION_LOCATOR_DATA";
}

interface IReceiveTransactionLocatorAction {
  type: "RECEIVE_TRANSACTION_LOCATOR_DATA";
  transactionLocatorResponseViewModel: TransactionLocatorResponseViewModel;
}

interface IFailureTransactionLocatorAction {
  type: "RECEIVE_TRANSACTION_LOCATOR_FAILURE";
  errors: any;
}

interface IRefreshDataAction {
  type: "REQUEST_REFRESH_DATA";
}

interface IRequestTransactionReportSingleDataModelDataAction {
  type: "REQUEST_TRANSACTION_REPORT_SINGE_DATA_MODEL_DATA";
}

interface IReceiveTransactionReportSingleDataModelDataAction {
  type: "RECEIVE_TRANSACTION_REPORT_SINGE_DATA_MODEL_DATA";
  transactionReportSingleDataModel: TransactionReportResponseViewModel;
}

interface IRequestFeeluserAction {
  type: "REQUEST_FEEL_USERS";
}

interface IReceiveFeelUserAction {
  type: "RECEIVE_FEEL_USERS";
  feelUsers: FeelUserReportResponseViewModel;
}
interface IReceivePlacesaAction {
  type: "RECEIVE_ALLPLACES_DATA";
  places: PlacesReponseModel;
}

interface IRequestPlacesAction {
  type: "REQUEST_ALLPLACES_DATA";
}

type KnownAction = IRequestEventDataAction | IReceiveEventDataAction |
  IRequestSubEventDataAction | IReceiveSubEventDataAction | IRequestEventReportDataAction |
  IReceiveEventReportDataAction | IFailureEventReportDataAction | IRefreshDataAction |
  IRequestTransactionReportSingleDataModelDataAction | IReceiveTransactionReportSingleDataModelDataAction | IRequestFeeluserAction
  | IReceiveFeelUserAction | IReceivePlacesaAction | IRequestPlacesAction | IRequestTicketAlertDataAction
  | IReceiveTicketAlertDataAction | IRequestTransactionLocatorAction | IReceiveTransactionLocatorAction | IFailureTransactionLocatorAction;


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
  requestTicketAlertEventData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/ticketAlertEvents`)
      .then((response) => response.json() as Promise<IEvents>)
      .then((data) => {
        dispatch({ type: "RECEIVE_EVENT_DATA", allEvents: data });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_EVENT_DATA" });
  },
  requestTransactionAlertReportData: (altId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/ticketAlertReport/` + altId)
      .then((response) => response.json() as Promise<TicketAlertResponseViewModels>)
      .then((data) => {
        dispatch({ type: "RECEIVE_TICKET_ALERT_DATA", ticketAlertReport: data });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_TICKET_ALERT_DATA" });
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
  requestMultipleSubEventData: (altId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/report/multiplesubevent/` + altId)
      .then((response) => response.json() as Promise<ISubEvents>)
      .then((data) => {
        dispatch({ type: "RECEIVE_SUBEVENT_DATA", allSubEvents: data });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_SUBEVENT_DATA" });
  },
  requestFeelUsers: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/get/feeluserreport`)
      .then((response) => response.json() as Promise<FeelUserReportResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_FEEL_USERS", feelUsers: data });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_FEEL_USERS" });
  },
  getTransactionReport: (values: ReportFormDataViewModel, callback: (ReportResponseDataViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    dispatch({ type: "REQUEST_REPORT_DATA" });
    reportService.getTransactionReport(values)
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
  getTransactionReportAsSingleDataModel: (values: TransactionReportRequestViewModel, callback: (TransactionReportResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    dispatch({ type: "REQUEST_TRANSACTION_REPORT_SINGE_DATA_MODEL_DATA" });
    reportService.getTransactionReportDataAsSingleDataModelReturn(values)
      .then(
        (response: TransactionReportResponseViewModel) => {
          dispatch({ type: "RECEIVE_TRANSACTION_REPORT_SINGE_DATA_MODEL_DATA", transactionReportSingleDataModel: response });
          callback(response);
        },
        (error) => {
        },
      );
  },
  requestAllPlaces: (altId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/report/get-places/` + altId)
      .then((response) => response.json() as Promise<PlacesReponseModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_ALLPLACES_DATA", places: data });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_ALLPLACES_DATA" });
  },
  requestTransactionLocator: (query: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/report/transaction-locator` + query)
      .then((response) => response.json() as Promise<TransactionLocatorResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_TRANSACTION_LOCATOR_DATA", transactionLocatorResponseViewModel: data });
      }, (error) => {
        dispatch({ type: "RECEIVE_TRANSACTION_LOCATOR_FAILURE", errors: "" });

      })
    addTask(fetchTask);
    dispatch({ type: "REQUEST_TRANSACTION_LOCATOR_DATA" });
  },
};

const emptyEvent: IEvents = {
  events: []
};

const emptySubEvent: ISubEvents = {
  subEvents: [],
  currencyTypes: []
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
  ticketFeeDetail: []
};

const unloadedTransactionReportViewModel: TransactionReportViewModel = {
  transactionData: [],
  reportColumns: [],
  channelWiseSummary: [],
  currencyWiseSummary: [],
  eventWiseSummary: [],
  ticketTypeWiseSummary: [],
  venueWiseSummary: [],
  summaryColumns: [],
  dynamicSummaryColumns: [],
  dynamicSummaryInfoColumns: [],
};

const unloadedTransactionReportAsSingleDataModel: TransactionReportResponseViewModel = {
  transactionReports: unloadedTransactionReportViewModel
};

const unloadedFeelUsers: FeelUserReportResponseViewModel = {
  feelUsers: []
};

const emptyTicketAlertResponseViewModels: TicketAlertResponseViewModels = {
  ticketAlertData: []
}

const emptyFILTransactionLocator: FilTransactionLocator = {
  transactionData: []
}

const emptyTransactionLocator: TransactionLocatorResponseViewModel = {
  success: false,
  filTransactionLocator: emptyFILTransactionLocator
}

const unloadedState: ITransactionReportComponentState = {
  allEvents: emptyEvent,
  allSubEvents: emptySubEvent,
  transactionreport: emptyEventReport,
  isLoading: false,
  fetchEventSuccess: false,
  fetchSubEventSuccess: false,
  fetchReportSuccess: false,
  errors: null,
  feelUsers: unloadedFeelUsers,
  fetchAllPlaces: false,
  ticketAlertReport: emptyTicketAlertResponseViewModels,
  places: null,
  transactionLocatorResponseViewModel: emptyTransactionLocator
}

export const reducer: Reducer<ITransactionReportComponentState> = (state: ITransactionReportComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_DATA":
      return {
        ...state,
        fetchEventSuccess: false,
        fetchSubEventSuccess: false,
        fetchReportSuccess: false,
        isLoading: true,
        errors: {}
      };
    case "RECEIVE_EVENT_DATA":
      return {
        ...state,
        allEvents: action.allEvents,
        fetchEventSuccess: true,
        fetchSubEventSuccess: false,
        fetchReportSuccess: false,
        isLoading: false,
        errors: {}
      };
    case "REQUEST_FEEL_USERS":
      return {
        ...state,
        fetchEventSuccess: false,
        fetchSubEventSuccess: false,
        fetchReportSuccess: false,
        feelUsers: unloadedFeelUsers,
        isLoading: true,
        errors: {}
      };
    case "RECEIVE_FEEL_USERS":
      return {
        ...state,
        fetchEventSuccess: true,
        fetchSubEventSuccess: false,
        fetchReportSuccess: false,
        isLoading: false,
        feelUsers: action.feelUsers,
        errors: {}
      };

    case "REQUEST_TICKET_ALERT_DATA":
      return {
        fetchSubEventSuccess: false,
        fetchReportSuccess: false,
        isLoading: true,
        isTicketAlertRequest: true,
        errors: {}
      };
    case "RECEIVE_TICKET_ALERT_DATA":
      return {
        ticketAlertReport: action.ticketAlertReport,
        fetchSubEventSuccess: true,
        fetchReportSuccess: true,
        isLoading: false,
        isTicketAlertRequest: false,
        errors: {}
      };
    case "REQUEST_SUBEVENT_DATA":
      return {
        ...state,
        fetchSubEventSuccess: false,
        fetchReportSuccess: false,
        isLoading: true,
        errors: {}
      };
    case "RECEIVE_SUBEVENT_DATA":
      return {
        ...state,
        allSubEvents: action.allSubEvents,
        fetchSubEventSuccess: true,
        fetchReportSuccess: false,
        isLoading: false,
        errors: {}
      };
    case "REQUEST_REPORT_DATA":
      return {
        ...state,
        fetchReportSuccess: false,
        isLoading: true,
        errors: {}
      };
    case "RECEIVE_REPORT_DATA":
      return {
        ...state,
        transactionreport: action.reportData,
        fetchReportSuccess: true,
        isLoading: false,
        errors: {}
      };
    case "REQUEST_TRANSACTION_LOCATOR_DATA":
      return {
        ...state,
        fetchReportSuccess: false,
        isLoading: true,
        errors: {}
      };
    case "RECEIVE_TRANSACTION_LOCATOR_DATA":
      return {
        ...state,
        transactionLocatorResponseViewModel: action.transactionLocatorResponseViewModel,
        fetchReportSuccess: true,
        isLoading: false,
        errors: {}
      };
    case "RECEIVE_TRANSACTION_LOCATOR_FAILURE":
      return {
        ...state,
        fetchReportSuccess: true,
        isLoading: false,
        errors: {}
      };
    case "REQUEST_TRANSACTION_REPORT_SINGE_DATA_MODEL_DATA":
      return {
        ...state,
        fetchReportSuccess: false,
        isLoading: true,
        transactionReportSingleDataModel: unloadedTransactionReportAsSingleDataModel,
        errors: {}
      };
    case "RECEIVE_TRANSACTION_REPORT_SINGE_DATA_MODEL_DATA":
      return {
        ...state,
        fetchReportSuccess: true,
        isLoading: false,
        transactionReportSingleDataModel: action.transactionReportSingleDataModel,
        errors: {}
      };
    case "RECEIVE_REPORT_DATA_FAILURE":
      return {
        ...state,
        fetchReportSuccess: false,
        isLoading: false,
        transactionReportSingleDataModel: state.transactionReportSingleDataModel,
        errors: action.errors
      };
    case "REQUEST_REFRESH_DATA":
      return {
        ...state,
        allSubEvents: emptySubEvent,
        transactionreport: emptyEventReport,
        valEvent: "",
        valSubEvent: "",
        fetchEventSuccess: true,
        fetchSubEventSuccess: false,
        valFromDate: "",
        valToDate: "",
        fetchReportSuccess: false,
        isLoading: false,
        transactionReportSingleDataModel: state.transactionReportSingleDataModel,
        errors: {}
      };
    case "RECEIVE_ALLPLACES_DATA":
      return {
        ...state,
        fetchAllPlaces: true,
        places: action.places
      };
    case "REQUEST_ALLPLACES_DATA":
      return {
        ...state,
        fetchAllPlaces: false,
        places: state.places
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedState;
};