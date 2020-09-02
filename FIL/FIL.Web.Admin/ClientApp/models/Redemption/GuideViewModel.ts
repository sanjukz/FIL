// This file was generated from the Models.tst template
//

import { Service } from "../../models/Redemption/Service";

export class CreateGuideInputModel {
    eventIDs: number[];
    firstName: string;
    lastName: string;
    email: string;
    addressLineOne?: string;
    addressLineTwo?: string;
    zip?: string;
    residentCountryAltId?: string;
    residentStateId?: number;
    residentCityId: number;
    languageId: number[];
    phoneCode: string;
    phoneNumber: string;
    sellerType?: number;
    currencyId: number;
    financeCountryAltId: string;
    financeStateId: number;
    bankAccountType: number;
    accountType: number;
    bankName: string;
    accountNumber: string;
    branchCode: string;
    taxId?: string;
    routingNumber: string;
    services?: Service[];
    serviceNotes: string;
    document: number[];
    isEditGuide: boolean;
}

export class GuideResponseModel {
    success: boolean;
    isSaving: boolean;
}
