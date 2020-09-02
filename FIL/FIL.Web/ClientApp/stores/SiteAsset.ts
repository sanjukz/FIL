import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from './';
import {
    SiteContentViewModel,
    Content,
    SiteBanner,
    DefaultCitySearchResult,
    DefaultStateSearchResult,
    DefaultCountrySearchResult
} from "../models/SiteContentViewModel";
import BlogResponseModelList from '../models/BlogResponseModel';

//Action type constants

export interface ISiteAssestProps {
    siteAsset: ISiteAssestState;
}

export interface ISiteAssestState {
    jumbotron: Content;
    siteBanners: SiteBanner[];
    defaultSearchCities: DefaultCitySearchResult[];
    defaultSearchStates: DefaultStateSearchResult[];
    defaultSearchCountries: DefaultCountrySearchResult[];
    isLoading: boolean;
    fetchSuccess: boolean;
    blogs?: BlogResponseModelList;
}

interface ISITE_ASSET_REQUEST extends Action {
    type: "SITE_ASSET_REQUEST";
}

interface ISITE_ASSET_FAILURE extends Action {
    type: "SITE_ASSET_FAILURE";
}

interface ISITE_ASSET_SUCCESS extends Action {
    type: "SITE_ASSET_SUCCESS";
    payload: SiteContentViewModel;
}
interface IBLOG_REQUEST extends Action {
    type: "BLOG_REQUEST";
}

interface IBLOG_FAILURE extends Action {
    type: "BLOG_FAILURE";
}

interface IBLOG_SUCCESS extends Action {
    type: "BLOG_SUCCESS";
    blogs: BlogResponseModelList;
}
const initialState: ISiteAssestState = {
    jumbotron: <Content>{},
    siteBanners: [],
    defaultSearchCities: [],
    defaultSearchStates: [],
    defaultSearchCountries: [],
    isLoading: false,
    fetchSuccess: false,
    blogs: null
}

type KnownAction =
    | ISITE_ASSET_REQUEST
    | ISITE_ASSET_FAILURE
    | ISITE_ASSET_SUCCESS
    | IBLOG_REQUEST
    | IBLOG_FAILURE
    | IBLOG_SUCCESS;

export const actionCreators = {
    //action for getting category places based on slug for that category
    requestSiteAssest: (): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch("api/content")
            .then(response => response.json() as Promise<SiteContentViewModel>)
            .then(data => {
                dispatch({
                    type: "SITE_ASSET_SUCCESS",
                    payload: data
                });
            })
            .catch(error => {
                dispatch({ type: "SITE_ASSET_FAILURE" });
                throw error;
            });
        addTask(fetchTask);
        dispatch({ type: "SITE_ASSET_REQUEST" });
    },
    requestBlogContent: (callback?: (BlogResponseModelList) => void): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch("api/get/blogs")
            .then(response => response.json() as Promise<BlogResponseModelList>)
            .then(data => {
                if (callback)
                    callback(data);
                dispatch({
                    type: "BLOG_SUCCESS",
                    blogs: data
                });
            })
            .catch(error => {
                dispatch({ type: "BLOG_FAILURE" });
                throw error;
            });
        addTask(fetchTask);
        dispatch({ type: "BLOG_REQUEST" });
    }
};

//reducer functions
export const reducer: Reducer<ISiteAssestState> = (
    state = initialState,
    action: KnownAction) => {

    switch (action.type) {
        case 'SITE_ASSET_REQUEST':
            return {
                ...state,
                isLoading: true,
                fetchSuccess: false
            };
        case 'SITE_ASSET_SUCCESS':
            return {
                jumbotron: action.payload.content,
                siteBanners: action.payload.siteBanners,
                defaultSearchCities: action.payload.defaultSearchCities,
                defaultSearchStates: action.payload.defaultSearchStates,
                defaultSearchCountries: action.payload.defaultSearchCountries,
                isLoading: false,
                fetchSuccess: true
            };

        case 'SITE_ASSET_FAILURE':
            return {
                ...state,
                isLoading: false,
                fetchSuccess: false
            };
        case 'BLOG_REQUEST':
            return {
                ...state,
                isLoading: true,
                fetchSuccess: false
            };
        case 'BLOG_SUCCESS':
            return {
                ...state,
                blogs: action.blogs
            };

        case 'BLOG_FAILURE':
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
