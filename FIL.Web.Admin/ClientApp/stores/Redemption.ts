import { Action, ActionCreator, Reducer } from "redux";
import { IAppThunkAction } from "./";
import { fetch, addTask } from 'domain-task';
import { RedemptionService } from "../services/Redemption";
import { CreateGuideInputModel, GuideResponseModel } from "../models/Redemption/GuideViewModel";
import { AllGuidesResponseModel } from "../models/Redemption/AllGuidesResponseModel";
import { ConfirmGuideResponseModel } from "../models/Redemption/ConfirmGuideResponseModel";
import { GuideOrderDetailResponseModel } from "../models/Redemption/GuideOrderDetailModel";
import { EditGuideResponseModel } from "../models/Redemption/EditGuideResponseModel";

export interface RedemptionComponentProps {
    Redemption: RedemptionComponentState;
}

export interface RedemptionComponentState {
    isLoading?: boolean;
    errors?: any;
    fetchSaveSuccess?: boolean;
    isError?: boolean;
    fetchSaveFailure?: boolean;
    fetchAllGuideRequest?: boolean;
    fetchAllGuideSuccess?: boolean;
    fetchAllGuideFailure?: boolean;
    fetchConfirmRequest?: boolean;
    fetchConfirmSuccess?: boolean;
    fetchConfirmFailure?: boolean;
    fetchGuideOrdersRequest?: boolean;
    fetchGuideOrdersSuccess?: boolean;
    fetchGuideOrdersFailure?: boolean;
    fetchConfirmOrderRequest?: boolean;
    fetchConfirmOrderSuccess?: boolean;
    fetchConfirmOrderFailure?: boolean;
    saveGuideResponse: GuideResponseModel;
    allGuides: AllGuidesResponseModel;
    confirmGuide: ConfirmGuideResponseModel;
    guideOrders: GuideOrderDetailResponseModel;
    confirmOrder: ConfirmGuideResponseModel;
    editGuideDetail: EditGuideResponseModel;
    fetchEditGuideDetailSuccess: boolean;
    isLoadingEditGuideDetail: boolean;
}

interface IRequestSaveGuide {
    type: "REQUEST_SAVE_GUIDE_ACTION";
}

interface IReceiveSaveGuide {
    type: "RECEIVE_SAVE_GUIDE_ACTION";
    result: GuideResponseModel;
}

interface IFailureSaveGuide {
    type: "FAILURE_SAVE_GUIDE_ACTION";
}

interface IRequestConfirmGuide {
    type: "REQUEST_CONFIRM_GUIDE_ACTION";
}

interface IReceiveConfirmGuide {
    type: "RECEIVE_CONFIRM_GUIDE_ACTION";
    result: ConfirmGuideResponseModel;
}

interface IFailureConfirmGuide {
    type: "FAILURE_CONFIRM_GUIDE_ACTION";
}

interface IRequestAllGuides {
    type: "REQUEST_ALL_GUIDE_ACTION";
}

interface IReceiveAllGuide {
    type: "RECEIVE_ALL_GUIDE_ACTION";
    result: AllGuidesResponseModel;
}

interface IFailureAllGuide {
    type: "FAILURE_ALL_GUIDE_ACTION";
}

interface IRequestGuideOrders {
    type: "REQUEST_GUIDE_ORDERS_ACTION";
}

interface IReceiveGuideOrders {
    type: "RECEIVE_GUIDE_ORDERS_ACTION";
    guideOrders: GuideOrderDetailResponseModel;
}

interface IFailureGuideOrders {
    type: "FAILURE_GUIDE_ORDERS_ACTION";
}

interface IRequestConfirmOrder {
    type: "REQUEST_CONFIRM_ORDERS_ACTION";
}

interface IReceiveConfirmOrder {
    type: "RECEIVE_CONFIRM_ORDERS_ACTION";
    confirmOrder: ConfirmGuideResponseModel;
}

interface IFailureConfirmOrder {
    type: "FAILURE_CONFIRM_Order_ACTION";
}

interface IRequestEditGuideDetail {
    type: "EDIT_GUIDE_DETAIL_REQUEST";
}

interface ISuccessEditGuideDetail {
    type: "EDIT_GUIDE_DETAIL_SUCCESS";
    payload: EditGuideResponseModel;
}

type KnownAction = IRequestSaveGuide | IReceiveSaveGuide | IFailureSaveGuide | IRequestAllGuides
    | IReceiveAllGuide | IFailureAllGuide | IRequestConfirmGuide | IReceiveConfirmGuide | IFailureConfirmGuide |
    IRequestGuideOrders | IReceiveGuideOrders | IFailureGuideOrders | IRequestConfirmOrder | IReceiveConfirmOrder |
    IFailureConfirmOrder | IRequestEditGuideDetail | ISuccessEditGuideDetail;

export const actionCreators = {
    saveGuide: (redemptionModel: CreateGuideInputModel, callback: (DescriptionResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "REQUEST_SAVE_GUIDE_ACTION" });
            RedemptionService.SaveGuide(redemptionModel)
                .then((response: GuideResponseModel) => {
                    dispatch({ type: "RECEIVE_SAVE_GUIDE_ACTION", result: response });
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "FAILURE_SAVE_GUIDE_ACTION" });
                    });
        },
    requestAllGuideData: (approveStatusId: number): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/Guide/GetAll/${approveStatusId}`)
            .then((response) => response.json() as Promise<AllGuidesResponseModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_ALL_GUIDE_ACTION", result: data, });
            },
                (error) => {
                    dispatch({ type: "FAILURE_ALL_GUIDE_ACTION" });
                });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_ALL_GUIDE_ACTION" });
    },
    confirmGuide: (guideId: number, approveStatusId: number, callback: (ConfirmGuideResponseModel) => void)
        : IAppThunkAction<KnownAction> => (dispatch, getState) => {
            const fetchTask = fetch(`api/Guide/Confirm/${guideId}/${approveStatusId}`)
                .then((response) => response.json() as Promise<ConfirmGuideResponseModel>)
                .then((data) => {
                    dispatch({ type: "RECEIVE_CONFIRM_GUIDE_ACTION", result: data, });
                    callback(data);
                },
                    (error) => {
                        dispatch({ type: "FAILURE_CONFIRM_GUIDE_ACTION" });
                    });
            addTask(fetchTask);
            dispatch({ type: "REQUEST_CONFIRM_GUIDE_ACTION" });
        },
    requestGuideOrders: (orderStatusId: number)
        : IAppThunkAction<KnownAction> => (dispatch, getState) => {
            const fetchTask = fetch(`api/Get/Orders/${orderStatusId}`)
                .then((response) => response.json() as Promise<GuideOrderDetailResponseModel>)
                .then((data) => {
                    dispatch({ type: "RECEIVE_GUIDE_ORDERS_ACTION", guideOrders: data, });
                },
                    (error) => {
                        dispatch({ type: "FAILURE_GUIDE_ORDERS_ACTION" });
                    });
            addTask(fetchTask);
            dispatch({ type: "REQUEST_GUIDE_ORDERS_ACTION" });
        },
    confirmOrder: (approveStatusId: number, transactionId: number, callback: (ConfirmGuideResponseModel) => void)
        : IAppThunkAction<KnownAction> => (dispatch, getState) => {
            const fetchTask = fetch(`api/Order/Confirm/${approveStatusId}/${transactionId}`)
                .then((response) => response.json() as Promise<ConfirmGuideResponseModel>)
                .then((data) => {
                    dispatch({ type: "RECEIVE_CONFIRM_ORDERS_ACTION", confirmOrder: data, });
                    callback(data);
                },
                    (error) => {
                        dispatch({ type: "FAILURE_CONFIRM_Order_ACTION" });
                    });
            addTask(fetchTask);
            dispatch({ type: "REQUEST_CONFIRM_ORDERS_ACTION" });
        },
    requestEditGuideDetail: (userAltId: string, callback?: (res: EditGuideResponseModel) => void)
        : IAppThunkAction<KnownAction> => (dispatch, getState) => {
            const fetchTask = fetch(`api/guide/edit/${userAltId}`)
                .then((response) => response.json() as Promise<EditGuideResponseModel>)
                .then((data) => {
                    dispatch({ type: "EDIT_GUIDE_DETAIL_SUCCESS", payload: data, });
                    callback(data);
                },
                    (error) => {
                        console.log(JSON.stringify(error));
                    });
            addTask(fetchTask);
            dispatch({ type: "EDIT_GUIDE_DETAIL_REQUEST" });
        },
};

const unloadedGuideResponse: GuideResponseModel = {
    success: false,
    isSaving: false
}

const unloadedAllGuidesResponseModel: AllGuidesResponseModel = {
    approvedByUsers: [],
    guideDetails: []
}

const unloadedConfirmGuideResponseModel: ConfirmGuideResponseModel = {
    success: false
}

const unloadedGuideOrderDetailResponseModel: GuideOrderDetailResponseModel = {
    success: false,
    approvedByUsers: [],
    orderDetails: []
}

const unloadedState: RedemptionComponentState = {
    errors: null,
    fetchSaveSuccess: false,
    isLoading: false,
    isError: false,
    fetchSaveFailure: false,
    saveGuideResponse: unloadedGuideResponse,
    fetchAllGuideRequest: false,
    allGuides: unloadedAllGuidesResponseModel,
    confirmGuide: unloadedConfirmGuideResponseModel,
    fetchAllGuideSuccess: false,
    fetchConfirmRequest: false,
    fetchAllGuideFailure: false,
    fetchConfirmFailure: false,
    fetchConfirmSuccess: false,
    fetchGuideOrdersFailure: false,
    fetchGuideOrdersRequest: false,
    fetchGuideOrdersSuccess: false,
    fetchConfirmOrderFailure: false,
    fetchConfirmOrderRequest: false,
    fetchConfirmOrderSuccess: false,
    guideOrders: unloadedGuideOrderDetailResponseModel,
    confirmOrder: unloadedConfirmGuideResponseModel,
    editGuideDetail: <EditGuideResponseModel>{},
    fetchEditGuideDetailSuccess: false,
    isLoadingEditGuideDetail: false,
};

export const reducer: Reducer<RedemptionComponentState> = (state: RedemptionComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_SAVE_GUIDE_ACTION":
            return {
                ...state,
                saveGuideResponse: { success: unloadedGuideResponse.success, isSaving: true },
            };
        case "RECEIVE_SAVE_GUIDE_ACTION":
            return {
                ...state,
                isLoading: false,
                fetchSaveSuccess: true,
                saveGuideResponse: action.result
            };
        case "FAILURE_SAVE_GUIDE_ACTION":
            return {
                ...state,
                fetchSaveFailure: true
            };
        case "REQUEST_ALL_GUIDE_ACTION":
            return {
                ...state,
                fetchAllGuideRequest: true
            };
        case "RECEIVE_ALL_GUIDE_ACTION":
            return {
                ...state,
                fetchAllGuideSuccess: true,
                fetchAllGuideRequest: false,
                allGuides: action.result
            };
        case "FAILURE_ALL_GUIDE_ACTION":
            return {
                ...state,
                fetchAllGuideRequest: false,
                fetchAllGuideFailure: true
            };
        case "REQUEST_CONFIRM_GUIDE_ACTION":
            return {
                ...state,
                fetchConfirmRequest: true
            };
        case "RECEIVE_CONFIRM_GUIDE_ACTION":
            return {
                ...state,
                fetchConfirmSuccess: true,
                fetchConfirmRequest: false,
                confirmGuide: action.result
            };
        case "FAILURE_CONFIRM_GUIDE_ACTION":
            return {
                ...state,
                fetchConfirmFailure: true,
                fetchConfirmRequest: false
            };
        case "REQUEST_GUIDE_ORDERS_ACTION":
            return {
                ...state,
                fetchGuideOrdersRequest: true,
                fetchGuideOrdersFailure: false
            };
        case "RECEIVE_GUIDE_ORDERS_ACTION":
            return {
                ...state,
                fetchGuideOrdersSuccess: true,
                fetchGuideOrdersRequest: false,
                guideOrders: action.guideOrders
            };
        case "FAILURE_GUIDE_ORDERS_ACTION":
            return {
                ...state,
                fetchGuideOrdersFailure: true,
                fetchGuideOrdersRequest: false
            };
        case "REQUEST_CONFIRM_ORDERS_ACTION":
            return {
                ...state,
                fetchConfirmOrderRequest: true
            };
        case "RECEIVE_CONFIRM_ORDERS_ACTION":
            return {
                ...state,
                fetchConfirmOrderSuccess: true,
                fetchConfirmOrderRequest: false,
                confirmOrder: action.confirmOrder
            };
        case "FAILURE_CONFIRM_Order_ACTION":
            return {
                ...state,
                fetchConfirmOrderFailure: true,
                fetchConfirmOrderRequest: false
            };
        case "EDIT_GUIDE_DETAIL_REQUEST":
            return {
                ...state,
                fetchEditGuideDetailSuccess: false,
                isLoadingEditGuideDetail: true,
            };
        case "EDIT_GUIDE_DETAIL_SUCCESS":
            return {
                ...state,
                editGuideDetail: action.payload,
                fetchEditGuideDetailSuccess: true,
                isLoadingEditGuideDetail: false,
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};