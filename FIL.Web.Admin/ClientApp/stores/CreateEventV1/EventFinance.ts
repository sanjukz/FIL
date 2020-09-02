import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "../";
import { EventFinanceViewModel } from "../../models/CreateEventV1/EventFinanceViewModal";
import { EventFinanceService } from "../../services/CreateEventV1/EventFinance";

export interface IEventFinanceProps {
  EventFinance: IEventFinanceComponentState;
}

export interface IEventFinanceComponentState {
  isLoading?: boolean;
  isEventFinanceSuccess?: boolean;
  isEventFinanceFailure?: boolean;
  isSaveRequest?: boolean;
  eventFinance?: any;
}

interface IRequestEventFinance {
  type: "REQUEST_EVENT_FINANCE_DATA";
  isSaveRequest: boolean
}

interface IReceiveEventFinance {
  type: "RECEIVE_EVENT_FINANCE_DATA";
  eventFinance: EventFinanceViewModel;
}

interface IFailureEventFinance {
  type: "FAILURE_EVENT_FINANCE_DATA";
}

type KnownAction = IRequestEventFinance | IReceiveEventFinance | IFailureEventFinance;

export const actionCreators = {
  requestEventFinance: (eventId: number, callback: (EventDetailViewModel) => void)
    : IAppThunkAction<KnownAction> => (dispatch, getState) => {
      const fetchTask = fetch(`api/get/event-finance/${eventId}`)
        .then((response) => response.json() as Promise<EventFinanceViewModel>)
        .then((data: EventFinanceViewModel) => {
          if (data.success) {
            dispatch({ type: "RECEIVE_EVENT_FINANCE_DATA", eventFinance: data });
          } else {
            dispatch({ type: "FAILURE_EVENT_FINANCE_DATA" });
          }
          callback(data);
        });

      addTask(fetchTask);
      dispatch({ type: "REQUEST_EVENT_FINANCE_DATA", isSaveRequest: false });
    },
  saveEventFinance: (eventFinanceDataModel: EventFinanceViewModel, callback: (EventFinanceViewModel) => void)
    : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
      dispatch({ type: "REQUEST_EVENT_FINANCE_DATA", isSaveRequest: true });
      EventFinanceService.saveEventFinanceRequest(eventFinanceDataModel)
        .then((response: EventFinanceViewModel) => {
          if (response.success) {
            dispatch({ type: "RECEIVE_EVENT_FINANCE_DATA", eventFinance: response });
          } else {
            dispatch({ type: "FAILURE_EVENT_FINANCE_DATA" });
          }
          callback(response);
        })
    },
};

let unloadedEventFinanceDetailModel: EventFinanceViewModel = {
  stripeAccount: 0,
  stripeConnectAccountId: "",
  currencyType: undefined,
  eventFinanceDetailModel: undefined,
  eventId: 0,
  isDraft: false,
  isValidLink: false,
  success: false,
  eventAltId: "",
  isoAlphaTwoCode: ""
}

const unloadedEventFinance: IEventFinanceComponentState = {
  isLoading: false,
  eventFinance: unloadedEventFinanceDetailModel,
  isEventFinanceFailure: false,
  isEventFinanceSuccess: false,
  isSaveRequest: false,
};

export const reducer: Reducer<IEventFinanceComponentState> = (state: IEventFinanceComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_EVENT_FINANCE_DATA":
      return {
        isLoading: true,
        isEventFinanceFailure: false,
        isEventFinanceSuccess: false,
        isSaveRequest: action.isSaveRequest,
        eventFinance: unloadedEventFinance
      };
    case "RECEIVE_EVENT_FINANCE_DATA":
      return {
        isLoading: false,
        isEventFinanceFailure: false,
        isEventFinanceSuccess: true,
        isSaveRequest: false,
        eventFinance: action.eventFinance
      };
    case "FAILURE_EVENT_FINANCE_DATA":
      return {
        isLoading: false,
        isEventFinanceFailure: true,
        isEventFinanceSuccess: false,
        isSaveRequest: false,
        eventFinance: unloadedEventFinance
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedEventFinance;
};
