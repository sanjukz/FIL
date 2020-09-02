import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "./";
import { CurrencyTypesResponseViewModel } from '../models/Inventory/CurrencyTypesResponseViewModel';
import DocumentTypesResponseViewModel from '../models/Inventory/DocumentTypesResponseViewModel';
import DeliveryTypesResponseViewModel from '../models/Inventory/DeliveryTypesResponseViewModel';
import RefundPolicyResponseViewModel from '../models/Inventory/RefundPolicyResponseViewModel';
import LanguageViewModel from '../models/Redemption/Language';
import CustomerInformationResposeViewModel from '../models/CustomerInformation/CustomerInformationResposeViewModel';

export interface ICurrencyTypeProps {
  currencyType: ICurrencyTypeComponentState;
}

export interface ICurrencyTypeComponentState {
  isLoading: boolean;
  currencyTypeSuccess: boolean;
  documentTypeSuccess: boolean;
  deliveryTypeSuccess: boolean;
  refundPolicySuccess?: boolean;
  languageSuccess?: boolean;
  currencyTypes: CurrencyTypesResponseViewModel;
  documentTypes: DocumentTypesResponseViewModel;
  deliveryTypes: DeliveryTypesResponseViewModel;
  refundPolicies: RefundPolicyResponseViewModel;
  languages: LanguageViewModel;
  customerInformationControl: CustomerInformationResposeViewModel;
}

interface IRequestCurrencyTypes {
  type: "REQUEST_CURRENCY_TYPE_DATA";
}

interface IReceiveCurrencyTypes {
  type: "RECEIVE_CURRENCY_TYPE_DATA";
  currencyTypes: CurrencyTypesResponseViewModel;
}

interface IFailCurrencyTypes {
  type: "FAIL_CURRENCY_TYPE_DATA";
}

interface IRequestDocumentTypes {
  type: "REQUEST_DOCUMENT_TYPE_DATA";
}

interface IReceiveDocumentTypes {
  type: "RECEIVE_DOCUMENT_TYPE_DATA";
  documentTypes: DocumentTypesResponseViewModel;
}

interface IRequestDeliveryTypes {
  type: "REQUEST_DELIVERY_TYPE_DATA";
}

interface IReceiveDeliveryTypes {
  type: "RECEIVE_DELIVERY_TYPE_DATA";
  deliveryTypes: DeliveryTypesResponseViewModel;
}

interface IRequestRefundPolicies {
  type: "REQUEST_REFUND_POLICY_DATA";
}

interface IReceiveRefundPolicies {
  type: "RECEIVE_REFUND_POLICY_DATA";
  refundPolicies: RefundPolicyResponseViewModel;
}

interface ICustomerInformationRequest {
  type: "REQUEST_CUSTOMER_INFORMATION_CONTROL_DATA";
}

interface IReceiveCustomerInformation {
  type: "RECEIVE_RCUSTOMER_INFORMATION_CONTROL_DATA";
  customerInformationControl: CustomerInformationResposeViewModel;
}

interface ILanguageRequest {
  type: "REQUEST_LANGUAGE_DATA";
}

interface ILanguageReceive {
  type: "RECEIVE_LANGUAGE_DATA";
  languages: LanguageViewModel;
}

type KnownAction = IRequestCurrencyTypes | IReceiveCurrencyTypes | IRequestDocumentTypes
  | IReceiveDocumentTypes | IRequestDeliveryTypes | IReceiveDeliveryTypes
  | IRequestRefundPolicies | IReceiveRefundPolicies | ICustomerInformationRequest
  | IReceiveCustomerInformation | ILanguageRequest | ILanguageReceive;

export const actionCreators = {
  requestCurrencyTypeData: (callback?: (res: CurrencyTypesResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {

    const fetchTask = fetch(`api/get/Currencies`)
      .then((response) => response.json() as Promise<CurrencyTypesResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_CURRENCY_TYPE_DATA", currencyTypes: data, });
        if (callback) {
          callback(data);
        }
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_CURRENCY_TYPE_DATA" });
  },

  requestCustomerInformationControlData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {

    const fetchTask = fetch(`api/get/customerInformationControl`)
      .then((response) => response.json() as Promise<CustomerInformationResposeViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_RCUSTOMER_INFORMATION_CONTROL_DATA", customerInformationControl: data, });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_CUSTOMER_INFORMATION_CONTROL_DATA" });
  },

  requestDocumentTypeData: (callback?: (res: DocumentTypesResponseViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {

    const fetchTask = fetch(`api/get/DocumentTypes`)
      .then((response) => response.json() as Promise<DocumentTypesResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_DOCUMENT_TYPE_DATA", documentTypes: data, });
        callback(data);
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_DOCUMENT_TYPE_DATA" });
  },
  requestDeliveryTypeData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {

    const fetchTask = fetch(`api/get/DeliveryTypes`)
      .then((response) => response.json() as Promise<DeliveryTypesResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_DELIVERY_TYPE_DATA", deliveryTypes: data, });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_DELIVERY_TYPE_DATA" });
  },
  requestRefundPolicies: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {

    const fetchTask = fetch(`api/get/refundPolicy`)
      .then((response) => response.json() as Promise<RefundPolicyResponseViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_REFUND_POLICY_DATA", refundPolicies: data, });
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_REFUND_POLICY_DATA" });
  },
  requestLanguages: (callback?: (res: LanguageViewModel) => void): IAppThunkAction<KnownAction> => (dispatch, getState) => {
    const fetchTask = fetch(`api/Guide/Languages`)
      .then((response) => response.json() as Promise<LanguageViewModel>)
      .then((data) => {
        dispatch({ type: "RECEIVE_LANGUAGE_DATA", languages: data, });
        callback(data);
      });

    addTask(fetchTask);
    dispatch({ type: "REQUEST_LANGUAGE_DATA" });
  },
};

const unloadedCurrencyTypes: CurrencyTypesResponseViewModel = {
  currencyTypes: [],
};

const unloadedDocumentTypes: DocumentTypesResponseViewModel = {
  documentTypes: [],
};

const unloadedDeliveryTypes: DeliveryTypesResponseViewModel = {
  deliveryTypes: [],
};

const unloadedRefundPolicies: RefundPolicyResponseViewModel = {
  refundPolicies: [],
};

const unloadedCustomerInformationm: CustomerInformationResposeViewModel = {
  customerInformationControls: [],
};

const unloadedLanguage: LanguageViewModel = {
  languages: [],
};

const unloadedCurrencyTypeState: ICurrencyTypeComponentState = {
  isLoading: false,
  currencyTypeSuccess: false,
  documentTypeSuccess: false,
  deliveryTypeSuccess: false,
  refundPolicySuccess: false,
  languageSuccess: false,
  currencyTypes: unloadedCurrencyTypes,
  documentTypes: unloadedDocumentTypes,
  deliveryTypes: unloadedDeliveryTypes,
  refundPolicies: unloadedRefundPolicies,
  customerInformationControl: unloadedCustomerInformationm,
  languages: unloadedLanguage
};

export const reducer: Reducer<ICurrencyTypeComponentState> = (state: ICurrencyTypeComponentState, incomingAction: Action) => {
  const action = incomingAction as KnownAction;
  switch (action.type) {
    case "REQUEST_CURRENCY_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: false,
        currencyTypes: unloadedCurrencyTypes,
        documentTypeSuccess: false,
        refundPolicySuccess: state.refundPolicySuccess,
        documentTypes: state.documentTypes,
        deliveryTypeSuccess: state.deliveryTypeSuccess,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "RECEIVE_CURRENCY_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: true,
        currencyTypes: action.currencyTypes,
        documentTypeSuccess: false,
        documentTypes: state.documentTypes,
        refundPolicySuccess: state.refundPolicySuccess,
        deliveryTypeSuccess: state.deliveryTypeSuccess,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "REQUEST_DOCUMENT_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: false,
        currencyTypes: state.currencyTypes,
        refundPolicySuccess: state.refundPolicySuccess,
        documentTypes: unloadedDocumentTypes,
        deliveryTypeSuccess: state.deliveryTypeSuccess,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "RECEIVE_DOCUMENT_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: true,
        currencyTypes: state.currencyTypes,
        documentTypes: action.documentTypes,
        refundPolicySuccess: state.refundPolicySuccess,
        deliveryTypeSuccess: state.deliveryTypeSuccess,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "REQUEST_DELIVERY_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: state.deliveryTypeSuccess,
        currencyTypes: state.currencyTypes,
        documentTypes: state.documentTypes,
        deliveryTypeSuccess: false,
        deliveryTypes: unloadedDeliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "RECEIVE_DELIVERY_TYPE_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: state.deliveryTypeSuccess,
        currencyTypes: state.currencyTypes,
        documentTypes: state.documentTypes,
        deliveryTypeSuccess: true,
        deliveryTypes: action.deliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "REQUEST_REFUND_POLICY_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: state.deliveryTypeSuccess,
        currencyTypes: state.currencyTypes,
        documentTypes: state.documentTypes,
        deliveryTypeSuccess: false,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: unloadedRefundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "RECEIVE_REFUND_POLICY_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: state.deliveryTypeSuccess,
        currencyTypes: state.currencyTypes,
        documentTypes: state.documentTypes,
        deliveryTypeSuccess: true,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: action.refundPolicies,
        customerInformationControl: state.customerInformationControl
      };
    case "REQUEST_CUSTOMER_INFORMATION_CONTROL_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: state.deliveryTypeSuccess,
        currencyTypes: state.currencyTypes,
        documentTypes: state.documentTypes,
        deliveryTypeSuccess: false,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: unloadedCustomerInformationm,
      };
    case "RECEIVE_RCUSTOMER_INFORMATION_CONTROL_DATA":
      return {
        ...state,
        isLoading: true,
        currencyTypeSuccess: state.currencyTypeSuccess,
        documentTypeSuccess: state.deliveryTypeSuccess,
        currencyTypes: state.currencyTypes,
        documentTypes: state.documentTypes,
        deliveryTypeSuccess: true,
        deliveryTypes: state.deliveryTypes,
        refundPolicies: state.refundPolicies,
        customerInformationControl: action.customerInformationControl
      };
    case "REQUEST_LANGUAGE_DATA":
      return {
        ...state,
        languages: unloadedLanguage,
        languageSuccess: false
      };
    case "RECEIVE_LANGUAGE_DATA":
      return {
        ...state,
        languages: action.languages,
        languageSuccess: true
      };
    default:
      const exhaustiveCheck: never = action;
  }
  return state || unloadedCurrencyTypeState;
};
