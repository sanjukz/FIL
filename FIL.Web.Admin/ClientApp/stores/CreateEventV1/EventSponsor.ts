import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../index";
import { EventSponsorViewModel } from "../../models/CreateEventV1/EventSponsorViewModel";
import { DeleteEventSponsorViewModel } from "../../models/CreateEventV1/DeleteEventSponsorViewModel";
import { eventSponsorService } from "../../services/CreateEventV1/EventSponsor";

export interface IEventSponsorProps {
  EventSponsor: IEventSponsorComponentState;
}

export interface IEventSponsorComponentState {
  isLoading?: boolean;
  isEventSponsorSuccess?: boolean;
  isEventSponsorFailure?: boolean;
  isSaveRequest?: boolean;
  eventSponsor?: EventSponsorViewModel;
  deleteEventSponsorViewModel?: DeleteEventSponsorViewModel
}

interface IRequestEventSponsor {
  type: "REQUEST_EVENT_SPONSOR_DATA";
  isSaveRequest: boolean
}

interface IReceiveEventSponsor {
  type: "RECEIVE_EVENT_SPONSOR_DATA";
  eventSponsor: EventSponsorViewModel;
}

interface IFailureEventSponsor {
  type: "FAILURE_EVENT_SPONSOR_DATA";
}

interface IRequestDeleteEventSponsor {
  type: "REQUEST_DELETE_EVENT_SPONSOR_DATA";
}

interface IReceiveDeleteEventSponsor {
  type: "RECEIVE_DELETE_EVENT_SPONSOR_DATA";
  deleteEventSponsorViewModel: DeleteEventSponsorViewModel;
}

interface IFailureDeleteEventSponsor {
  type: "FAILURE_DELETE_EVENT_SPONSOR_DATA";
}

type KnownAction = IRequestEventSponsor | IReceiveEventSponsor | IFailureEventSponsor | IRequestDeleteEventSponsor | IReceiveDeleteEventSponsor | IFailureDeleteEventSponsor;

export const actionCreators = {
  requestEventSponsor: (eventId: number, callback: (EventSponsorViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-sponsors/${eventId}`)
        .then((response) => response.json() as Promise<EventSponsorViewModel>)
        .then((data: EventSponsorViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_SPONSOR_DATA", eventSponsor: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_SPONSOR_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_SPONSOR_DATA", isSaveRequest: false });
    },
  saveEventSponsor: (eventSponsorInputViewModel: EventSponsorViewModel, callback: (EventSponsorViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_SPONSOR_DATA", isSaveRequest: true });
      eventSponsorService.saveEventSponsorRequest(eventSponsorInputViewModel)
        .then((response: EventSponsorViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_SPONSOR_DATA", eventSponsor: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_SPONSOR_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_SPONSOR_DATA" });
          });
    },
  deleteSponsor: (eventSponsorAltId: string,
    currentStep: number,
    ticketLength: number,
    callback: (DeleteEventHostViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/delete/event-sponsor/${eventSponsorAltId}?currentStep=${currentStep}&ticketLength=${ticketLength}`, {
        method: 'DELETE',
      })
        .then((response) => response.json() as Promise<DeleteEventSponsorViewModel>)
        .then((data: DeleteEventSponsorViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_DELETE_EVENT_SPONSOR_DATA", deleteEventSponsorViewModel: data });
          } else {
            dispatch({ type: "FAILURE_DELETE_EVENT_SPONSOR_DATA" });
          }
          callback(data);
        });
      addTask(fetchTask);
      dispatch({ type: "REQUEST_DELETE_EVENT_SPONSOR_DATA" });
    },
};

let unloadedEventSponsorViewModel: EventSponsorViewModel = {
  sponsorDetails: [],
  success: false,
  eventId: 0,
  isDraft: false,
  isValidLink: false,
  completedStep: "",
  currentStep: 1
}

let unloadedDeleteEventSponsorViewModel: DeleteEventSponsorViewModel = {
  success: false
}

const unloadedeventSponsor: IEventSponsorComponentState = {
  isLoading: false,
  eventSponsor: unloadedEventSponsorViewModel,
  deleteEventSponsorViewModel: unloadedDeleteEventSponsorViewModel,
  isEventSponsorFailure: false,
  isEventSponsorSuccess: false,
  isSaveRequest: false
};

export const reducer: Reducer<IEventSponsorComponentState> = (state: IEventSponsorComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_SPONSOR_DATA":
      return {
        ...state,
        isLoading: true,
        isEventSponsorFailure: false,
        isEventSponsorSuccess: false,
        isSaveRequest: action.isSaveRequest,
        eventSponsor: unloadedEventSponsorViewModel
      };
    case "RECEIVE_EVENT_SPONSOR_DATA":
      return {
        ...state,
        isLoading: false,
        isEventSponsorFailure: false,
        isEventSponsorSuccess: true,
        isSaveRequest: false,
        eventSponsor: action.eventSponsor
      };
    case "FAILURE_EVENT_SPONSOR_DATA":
      return {
        ...state,
        isLoading: false,
        isEventSponsorFailure: true,
        isEventSponsorSuccess: false,
        isSaveRequest: false,
        eventSponsor: unloadedEventSponsorViewModel
      };
    case "REQUEST_DELETE_EVENT_SPONSOR_DATA":
      return {
        ...state,
        isLoading: true,
        isEventSponsorFailure: false,
        isEventSponsorSuccess: false,
        deleteEventSponsorViewModel: unloadedDeleteEventSponsorViewModel
      };
    case "RECEIVE_DELETE_EVENT_SPONSOR_DATA":
      return {
        ...state,
        isLoading: false,
        isEventSponsorFailure: false,
        isEventSponsorSuccess: true,
        deleteEventSponsorViewModel: action.deleteEventSponsorViewModel
      };
    case "FAILURE_DELETE_EVENT_SPONSOR_DATA":
      return {
        ...state,
        isLoading: false,
        isEventSponsorFailure: false,
        isEventSponsorSuccess: false,
        deleteEventSponsorViewModel: unloadedDeleteEventSponsorViewModel
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedeventSponsor;
};
