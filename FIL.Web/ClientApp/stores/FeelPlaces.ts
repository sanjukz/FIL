import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from './';
import { CategoryPageResponseViewModel } from "../models/CategoryPageResponseViewModel";
import { CategoryViewModel } from "../models/CategoryViewModel";

//Action type constants
interface IFeelPlaces {
    category: CategoryViewModel;
    categoryEvents: CategoryPageResponseViewModel[];
}

export interface IFeelPlacesPorps {
    categoryPlaces: IFeelPlacesState;
}

interface IPlacesBySlug {
    [slug: string]: CategoryPageResponseViewModel[];
}

export interface IFeelPlacesState {
    currentCategory: CategoryViewModel;
    placesBySlug: IPlacesBySlug;
    isLoading: boolean;
    isHideLiveSection?: boolean;
    fetchSuccess: boolean;
}

interface IFEEL_PLACES_REQUEST extends Action {
    type: 'FEEL_PLACES_REQUEST';
}

interface IFEEL_PLACES_FAILURE extends Action {
    type: 'FEEL_PLACES_FAILURE';
}

interface IFEEL_PLACES_SUCCESS extends Action {
    type: 'FEEL_PLACES_SUCCESS';
    payload: IFeelPlaces;
}

interface IFEEL_CATEGORY_TOGGLE extends Action {
    type: 'FEEL_CATEGORY_TOGGLE';
    payload: boolean;
}

const initialState: IFeelPlacesState = {
    currentCategory: <CategoryViewModel>{},
    placesBySlug: <IPlacesBySlug>{},
    isLoading: false,
    fetchSuccess: false,
    isHideLiveSection: false
}

type KnownAction =
    | IFEEL_PLACES_REQUEST
    | IFEEL_PLACES_FAILURE
    | IFEEL_PLACES_SUCCESS
    | IFEEL_CATEGORY_TOGGLE;

export const actionCreators = {
    //action for getting category places based on slug for that category
    getCategoryPlaces: (slug: string, callback: (any) => void): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch(`api/category/${slug}`)
            .then(response => {
                return response.json() as Promise<IFeelPlaces>
            })
            .then(data => {
                dispatch({ type: 'FEEL_PLACES_SUCCESS', payload: data });
                callback(data);
            })
            .catch((error) => {
                dispatch({ type: 'FEEL_PLACES_FAILURE' })
                throw error;
            });
        addTask(fetchTask);
        dispatch({ type: 'FEEL_PLACES_REQUEST' });
    },
    //action for show/ Hide Live section from homepage
    updateShowHideState: (isHide: boolean): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        dispatch({ type: 'FEEL_CATEGORY_TOGGLE', payload: isHide });
    }
};

//reducer functions
export const reducer: Reducer<IFeelPlacesState> = (
    state = initialState,
    action: KnownAction) => {

    switch (action.type) {
        case 'FEEL_PLACES_REQUEST':
            return {
                ...state,
                isLoading: true,
                fetchSuccess: false
            };
        case 'FEEL_PLACES_SUCCESS':
            return {
                isLoading: false,
                isHideLiveSection: state.isHideLiveSection,
                currentCategory: action.payload.category,
                placesBySlug: { ...state.placesBySlug, [action.payload.category.slug]: action.payload.categoryEvents },
                fetchSuccess: true
            };

        case 'FEEL_PLACES_FAILURE':
            return {
                ...state,
                isLoading: false,
                fetchSuccess: false
            };
        case 'FEEL_CATEGORY_TOGGLE':
            return {
                ...state,
                isHideLiveSection: action.payload
            };
        default:
            return {
                ...state
            }
    }
};
