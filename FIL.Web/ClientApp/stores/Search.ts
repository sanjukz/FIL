import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { SearchResponseViewModel } from "../models/Search/SearchResponseViewModel";

export interface ISearchComponentProps {
    search: ISearchComponentState;
}

export interface ISearchComponentState {
    searchText: string;
    requesting: boolean;
    searchSuccess: boolean;
    clearSearchSuccess: boolean;
    searchResult: SearchResponseViewModel;
}

interface IRequestSearchDataAction {
    type: "REQUEST_SEARCH_DATA";
    searchText: string;
}

interface IReceiveSearchDataAction {
    searchResult: SearchResponseViewModel;
    type: "RECEIVE_SEARCH_DATA";
}

interface IClearSearchDataAction {
    type: "CLEAR_SEARCH_DATA";
}

const emptySearchResult: SearchResponseViewModel = {
    categoryEvents: [],
    cities: [],
    states: [],
    countries: [],
};

const emptySearchState: ISearchComponentState = {
    searchText: "",
    requesting: false,
    searchSuccess: false,
    clearSearchSuccess: true,
    searchResult: emptySearchResult,
};

type KnownAction = IRequestSearchDataAction | IReceiveSearchDataAction | IClearSearchDataAction;

export interface ISearchBarRequestDataViewModel {
    search: string;
}

export const actionCreators = {
    searchAction: (searchString: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/search/${searchString}`)
            .then((response) => response.json() as Promise<SearchResponseViewModel>)
            .then((data) => {
                dispatch({
                    type: "RECEIVE_SEARCH_DATA",
                    searchResult: data
                });
            });
        addTask(fetchTask);
        dispatch({
            type: "REQUEST_SEARCH_DATA",
            searchText: searchString
        });
    },
    clearSearchAction: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ 
            type: "CLEAR_SEARCH_DATA" 
        });
    }
};

export const reducer: Reducer<ISearchComponentState> = (state: ISearchComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_SEARCH_DATA":
            return {
                searchText: action.searchText,
                searchResult: state.searchResult,
                requesting: true,
                searchSuccess: false,
                clearSearchSuccess: false,
            };
        case "RECEIVE_SEARCH_DATA":
            return {
                searchText: state.searchText,
                searchResult: action.searchResult,
                requesting: false,
                searchSuccess: true,
                clearSearchSuccess: false,
            };
        case "CLEAR_SEARCH_DATA":
            return {
                searchText: "",
                searchResult: emptySearchResult,
                requesting: false,
                searchSuccess: false,
                clearSearchSuccess: true,
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || emptySearchState;
};