import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { TicketCategoryResponseViewModel, regularViewModel } from "../models/TicketCategoryResponseViewModel";
import SubEventTicketCategoryResponseViewModel from "../models/SubEventTicketCategoryResponseViewModel";
import CountryDataViewModel from "../models/Country/CountryDataViewModel";
import { CategoryViewModel } from "../models/CategoryViewModel";
import TiqetsTimeSlotResponseModel, { TimeSlotResponseModel } from "../models/Tiqets/TiqetsTimeSlotResponseModel";
import HohoTimeSlotResponseModel from "../models/CitySightSeeing/TimeSlotResponseModel";
export const GET_COUNTRY_LIST_REQUEST = "TICKET_CATEGORY_GET_COUNTRY_LIST_REQUEST";
export const GET_COUNTRY_LIST_SUCCESS = "TICKET_CATEGORY_GET_COUNTRY_LIST_SUCCESS";
export const GET_COUNTRY_LIST_FAILURE = "TICKET_CATEGORY_GET_COUNTRY_LIST_FAILURE";

export interface ITicketCategoryPageProps {
    ticketCategory: ITicketCategoryPageState;
}

export interface ITicketCategoryPageState {
    altId?: string;
    isLoading?: boolean;
    errors?: any;
    fetchEventSuccess: boolean;
    ticketCategories?: TicketCategoryResponseViewModel;
    fetchCountriesSuccess?: boolean;
    countryList?: CountryDataViewModel;
    fetchTiqetsTimeSlots?: boolean;
    tiqetsTimeSlots?: TiqetsTimeSlotResponseModel;
    fetchHohoTimeSlots?: boolean;
    hohoTimeSlots?: HohoTimeSlotResponseModel;
}

const emptyCountries: CountryDataViewModel = {
    countries: [],
};

interface IRequestTicketCategoryData {
    type: "REQUEST_TICKET_CATEGORY_DATA";
}

interface IReceiveTicketCategoryData {
    type: "RECEIVE_TICKET_CATEGORY_DATA";
    ticketCategories: TicketCategoryResponseViewModel;
}

interface IRequestGetCountryListAction {
    type: "TICKET_CATEGORY_GET_COUNTRY_LIST_REQUEST";
}

interface IReceiveGetCountryListAction {
    type: "TICKET_CATEGORY_GET_COUNTRY_LIST_SUCCESS";
    countryList: CountryDataViewModel;
}

interface IGetCountryListFailureAction {
    type: "TICKET_CATEGORY_GET_COUNTRY_LIST_FAILURE";
    errors: any;
}

interface IRequestTiqetsTimeSlots {
    type: "REQUEST_TIQETS_TIMESLOTS";
}

interface IReceiveTiqetsTimeSlots {
    type: "RECEIVE_TIQETS_TIMESLOTS";
    tiqetsTimeSlots: TiqetsTimeSlotResponseModel;
}
interface IRequestHohoTimeSlots {
    type: "REQUEST_HOHO_TIMESLOTS";
}

interface IReceiveHohoTimeSlots {
    type: "RECEIVE_HOHO_TIMESLOTS";
    hohoTimeSlots: HohoTimeSlotResponseModel;
}
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
    subCategories: [],
};

type KnownAction = IRequestTicketCategoryData | IReceiveTicketCategoryData |
    IRequestGetCountryListAction | IReceiveGetCountryListAction | IGetCountryListFailureAction |
    IReceiveTiqetsTimeSlots | IRequestTiqetsTimeSlots | IReceiveHohoTimeSlots | IRequestHohoTimeSlots;

export const actionCreators = {
    getTicketCategory: (eventAltId: string, callback: (TicketCategoryResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/ticketcategories/${eventAltId}`)
            .then((response) => response.json() as Promise<TicketCategoryResponseViewModel>)
            .then((data) => {
                if (data.event) {
                    callback(data);
                }
                dispatch({ type: "RECEIVE_TICKET_CATEGORY_DATA", ticketCategories: data });
            })
            .catch(error => { throw error });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_TICKET_CATEGORY_DATA" });
    },
    requestCountryData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/country/all`)
            .then((response) => response.json() as Promise<CountryDataViewModel>)
            .then((data) => {
                dispatch({ type: "TICKET_CATEGORY_GET_COUNTRY_LIST_SUCCESS", countryList: data, });
            },
                (error) => {
                    dispatch({ type: "TICKET_CATEGORY_GET_COUNTRY_LIST_FAILURE", errors: error });
                },
            );
        addTask(fetchTask);
        dispatch({ type: "TICKET_CATEGORY_GET_COUNTRY_LIST_REQUEST" });
    },
    getTiqetsTimeSlots: (productId: string, day: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/get-timeslots/${productId}/${day}`)
            .then((response) => response.json() as Promise<TiqetsTimeSlotResponseModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_TIQETS_TIMESLOTS", tiqetsTimeSlots: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_TIQETS_TIMESLOTS" });

    },
    getHoHoTimeSlots: (ticketId: string, day: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/availability/${day}/${ticketId}`)
            .then((response) => response.json() as Promise<HohoTimeSlotResponseModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_HOHO_TIMESLOTS", hohoTimeSlots: data });
            });
        addTask(fetchTask);
        dispatch({ type: "REQUEST_HOHO_TIMESLOTS" });

    },
};

const emptyRegularView: regularViewModel = {
    isSameTime: false,
    customTimeModel: [],
    timeModel: [],
    daysOpen: [],
}
const emptytiqetsTimeSlots: TiqetsTimeSlotResponseModel = {
    timeSlots: []
}

const eventTicketCategoryData: TicketCategoryResponseViewModel = {
    event: undefined,
    eventDetail: undefined,
    city: undefined,
    eventTicketAttribute: undefined,
    venue: undefined,
    currencyType: undefined,
    eventTicketDetail: undefined,
    ticketCategory: undefined,
    eventCategory: "",
    eventCategoryName: "",
    customerDocumentTypes: [],
    PlaceCustomerDocumentTypeMappings: [],
    PlaceHolidayDates: [],
    PlaceWeekOffs: [],
    ticketCategorySubTypes: [],
    eventTicketDetailTicketCategoryTypeMappings: [],
    ticketCategoryTypes: [],
    regularTimeModel: emptyRegularView,
    seasonTimeModel: [],
    specialDayModel: [],
    eventDeliveryTypeDetails: [],
    deliveryOptions: [],
    eventVenueMappings: undefined,
    eventVenueMappingTimes: [],
    tiqetsCheckoutDetails: [],
    validWithVariantModel: [],
    citySightSeeingTicketDetail: undefined,
    category: undefined,
    subCategory: undefined,
    eventRecurranceScheduleModels: [],
    eventHostMapping: [],
    EventAttributes: [],
    formattedDateString: null
};

const eventSubEventTicketCategoryData: SubEventTicketCategoryResponseViewModel = {
    eventDetail: undefined,
    city: undefined,
    eventTicketAttribute: undefined,
    venue: undefined,
    currencyType: undefined,
    eventTicketDetail: undefined,
    ticketCategory: undefined,
};

const unloadedState: ITicketCategoryPageState = {
    ticketCategories: eventTicketCategoryData,
    isLoading: false,
    errors: null,
    fetchEventSuccess: false,
    fetchCountriesSuccess: false,
    countryList: emptyCountries,
    fetchTiqetsTimeSlots: false,
    tiqetsTimeSlots: emptytiqetsTimeSlots
};

export const reducer: Reducer<ITicketCategoryPageState> = (state: ITicketCategoryPageState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_TICKET_CATEGORY_DATA":
            return {
                ...state,
                fetchEventSuccess: false,
            };
        case "RECEIVE_TICKET_CATEGORY_DATA":
            return {
                ...state,
                ticketCategories: action.ticketCategories,
                altId: (action.ticketCategories.eventDetail.length > 0 ? action.ticketCategories.eventDetail[0].AltId : ""),
                fetchEventSuccess: true,
            };
        case "TICKET_CATEGORY_GET_COUNTRY_LIST_REQUEST":
            return {
                ...state,
                fetchCountriesSuccess: false,
            };
        case "TICKET_CATEGORY_GET_COUNTRY_LIST_SUCCESS":
            return {
                ...state,
                countryList: action.countryList,
                fetchCountriesSuccess: true,
            };
        case "TICKET_CATEGORY_GET_COUNTRY_LIST_FAILURE":
            return {
                ...state,
                fetchCountriesSuccess: false,
            };
        case "RECEIVE_TIQETS_TIMESLOTS":
            return {
                ...state,
                fetchTiqetsTimeSlots: true,
                tiqetsTimeSlots: action.tiqetsTimeSlots,
                isLoading: false
            };
        case "REQUEST_TIQETS_TIMESLOTS":
            return {
                ...state,
                fetchTiqetsTimeSlots: false,
                tiqetsTimeSlots: emptytiqetsTimeSlots,
                isLoading: true
            };
        case "RECEIVE_HOHO_TIMESLOTS":
            return {
                ...state,
                fetchHohoTimeSlots: true,
                hohoTimeSlots: action.hohoTimeSlots,
                isLoading: false
            };
        case "REQUEST_HOHO_TIMESLOTS":
            return {
                ...state,
                fetchHohoTimeSlots: false,
                hohoTimeSlots: state.hohoTimeSlots,
                isLoading: true
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
