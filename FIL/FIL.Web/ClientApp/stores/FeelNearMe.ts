import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from './';
import { CategoryViewModel } from "../models/CategoryViewModel";
import { CategoryPageResponseViewModel } from "../models/CategoryPageResponseViewModel";
import { FeelNearbyQueryResult } from "../models/FeelNearByLocationsViewModel";

//interfaces
export interface IFeelNearMeProps {
    nearMe: IFeelNearMeState;
}

export interface IFeelNearMeState {
    currentLocation: string;
    nearMePlaces: CategoryPageResponseViewModel[];
    isLoadingPlaces: boolean;
    fetchPlacesSuccess: boolean;
    nearByLocations: string[];
    isLoadingNearByLocations: boolean;
    fetchNearBySuccess: boolean;
}

interface IFeelNearMe {
    category: CategoryViewModel;
    categoryEvents: CategoryPageResponseViewModel[];
}

interface INEARME_PLACES_REQUEST {
    type: 'NEARME_PLACES_REQUEST';
}

interface INEARME_PLACES_SUCCESS {
    type: 'NEARME_PLACES_SUCCESS';
    payload: {
        palces: CategoryPageResponseViewModel[];
        location: string;
    };
}

interface INEARME_PLACES_FAILURE {
    type: 'NEARME_PLACES_FAILURE';
}

interface INEARBY_LOCATIONS_REQUEST {
    type: 'NEARBY_LOCATIONS_REQUEST';
}

interface INEARBY_LOCATIONS_SUCCESS {
    type: 'NEARBY_LOCATIONS_SUCCESS';
    payload: FeelNearbyQueryResult;
}

interface INEARBY_LOCATIONS_FAILURE {
    type: 'NEARBY_LOCATIONS_FAILURE';
}

const initialState: IFeelNearMeState = {
    currentLocation: "",
    nearMePlaces: [],
    isLoadingPlaces: false,
    fetchPlacesSuccess: false,
    nearByLocations: [],
    isLoadingNearByLocations: false,
    fetchNearBySuccess: false
};

export type KnownAction =
    | INEARME_PLACES_REQUEST
    | INEARME_PLACES_SUCCESS
    | INEARME_PLACES_FAILURE
    | INEARBY_LOCATIONS_REQUEST
    | INEARBY_LOCATIONS_SUCCESS
    | INEARBY_LOCATIONS_FAILURE;

export const actionCreators = {
    //action for getting page metadata
    /*getNearByPlacesByPagination: (
        index: number,
        slug: string,
        search: string
    ): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(
            `api/placeByIndex/${index}?slug=${slug}?${search}`
        )
            .then(response => response.json() as Promise<IFeelNearMe>)
            .then(data => {
                dispatch({
                    type: 'NEARME_PLACES_SUCCESS',
                    payload: {
                        palces: data.categoryEvents,
                        location: search
                    }
                });
            })
            .catch((error) => {
                Raven.captureException(error);
                dispatch({ type: 'NEARME_PLACES_FAILURE' });
            });
        addTask(fetchTask);
        dispatch({ type: 'NEARME_PLACES_REQUEST' });
    },*/

    getNearByLocations: (
        lat: number,
        lon: number,
        distance: number
    ): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        return new Promise((resolve, reject) => {
            fetch(
                `api/nearby?lat=${lat}&lon=${lon}&distance=${distance}`
            )
                .then(response => response.json() as Promise<FeelNearbyQueryResult>)
                .then(data => {
                    dispatch({
                        type: 'NEARBY_LOCATIONS_SUCCESS',
                        payload: data
                    });
                    resolve(data);
                })
                .catch((error) => {
                    dispatch({ type: 'NEARBY_LOCATIONS_FAILURE' });
                    reject(error);
                });
            dispatch({ type: 'NEARBY_LOCATIONS_REQUEST' });
        })
    }
};

//reducer functions
export const reducer: Reducer<IFeelNearMeState> = (
    state: IFeelNearMeState = initialState,
    action: KnownAction) => {

    switch (action.type) {
        case 'NEARME_PLACES_REQUEST':
            return {
                ...state,
                isLoadingPlaces: true,
                fetchPlacesSuccess: false
            };
        case 'NEARME_PLACES_SUCCESS':
            return {
                ...state,
                isLoadingPlaces: false,
                nearMeLocation: action.payload.location,
                nearMePlaces: action.payload.palces,
                fetchPlacesSuccess: true
            };

        case 'NEARME_PLACES_FAILURE':
            return {
                ...state,
                isLoadingPlaces: false,
                fetchPlacesSuccess: false
            };

        case 'NEARBY_LOCATIONS_REQUEST':
            return {
                ...state,
                isLoadingNearByLocations: true,
                fetchNearBySuccess: false
            };

        case 'NEARBY_LOCATIONS_SUCCESS':
            return {
                ...state,
                nearByLocations: action.payload.nearbyPlaces.map(item => item.city),
                isLoadingNearByLocations: false,
                fetchNearBySuccess: true
            };

        case 'NEARBY_LOCATIONS_FAILURE':
            return {
                ...state,
                isLoadingNearByLocations: false,
                fetchNearBySuccess: false
            };
        default:
            return {
                ...state
            }
    }
};
