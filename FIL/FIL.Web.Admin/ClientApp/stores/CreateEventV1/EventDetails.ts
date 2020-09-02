import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../";
import { EventDetailViewModel, EventDetailModel } from "../../models/CreateEventV1/EventDetailViewModel";
import { eventDetailService } from "../../services/CreateEventV1/EventDetails";

export interface IEventDetailsProps {
  EventDetails: IEventDetailComponentState;
}

export interface IEventDetailComponentState {
  isLoading?: boolean;
  isEventDetailSuccess?: boolean;
  isEventDetailFailure?: boolean;
  eventDetails?: EventDetailViewModel;
}

interface IRequestEventDetail {
  type: "REQUEST_EVENT_DETAIL_DATA";
}

interface IReceiveEventDetail {
  type: "RECEIVE_EVENT_DETAIL_DATA";
  eventDetails: EventDetailViewModel;
}

interface IFailureEventDetail {
  type: "FAILURE_EVENT_DETAIL_DATA";
}

type KnownAction = IRequestEventDetail | IReceiveEventDetail | IFailureEventDetail;

export const actionCreators = {
  requestEventDetails: (eventId: number, callback: (EventDetailViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-details/${eventId}`)
        .then((response) => response.json() as Promise<EventDetailViewModel>)
        .then((data: EventDetailViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_DETAIL_DATA", eventDetails: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_DETAIL_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_DETAIL_DATA" });
    },
  saveEventDetails: (eventDetailViewModel: EventDetailViewModel, callback: (EventDetailViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_DETAIL_DATA" });
      eventDetailService.saveEventDetailRequest(eventDetailViewModel)
        .then((response: EventDetailViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_DETAIL_DATA", eventDetails: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_DETAIL_DATA" });
          }
          callback(response);
        })
    },
};

let unloadedEventDetailViewModel: EventDetailViewModel = {
  eventDetail: undefined,
  success: false
}

const unloadedEventDetails: IEventDetailComponentState = {
  isLoading: false,
  eventDetails: unloadedEventDetailViewModel,
  isEventDetailFailure: false,
  isEventDetailSuccess: false
};

export const reducer: Reducer<IEventDetailComponentState> = (state: IEventDetailComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_DETAIL_DATA":
      return {
        isLoading: true,
        isEventDetailFailure: false,
        isEventDetailSuccess: false,
        eventDetails: unloadedEventDetailViewModel
      };
    case "RECEIVE_EVENT_DETAIL_DATA":
      return {
        isLoading: false,
        isEventDetailFailure: false,
        isEventDetailSuccess: true,
        eventDetails: action.eventDetails
      };
    case "FAILURE_EVENT_DETAIL_DATA":
      return {
        isLoading: false,
        isEventDetailFailure: true,
        isEventDetailSuccess: false,
        eventDetails: unloadedEventDetailViewModel
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedEventDetails;
};
