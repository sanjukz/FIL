import { Action, Reducer } from "redux";
import { FooterService } from "../services/Footer";
import { IAppThunkAction } from "./";
import { NewsLetterSignupFooterDataViewModel } from "../models/Footer/NewsLetterSignupFooterDataViewModel";
import { NewsLetterSignupFooterResponseDataViewModel } from "../models/Footer/NewsLetterSignupFooterResponseDataViewModel";

export const REGISTER_REQUEST = "NEWS_LETTER_REGISTER_REQUEST";
export const REGISTER_SUCCESS = "NEWS_LETTER_REGISTER_SUCCESS";
export const REGISTER_FAILURE = "NEWS_LETTER_REGISTER_FAILURE";
export const ALREADY_SUBSCRIBED = "USER_ALREADY_SUBSCRIBED";
export const HIDE_ALERT = "HIDE_LETTER_REGISTER_ALERT";

export interface INewsLetterState {
    requesting: boolean;
    registered: boolean;
    alertVisible: boolean;
    subscriptionExists: boolean;
    errors: any;
}

const DefaultRegisterState: INewsLetterState = {
    requesting: false,
    registered: false,
    alertVisible: false,
    subscriptionExists: false,
    errors: {}
};

interface IRegistrationRequestAction {
    type: "NEWS_LETTER_REGISTER_REQUEST";
}

interface IRegistrationSuccesstAction {
    type: "NEWS_LETTER_REGISTER_SUCCESS";
}

interface IRegistrationFailureAction {
    type: "NEWS_LETTER_REGISTER_FAILURE";
}

interface ISubscribedAlreadyAction {
    type: "USER_ALREADY_SUBSCRIBED";
}

interface IHideAlertAction {
    type: "HIDE_LETTER_REGISTER_ALERT";
}

type KnownAction = IRegistrationRequestAction | IRegistrationSuccesstAction |
    IRegistrationFailureAction | ISubscribedAlreadyAction | IHideAlertAction;

export const actionCreators = {
    newsLetterSignUp: (newsLetterRegisterModel: NewsLetterSignupFooterDataViewModel, callback: (NewsLetterSignupFooterResponseDataViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: REGISTER_REQUEST });
            FooterService.NewsLetterSignUpRegister(newsLetterRegisterModel)
                .then((response: NewsLetterSignupFooterResponseDataViewModel) => {
                    if (response.isExisting) {
                        dispatch({ type: ALREADY_SUBSCRIBED });
                    } else if (response.success) {
                        dispatch({ type: REGISTER_SUCCESS });
                    }
                    callback(response);
                },
                (error) => {
                    dispatch({ type: REGISTER_FAILURE });
                });
        },
    hideAlertAction: (): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: HIDE_ALERT });
    },
};

export const reducer: Reducer<INewsLetterState> = (state: INewsLetterState, action: KnownAction) => {
    switch (action.type) {
        case REGISTER_REQUEST:
            return {
                requesting: true, registered: false, subscriptionExists: false, alertVisible: false, errors: {}
            };
        case REGISTER_SUCCESS:
            return {
                requesting: false, registered: true, subscriptionExists: false, alertVisible: true, errors: {}
            };
        case REGISTER_FAILURE:
            return {
                requesting: false, registered: false, subscriptionExists: false, alertVisible: false, errors: {}
            };
        case ALREADY_SUBSCRIBED:
            return {
                requesting: false, registered: false, subscriptionExists: true, alertVisible: true, errors: {}
            };
        case HIDE_ALERT:
            return {
                requesting: false, registered: true, subscriptionExists: false, alertVisible: false, errors: {}
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultRegisterState;
};