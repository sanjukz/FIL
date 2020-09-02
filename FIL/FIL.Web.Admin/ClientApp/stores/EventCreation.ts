import { fetch, addTask } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { eventCreationService } from "../services/EventCreation";
import { AmenityViewModel } from "../models/EventCreation/AmenityViewModel";
import { EventCreationViewModel } from "../models/EventCreation/EventCreationViewModel";
import { EventCreationResponseViewModel } from "../models/EventCreation/EventCreationResponseViewModel";
import { AlertDataViewModel } from "../models/Alert/AlertDataViewModel";
import GetEventsResponseViewModel from "../models/EventCreation/GetEventsResponseViewModel";
import GeteventViewModel from "../models/EventCreation/GeteventViewModel";
import { Host } from "../models/CreateEvent/Host";
import { IPDetailViewModel } from "../models/CustomerIpDetails/IPDetailViewModel";
import TagResponseViewModel from "../models/Tags/TagResponseViewModel";
import { IAppThunkAction } from "./";

export const GET_EVENT_CATEGORIES_REQUEST = "EVENT_CATEGORIES_LIST_REQUEST";
export const GET_EVENT_CATEGORIES_SUCCESS = "EVENT_CATEGORIES_LIST_SUCCESS";
export const GET_EVENT_CATEGORIES_FAILURE = "EVENT_CATEGORIES_LIST_FAILURE";

export const GET_TAG_REQUEST = "TAG_REQUEST";
export const GET_TAG_SUCCESS = "TAG_SUCCESS";
export const GET_TAG_FAILURE = "TAG_FAILURE";

export const GET_EVENT_TICKETTYPE_REQUEST = "EVENT_TICKETTYPE_REQUEST";
export const GET_EVENT_TICKETTYPE_SUCCESS = "EVENT_TICKETTYPE_SUCCESS";
export const GET_EVENT_TICKETTYPE_FAILURE = "EVENT_TICKETTYPE_FAILURE";

export const SAVE_EVENT_REQUEST = "saveEventRequestAction";
export const SAVE_EVENT_SUCCESS = "saveEventSuccessAction";
export const SAVE_EVENT_FAILURE = "saveEventFailure";


export const SAVE_AMENITY_REQUEST = "saveAmenityRequestAction";
export const SAVE_AMENITY_SUCCESS = "saveAmenitySuccessAction";
export const SAVE_AMENITY_FAILURE = "saveAmenityFailure";

export const GET_IP_REQUEST = "getIPRequestAction";
export const GET_IP_SUCCESS = "getIPSuccessAction";
export const GET_IP_FAILURE = "getIPFailure";

export interface IEventprops {
  eventCreation: IEventCategoriesComponentState;
}
export interface ICategory {
  categoryId: number;
  displayName: string;
  isHomePage: boolean;
  order: number;
  slug: string;
  value: number;
}

export interface ISavedData {
  address1: string;
  address2: string;
  altId: string;
  amenityId: string;
  archdetail: string;
  categoryid: string;
  city: string;
  country: string;
  description: string;
  highlights: string;
  history: string;
  id: number;
  immersiveExperience: string;
  inventoryPageBannerImage: string;
  TilesSliderImages: string;
  DescpagebannerImage: string;
  PlacemapImages: string;
  TimelineImages: string;
  ImmersiveexpImages: string;
  ArchdetailImages: string;
  location: string;
  metadescription: string;
  metatags: string;
  metatitle: string;
  placename: string;
  state: string;
  subcategoryid: string;
  title: string;
  zip: string;
  lat: string;
  long: string;
  hourTimeDuration: string;
  minuteTimeDuration: string;
  eventHostMappings: Host[]
}

export interface ISavedAmenityData {
  description: string
}

export interface ICurrency {
  currencyId: number;
  code: string;
  countryId: number;
  exchangeRate: number;
  taxes: string;
  isEnabled: boolean;
}
export interface IEventCategories {
  categories: ICategory[];
}

export interface ISavedEventData {
  savedData: ISavedData[];
}
export interface ISavedAmenityData {
  savedAmenityData: ISavedAmenityData[];
}

export interface IEventCurrencies {
  currencies: ICurrency[];
}

export interface IEventCategoriesComponentState {
  fetchEventCategoriesSuccess?: boolean;
  fetchAminitySuccess?: boolean;
  eventCategoriesList?: IEventCategories;
  tags?: TagResponseViewModel;
  amenityList?: AmenityViewModel;
  eventCategories?: any;
  eventType?: any;
  errors?: any;
  success?: boolean;
  altId?: string;
  alertMessage?: AlertDataViewModel;
  errorMessage?: string;
  EventSaveSuccessful?: boolean;
  EventSaveFailure?: boolean;
  error?: boolean;
  updateSuccess?: boolean;
  eventData?: GetEventsResponseViewModel;
  eventDataFetchSuccess?: boolean;
  savedData?: ISavedData;
  isSaveEventRequest: boolean;
  isShowSuccessAlert: boolean;
  currentIP: IPDetailViewModel;
  isAlreadyExists?: boolean;
}

export interface IAmenities {
  amenities: string[];
}

const emptyCategories: IEventCategories = {
  categories: [],
};

const emptyIPDetailViewModel: IPDetailViewModel = {
  latitude: 0,
  longitude: 0
};


const initialAlert: AlertDataViewModel = {
  success: false,
  subject: "",
  body: "",
};

const events: GetEventsResponseViewModel = {
  event: [],
  id: null,
  name: ""
};

const emptyTags: TagResponseViewModel = {
  tags: []
};
const DefaultEventCategoriesCategories: IEventCategoriesComponentState = {
  eventCategoriesList: emptyCategories,
  fetchAminitySuccess: false,
  fetchEventCategoriesSuccess: false,
  success: false,
  alertMessage: initialAlert,
  errorMessage: "",
  error: false,
  EventSaveSuccessful: false,
  EventSaveFailure: false,
  updateSuccess: false,
  eventData: events,
  eventDataFetchSuccess: false,
  isSaveEventRequest: false,
  isShowSuccessAlert: false,
  isAlreadyExists: false,
  currentIP: emptyIPDetailViewModel,
  tags: emptyTags
};

// interface IRequestGetEventCategoriesListAction {
//     type: "EVENT_CATEGORIES_LIST_REQUEST";
// }

// interface IReceiveGetEventCategoriesListAction {
//     type: "EVENT_CATEGORIES_LIST_SUCCESS";
//     eventCategoriesList: EventCategoriesViewModel;
// }

// interface IGetEventCategoriesListFailureAction {
//     type: "EVENT_CATEGORIES_LIST_FAILURE";
//     errors: any;
// }

interface IRequestGetAminityListAction {
  type: "AMENITY_LIST_REQUEST";
}

interface IReceiveGetAminityListAction {
  type: "AMENITY_LIST_SUCCESS";
  amenityList: AmenityViewModel;
}

interface IGetAminityListFailureAction {
  type: "AMENITY_LIST_FAILURE";
  errors: any;
}

interface ISaveEventRequestAction {
  type: "saveEventRequestAction";
}

interface ISaveEventSuccesstAction {
  type: "saveEventSuccessAction";
  success: boolean;
  altId: string;
  alertMessage: AlertDataViewModel;
  isAlreadyExists: boolean
}

interface ISaveEventFailureAction {
  type: "saveEventFailure";
  alertMessage: AlertDataViewModel;
  error: any;
}

interface IRequestEventsData {
  type: "USERS_EventData_REQUEST";
}

interface IReceiveEventsData {
  type: "USERS_EventData_SUCCESS";
  event: GetEventsResponseViewModel;
}

interface IRequestEventCategoryAction {
  type: "REQUEST_EVENT_CATEGORIES";
}

interface IReceiveEventCategoryAction {
  type: "RECEIVE_EVENT_CATEGORIES";
  eventCategoriesList: IEventCategories;
}

interface IRequestTagAction {
  type: "TAG_REQUEST";
}

interface IReceiveTagAction {
  type: "TAG_SUCCESS";
  tags: TagResponseViewModel;
}

interface IRequestSavedEventData {
  type: "REQUEST_EVENT_SAVED_DATA";
}

interface IReceiveSavedEventData {
  type: "RECEIVE_EVENT_SAVED_DATA";
  savedEventData: ISavedData;
}

interface IRequestSavedAmenityData {
  type: "saveAmenityRequestAction";
}

interface ISavedAmenityDataSuccess {
  type: "saveAmenitySuccessAction";
  success: boolean;
}

interface IRequestGetIPDetail {
  type: "getIPRequestAction";
}

interface IGetIPDetailSuccess {
  type: "getIPSuccessAction";
  currentIP: IPDetailViewModel
}


type KnownAction = IRequestEventCategoryAction | IReceiveEventCategoryAction | ISaveEventRequestAction
  | ISaveEventSuccesstAction | ISaveEventFailureAction | IRequestEventsData | IReceiveEventsData
  | IRequestGetAminityListAction | IReceiveGetAminityListAction | IRequestSavedEventData | IReceiveSavedEventData
  | IRequestSavedAmenityData | ISavedAmenityDataSuccess | IRequestGetIPDetail | IGetIPDetailSuccess | IRequestTagAction | IReceiveTagAction;

export const actionCreators = {

  requestEventCategories: (callback?: (IEventCategories) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/eventcategory/all`)
      .then((response) => response.json() as Promise<IEventCategories>)
      .then((data) => {
        dispatch({ type: "RECEIVE_EVENT_CATEGORIES", eventCategoriesList: data });
        callback(data);
      });
    addTask(fetchTask);
    dispatch({ type: "REQUEST_EVENT_CATEGORIES" });
  },

  requestTags: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/tags/all`)
      .then((response) => response.json() as Promise<TagResponseViewModel>)
      .then((data) => {
        dispatch({ type: "TAG_SUCCESS", tags: data, });
      });

    addTask(fetchTask);
    dispatch({ type: "TAG_REQUEST" });
  },

  requestAmenities: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/amenities/all`)
      .then((response) => response.json() as Promise<IAmenities>)
      .then((data) => {

        dispatch({ type: "AMENITY_LIST_SUCCESS", amenityList: data });
      });
    addTask(fetchTask);
    dispatch({ type: "AMENITY_LIST_REQUEST" });
  },

  requestCustomerIp: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/customeripdetail`)
      .then((response) => response.json() as Promise<IPDetailViewModel>)
      .then((data) => {

        dispatch({ type: "getIPSuccessAction", currentIP: data });
      });
    addTask(fetchTask);
    dispatch({ type: "getIPRequestAction" });
  },

  saveEvent: (eventData: EventCreationViewModel, callback: (EventCreationResponseViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: SAVE_EVENT_REQUEST });
      eventCreationService.saveEventData(eventData)
        .then((response: EventCreationResponseViewModel) => {
          if (response.success) {

            var alertModel: AlertDataViewModel = {
              success: true,
              subject: "Event saved successfully",
              body: "Event saved successfully",
            };
            localStorage.setItem("venueAltId", response.venueAltId);
            localStorage.setItem("placeAltId", response.altId);
            localStorage.setItem("placeId", response.eventId.toString());
          }
          dispatch({ type: SAVE_EVENT_SUCCESS, success: response.success, altId: response.altId, alertMessage: alertModel, isAlreadyExists: response.isAlreadyExists });
          callback(response);
        },
          (error) => {
            var alertModel: AlertDataViewModel = {
              success: false,
              subject: "Event Save failed",
              body: error,
            };

            dispatch({ type: SAVE_EVENT_FAILURE, alertMessage: alertModel, error: error });
          });
    },

  saveAmenities: (amenity: string, callback: (SaveAmenityDataViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: SAVE_AMENITY_REQUEST });
      eventCreationService.saveAmenityData(amenity)
        .then((response: EventCreationResponseViewModel) => {
          if (response.success) {


          }
          dispatch({ type: SAVE_AMENITY_SUCCESS, success: response.success, });
          callback(response);
        },
          (error) => {
            var alertModel: AlertDataViewModel = {
              success: false,
              subject: "Event Save failed",
              body: error,
            };

            dispatch({ type: SAVE_EVENT_FAILURE, alertMessage: alertModel, error: error });
          });
    },

  geEventsData: (eventId: GeteventViewModel, callback: (GetEventsResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
    dispatch({ type: "USERS_EventData_REQUEST" });

    eventCreationService.getEventsData(eventId)
      .then(
        (eventData: GetEventsResponseViewModel) => {

          dispatch({ type: "USERS_EventData_SUCCESS", event: eventData });
        },
      );
  },

  getEventSavedData: (eventId: string, callback: (ISavedData) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {

    dispatch({ type: "REQUEST_EVENT_SAVED_DATA" });
    eventCreationService.getEventSavedData(eventId)
      .then(
        (eventData: any) => {
          dispatch({ type: "RECEIVE_EVENT_SAVED_DATA", savedEventData: eventData });
          callback(eventData)
        },
      );
  },
}

export const reducer: Reducer<IEventCategoriesComponentState> = (state: IEventCategoriesComponentState, action: KnownAction) => {
  switch (action.type) {
    case SAVE_EVENT_REQUEST:
      return {
        errors: {},
        success: false,
        altId: undefined,
        alertMessage: initialAlert,
        errorMessage: "",
        EventSaveSuccessful: false,
        EventSaveFailure: false,
        subEventFetchSuccess: false,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        fetchAminitySuccess: state.fetchAminitySuccess,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        isSaveEventRequest: true,
        isShowSuccessAlert: false,
        isAlreadyExists: false,
        currentIP: state.currentIP,
        tags: state.tags,
        savedData: state.savedData,
      };
    case SAVE_EVENT_SUCCESS:
      return {
        errors: {},
        errorMessage: "",
        EventSaveSuccessful: true,
        EventSaveFailure: false,
        subEventFetchSuccess: false,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        fetchAminitySuccess: state.fetchAminitySuccess,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        isSaveEventRequest: false,
        isShowSuccessAlert: true,
        isAlreadyExists: action.isAlreadyExists,
        currentIP: state.currentIP,
        tags: state.tags,
        savedData: state.savedData,
      };
    case SAVE_AMENITY_REQUEST:
      return {
        errors: {},
        success: false,
        altId: undefined,
        EventSaveSuccessful: false,
        EventSaveFailure: true,
        subEventFetchSuccess: false,
        isAlreadyExists: state.isAlreadyExists,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        tags: state.tags,
        savedData: state.savedData,
      };
    case SAVE_EVENT_REQUEST:
      return {
        errors: {},
        success: false,
        altId: undefined,
        alertMessage: initialAlert,
        errorMessage: "",
        EventSaveSuccessful: false,
        EventSaveFailure: false,
        subEventFetchSuccess: false,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        fetchAminitySuccess: state.fetchAminitySuccess,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };
    case SAVE_EVENT_FAILURE:
      return {
        errors: {},
        success: false,
        altId: undefined,
        EventSaveSuccessful: false,
        EventSaveFailure: true,
        subEventFetchSuccess: false,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };

    case "RECEIVE_EVENT_CATEGORIES":
      return {
        eventCategoriesList: action.eventCategoriesList,
        fetchEventCategoriesSuccess: true,
        errors: {},
        alertMessage: initialAlert,
        errorMessage: "",
        subEventFetchSuccess: false,
        amenityList: state.amenityList,
        fetchAminitySuccess: false,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };
    case "REQUEST_EVENT_CATEGORIES":
      return {
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: false,
        errors: {},
        alertMessage: initialAlert,
        errorMessage: "",
        subEventFetchSuccess: false,
        amenityList: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };
    case "TAG_REQUEST":
      return {
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        errors: {},
        alertMessage: initialAlert,
        errorMessage: "",
        subEventFetchSuccess: false,
        amenityList: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: state.isShowSuccessAlert,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: emptyTags,
        savedData: state.savedData,
      };
    case "TAG_SUCCESS":
      return {
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        errors: {},
        alertMessage: initialAlert,
        errorMessage: "",
        subEventFetchSuccess: false,
        amenityList: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: state.isShowSuccessAlert,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: action.tags,
        savedData: state.savedData,
      };
    case "USERS_EventData_REQUEST":
      return {
        eventData: state.eventData,
        eventDataFetchSuccess: true,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      }
    case "USERS_EventData_SUCCESS":
      return {
        eventData: action.event,
        eventDataFetchSuccess: true,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        eventTypeLamenityListist: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };
    case "AMENITY_LIST_REQUEST":
      return {
        eventData: state.eventData,
        eventDataFetchSuccess: true,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };
    case "AMENITY_LIST_SUCCESS":
      return {
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: action.amenityList,
        fetchAminitySuccess: true,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };

    case "getIPRequestAction":
      return {
        eventData: state.eventData,
        eventDataFetchSuccess: true,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        currentIP: emptyIPDetailViewModel,
        isShowSuccessAlert: false,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };
    case "getIPSuccessAction":
      return {
        eventData: state.eventData,
        eventDataFetchSuccess: state.eventDataFetchSuccess,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        currentIP: action.currentIP,
        fetchAminitySuccess: true,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags,
        savedData: state.savedData,
      };

    case "RECEIVE_EVENT_SAVED_DATA":
      return {

        savedData: action.savedEventData,
        eventDataFetchSuccess: true,
        eventCategoriesList: state.eventCategoriesList,
        fetchEventCategoriesSuccess: state.fetchEventCategoriesSuccess,
        amenityList: state.amenityList,
        fetchAminitySuccess: state.fetchAminitySuccess,
        isSaveEventRequest: state.isSaveEventRequest,
        isShowSuccessAlert: false,
        currentIP: state.currentIP,
        isAlreadyExists: state.isAlreadyExists,
        tags: state.tags
      };
  }
  return state || DefaultEventCategoriesCategories;
}
