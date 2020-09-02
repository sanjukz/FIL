import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../index";
import { EventStepViewModel } from "../../models/CreateEventV1/EventStepViewModel";
import { EventStepService } from "../../services/CreateEventV1/EventStep";

export interface IEventStepProps {
  EventStep: IEventStepComponentState;
}

export interface IEventStepComponentState {
  isLoading?: boolean;
  isEventStepSuccess?: boolean;
  isEventStepFailure?: boolean;
  eventStep?: any
}

interface IRequestEventStep {
  type: "REQUEST_EVENT_STEP_DATA";
}

interface IReceiveEventStep {
  type: "RECEIVE_EVENT_STEP_DATA";
  eventStep: EventStepViewModel;
}

interface IFailureEventStep {
  type: "FAILURE_EVENT_STEP_DATA";
}

type KnownAction = IRequestEventStep | IReceiveEventStep | IFailureEventStep;

export const actionCreators = {
  saveEventStep: (eventStepModel: EventStepViewModel, callback: (EventStepViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_STEP_DATA" });
      EventStepService.saveEventStepRequest(eventStepModel)
        .then((response: EventStepViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_STEP_DATA", eventStep: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_STEP_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_STEP_DATA" });
          });
    },
};

let unloadedEventStepViewModel: EventStepViewModel = {
  completedStep: "",
  currentStep: 0,
  eventId: 0,
  success: false
}

const unloadedEventStep: IEventStepComponentState = {
  isLoading: false,
  isEventStepFailure: false,
  isEventStepSuccess: false
};

export const reducer: Reducer<IEventStepComponentState> = (state: IEventStepComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_STEP_DATA":
      return {
        ...state,
        isLoading: true,
        isEventStepSuccess: false,
        isEventStepFailure: false,
        eventStep: unloadedEventStep
      };
    case "RECEIVE_EVENT_STEP_DATA":
      return {
        ...state,
        isLoading: false,
        isEventStepFailure: false,
        isEventStepSuccess: true,
        eventStep: action.eventStep
      };
    case "FAILURE_EVENT_STEP_DATA":
      return {
        ...state,
        isLoading: false,
        isEventStepFailure: true,
        isEventStepSuccess: false,
        eventStep: unloadedEventStep
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedEventStep;
};
