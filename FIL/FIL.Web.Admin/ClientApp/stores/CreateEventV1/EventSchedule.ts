import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../index";
import { EventScheduleViewModel, EventRecurranceInputViewModel, EventRecurranceResponseViewModel, EventRecurranceScheduleModel } from "../../models/CreateEventV1/EventScheduleViewModel";
import { eventScheduleService } from "../../services/CreateEventV1/EventSchedule";

export interface IEventScheduleProps {
  EventSchedule: IEventScheduleComponentState;
}

export interface IEventScheduleComponentState {
  isLoading?: boolean;
  isEventScheduleSuccess?: boolean;
  isEventScheduleFailure?: boolean;
  eventSchedule?: EventScheduleViewModel;
  recurrentEventSchedule?: EventRecurranceResponseViewModel;
  timeZoneKey: string;
}

interface IRequestEventSchedule {
  type: "REQUEST_EVENT_SCHEDULE_DATA";
}

interface IReceiveEventSchedule {
  type: "RECEIVE_EVENT_SCHEDULE_DATA";
  eventSchedule?: EventScheduleViewModel;
  recurrentEventSchedule?: EventRecurranceResponseViewModel;
}

interface IFailureEventSchedule {
  type: "FAILURE_EVENT_SCHEDULE_DATA";
}

interface ITimeZoneChange {
  type: "ON_CHANGE_TIME_ZONE";
  timeZoneKey?: string;
}

type KnownAction = IRequestEventSchedule | IReceiveEventSchedule | IFailureEventSchedule | ITimeZoneChange;

export const actionCreators = {
  requestEventSchedule: (eventId: number, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-schedule/${eventId}`)
        .then((response) => response.json() as Promise<EventScheduleViewModel>)
        .then((data: EventScheduleViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", eventSchedule: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
    },
  saveEventSchedule: (eventScheduleInputModel: EventScheduleViewModel, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
      eventScheduleService.saveEventScheduleRequest(eventScheduleInputModel)
        .then((response: EventScheduleViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", eventSchedule: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          });
    },
  requestReschedule: (eventId: number, startDate: string, endDate: string, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/recurrance-schedule?eventId=${eventId}&startDate=${startDate}&endDate=${endDate}`)
        .then((response) => response.json() as Promise<EventRecurranceResponseViewModel>)
        .then((data: EventRecurranceResponseViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", recurrentEventSchedule: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
    },
  saveBulkInsert: (eventScheduleInputModel: EventRecurranceInputViewModel, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
      eventScheduleService.saveBulkInsertRequest(eventScheduleInputModel)
        .then((response: EventScheduleViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", eventSchedule: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          });
    },
  saveBulkReschedule: (eventScheduleInputModel: EventRecurranceInputViewModel, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
      eventScheduleService.saveBulkRescheduleRequest(eventScheduleInputModel)
        .then((response: EventScheduleViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", eventSchedule: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          });
    },
  saveSingleReschedule: (eventScheduleInputModel: EventRecurranceInputViewModel, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
      eventScheduleService.saveSingleRescheduleRequest(eventScheduleInputModel)
        .then((response: EventScheduleViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", eventSchedule: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          });
    },
  deleteBulkSchedule: (eventScheduleInputModel: EventRecurranceInputViewModel, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
      eventScheduleService.deleteBulkScheduleRequest(eventScheduleInputModel)
        .then((response: EventScheduleViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", eventSchedule: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          });
    },
  deleteSingleSchedule: (eventScheduleInputModel: EventRecurranceInputViewModel, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_SCHEDULE_DATA" });
      eventScheduleService.deleteSingleScheduleRequest(eventScheduleInputModel)
        .then((response: EventScheduleViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_SCHEDULE_DATA", eventSchedule: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_SCHEDULE_DATA" });
          });
    },
  onChangeTimezone: (timeZoneKey: string)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "ON_CHANGE_TIME_ZONE", timeZoneKey: timeZoneKey });
    }
};

let unloadedEventScheduleViewModel: EventScheduleViewModel = {
  eventScheduleModel: undefined,
  success: false
}

let unloadedEventRecurranceResponseViewModel: EventRecurranceResponseViewModel = {
  eventRecurranceScheduleModel: [],
  success: false,
  isDraft: false,
  isValidLink: false
}

const unloadedEventSchedule: IEventScheduleComponentState = {
  isLoading: false,
  eventSchedule: unloadedEventScheduleViewModel,
  recurrentEventSchedule: unloadedEventRecurranceResponseViewModel,
  isEventScheduleFailure: false,
  isEventScheduleSuccess: false,
  timeZoneKey: ''
};

export const reducer: Reducer<IEventScheduleComponentState> = (state: IEventScheduleComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_SCHEDULE_DATA":
      return {
        ...state,
        isLoading: true,
        isEventScheduleFailure: false,
        isEventScheduleSuccess: false,
        eventSchedule: unloadedEventScheduleViewModel
      };
    case "RECEIVE_EVENT_SCHEDULE_DATA":
      return {
        ...state,
        isLoading: false,
        isEventScheduleFailure: false,
        isEventScheduleSuccess: true,
        eventSchedule: action.eventSchedule ? action.eventSchedule : state.eventSchedule,
        recurrentEventSchedule: action.recurrentEventSchedule ? action.recurrentEventSchedule : state.recurrentEventSchedule
      };
    case "FAILURE_EVENT_SCHEDULE_DATA":
      return {
        ...state,
        isLoading: false,
        isEventScheduleFailure: true,
        isEventScheduleSuccess: false,
        eventSchedule: unloadedEventScheduleViewModel
      };
    case "ON_CHANGE_TIME_ZONE":
      return {
        ...state,
        timeZoneKey: action.timeZoneKey
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedEventSchedule;
};
