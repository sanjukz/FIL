import CityViewModel from "../Common/CityViewModel";
import StateViewModel from "../Common/StateViewModel";
import CountryViewModel from "../Common/CountryViewModel";

export interface EditGuideResponseModel {
    user: User;
    userAddressDetail: UserAddressDetail;
    userAddressDetailMapping: UserAddressDetailMapping;
    guideDetail: GuideDetail;
    guidePlaceMappings: Guide[];
    guidePlaces: GuidePlaces[];
    guideServices: Guide[];
    services: Service[];
    guideFinanceMapping: Guide;
    masterFinanceDetails: MasterFinanceDetails;
    guideDocumentMappings: Guide[];
}

export interface GuideDetail {
    id: number;
    userId: number;
    userAddressDetailId: number;
    languageId: string;
    approveStatusId: number;
    isEnabled: boolean;
    createdUtc: Date;
    createdBy: string;
    modifiedBy: string;
}

export interface Guide {
    id: number;
    guideId: number;
    documentID?: number;
    documentSouceURL?: string;
    isEnabled: boolean;
    createdUtc: Date;
    createdBy: string;
    modifiedBy: string;
    masterFinanceDetailId?: number;
    eventId?: number;
    approveStatusId?: number;
    serviceId?: number;
    notes?: string;
}

export interface MasterFinanceDetails {
    id: number;
    currenyId: number;
    countryId: number;
    stateId: number;
    accountTypeId: number;
    bankAccountTypeId: number;
    bankName: string;
    accountNumber: string;
    branchCode: string;
    taxId: string;
    routingNumber: string;
    isEnabled: boolean;
    createdUtc: Date;
    createdBy: string;
    modifiedBy: string;
}

export interface Service {
    id: number;
    name: string;
    description: string;
    isEnabled: boolean;
    createdUtc: Date;
    createdBy: string;
    modifiedBy: string;
}

export interface User {
    id: number;
    altId: string;
    rolesId: number;
    userName: string;
    password: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    phoneConfirmed: boolean;
    signUpMethodId: number;
    channelId: number;
    isActivated: boolean;
    lockOutEnabled: boolean;
    accessFailedCount: number;
    loginCount: number;
    isEnabled: boolean;
    createdUtc: Date;
    createdBy: string;
    profilePic: boolean;
    modifiedBy: string;
}

export interface UserAddressDetail {
    id: number;
    userId: number;
    altId: string;
    firstName: string;
    lastName: string;
    phoneNumber: string;
    addressLine1: string;
    addressLine2: string;
    cityId: number;
    zipcode: number;
    addressTypeId: number;
    isDefault: boolean;
    isEnabled: boolean;
    createdUtc: Date;
    updatedUtc: Date;
    createdBy: string;
    updatedBy: string;
    modifiedBy: string;
}

export interface UserAddressDetailMapping {
    residentialCity?: CityViewModel;
    residentialState?: StateViewModel;
    residentialCountry?: CountryViewModel;
    financialCity?: CityViewModel;
    financialState?: StateViewModel;
    financialCountry?: CountryViewModel;
}

export interface GuidePlaces {
    name: string;
    id: number;
    altId: string;
    eventCategoryId: number;
    eventTypeId: number;
}
