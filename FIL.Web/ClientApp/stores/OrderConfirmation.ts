import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { OrderConfirmationNewResponseViewModel as OrderConfirmationResponseViewModel } from "../models/OrderConfirmationNewResponseViewModel";
import { OrderConfirmationFormDataViewModel } from "../models/OrderConfirmationFormDataViewModel";
import { orderConfirmationService } from "../services/orderConfirmation";

export interface IOrderConfirmationState {
    isLoading?: boolean;
    fetchOrderConfirmationSuccess: boolean;
    errors?: any;
    orderconfirmations: OrderConfirmationResponseViewModel;
}

interface IRequestCategoryData {
    type: "REQUEST_ORDER_CONFIRMATION_DATA";
}

interface IReceiveCategoryData {
    type: "RECEIVE_ORDER_CONFIRMATION_DATA";
    orderconfirmations: OrderConfirmationResponseViewModel;
}

type KnownAction = IRequestCategoryData | IReceiveCategoryData;

export const actionCreators = {
    requestConfirmationData: (values: OrderConfirmationFormDataViewModel): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_ORDER_CONFIRMATION_DATA" });
        orderConfirmationService.getOrderConfirmation(values)
            .then(
            (response: OrderConfirmationResponseViewModel) => {
                    dispatch({ type: "RECEIVE_ORDER_CONFIRMATION_DATA", orderconfirmations: response });
                }
            );
    },
};

const confirmationData: OrderConfirmationResponseViewModel = {
    currencyType: undefined,
    transaction: undefined,
    userCardDetail: undefined,
    transactionQrcode: undefined,
    cardTypes: undefined,
    paymentOption: undefined,
    transactionPaymentDetail: undefined,
    orderConfirmationSubContainer: undefined
};


const unloadedState: IOrderConfirmationState = {
    orderconfirmations: confirmationData, isLoading: false, fetchOrderConfirmationSuccess: false, errors: null
};

export const reducer: Reducer<IOrderConfirmationState> = (state: IOrderConfirmationState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_ORDER_CONFIRMATION_DATA":
            return {
                orderconfirmations: state.orderconfirmations,
                isLoading: true,
                fetchOrderConfirmationSuccess: false
            };
        case "RECEIVE_ORDER_CONFIRMATION_DATA":
            return {
                orderconfirmations: action.orderconfirmations,
                isLoading: false,
                fetchOrderConfirmationSuccess: true
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};