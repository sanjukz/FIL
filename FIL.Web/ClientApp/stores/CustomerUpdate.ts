import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { CustomerUpdateResponseViewModel } from "../models/CustomerUpdate/CustomerUpdateResponseViewModel";

export const CUSTOMER_UPDATE_REQUEST = "CUSTOMER_UPDATE_REQUEST";
export const CUSTOMER_UPDATE_SUCCESS = "CUSTOMER_UPDATE_SUCCESS";
export const CUSTOMER_UPDATE_FAILURE = "CUSTOMER_UPDATE_FAILURE";

export interface ICustomerUpdateComponentState {
    isLoading?: boolean;
    fetchCustomerUpdateSuccess: boolean;
    errors?: any;
	customerUpdate: CustomerUpdateResponseViewModel;
}

const customerUpdateData: CustomerUpdateResponseViewModel = {
	customerUpdates: []
};

interface IRequestCustomerUpdateData {
	type: "REQUEST_CUSTOMER_UPDATE_DATA";
}

interface IReceiveCustomerUpdateData {
	type: "RECEIVE_CUSTOMER_UPDATE_DATA";	
	customerUpdate: CustomerUpdateResponseViewModel;
}

type KnownAction = IRequestCustomerUpdateData | IReceiveCustomerUpdateData ;

export const actionCreators = {
    requestCustomerUpdateData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/customerupdate`)
			.then((response) => response.json() as Promise<CustomerUpdateResponseViewModel>)
            .then((data) => {
				dispatch({ type: "RECEIVE_CUSTOMER_UPDATE_DATA", customerUpdate: data, });
            });
        addTask(fetchTask);
		dispatch({ type: "REQUEST_CUSTOMER_UPDATE_DATA" });

    }
};

const unloadedState: ICustomerUpdateComponentState = {
	customerUpdate: customerUpdateData, isLoading: false, fetchCustomerUpdateSuccess:false, errors: null
};

export const reducer: Reducer<ICustomerUpdateComponentState> = (state: ICustomerUpdateComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
		case "REQUEST_CUSTOMER_UPDATE_DATA":
            return {
				customerUpdate: state.customerUpdate,
                fetchCustomerUpdateSuccess: false,
                isLoading: true
            };
		case "RECEIVE_CUSTOMER_UPDATE_DATA":
            return {
				customerUpdate: action.customerUpdate,
                fetchCustomerUpdateSuccess:true,
                isLoading: false
            };
        
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};