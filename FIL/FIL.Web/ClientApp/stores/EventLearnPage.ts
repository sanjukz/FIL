import { addTask, fetch } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { EventLearnPageViewModel } from "../models/EventLearnPageViewModel";
import ReviewsRatingDataViewModel from "../models/ReviewsRating/ReviewsRatingViewModel";
import ReviewsRatingResponseViewModel from "../models/ReviewsRating/ReviewsRatingResponseViewModel";
import ReviewsRatingModeratorViewModel from "../models/ReviewsRating/ReviewsRatingModeratorViewModel";
import ReviewsRatingValidationViewModel from "../models/ReviewsRating/ReviewsRatingValidationViewModel";
import userImageMap from "../models/ReviewsRating/UserImageViewModel";
import EventLearnPageDataViewModel from "../models/EventLearnPageDataViewModel";
import CurrencyType from "../models/Comman/CurrencyTypeViewModel";
import { UserOrderRespnseViewModel } from "../models/Account/UserOrderRespnseViewModel";
import { regularViewModel } from "../models/EventLearnPageViewModel";
import { ReviewRatingservice } from "../services/ReviewAndRating";
import { IAppThunkAction } from "./";
import { stat } from "fs";

export const SAVE_USER_REVIEWS_AND_RATING_REQUEST = "USER_REVIEWS_AND_RATING_REQUEST";
export const SAVE_USER_REVIEWS_AND_RATING_SUCCESS = "USER_REVIEWS_AND_RATING_SUCCESS";
export const SAVE_USER_REVIEWS_AND_RATING_FAILURE = "USER_REVIEWS_AND_RATING_FAILURE";

export const IS_USER_PURCHASE_APLACE_REQUEST = "IS_USER_PURCHASE_APLACE_REQUEST";
export const IS_USER_PURCHASE_APLACE_SUCCESS = "IS_USER_PURCHASE_APLACE_SUCCESS";

export const GET_USERORDER_LIST_REQUEST = "USERS_GET_ORDER_LIST_REQUEST";
export const GET_USERORDER_SUCCESS = "USERS_GET_ORDER_LIST_SUCCESS";
export const GET_USERORDER_FAILURE = "USERS_GET_ORDER_LIST_FAILURE";


export interface IEventLearnPageComponentProps {
    eventLearnPage: IEventLearnPageComponentState;
}

export interface IEventLearnPageComponentState {
    isLoading?: boolean;
    errors?: any;
    fetchSuccess: boolean;
    eventDetails: EventLearnPageViewModel;
    userOrders?: UserOrderRespnseViewModel;
    requesting: boolean;
    registered: boolean;
    isPurchased: boolean;
    fetchSuccessEventLeanMore?: boolean;
}

interface IRequestEventLearnPageData {
    type: "REQUEST_EVENT_LEARN_PAGE_DATA";
}

interface IReceiveEventLearnPageData {
    type: "RECEIVE_EVENT_LEARN_PAGE_DATA";
    eventDetails: EventLearnPageViewModel;
}

interface ISaveUserReviewsAndRatingRequestAction {
    type: "USER_REVIEWS_AND_RATING_REQUEST";
}

interface ISaveUserReviewsAndRatingSuccesstAction {
    type: "USER_REVIEWS_AND_RATING_SUCCESS";
}

interface ISaveUserReviewsAndRatingFailureAction {
    type: "USER_REVIEWS_AND_RATING_FAILURE";
}

interface IsUserPurchaseAPlaceRequestAction {
    type: "IS_USER_PURCHASE_APLACE_REQUEST";
}

interface IsUserPurchaseAPlaceSuccesstAction {
    type: "IS_USER_PURCHASE_APLACE_SUCCESS";
    isPurchased: boolean;
}

interface IRequestUserOrderData {
    type: "USERS_GET_ORDER_LIST_REQUEST";
}

interface IReceiveUserOrderData {
    type: "USERS_GET_ORDER_LIST_SUCCESS";
    userOrders: UserOrderRespnseViewModel;
}

type KnownAction = IRequestEventLearnPageData | IReceiveEventLearnPageData | ISaveUserReviewsAndRatingRequestAction |
    ISaveUserReviewsAndRatingSuccesstAction | ISaveUserReviewsAndRatingFailureAction | IsUserPurchaseAPlaceRequestAction |
    IsUserPurchaseAPlaceSuccesstAction | IRequestUserOrderData | IReceiveUserOrderData;

export const actionCreators = {
    requestEventLearnPageData: (eventLearnPageDataViewModel: EventLearnPageDataViewModel): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: "REQUEST_EVENT_LEARN_PAGE_DATA" });
        ReviewRatingservice.getEventLearnPageData(eventLearnPageDataViewModel)
            .then(
                (eventdata: EventLearnPageViewModel) => {
                    dispatch({ type: "RECEIVE_EVENT_LEARN_PAGE_DATA", eventDetails: eventdata });
                });
    },

    userRatingAndReviews: (userRatingReviewModel: ReviewsRatingDataViewModel, callback: (ReviewsRatingResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: SAVE_USER_REVIEWS_AND_RATING_REQUEST });
            ReviewRatingservice.saveReviewAndRatings(userRatingReviewModel)
                .then((response: ReviewsRatingResponseViewModel) => {
                    if (response.success) {
                        dispatch({ type: SAVE_USER_REVIEWS_AND_RATING_SUCCESS });
                    }
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: SAVE_USER_REVIEWS_AND_RATING_FAILURE });
                    });
        },

    activeUserRatingAndReviews: (ratingAltId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/review/activate/${ratingAltId}`)
            .then((response) => response.json() as Promise<ReviewsRatingResponseViewModel>)
            .then((data) => {
                dispatch({ type: SAVE_USER_REVIEWS_AND_RATING_SUCCESS });
            });
        addTask(fetchTask);
        dispatch({ type: SAVE_USER_REVIEWS_AND_RATING_REQUEST });
    },

    getUserOrdertData: (userAltId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/placeReviewRatingValidator/${userAltId}`)
            .then((response) => response.json() as Promise<UserOrderRespnseViewModel>)
            .then((data) => {
                dispatch({ type: "USERS_GET_ORDER_LIST_SUCCESS", userOrders: data, });
            });
        addTask(fetchTask);
        dispatch({ type: "USERS_GET_ORDER_LIST_REQUEST" });
    }

};

const emptyCurrencyType: CurrencyType = {
    id: 0,
    code: "",
    name: "",
    taxes: 0,
    exchangeRate: 0,
}

const unloadedRegularState: regularViewModel = {
    isSameTime: false,
    customTimeModel: [],
    timeModel: [],
    daysOpen: [],
}

const eventLearnPageData: EventLearnPageViewModel = {
    event: undefined,
    eventType: "",
    eventCategory: "",
    eventDetail: undefined,
    venue: undefined,
    city: undefined,
    state: undefined,
    country: undefined,
    user: [],
    eventTicketAttribute: [],
    eventTicketDetail: [],
    ticketCategory: [],
    currencyType: emptyCurrencyType,
    rating: [],
    eventAmenitiesList: [],
    clientPointOfContact: undefined,
    categories: [],
    eventGalleryImage: [],
    eventCategoryName: "",
    eventLearnMoreAttributes: [],
    userImageMap: [],
    seasonTimeModel: [],
    regularTimeModel: unloadedRegularState,
    specialDayModel: [],
    citySightSeeingRouteDetails: [],
    citySightSeeingRoutes: [],
    tiqetsCheckoutDetails: [],
    category: undefined,
    subCategory: undefined,
    eventHostMappings: null,
    onlineStreamStartTime: null,
    onlineEventTimeZone: null
};


const unloadedState: IEventLearnPageComponentState = {
    errors: null,
    fetchSuccess: false,
    eventDetails: eventLearnPageData,
    isPurchased: false,
    isLoading: false,
    requesting: false,
    registered: false,
    fetchSuccessEventLeanMore: false
};

export const reducer: Reducer<IEventLearnPageComponentState> = (state: IEventLearnPageComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case SAVE_USER_REVIEWS_AND_RATING_REQUEST:
            return {
                eventDetails: state.eventDetails,
                userOrders: state.userOrders,
                fetchSuccess: state.fetchSuccess,
                isLoading: true,
                requesting: true,
                registered: false,
                isPurchased: state.isPurchased,
                fetchSuccessEventLeanMore: false
            };
        case SAVE_USER_REVIEWS_AND_RATING_SUCCESS:
            return {
                eventDetails: state.eventDetails,
                userOrders: state.userOrders,
                fetchSuccess: state.fetchSuccess,
                isLoading: true,
                requesting: false,
                registered: true,
                isPurchased: state.isPurchased,
                fetchSuccessEventLeanMore: state.fetchSuccessEventLeanMore
            };
        case SAVE_USER_REVIEWS_AND_RATING_FAILURE:
            return {
                eventDetails: state.eventDetails,
                userOrders: state.userOrders,
                fetchSuccess: state.fetchSuccess,
                isPurchased: false,
                isLoading: true,
                requesting: true,
                registered: false,
                fetchSuccessEventLeanMore: state.fetchSuccessEventLeanMore
            };
        case IS_USER_PURCHASE_APLACE_REQUEST:
            return {
                eventDetails: state.eventDetails,
                userOrders: state.userOrders,
                fetchSuccess: state.fetchSuccess,
                isLoading: true,
                requesting: false,
                registered: true,
                isPurchased: false,
                fetchSuccessEventLeanMore: state.fetchSuccessEventLeanMore
            };
        case IS_USER_PURCHASE_APLACE_SUCCESS:
            return {
                eventDetails: state.eventDetails,
                userOrders: state.userOrders,
                fetchSuccess: state.fetchSuccess,
                isLoading: true,
                requesting: true,
                fetchSuccessEventLeanMore: state.fetchSuccessEventLeanMore,
                registered: false,
                isPurchased: action.isPurchased
            };
        case "REQUEST_EVENT_LEARN_PAGE_DATA":
            return {
                eventDetails: state.eventDetails,
                userOrders: state.userOrders,
                requesting: state.requesting,
                registered: state.registered,
                isPurchased: state.isPurchased,
                fetchSuccess: false,
                fetchSuccessEventLeanMore: false,
                isLoading: true,
            };
        case "RECEIVE_EVENT_LEARN_PAGE_DATA":
            return {
                eventDetails: action.eventDetails,
                userOrders: state.userOrders,
                requesting: state.requesting,
                isPurchased: state.isPurchased,
                registered: state.registered,
                fetchSuccess: true,
                fetchSuccessEventLeanMore: true,
                isLoading: false,
            };
        case "USERS_GET_ORDER_LIST_REQUEST":
            return {
                userOrders: state.userOrders,
                eventDetails: state.eventDetails,
                requesting: state.requesting,
                isPurchased: state.isPurchased,
                registered: state.registered,
                fetchSuccess: true,
                fetchSuccessEventLeanMore: state.fetchSuccessEventLeanMore,
                isLoading: false,
            };
        case "USERS_GET_ORDER_LIST_SUCCESS":
            return {
                userOrders: action.userOrders,
                eventDetails: state.eventDetails,
                requesting: state.requesting,
                isPurchased: state.isPurchased,
                registered: state.registered,
                fetchSuccessEventLeanMore: state.fetchSuccessEventLeanMore,
                fetchSuccess: true,
                isLoading: false,
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
