import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from ".";
import { CountryTypesResponseViewModel } from '../models/Finance/CountryTypesResponseViewModel';

export interface ICountryTypeProps {
  countryType: ICountryTypeComponentState;
}

export interface ICountryTypeComponentState {
  isLoading: boolean;
  countryTypeSuccess: boolean;
  countryTypes: CountryTypesResponseViewModel;
}

interface IRequestCountryTypes {
  type: "REQUEST_COUNTRY_TYPE_DATA";
}

interface IReceiveCountryTypes {
  type: "RECEIVE_COUNTRY_TYPE_DATA";
  payload: CountryTypesResponseViewModel;
}

type KnownAction = IRequestCountryTypes | IReceiveCountryTypes;

export const actionCreators = {
  requestCountryTypeData: (callback?: (res: CountryTypesResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/country/all`)
      .then((response) => response.json() as Promise<CountryTypesResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_COUNTRY_TYPE_DATA", payload: data });
        if (callback) {
          callback(data);
        }
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_COUNTRY_TYPE_DATA" });
  },

};

const unloadedCountryTypes: CountryTypesResponseViewModel = {
  countries: [],
};

const unloadedCountryTypeState: ICountryTypeComponentState = {
  isLoading: false,
  countryTypeSuccess: false,
  countryTypes: unloadedCountryTypes,

};

export const reducer: Reducer<ICountryTypeComponentState> = (state: ICountryTypeComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;

  switch (action.type) {
    case "REQUEST_COUNTRY_TYPE_DATA":
      return {
        isLoading: true,
        countryTypeSuccess: false,
        countryTypes: unloadedCountryTypes
      };
    case "RECEIVE_COUNTRY_TYPE_DATA":
      return {

        isLoading: true,
        countryTypeSuccess: true,
        countryTypes: action.payload

      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedCountryTypeState;
};
