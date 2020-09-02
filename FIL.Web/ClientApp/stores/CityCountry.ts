import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from './';
import { CityCountryViewModel, CityCountryResult, CityResult } from '../models/CityCountyLanding/CityCountryViewModel';
import { CategoryPageResponseViewModel } from '../models/CategoryPageResponseViewModel';

//constants
export const REQUEST_CITY_COUNTRY_DATA = 'REQUEST_CITY_COUNTRY_DATA';
export const RECEIVE_CITY_COUNTRY_DATA = 'RECEIVE_CITY_COUNTRY_DATA';
export const CLEAR_CITY_COUNTRY_DATA = 'CLEAR_CITY_COUNTRY_DATA';

export const REQUEST_COUNTRY_ALL_PLACE_DATA = 'REQUEST_COUNTRY_ALL_PLACE_DATA';
export const RECEIVE_COUNTRY_ALL_PLACE_DATA = 'RECEIVE_COUNTRY_ALL_PLACE_DATA';
export const CLEAR_COUNTRY_ALL_PLACE_DATA = 'CLEAR_COUNTRY_ALL_PLACE_DATA';

export const REQUEST_COUNTRY_STATE_DATA = 'REQUEST_COUNTRY_STATE_DATA';
export const RECEIVE_COUNTRY_STATE_DATA = 'RECEIVE_COUNTRY_STATE_DATA';
export const CLEAR_COUNTRY_STATE_DATA = 'CLEAR_COUNTRY_STATE_DATA';

export const REQUEST_CITY_DATA = 'REQUEST_CITY_DATA';
export const RECEIVE_CITY_DATA = 'RECEIVE_CITY_DATA';
export const CLEAR_CITY_DATA = 'CLEAR_CITY_DATA';

export interface ICityCountryProps {
	cityCountry: ICityCountryState;
}

export interface ICityCountryState {
	isLoading?: boolean;
	cityCountry: CityCountryViewModel;
	countryAllPlace: CategoryPageResponseViewModel[];
	countryState: CityCountryResult;
	cityData: CityResult;
	fetchCountryallPlaceData: boolean;
}
export interface ICountryAllPlace {
	countryAllPlace: CategoryPageResponseViewModel[];
}

export interface ICountryState {
	countryState: CityCountryResult[];
}

//----------------------------------------
interface IRequestCityCountryAction {
	type: 'REQUEST_CITY_COUNTRY_DATA';
}

interface IReceiveCityCountryDataAction {
	type: 'RECEIVE_CITY_COUNTRY_DATA';
	cityCountry: CityCountryViewModel;
}

interface IClearCityCountryDataAction {
	type: 'CLEAR_CITY_COUNTRY_DATA';
}
//----------------------------------------
interface IRequestCountryAllPlaceAction {
	type: 'REQUEST_COUNTRY_ALL_PLACE_DATA';
}

interface IReceiveCountryAllPlaceDataAction {
	type: 'RECEIVE_COUNTRY_ALL_PLACE_DATA';
	countryAllPlace: CategoryPageResponseViewModel[];
}

interface IClearCountryAllPlaceDataAction {
	type: 'CLEAR_COUNTRY_ALL_PLACE_DATA';
}
//---------------------------------------
interface IRequestCountryStateAction {
	type: 'REQUEST_COUNTRY_STATE_DATA';
}

interface IReceiveCountryStateDataAction {
	type: 'RECEIVE_COUNTRY_STATE_DATA';
	countryState: CityCountryResult;
}

interface IClearCountryStateDataAction {
	type: 'CLEAR_COUNTRY_STATE_DATA';
}
//----------------------------------------
interface IRequestCityAction {
	type: 'REQUEST_CITY_DATA';
}

interface IReceiveCityDataAction {
	type: 'RECEIVE_CITY_DATA';
	cityData: CityResult;
}

interface IClearCityDataAction {
	type: 'CLEAR_CITY_DATA';
}

const emptyCityCountryResult: CityCountryViewModel = {
	cityCountry: [],
	countryAllPlace: [],
	countryState: [],
	cityData: null
};

type KnownAction =
	| IRequestCityCountryAction
	| IReceiveCityCountryDataAction
	| IClearCityCountryDataAction
	| IRequestCountryAllPlaceAction
	| IReceiveCountryAllPlaceDataAction
	| IClearCountryAllPlaceDataAction
	| IRequestCountryStateAction
	| IReceiveCountryStateDataAction
	| IClearCountryStateDataAction
	| IRequestCityAction
	| IReceiveCityDataAction
	| IClearCityDataAction;

export const actionCreators = {
	//action for getting all country and there places counts
	requestCountryPlaceData: (category?: number): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/get/countryplace/${category}`)
			.then((response) => response.json() as Promise<CityCountryViewModel>)
			.then((cityCountry) => {
				dispatch({ type: 'RECEIVE_CITY_COUNTRY_DATA', cityCountry });
			});
		addTask(fetchTask);
		dispatch({ type: 'REQUEST_CITY_COUNTRY_DATA' });
	},

	//action for get all place details based on country
	requestCountryAllPlaceData: (country): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/get/all/countryplace/${country}`)
			.then((response) => response.json() as Promise<CategoryPageResponseViewModel[]>)
			.then((data) => {
				dispatch({ type: 'RECEIVE_COUNTRY_ALL_PLACE_DATA', countryAllPlace: data });
			});
		addTask(fetchTask);
		dispatch({ type: 'REQUEST_COUNTRY_ALL_PLACE_DATA' });
	},

	requestCountryStateData: (country): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/get/stateplace/${country}`)
			.then((response) => response.json() as Promise<any>)
			.then((data) => {
				dispatch({ type: 'RECEIVE_COUNTRY_STATE_DATA', countryState: data });
			});
		addTask(fetchTask);
		dispatch({ type: 'REQUEST_COUNTRY_STATE_DATA' });
	},

	requestCityData: (city): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/get/all/cityplace/${city}`)
			.then((response) => response.json() as Promise<any>)
			.then((data) => {
				dispatch({ type: 'RECEIVE_CITY_DATA', cityData: data });
			});
		addTask(fetchTask);
		dispatch({ type: 'REQUEST_CITY_DATA' });
	},

	clearCityCountryAction: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		dispatch({ type: 'CLEAR_COUNTRY_STATE_DATA' });
	}
};

const unloadedState: ICityCountryState = {
	isLoading: false,
	cityCountry: emptyCityCountryResult,
	countryAllPlace: undefined,
	countryState: null,
	cityData: null,
	fetchCountryallPlaceData: false
};
const emptycountryAllPlace: CategoryPageResponseViewModel = {
	event: null,
	venue: [],
	city: [],
	state: [],
	country: [],
	rating: [],
	categoryEvent: null,
	eventTicketAttribute: [],
	eventTicketDetail: [],
	eventCategories: [],
	currencyType: null,
	eventType: null,
	eventCategory: null,
	parentCategory: null,
	countryDescription: null,
	CoutryContentMapping: [],
	cityDescription: null,
	duration: null
}

export const reducer: Reducer<ICityCountryState> = (state: ICityCountryState, incomingAction: Action) => {
	const action = incomingAction as KnownAction;
	switch (action.type) {
		case REQUEST_CITY_COUNTRY_DATA:
			return {
				...state,
				isLoading: true
			};
		case RECEIVE_CITY_COUNTRY_DATA:
			return {
				...state,
				isLoading: false,
				cityCountry: action.cityCountry
			};

		case CLEAR_CITY_COUNTRY_DATA:
			return {
				...state,
				isLoading: false,
				cityCountry: emptyCityCountryResult
			};
		case REQUEST_COUNTRY_ALL_PLACE_DATA:
			return {
				...state,
				isLoading: true
			};
		case RECEIVE_COUNTRY_ALL_PLACE_DATA:
			return {
				...state,
				isLoading: false,
				countryAllPlace: action.countryAllPlace
			};
		case CLEAR_COUNTRY_ALL_PLACE_DATA:
			return {
				...state,
				isLoading: false,
				fetchCountryallPlaceData: false
			};
		case REQUEST_COUNTRY_STATE_DATA:
			return {
				...state,
				isLoading: true
			};
		case RECEIVE_COUNTRY_STATE_DATA:
			return {
				...state,
				isLoading: false,
				countryState: action.countryState,
				fetchCountryallPlaceData: true
			};
		case CLEAR_COUNTRY_STATE_DATA:
			return {
				...state,
				isLoading: false
			};
		case REQUEST_CITY_DATA:
			return {
				...state,
				isLoading: true
			};
		case RECEIVE_CITY_DATA:
			return {
				...state,
				isLoading: false,
				cityData: action.cityData
			};
		case CLEAR_CITY_DATA:
			return {
				...state
			};
		default:
			const exhaustiveCheck: never = action;
	}
	return state || unloadedState;
};
