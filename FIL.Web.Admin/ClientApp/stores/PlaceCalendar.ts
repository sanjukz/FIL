import { Action, ActionCreator, Reducer } from "redux";
import PlaceCalendarResponseViewModel from "../models/PlaceCalendar/PlaceCalendarResponseViewModel";
import { PlaceCalendarRequestViewModel } from "../models/PlaceCalendar/PlaceCalendarRequestViewModel";
import { placeCalendarService } from "../services/PlaceCalendar";
import { IAppThunkAction } from "./";

export interface IPlaceCalendarProps {
    placeCalendar: IPlaceCalendarState;
}

export interface IPlaceCalendarState {
    placeCalendarResponse?: PlaceCalendarResponseViewModel;
    isCalendarSaveRequest: boolean;
    isShowSuccessAlert: boolean;
    errors?: any;
}

const emptyPlaceCalendarResponse: PlaceCalendarResponseViewModel = {
    success: false
}

const DefaultLoginState: IPlaceCalendarState = {
    placeCalendarResponse: emptyPlaceCalendarResponse,
    isCalendarSaveRequest: false,
    errors: undefined,
    isShowSuccessAlert: false
};

interface IPlaceCalendarRequestAction {
    type: "PLACE_CALENDAR_REQUEST_ACTION";
}

interface IPlaceCalendarSuccesstAction {
    type: "PLACE_CALENDAR_SUCCESS_ACTION";
    placeCalendarResponse: PlaceCalendarResponseViewModel;
}

interface IPlaceCalendarFailureAction {
    type: "PLACE_CALENDAR_FAILURE_ACTION";
}


type KnownAction = IPlaceCalendarSuccesstAction
    | IPlaceCalendarRequestAction
    | IPlaceCalendarFailureAction;

export const actionCreators = {
    placeCalendarSubmit: (placeCalendarViewModel: PlaceCalendarRequestViewModel, callback: (PlaceCalendarResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "PLACE_CALENDAR_REQUEST_ACTION" });
            placeCalendarService.placeCalendarCreation(placeCalendarViewModel)
                .then((response: PlaceCalendarResponseViewModel) => {
                    dispatch({ type: "PLACE_CALENDAR_SUCCESS_ACTION", placeCalendarResponse: response });
                    localStorage.setItem("eventDetails", JSON.stringify(response));
                    callback(response);
                },
                (error) => {
                    dispatch({ type: "PLACE_CALENDAR_FAILURE_ACTION" });
                });
        },
};

export const reducer: Reducer<IPlaceCalendarState> = (state: IPlaceCalendarState, action: KnownAction) => {
    switch (action.type) {
        case "PLACE_CALENDAR_REQUEST_ACTION":
            return {
                placeCalendarResponse: emptyPlaceCalendarResponse,
                isCalendarSaveRequest: true,
                isShowSuccessAlert: false
            };
        case "PLACE_CALENDAR_SUCCESS_ACTION":
            return {
                placeCalendarResponse: action.placeCalendarResponse,
                isCalendarSaveRequest: false,
                isShowSuccessAlert: true
            };
        case "PLACE_CALENDAR_FAILURE_ACTION":
            return {
                placeCalendarResponse: emptyPlaceCalendarResponse,
                isCalendarSaveRequest: false,
                isShowSuccessAlert: false
            };

        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultLoginState;
};
