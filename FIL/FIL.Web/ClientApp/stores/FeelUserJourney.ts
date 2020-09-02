import { addTask, fetch } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { ItinerarySearchResponseModel } from "../models/ItinerarySearchResponseModel";
import { IAppThunkAction } from "./";
import FeelUserJourneyRequestViewModel from "../models/FeelUserJourney/FeelUserJourneyRequestViewModel";
import { FeelUserJourneyService } from "../services/FeelUserJourney";
import FeelUserJourneyResponseViewModel from "../models/FeelUserJourney/FeelUserJourneyResponseViewModel";

export interface IFeelUserJourneyProps {
    feelUserJourney: IFeelUserJourneyState;
}

export interface IFeelUserJourneyState {
    isLoading?: boolean;
    errors?: any;
    fetchSuccess?: boolean;
    dynamicSections: FeelUserJourneyResponseViewModel;
    isError: boolean
}

interface IRequestFeelUserJourneyData {
    type: "REQUEST_FEEL_USER_JOURNEY_DATA";
}

interface IReceiveFeelUserJourneyData {
    type: "RECEIVE_FEEL_USER_JOURNEY_DATA";
    dynamicSections: FeelUserJourneyResponseViewModel;
}

interface IFailureFeelUserJourneyData {
    type: "FAILURE_FEEL_USER_JOURNEY_DATA";

}
type KnownAction = IRequestFeelUserJourneyData | IReceiveFeelUserJourneyData | IFailureFeelUserJourneyData;

export const actionCreators = {
    requestDyanamicSectionResponseData: (feelUserJourneyInputModel: FeelUserJourneyRequestViewModel)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "REQUEST_FEEL_USER_JOURNEY_DATA" });
            FeelUserJourneyService.requstDynamicSections(feelUserJourneyInputModel)
                .then((response: FeelUserJourneyResponseViewModel) => {
                    if (response.success) {
                        dispatch({ type: "RECEIVE_FEEL_USER_JOURNEY_DATA", dynamicSections: response, });
                    } else {
                        dispatch({ type: "FAILURE_FEEL_USER_JOURNEY_DATA" });
                    }
                },
                (error) => {
                    dispatch({ type: "FAILURE_FEEL_USER_JOURNEY_DATA" });
                });
        },
};

const emptyFeelUserJourneyData: FeelUserJourneyResponseViewModel = {
    allPlaceTiles: undefined,
    dynamicPlaceSections: undefined,
    geoData: undefined,
    subCategories: [],
    searchValue: "",
    contryPageDetails: undefined,
    success: false
};

const unloadedState: IFeelUserJourneyState = {
    errors: null,
    fetchSuccess: false,
    dynamicSections: emptyFeelUserJourneyData,
    isLoading: false,
    isError: false
};

export const reducer: Reducer<IFeelUserJourneyState> = (state: IFeelUserJourneyState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_FEEL_USER_JOURNEY_DATA":
            return {
                dynamicSections: emptyFeelUserJourneyData,
                fetchSuccess: false,
                isLoading: true,
                isError: false,
            };
        case "RECEIVE_FEEL_USER_JOURNEY_DATA":
            return {
                dynamicSections: action.dynamicSections,
                fetchSuccess: true,
                isLoading: false,
                isError: false,
            }
        case "FAILURE_FEEL_USER_JOURNEY_DATA":
            return {
                dynamicSections: state.dynamicSections,
                fetchSuccess: false,
                isLoading: false,
                isError: true,
            }
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};