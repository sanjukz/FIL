import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "..";
import { ReplayViewModel, ReplayModel } from "../../models/CreateEventV1/ReplayViewModel";
import { eventReplayService } from "../../services/CreateEventV1/EventReplay";

export interface IEventReplayProps {
  EventReplay: IEventReplayComponentState;
}

export interface IEventReplayComponentState {
  isLoading?: boolean;
  isEventReplaySuccess?: boolean;
  isEventReplayFailure?: boolean;
  isSaveRequest: boolean,
  eventReplay?: any;
}

interface IRequestEventReplay {
  type: "REQUEST_EVENT_REPLAY_DATA";
  isSaveRequest: boolean;
}

interface IReceiveEventReplay {
  type: "RECEIVE_EVENT_REPLAY_DATA";
  eventReplay: ReplayViewModel;
}

interface IFailureEventReplay {
  type: "FAILURE_EVENT_REPLAY_DATA";
}

type KnownAction = IRequestEventReplay | IReceiveEventReplay | IFailureEventReplay;

export const actionCreators = {
  requestEventReplayDetails: (eventId: number, callback: (EventDetailViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/replay-details/${eventId}`)
        .then((response) => response.json() as Promise<ReplayViewModel>)
        .then((data: ReplayViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_REPLAY_DATA", eventReplay: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_REPLAY_DATA" });
          }
          callback(data);
        });
      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_REPLAY_DATA", isSaveRequest: false });
    },
  saveEventReplay: (eventDetailViewModel: ReplayViewModel, callback: (ReplayViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_REPLAY_DATA", isSaveRequest: true });
      eventReplayService.saveEventReplay(eventDetailViewModel)
        .then((response: ReplayViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_REPLAY_DATA", eventReplay: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_REPLAY_DATA" });
          }
          callback(response);
        })
    },
};

let unloadedEventReplayViewModel: ReplayViewModel = {
  replayDetailModel: [],
  eventId: 0,
  success: false
}

const unloadedEventReplay: IEventReplayComponentState = {
  isLoading: false,
  eventReplay: unloadedEventReplayViewModel,
  isEventReplayFailure: false,
  isSaveRequest: false,
  isEventReplaySuccess: false
};

export const reducer: Reducer<IEventReplayComponentState> = (state: IEventReplayComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_REPLAY_DATA":
      return {
        isLoading: true,
        isEventReplayFailure: false,
        isEventReplaySuccess: false,
        isSaveRequest: action.isSaveRequest,
        eventReplay: unloadedEventReplay
      };
    case "RECEIVE_EVENT_REPLAY_DATA":
      return {
        isLoading: false,
        isEventReplayFailure: false,
        isEventReplaySuccess: true,
        isSaveRequest: false,
        eventReplay: action.eventReplay
      };
    case "FAILURE_EVENT_REPLAY_DATA":
      return {
        isLoading: false,
        isEventReplayFailure: true,
        isEventReplaySuccess: false,
        eventReplay: unloadedEventReplay,
        isSaveRequest: false,
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedEventReplay;
};
