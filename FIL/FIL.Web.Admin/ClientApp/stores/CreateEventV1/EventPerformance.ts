import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../";
import { EventPerformanceViewModel, PerformanceTypeModel } from "../../models/CreateEventV1/EventPerformanceViewModel";
import { eventPerformanceService } from "../../services/CreateEventV1/EventPerformance";

export interface IEventPerformanceProps {
  EventPerformance: IEventPerformanceComponentState;
}

export interface IEventPerformanceComponentState {
  isLoading?: boolean;
  isEventPerformanceSuccess?: boolean;
  isEventPerformanceFailure?: boolean;
  isSaveRequest?: boolean;
  eventPerformance?: any;
}

interface IRequestEventPerformance {
  type: "REQUEST_EVENT_PERFORMANCE_DATA";
  isSaveRequest: boolean
}

interface IReceiveEventPerformance {
  type: "RECEIVE_EVENT_PERFORMANCE_DATA";
  eventPerformance: EventPerformanceViewModel;
}

interface IFailureEventPerformance {
  type: "FAILURE_EVENT_PERFORMANCE_DATA";
}

type KnownAction = IRequestEventPerformance | IReceiveEventPerformance | IFailureEventPerformance;

export const actionCreators = {
  requestEventPerformance: (eventId: number, callback: (EventPerformanceViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-performance/${eventId}`)
        .then((response) => response.json() as Promise<EventPerformanceViewModel>)
        .then((data: EventPerformanceViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_PERFORMANCE_DATA", eventPerformance: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_PERFORMANCE_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_PERFORMANCE_DATA", isSaveRequest: false });
    },
  saveEventPerformance: (eventPerformanceViewModel: EventPerformanceViewModel, callback: (EventPerformanceViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_PERFORMANCE_DATA", isSaveRequest: true });
      eventPerformanceService.saveEventPerformanceRequest(eventPerformanceViewModel)
        .then((response: EventPerformanceViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_PERFORMANCE_DATA", eventPerformance: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_PERFORMANCE_DATA" });
          }
          callback(response);
        })
    },
};

let unloadedPerformanceTypeModel: PerformanceTypeModel = {
  eventId: 0,
  id: 0,
  isEnabled: false,
  onlineEventTypeId: 0,
  performanceTypeId: 0
}

let unloadedEventPerformanceViewModel: EventPerformanceViewModel = {
  eventId: 0,
  onlineEventType: "",
  performanceTypeModel: unloadedPerformanceTypeModel,
  success: false
}

const unloadedEventPerformanceComponent: IEventPerformanceComponentState = {
  isLoading: false,
  eventPerformance: unloadedEventPerformanceViewModel,
  isEventPerformanceFailure: false,
  isEventPerformanceSuccess: false,
  isSaveRequest: false
};

export const reducer: Reducer<IEventPerformanceComponentState> = (state: IEventPerformanceComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_PERFORMANCE_DATA":
      return {
        isLoading: true,
        isSaveRequest: action.isSaveRequest,
        isEventPerformanceFailure: false,
        isEventPerformanceSuccess: false,
        eventPerformance: unloadedEventPerformanceComponent
      };
    case "RECEIVE_EVENT_PERFORMANCE_DATA":
      return {
        ...state,
        isLoading: false,
        isEventPerformanceFailure: false,
        isEventPerformanceSuccess: true,
        eventPerformance: action.eventPerformance
      };
    case "FAILURE_EVENT_PERFORMANCE_DATA":
      return {
        ...state,
        isLoading: false,
        isEventPerformanceFailure: true,
        isEventPerformanceSuccess: false,
        eventPerformance: unloadedEventPerformanceComponent
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedEventPerformanceComponent;
};
