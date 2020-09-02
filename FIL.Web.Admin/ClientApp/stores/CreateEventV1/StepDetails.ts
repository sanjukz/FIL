import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../";
import { StepViewModel, StepModel } from "../../models/CreateEventV1/StepViewModel";

export interface IStepDetailsProps {
  StepDetails: IStepDetailComponentState;
}

export interface IStepDetailComponentState {
  isLoading?: boolean;
  isStepDetailSuccess?: boolean;
  isStepDetailFailure?: boolean;
  stepDetails?: any;
}

interface IRequestStepDetail {
  type: "REQUEST_STEP_DETAIL_DATA";
}

interface IReceiveStepDetail {
  type: "RECEIVE_STEP_DETAIL_DATA";
  stepDetails: StepViewModel;
}

interface IFailureStepDetail {
  type: "FAILURE_STEP_DETAIL_DATA";
}

type KnownAction = IRequestStepDetail | IReceiveStepDetail | IFailureStepDetail;

export const actionCreators = {
  requestStepDetails: (masterEventType: number, eventId: number, callback: (EventDetailViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/steps/${masterEventType}?eventId=${eventId}`)
        .then((response) => response.json() as Promise<StepViewModel>)
        .then((data: StepViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_STEP_DETAIL_DATA", stepDetails: data });
          } else {
            dispatch({ type: "FAILURE_STEP_DETAIL_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_STEP_DETAIL_DATA" });
    },
};

let unloadedStepModel: StepModel = {
  stepId: 1,
  description: "",
  icon: "",
  isEnabled: false,
  name: "",
  slug: "",
  sortOrder: 1
}

export const unloadedStepViewModel: StepViewModel = {
  stepModel: [],
  completedStep: "",
  currentStep: 1,
  eventId: 0,
  success: false
}

const unloadedStepDetailComponentState: IStepDetailComponentState = {
  isLoading: false,
  stepDetails: unloadedStepViewModel,
  isStepDetailFailure: false,
  isStepDetailSuccess: false
};

export const reducer: Reducer<IStepDetailComponentState> = (state: IStepDetailComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_STEP_DETAIL_DATA":
      return {
        isLoading: true,
        isStepDetailFailure: false,
        isStepDetailSuccess: false,
        stepDetails: unloadedStepDetailComponentState
      };
    case "RECEIVE_STEP_DETAIL_DATA":
      return {
        isLoading: false,
        isStepDetailFailure: false,
        isStepDetailSuccess: true,
        stepDetails: action.stepDetails
      };
    case "FAILURE_STEP_DETAIL_DATA":
      return {
        isLoading: false,
        isStepDetailFailure: true,
        isStepDetailSuccess: false,
        stepDetails: unloadedStepDetailComponentState
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedStepDetailComponentState;
};
