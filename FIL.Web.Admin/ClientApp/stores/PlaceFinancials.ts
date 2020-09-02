import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "./";
import { CurrencyTypesResponseViewModel } from '../models/Inventory/CurrencyTypesResponseViewModel';
import { FinanceResponseViewModel } from "../models/Finance/FinanceResponseViewModel";

export interface ICurrencyTypeProps {
    currencyType: ICurrencyTypeComponentState;
}

export interface ICurrencyTypeComponentState {
    isLoading: boolean;
    currencyTypeSuccess: boolean;
    currencyTypes: CurrencyTypesResponseViewModel;

}



interface IRequestCurrencyTypes {
    type: "REQUEST_CURRENCY_TYPE_DATA";
}

interface IReceiveCurrencyTypes {
    type: "RECEIVE_CURRENCY_TYPE_DATA";
    currencyTypes: CurrencyTypesResponseViewModel;
}

interface IRequestSaveDataTypes {
    type: "SAVE_FINANCE_TYPE_DATA";
}

interface IReceiveSaveDataTypes {
    type: "RECEIVE_FINANCE_TYPE_DATA";
}


type KnownAction = IRequestCurrencyTypes | IReceiveCurrencyTypes | IRequestSaveDataTypes | IReceiveSaveDataTypes;

export const actionCreators = {
    requestCurrencyTypeData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {

        const fetchTask = fetch(`api/get/Currencies`)
            .then((response) => response.json() as Promise<CurrencyTypesResponseViewModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_CURRENCY_TYPE_DATA", currencyTypes: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_CURRENCY_TYPE_DATA" });
    },

    saveFinancialDetails: (FinanceDetilModel: FinanceResponseViewModel, callback: (FinanceResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            
            dispatch({ type: "SAVE_FINANCE_TYPE_DATA" });

            fetch('api/financial/save', {
                method: 'POST',
                body: JSON.stringify(FinanceDetilModel),
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then((response: any) => {
                if (response.ok) {
                    alert('Your financial information has been saved successfully!');
                    dispatch({ type: "SAVE_FINANCE_TYPE_DATA" });
                }
                else{
                    alert('Error while saving finance details!');
                }
                callback(response);
            },
            );
        },


};

const unloadedCurrencyTypes: CurrencyTypesResponseViewModel = {
    currencyTypes: [],
};





const unloadedCurrencyTypeState: ICurrencyTypeComponentState = {
    isLoading: false,
    currencyTypeSuccess: false,
    currencyTypes: unloadedCurrencyTypes,

};


export const reducer: Reducer<ICurrencyTypeComponentState> = (state: ICurrencyTypeComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_CURRENCY_TYPE_DATA":
            return {
                isLoading: true,
                currencyTypeSuccess: false,
                currencyTypes: unloadedCurrencyTypes,


            };
        case "RECEIVE_CURRENCY_TYPE_DATA":

            return {

                isLoading: false,
                currencyTypeSuccess: true,
                currencyTypes: action.currencyTypes,

            };

        case "SAVE_FINANCE_TYPE_DATA":

            return {

                isLoading: true,
                currencyTypeSuccess: true,
                currencyTypes: this.state.currencyTypes,
            };

        case "RECEIVE_FINANCE_TYPE_DATA":

            return {

                isLoading: true,
                currencyTypeSuccess: true,
                currencyTypes: this.state.currencyTypes,
            };



        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedCurrencyTypeState;
};
