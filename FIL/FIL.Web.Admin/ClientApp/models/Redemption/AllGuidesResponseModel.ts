// This file was generated from the Models.tst template
//
import User from "../../models/Common/UserViewModel"
export class GuideCustomeModel {
    id: number;
    userId: number;
    altId: string;
    firstName: string;
    lastName: string;
    email: string;
    phonceCode: string;
    phoneNumber: string;
    placeName: string;
    venueName: string;
    cityName: string;
    stateName: string;
    countryName: string;
    languageId: string;
    approveStatusId: string;
    approvedBy: string;
    approvedUtc: string;
    isEnabled: boolean;
    createdUtc: string;
    updatedUtc: string;
    createdBy: string;
    updatedBy: string;
    accountNumber: string;
    accountType: number;
    bankAccountType: number
    bankName: string;
    branchCode: string;
    currencyName: string;
    routingNumber: string;
    taxId: string;
}

export class AllGuidesResponseModel {
    guideDetails: GuideCustomeModel[];
    approvedByUsers: User[];
}
