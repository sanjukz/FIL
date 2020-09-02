import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import MyFeelResponseViewModel from "../models/MyFeel/MyFeelResponseViewModel";

export interface ISellerProps {
  seller: ISellerEventState;
}

export interface ISellerEventState {
  isFetchSuccess: boolean;
  isLoading: boolean;
  events: MyFeelResponseViewModel;
}

interface IRequestSellerEventData {
  type: "REQUEST_SELLER_EVENT_DATA";
}

interface IResponseSellerEventData {
  type: "RESPONSE_SELLER_EVENT_DATA";
  payload: MyFeelResponseViewModel;
}

interface IErrorSellerEventData {
  type: "ERROR_SELLER_EVENT_DATA"
}

type IKnownAction = IRequestSellerEventData | IResponseSellerEventData | IErrorSellerEventData;

export const actionCreators = {
  requestSellerEventData: (userAltId: string, isApproveModerateScreen?: boolean, isActive?: boolean) => (dispatch, getState) => {
    const fetchTask = fetch(`/api/myfeel/${userAltId}?isApproveModerateScreen=${isApproveModerateScreen}&isActive=${isActive}`)
      .then((response) => response.json() as Promise<MyFeelResponseViewModel>)
      .then((data) => {
        dispatch({
          type: "RESPONSE_SELLER_EVENT_DATA", payload: data
        });
      })
      .catch((error) => {
        dispatch({ type: "ERROR_SELLER_EVENT_DATA" });
      });
    addTask(fetchTask);
    dispatch({ type: "REQUEST_SELLER_EVENT_DATA" });
  }
};

const initialFeelState: MyFeelResponseViewModel = {
  myFeels: [],
  success: false
}

const initialState: ISellerEventState = {
  events: initialFeelState,
  isLoading: false,
  isFetchSuccess: false
}

export const reducer = (state: ISellerEventState, incomingAction: Action) => {
  const action = incomingAction as IKnownAction;
  switch (action.type) {
    case "REQUEST_SELLER_EVENT_DATA":
      return {
        events: state.events,
        isLoading: true,
        isFetchSuccess: false,
      };
    case "RESPONSE_SELLER_EVENT_DATA":
      return {
        events: action.payload,
        isLoading: false,
        isFetchSuccess: true,
      }
    case "ERROR_SELLER_EVENT_DATA":
      return {
        events: state.events,
        isLoading: false,
        isFetchSuccess: false,
      }
  }
  return state || initialState;
}

