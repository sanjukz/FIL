import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import { FeatureResponseViewModel } from "../models/FeatureResponseViewModel";

export interface IAuthedNavMenuFeatureState {
    featureFetchSuccess: boolean;
    feature: FeatureResponseViewModel;
    errors: any;
}

interface IRequestRoleFeatureAction {
    type: "REQUEST_ROLE_FEATURE";
}

interface IReceiveRoleFeatureAction {
    type: "RECEIVE_ROLE_FEATURE";
    featureList: FeatureResponseViewModel;
}

type KnownAction = IRequestRoleFeatureAction | IReceiveRoleFeatureAction;

export const actionCreators = {
    getRoleFeatures: (altId): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        const fetchTask = fetch(`api/feature/` + altId)
            .then((response) => response.json() as Promise<FeatureResponseViewModel>)
            .then((data) => {
                localStorage.setItem("featureList", JSON.stringify(data));
                dispatch({ type: "RECEIVE_ROLE_FEATURE", featureList: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_ROLE_FEATURE" });
    },
};

const emptyFeature: FeatureResponseViewModel = {
    feature: []
};

const unloadedState: IAuthedNavMenuFeatureState = {
    featureFetchSuccess: false,
    feature : emptyFeature,
     errors: null
}

export const reducer: Reducer<IAuthedNavMenuFeatureState> = (state: IAuthedNavMenuFeatureState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_ROLE_FEATURE":
            return {
                featureFetchSuccess: false,
                feature: state.feature,
                errors: {}
            };
        case "RECEIVE_ROLE_FEATURE":
            return {
                featureFetchSuccess: true,
                feature: action.featureList,
                errors: {}
            };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
