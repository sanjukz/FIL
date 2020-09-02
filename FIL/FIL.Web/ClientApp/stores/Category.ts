import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { CategoryDataModel } from "../models/CategoryDataModel";
import { CategoryResponseViewModel } from "../models/CategoryResponseViewModel";
import {
    SiteContentViewModel,
    Content,
    SiteBanner,
    DefaultCitySearchResult,
    DefaultStateSearchResult,
    DefaultCountrySearchResult
} from "../models/SiteContentViewModel";
import { CategoryViewModel } from "../models/CategoryViewModel";
import { PackagesViewModel } from "../models/PackagesViewModel";
import { CategoryPageResponseViewModel } from "../models/CategoryPageResponseViewModel";
import {
    SearchResponseViewModel
} from "../models/Search/SearchResponseViewModel";
import { ItinerarySearchResponseModel } from "../models/ItinerarySearchResponseModel";
export const REQUEST_SEARCH_DATA = "REQUEST_SEARCH_DATA";
export const RECEIVE_SEARCH_DATA = "RECEIVE_SEARCH_DATA";
export const CLEAR_SEARCH_DATA = "CLEAR_SEARCH_DATA";
export const REQUEST_PACKAGES_DATA = "REQUEST_PACKAGES_DATA";
export const RECEIVE_PACKAGES_DATA = "RECEIVE_PACKAGES_DATA";
import * as algoliasearch from "algoliasearch";

const searchClient = algoliasearch(                            // intializing alogolia client
    '8AMLTQYRXE',                                               //ToDo Set this keys in environmental variable
    '122a9a1f411959c3cd46db326c56334a'
);
const index = searchClient.initIndex('Places');

export interface ICategoryProps {
    category: ICategoryState;
}

export interface ICategoryState {
    isLoading?: boolean;
    isLoadingCategory?: boolean;
    siteContentFetchSuccess?: boolean;
    requesting: boolean;
    searchSuccess: boolean;
    clearSearchSuccess: boolean;
    searchResult: SearchResponseViewModel;
    errors?: any;
    fetchsuccess?: boolean;
    categories: CategoryViewModel[];
    content: Content;
    siteBanners: SiteBanner[];
    defaultSearchCities: DefaultCitySearchResult[];
    defaultSearchStates: DefaultStateSearchResult[];
    defaultSearchCountries: DefaultCountrySearchResult[];
    category: ICategoryEventData;
    fetchCategorySuccess?: boolean;
    packages?: PackagesViewModel;
    packagesFetchStatus?: boolean;
    currentLocation?: string;
    countryCities?: ItinerarySearchResponseModel[];
    fetchCountryCities?: boolean;
    fetchAlgoliaResults?: boolean;
    algoliaResults?: any;
}

interface IRequestAlgoliaSearchDataAction {
    type: "REQUEST_ALGOLIA_SEARCH_DATA";
}

interface IReceiveAlgoliaSearchDataAction {
    algoliaResults: any;
    type: "RECEIVE_ALGOLIA_SEARCH_DATA";
}
interface IFailureAlgoliaSearchDataAction {
    error: any;
    type: "FAILURE_ALGOLIA_SEARCH_DATA";
}
export interface ICategoryEventData {
    category: CategoryViewModel;
    categoryEvents: CategoryPageResponseViewModel[];
}

export interface IPackagesEventData {
    packages: PackagesViewModel;
}

interface IRequestSearchDataAction {
    type: "REQUEST_SEARCH_DATA";
}

interface IReceiveSearchDataAction {
    searchResult: SearchResponseViewModel;
    type: "RECEIVE_SEARCH_DATA";
}

interface IClearSearchDataAction {
    type: "CLEAR_SEARCH_DATA";
}

interface IRequestCategoryData {
    type: "REQUEST_CATEGORY_DATA";
}

interface IReceiveCategoryData {
    type: "RECEIVE_CATEGORY_DATA";
    categories: CategoryViewModel[];
}

interface IRequestPackagesData {
    type: "REQUEST_PACKAGES_DATA";
}

interface IReceivePackagesData {
    type: "RECEIVE_PACKAGES_DATA";
    packages: PackagesViewModel;
}

interface IRequestSiteContentData {
    type: "REQUEST_SITE_CONTENT";
}

interface IReceiveSiteContentData {
    type: "RECEIVE_SITE_CONTENT";
    content: Content;
    siteBanners: SiteBanner[];
    defaultSearchCities: DefaultCitySearchResult[];
    defaultSearchStates: DefaultStateSearchResult[];
    defaultSearchCountries: DefaultCountrySearchResult[];
}

interface IRequestCategoryPageData {
    type: "REQUEST_CATEGORY_PAGE_DATA";
}

interface IReceiveCategoryPageData {
    type: "RECEIVE_CATEGORY_PAGE_DATA";
    category: ICategoryEventData;
}

interface IChangeNearMeData {
    type: "CHANGE_NEAR_ME_DATA",
    currentLocation: any
}
interface IRequestCityCountryData {
    type: "REQUEST_COUNTRYCITY_DATA",
}interface IRecieveCityCountryData {
    type: "RECIEVE_COUNTRYCITY_DATA",
    countryCities: any
}

const emptySearchResult: SearchResponseViewModel = {
    categoryEvents: [],
    cities: [],
    states: [],
    countries: []
};

const emptyCategory: CategoryViewModel = {
    id: 0,
    eventCategory: 0,
    eventCategoryId: 0,
    categoryId: 0,
    displayName: "",
    slug: "",
    order: null,
    isHomePage: false,
    isFeel: true,
    subCategories: []
};

const emptyPackages: PackagesViewModel = {
    categoryId: 0,
    displayName: "",
    slug: "",
    eventCategory: null,
    order: null,
    isHomePage: false,
    isFeel: true,
    subCategories: []
};

const emptyContent: Content = {
    altId: "",
    siteTitle: "",
    siteLogo: "",
    bannerText: "",
    siteLevel: ""
};

export interface ISearchBarRequestDataViewModel {
    search: string;
}

type KnownAction =
    | IRequestCategoryData
    | IReceiveCategoryData
    | IRequestCategoryPageData
    | IReceiveCategoryPageData
    | IRequestSearchDataAction
    | IReceiveSearchDataAction
    | IClearSearchDataAction
    | IRequestSiteContentData
    | IReceiveSiteContentData
    | IReceivePackagesData
    | IRequestPackagesData
    | IChangeNearMeData
    | IRecieveCityCountryData |
    IRequestCityCountryData | IRequestAlgoliaSearchDataAction | IReceiveAlgoliaSearchDataAction
    | IFailureAlgoliaSearchDataAction;

export const actionCreators = {
    requestCategoryData: (): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch("api/category")
            .then(response => response.json() as Promise<CategoryResponseViewModel>)
            .then(data => {
                dispatch({
                    type: "RECEIVE_CATEGORY_DATA",
                    categories: data.categories
                });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_CATEGORY_DATA" });
    },
    requestCategoryByLocation: (cityIds: string): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch(`api/LocationCategories/${cityIds}`)
            .then(response => response.json() as Promise<CategoryResponseViewModel>)
            .then(data => {
                dispatch({
                    type: "RECEIVE_CATEGORY_DATA",
                    categories: data.categories
                });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_CATEGORY_DATA" });
    },
    requestSiteContent: (): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch("api/content")
            .then(response => response.json() as Promise<SiteContentViewModel>)
            .then(data => {
                dispatch({
                    type: "RECEIVE_SITE_CONTENT",
                    content: data.content,
                    siteBanners: data.siteBanners,
                    defaultSearchCities: data.defaultSearchCities,
                    defaultSearchStates: data.defaultSearchStates,
                    defaultSearchCountries: data.defaultSearchCountries
                });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_SITE_CONTENT" });
    },

    getCategoryEvents: (slug: string): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch(`api/category/${slug}`)
            .then(response => response.json() as Promise<ICategoryEventData>)
            .then(data => {
                dispatch({ type: "RECEIVE_CATEGORY_PAGE_DATA", category: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_CATEGORY_PAGE_DATA" });
    },

    getCategoryEventsByPaginationidex: (
        index: number,
        slug: string,
        search: string
    ): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(
            `api/placeByIndex/${index}?slug=${slug}?${search}`
        )
            .then(response => response.json() as Promise<ICategoryEventData>)
            .then(data => {
                dispatch({ type: "RECEIVE_CATEGORY_PAGE_DATA", category: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_CATEGORY_PAGE_DATA" });
    },

    //same as the above action but returns promise so that it can be used to chain on resposnse
    getNearbyPlacesByPagination: (
        index: number,
        slug: string,
        search: string
    ): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        return new Promise((resolve, reject) => {
            fetch(`api/placeByIndex/${index}?slug=${slug}?${search}`)
                .then(response => response.json() as Promise<ICategoryEventData>)
                .then(data => {
                    dispatch({ type: "RECEIVE_CATEGORY_PAGE_DATA", category: data });
                    resolve(data);
                })
                .catch(error => {
                    reject(error);
                });
            dispatch({ type: "REQUEST_CATEGORY_PAGE_DATA" });
        });
    },

    requestSelectedSearchData: (
        search: string,
        searchType: string,
        slug: string
    ): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(
            `api/selectedSearch/${search}?searchType=${searchType}?${slug}`
        )
            .then(response => response.json() as Promise<ICategoryEventData>)
            .then(data => {
                dispatch({ type: "RECEIVE_CATEGORY_PAGE_DATA", category: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_CATEGORY_PAGE_DATA" });
    },

    getSearchEvents: (search: string): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch(
            `api/searchcategory/${search}`
        )
            .then(response => response.json() as Promise<ICategoryEventData>)
            .then(data => {
                dispatch({ type: "RECEIVE_CATEGORY_PAGE_DATA", category: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_CATEGORY_PAGE_DATA" });
    },

    searchAction: (
        searchString: string,
        siteLevel: number
    ): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(
            `api/search/${searchString}?siteLevel=${siteLevel}`
        )
            .then(response => response.json() as Promise<SearchResponseViewModel>)
            .then(data => {
                dispatch({ type: "RECEIVE_SEARCH_DATA", searchResult: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_SEARCH_DATA" });
    },

    getPassesPackages: (slug: string): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        const fetchTask = fetch(`api/category/${slug}`)
            .then(response => response.json() as Promise<PackagesViewModel>)
            .then(data => {
                dispatch({ type: "RECEIVE_PACKAGES_DATA", packages: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_PACKAGES_DATA" });
    },

    clearSearchAction: (): IAppThunkAction<KnownAction> => (
        dispatch,
        getState
    ) => {
        dispatch({ type: "CLEAR_SEARCH_DATA" });
    },

    changeNearMeData: (loc: any): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "CHANGE_NEAR_ME_DATA", currentLocation: loc });
    },
    getCountryCities: (): IAppThunkAction<KnownAction> => (
        dispatch, getState
    ) => {
        const fetchTask = fetch(`api/itinerary/cities`)
            .then(response => response.json() as Promise<ItinerarySearchResponseModel>)
            .then(data => {
                dispatch({
                    type: "RECIEVE_COUNTRYCITY_DATA",
                    countryCities: data
                })
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_COUNTRYCITY_DATA" })
    },
    getAlgoliaResults: (keyword: string): IAppThunkAction<KnownAction> => (
        dispatch, getState
    ) => {
        index
            .search({ query: keyword, hitsPerPage: 10 })
            .then(({ hits }) => {
                dispatch({
                    type: "RECEIVE_ALGOLIA_SEARCH_DATA",
                    algoliaResults: hits
                })
            })
            .catch(err => {
                dispatch({ type: "FAILURE_ALGOLIA_SEARCH_DATA", error: err })
            });
        dispatch({ type: "REQUEST_ALGOLIA_SEARCH_DATA" })

    },
};



/*const emptyCategoryEvents = {
    event: {},
    venue: [],
    city: [],
    state: [],
    country: [],
    rating: [],
    categoryEvent: {},
    eventTicketAttribute: [],
    eventTicketDetail: [],
    eventCategories: [],
    currencyType: {},
    eventType: "",
    eventCategory: "",
    parentCategory: ""
};*/

const categoryData: ICategoryEventData = {
    category: emptyCategory,
    categoryEvents: []
};

const packagesData: PackagesViewModel = {
    displayName: null,
    slug: null,
    eventCategory: null,
    order: null,
    isHomePage: null,
    categoryId: null,
    isFeel: null,
    subCategories: null
};

const unloadedState: ICategoryState = {
    categories: [],
    content: emptyContent,
    isLoading: false,
    errors: null,
    packagesFetchStatus: false,
    fetchsuccess: false,
    category: categoryData,
    currentLocation: null,
    fetchCategorySuccess: false,
    requesting: false,
    searchSuccess: false,
    clearSearchSuccess: true,
    searchResult: emptySearchResult,
    siteBanners: [],
    defaultSearchCities: [],
    defaultSearchStates: [],
    defaultSearchCountries: [],
    isLoadingCategory: false,
    siteContentFetchSuccess: false,
    packages: packagesData,
    countryCities: [],
    fetchCountryCities: false,
    algoliaResults: [],
    fetchAlgoliaResults: false
};

export const reducer: Reducer<ICategoryState> = (
    state: ICategoryState,
    incomingAction: Action
) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_CATEGORY_DATA":
            return {
                ...state,
                categories: state.categories,
                content: state.content,
                siteBanners: state.siteBanners,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                fetchCategorySuccess: state.fetchCategorySuccess,
                isLoading: true,
                fetchsuccess: false,
                requesting: false,
                searchSuccess: false,
                clearSearchSuccess: true,
                category: state.category,
                isLoadingCategory: true,
                siteContentFetchSuccess: state.siteContentFetchSuccess,
                searchResult: state.searchResult,
                packages: state.packages
            };
        case "RECEIVE_CATEGORY_DATA":
            return {
                ...state,
                categories: action.categories,
                content: state.content,
                siteBanners: state.siteBanners,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                fetchCategorySuccess: state.fetchCategorySuccess,
                isLoading: false,
                fetchsuccess: true,
                requesting: false,
                searchSuccess: false,
                clearSearchSuccess: true,
                searchResult: state.searchResult,
                isLoadingCategory: false,
                siteContentFetchSuccess: state.siteContentFetchSuccess,
                category: state.category,
                packages: state.packages
            };
        case "REQUEST_CATEGORY_PAGE_DATA":
            return {
                ...state,
                category: state.category,
                content: state.content,
                siteBanners: state.siteBanners,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                isLoading: true,
                fetchCategorySuccess: false,
                fetchsuccess: false,
                requesting: false,
                searchSuccess: false,
                clearSearchSuccess: true,
                searchResult: state.searchResult,
                isLoadingCategory: state.isLoadingCategory,
                siteContentFetchSuccess: state.siteContentFetchSuccess,
                categories: state.categories,
                packages: state.packages
            };
        case "RECEIVE_CATEGORY_PAGE_DATA":
            return {
                ...state,
                category: action.category,
                content: state.content,
                siteBanners: state.siteBanners,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                isLoading: false,
                fetchCategorySuccess: true,
                fetchsuccess: true,
                requesting: false,
                searchSuccess: false,
                clearSearchSuccess: true,
                searchResult: state.searchResult,
                isLoadingCategory: state.isLoadingCategory,
                siteContentFetchSuccess: state.siteContentFetchSuccess,
                categories: state.categories,
                packages: state.packages
            };
        case "REQUEST_SITE_CONTENT":
            return {
                ...state,
                category: state.category,
                content: state.content,
                siteBanners: state.siteBanners,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                categories: state.categories,
                fetchCategorySuccess: state.fetchCategorySuccess,
                fetchsuccess: state.fetchsuccess,
                searchResult: state.searchResult,
                requesting: true,
                searchSuccess: false,
                isLoadingCategory: state.isLoadingCategory,
                siteContentFetchSuccess: false,
                clearSearchSuccess: false,
                packages: state.packages
            };
        case "RECEIVE_SITE_CONTENT":
            return {
                ...state,
                categories: state.categories,
                content: action.content,
                siteBanners: action.siteBanners,
                defaultSearchCities: action.defaultSearchCities,
                defaultSearchStates: action.defaultSearchStates,
                defaultSearchCountries: action.defaultSearchCountries,
                fetchCategorySuccess: state.fetchCategorySuccess,
                isLoading: false,
                fetchsuccess: true,
                requesting: false,
                searchSuccess: false,
                clearSearchSuccess: true,
                searchResult: state.searchResult,
                isLoadingCategory: state.isLoadingCategory,
                siteContentFetchSuccess: true,
                category: state.category,
                packages: state.packages
            };
        case CLEAR_SEARCH_DATA:
            return {
                ...state,
                category: state.category,
                categories: state.categories,
                content: state.content,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                siteBanners: state.siteBanners,
                fetchCategorySuccess: state.fetchCategorySuccess,
                fetchsuccess: state.fetchsuccess,
                searchResult: emptySearchResult,
                requesting: false,
                searchSuccess: false,
                isLoadingCategory: state.isLoadingCategory,
                siteContentFetchSuccess: state.siteContentFetchSuccess,
                clearSearchSuccess: true,
                packages: state.packages,
                algoliaResults: []
            };
        case REQUEST_SEARCH_DATA:
            return {
                ...state,
                category: state.category,
                content: state.content,
                siteBanners: state.siteBanners,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                categories: state.categories,
                fetchCategorySuccess: state.fetchCategorySuccess,
                fetchsuccess: state.fetchsuccess,
                searchResult: state.searchResult,
                requesting: true,
                searchSuccess: false,
                isLoadingCategory: state.isLoadingCategory,
                siteContentFetchSuccess: state.siteContentFetchSuccess,
                clearSearchSuccess: false,
                packages: state.packages
            };
        case RECEIVE_SEARCH_DATA:
            return {
                ...state,
                category: state.category,
                categories: state.categories,
                content: state.content,
                defaultSearchCities: state.defaultSearchCities,
                defaultSearchStates: state.defaultSearchStates,
                defaultSearchCountries: state.defaultSearchCountries,
                siteBanners: state.siteBanners,
                fetchCategorySuccess: state.fetchCategorySuccess,
                fetchsuccess: state.fetchsuccess,
                searchResult: action.searchResult,
                requesting: false,
                searchSuccess: true,
                isLoadingCategory: state.isLoadingCategory,
                siteContentFetchSuccess: state.siteContentFetchSuccess,
                clearSearchSuccess: false,
                packages: state.packages
            };
        case REQUEST_PACKAGES_DATA:
            return {
                ...state,
                packagesFetchStatus: false
            };
        case RECEIVE_PACKAGES_DATA:
            return {
                ...state,
                packagesFetchStatus: true,
                packages: action.packages
            };
        case "CHANGE_NEAR_ME_DATA":
            return {
                ...state,
                currentLocation: action.currentLocation
            }
        case "REQUEST_COUNTRYCITY_DATA":
            return {
                ...state,
                countryCities: state.countryCities,
                fetchCountryCities: false
            }
        case "RECIEVE_COUNTRYCITY_DATA":
            return {
                ...state,
                countryCities: action.countryCities,
                fetchCountryCities: true
            }
        case "REQUEST_ALGOLIA_SEARCH_DATA":
            return {
                ...state,
                fetchAlgoliaResults: false,
                algoliaResults: []
            }
        case "RECEIVE_ALGOLIA_SEARCH_DATA":
            return {
                ...state,
                fetchAlgoliaResults: true,
                algoliaResults: action.algoliaResults
            }
        case "FAILURE_ALGOLIA_SEARCH_DATA":
            return {
                ...state,
                fetchAlgoliaResults: true,
                errors: action.error,
                algoliaResults: []
            }

        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
