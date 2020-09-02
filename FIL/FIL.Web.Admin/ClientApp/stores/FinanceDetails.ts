import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "./";
import { FinanceDetailResponseViewModel } from "../models/Finance/FinanceDetailResponseViewModel";


export interface IFinanceDetailsProps {
    financeDetails:IFinanceDetailsComponentState;
}

export interface IFinanceDetailsComponentState {
    isLoading: boolean;
    financeDetails: any;

}



interface IRequestFinanceDetail {
    type: "REQUEST_FINANCE_DETAIL_DATA";
}

interface IReceiveFinanceType {
    type: "RECEIVE_FINANCE_DETAIL_DATA";
    payload: any;
}



type KnownAction = IRequestFinanceDetail | IReceiveFinanceType ;

export const actionCreators = {

    requestFinanceDetailsData: (financeModel: string, callback: (FinanceDetailResponseViewModel) => void)


        : IAppThunkAction<KnownAction> => (dispatch, getState) => {

            const fetchTask = fetch(`api/finance/` + financeModel)
                .then((response) => response.json() as Promise<any>)
                .then((data) => {
                    
                    dispatch({ type: "RECEIVE_FINANCE_DETAIL_DATA", payload: data });
                    callback(data);
                },
                err=>{
                    console.log(err);
                });

            addTask(fetchTask);
            dispatch({ type: "REQUEST_FINANCE_DETAIL_DATA" });
        },

        


};

const unloadedFinanceDetails: FinanceDetailResponseViewModel = {
    financeDetail: [],
};





const unloadedFinanceDetailsState: IFinanceDetailsComponentState = {
    isLoading: false,
    financeDetails: unloadedFinanceDetails

};


export const reducer: Reducer<IFinanceDetailsComponentState> = (state: IFinanceDetailsComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_FINANCE_DETAIL_DATA":
            return {
                isLoading: true,
                financeDetails: unloadedFinanceDetails,


            };
        case "RECEIVE_FINANCE_DETAIL_DATA":

            return {

                isLoading: false,
                financeDetails: action.payload,

            };




        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedFinanceDetailsState;
};
