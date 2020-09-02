import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from ".";
import { StateTypesResponseViewModel } from '../models/Finance/StateTypesResponseViewModel';
import City from '../models/Common/CityViewModel';

export interface IStatesTypeProps {
  statesType: IStateTypeComponentState;
}

export class CitiesResponseModel {
  cities: City[]
}

export interface IStateTypeComponentState {
  isLoading?: boolean;
  statesTypeSuccess?: boolean;
  citiesSuccess: boolean;
  statesTypes?: StateTypesResponseViewModel;
  cities: CitiesResponseModel;
}

interface IRequestStatesTypes {
  type: "REQUEST_STATES_TYPE_DATA";
}

interface IReceiveStatesTypes {
  type: "RECEIVE_STATES_TYPE_DATA";
  payload: StateTypesResponseViewModel;
}

interface IRequestCities {
  type: "REQUEST_CITIES_DATA";
}

interface IReceiveCities {
  type: "RECEIVE_CITIES_DATA";
  cities: CitiesResponseModel;
}

type KnownAction = IRequestStatesTypes | IReceiveStatesTypes | IRequestCities | IReceiveCities;

export const actionCreators = {
  requestStatesTypeData: (countryModel: any, callback: (StateTypesResponseViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {

      const fetchTask = fetch(`api/state/` + countryModel)
        .then((response) => response.json() as Promise<StateTypesResponseViewModel>)
        .then((data) => {
          dispatch({ type: "RECEIVE_STATES_TYPE_DATA", payload: data });
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_STATES_TYPE_DATA" });
    },
  requestCitiesData: (stateAltId: string, callback: (CitiesResponseModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/cities/${stateAltId}`)
        .then((response) => response.json() as Promise<CitiesResponseModel>)
        .then((data) => {
          dispatch({ type: "RECEIVE_CITIES_DATA", cities: data });
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_CITIES_DATA" });
    },
};

const unloadedStateTypes: StateTypesResponseViewModel = {
  states: [],
};

const unloadedCities: CitiesResponseModel = {
  cities: [],
};

const unloadedStatesType: IStateTypeComponentState = {
  isLoading: false,
  statesTypeSuccess: false,
  statesTypes: unloadedStateTypes,
  cities: unloadedCities,
  citiesSuccess: false
};

export const reducer: Reducer<IStateTypeComponentState> = (state: IStateTypeComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_STATES_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        statesTypeSuccess: false,
        statesTypes: unloadedStateTypes
      };
    case "RECEIVE_STATES_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        statesTypeSuccess: true,
        statesTypes: action.payload
      };
    case "REQUEST_CITIES_DATA":
      return {
        ...state,
        isLoading: true,
      };
    case "RECEIVE_CITIES_DATA":
      return {
        ...state,
        isLoading: false,
        cities: action.cities,
        citiesSuccess: true
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedStatesType;
};
