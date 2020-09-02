import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { InventoryRequestViewModel } from "../models/Inventory/InventoryRequestViewModel";
import InventoryResponseViewModel from "../models/Inventory/InventoryResponseViewModel";
import DocumentTypesRequestViewModel from "../models/Inventory/DocumentTypesRequestViewModel";
import DocumentTypesSaveResponseViewModel from "../models/Inventory/DocumentTypesSaveResponseViewModel";
import { inventoryService } from "../services/Inventory";
import { IAppThunkAction } from "./";
import { GetPlaceInventoryDataResponseViewModel, regularViewModel } from "../models/Inventory/GetPlaceInventoryDataResponseViewModel";
import TicketCategoryTypesResponseViewModel from "../models/TicketCategoryType/TicketCategoryTypesResponseViewModel";
import { TicketCategoryDetailResponseViewModel } from "../models/TicketCategory/TicketCategory";
import { EventCreation } from '../models/CreateEvent/EventCreation';
import PlaceCalendarResponseViewModel from "../models/PlaceCalendar/PlaceCalendarResponseViewModel";
import { FinanceDetailViewModal } from "../models/Finance/FinanceDetailViewModal";

export interface InventoryProps {
    inventory: InventoryState;
}

export interface InventoryState {
    inventoryResponse?: InventoryResponseViewModel;
    customerIdTypeSaveResponse: DocumentTypesSaveResponseViewModel;
    getPlaceInventoryDataResponseViewModel: GetPlaceInventoryDataResponseViewModel;
    ticketCategoryTypeResponse?: TicketCategoryTypesResponseViewModel;
    stripeConnectResponse?: InventoryResponseViewModel;
    ticketCategoryDetails?: TicketCategoryDetailResponseViewModel;
    saveFinanceDetailResponse: PlaceCalendarResponseViewModel;
    isInventorySaveRequest: boolean;
    isShowSuccessAlert: boolean;
    isInventoryDataSuccess: boolean;
    isEventCreationSaveSuccess: boolean;
    isEventCreationSaveRequest: boolean;
    isEventCreationSaveFailed: boolean;
    isStripeAccountSaveSuccess: boolean;
    isStripeAccountSaveRequest: boolean;
    isStripeAccountSaveFailed: boolean;
    isTicketCategoryDetailsRequest: boolean;
    isTicketCategoryDetailsSuccess: boolean;
    isTicketCategoryDetailsFailure: boolean;
    isFinanceSaveSuccess: boolean;
    isFinanceSaveRequest: boolean;
    isFinanceSaveFailed: boolean;
    errors?: any;
}

const emptyInventoryResponse: InventoryResponseViewModel = {
    success: false
}

const emptyDocumentTypesResponse: DocumentTypesSaveResponseViewModel = {
    success: false
}

const emptyPlaceCalendarResponseViewModel: PlaceCalendarResponseViewModel = {
    success: false,
    eventAltId: "",
    eventHosts: []
}

const emptyTicketCategoryTypesResponseViewModel: TicketCategoryTypesResponseViewModel = {
    ticketCategorySubTypes: [],
    ticketCategoryTypes: []
}

const emptyRegularView: regularViewModel = {
    isSameTime: false,
    customTimeModel: [],
    timeModel: [],
    daysOpen: [],
}

const emptyGetPlaceInventoryDataResponseViewModel: GetPlaceInventoryDataResponseViewModel = {
    customerDocumentTypes: [],
    deliveryTypes: [],
    eventDeliveryTypeDetails: [],
    eventDetails: [],
    placeCustomerDocumentTypeMappings: [],
    placeTicketRedemptionDetails: [],
    ticketCategoryContainer: [],
    ticketValidityTypes: [],
    placeHolidayDates: [],
    placeWeekOffs: [],
    event: undefined,
    eventTicketDetailTicketCategoryTypeMappings: [],
    customerInformations: [],
    eventCustomerInformationMappings: [],
    regularTimeModel: emptyRegularView,
    seasonTimeModel: [],
    specialDayModel: [],
    eventAttribute: undefined
}

const emptyTicketCategoryDetailResponseViewModel: TicketCategoryDetailResponseViewModel = {
    ticketCategoryDetails: []
}

const DefaultInventoryState: InventoryState = {
    inventoryResponse: emptyInventoryResponse,
    customerIdTypeSaveResponse: emptyDocumentTypesResponse,
    errors: undefined,
    ticketCategoryTypeResponse: emptyTicketCategoryTypesResponseViewModel,
    getPlaceInventoryDataResponseViewModel: emptyGetPlaceInventoryDataResponseViewModel,
    ticketCategoryDetails: emptyTicketCategoryDetailResponseViewModel,
    saveFinanceDetailResponse: emptyPlaceCalendarResponseViewModel,
    isInventorySaveRequest: false,
    isShowSuccessAlert: false,
    isInventoryDataSuccess: false,
    isEventCreationSaveSuccess: false,
    isEventCreationSaveRequest: false,
    isEventCreationSaveFailed: false,
    isStripeAccountSaveFailed: false,
    isStripeAccountSaveRequest: false,
    isStripeAccountSaveSuccess: false,
    isTicketCategoryDetailsRequest: false,
    isTicketCategoryDetailsSuccess: false,
    isTicketCategoryDetailsFailure: false,
    isFinanceSaveFailed: false,
    isFinanceSaveRequest: false,
    isFinanceSaveSuccess: false
};

interface IInventoryRequestAction {
    type: "PLACE_INVENTORY_REQUEST_ACTION";
}

interface IInventorySuccesstAction {
    type: "PLACE_INVENTORY_SUCCESS_ACTION";
    inventoryResponse: InventoryResponseViewModel;
}

interface ITicketCategoryTypesRequestAction {
    type: "TICKET_CATEGORY_TYPE_REQUEST_ACTION";
}

interface ITicketCategoryTypesSuccesstAction {
    type: "TICKET_CATEGORY_TYPE_SUCCESS_ACTION";
    ticketCategoryTypeResponse: TicketCategoryTypesResponseViewModel;
}

interface IInventoryFailureAction {
    type: "PLACE_INVENTORY_FAILURE_ACTION";
}

interface ICustomerIdTypeSaveRequestAction {
    type: "CUSTOMER_ID_TYPE_SAVE_REQUEST_ACTION";
}

interface ICustomerIdTypeSaveSuccesstAction {
    type: "CUSTOMER_ID_TYPE_SAVE_SUCCESS_ACTION";
    customerIdTypeSaveResponse: DocumentTypesSaveResponseViewModel;
}

interface ICustomerIdTypeSaveFailureAction {
    type: "CUSTOMER_ID_TYPE_SAVE_FAILURE_ACTION";
}


interface IRequestInventoryDataAction {
    type: "REQUEST_INVENTORY_DATA_ACTION";
}

interface IReceiveInventoryDataAction {
    type: "RECEIVE_INVENTORY_DATA_ACTION";
    getPlaceInventoryDataResponseViewModel: GetPlaceInventoryDataResponseViewModel;
}

interface IFailureInventoryDataAction {
    type: "FAILURE_INVENTORY_DATA_ACTION";
}

interface IRequestEventCreationDataAction {
    type: "REQUEST_EVENT_CREATION_DATA_ACTION";
}

interface IReceiveEventCreationDataAction {
    type: "RECEIVE_EVENT_CREATION_DATA_ACTION";
    eventCreationRequest: PlaceCalendarResponseViewModel;
}

interface IFailureEventCreationDataAction {
    type: "FAILURE_EVENT_CREATION_DATA_ACTION";
}

interface IRequestSaveFinanceDataAction {
    type: "REQUEST_SAVE_FINANCE_DATA_ACTION";
}

interface IReceiveSaveFinanceDataAction {
    type: "RECEIVE_SAVE_FINANCE_DATA_ACTION";
    saveFinanceDetailResponse: PlaceCalendarResponseViewModel;
}

interface IFailureSaveFinanceDataAction {
    type: "FAILURE_SAVE_FINANCE_ACTION";
}

interface IRequestSaveStripeConnectAccountAction {
    type: "REQUEST_SAVE_STRIPE_DATA_ACTION";
}

interface IReceiveSaveStripeConnectAccountAction {
    type: "RECEIVE_SAVE_STRIPE_DATA_ACTION";
    stripeConnectResponse: InventoryResponseViewModel;
}

interface IFailureSaveStripeConnectAccountAction {
    type: "FAILURE_SAVE_STRIPE_DATA_ACTION";
}

interface IRequestTicketCategoryDetailAction {
    type: "REQUEST_TICKET_CATEGORY_DATA_ACTION";
}

interface IReceiveTicketCategoryDetailAction {
    type: "RECEIVE_TICKET_CATEGORY_DATA_ACTION";
    ticketCategoryDetails: TicketCategoryDetailResponseViewModel;
}

interface IFailureTicketCategoryDetailAction {
    type: "FAILURE_TICKET_CATEGORY_DATA_ACTION";
}

type KnownAction = IInventoryRequestAction
    | ITicketCategoryTypesRequestAction | ITicketCategoryTypesSuccesstAction
    | IInventorySuccesstAction | IInventoryFailureAction | ICustomerIdTypeSaveRequestAction
    | ICustomerIdTypeSaveSuccesstAction | ICustomerIdTypeSaveFailureAction | IRequestInventoryDataAction
    | IReceiveInventoryDataAction | IRequestEventCreationDataAction | IReceiveEventCreationDataAction
    | IFailureEventCreationDataAction | IRequestSaveStripeConnectAccountAction | IReceiveSaveStripeConnectAccountAction
    | IFailureSaveStripeConnectAccountAction | IRequestTicketCategoryDetailAction | IReceiveTicketCategoryDetailAction
    | IFailureTicketCategoryDetailAction | IRequestSaveFinanceDataAction | IReceiveSaveFinanceDataAction |
    IFailureSaveFinanceDataAction;

export const actionCreators = {
    inventoryDataSubmit: (inventoryViewModel: InventoryRequestViewModel, callback: (InventoryResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "PLACE_INVENTORY_REQUEST_ACTION" });
            inventoryService.inventoryRequest(inventoryViewModel)
                .then((response: InventoryResponseViewModel) => {
                    dispatch({ type: "PLACE_INVENTORY_SUCCESS_ACTION", inventoryResponse: response });
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "PLACE_INVENTORY_FAILURE_ACTION" });
                    });
        },
    saveCustomerIdType: (customerIdTypeViewModel: DocumentTypesRequestViewModel, callback: (DocumentTypesSaveResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "CUSTOMER_ID_TYPE_SAVE_REQUEST_ACTION" });
            inventoryService.customerIdTypeSaveRequest(customerIdTypeViewModel)
                .then((response: InventoryResponseViewModel) => {
                    dispatch({ type: "CUSTOMER_ID_TYPE_SAVE_SUCCESS_ACTION", customerIdTypeSaveResponse: response });
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "CUSTOMER_ID_TYPE_SAVE_FAILURE_ACTION" });
                    });
        },
    requestInentoryData: (placeAltId: string, callback: (GetPlaceInventoryDataResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/get/inventory/${placeAltId}`)
            .then((response) => response.json() as Promise<GetPlaceInventoryDataResponseViewModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_INVENTORY_DATA_ACTION", getPlaceInventoryDataResponseViewModel: data, });
                callback(data);
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_INVENTORY_DATA_ACTION" });
    },

    requestTicketCategoryTypes: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/get/ticketCategoryTypes`)
            .then((response) => response.json() as Promise<TicketCategoryTypesResponseViewModel>)
            .then((data) => {
                dispatch({ type: "TICKET_CATEGORY_TYPE_SUCCESS_ACTION", ticketCategoryTypeResponse: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "TICKET_CATEGORY_TYPE_REQUEST_ACTION" });
    },
    saveEventDetailCreation: (inventoryViewModel: EventCreation, callback: (PlaceCalendarResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_EVENT_CREATION_DATA_ACTION" });
        inventoryService.saveEventDetail(inventoryViewModel)
            .then((response: PlaceCalendarResponseViewModel) => {
                localStorage.setItem('placeAltId', response.eventAltId.toString());
                dispatch({ type: "RECEIVE_EVENT_CREATION_DATA_ACTION", eventCreationRequest: response });
                callback(response);
            },
                (error) => {
                    dispatch({ type: "FAILURE_EVENT_CREATION_DATA_ACTION" });
                });
    },
    saveFinanceDetails: (financeModal: FinanceDetailViewModal, callback: (PlaceCalendarResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_SAVE_FINANCE_DATA_ACTION" });
        inventoryService.saveFinanceDetails(financeModal)
            .then((response: PlaceCalendarResponseViewModel) => {
                if (response.success) {
                    dispatch({ type: "RECEIVE_SAVE_FINANCE_DATA_ACTION", saveFinanceDetailResponse: response });
                } else {
                    dispatch({ type: "FAILURE_SAVE_FINANCE_ACTION" });
                }
                callback(response);
            },
                (error) => {
                    dispatch({ type: "FAILURE_SAVE_FINANCE_ACTION" });
                });
    },
    saveStripeConnectAccount: (authCode: string, eventId: string, callback: (InventoryResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_SAVE_STRIPE_DATA_ACTION" });
        const fetchTask = fetch(`api/save/stripeConnectAcccountIds/${authCode}/${eventId}`)
            .then((response) => response.json() as Promise<InventoryResponseViewModel>)
            .then((data) => {
                if (data.success) {
                    dispatch({ type: "RECEIVE_SAVE_STRIPE_DATA_ACTION", stripeConnectResponse: data, });
                    callback(data);
                } else {
                    dispatch({ type: "FAILURE_SAVE_STRIPE_DATA_ACTION" });
                }
            }, (error) => {
                dispatch({ type: "FAILURE_SAVE_STRIPE_DATA_ACTION" });
            });
        addTask(fetchTask);
    },
    getTicketCategoryDetails: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_TICKET_CATEGORY_DATA_ACTION" });
      const fetchTask = fetch(`api/get/all/ticketCategoryDetail`)
            .then((response) => response.json() as Promise<TicketCategoryDetailResponseViewModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TICKET_CATEGORY_DATA_ACTION", ticketCategoryDetails: data, });
            }, (error) => {
                dispatch({ type: "FAILURE_TICKET_CATEGORY_DATA_ACTION" });
            });
        addTask(fetchTask);
    },
};

export const reducer: Reducer<InventoryState> = (state: InventoryState, action: KnownAction) => {
    switch (action.type) {
        case "PLACE_INVENTORY_REQUEST_ACTION":
            return {
                ...state,
                inventoryResponse: emptyInventoryResponse,
                customerIdTypeSaveResponse: state.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: true,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };
        case "PLACE_INVENTORY_SUCCESS_ACTION":
            return {
                ...state,
                inventoryResponse: action.inventoryResponse,
                customerIdTypeSaveResponse: state.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: false,
                isShowSuccessAlert: true,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };
        case "PLACE_INVENTORY_FAILURE_ACTION":
            return {
                ...state,
                inventoryResponse: emptyInventoryResponse,
                customerIdTypeSaveResponse: state.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: false,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };
        case "REQUEST_EVENT_CREATION_DATA_ACTION":
            return {
                ...state,
                isEventCreationSaveRequest: true
            };
        case "RECEIVE_EVENT_CREATION_DATA_ACTION":
            return {
                ...state,
                isEventCreationSaveSuccess: true,
                isEventCreationSaveRequest: false,
                eventCreationRequest: action.eventCreationRequest
            };
        case "FAILURE_EVENT_CREATION_DATA_ACTION":
            return {
                ...state,
                isEventCreationSaveFailed: false,
                isEventCreationSaveRequest: false
            };
        case "REQUEST_SAVE_STRIPE_DATA_ACTION":
            return {
                ...state,
                isStripeAccountSaveRequest: true
            };
        case "RECEIVE_SAVE_STRIPE_DATA_ACTION":
            return {
                ...state,
                isStripeAccountSaveSuccess: true,
                isStripeAccountSaveRequest: false,
                stripeConnectResponse: action.stripeConnectResponse
            };
        case "FAILURE_SAVE_STRIPE_DATA_ACTION":
            return {
                ...state,
                isStripeAccountSaveFailed: true,
                isStripeAccountSaveRequest: false
            };
        case "REQUEST_SAVE_FINANCE_DATA_ACTION":
            return {
                ...state,
                isFinanceSaveRequest: true
            };
        case "RECEIVE_SAVE_FINANCE_DATA_ACTION":
            return {
                ...state,
                isFinanceSaveSuccess: true,
                isFinanceSaveRequest: false,
                saveFinanceDetailResponse: action.saveFinanceDetailResponse
            };
        case "FAILURE_SAVE_FINANCE_ACTION":
            return {
                ...state,
                isFinanceSaveFailed: true,
                isFinanceSaveRequest: false
            };
        case "REQUEST_TICKET_CATEGORY_DATA_ACTION":
            return {
                ...state,
                isTicketCategoryDetailsRequest: true
            };
        case "RECEIVE_TICKET_CATEGORY_DATA_ACTION":
            return {
                ...state,
                isTicketCategoryDetailsSuccess: true,
                isTicketCategoryDetailsRequest: false,
                ticketCategoryDetails: action.ticketCategoryDetails
            };
        case "FAILURE_TICKET_CATEGORY_DATA_ACTION":
            return {
                ...state,
                isTicketCategoryDetailsFailure: true,
                isTicketCategoryDetailsRequest: false
            };
        case "CUSTOMER_ID_TYPE_SAVE_REQUEST_ACTION":
            return {
                ...state,
                inventoryResponse: state.inventoryResponse,
                customerIdTypeSaveResponse: emptyDocumentTypesResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: false,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };
        case "CUSTOMER_ID_TYPE_SAVE_SUCCESS_ACTION":
            return {
                ...state,
                inventoryResponse: state.inventoryResponse,
                customerIdTypeSaveResponse: action.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: state.isInventorySaveRequest,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };
        case "CUSTOMER_ID_TYPE_SAVE_FAILURE_ACTION":
            return {
                ...state,
                inventoryResponse: state.inventoryResponse,
                customerIdTypeSaveResponse: emptyDocumentTypesResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: state.isInventorySaveRequest,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };

        case "REQUEST_INVENTORY_DATA_ACTION":
            return {
                ...state,
                inventoryResponse: state.inventoryResponse,
                customerIdTypeSaveResponse: state.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: emptyGetPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: state.isInventorySaveRequest,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: false
            };
        case "RECEIVE_INVENTORY_DATA_ACTION":
            return {
                ...state,
                inventoryResponse: state.inventoryResponse,
                customerIdTypeSaveResponse: state.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: action.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: state.isInventorySaveRequest,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: true,
            };
        case "TICKET_CATEGORY_TYPE_REQUEST_ACTION":
            return {
                ...state,
                inventoryResponse: state.inventoryResponse,
                customerIdTypeSaveResponse: state.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: state.ticketCategoryTypeResponse,
                isInventorySaveRequest: state.isInventorySaveRequest,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };
        case "TICKET_CATEGORY_TYPE_SUCCESS_ACTION":
            return {
                ...state,
                inventoryResponse: state.inventoryResponse,
                customerIdTypeSaveResponse: state.customerIdTypeSaveResponse,
                getPlaceInventoryDataResponseViewModel: state.getPlaceInventoryDataResponseViewModel,
                ticketCategoryTypeResponse: action.ticketCategoryTypeResponse,
                isInventorySaveRequest: state.isInventorySaveRequest,
                isShowSuccessAlert: false,
                isInventoryDataSuccess: state.isInventoryDataSuccess
            };

        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultInventoryState;
};
