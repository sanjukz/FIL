import { fetch, addTask } from "domain-task";
import { Action, Reducer, ActionCreator } from "redux";
import { IAppThunkAction } from "./";
import AddressResponseViewModel from "../models/Account/AddressResponseViewModel";
import { AlertDataViewModel } from "../models/Alert/AlertDataViewModel";
import CardResponseViewModel from "../models/Account/CardResponseViewModel";
import UserProfileResponseViewModel from "../models/Account/UserProfileResponseViewModel";
import { ChangePasswordFormDataViewModel } from "../models/Account/ChangePasswordFormDataViewModel";
import { ChangePasswordResponseViewModel } from "../models/Account/ChangePasswordResponseViewModel";
import { DeleteAddressDataViewModel } from "../models/Account/DeleteAddressDataViewModel";
import { DeleteAddressResponseViewModel } from "../models/Account/DeleteAddressResponseViewModel";
import { DeleteCardDataViewModel } from "../models/Account/DeleteCardDataViewModel";
import { DeleteCardResponseViewModel } from "../models/Account/DeleteCardResponseViewModel";
import { GetAddressesDataViewModel } from "../models/Account/GetAddressesDataViewModel";
import { GetCardListDataViewModel } from "../models/Account/GetCardListDataViewModel";
import { SaveAddressFormDataViewModel } from "../models/Account/SaveAddressFormDataViewModel";
import { SaveAddressResponseViewModel } from "../models/Account/SaveAddressResponseViewModel";
import { SaveCardFormDataViewModel } from "../models/Account/SaveCardFormDataViewModel";
import { SaveCardResponseViewModel } from "../models/Account/SaveCardResponseViewModel";
import { SetDefaultAddressFormDataViewModel } from "../models/Account/SetDefaultAddressFormDataViewModel";
import { SetDefaultAddressResponseViewModel } from "../models/Account/SetDefaultAddressResponseViewModel";
import { UserOrderRespnseViewModel } from "../models/Account/UserOrderRespnseViewModel";
import CountryDataViewModel, { CountryModel } from "../models/Country/CountryDataViewModel";
import { accountService } from "../services/account";
import { SaveGuestFormDataModel } from '../models/Account/SaveGuestFormDataModel';
import { MobileExistModel } from "../models/Account/MobileExistModel";
import { SaveGuestFormResponseModel } from '../models/Account/SaveGuestFormResponseModel';
import { SaveGuestResponseViewModel } from '../models/Account/SaveGuestResponseViewModel';
import { NotificationModel } from "../models/Account/NotificationModel";

export interface IUserAccountProps {
	account: IUserAccountState;
}

export interface IUserAccountState {
	isLoadingUserProfile: boolean;
	isLoadingUserOrderList: boolean;
	isLoadingUserAddressList: boolean;
	isLoadingUserCardList: boolean;
	isLoadingUserGuestList: boolean;
	saveUserCardSuccess: boolean;
	saveUserAddressSuccess: boolean;
	saveGuestDetailSuccess: boolean;
	isLoadingCountryList: boolean;
	updateUserNameSuccess: boolean;
	updateUserDetailsSuccess: boolean;
	updateGuestDetailSuccess: boolean;
	deleteAddressSuccess: boolean;
	deleteGuestItemSuccess: boolean;
	deleteCardSuccess: boolean;
	setDefaultAddressSuccess: boolean;
	userAddresses?: AddressResponseViewModel;
	userCards?: CardResponseViewModel;
	userProfile?: UserProfileResponseViewModel;
	userOrders?: UserOrderRespnseViewModel;
	countryList?: CountryDataViewModel;
	guestList?: SaveGuestFormResponseModel;
	isLoading?: boolean;
	mobileDetails?: MobileExistModel;
	loginAndSecurityUser?: ChangePasswordResponseViewModel
}


const emptyUserAddress: AddressResponseViewModel = {
	userAddresses: [],
};

const emptyUserCards: CardResponseViewModel = {
	userCards: [],
};

const userProfile: UserProfileResponseViewModel = {
	userProfile: {
		id: null,
		altId: '',
		userName: '',
		firstName: '',
		lastName: '',
		email: '',
		phoneCode: null,
		phoneNumber: null,
		dob: '',
		gender: ''
	}
};


const emptyUserOrders: UserOrderRespnseViewModel = {
	transaction: [],
	transactionDetail: [],
	eventTicketAttribute: [],
	eventTicketDetail: [],
	ticketCategory: [],
	transactionPaymentDetail: [],
	event: [],
	currenctType: []
};

const emptyCountries: CountryDataViewModel = {
	countries: [],
};

const emptyGuestDetails: SaveGuestFormResponseModel = {
	guestUserDetails: []
}

const DefaultRegisterState: IUserAccountState = {
	userAddresses: emptyUserAddress,
	userCards: emptyUserCards,
	userProfile: userProfile,
	userOrders: emptyUserOrders,
	countryList: emptyCountries,
	guestList: emptyGuestDetails,
	isLoadingUserProfile: false,
	isLoadingCountryList: false,
	isLoadingUserOrderList: false,
	isLoadingUserAddressList: false,
	isLoadingUserCardList: false,
	isLoadingUserGuestList: false,
	saveUserCardSuccess: false,
	saveUserAddressSuccess: false,
	updateUserNameSuccess: false,
	updateUserDetailsSuccess: false,
	updateGuestDetailSuccess: false,
	deleteAddressSuccess: false,
	deleteCardSuccess: false,
	setDefaultAddressSuccess: false,
	saveGuestDetailSuccess: false,
	deleteGuestItemSuccess: false,
	isLoading: false,
	mobileDetails: null,
	loginAndSecurityUser: null
};
/**
 * Action for updating user details 
 */

interface IRequestUpdateUserAction {
	type: "USER_DETAILS_UPDATE_REQUEST";
}

interface IReceiveUpdateUserAction {
	type: "USER_DETAILS_UPDATE_SUCCESS";
	userProfile: UserProfileResponseViewModel;
}

/**
 * Actions for updating password
 */

interface IRequestChangePasswordAction {
	type: "USER_PASSWORD_UPDATE_REQUEST";
}

interface IReceiveChangePasswordAction {
	type: "USER_PASSWORD_UPDATE_SUCCESS";
	loginAndSecurityUser: ChangePasswordResponseViewModel;
}

interface IRequestNotificationUpdateAction {
	type: "USER_NOTIFICATION_UPDATE_REQUEST";
}

interface IReceiveNotificationUpdateAction {
	type: "USER_NOTIFICATION_UPDATE_SUCCESS";
	user: NotificationModel;
}

/**
 * Action for deleting user address
 */

interface IRequestDeleteAddressAction {
	type: "USER_ADDRESS_DELETE_REQUEST";
}

interface IReceiveDeleteAddressAction {
	type: "USER_ADDRESS_DELETE_SUCCESS";
	alertMessage: AlertDataViewModel;
}

/**
 * Actions for deleting card
 */

interface IRequestDeleteCardAction {
	type: "USER_CARD_DELETE_REQUEST";
}

interface IReceiveDeleteCardAction {
	type: "USER_CARD_DELETE_SUCCESS";
	alertMessage: AlertDataViewModel;
}

/**
 * Actions for getting address list
 */

interface IRequestGetAddressListAction {
	type: "USER_ADDRESS_LIST_GET_REQUEST";
}

interface IReceiveGetAddressListAction {
	type: "USER_ADDRESS_LIST_GET_SUCCESS";
	userAddresses: AddressResponseViewModel;
}

/**
 * Actions for getting user card lists
 */

interface IRequestGetCardListAction {
	type: "USER_CARD_LIST_GET_REQUEST";
}

interface IReceiveGetCardListAction {
	type: "USER_CARD_LIST_GET_SUCCESS";
	userCards: CardResponseViewModel;
}

/**
 *Actions for deleting guest lists 
 */
interface IRequestDeleteGuestListAction {
	type: "USER_GUEST_LIST_DELETE_REQUEST";
}

interface IReceiveDeleteGuestListAction {
	type: "USER_GUEST_LIST_DELETE_SUCCESS";
	deletedUser: boolean;
}

/**
 * Actions for saving user address
 */

interface IRequestSaveAddressAction {
	type: "USER_ADDRESS_SAVE_REQUEST";
}

interface IReceiveSaveAddressAction {
	type: "USER_ADDRESS_SAVE_SUCCESS";
	alertMessage: AlertDataViewModel;
}

/**
 * Actions for setting default address 
 */

interface IRequestSetDefaultAddressAction {
	type: "USER_DEFAULT_ADDRESS_SET_REQUEST";
}

interface IReceiveSetDefaultAddressAction {
	type: "USER_DEFAULT_ADDRESS_SET_SUCCESS";
	alertMessage: AlertDataViewModel;
}

/**
 * Actions for saving user credit cards
 */

interface IRequestSaveCardAction {
	type: "USER_CARD_SAVE_REQUEST";
}

interface IReceiveSaveCardAction {
	type: "USER_CARD_SAVE_SUCCESS";
	alertMessage: AlertDataViewModel;
}

/**
 * Actions for getting user order data
 */

interface IRequestUserOrderData {
	type: "USER_ORDER_LIST_GET_REQUEST";
}

interface IReceiveUserOrderData {
	type: "USER_ORDER_LIST_GET_SUCCESS";
	userOrders: UserOrderRespnseViewModel;
}

/**
 * Actions for getting country list
 */

interface IRequestGetCountryListAction {
	type: "COUNTRY_LIST_GET_REQUEST";
}

interface IReceiveGetCountryListAction {
	type: "COUNTRY_LIST_GET_SUCCESS";
	countryList: CountryDataViewModel;
}

/**
 * Actions for saving user guest details
 */
interface IRequestSaveGuestDetailsAction {
	type: "USER_GUEST_DETAILS_SAVE_REQUEST";
}

interface IReceiveSaveGuestDetailsAction {
	type: "USER_GUEST_DETAILS_SAVE_SUCCESS";
	guestDetails: boolean;
}

/**
 * Actions for getting guest list
 */

interface IRequestGetGuestListAction {
	type: "USER_GUEST_LIST_GET_REQUEST";
}

interface IReceiveGetGuestListAction {
	type: "USER_GUEST_LIST_GET_SUCCESS";
	guestList: SaveGuestFormResponseModel;
}

interface IRequestUpdateGuestDetailsAction {
	type: "USER_GUEST_DETAILS_UPDATE_REQUEST";
}

interface IRecieveUpdateGuestDetailsAction {
	type: "USER_GUEST_DETAILS_UPDATE_SUCCESS";
	guestDetails: boolean;
}
interface IRequestMobileDetailsAction {
	type: "USER_MOBILE_DETAILS_REQUEST";
}

interface IRecieveMobileDetailsAction {
	type: "USER_MOBILE_DETAILS_SUCCESS";
	mobileDetails: MobileExistModel;
}

type KnownAction = IRequestSaveGuestDetailsAction | IReceiveSaveGuestDetailsAction |
	IRequestChangePasswordAction | IReceiveChangePasswordAction |
	IRequestDeleteAddressAction | IReceiveDeleteAddressAction |
	IRequestDeleteCardAction | IReceiveDeleteCardAction |
	IRequestSaveAddressAction | IReceiveSaveAddressAction |
	IRequestSaveCardAction | IReceiveSaveCardAction |
	IRequestSetDefaultAddressAction | IReceiveSetDefaultAddressAction |
	IRequestGetAddressListAction | IReceiveGetAddressListAction |
	IRequestGetCardListAction | IReceiveGetCardListAction |
	IRequestUserOrderData | IReceiveUserOrderData | IRequestGetCountryListAction | IReceiveGetCountryListAction
	| IReceiveGetGuestListAction | IRequestGetGuestListAction | IRequestDeleteGuestListAction
	| IReceiveDeleteGuestListAction | IRequestUpdateUserAction | IReceiveUpdateUserAction | IRequestUpdateGuestDetailsAction
	| IRecieveUpdateGuestDetailsAction | IRequestMobileDetailsAction | IRecieveMobileDetailsAction |
	IRequestNotificationUpdateAction | IReceiveNotificationUpdateAction;

export const actionCreators = {

	deleteAddressAction: (deleteAddressModel: DeleteAddressDataViewModel, callback: (DeleteAddressResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_ADDRESS_DELETE_REQUEST" });
		accountService.deleteAddress(deleteAddressModel)
			.then(
				(address: DeleteAddressResponseViewModel) => {
					let alertModel: AlertDataViewModel = {
						success: true,
						subject: "Delete Address",
						body: "Address deleted successfully",
					};
					dispatch({ type: "USER_ADDRESS_DELETE_SUCCESS", alertMessage: alertModel });
					callback(address);
				},
				(error) => {
				},
			);
	},

	deleteCardAction: (deleteCardModel: DeleteCardDataViewModel, callback: (DeleteCardResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_CARD_DELETE_REQUEST" });
		accountService.deleteCard(deleteCardModel)
			.then(
				(card: DeleteCardResponseViewModel) => {
					let alertModel: AlertDataViewModel = {
						success: true,
						subject: "Delete Card",
						body: "Card deleted successfully",
					};
					dispatch({ type: "USER_CARD_DELETE_SUCCESS", alertMessage: alertModel });
					callback(card);
				},
				(error) => {
				},
			);
	},

	saveAddressAction: (saveAddressModel: SaveAddressFormDataViewModel, callback: (SaveAddressResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_ADDRESS_SAVE_REQUEST" });
		accountService.saveAddress(saveAddressModel)
			.then(
				(address: SaveAddressResponseViewModel) => {
					let alertModel: AlertDataViewModel = {
						success: true,
						subject: "Save Address",
						body: "Address saved successfully",
					};
					dispatch({ type: "USER_ADDRESS_SAVE_SUCCESS", alertMessage: alertModel });
					callback(address);
				},
				(error) => {
				},
			);
	},

	saveCardAction: (saveCardModel: SaveCardFormDataViewModel, callback: (SaveCardResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_CARD_SAVE_REQUEST" });
		accountService.saveCard(saveCardModel)
			.then(
				(response: SaveCardResponseViewModel) => {
					let alertModel: AlertDataViewModel = {
						success: true,
						subject: "Save Card",
						body: "Card saved successfully",
					};
					dispatch({ type: "USER_CARD_SAVE_SUCCESS", alertMessage: alertModel });
					callback(response);
				},
				(error) => {
				},
			);
	},

	setDefaultAddressAction: (setDefaultAddressModel: SetDefaultAddressFormDataViewModel, callback: (SetDefaultAddressResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_DEFAULT_ADDRESS_SET_REQUEST" });
		accountService.setDefaultAddress(setDefaultAddressModel)
			.then(
				(address: SetDefaultAddressResponseViewModel) => {
					let alertModel: AlertDataViewModel = {
						success: true,
						subject: "Set Default Address",
						body: "Default address set successfully",
					};
					dispatch({ type: "USER_DEFAULT_ADDRESS_SET_SUCCESS", alertMessage: alertModel });
					callback(address);
				},
				(error) => {
				},
			);
	},

	getAddressListAction: (addressModel: GetAddressesDataViewModel, callback?: (AddressResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_ADDRESS_LIST_GET_REQUEST" });
		accountService.getUserAddressList(addressModel)
			.then(
				(address: AddressResponseViewModel) => {
					dispatch({ type: "USER_ADDRESS_LIST_GET_SUCCESS", userAddresses: address });
				},
				(error) => {
				},
			);
	},

	getCardListAction: (cardModel: GetCardListDataViewModel, callback: (CardResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_CARD_LIST_GET_REQUEST" });
		accountService.getUserCardList(cardModel)
			.then(
				(card: CardResponseViewModel) => {
					dispatch({ type: "USER_CARD_LIST_GET_SUCCESS", userCards: card });
				},
				(error) => {
				},
			);
	},

	saveUserGuestDetails: (guestDetails: SaveGuestFormDataModel, callback: (SaveGuestFormResponseModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_GUEST_DETAILS_SAVE_REQUEST" });
		accountService.saveGuestDetails(guestDetails)
			.then(
				(guestData: SaveGuestResponseViewModel) => {
					callback(guestData);
					dispatch({ type: "USER_GUEST_DETAILS_SAVE_SUCCESS", guestDetails: guestData.success });
				},
				(error) => {
				},
			);
	},

	getUserGuestList: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/get/guest-user-details`)
			.then((response) => response.json() as Promise<SaveGuestFormResponseModel>)
			.then((data) => {
				dispatch({ type: "USER_GUEST_LIST_GET_SUCCESS", guestList: data, });
			});
		addTask(fetchTask);
		dispatch({ type: "USER_GUEST_LIST_GET_REQUEST" });
	},

	deleteUserGuestList: (userAltId: string, callback: (SaveGuestResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/delete/guest-user-details/${userAltId}`)
			.then((response) => response.json() as Promise<SaveGuestResponseViewModel>)
			.then((data) => {
				dispatch({ type: "USER_GUEST_LIST_DELETE_SUCCESS", deletedUser: data.success, });
				callback(data);
			},
				(error) => {
				},
			);
		addTask(fetchTask);
		dispatch({ type: "USER_GUEST_LIST_DELETE_REQUEST" });
	},

	updateUserGuestDetails: (guestDetails: SaveGuestFormDataModel, callback: (SaveGuestFormResponseModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_GUEST_DETAILS_UPDATE_REQUEST" });
		accountService.updateGuestDetails(guestDetails)
			.then(
				(guestData: SaveGuestResponseViewModel) => {
					callback(guestData)
					dispatch({ type: "USER_GUEST_DETAILS_UPDATE_SUCCESS", guestDetails: guestData.success });
				},
				(error) => {
				},
			);
	},
	// use this action to get or update user details in personal info tab
	updateUserDetails: (updateUserModel: UserProfileResponseViewModel, callback: (UserProfileResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_DETAILS_UPDATE_REQUEST" });
		accountService.updateUserDetails(updateUserModel)
			.then(
				(user: UserProfileResponseViewModel) => {
					dispatch({ type: "USER_DETAILS_UPDATE_SUCCESS", userProfile: user });
					callback(user);
				},
				(error) => {
				},
			);
	},
	getUserOrdertData: (userAltId: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/userorders/${userAltId}`)
			.then((response) => response.json() as Promise<UserOrderRespnseViewModel>)
			.then((data) => {
				dispatch({ type: "USER_ORDER_LIST_GET_SUCCESS", userOrders: data, });
			});
		addTask(fetchTask);
		dispatch({ type: "USER_ORDER_LIST_GET_REQUEST" });
	},

	requestCountryData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
		const fetchTask = fetch(`api/country/all`)
			.then((response) => response.json() as Promise<CountryDataViewModel>)
			.then((data) => {
				dispatch({ type: "COUNTRY_LIST_GET_SUCCESS", countryList: data, });
			})
		addTask(fetchTask);
		dispatch({ type: "COUNTRY_LIST_GET_REQUEST" });
	},

	checkMobileExists: (userDetails: MobileExistModel, callback: (MobileExistModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_MOBILE_DETAILS_REQUEST" });
		accountService.checkMobileExists(userDetails)
			.then(
				(data: MobileExistModel) => {
					callback(data)
					dispatch({ type: "USER_MOBILE_DETAILS_SUCCESS", mobileDetails: data });
				},
				(error) => {
				},
			);
	},
	loginAndSecurityAction: (changePasswordModel: ChangePasswordFormDataViewModel, callback: (ChangePasswordResponseViewModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_PASSWORD_UPDATE_REQUEST" });
		accountService.changePassword(changePasswordModel)
			.then((user: ChangePasswordResponseViewModel) => {
				dispatch({ type: "USER_PASSWORD_UPDATE_SUCCESS", loginAndSecurityUser: user });
				callback(user);
			},
				(error) => {
				},
			);
	},
	notificationAction: (changePasswordModel: NotificationModel, callback: (NotificationModel) => void): IAppThunkAction<KnownAction> => async (dispatch, getState) => {
		dispatch({ type: "USER_NOTIFICATION_UPDATE_REQUEST" });
		accountService.updateNotification(changePasswordModel)
			.then((user: NotificationModel) => {
				dispatch({ type: "USER_NOTIFICATION_UPDATE_SUCCESS", user: user });
				callback(user);
			},
				(error) => {
				},
			);
	},
};

export const reducer: Reducer<IUserAccountState> = (state: IUserAccountState, incomingAction: Action) => {
	const action = incomingAction as KnownAction;
	switch (action.type) {
		case "USER_DETAILS_UPDATE_REQUEST":
			return {
				...state,
				updateUserDetailsSuccess: false,
				isLoadingUserProfile: true,
				userProfile: state.userProfile
			}
		case "USER_DETAILS_UPDATE_SUCCESS":
			return {
				...state,
				updateUserDetailsSuccess: true,
				userProfile: action.userProfile,
				isLoadingUserProfile: false
			}
		case "USER_ADDRESS_DELETE_REQUEST":
			return {
				...state,
				deleteAddressSuccess: false
			};
		case "USER_ADDRESS_DELETE_SUCCESS":
			return {
				...state,
				alertMessage: action.alertMessage,
				deleteAddressSuccess: true
			};
		case "USER_CARD_DELETE_REQUEST":
			return {
				...state,
				deleteCardSuccess: false
			};
		case "USER_CARD_DELETE_SUCCESS":
			return {
				...state,
				alertMessage: action.alertMessage,
				deleteCardSuccess: true
			};
		case "USER_ADDRESS_SAVE_REQUEST":
			return {
				...state,
				saveUserAddressSuccess: false
			};
		case "USER_ADDRESS_SAVE_SUCCESS":
			return {
				...state,
				alertMessage: action.alertMessage,
				saveUserAddressSuccess: true
			};
		case "USER_CARD_SAVE_REQUEST":
			return {
				...state,
				saveUserCardSuccess: false
			};
		case "USER_CARD_SAVE_SUCCESS":
			return {
				...state,
				alertMessage: action.alertMessage,
				saveUserCardSuccess: true
			};
		case "USER_DEFAULT_ADDRESS_SET_REQUEST":
			return {
				...state,
				setDefaultAddressSuccess: false
			};
		case "USER_DEFAULT_ADDRESS_SET_SUCCESS":
			return {
				...state,
				alertMessage: action.alertMessage,
				setDefaultAddressSuccess: true
			};
		case "USER_ADDRESS_LIST_GET_REQUEST":
			return {
				...state,
				isLoadingUserAddressList: true
			};
		case "USER_ADDRESS_LIST_GET_SUCCESS":
			return {
				...state,
				userAddresses: action.userAddresses,
				isLoadingUserAddressList: false
			};
		case "USER_CARD_LIST_GET_REQUEST":
			return {
				...state,
				isLoadingUserCardList: true
			};
		case "USER_CARD_LIST_GET_SUCCESS":
			return {
				...state,
				userCards: action.userCards,
				isLoadingUserCardList: false
			};
		case "USER_ORDER_LIST_GET_REQUEST":
			return {
				...state,
				isLoadingUserOrderList: true
			};
		case "USER_ORDER_LIST_GET_SUCCESS":
			return {
				...state,
				userOrders: action.userOrders,
				isLoadingUserOrderList: false
			};
		case "COUNTRY_LIST_GET_REQUEST":
			return {
				...state,
				isLoadingCountryList: true
			};
		case "COUNTRY_LIST_GET_SUCCESS":
			return {
				...state,
				countryList: action.countryList,
				isLoadingCountryList: false
			};

		case "USER_GUEST_DETAILS_SAVE_REQUEST":
			return {
				...state,
				saveGuestDetailSuccess: false
			};
		case "USER_GUEST_DETAILS_SAVE_SUCCESS":
			return {
				...state,
				saveGuestDetailSuccess: action.guestDetails
			};

		case "USER_GUEST_LIST_GET_REQUEST":
			return {
				...state,
				isLoadingUserGuestList: true
			};
		case "USER_GUEST_LIST_GET_SUCCESS":
			return {
				...state,
				guestList: action.guestList,
				isLoadingUserGuestList: false,
			};

		case "USER_GUEST_LIST_DELETE_REQUEST":
			return {
				...state,
				deleteGuestItemSuccess: false
			}
		case "USER_GUEST_LIST_DELETE_SUCCESS":
			return {
				...state,
				deleteGuestItemSuccess: action.deletedUser,
			};
		case "USER_GUEST_DETAILS_UPDATE_REQUEST":
			return {
				...state,
				updateGuestDetailSuccess: false
			};
		case "USER_GUEST_DETAILS_UPDATE_SUCCESS":
			return {
				...state,
				updateGuestDetailSuccess: action.guestDetails,
			};
		case "USER_MOBILE_DETAILS_REQUEST":
			return {
				...state,
				isLoading: true
			};
		case "USER_MOBILE_DETAILS_SUCCESS":
			return {
				...state,
				isLoading: false,
				mobileDetails: action.mobileDetails
			};
		case "USER_NOTIFICATION_UPDATE_REQUEST":
			return {
				...state,
				isLoading: true
			};
		case "USER_NOTIFICATION_UPDATE_SUCCESS":
			return {
				...state,
				isLoading: false,
				user: action.user
			};
		case "USER_PASSWORD_UPDATE_REQUEST":
			return {
				...state,
				updateUserPasswordSuccess: false,
				updateUserPasswordInvalid: false,
				isLoading: true
			};
		case "USER_PASSWORD_UPDATE_SUCCESS":
			return {
				...state,
				loginAndSecurityUser: action.loginAndSecurityUser,
				updateUserPasswordSuccess: true,
				updateUserPasswordInvalid: false,
				isLoading: false
			};
		default:
			return state || DefaultRegisterState;
	}
};
