// This file was generated from the Models.tst template
//
import User from "../../models/Common/UserViewModel"
export class GuideOrderDetailModel {
    transactionId: number;
    placeName: string;
    placeCity: string;
    placeState: string;
    PlaceCountry: string;
    visitDate: string;
    ticketCategory: string;
    phonceCode: string;
    phoneNumber: string;
    ticketCategoryId: number;
    eventTicketAttribueId: number;
    orderStatus: string
    customerFirstName: string;
    customerLastName: string
    orderApprovedDate: string;
    orderCompletedDate: string;
    orderApprovedBy: string;
    currency: string;
    ticketPrice: number;
    guideFirstName: string
    guideLastName: string;
}

export class GuideOrderDetailResponseModel {
    orderDetails: GuideOrderDetailModel[];
    approvedByUsers: User[];
    success: boolean;
}
