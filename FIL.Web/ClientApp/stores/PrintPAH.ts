import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { PrintPAHResponseViewModel } from "../models/PrintPAHResponseViewModel";


export interface IReprintRequestProps {
    reprintRequest: IPrintPAHComponentPropsState;
}

export interface IPrintPAHComponentPropsState {
    isLoading: boolean;
    errors?: any;
    success?: boolean;
    isPAHLoading?: boolean;
    fetchPAHDataSucess?: boolean;
    pahDetail?: PrintPAHResponseViewModel;
}

interface IRequestPrintPAHData {
    type: "REQUEST_PAH_DATA";
}

interface IReceivePrintPAHData {
    type: "RECEIVE_PAH_DATA";
    pahDetail: PrintPAHResponseViewModel;
}

type KnownAction = IRequestPrintPAHData | IReceivePrintPAHData;

export const actionCreators = {

    printPAHData: (transactionId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "REQUEST_PAH_DATA" });
        const fetchTask = fetch(`api/feelaplace/assignbarcode/${transactionId}`)
            .then((response) => {
                return response.json() as Promise<PrintPAHResponseViewModel>
            })
            .then((data) => {
                dispatch({ type: "RECEIVE_PAH_DATA", pahDetail: data });
            });
        addTask(fetchTask);
    },
};



const emptyPAHDetails: PrintPAHResponseViewModel = {
    pahDetail: [],
    success: false
}


const unloadedState: IPrintPAHComponentPropsState = {
    isLoading: true, errors: null, success: false, isPAHLoading: false

}

export const reducer: Reducer<IPrintPAHComponentPropsState> = (state: IPrintPAHComponentPropsState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "RECEIVE_PAH_DATA":
            return {
                isLoading: false,
                fetchDataSucess: true,
                pahDetail: action.pahDetail,
                fetchPAHDataSucess: true,
                errors: {},
                isPAHLoading: false
            };
        case "REQUEST_PAH_DATA":
            return {
                isLoading: true,
                fetchDataSucess: false,
                pahDetail: emptyPAHDetails,
                fetchPAHDataSucess: false,
                errors: {},
                isPAHLoading: true
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};