import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from './';
import { FeelSiteDynamicLayoutSectionViewModel, PageView, SectionView } from '../models/FeelSiteDynamicLayoutViewModel';

//Action type constants
export const REQUEST_SECTION_DATA = 'REQUEST_SECTION_DATA';
export const RECEIVE_SECTION_DATA = 'RECEIVE_SECTION_DATA';
export const ERROR_SECTION_DATA = 'ERROR_SECTION_DATA';

//interfaces
export interface IFeelSiteDynamicLayoutState {
    isLoading: boolean;
    pageMetaData: FeelSiteDynamicLayoutSectionViewModel;
    fetchSuccess: boolean;
}

export interface IFeelSiteDynamicLayoutProps {
    pageMetaData: IFeelSiteDynamicLayoutState;
}

interface IREQUEST_SECTION_DATA {
    type: 'REQUEST_SECTION_DATA';
}

interface IRECEIVE_SECTION_DATA {
    type: 'RECEIVE_SECTION_DATA';
    payload: FeelSiteDynamicLayoutSectionViewModel
}

interface IERROR_SECTION_DATA {
    type: 'ERROR_SECTION_DATA';
}

const initialSectionViewModel: FeelSiteDynamicLayoutSectionViewModel = {
    pageName: null,
    sections: []
};

type KnownAction =
    | IREQUEST_SECTION_DATA
    | IRECEIVE_SECTION_DATA
    | IERROR_SECTION_DATA;

export const actionCreators = {
    //action for getting page metadata
    getAllSections: (pageId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/section/${pageId}`)
            .then((response) => response.json() as Promise<FeelSiteDynamicLayoutSectionViewModel>)
            .then((data) => {
                dispatch({ type: RECEIVE_SECTION_DATA, payload: data });
            })
            .catch((error) => {
                dispatch({ type: ERROR_SECTION_DATA })
            });
        addTask(fetchTask);
        dispatch({ type: REQUEST_SECTION_DATA });
    }
};

//initial state for this store
const initialState: IFeelSiteDynamicLayoutState = {
    isLoading: false,
    pageMetaData: initialSectionViewModel,
    fetchSuccess: false
};

//reducer functions
export const reducer: Reducer<IFeelSiteDynamicLayoutState> = (
    state: IFeelSiteDynamicLayoutState = initialState,
    action: KnownAction) => {

    switch (action.type) {
        case REQUEST_SECTION_DATA:
            return {
                ...state,
                isLoading: true
            };
        case RECEIVE_SECTION_DATA:
            return {
                isLoading: false,
                pageMetaData: action.payload,
                fetchSuccess: true
            };

        case ERROR_SECTION_DATA:
            return {
                ...state,
                isLoading: false,
                fetchSuccess: true
            };
        default:
            return {
                ...state
            }
    }
};
