import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "./";
import ApproveModerateResponseViewModel from '../models/ApproveModerate/ApproveModerateResponseViewModel';

export interface IApproveModerateProps {
  ApproveModerate: IApproveModerateState;
}

export interface IApproveModerateState {
  isLoading: boolean;
  approveSuccess: boolean;
  isPlaceApproved: boolean;
  approveModerateResponseViewModel: ApproveModerateResponseViewModel;
}

interface IRequestApproveModeratePlace {
  type: "REQUEST_APPROVE_MODERATE";
}

interface IRecieveApproveModeratePlace {
  type: "RECEIVE_APPROVE_MODERATE";
  approveModerateResponseViewModel: ApproveModerateResponseViewModel;
}

interface IRequestIsPlaceApproved {
  type: "REQUEST_APPROVE_PLACE";
}

interface IRecieveIsPlaceApproved {
  type: "RECEIVE_APPROVE_PLACE";
  isPlaceApproved: boolean
}

type KnownAction = IRequestApproveModeratePlace | IRecieveApproveModeratePlace | IRequestIsPlaceApproved | IRecieveIsPlaceApproved;

export const actionCreators = {
  requestApproveModeratePlaceData: (isDeactivatedPlace: boolean): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/feeladmin/place/${isDeactivatedPlace}`)
      .then((response) => response.json() as Promise<ApproveModerateResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_APPROVE_MODERATE", approveModerateResponseViewModel: data, });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_APPROVE_MODERATE" });
  },
  requestEnableApproveModeratePlace: (placeAltID: string, isDeactivate: boolean, eventId: number, eventStatus: number, callback: ({ success: boolean }) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/approvePlace/${placeAltID}/${isDeactivate}?eventId=${eventId}&eventStatus=${eventStatus}`)
        .then((response) => response.json() as Promise<{ success: boolean }>)
        .then((data) => {
          dispatch({ type: "RECEIVE_APPROVE_PLACE", isPlaceApproved: true, });
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_APPROVE_PLACE" });
    },
};

const unloadedApproveModerate: ApproveModerateResponseViewModel = {
  events: [],
  users: [],
  eventAttributes: [],
  myFeelDetails: []
};

const unloadedCurrencyTypeState: IApproveModerateState = {
  isLoading: false,
  approveSuccess: false,
  isPlaceApproved: false,
  approveModerateResponseViewModel: unloadedApproveModerate
};

export const reducer: Reducer<IApproveModerateState> = (state: IApproveModerateState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_APPROVE_MODERATE":
      return {
        isLoading: true,
        approveSuccess: false,
        approveModerateResponseViewModel: unloadedApproveModerate,
        isPlaceApproved: state.isPlaceApproved
      };
    case "RECEIVE_APPROVE_MODERATE":
      return {
        isLoading: false,
        approveSuccess: true,
        approveModerateResponseViewModel: action.approveModerateResponseViewModel,
        isPlaceApproved: state.isPlaceApproved
      };
    case "REQUEST_APPROVE_PLACE":
      return {
        isLoading: true,
        approveSuccess: state.approveSuccess,
        approveModerateResponseViewModel: state.approveModerateResponseViewModel,
        isPlaceApproved: false
      };
    case "RECEIVE_APPROVE_PLACE":
      return {
        isLoading: false,
        approveSuccess: state.approveSuccess,
        approveModerateResponseViewModel: state.approveModerateResponseViewModel,
        isPlaceApproved: true
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedCurrencyTypeState;
};
