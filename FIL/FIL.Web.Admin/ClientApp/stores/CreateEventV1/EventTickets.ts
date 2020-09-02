import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../index";
import { TicketViewModel, TicketModel } from "../../models/CreateEventV1/TicketViewModel";
import { DeleteTicketViewModel } from "../../models/CreateEventV1/DeleteTicketViewModel";
import { eventTicketsService } from "../../services/CreateEventV1/EventTickets";

export interface IEventTicketsProps {
  EventTickets: IEventTicketsComponentState;
}

export interface IEventTicketsComponentState {
  isLoading?: boolean;
  isEventTicketsSuccess?: boolean;
  isEventTicketsFailure?: boolean;
  isSaveRequest?: boolean;
  tickets?: TicketViewModel;
  deleteTicketViewModel?: DeleteTicketViewModel
}

interface IRequestEventTickets {
  type: "REQUEST_EVENT_TICKETS_DATA";
  isSaveRequest: boolean
}

interface IReceiveEventTickets {
  type: "RECEIVE_EVENT_TICKETS_DATA";
  tickets: TicketViewModel;
}

interface IFailureEventTickets {
  type: "FAILURE_EVENT_TICKETS_DATA";
}

interface IRequestDeleteEventTickets {
  type: "REQUEST_DELETE_EVENT_TICKETS_DATA";
}

interface IReceiveDeleteEventTickets {
  type: "RECEIVE_DELETE_EVENT_TICKETS_DATA";
  deleteTicketViewModel: DeleteTicketViewModel;
}

interface IFailureDeleteEventTickets {
  type: "FAILURE_DELETE_EVENT_TICKETS_DATA";

}
type KnownAction = IRequestEventTickets | IReceiveEventTickets | IFailureEventTickets | IRequestDeleteEventTickets | IReceiveDeleteEventTickets
  | IFailureDeleteEventTickets;

export const actionCreators = {
  requestEventTickets: (eventId: number, ticketCategoryTypeId: number, callback: (EventScheduleViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-tickets/${eventId}?ticketCategoryTypeId=${ticketCategoryTypeId}`)
        .then((response) => response.json() as Promise<TicketViewModel>)
        .then((data: TicketViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_TICKETS_DATA", tickets: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_TICKETS_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_TICKETS_DATA", isSaveRequest: false });
    },
  saveEventTickets: (eventHostInputViewModel: TicketViewModel, callback: (TicketViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_TICKETS_DATA", isSaveRequest: true });
      eventTicketsService.saveEventTicketsRequest(eventHostInputViewModel)
        .then((response: TicketViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_TICKETS_DATA", tickets: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_TICKETS_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_EVENT_TICKETS_DATA" });
          });
    },
  deleteEventTickets: (deleteTicketViewModel: DeleteTicketViewModel, callback: (TicketViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_DELETE_EVENT_TICKETS_DATA" });
      eventTicketsService.deleteEventTicketsRequest(deleteTicketViewModel)
        .then((response: DeleteTicketViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_DELETE_EVENT_TICKETS_DATA", deleteTicketViewModel: response });
          } else {
            dispatch({ type: "FAILURE_DELETE_EVENT_TICKETS_DATA" });
          }
          callback(response);
        },
          (error) => {
            dispatch({ type: "FAILURE_DELETE_EVENT_TICKETS_DATA" });
          });
    },
};

let unloadedTicketViewModel: TicketViewModel = {
  eventDetailId: 0,
  eventId: 0,
  isCreate: false,
  success: false,
  tickets: []
}

let unloadedDeleteTicketViewModel: DeleteTicketViewModel = {
  isTicketSold: false,
  etdAltId: "",
  eventId: 0,
  completedStep: "",
  currentStep: 1,
  ticketLength: 0,
  success: false
}

const unloadedEventTickets: IEventTicketsComponentState = {
  isLoading: false,
  tickets: unloadedTicketViewModel,
  deleteTicketViewModel: unloadedDeleteTicketViewModel,
  isEventTicketsFailure: false,
  isEventTicketsSuccess: false,
  isSaveRequest: false
};

export const reducer: Reducer<IEventTicketsComponentState> = (state: IEventTicketsComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_TICKETS_DATA":
      return {
        ...state,
        isLoading: true,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: false,
        isSaveRequest: action.isSaveRequest,
        tickets: unloadedTicketViewModel
      };
    case "RECEIVE_EVENT_TICKETS_DATA":
      return {
        ...state,
        isLoading: false,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: true,
        isSaveRequest: false,
        tickets: action.tickets
      };
    case "FAILURE_EVENT_TICKETS_DATA":
      return {
        ...state,
        isLoading: false,
        isEventTicketsFailure: true,
        isEventTicketsSuccess: false,
        isSaveRequest: false,
        tickets: unloadedTicketViewModel
      };
    case "REQUEST_DELETE_EVENT_TICKETS_DATA":
      return {
        ...state,
        isLoading: true,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: false,
        deleteTicketViewModel: unloadedDeleteTicketViewModel
      };
    case "RECEIVE_DELETE_EVENT_TICKETS_DATA":
      return {
        ...state,
        isLoading: false,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: true,
        deleteTicketViewModel: action.deleteTicketViewModel
      };
    case "FAILURE_DELETE_EVENT_TICKETS_DATA":
      return {
        ...state,
        isLoading: false,
        isEventTicketsFailure: false,
        isEventTicketsSuccess: false,
        deleteTicketViewModel: unloadedDeleteTicketViewModel
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedEventTickets;
};
