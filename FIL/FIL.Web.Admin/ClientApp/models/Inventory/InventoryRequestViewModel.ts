// This file was generated from the Models.tst template
//
export class InventoryRequestViewModel {
    ticketCategoriesViewModels: TicketCategories[];
    termsAndCondition: string;
    deliverTypeId: number[];
    customerIdTypes: number[];
    eventDetailAltIds: string[];
    customerInformation: number[];
    isCustomerIdRequired: boolean;
    redemptionInstructions: string;
    redemptionAddress: string;
    redemptionCity: string;
    redemptionState: string;
    redemptionCountry: string;
    redemptionZipcode: string;
    redemptionDateTime: string;
    refundPolicy: number;
    placeAltId: string;
    isEdit: boolean;
    feeTypes: FeeTypes[];
}

export class TicketCategories {
    id?: number;
    ticketCategoryId?: number;
    isEventTicketAttributeUpdated?: boolean;
    eventTicketDetailId?: number;
    categoryName: string;
    quantity: number;
    ticketCategoryDescription: string;
    currencyId: number;
    pricePerTicket: number;
    isRollingTicketValidityType?: boolean;
    ticketValidityFixDate?: string;
    ticketCategoryNote?: string;
    days?: string;
    month: string;
    year?: string;
    ticketCategoryTypeId?: number;
    ticketSubCategoryTypeId?: number;
    currencyCountryId?: number;
}



export class FeeTypes {
    feeTypeId: number;
    valueTypeId: number;
    value: number;
    feeType: string;
    categoryName: string;
    eventTicketAttributeId: number;
}