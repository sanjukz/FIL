import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../index";
import { EventHostsViewModel, Host } from "../../models/CreateEventV1/EventHostsViewModel";
import { DeleteEventHostViewModel } from "../../models/CreateEventV1/DeleteEventHostViewModel";
import { eventHostsService } from "../../services/CreateEventV1/EventHosts";

export interface IEventHostsProps {
  EventHosts: IEventHostsComponentState;
}

export interface IEventHostsComponentState {
  isLoading?: boolean;
  isEventHostSuccess?: boolean;
  isEventHostFailure?: boolean;
  isSaveRequest?: boolean;
  eventHosts?: EventHostsViewModel;
  deleteEventHostViewModel?: DeleteEventHostViewModel
}

interface IRequestEventHost {
  type: "REQUEST_EVENT_HOST_DATA";
  isSaveRequest: boolean
}

interface IReceiveEventHost {
  type: "RECEIVE_EVENT_HOST_DATA";
  eventHosts: EventHostsViewModel;
}

interface IFailureEventHost {
  type: "FAILURE_EVENT_HOST_DATA";
}

interface IRequestDeleteEventHosts {
  type: "REQUEST_DELETE_EVENT_HOSTS_DATA";
}

interface IReceiveDeleteEventHosts {
  type: "RECEIVE_DELETE_EVENT_HOSTS_DATA";
  deleteEventHostViewModel: DeleteEventHostViewModel;
}

interface IFailureDeleteEventHosts {
  type: "FAILURE_DELETE_EVENT_HOSTS_DATA";
}

type KnownAction = IRequestEventHost | IReceiveEventHost | IFailureEventHost | IRequestDeleteEventHosts | IReceiveDeleteEventHosts | IFailureDeleteEventHosts;

export const actionCreators = {
  requestEventHosts: (eventId: number, callback: (EventHostsViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-hosts/${eventId}`)
        .then((response) => response.json() as Promise<EventHostsViewModel>)
        .then((data: EventHostsViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_HOST_DATA", eventHosts: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_HOST_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_HOST_DATA", isSaveRequest: false });
    },
  saveEventHosts: (eventHostInputViewModel: EventHostsViewModel, callback: (EventHostsViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_HOST_DATA", isSaveRequest: true });
      eventHostsService.saveEventHostsRequest(eventHostInputViewModel)
        .then((response: EventHostsViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_HOST_DATA", eventHosts: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_HOST_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_HOST_DATA" });
          });
    },
  deleteHost: (eventHostMappingId: number, currentStep: number, ticketLength: number, callback: (DeleteEventHostViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/delete/event-host/${eventHostMappingId}?currentStep=${currentStep}&ticketLength=${ticketLength}`, {
        method: 'DELETE',
      })
        .then((response) => response.json() as Promise<DeleteEventHostViewModel>)
        .then((data: DeleteEventHostViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_DELETE_EVENT_HOSTS_DATA", deleteEventHostViewModel: data });
          } else {
            dispatch({ type: "FAILURE_DELETE_EVENT_HOSTS_DATA" });
          }
          callback(data);
        });
      addTask(fetchTask);
      dispatch({ type: "REQUEST_DELETE_EVENT_HOSTS_DATA" });
    },
};

let unloadedEventHostViewModel: EventHostsViewModel = {
  eventHostMapping: [],
  success: false,
  eventId: 0,
  isDraft: false,
  isValidLink: false
}

let unloadedDeleteEventHostViewModel: DeleteEventHostViewModel = {
  isHostStreamLinkCreated: false,
  success: false
}

const unloadedeventHosts: IEventHostsComponentState = {
  isLoading: false,
  eventHosts: unloadedEventHostViewModel,
  deleteEventHostViewModel: unloadedDeleteEventHostViewModel,
  isEventHostFailure: false,
  isEventHostSuccess: false,
  isSaveRequest: false
};

export const reducer: Reducer<IEventHostsComponentState> = (state: IEventHostsComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_HOST_DATA":
      return {
        ...state,
        isLoading: true,
        isEventHostFailure: false,
        isEventHostSuccess: false,
        isSaveRequest: action.isSaveRequest,
        eventHosts: unloadedEventHostViewModel
      };
    case "RECEIVE_EVENT_HOST_DATA":
      return {
        ...state,
        isLoading: false,
        isEventHostFailure: false,
        isEventHostSuccess: true,
        isSaveRequest: false,
        eventHosts: action.eventHosts
      };
    case "FAILURE_EVENT_HOST_DATA":
      return {
        ...state,
        isLoading: false,
        isEventHostFailure: true,
        isEventHostSuccess: false,
        isSaveRequest: false,
        eventHosts: unloadedEventHostViewModel
      };
    case "REQUEST_DELETE_EVENT_HOSTS_DATA":
      return {
        ...state,
        isLoading: true,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: false,
        deleteTicketViewModel: unloadedDeleteEventHostViewModel
      };
    case "RECEIVE_DELETE_EVENT_HOSTS_DATA":
      return {
        ...state,
        isLoading: false,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: true,
        deleteEventHostViewModel: action.deleteEventHostViewModel
      };
    case "FAILURE_DELETE_EVENT_HOSTS_DATA":
      return {
        ...state,
        isLoading: false,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: false,
        deleteTicketViewModel: unloadedDeleteEventHostViewModel
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedeventHosts;
};
