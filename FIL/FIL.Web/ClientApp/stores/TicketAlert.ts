import { addTask, fetch } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { IAppThunkAction } from ".";
import { ticketAlertservice } from "../services/ticketAlert";

import { TicketAlertUserMappingResponseViewModel } from "../models/TicketAlert/TicketAlertUserMappingResponseViewModel";
import { TicketAlertRequestViewModel } from "../models/TicketAlert/TicketAlertRequestViewModel";

export const TICKET_ALERT_REGISTER_REQUEST = "TicketAlertRequestAction";
export const TICKET_ALERT_REGISTER_SUCCESS = "TicketAlertSuccessAction";
export const TICKET_ALERT_REGISTER_FAILURE = "TicketAlertFailure";

export interface ITicketAlertProps {
    ticketAlert: ITicketAlertDataState;
}

export interface ITicketAlertDataState {
    requesting?: boolean;
    registered?: boolean;
    fetchCountriesSuccess?: boolean;
    isAlreadySignUp?: boolean;
    success: boolean;
    message?: string;
    errors?: any;
}

const DefaultTicketAlertState: ITicketAlertDataState = {
    requesting: false,
    registered: false,
    success: false,
    fetchCountriesSuccess: false,
    message: "",
    errors: {},
};

interface ITicketAlertRegistrationRequestAction {
    type: "TicketAlertRequestAction";
}

interface ITicketAlertRegistrationSuccesstAction {
    type: "TicketAlertSuccessAction";
    isAlreadySignUp: boolean;
    success: boolean;
    message: string;
}

interface ITicketAlertRegistrationFailureAction {
    type: "TicketAlertFailure";
    isAlreadySignUp: boolean;
    success: boolean;
    message: string;
}

type KnownAction = ITicketAlertRegistrationRequestAction
            | ITicketAlertRegistrationSuccesstAction
                | ITicketAlertRegistrationFailureAction;

export const actionCreators = {
   ticketAlertSignUp: (ticketAlertRegisterModel: TicketAlertRequestViewModel, callback: (TicketAlertUserMappingResponseViewModel) => void)
		: IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: TICKET_ALERT_REGISTER_REQUEST });
            ticketAlertservice.ticketAlertRegister(ticketAlertRegisterModel)
                .then((response: TicketAlertUserMappingResponseViewModel) => {
                    if (response.success && !response.isAlreadySignUp) {
                        dispatch({ type: TICKET_ALERT_REGISTER_SUCCESS, message: "You have successfully signed up for the ticket alert, Thanks!", isAlreadySignUp: false, success: true });
                    } else if(!response.success && response.isAlreadySignUp) {
                        dispatch({ type: TICKET_ALERT_REGISTER_FAILURE, message: "You have already signed up with same email.", isAlreadySignUp: true, success: false });
                    }
					callback(response);
				},
				(error) => {
                    dispatch({ type: TICKET_ALERT_REGISTER_FAILURE, message: "You have already signed up with same email.", isAlreadySignUp: true, success: false });
				});
        },
};

export const reducer: Reducer<ITicketAlertDataState> = (state: ITicketAlertDataState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
         case TICKET_ALERT_REGISTER_REQUEST:
            return { requesting: true, registered: false, message: "", success: false, isAlreadySignUp: false , errors: {} };
        case TICKET_ALERT_REGISTER_SUCCESS:
            return {
                requesting: false, registered: true, message: action.message, success: action.success, isAlreadySignUp: action.isAlreadySignUp, errors: {}
            };
        case TICKET_ALERT_REGISTER_FAILURE:
            return { requesting: false, registered: false,message: action.message, success: false, isAlreadySignUp: true, invalidCredentials: false, errors: {} };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultTicketAlertState;
};
