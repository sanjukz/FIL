import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from ".";
import { FinanceResponseViewModel } from  "../models/Finance/FinanceResponseViewModel";


export interface ISaveFinancialsProps {
    saveResponse: ISaveFinancialComponentState;
}

export interface ISaveFinancialComponentState {
    isLoading: boolean;
    saveSuccess: boolean;
   
}


interface IRequestSaveDataTypes {
    type: "SAVE_FINANCE_TYPE_DATA";
}

interface IReceiveSaveDataTypes {
    type: "RECEIVE_FINANCE_TYPE_DATA";
}


type KnownAction = IRequestSaveDataTypes|IReceiveSaveDataTypes  ;

export const actionCreators = {
  
    saveFinancialDetails: (InviteModel: FinanceResponseViewModel, callback: (FinanceResponseViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "SAVE_FINANCE_TYPE_DATA" });
        
        fetch('/api/invite/edit', {
            method: 'POST',
            body: JSON.stringify(InviteModel),
            headers:{
              'Content-Type': 'application/json'
            }
          }).then((response:any) => {    
              
                
                if (response.ok) {
                    dispatch({ type: "SAVE_FINANCE_TYPE_DATA" });
                }
               
                callback(response);
            },
           );
    },
    

};

// const unloadedSaveFinancial: FinanceResponseViewModel = {
//     financialsAccountFirstName: "",
//     isUsefinancialsAccountLastNamed: "",
//     financialsAccountLocationPlaceName: ""
// };





const unloadedSaveFinancialState: ISaveFinancialComponentState = {
    isLoading: false,
    saveSuccess: false,
    

};


export const reducer: Reducer<ISaveFinancialComponentState> = (state: ISaveFinancialComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
      
            case "SAVE_FINANCE_TYPE_DATA":
    
            return {
                
                isLoading: true,
                saveSuccess: true,
            };

            case "RECEIVE_FINANCE_TYPE_DATA":
    
            return {
                isLoading: true,
                saveSuccess: true,
            };

        
      
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedSaveFinancialState;
};
