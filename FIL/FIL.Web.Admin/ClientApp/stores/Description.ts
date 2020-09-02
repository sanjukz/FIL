import { addTask, fetch } from "domain-task";
import { Action, Reducer } from "redux";
import CitiesResponseViewModel from "../models/Description/CitiesResponseViewModel";
import { IAppThunkAction } from "./";
import { descriptionService } from "../services/Description";
import DescriptionInputViewModel from "../models/Description/DescriptionInputViewModel";
import DescriptionResponseViewModel from "../models/Description/DescriptionResponseViewModel";
import CityCountryDescriptionResponseViewModel from "../models/Description/CityCountryDescriptionResponseViewModel";

export interface DescriptionComponentProps {
    Description: DescriptionComponentState;
}

export interface DescriptionComponentState {
    isLoading?: boolean;
    errors?: any;
    fetchSearchSuccess?: boolean;
    citiesResult: CitiesResponseViewModel;
    descriptionResponseViewModel: DescriptionResponseViewModel;
    descriptionResult: CityCountryDescriptionResponseViewModel;
    isError?: boolean;
}

interface IRequestCitiesData {
    type: "REQUEST_CITIES_DATA";
}

interface IReceiveCitiesData {
    type: "RECEIVE_CITIES_DATA";
    result: CitiesResponseViewModel;
}

interface IDescriptionSaveRequestAction {
    type: "DESCRIPTION_SAVE_REQUEST_ACTION";
}

interface IDescriptionReuest {
    type: "REQUEST_DESCRIPTION_DATA";
}

interface IReceiveDescriptionData {
    type: "RECEIVE_DESCRIPTION_DATA";
    descriptionResult: CityCountryDescriptionResponseViewModel;
}

interface IDescriptionSuccesstAction {
    type: "DESCRIPTION_SAVE_SUCCESS_ACTION";
    descriptionResponseViewModel: DescriptionResponseViewModel;
}

interface IDescriptionSaveFailureAction {
    type: "DESCRIPTION_SAVE_FAILURE_ACTION";
}

type KnownAction = IRequestCitiesData | IReceiveCitiesData | IDescriptionSaveRequestAction | IDescriptionSuccesstAction | IDescriptionSaveFailureAction
    | IDescriptionReuest | IReceiveDescriptionData;

export const actionCreators = {
    requestDescriptionSearchData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/active/cities`)
            .then((response) => response.json() as Promise<CitiesResponseViewModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_CITIES_DATA", result: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_CITIES_DATA" });
    },
    saveDescription: (descriptionModel: DescriptionInputViewModel, callback: (DescriptionResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "DESCRIPTION_SAVE_REQUEST_ACTION" });
            descriptionService.saveDescription(descriptionModel)
                .then((response: DescriptionResponseViewModel) => {
                    dispatch({ type: "DESCRIPTION_SAVE_SUCCESS_ACTION", descriptionResponseViewModel: response });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: "DESCRIPTION_SAVE_FAILURE_ACTION" });
                });
        },
    getDescription: (descriptionModel: DescriptionInputViewModel, callback: (CityCountryDescriptionResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "REQUEST_DESCRIPTION_DATA" });
            descriptionService.getDescription(descriptionModel)
                .then((response: CityCountryDescriptionResponseViewModel) => {
                    dispatch({ type: "RECEIVE_DESCRIPTION_DATA", descriptionResult: response });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: "DESCRIPTION_SAVE_FAILURE_ACTION" });
                });
        },
};

const emptyCitiesMasterModel: CitiesResponseViewModel = {
    itinerarySerchData: [],
    feelStateData: []
};

const emptyDescriptionResponseViewModel: DescriptionResponseViewModel = {
    success: false
};

const emptyGetDescriptionModel: CityCountryDescriptionResponseViewModel = {
    success: false,
    description: ""
};

const unloadedState: DescriptionComponentState = {
    errors: null,
    fetchSearchSuccess: false,
    citiesResult: emptyCitiesMasterModel,
    isLoading: false,
    isError: false,
    descriptionResponseViewModel: emptyDescriptionResponseViewModel,
    descriptionResult: emptyGetDescriptionModel
};

export const reducer: Reducer<DescriptionComponentState> = (state: DescriptionComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_CITIES_DATA":
            return {
                citiesResult: emptyCitiesMasterModel,
                descriptionResponseViewModel: state.descriptionResponseViewModel,
                fetchSearchSuccess: false,
                descriptionResult: state.descriptionResult
            };
        case "RECEIVE_CITIES_DATA":
            return {
                citiesResult: action.result,
                descriptionResponseViewModel: state.descriptionResponseViewModel,
                descriptionResult: state.descriptionResult,
                fetchSearchSuccess: true
            };
        case "DESCRIPTION_SAVE_REQUEST_ACTION":
            return {
                citiesResult: state.citiesResult,
                fetchSearchSuccess: state.fetchSearchSuccess,
                descriptionResponseViewModel: emptyDescriptionResponseViewModel,
                descriptionResult: state.descriptionResult
            };
        case "DESCRIPTION_SAVE_SUCCESS_ACTION":
            return {
                citiesResult: state.citiesResult,
                fetchSearchSuccess: state.fetchSearchSuccess,
                descriptionResponseViewModel: action.descriptionResponseViewModel,
                descriptionResult: state.descriptionResult
            };
        case "DESCRIPTION_SAVE_FAILURE_ACTION":
            return {
                citiesResult: state.citiesResult,
                fetchSearchSuccess: state.fetchSearchSuccess,
                descriptionResponseViewModel: emptyDescriptionResponseViewModel,
                descriptionResult: state.descriptionResult
            };
        case "REQUEST_DESCRIPTION_DATA":
            return {
                citiesResult: state.citiesResult,
                fetchSearchSuccess: state.fetchSearchSuccess,
                descriptionResponseViewModel: state.descriptionResponseViewModel,
                descriptionResult: emptyGetDescriptionModel
            };
        case "RECEIVE_DESCRIPTION_DATA":
            return {
                citiesResult: state.citiesResult,
                fetchSearchSuccess: state.fetchSearchSuccess,
                descriptionResponseViewModel: state.descriptionResponseViewModel,
                descriptionResult: action.descriptionResult
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};