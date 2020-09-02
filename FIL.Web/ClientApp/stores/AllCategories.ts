import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from './';
import { CategoryViewModel } from "../models/CategoryViewModel";
import { CategoryResponseViewModel } from "../models/CategoryResponseViewModel";

//Action type constants

export interface IAllCategoriesProps {
    allCategories: IAllCategoriesState;
}

export interface IAllCategoriesState {
    allCategories: CategoryViewModel[];
    isLoading: boolean;
    fetchSuccess: boolean;
}

interface IALL_CATEGORIES_REQUEST extends Action {
    type: 'ALL_CATEGORIES_REQUEST';
}

interface IALL_CATEGORIES_FAILURE extends Action {
    type: 'ALL_CATEGORIES_FAILURE';
}

interface IALL_CATEGORIES_SUCCESS extends Action {
    type: 'ALL_CATEGORIES_SUCCESS';
    payload: CategoryResponseViewModel;
}

const initialState: IAllCategoriesState = {
    allCategories: [],
    isLoading: false,
    fetchSuccess: false
}

type KnownAction =
    | IALL_CATEGORIES_REQUEST
    | IALL_CATEGORIES_FAILURE
    | IALL_CATEGORIES_SUCCESS;

export const actionCreators = {
    //action for getting category places based on slug for that category
    getAllCategories: (): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch(`api/category`)
            .then(response => {
                return response.json() as Promise<CategoryResponseViewModel>
            })
            .then(data => {
                dispatch({ type: 'ALL_CATEGORIES_SUCCESS', payload: data });
            })
            .catch((error) => {
                dispatch({ type: 'ALL_CATEGORIES_FAILURE' })
            });
        addTask(fetchTask);
        dispatch({ type: 'ALL_CATEGORIES_REQUEST' });
    }
};

//reducer functions
export const reducer: Reducer<IAllCategoriesState> = (
    state = initialState,
    action: KnownAction) => {

    switch (action.type) {
        case 'ALL_CATEGORIES_REQUEST':
            return {
                ...state,
                isLoading: true,
                fetchSuccess: false
            };
        case 'ALL_CATEGORIES_SUCCESS':
            return {
                allCategories: action.payload.categories,
                isLoading: false,
                fetchSuccess: true
            };

        case 'ALL_CATEGORIES_FAILURE':
            return {
                ...state,
                isLoading: false,
                fetchSuccess: false
            };
        default:
            return {
                ...state
            }
    }
};
