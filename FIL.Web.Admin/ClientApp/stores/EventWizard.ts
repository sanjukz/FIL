import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";

export const SAVE_REQUEST = "SAVE_REQUEST";
export const SAVE_SUCCESS = "SAVE_SUCCESS";
export const SAVE_FAILURE = "SAVE_FAILURE";

export interface IEventWizardState {
    progress?: number;
    isLoading?: boolean;
    val?: any;
    venues: IVenues;
    errors?: any;
}

export interface ISaveEventState {
    progress?: number;
    requesting?: boolean;
    registered?: boolean;
    isLoading?: boolean;
    errors?: any;
}

export interface IEventWizardComponentState {
    eventWizardData: IEventWizardData;
}

export interface IEventForm {
    eventCategoryId: number;
    eventTypeId: number;
    name: string;
    description: string;
    clientPointOfContactId: number;
    fbEventId: number;
    metaDetails: string;
    termsAndConditions: string;
    isPublishedOnSite: boolean;
}

export interface IEventDetail {
    venueId: number;
    startDateTime: Date;
    endDateTime: Date;
    groupId: number;
}

export interface IEventTicketAttribute {
    salesStartDateTime: Date;
    salesEndDatetime: Date;
    ticketTypeId: number;
    channelId: number;
    currencyId: number;
    sharedInventoryGroupId: number;
    availableTicketForSale: number;
    remainingTicketForSale: number;
    ticketCategoryDescription: string;
    viewFromStand: string;
    isSeatSelection: boolean;
    price: number;
    isInternationalCardAllowed: boolean;
    isEMIApplicable: boolean;
}

export interface IEventAttribute {
    matchNo: number;
    matchDay: number;
    gateOpenTime: string;
    timeZone: string;
    timeZoneAbbreviation: string;
    ticketHtml: string;
}

export interface IVenueForm {
    name: string;
    addressLineOne: string;
    addressLineTwo: string;
    cityId: number;
    latitude: string;
    longitude: string;
    prefix: string;
}

export interface IEventWizardData {
    eventForm: IEventForm;
    eventDetail: IEventDetail;
    eventTicketAttribute: IEventTicketAttribute;
    eventAttribute: IEventAttribute;
    venueForm: IVenueForm;
}

export interface IVenue {
    name: string;
    addressLineOne: string;
    addressLineTwo: string;
    cityId: number;
    latitude: string;
    longitude: string;
    prefix: string;
}

export interface IVenues {
    venues: IVenue[];
}

interface IEventWizardPostRequestAction {
    type: "SAVE_REQUEST";
}

interface IEventWizardPostSuccesstAction {
    type: "SAVE_SUCCESS";
}

interface IEventWizardPostFailureAction {
    type: "SAVE_FAILURE";
}

interface IRequestVenueDataAction {
    type: "REQUEST_VENUE_DATA";
}

interface IReceiveVenueDataAction {
    type: "RECEIVE_VENUE_DATA";
    venues: IVenues;
}

type KnownAction = IRequestVenueDataAction | IReceiveVenueDataAction

export const actionCreators = {
    requestVenues: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/venue/all`)
            .then((response) => response.json() as Promise<IVenues>)
            .then((data) => {
                dispatch({ type: "RECEIVE_VENUE_DATA", venues: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_VENUE_DATA" });
    },
};

const emptyVenues: IVenues = {
    venues: []
}

const unloadedState: IEventWizardState = {
    isLoading: false, errors: null, venues: emptyVenues
};

export const reducer: Reducer<IEventWizardState> = (state: IEventWizardState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_VENUE_DATA":
            return {
                venues: state.venues,
                isLoading: true,
            };
        case "RECEIVE_VENUE_DATA":
            return {
                venues: action.venues,
                isLoading: false,
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
