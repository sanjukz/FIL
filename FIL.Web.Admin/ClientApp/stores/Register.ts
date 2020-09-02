import { Action, Reducer } from "redux";
import { userService } from "../services/user";
import { IAppThunkAction } from "./";
import { RegistrationFormDataViewModel } from "shared/models/RegistrationFormDataViewModel";
import { RegistrationResponseViewModel } from "shared/models/RegistrationResponseViewModel";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export const REGISTER_REQUEST = "USERS_REGISTER_REQUEST";
export const REGISTER_SUCCESS = "USERS_REGISTER_SUCCESS";
export const REGISTER_FAILURE = "USERS_REGISTER_FAILURE";

export interface IRegisterState {
    requesting: boolean;
    registered: boolean;
    errors: any;
}

const DefaultRegisterState: IRegisterState = {
    requesting: false,
    registered: false,
    errors: {}
};

interface IRegistrationRequestAction {
    type: "USERS_REGISTER_REQUEST";
}

interface IRegistrationSuccesstAction {
    type: "USERS_REGISTER_SUCCESS";

}

interface IRegistrationFailureAction {
    type: "USERS_REGISTER_FAILURE";
}

type KnownAction = IRegistrationRequestAction | IRegistrationSuccesstAction | IRegistrationFailureAction;


export const actionCreators = {
    register: (registerModel: RegistrationFormDataViewModel, callback: (RegistrationResponseViewModel) => void)
        : IAppThunkAction<KnownAction> => async (dispatch, getState) => {

            dispatch({ type: REGISTER_REQUEST });

            userService.register(registerModel)
                .then((response: RegistrationResponseViewModel) => {
                    dispatch({ type: REGISTER_SUCCESS });
                    this.props.history.replace("/login");
                },
                (error) => {
                    dispatch({ type: REGISTER_FAILURE });
                });
        },
};

export const reducer: Reducer<IRegisterState> = (state: IRegisterState, action: KnownAction) => {
    switch (action.type) {
        case REGISTER_REQUEST:
            return { requesting: true, registered: false, errors: {} };
        case REGISTER_SUCCESS:
            return { requesting: false, registered: false, errors: {} };
        case REGISTER_FAILURE:
            return { requesting: false, registered: false, errors: {} };
        default:
            const exhaustiveCheck: never = action;
    }
    return state || DefaultRegisterState;
};
