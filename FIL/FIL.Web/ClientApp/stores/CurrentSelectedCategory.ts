import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from './';
import { CategoryViewModel } from "../models/CategoryViewModel";

export interface ICurrentSelectedCategoryProps {
    selectedCategory: ICurrentSelectedCategoryState;
}

export interface ICurrentSelectedCategoryState {
    currentSelectedCategory?: CategoryViewModel;
    currentSelectedSubCategory?: CategoryViewModel;
}

interface ISET_CURRENT_SELECTED_CATEGORY extends Action {
    type: 'SET_CURRENT_SELECTED_CATEGORY';
    payload: CategoryViewModel;
}

interface ISET_CURRENT_SELECTED_SUBCATEGORY extends Action {
    type: 'SET_CURRENT_SELECTED_SUBCATEGORY';
    payload: CategoryViewModel;
}

const initialState: ICurrentSelectedCategoryState  = {
    currentSelectedCategory: <CategoryViewModel>{},
    currentSelectedSubCategory: <CategoryViewModel>{}
}

type KnownAction =
    | ISET_CURRENT_SELECTED_CATEGORY
    | ISET_CURRENT_SELECTED_SUBCATEGORY;
   

export const actionCreators = {
    //action for getting category places based on slug for that category
    setCurrentSelectedCategory: (category: CategoryViewModel): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        dispatch({ type: 'SET_CURRENT_SELECTED_CATEGORY', payload: category });
    },
    setCurrentSelectedSubCategory: (category: CategoryViewModel): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        dispatch({ type: 'SET_CURRENT_SELECTED_SUBCATEGORY', payload: category });
    }
};

//reducer functions
export const reducer: Reducer<ICurrentSelectedCategoryState> = (
    state = initialState,
    action: KnownAction) => {

    switch (action.type) {
        case 'SET_CURRENT_SELECTED_CATEGORY':
            return {
                currentSelectedCategory: action.payload,
                currentSelectedSubCategory: action.payload.subCategories[0]
            };
        case 'SET_CURRENT_SELECTED_SUBCATEGORY':
            return {
                ...state,
                currentSelectedSubCategory: action.payload,
            };
        default:
            return {
                ...state
            }
    }
};
