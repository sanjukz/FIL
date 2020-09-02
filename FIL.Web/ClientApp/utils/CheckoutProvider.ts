import { LoginFormDataViewModel } from "shared/models/LoginFormDataViewModel";
import { EventTicketDetailDataViewModel, EventTicketAttribute } from "../models/Checkout/EventTicketDetailDataViewModel";
import { LoginTransactionFormDataViewModel } from "../models/Checkout/TransactionFormDataViewModel"
import { DeliveryOptionsFormDataViewModel, DeliveryDetail } from "../models/DeliveryOptions/DeliveryDetailFormDataViewModel";

export const getCheckoutModel = (values: LoginFormDataViewModel, userAltid: string) => {
    var data = JSON.parse(localStorage.getItem("cartItems"));
    var ticketData = [];
    var isItineray = false, isTiqets = false;
    var currency = "";
    let donationAmount = 0;
    let isBSPUpgrade = false;
    data.forEach(function (val) {
        val.guestList.age = +val.guestList.age;
        let eventDetail = GetEventTicketAttributeRequestModel(val);
        if (val.isItinerary != undefined && val.isItinerary) {
            isItineray = true;
            currency = val.currencyName
        }
        if (val.isTiqetsPlace != undefined && val.isTiqetsPlace) {
            isTiqets = true;
        }
        donationAmount = val.donationAmount ? val.donationAmount : 0;
        isBSPUpgrade = val.isBSPUpgrade ? true : false;
        ticketData.push(eventDetail);
    });
    var userCheckoutData: LoginTransactionFormDataViewModel = {
        userAltId: userAltid,
        userDetail: values,
        eventTicketAttributeList: ticketData,
        isItinerary: isItineray,
        transactionCurrency: currency,
        isTiqets: isTiqets,
        donationAmount: donationAmount,
        referralId: sessionStorage.getItem('referralId') != null ? sessionStorage.getItem('referralId') : null,
        isBSPUpgrade: isBSPUpgrade
    }
    return userCheckoutData;
};


export const GetEventTicketAttributeRequestModel = (val: any) => {
    let eventDetail: EventTicketAttribute = {
        id: val.eventTicketAttributeId,
        ticketType: 1,
        totalTickets: val.quantity,
        visitDate: new Date(val.selectedDate),
        guestDetails: val.guestList,
        eventVenueMappingTimeId: val.eventVenueMappingTimeId,
        isAdult: val.isAdult ? val.isAdult : false,
        price: val.pricePerTicket ? val.pricePerTicket : 0,
        visitEndTime: val.visitEndTime ? val.visitEndTime : "",
        visitStartTime: val.visitStartTime ? val.visitStartTime : "",
        purchaserAddress: {
            address1: val.pickupLocation && val.pickupLocation.address1 ? val.pickupLocation.address1 : "",
            address2: val.pickupLocation && val.pickupLocation.address2 ? val.pickupLocation.address2 : "",
            town: val.pickupLocation && val.pickupLocation.town ? val.pickupLocation.town : "",
            region: val.pickupLocation && val.pickupLocation.region ? val.pickupLocation.region : "",
            postalCode: val.pickupLocation && val.pickupLocation.postalCode ? val.pickupLocation.postalCode : ""
        },
        timeSlot: val.timeSlot,
        reserveHohoBook: (val.isHohoPlace && val.timeSlot != null && val.timeSlot != "") ? true : false,
        transactionType: val.transactionType ? val.transactionType : 1,
        discountedPrice: val.discountAmount ? val.discountAmount : 0,
        overridedAmount: val.overridedAmount ? val.overridedAmount : 0,
        scheduleDetailId: val.scheduleDetailId ? val.scheduleDetailId : 0,
        donationAmount: ((val.ticketCategoryId == 19452 || val.ticketCategoryId == 12259) && val.donationAmount && val.donationAmount > 0) ? val.donationAmount : 0
    };
    return eventDetail;
}


export const DeliveryOptionResponse = (userDetails: any, transactionId: any) => {
    if (localStorage.getItem('cartItems') != null && localStorage.getItem('cartItems') != '0') {
        var data = JSON.parse(localStorage.getItem("cartItems"));
        var detailData = [];
        var ticketData = [];
        data.forEach(function (val) {
            var userData = userDetails.filter(function (item) {
                return item.eventAltid == val.altId
            })
            let eventDetail: DeliveryDetail = {
                eventTicketAttributeId: val.eventTicketAttributeId,
                deliveryTypeId: userData.length > 0 ? userData[0].deliveryTypeId : 5,
                firstName: userData.length > 0 ? userData[0].firstName : '',
                lastName: userData.length > 0 ? userData[0].lastName : '',
                email: userData.length > 0 ? userData[0].email : '',
                phoneCode: userData.length > 0 ? userData[0].phoneCode : '',
                phoneNumber: userData.length > 0 ? userData[0].phoneNumber : '',
            };
            val.guestList.age = +val.guestList.age;
            let eventTicketDetail = GetEventTicketAttributeRequestModel(val);
            ticketData.push(eventTicketDetail);
            detailData.push(eventDetail);
        });
        var pickupDetail: DeliveryOptionsFormDataViewModel = {
            transactionId: transactionId,
            deliveryDetail: detailData,
            eventTicketAttributeList: ticketData
        }
        return pickupDetail;
    } else {
        return false
    }
}