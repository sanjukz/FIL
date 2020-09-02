import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";

export interface IEventComponentState {
    isLoading: boolean;
    events: IVenues;
    categorylist: any;
    eventcategories: IEventCategories,
    searchstring: string;
    CategoriesId: number[];
    SubCategories: IEventCategories;
}

export interface IEvent {
    id: number;
    altId: string;
    eventCategoryId: any;
    eventTypeId: any;
    name: string;
    description: string;
    isEnabled: boolean;
    isFeel: boolean;
    eventSourceId: any;
    isPublishedOnSite: boolean;
    publishedDateTime: Date;
    imagePath: string;
}

export interface ICategory {
    categoryId: number;
    displayName: string;
    isHomePage: boolean;
    order: number;
    slug: string;
    value: number;
}
export interface IEventCategories {
    categories: ICategory[];
}

interface IRequestEventCategoryAction {
    type: "REQUEST_EVENT_CATEGORIES";
}

interface IReceiveEventCategoryAction {
    type: "RECEIVE_EVENT_CATEGORIES";
    eventcategories: IEventCategories;
}



export interface IVenues {
    venues: IEvent[];
}

interface IRequestEventDataAction {
    type: "REQUEST_EVENT_DATA";

}

interface IReceiveEventDataAction {
    type: "RECEIVE_EVENT_DATA";
    events: IVenues;
}

interface ISetSearchDataAction {
    type: "SET_EVENT_SEARCH_DATA";
    category: number[];
    subcategory: IEventCategories;
    selectedcategory: any;
    searchstring: string;
}

type KnownAction = IRequestEventDataAction | IReceiveEventDataAction | IRequestEventCategoryAction | IReceiveEventCategoryAction | ISetSearchDataAction;

export const actionCreators = {
    RequestEventCategories: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/event/categories`)
            .then((response) => response.json() as Promise<IEventCategories>)
            .then((data) => {
                dispatch({ type: "RECEIVE_EVENT_CATEGORIES", eventcategories: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_CATEGORIES" });
    },
    requestEventdata: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/event`)
            .then((response) => response.json() as Promise<IVenues>)
            .then((data) => {

                dispatch({ type: "RECEIVE_EVENT_DATA", events: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_DATA" });
    },
    requestSearchEventdata: (searchstring: string, cate: number[], SubCat: IEventCategories, selected: any): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        dispatch({ type: "SET_EVENT_SEARCH_DATA", category: cate, subcategory: SubCat, selectedcategory: selected, searchstring: searchstring });
        const fetchTask = fetch(`api/event`)
            .then((response) => response.json() as Promise<IVenues>)
            .then((data) => {

                dispatch({ type: "RECEIVE_EVENT_DATA", events: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_EVENT_DATA" });
    },
    searchEventdata: (searchstring: string, cate: number[], SubCat: IEventCategories, selected: any): IAppThunkAction<KnownAction> => (dispatch, getState) => {

        dispatch({ type: "SET_EVENT_SEARCH_DATA", category: cate, subcategory: SubCat, selectedcategory: selected, searchstring: searchstring });

        if (!searchstring) searchstring = "";

        if (cate.length > 0 || SubCat.categories.length > 0) {
            var searchdata = "";
            let category = cate.toString();
            let subcategory = SubCat.toString();
            if (searchstring.length > 2) {
                searchdata = searchdata + searchstring;
            }
            searchdata = searchdata + "_";
            if (category !== "") {
                searchdata = searchdata + cate;
            }
            searchdata = searchdata + ",";
            let subcatids = [];
            for (let i = 0; i < SubCat.categories.length; i++) {
                subcatids.push(SubCat.categories[i].value);
            }
            if (subcategory !== "") {
                searchdata = searchdata + subcatids.toString();
            }


            const fetchTask = fetch(`api/eventsearch/${searchdata}`)
                .then((response) => response.json() as Promise<IVenues>)
                .then((data) => {
                    dispatch({ type: "RECEIVE_EVENT_DATA", events: data });
                });

            addTask(fetchTask);
            dispatch({ type: "REQUEST_EVENT_DATA" });
        }
        else if (cate.length == 0 && SubCat.categories.length == 0 && searchstring != "") {
            if (searchstring.length > 2) {
                searchdata = searchstring + "_";
                const fetchTask = fetch(`api/eventsearch/${searchdata}`)
                    .then((response) => response.json() as Promise<IVenues>)
                    .then((data) => {
                        dispatch({ type: "RECEIVE_EVENT_DATA", events: data });
                    });

                addTask(fetchTask);
                dispatch({ type: "REQUEST_EVENT_DATA" });
            }

        }
    }

};

const emptyVenue: IVenues = {
    venues: []
};
const empty: IEventCategories =
{
    categories: []
}
const EmptyData = [];

const unloadedState: IEventComponentState = {
    events: emptyVenue, isLoading: false, categorylist: [], eventcategories: empty, searchstring: null, CategoriesId: EmptyData, SubCategories: empty
}

export const reducer: Reducer<IEventComponentState> = (state: IEventComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_EVENT_DATA":
            return {
                events: state.events,
                isLoading: true,
                categorylist: state.categorylist,
                eventcategories: state.eventcategories,
                searchstring: state.searchstring,
                CategoriesId: state.CategoriesId,
                SubCategories: state.SubCategories
            };
        case "RECEIVE_EVENT_DATA":
            return {
                events: action.events,
                isLoading: false,
                categorylist: state.categorylist,
                eventcategories: state.eventcategories,
                searchstring: state.searchstring,
                CategoriesId: state.CategoriesId,
                SubCategories: state.SubCategories
            };
        case "RECEIVE_EVENT_CATEGORIES":
            return {
                events: state.events,
                isLoading: false,
                categorylist: state.categorylist,
                eventcategories: action.eventcategories,
                searchstring: state.searchstring,
                CategoriesId: state.CategoriesId,
                SubCategories: state.SubCategories
            };
        case "REQUEST_EVENT_CATEGORIES":
            return {
                events: state.events,
                isLoading: false,
                categorylist: state.categorylist,
                eventcategories: state.eventcategories,
                searchstring: state.searchstring,
                CategoriesId: state.CategoriesId,
                SubCategories: state.SubCategories
            };
        case "SET_EVENT_SEARCH_DATA":
            return {
                events: state.events,
                isLoading: false,
                categorylist: action.selectedcategory,
                eventcategories: state.eventcategories,
                searchstring: action.searchstring,
                CategoriesId: action.category,
                SubCategories: action.subcategory
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
