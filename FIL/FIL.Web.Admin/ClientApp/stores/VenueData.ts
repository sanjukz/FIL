import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { CountryFormDataViewModel } from "../models/CountryFormDataViewModel";
import { SaveCountryResponseViewModel } from "../models/SaveCountryResponseViewModel";
import { StateFormDataViewModel } from "../models/StateFormDataViewModel";
import { StateResponseViewModel } from "../models/StateResponseViewModel";
import { CityFormDataViewModel } from "../models/CityFormDataViewModel";
import { CityResponseViewModel } from "../models/CityResponseViewModel";
import { ZipcodeFormDataViewModel } from "../models/ZipcodeFormDataViewModel";
import { ZipcodeResponseViewModel } from "../models/ZipcodeResponseViewModel";
import { VenueCreationFormDataviewModel } from "../models/VenueCreationFormDataviewModel";
import { VenueCreationResponseViewModel } from "../models/VenueCreationResponseViewModel";
import { venueService } from "../services/venue";

export const REGISTER_REQUEST = "REGISTER_REQUEST";
export const REGISTER_SUCCESS = "REGISTER_SUCCESS";
export const REGISTER_FAILURE = "REGISTER_FAILURE";

export interface IVenueComponentState {
    isLoading?: boolean;
    val?: any;
    valCountry?: any;
    valState?: any;
    valCity?: any;
    valZip?: any;
    isHiddenCountry?: boolean;
    isHiddenState?: boolean;
    isHiddenCity?: boolean;
    isHiddenZip?: boolean;
    countries: ICountries;
    states: IStates;
    cities: ICities;
    zipcodes: IZipcodes;
    requesting?: boolean;
    registered?: boolean;
    errors?: any;
}

export interface ICountry {
    id: string;
    name: string;
    isoAlphaTwoCode: string;
    isoAlphaThreeCode: string;
    numcode: number;
    phonecode: number;
}

export interface IState {
    id: string;
    name: string;
    abbreviation: string;
    countryId: number;
}

export interface ICity {
    id: string;
    name: string;
    stateId: number;
}

export interface IZipcode {
    id: string;
    postalcode: string;
    region: string;
    cityId: number;
}

export interface ICountries {
    countries: ICountry[];
}

export interface IStates {
    states: IState[];
}

export interface ICities {
    cities: ICity[];
}

export interface IZipcodes {
    zipcodes: IZipcode[];
}

interface IRequestCountryDataAction {
    type: "REQUEST_COUNTRY_DATA";
}

interface IReceiveCountryDataAction {
    type: "RECEIVE_COUNTRY_DATA";
    countries: ICountries;
}

interface IRequestStateDataAction {
    type: "REQUEST_STATE_DATA";
}

interface IReceiveStateDataAction {
    type: "RECEIVE_STATE_DATA";
    states: IStates;
}

interface IRequestCityDataAction {
    type: "REQUEST_CITY_DATA";
}

interface IReceiveCityDataAction {
    type: "RECEIVE_CITY_DATA";
    cities: ICities;
}

interface IRequestZipeDataAction {
    type: "REQUEST_ZIP_DATA";
}

interface IReceiveZipDataAction {
    type: "RECEIVE_ZIP_DATA";
    zipcodes: IZipcodes;

}

interface ICountryRegistrationRequestAction {
    type: "REGISTER_REQUEST";
}

interface ICountryRegistrationSuccesstAction {
    type: "REGISTER_SUCCESS";
}

interface ICountryRegistrationFailureAction {
    type: "REGISTER_FAILURE";
}

type KnownAction = IRequestCountryDataAction | IReceiveCountryDataAction | ICountryRegistrationRequestAction | ICountryRegistrationSuccesstAction | ICountryRegistrationFailureAction | IRequestStateDataAction | IReceiveStateDataAction | IRequestCityDataAction | IReceiveCityDataAction | IRequestZipeDataAction | IReceiveZipDataAction;

export const actionCreators = {
    requestStateData: (id: number): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/state?id=${id}`)
            .then((response) => response.json() as Promise<IStates>)
            .then((data) => {
                dispatch({ type: "RECEIVE_STATE_DATA", states: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_STATE_DATA" });
    },
    requestCityData: (id: number): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/city?id=${id}`)
            .then((response) => response.json() as Promise<ICities>)
            .then((data) => {
                dispatch({ type: "RECEIVE_CITY_DATA", cities: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_CITY_DATA" });
    },
    requestZipData: (id: number): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/zipcode?id=${id}`)
            .then((response) => response.json() as Promise<IZipcodes>)
            .then((data) => {
                dispatch({ type: "RECEIVE_ZIP_DATA", zipcodes: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_ZIP_DATA" });
    },

    requestCountryData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/country/all`)
            .then((response) => response.json() as Promise<ICountries>)
            .then((data) => {
                dispatch({ type: "RECEIVE_COUNTRY_DATA", countries: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_COUNTRY_DATA" });

    },

    addCountry: (addCountry: CountryFormDataViewModel, callback: (SaveCountryResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: REGISTER_REQUEST });

            venueService.registerCoutry(addCountry)
                .then((response: SaveCountryResponseViewModel) => {
                    dispatch({ type: REGISTER_SUCCESS });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: REGISTER_FAILURE });
                });
        },
    addState: (addState: StateFormDataViewModel, callback: (StateResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: REGISTER_REQUEST });

            venueService.registerState(addState)
                .then((response: StateResponseViewModel) => {
                    dispatch({ type: REGISTER_SUCCESS });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: REGISTER_FAILURE });
                });
        },

    addCity: (addCity: CityFormDataViewModel, callback: (CityResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            alert(addCity.stateId)
            dispatch({ type: REGISTER_REQUEST });

            venueService.registerCity(addCity)
                .then((response: CityResponseViewModel) => {
                    dispatch({ type: REGISTER_SUCCESS });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: REGISTER_FAILURE });
                });
        },
    addZipcode: (addZipcode: ZipcodeFormDataViewModel, callback: (ZipcodeResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: REGISTER_REQUEST });

            venueService.registerZipcode(addZipcode)
                .then((response: ZipcodeResponseViewModel) => {
                    dispatch({ type: REGISTER_SUCCESS });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: REGISTER_FAILURE });
                });
        },
    addVenue: (addVenue: VenueCreationFormDataviewModel, callback: (VenueCreationResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {

            dispatch({ type: REGISTER_REQUEST });

            venueService.saveVenue(addVenue)
                .then((response: VenueCreationResponseViewModel) => {
                    dispatch({ type: REGISTER_SUCCESS });
                    callback(response);
                },
                (error) => {
                    dispatch({ type: REGISTER_FAILURE });
                });
        },
};

const emptyCountries: ICountries = {
    countries: []
};

const emptyStates: IStates = {
    states: []
};
const emptyCities: ICities = {
    cities: []
};
const emptyZipcodes: IZipcodes = {
    zipcodes: []
};

const unloadedState: IVenueComponentState = {
    countries: emptyCountries, isLoading: false, requesting: false, registered: false, errors: null, states: emptyStates, cities: emptyCities, zipcodes: emptyZipcodes
};

export const reducer: Reducer<IVenueComponentState> = (state: IVenueComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_COUNTRY_DATA":
            return {
                countries: state.countries,
                isLoading: true,
                states: state.states,
                cities: state.cities,
                zipcodes: state.zipcodes

            };
        case "RECEIVE_COUNTRY_DATA":
            return {
                countries: action.countries,
                isLoading: false,
                states: state.states,
                cities: state.cities,
                zipcodes: state.zipcodes
            };
        case "REQUEST_STATE_DATA":
            return {
                countries: state.countries,
                isLoading: true,
                states: state.states,
                cities: state.cities,
                zipcodes: state.zipcodes
            };
        case "RECEIVE_STATE_DATA":
            return {
                states: action.states,
                isLoading: false,
                countries: state.countries,
                cities: state.cities,
                zipcodes: state.zipcodes
            };
        case "REQUEST_CITY_DATA":
            return {
                countries: state.countries,
                isLoading: true,
                states: state.states,
                cities: state.cities,
                zipcodes: state.zipcodes
            };
        case "RECEIVE_CITY_DATA":
            return {
                cities: action.cities,
                isLoading: false,
                countries: state.countries,
                states: state.states,
                zipcodes: state.zipcodes
            };
        case "REQUEST_ZIP_DATA":
            return {
                countries: state.countries,
                isLoading: true,
                states: state.states,
                cities: state.cities,
                zipcodes: state.zipcodes,

            };
        case "RECEIVE_ZIP_DATA":
            return {
                zipcodes: action.zipcodes,
                isLoading: false,
                countries: state.countries,
                states: state.states,
                cities: state.cities
            };
        case "REGISTER_REQUEST":
            return {
                requesting: true,
                registered: false,
                countries: state.countries,
                states: state.states,
                cities: state.cities,
                zipcodes: state.zipcodes
            };
        case "REGISTER_SUCCESS":
            return {
                requesting: false,
                registered: true,
                countries: state.countries,
                states: state.states,
                zipcodes: state.zipcodes,
                cities: state.cities
            };
        case "REGISTER_FAILURE":
            return {
                requesting: false,
                registered: false,
                countries: state.countries,
                states: state.states,
                cities: state.cities,
                zipcodes: state.zipcodes
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
