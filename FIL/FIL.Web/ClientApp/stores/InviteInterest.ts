import { Action, Reducer } from "redux";
import { InviteInterestService } from "../services/InviteInterest";
import { IAppThunkAction } from "./";
import { InviteInterestResponseViewModel } from "../models/InviteInterest/InviteInterestResponseViewModel";
import { InviteInterestRequestViewModel } from  "../models/InviteInterest/InviteInterestRequestViewModel";
import CountryDataViewModel from "../models/Country/CountryDataViewModel";
import { fetch, addTask } from "domain-task";

export const INVITE_INTEREST_REQUEST = "INVITE_INTEREST_REQUEST";
export const INVITE_INTEREST_SUCCESS = "INVITE_INTEREST_SUCCESS";
export const INVITE_INTEREST_FAILURE = "INVITE_INTEREST_FAILURE";
export const ALREADY_INVITE_INTERESTED = "USER_ALREADY_INVITE_INTERESTED";
export const HIDE_ALERT = "HIDE_LETTER_INVITE_INTEREST_ALERT";

export interface IInviteInterestState {
    requesting: boolean;
    inviteInterested: boolean;
    alertVisible: boolean;
    inviteInterestSuccess: boolean;
    inviteInterestFailure: boolean;
    subscriptionExists: boolean;
    countryList: CountryDataViewModel;
    errors: any;
    alertMessage: string;
}
const emptyCountries: CountryDataViewModel = {
    countries: [],
};
const DefaultInviteInterestState: IInviteInterestState = {
    requesting: false,
    inviteInterested: false,
    alertVisible: false,
    subscriptionExists: false,
    errors: {},
    inviteInterestSuccess: false,
    inviteInterestFailure: false,
    countryList: emptyCountries,
    alertMessage: ""
};

interface IRegistrationRequestAction {
    type: "INVITE_INTEREST_REQUEST";
}

interface IRegistrationSuccesstAction {
    type: "INVITE_INTEREST_SUCCESS";
}

interface IRegistrationFailureAction {
    type: "INVITE_INTEREST_FAILURE";
}

interface ISubscribedAlreadyAction {
    type: "USER_ALREADY_INVITE_INTERESTED";
}

interface IHideAlertAction {
    type: "HIDE_LETTER_INVITE_INTEREST_ALERT";
}
interface IRequestGetCountryListAction {
    type: "REGISTER_GET_COUNTRY_LIST_REQUEST";
}

interface IReceiveGetCountryListAction {
    type: "REGISTER_GET_COUNTRY_LIST_SUCCESS";
    countryList: CountryDataViewModel;
}

interface IGetCountryListFailureAction {
    type: "REGISTER_GET_COUNTRY_LIST_FAILURE";
    errors: any;
}
type KnownAction = IRegistrationRequestAction | IRegistrationSuccesstAction |
    IRegistrationFailureAction | ISubscribedAlreadyAction | IHideAlertAction
    | IRequestGetCountryListAction | IReceiveGetCountryListAction | IGetCountryListFailureAction;

export const actionCreators = {
    InviteInterestRequest: (InviteInterestModel: InviteInterestRequestViewModel, callback: (InviteInterestResponseDataViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: INVITE_INTEREST_REQUEST });
            InviteInterestService.sendUserInviteInterest(InviteInterestModel)
                .then((response:any) => {
                    
                    if (response.isUsed ) {
                        dispatch({ type: ALREADY_INVITE_INTERESTED });
                    } else if (response.success) {
                        dispatch({ type: INVITE_INTEREST_SUCCESS });
                    }
                    else if(!response.success)
                    {
                        dispatch({ type: INVITE_INTEREST_FAILURE });
                    }
                    callback(response);
                },
                (error) => {
                    dispatch({ type: INVITE_INTEREST_FAILURE });
                });
        },
    hideAlertAction: (): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
        dispatch({ type: HIDE_ALERT });
    },
    requestCountryData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        
        const fetchTask = fetch(`api/country/all`)
            .then((response) => response.json() as Promise<CountryDataViewModel>)
            .then((data) => {
                
                dispatch({ type: "REGISTER_GET_COUNTRY_LIST_SUCCESS", countryList: data, });
            },
            (error) => {
                dispatch({ type: "REGISTER_GET_COUNTRY_LIST_FAILURE", errors: error });
            },
        );
        addTask(fetchTask);
        dispatch({ type: "REGISTER_GET_COUNTRY_LIST_REQUEST" });
    }
};

export const reducer: Reducer<IInviteInterestState> = (state: IInviteInterestState, action: KnownAction) => {
    let successmessage = "Thank you for registering with feelitlive. If you’re one of the lucky ones, you will soon receive an account activation link with your invite code so you can feelaplace #YourWay";
    let failuremessage = "This is not a valid invite code. Please try again with a valid code that was emailed to you.";
    switch (action.type) {
        case INVITE_INTEREST_REQUEST:
            return {
                countryList: state.countryList, 
                requesting: true, inviteInterested: false, subscriptionExists: false, alertVisible: false, errors: {},
                inviteInterestSuccess : false, inviteInterestFailure:false, alertMessage:"Invite request"
            };
        case INVITE_INTEREST_SUCCESS:
            return {
                countryList: state.countryList, 
                requesting: false, inviteInterested: true, subscriptionExists: false, alertVisible: true, errors: {},
                inviteInterestSuccess : true, inviteInterestFailure:false, alertMessage:successmessage, 
            };
        case INVITE_INTEREST_FAILURE:
            return {
                countryList: state.countryList, 
                requesting: false, inviteInterested: false, subscriptionExists: false, alertVisible: false, errors: {},
                inviteInterestSuccess : false, inviteInterestFailure:false, alertMessage:failuremessage
            };
        case ALREADY_INVITE_INTERESTED:
            return {
                countryList: state.countryList, 
                requesting: false, inviteInterested: false, subscriptionExists: true, alertVisible: true, errors: {},
                inviteInterestSuccess : false, inviteInterestFailure:false, alertMessage:"Already Invited",
            };
        case HIDE_ALERT:
            return {
                countryList: state.countryList, 
                requesting: false, inviteInterested: true, subscriptionExists: false, alertVisible: false, errors: {},
                inviteInterestSuccess : false, inviteInterestFailure:false, alertMessage:"Alert hidden",
            };
        case "REGISTER_GET_COUNTRY_LIST_REQUEST":
            return {
                countryList: state.countryList,                
                requesting: false, inviteInterested: true, subscriptionExists: false, alertVisible: false, errors: {},
                inviteInterestSuccess : false, inviteInterestFailure:false, alertMessage:"Alert hidden",               
            };
        case "REGISTER_GET_COUNTRY_LIST_SUCCESS":
        
            return {
                countryList: action.countryList,                
                requesting: false, inviteInterested: true, subscriptionExists: false, alertVisible: false, errors: {},
                inviteInterestSuccess : false, inviteInterestFailure:false, alertMessage:"Alert hidden",                             
            };
        case "REGISTER_GET_COUNTRY_LIST_FAILURE":
            return {                
                countryList: state.countryList,                
                requesting: false, inviteInterested: true, subscriptionExists: false, alertVisible: false, errors: {},
                inviteInterestSuccess : false, inviteInterestFailure:false, alertMessage:"Alert hidden",                            
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultInviteInterestState;
};