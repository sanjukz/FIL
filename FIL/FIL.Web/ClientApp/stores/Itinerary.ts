import { addTask, fetch } from "domain-task";
import { Action, ActionCreator, Reducer } from "redux";
import { ItinerarySearchResponseModel } from "../models/ItinerarySearchResponseModel";
import { IAppThunkAction } from "./";
import responseModel from "../models/ItinenaryDataResonseModel";
import { itineraryService } from "../services/Itinerary";
import ItineraryRequestViewModel from "../models/Itinerary/ItineraryRequestViewModel";
import ItineraryTicketResponseViewModel, { TicketResponse } from "../models/Itinerary/ItineraryTicketResponseViewModel";
import { MasterBudgetRangeResponseModel } from "../models/MasterBudgetRange/MasterBudgetRangeResponseModel";
import ItineraryBoardInputViewModel from "../models/Itinerary/ItineraryBoardInputViewModel";
import ItineraryBoardResponseViewModel from "../models/Itinerary/ItineraryBoardResponseViewModel";

; export interface ICategoryProps {
    itinerary: ItineraryComponentState;
}
export interface ItineraryComponentState {
    isLoading?: boolean;
    errors?: any;
    fetchSearchSuccess?: boolean;
    result?: ItinerarySearchResponseModel;
    itineraryResult?: responseModel[][];
    itineraryTicketResult?: ItineraryTicketResponseViewModel;
    masterBudgetRange: MasterBudgetRangeResponseModel;
    fetchItinerarySuccess?: boolean;
    isItineraryTicketRequest?: boolean;
    itineraryBoardRequest: boolean;
    itineraryBoardResponse: ItineraryBoardResponseViewModel;
    isError: boolean;
}

interface IRequestItinerarySearchData {
    type: "REQUEST_ITINERARY_SEARCH_DATA";
}

interface IReceiveItinerarySearchData {
    type: "RECEIVE_ITINERARY_SEARCH_DATA";
    result: ItinerarySearchResponseModel;
}

interface IRequestMasterBudgetRangeData {
    type: "REQUEST_MASTER_BUDGET_DATA";
}

interface IReceiveMasterBudgetRangeData {
    type: "RECEIVE_MASTER_BUDGET_DATA";
    masterBudgetRange: MasterBudgetRangeResponseModel;
}

interface IRequestItineraryTicketData {
    type: "REQUEST_ITINERARY_TICKET_DATA";
}

interface IReceiveItineraryTicketData {
    type: "RECEIVE_ITINERARY_TICKET_DATA";
    itineraryTicketResult: ItineraryTicketResponseViewModel;
}

interface IRequestItineraryResultData {
    type: "REQUEST_ITINERARY_RESULT_DATA";
}

interface IReceiveItineraryResultData {
    type: "RECEIVE_ITINERARY_RESULT_DATA";
    itineraryResult: responseModel[][];
}
interface IReceiveItineraryDataFailureAction {
    type: "RECEIVE_ITINERARY_RESULT_DATA_FAILURE";
    errors: any;
}

interface IRequestItineraryBoardData {
    type: "REQUEST_ITINERARY_BOARD_DATA";
}

interface IReceiveItineraryBoardData {
    type: "RECEIVE_ITINERARY_BOARD_DATA";
    itineraryBoardResponse: ItineraryBoardResponseViewModel;
}
interface IReceiveItineraryBoardDataFailureAction {
    type: "RECEIVE_ITINERARY_BOARD_DATA_FAILURE";
    errors: any;
}

type KnownAction = IRequestItinerarySearchData |
    IReceiveItinerarySearchData | IReceiveItineraryResultData | IRequestItineraryResultData |
    IReceiveItineraryDataFailureAction | IRequestItineraryTicketData | IReceiveItineraryTicketData
    | IRequestMasterBudgetRangeData | IReceiveMasterBudgetRangeData | IRequestItineraryBoardData |
    IReceiveItineraryBoardData | IReceiveItineraryBoardDataFailureAction;

export const actionCreators = {
    requestItinerarySearchData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/itinerary/cities`)
            .then((response) => response.json() as Promise<ItinerarySearchResponseModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_ITINERARY_SEARCH_DATA", result: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_ITINERARY_SEARCH_DATA" });
    },
    requestMasterBudgetData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/all/budgetRange`)
            .then((response) => response.json() as Promise<MasterBudgetRangeResponseModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_MASTER_BUDGET_DATA", masterBudgetRange: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_MASTER_BUDGET_DATA" });
    },
    requestItineraryTicketData: (search: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/Itinerary/ItineraryTicketData?` + search)
            .then((response) => response.json() as Promise<ItineraryTicketResponseViewModel>)
            .then((data) => {
                dispatch({ type: "RECEIVE_ITINERARY_TICKET_DATA", itineraryTicketResult: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_ITINERARY_TICKET_DATA" });
    },
    requestItinerarayResponseData: (itineraryInput: ItineraryRequestViewModel)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "REQUEST_ITINERARY_RESULT_DATA" });
            itineraryService.requstItinerary(itineraryInput)
                .then((response: responseModel[][]) => {
                    dispatch({ type: "RECEIVE_ITINERARY_RESULT_DATA", itineraryResult: response, });
                },
                    (error) => {
                        dispatch({ type: "RECEIVE_ITINERARY_RESULT_DATA_FAILURE", errors: error });
                    });
        },
    requestItinerarayBoardData: (itineraryBoardInput: ItineraryBoardInputViewModel, callback: (UserCheckoutResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {
            dispatch({ type: "REQUEST_ITINERARY_BOARD_DATA" });
            itineraryService.requstItineraryBoard(itineraryBoardInput)
                .then((response: ItineraryBoardResponseViewModel) => {
                    dispatch({ type: "RECEIVE_ITINERARY_BOARD_DATA", itineraryBoardResponse: response, });
                    callback(response);
                },
                    (error) => {
                        dispatch({ type: "RECEIVE_ITINERARY_BOARD_DATA_FAILURE", errors: error });
                    });
        },
};

const activateUserPageData: ItinerarySearchResponseModel = {
    itinerarySerchData: null
};

const emptyItineraryTicketData: ItineraryTicketResponseViewModel = {
    itineraryTicketDetails: []
};

const emptyMasterBudgetRangeData: MasterBudgetRangeResponseModel = {
    currencyTypes: [],
    masterBudgetRanges: []
};

const emptyItineraryBoardResponse: ItineraryBoardResponseViewModel = {
    itineraryBoardData: [[]],
    success: false,
    isSourceCountZero: false,
    isValidDandD: false,
    isTargetDateExceed: false
};

const unloadedState: ItineraryComponentState = {
    errors: null,
    fetchSearchSuccess: false,
    result: activateUserPageData,
    isLoading: false,
    fetchItinerarySuccess: false,
    itineraryResult: [[]],
    isError: false,
    itineraryTicketResult: emptyItineraryTicketData,
    isItineraryTicketRequest: false,
    masterBudgetRange: emptyMasterBudgetRangeData,
    itineraryBoardResponse: emptyItineraryBoardResponse,
    itineraryBoardRequest: false
};

export const reducer: Reducer<ItineraryComponentState> = (state: ItineraryComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_ITINERARY_SEARCH_DATA":
            return {
                result: state.result,
                fetchSearchSuccess: false,
                isLoading: true,
                isError: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: state.isItineraryTicketRequest,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: emptyItineraryBoardResponse
            };
        case "RECEIVE_ITINERARY_SEARCH_DATA":
            return {
                result: action.result,
                fetchSearchSuccess: true,
                isLoading: false,
                isError: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: state.isItineraryTicketRequest,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: emptyItineraryBoardResponse
            };
        case "REQUEST_ITINERARY_TICKET_DATA":
            return {
                result: state.result,
                fetchSearchSuccess: state.fetchSearchSuccess,
                isLoading: true,
                isError: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: true,
                fetchItinerarySuccess: state.fetchItinerarySuccess,
                itineraryResult: state.itineraryResult,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: emptyItineraryBoardResponse
            };
        case "RECEIVE_ITINERARY_TICKET_DATA":
            return {
                result: state.result,
                fetchSearchSuccess: state.fetchSearchSuccess,
                isLoading: false,
                isError: false,
                fetchItinerarySuccess: state.fetchItinerarySuccess,
                itineraryTicketResult: action.itineraryTicketResult,
                itineraryResult: state.itineraryResult,
                isItineraryTicketRequest: false,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: state.itineraryBoardResponse
            };
        case "REQUEST_ITINERARY_RESULT_DATA":
            return {
                itineraryResult: state.itineraryResult,
                fetchItinerarySuccess: false,
                fetchSearchSuccess: true,
                isLoading: true,
                result: state.result,
                isError: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: false,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: emptyItineraryBoardResponse
            };
        case "RECEIVE_ITINERARY_RESULT_DATA":
            return {
                itineraryResult: action.itineraryResult,
                fetchItinerarySuccess: true,
                fetchSearchSuccess: true,
                isLoading: false,
                result: state.result,
                isError: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: state.isItineraryTicketRequest,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: emptyItineraryBoardResponse
            };
        case "RECEIVE_ITINERARY_RESULT_DATA_FAILURE":
            return {
                errors: action.errors,
                isError: true,
                itineraryResult: state.itineraryResult,
                fetchItinerarySuccess: state.fetchItinerarySuccess,
                fetchSearchSuccess: true,
                isLoading: false,
                result: state.result,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: state.isItineraryTicketRequest,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: emptyItineraryBoardResponse
            };
        case "REQUEST_ITINERARY_BOARD_DATA":
            return {
                itineraryResult: state.itineraryResult,
                fetchItinerarySuccess: false,
                fetchSearchSuccess: true,
                isLoading: true,
                result: state.result,
                isError: false,
                itineraryBoardRequest: true,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: false,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardResponse: emptyItineraryBoardResponse
            };
        case "RECEIVE_ITINERARY_BOARD_DATA":
            return {
                itineraryResult: state.itineraryResult,
                fetchItinerarySuccess: true,
                fetchSearchSuccess: true,
                isLoading: false,
                result: state.result,
                isError: false,
                itineraryBoardRequest: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: state.isItineraryTicketRequest,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardResponse: action.itineraryBoardResponse
            };
        case "RECEIVE_ITINERARY_BOARD_DATA_FAILURE":
            return {
                errors: action.errors,
                isError: true,
                itineraryResult: state.itineraryResult,
                fetchItinerarySuccess: state.fetchItinerarySuccess,
                fetchSearchSuccess: true,
                isLoading: false,
                result: state.result,
                itineraryBoardRequest: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: state.isItineraryTicketRequest,
                masterBudgetRange: state.masterBudgetRange,
                itineraryBoardResponse: state.itineraryBoardResponse
            };
        case "REQUEST_MASTER_BUDGET_DATA":
            return {
                itineraryResult: state.itineraryResult,
                fetchItinerarySuccess: false,
                fetchSearchSuccess: true,
                isLoading: true,
                result: state.result,
                isError: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: false,
                masterBudgetRange: emptyMasterBudgetRangeData,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: state.itineraryBoardResponse
            };
        case "RECEIVE_MASTER_BUDGET_DATA":
            return {
                itineraryResult: state.itineraryResult,
                fetchItinerarySuccess: true,
                fetchSearchSuccess: true,
                isLoading: false,
                result: state.result,
                isError: false,
                itineraryTicketResult: state.itineraryTicketResult,
                isItineraryTicketRequest: state.isItineraryTicketRequest,
                masterBudgetRange: action.masterBudgetRange,
                itineraryBoardRequest: state.itineraryBoardRequest,
                itineraryBoardResponse: state.itineraryBoardResponse
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};